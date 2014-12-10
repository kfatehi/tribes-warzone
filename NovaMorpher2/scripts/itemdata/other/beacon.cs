$SpecialArmor::Beacon[0] = "sniperxarmor"; 
$SpecialArmor::BeaconInfo[0] = "Satchel";

$SpecialArmor::Beacon[1] = "sniperxfemale"; 
$SpecialArmor::BeaconInfo[1] = "Satchel";

$SpecialArmor::Beacon[2] = "harmor"; 
$SpecialArmor::BeaconInfo[2] = "Booster";

$SpecialArmor::Beacon[3] = "larmor"; 
$SpecialArmor::BeaconInfo[3] = "Emp";

$SpecialArmor::Beacon[4] = "lfemale"; 
$SpecialArmor::BeaconInfo[4] = "Emp";

ItemData Beacon
{
   description = "Beacon";
   shapeFile = "sensor_small";
   heading = "yMiscellany";
   shadowDetailMask = 4;
   price = 5;
	className = "HandAmmo";
   validateShape = false;
   validateMaterials = true;
};

function Beacon::onUse(%player,%item)
{
	if (Beacon::deployShape(%player,%item)) {
		Player::decItemCount(%player,%item);
	}
}

function Beacon::deployShape(%player,%item)
{
 	%client = Player::getClient(%player);
	%armor = Player::getArmor(%client);
	for(%i = 0; $SpecialArmor::Beacon[%i] != ""; %i++)
	{
		if($SpecialArmor::Beacon[%i] == %armor)
		{
			%specialty = $SpecialArmor::BeaconInfo[%i];
			%armorIsSpecial = True;
			break;
		}
	}

	if(!%armorIsSpecial)
	{
		if (GameBase::getLOSInfo(%player,3))
		{
			// GetLOSInfo sets the following globals:
			// 	los::position
			// 	los::normal
			// 	los::object
			%obj = getObjectType($los::object);
			if (%obj == "SimTerrain" || %obj == "InteriorShape")
			{
				// Try to stick it straight up or down, otherwise
				// just use the surface normal
				if (Vector::dot($los::normal,"0 0 1") > 0.6)
				{
					%rot = "0 0 0";
				}
				else
				{
					if (Vector::dot($los::normal,"0 0 -1") > 0.6)
					{
						%rot = "3.14159 0 0";
					}
					else
					{
						%rot = Vector::getRotation($los::normal);
					}
				}
			  	%set=newObject("set",SimSet);
				%num=containerBoxFillSet(%set,$StaticObjectType | $ItemObjectType | $SimPlayerObjectType,$los::position,0.3,0.3,0.3,1);
				deleteObject(%set);
				if(!%num)
				{
					%team = GameBase::getTeam(%player);
					if($TeamItemMax[%item] > $TeamItemCount[%team @ %item] || $TestCheats)
					{
						%beacon = newObject("Target Beacon", "StaticShape", "DefaultBeacon", true);
						addToSet("MissionCleanup", %beacon);
						//, CameraTurret, true);
						GameBase::setTeam(%beacon,GameBase::getTeam(%player));
						GameBase::setRotation(%beacon,%rot);
						GameBase::setPosition(%beacon,$los::position);
						Gamebase::setMapName(%beacon,"Target Beacon");
   				   		Beacon::onEnabled(%beacon);
						Client::sendMessage(%client,0,"Beacon deployed");
						//playSound(SoundPickupBackpack,$los::position);
						$TeamItemCount[GameBase::getTeam(%beacon) @ "Beacon"]++;
						return true;
					}
					else
						Client::sendMessage(%client,0,"Deployable Item limit reached");
				}
				else
					Client::sendMessage(%client,0,"Unable to deploy - Item in the way");
			}
			else
			{
				Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
			}
		}
		else
		{
			Client::sendMessage(%client,0,"Deploy position out of range");
		}
	}
	else if(%specialty == "Satchel")
	{
		if(DeployableSatchel::deployShape(%player,%item))
			Player::decItemCount(%player,%item);	
	}
	else if(%specialty == "Booster")
	{
		boosterCountDownStart(%player);
		Player::decItemCount(%player,%item);	
	}
	else if(%specialty == "Emp")
	{
		Beacon::useEmpFlash(%player);
		Player::decItemCount(%player,%item);
	}

	return false;
}

function boosterCountDownStart(%player)
{
 	%client = Player::getClient(%player);

	if(%player.hasBooster > 15)
	{
		client::SendMessage(%client, 1, "THE BOOSTERS HAVE OVERHEATED!!!");
		for(%i = 0;(!Player::isDead(%player) && %i < 10); %i++)
		{
			%obj = newObject("","Mine","InstantExplosive");
			GameBase::throw(%obj, %client, 0, false);
			GameBase::setPosition(%obj, gamebase::getposition(%player));
		}
	}
	else if(%player.hasBooster)
	{
		client::SendMessage(%client, 1, "Booster fuel level has been increased by 10 liters!");
		%player.hasBooster += 10;
	}
	else
	{
		client::SendMessage(%client, 1, "Boosters activated!");
		%player.hasBooster = 11;
		boosterCountDown(%player);
	}
}

function boosterCountDown(%player)
{
	%player.hasBooster--;
	if(%player.hasBooster < 1 || Player::isDead(%player))
	{
		client::SendMessage(Player::getClient(%player), 1, "Boosters depleated!");
		%player.hasBooster = false;
		return;
	}
	else
		schedule("boosterCountDown("@%player@");", 1.0);
}

function Beacon::onEnabled(%this)
{
   GameBase::setIsTarget(%this,true);
}

//================================================================================================== Satchel
TurretData DeployableSatchel
{
	className = "Turret";
	shapeFile = "camera";
	projectileType = "Undefined";
	maxDamage = 0.01;
	maxEnergy = 75;
	minGunEnergy = 10;
	maxGunEnergy = 60;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 10.0;
	speed = 4.0;
	speedModifier = 1.5;
	range = 0;
	visibleToSensor = true;
	shadowDetailMask = 4;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundFireLaser;
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = rocketExp;
	description = "Satchel Charge";
	damageSkinData = "objectDamageSkins";
};

function DeployableSatchel::onFire(%this)
{
	%player = %this.deployer;
	%this.deployer = "";

	GameBase::applyRadiusDamage($MineDamageType, getBoxCenter(%this), 20, 0.75, 305, %player);
	GameBase::setDamageLevel(%this,1000);
}

function DeployableSatchel::onAdd(%this)
{
	schedule("DeployableSatchel::deploy(" @ %this @ ");",1,%this);
}

function DeployableSatchel::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");	
}

function DeployableSatchel::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
	GameBase::setRechargeRate(%this,0);
	%this.shieldStrength = 0;
	if (GameBase::getMapName(%this) == "")
	{
		GameBase::setMapName (%this, "Satchel Charge");
	}	
}

function DeployableSatchel::onControl(%this)
{
	%player = %this.deployer;
	%this.deployer = "";

	GameBase::applyRadiusDamage($MineDamageType, getBoxCenter(%this), 20, 0.75, 305, %player);
}

function DeployableSatchel::onDestroyed(%this)
{
	%player = %this.deployer;
	%this.deployer = "";

	%this.shieldStrength = 0;
	GameBase::setRechargeRate(%this,0);
	Turret::onDeactivate(%this);
	CalcRadiusDamage(%this,$DebrisDamageType,30,0.2,25,20,20,1.5,0.5,200,100);
  	$TeamItemCount[GameBase::getTeam(%this) @ "SatchelPack"]--;

	Turret::onDestroyed(%this);
}

function DeployableSatchel::onPower(%this,%power,%generator) {}

function DeployableSatchel::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}	


$NeedPowerCheck[DeployableSatchel] = "false"; //== For future uses....
$CanControl[DeployableSatchel] = "true"; //== Can a player control this kind of turret?
$NoComStation[DeployableSatchel] = "true"; //== Can a player control this turret without going to a command center?
$StaticShape::NoEffect[DeployableSatchel] = "false"; //== No matter what the TD is, it will not effect this STATIC object?

function DeployableSatchel::deployShape(%player,%item)
{
	%this = deployable(%player,%item,"Turret","Satchel","False","False","False","False","False","3","True", "DeployableSatchel", "beacon");
	%this.deployer = %player;
}


$EmpDamageType = DamageTypes::AddReserve();
$deathMsg[$EmpDamageType , 0]      = "%2 was shocked to death!";
$deathMsg[$EmpDamageType , 1]      = "%1 calls in the disassemblers to disassemble %2";
$deathMsg[$EmpDamageType , 2]      = "%2 was electricuted...";
$deathMsg[$EmpDamageType , 3]      = "%2 took an EMP bath %1 has prepaired!";
$deathMsg[$EmpDamageType , 4]      = "%2 tried to repair the lethal sparkling wound!";

$SpecialDamageType[$EmpDamageType] = "EmpDamageType";

function EmpDamageType::DoSpecialDMG(%damagedPlayer, %shooterPlayer)
{
	%shooterClient = Player::getClient(%shooterPlayer); if($debug){echo("%shooterClient: "@%shooterClient);}
	%damagedClient = Player::getClient(%damagedPlayer); if($debug){echo("%damagedClient: "@%damagedClient);}
	%damagedPlTeam = Client::getTeam(%damagedClient);
	%shooterClTeam = Client::getTeam(%shooterClient);

	if(%damagedPlTeam != %shooterClTeam)
		EMP(%damagedPlayer, %damagedClient, %shooterClient, "You were hit with an EMP surge!",180);
}

function Beacon::useEmpFlash(%player)
{
	%Set = newObject("set",SimSet); 
	%Pos = GameBase::getPosition(%player); 
	%Mask = $SimPlayerObjectType|$StaticObjectType|$VehicleObjectType|$MineObjectType|$SimInteriorObjectType; //cloaks people, thiings, vehicles, mines, and the base itself
	containerBoxFillSet(%Set, %Mask, %Pos, 15, 15, 15, 15);
	%num = Group::objectCount(%Set);
	for(%i=0; %i < %num; %i++)
	{
		%obj = Group::getObject(%Set, %i);

		%shooterClient = Player::getClient(%player); if($debug){echo("%shooterClient: "@%shooterClient);}
		%damagedClient = Player::getClient(%obj); if($debug){echo("%damagedClient: "@%damagedClient);}

		if(GameBase::getTeam(%obj) != GameBase::getTeam(%player))
		{
			%objPos = GameBase::getPosition(%obj);
			%thisPos = GameBase::getPosition(%player);
			%distance = Vector::getDistance(%objPos, %thisPos);

			if(%distance <= 10)
			{
				EMP(%obj, %damagedClient, %shooterClient, "You were hit with an EMP surge!",180);
			}
		}
	}
	deleteObject(%set);
	%obj = newObject("","Mine","EmpFlash");
 	addToSet("MissionCleanup", %obj);
	GameBase::throw(%obj,%player,0,false);
	return;
}


ExplosionData slowShockExp
{
   shapeName = "shockwave_large.dts";
   soundId   = shockExplosion;

   faceCamera = false;
   randomSpin = false;
   hasLight   = true;
   lightRange = 10.0;

   timeZero = 0.200;
   timeOne  = 0.950;

   colors[0]  = { 1.0, 1.0, 1.0 };
   colors[1]  = { 1.0, 1.0, 1.0 };
   colors[2]  = { 0.0, 0.0, 0.0 };
   radFactors = { 1.0, 0.5, 0.0 };
};

MineData EmpFlash
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "mortar";
	shadowDetailMask = 4;
	explosionId = slowShockExp;
	explosionRadius = 15.0;
	damageValue = 0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 500;
	maxDamage = 0.01; //== Sensitivity guys!
};

function EmpFlash::onAdd(%this)
{
	%data = GameBase::getDataName(%this);
	schedule("Mine::Detonate(" @ %this @ ");",0.1,%this);
}