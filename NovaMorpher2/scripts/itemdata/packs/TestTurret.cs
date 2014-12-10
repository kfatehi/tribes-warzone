LaserData CentralUnitPulse
{
   laserBitmapName   = "lightningNew.bmp";
   hitName           = "shotgunex.dts";

   damageConversion  = 0;
   baseDamageType    = $LaserDamageType;

   beamTime          = 0.05;

   lightRange        = 2.0;
   lightColor        = { 1.0, 0.25, 0.25 };

   detachFromShooter = false;
};

RocketData MultiCannonMBolt
{
   bulletShapeName  = "plasmabolt.dts";
   explosionTag     = plasmaExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.35;
   damageType       = $DebrisDamageType;

   explosionRadius  = 5;
   kickBackStrength = 0.0;
   muzzleVelocity   = 65.0;
   terminalVelocity = 80.0;
   acceleration     = 5.0;
   totalTime        = 10.0;
   liveTime         = 11.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "plasmatrail.dts";
   smokeDist   = 1.8;

   soundId = SoundJetLight;
};

TurretData SideGun
{
	maxDamage = 1.0;
	maxEnergy = 45;
	minGunEnergy = 5;
	maxGunEnergy = 10;
	reloadDelay = 1.0;
	fireSound = SoundMortarTurretFire;
	activationSound = SoundMortarTurretOn;
	deactivateSound = SoundMortarTurretOff;
	whirSound = SoundMortarTurretTurn;
	range = 1;
	gunRange = 110;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	visibleToSensor = false;
	debrisId = defaultDebrisMedium;
	className = "Turret";
	shapeFile = "mortar_turret";
	shieldShapeName = "shield_medium";
	speed = 2.0;
	speedModifier = 2.0;
	projectileType = MultiCannonMBolt;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
	explosionId = LargeShockwave;
	description = "Side Gun";
};

function SideGun::onAdd(%this)
{
	schedule("DeployableTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	if (GameBase::getMapName(%this) == "")
		GameBase::setMapName (%this, "Multi-Cannon Turret");
}

function SideGun::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function SideGun::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function SideGun::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
}

// Override base class just in case.
function SideGun::onPower(%this,%power,%generator) {}
function SideGun::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
	%this.shieldStrength = 0.2;
}	

function SideGun::verifyTarget(%this,%target)
{
	%centralUnit = %this.mainUnit;
	%realTarget = %centralUnit.Target;
	if(%target == %realTarget)
	{
		%tPos = GameBase::getPosition(%target);
		%check = GameBase::testPosition(%this, %tPos);
		if(!%check)
				return True;
	}
	return False;
}

$damageScale[SideGun, $DebrisDamageType] = -0.01;

TurretData CentralComp
{
	className = "Turret";
	shapeFile = "camera";
	validateShape = false;
	validateMaterials = true;
	projectileType = CentralUnitPulse;
	maxDamage = 2;
	maxEnergy = 60;
	minGunEnergy = 0;
	maxGunEnergy = 0;
	reloadDelay = 0.01;
	speed = 20.0;
	speedModifier = 1.5;
	range = 100;
	visibleToSensor = true;
	shadowDetailMask = 4;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	explosionId = LargeShockwave;
	damageSkinData = "objectDamageSkins";
	description = "Central Station";
};


function CentralComp::onAdd(%this)
{
	schedule("DeployableTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Test Turret");
	}
}

function CentralComp::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function CentralComp::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function CentralComp::onDestroyed(%this)
{
	%turretA = %this.turretA;
	%turretB = %this.turretB;

	schedule("GameBase::setDamageLevel('" @ %turretA @ "','999');",2, %turretA);
	schedule("GameBase::setDamageLevel('" @ %turretB @ "','999');",2, %turretB);

	Turret::onDestroyed(%this);
  	schedule("$TeamItemCount["@GameBase::getTeam(%this)@" @ 'MultiCannonTurretPack']--;", 3);
}

// Override base class just in case.
function CentralComp::onPower(%this,%power,%generator) {}
function CentralComp::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
	%this.shieldStrength = 0.005;
}	

function CentralComp::verifyTarget(%this,%target)
{
	if(!%this.Target)
	{
		%tPos = GameBase::getPosition(%target);
		%pos = GameBase::getPosition(%this);
		%p1 = Vector::getDistance(%tPos, %pos) <= 100;
		if(%p1)
		{
			%p2 = GameBase::testPosition(%this, %tPos);
			if(!%p2)
			{
				%this.Target = %target;
				%turretA = %this.turretA;
				%turretB = %this.turretB;

				Turret::setTarget(%turretA, %target);
				Turret::setTarget(%turretB, %target);

				schedule("%this.Target = '';",2,%this);
				return "True";
			}
		}
	}
	return False;
}

$damageScale[CentralComp, $EnergyDamageType] = -0.01;






//----------------------------------------------------------------------------
																			
ItemImageData MultiCannonTurretPackImage
{
	shapeFile = "remoteturret";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData MultiCannonTurretPack
{
	description = "Multi-Cannon Turret";
	shapeFile = "remoteturret";
	className = "Backpack";
	heading = "eTurrets";
	imageType = MultiCannonTurretPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 350;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function MultiCannonTurretPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function MultiCannonTurretPack::onDeploy(%player,%item,%pos)
{
	if (MultiCannonTurretPack::deployShape(%player,%item)) {
		Player::decItemCount(%player,%item);
	}
}

function MultiCannonTurretPack::deployShape(%player,%item)
{
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ MultiCannonTurretPack] < $TeamItemMax[MultiCannonTurretPack])
	{
		if (GameBase::getLOSInfo(%player,3))
		{
			%obj = getObjectType($los::object);
			if (%obj == "SimTerrain" || %obj == "InteriorShape")
			{
				if (Vector::dot($los::normal,"0 0 1") > 0.7)
				{
					%pos = $los::position;
					%rot = GameBase::getRotation(%player); 
					%mTurret = newObject("Multi-Cannon Turret","Turret",CentralComp,true);

					%newPos = Vector::add(%pos, "0 0 0.25");
					addToSet("MissionCleanup", %mTurret);
					GameBase::setTeam(%mTurret,GameBase::getTeam(%player));
					GameBase::setPosition(%mTurret,%newPos);
					GameBase::setRotation(%mTurret,%rot);
					Gamebase::setMapName(%mTurret,"Multi-Cannon");

					Client::setOwnedObject(%client, %mTurret); 
					Client::setOwnedObject(%client, %player);


					%newPos = Vector::add(%pos, Vector::rotVector("1.1 0 0",%rot));
					%turret = newObject("Multi-Cannon Turret","Turret",SideGun,true);
					%mTurret.turretA = %turret;
					%turret.mainUnit = %mTurret;

					addToSet("MissionCleanup", %turret);
					GameBase::setTeam(%turret,GameBase::getTeam(%player));
					GameBase::setPosition(%turret,%newPos);
					GameBase::setRotation(%turret,%rot);
					Gamebase::setMapName(%turret,"Multi-Cannon");

					Client::setOwnedObject(%client, %turret); 
					Client::setOwnedObject(%client, %player);


					%newPos = Vector::add(%pos, Vector::rotVector("-1.1 0 0",%rot));
					%turret = newObject("Test Turret","Turret",SideGun,true);
					%mTurret.turretB = %turret;
					%turret.mainUnit = %mTurret;
					%mTurret.Shoot = "";

					addToSet("MissionCleanup", %turret);
					GameBase::setTeam(%turret,GameBase::getTeam(%player));
					GameBase::setPosition(%turret,%newPos);
					GameBase::setRotation(%turret,%rot);
					Gamebase::setMapName(%turret,"Multi-Cannon");

					Client::setOwnedObject(%client, %turret); 
					Client::setOwnedObject(%client, %player);


					Client::sendMessage(%client,0,"MultiCann0n Turret deployed");
					playSound(SoundPickupBackpack,%pos);

				  	$TeamItemCount[GameBase::getTeam(%player) @ "MultiCannonTurretPack"]++;
					return true;
				}
				else 
					Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
			}
			else 
				Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
		}
		else 
			Client::sendMessage(%client,0,"Deploy position out of range");
	}
	else																						  
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");

	return false;
}

$packDiscription[MultiCannonTurretPack] = "This multi-cannoned turret is the first prototype, it features 2 fusion cannons!";

$TeamItemMax[MultiCannonTurretPack] = 2;

$InvList[MultiCannonTurretPack] = 1;
$RemoteInvList[MultiCannonTurretPack] = 0;

$ItemMax[marmor, MultiCannonTurretPack] = 1;
$ItemMax[mfemale, MultiCannonTurretPack] = 1;

$ItemMax[harmor, MultiCannonTurretPack] = 1;

Patch::AddReInit("MultiCannonTurretPack");