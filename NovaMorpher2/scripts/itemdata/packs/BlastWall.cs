//============================================================================ BlastWall
StaticShapeData BlastWall //== Non-opening massive defence. MAX 4
{
	shapeFile = "newdoor5";
	maxDamage = 500.0;
	debrisId = defaultDebrisLarge;
	explosionId = debrisExpLarge;
	description = "BlastWall";
	damageSkinData = "objectDamageSkins";
};

function BlastWall::onCollision(%this,%obj)
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

function BlastWall::onDestroyed(%this)
{
	StaticShape::onDestroyed(%this);
	$TeamItemCount[GameBase::getTeam(%this) @ "BlastWallPack"]--;	
}

function BlastWall::onDamage(%this,%type,%value,%object)
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

ItemImageData BlastWallPackImage
{
	shapeFile = "AmmoPack";
	mountPoint = 2;
   	mountOffset = { 0, -0.03, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData BlastWallPack
{
	description = "BlastWall";
	shapeFile = "AmmoPack";
	className = "Backpack";
    	heading = "hPlatforms";
	imageType = BlastWallPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 880;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function BlastWallPack::onUse(%player,%item)
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

function BlastWallPack::onDeploy(%player,%item,%pos)
{
	if (BlastWallPack::deployShape(%player,%item)) 
	{
		Player::decItemCount(%player,%item);
	}
}

function BlastWallPack::deployShape(%player,%item)
{
	deployable(%player,%item,"StaticShape","Blast Wall",False,False,False,False,False,4,True,"BlastWall", "BlastWallPack");
}

$packDiscription[BlastWallPack] = "This deployable is great for defence. This defensive wall cannot be opened and would be damaged greater by weaker weapons rather then stronger ones.";

$TeamItemMax[BlastWallPack] = 30;

$InvList[BlastWallPack] = 1;
$RemoteInvList[BlastWallPack] = 0;

$ItemMax[marmor, BlastWallPack] = 1;
$ItemMax[mfemale, BlastWallPack] = 1;

