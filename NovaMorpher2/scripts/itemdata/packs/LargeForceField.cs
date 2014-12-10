//============================================================================ Small Force Field
// Deployable LargeForceField 

StaticShapeData DeployableLargeForceField
{
	shapeFile = "forcefield_4x17";
	debrisId = defaultDebrisSmall;
	maxDamage = 50.0;
	visibleToSensor = true;
	isTranslucent = true;
   	description = "Large Force Field";
};

function DeployableLargeForceField::onCollision(%this,%obj)
{
	%clientId = Player::getClient(%obj);
	%armor = Player::getArmor(%clientId);
	if(%this.isactive==True || getObjectType(%obj)!="Player" || Player::isDead(%obj))
	{
		return;
	}

	%genInRange = findAtvGen(GameBase::getTeam(%clientId), 50);
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

function DeployableLargeForceField::onDestroyed(%this)
{
   StaticShape::onDestroyed(%this);
   $TeamItemCount[GameBase::getTeam(%this) @ "LargeForceFieldPack"]--;
	
}


ItemImageData LargeForceFieldPackImage
{
	shapeFile = "AmmoPack";
	mountPoint = 2;
   	mountOffset = { 0, -0.03, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData LargeForceFieldPack
{
	description = "Large Force Field";
	shapeFile = "AmmoPack";
	className = "Backpack";
    	heading = "hPlateforms";
	imageType = LargeForceFieldPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 880;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function LargeForceFieldPack::onUse(%player,%item)
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

function LargeForceFieldPack::onDeploy(%player,%item,%pos)
{
	if (LargeForceFieldPack::deployShape(%player,%item)) 
	{
		Player::decItemCount(%player,%item);
	}
}

function LargeForceFieldPack::deployShape(%player,%item)
{
	deployable(%player,%item,"StaticShape","Large Force Field",True,False,False,False,False,4,True,"DeployableLargeForceField", "LargeForceFieldPack");
}

$packDiscription[LargeForceFieldPack] = "This deployable is great for defence. A lot weaker and stronger then blastwalls depending on what is used. It treats all damage types fairly unlike blastwalls.";

$TeamItemMax[LargeForceFieldPack] = 30;

$InvList[LargeForceFieldPack] = 1;
$RemoteInvList[LargeForceFieldPack] = 0;

$ItemMax[marmor, LargeForceFieldPack] = 1;
$ItemMax[mfemale, LargeForceFieldPack] = 1;

$ItemMax[harmor, LargeForceFieldPack] = 1;