BulletData HellsFireBolt
{
   bulletShapeName    = "plasmabolt.dts";
   explosionTag       = plasmaExp;

   damageClass        = 1;
   damageValue        = 0.005;
   damageType         = $FireDamageType;
   explosionRadius    = 3.0;

   muzzleVelocity     = 200.0;
   totalTime          = 3.0;
   liveTime           = 2.0;
   lightRange         = 3.0;
   lightColor         = { 1, 1, 0 };
   inheritedVelocityScale = 1.0;
   isVisible          = True;

   soundId = SoundJetLight;
};

function fireHellBolt(%this, %obj)
{
	%pos = GameBase::getPosition(%obj);
	%x = getWord(%pos, 0);
	%y = getWord(%pos, 1);
	%z = GetLowestZ(%pos);
	%pos = %x $+ " " $+ %y $+ " " $+ (%z+0.01);

	%x = 0;
	%y = 0;
	%z = math::degree2radian(90);
	%rot = %x $+ " " $+ %y $+ " " $+ %z;
	%trans = "0 0 -1 " @ %rot @ " 0 0 -1 " @ %pos;
	Projectile::spawnProjectile("HellsFireBolt", %trans, %this, "0 0 200"); //transform, object, velocity vector, <projectile target (seeker)>
}
//========================================================================================== Wrath of Satan
TurretData Hell
{
	className = "Turret";
	shapeFile = "plant2";
	maxDamage = 2;
	maxEnergy = 0;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	visibleToSensor = true;
	shadowDetailMask = 4;
	supressable = true;
	pinger = false;
	dopplerVelocity = 0;
	castLOS = true;
	supression = true;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "Repair plateau";
	damageSkinData = "objectDamageSkins";
};

function Hell::onAdd(%this)
{
	schedule("RepairBeacon::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	if (GameBase::getMapName(%this) == "") 
	{
		GameBase::setMapName (%this, "Warth of Satan);
	}
}

function Hell::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function Hell::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function Hell::onDisabled(%this)
{
	Turret::onDisabled(%this);
}
function Hell::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "HellPack"]--;
}


function Hell::onPower(%this,%power,%generator) {}

function Hell::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);

	%Set = newObject("set",SimSet); 
	%Pos = GameBase::getPosition(%this); 
	%Mask = $SimPlayerObjectType|$StaticObjectType|$VehicleObjectType|$MineObjectType|$SimInteriorObjectType; //cloaks people, thiings, vehicles, mines, and the base itself
	containerBoxFillSet(%Set, %Mask, %Pos, 250, 250, 250, 250);
	%num = Group::objectCount(%Set);
	for(%i; %i < %num; %i++)
	{
		%obj = Group::getObject(%Set, %i);

		if(GameBase::getTeam(%obj) == GameBase::getTeam(%this))
		{
			%objType = getObjectType(%obj);
			%clientId = Player::getClient(%player);

			%objPos = GameBase::getPosition(%obj);
			%thisPos = GameBase::getPosition(%this);
			%distance = Vector::getDistance(%objPos, %thisPos);

			if(%distance < 151)
			{
				%damage = GameBase::getDamageLevel(%obj);
				%hrate = %damage - 0.5; //== A WARMING welcome :)
				GameBase::setDamageLevel(%obj, %hrate); 
			}
		}
	}

	deleteObject(%set);

	schedule("Hell::checkRepairBeacon(" @ %this @ ");", 0.5, %this);

}	

function Hell::checkRepairBeacon(%this)
{
	if(GameBase::getDamageState(%this) != "Enabled")
		return;

	%this.evenodd = !%this.evenodd; //switches from 1 to 0... tells every other check... used to check if in both new & old sets

	%Set = newObject("set",SimSet); 
	%Pos = GameBase::getPosition(%this); 
	%Mask = $SimPlayerObjectType|$StaticObjectType|$VehicleObjectType|$MineObjectType|$SimInteriorObjectType; //cloaks people, thiings, vehicles, mines, and the base itself
	containerBoxFillSet(%Set, %Mask, %Pos, 100, 100, 100, 100);
	%num = Group::objectCount(%Set);
	for(%i; %i < %num; %i++)
	{
		%obj = Group::getObject(%Set, %i);
		%objType = getObjectType(%obj);
		%clientId = Player::getClient(%player);
		%armor = Player::getArmor(%obj);

		if(GameBase::getTeam(%obj) == GameBase::getTeam(%this))
		{
			%objPos = GameBase::getPosition(%obj);
			%thisPos = GameBase::getPosition(%this);
			%distance = Vector::getDistance(%objPos, %thisPos);

			if(%distance <= 75)
			{
				%damage = GameBase::getDamageLevel(%obj);
				%hrate = %damage - (1/240);
				GameBase::setDamageLevel(%obj, %hrate); 
			}
		}
	}
	deleteObject(%set);
	schedule("Hell::checkRepairBeacon(" @ %this @ ");", 0.125, %this);
}

//===============================

ItemImageData HellPackImage
{
	shapeFile = "ammopack";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData HellPack
{
	description = "Repair Plateau";
	shapeFile = "plasammo";
	className = "Backpack";
   	heading = "iArea Effect Items";
	imageType = HellPackImage;
	shadowDetailMask = 4;
	mass = 2.5;
	elasticity = 0.2;
	price = 1350;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function HellPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item)
	{
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else
	{
		Player::deployItem(%player,%item);
	}
}

function HellPack::onDeploy(%player,%item,%pos)
{
	if (HellPack::deployShape(%player,%item))
	{
		Player::decItemCount(%player,%item);
	}
}

function HellPack::deployShape(%player,%item)
{
	deployable(%player,%item,"Turret","Repair Plateau","False","False","False","False","True","10","True", "RepairBeacon", "HellPack");
}


$packDiscription[HellPack] = "This magical element is said to be to homes of biological duplicates of nanoprobes of the repair gun! Use it to your advantage!";

$TeamItemMax[HellPack] = 1;

$InvList[HellPack] = 0;
$RemoteInvList[HellPack] = 0;

$ItemMax[magearmor, HellPack] = 1;
$ItemMax[magefemale, HellPack] = 1;

Patch::AddReInit("HellPack");
