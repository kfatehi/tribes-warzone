//============================================================================ Small Force Field
// Deployable Forcefield 

StaticShapeData DeployableForceField
{
	shapeFile = "forcefield_3x4";
	debrisId = defaultDebrisSmall;
	maxDamage = 10.0;
	visibleToSensor = true;
	isTranslucent = true;
   	description = "Small Force Field";
};

function DeployableForceField::onCollision(%this,%obj)
{
	%clientId = Player::getClient(%obj);
	%armor = Player::getArmor(%clientId);
	if(%this.isactive==True || getObjectType(%obj)!="Player" || Player::isDead(%obj))
	{
		return;
	}

	%genInRange = findAtvGen(GameBase::getTeam(%clientId), 125);
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

function DeployableForceField::onDestroyed(%this)
{
   StaticShape::onDestroyed(%this);
   $TeamItemCount[GameBase::getTeam(%this) @ "ForceFieldPack"]--;
	
}


ItemImageData ForceFieldPackImage
{
	shapeFile = "AmmoPack";
	mountPoint = 2;
   	mountOffset = { 0, -0.03, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData ForceFieldPack
{
	description = "Small Force Field";
	shapeFile = "AmmoPack";
	className = "Backpack";
    	heading = "hPlateforms";
	imageType = ForceFieldPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 600;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function ForceFieldPack::onUse(%player,%item)
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

function ForceFieldPack::onDeploy(%player,%item,%pos)
{
	if (ForceFieldPack::deployShape(%player,%item)) 
	{
		Player::decItemCount(%player,%item);
	}
}

function ForceFieldPack::deployShape(%player,%item)
{
	deployable(%player,%item,"StaticShape","Small Force Field",True,False,False,False,False,4,True,"DeployableForceField", "ForceFieldPack");
}

$packDiscription[ForceFieldPack] = "This deployable is great for defence. A lot weaker and stronger then blastwalls depending on what is used. It treats all damage types fairly unlike blastwalls.";

$TeamItemMax[ForceFieldPack] = 90;

$InvList[ForceFieldPack] = 1;
$RemoteInvList[ForceFieldPack] = 1;

$ItemMax[marmor, ForceFieldPack] = 1;
$ItemMax[mfemale, ForceFieldPack] = 1;

$ItemMax[harmor, ForceFieldPack] = 1;