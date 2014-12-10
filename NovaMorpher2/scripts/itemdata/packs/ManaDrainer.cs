//========================================================================================== Arbitor Beacon
TurretData ManaDrainer
{
	className = "Turret";
	shapeFile = "plasammo";
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
	description = "Mana Drainer";
	damageSkinData = "objectDamageSkins";
};

function ManaDrainer::onAdd(%this)
{
	schedule("ManaDrainer::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	if (GameBase::getMapName(%this) == "") 
	{
		GameBase::setMapName (%this, "Mana Drainer");
	}
}

function ManaDrainer::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function ManaDrainer::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function ManaDrainer::onDisabled(%this)
{
	Turret::onDisabled(%this);
}
function ManaDrainer::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "ManaDrainerPack"]--;
}


function ManaDrainer::onPower(%this,%power,%generator) {}

function ManaDrainer::onEnabled(%this) 
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

		if(%objType == "player" && (Player::getArmor(%obj) == "magearmor" || Player::getArmor(%obj) == "magefemale") && GameBase::getTeam(%obj) != GameBase::getTeam(%this))
		{
			%objPos = GameBase::getPosition(%obj);
			%thisPos = GameBase::getPosition(%this);
			%distance = Vector::getDistance(%objPos, %thisPos);

			if(%distance < 151)
			{
				%manaCount = Player::getItemCount(%obj,Mana);
				if((%manaCount-(2*(150-%distance))) > 0)
					Player::decItemCount(%obj,Mana,(150-%distance));
				else if(%manaCount > 0)
					Player::setItemCount(%obj,Mana,0);
			}
		}
	}

	deleteObject(%set);

	schedule("ManaDrainer::checkManaDrainer(" @ %this @ ");", 0.1, %this);

}	

function ManaDrainer::checkManaDrainer(%this)
{
	if(GameBase::getDamageState(%this) != "Enabled")
		return;

	%this.evenodd = !%this.evenodd; //switches from 1 to 0... tells every other check... used to check if in both new & old sets

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
		%armor = Player::getArmor(%obj);

		if(%objType == "Player" && (%armor == "magearmor" || %armor == "magefemale") && GameBase::getTeam(%obj) != GameBase::getTeam(%this))
		{
			%objPos = GameBase::getPosition(%obj);
			%thisPos = GameBase::getPosition(%this);
			%distance = Vector::getDistance(%objPos, %thisPos);

			if(%distance < 151)
			{
				%manaCount = Player::getItemCount(%obj,Mana);
				if((%manaCount-((150-%distance)/8)) > 0)
					Player::decItemCount(%obj,Mana,(150-%distance)/8);
				else if(%manaCount > 0)
					Player::setItemCount(%obj,Mana,0);
			}
		}
	}
	deleteObject(%set);
	schedule("ManaDrainer::checkManaDrainer(" @ %this @ ");", 0.5, %this); //then recheck in 10 seconds
}

//===============================

ItemImageData ManaDrainerPackImage
{
	shapeFile = "ammopack";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData ManaDrainerPack
{
	description = "Mana Drainer Dev";
	shapeFile = "plasammo";
	className = "Backpack";
   	heading = "iArea Effect Items";
	imageType = ManaDrainerPackImage;
	shadowDetailMask = 4;
	mass = 2.5;
	elasticity = 0.2;
	price = 1350;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function ManaDrainerPack::onUse(%player,%item)
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

function ManaDrainerPack::onDeploy(%player,%item,%pos)
{
	if (ManaDrainerPack::deployShape(%player,%item))
	{
		Player::decItemCount(%player,%item);
	}
}

function ManaDrainerPack::deployShape(%player,%item)
{
	deployable(%player,%item,"Turret","Mana Drainer","False","False","False","False","True","10","True", "ManaDrainer", "ManaDrainerPack");
}


$packDiscription[ManaDrainerPack] = "This holy object is said to have the powers to drain mana from the outsiders.";

$TeamItemMax[ManaDrainerPack] = 3;

$InvList[ManaDrainerPack] = 1;
$RemoteInvList[ManaDrainerPack] = 1;

$ItemMax[magearmor, ManaDrainerPack] = 1;
$ItemMax[magefemale, ManaDrainerPack] = 1;

Patch::AddReInit("ManaDrainerPack");
