//============================================================================ Small Force Field
// Deployable MediumForceField 

StaticShapeData DeployableMediumForceField
{
	shapeFile = "forcefield";
	debrisId = defaultDebrisSmall;
	maxDamage = 25.0;
	visibleToSensor = true;
	isTranslucent = true;
   	description = "Medium Force Field";
};

function DeployableMediumForceField::onCollision(%this,%obj)
{
	%clientId = Player::getClient(%obj);
	%armor = Player::getArmor(%clientId);
	if(%this.isactive==True || getObjectType(%obj)!="Player" || Player::isDead(%obj))
	{
		return;
	}

	%genInRange = findAtvGen(GameBase::getTeam(%clientId), 75);
	if (GameBase::getTeam(%clientId) == Gamebase::getTeam(%this))
	{	
		%playerTeam = GameBase::getTeam(%obj);
		%fieldTeam = GameBase::getTeam(%this);
		OpenClose(%this);

		if(%genInRange)
			EMP(%this, Player::getClient(%this), %clientId, "You have been shocked with lethal EMP charges!");
		return;
	}
	return;
}

function DeployableMediumForceField::onDestroyed(%this)
{
   StaticShape::onDestroyed(%this);
   $TeamItemCount[GameBase::getTeam(%this) @ "MediumForceFieldPack"]--;
	
}


ItemImageData MediumForceFieldPackImage
{
	shapeFile = "AmmoPack";
	mountPoint = 2;
   	mountOffset = { 0, -0.03, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData MediumForceFieldPack
{
	description = "Medium Force Field";
	shapeFile = "AmmoPack";
	className = "Backpack";
    	heading = "hPlateforms";
	imageType = MediumForceFieldPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 780;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function MediumForceFieldPack::onUse(%player,%item)
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

function MediumForceFieldPack::onDeploy(%player,%item,%pos)
{
	if (MediumForceFieldPack::deployShape(%player,%item)) 
	{
		Player::decItemCount(%player,%item);
	}
}

function MediumForceFieldPack::deployShape(%player,%item)
{
	deployable(%player,%item,"StaticShape","Medium Force Field",True,False,False,False,False,4,True,"DeployableMediumForceField", "MediumForceFieldPack");
}

$packDiscription[MediumForceFieldPack] = "This deployable is great for defence. A lot weaker and stronger then blastwalls depending on what is used. It treats all damage types fairly unlike blastwalls.";

$TeamItemMax[MediumForceFieldPack] = 60;

$InvList[MediumForceFieldPack] = 1;
$RemoteInvList[MediumForceFieldPack] = 0;

$ItemMax[marmor, MediumForceFieldPack] = 1;
$ItemMax[mfemale, MediumForceFieldPack] = 1;

$ItemMax[harmor, MediumForceFieldPack] = 1;