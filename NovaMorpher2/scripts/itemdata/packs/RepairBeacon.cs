//========================================================================================== Repair Plateau
TurretData RepairBeacon
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

function RepairBeacon::onAdd(%this)
{
	schedule("RepairBeacon::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	if (GameBase::getMapName(%this) == "") 
	{
		GameBase::setMapName (%this, "Repair Plateau");
	}
}

function RepairBeacon::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function RepairBeacon::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function RepairBeacon::onDisabled(%this)
{
	Turret::onDisabled(%this);
}
function RepairBeacon::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "RepairBeaconPack"]--;
}


function RepairBeacon::onPower(%this,%power,%generator) {}

function RepairBeacon::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);

	%Set = newObject("set",SimSet); 
	%Pos = GameBase::getPosition(%this); 
	%Mask = $SimPlayerObjectType|$StaticObjectType|$VehicleObjectType|$MineObjectType|$SimInteriorObjectType; //cloaks people, thiings, vehicles, mines, and the base itself
	containerBoxFillSet(%Set, %Mask, %Pos, 160, 160, 160, 160);
	%num = Group::objectCount(%Set);
	for(%i; %i < %num; %i++)
	{
		%obj = Group::getObject(%Set, %i);
		%objType = getObjectType(%obj);
		%clientId = Player::getClient(%player);

		if(GameBase::getTeam(%obj) == GameBase::getTeam(%this))
		{
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

	schedule("RepairBeacon::checkRepairBeacon(" @ %this @ ");", 0.5, %this);

}	

function RepairBeacon::checkRepairBeacon(%this)
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
	schedule("RepairBeacon::checkRepairBeacon(" @ %this @ ");", 0.125, %this);
}

//===============================

ItemImageData RepairBeaconPackImage
{
	shapeFile = "ammopack";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData RepairBeaconPack
{
	description = "Repair Plateau";
	shapeFile = "plasammo";
	className = "Backpack";
   	heading = "iArea Effect Items";
	imageType = RepairBeaconPackImage;
	shadowDetailMask = 4;
	mass = 2.5;
	elasticity = 0.2;
	price = 1350;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function RepairBeaconPack::onUse(%player,%item)
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

function RepairBeaconPack::onDeploy(%player,%item,%pos)
{
	if (RepairBeaconPack::deployShape(%player,%item))
	{
		Player::decItemCount(%player,%item);
	}
}

function RepairBeaconPack::deployShape(%player,%item)
{
	deployable(%player,%item,"Turret","Repair Plateau","False","False","False","False","True","10","True", "RepairBeacon", "RepairBeaconPack");
}


$packDiscription[RepairBeaconPack] = "This magical element is said to be to homes of biological duplicates of nanoprobes of the repair gun! Use it to your advantage!";

$TeamItemMax[RepairBeaconPack] = 1;

$InvList[RepairBeaconPack] = 1;
$RemoteInvList[RepairBeaconPack] = 1;

$ItemMax[magearmor, RepairBeaconPack] = 1;
$ItemMax[magefemale, RepairBeaconPack] = 1;

Patch::AddReInit("RepairBeaconPack");
