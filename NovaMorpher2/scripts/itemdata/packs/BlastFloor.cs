//============================================================================ BlastFloor
StaticShapeData BlastFloor
{
	shapeFile = "elevator_8x8";
	maxDamage = 200.0;
	debrisId = defaultDebrisLarge;
	explosionId = debrisExpLarge;
	description = "BlastFloor";
	damageSkinData = "objectDamageSkins";
};

function BlastFloor::onCollision(%this,%obj)
{
 	%data = GameBase::getDataName(%obj);
	if(%data == Vehicle)
	{
		//== Die @!#$ idiots!! DIE!!!!!!!!!!!!
 		GameBase::setDamageLevel(%obj,999999);
		return;
	}

	%clientId = Player::getClient(%obj);
	%armor = Player::getArmor(%clientId);
	return;
}

function BlastFloor::onDestroyed(%this)
{
	StaticShape::onDestroyed(%this);
	$TeamItemCount[GameBase::getTeam(%this) @ "BlastFloorPack"]--;	
}

function BlastFloor::onDamage(%this,%type,%value,%object)
{
	if(%type == $NullDamageType)
		return;

	%value = (%value * -1) + 2;	//== Da stronger, da weeker :-P, if any weapon goes past 2, it will result in HEALING the object :'P

	%damageLevel = GameBase::getDamageLevel(%this);
	%dValue = %damageLevel + %value;

	if($debug)
		echo("%dValue = " @ %dValue);

	%this.lastDamageObject = %object;
	%this.lastDamageTeam = GameBase::getTeam(%object);
	if(GameBase::getTeam(%this) == GameBase::getTeam(%object))
	{
		%value = %value / 2;
	}
	GameBase::setDamageLevel(%this,%dValue);
}


//============ Actual Pack Function ============//

ItemImageData BlastFloorPackImage
{
	shapeFile = "AmmoPack";
	mountPoint = 2;
   	mountOffset = { 0, -0.03, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData BlastFloorPack
{
	description = "BlastFloor";
	shapeFile = "AmmoPack";
	className = "Backpack";
    	heading = "hPlatforms";
	imageType = BlastFloorPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 800;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function BlastFloorPack::onUse(%player,%item)
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

function BlastFloorPack::onDeploy(%player,%item,%pos)
{
	if (BlastFloorPack::deployShape(%player,%item)) 
	{
		Player::decItemCount(%player,%item);
	}
}

function BlastFloorPack::deployShape(%player,%item)
{
	deployable(%player,%item,"StaticShape","Blast Floor",True,False,False,False,False,4,True,"BlastFloor", "BlastFloorPack");
}

$packDiscription[BlastFloorPack] = "This is the same thing as Blast Wall BUT its a floor! The damaging rules stay the same";

$TeamItemMax[BlastFloorPack] = 30;

$InvList[BlastFloorPack] = 1;
$RemoteInvList[BlastFloorPack] = 0;

$ItemMax[marmor, BlastFloorPack] = 1;
$ItemMax[mfemale, BlastFloorPack] = 1;

