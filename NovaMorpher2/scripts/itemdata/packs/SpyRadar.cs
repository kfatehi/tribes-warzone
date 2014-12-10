StaticShapeData SpyRadar
{
	shapeFile = "sat_big";
	debrisId = defaultDebrisSmall;
	maxDamage = 2.0;
	isTranslucent = true;
   	description = "SpyRadar";

	mapFilter = 2;
	mapIcon = "M_turret";
	visibleToSensor = true;
};

function SpyRadar::onDestroyed(%this)
{
	StaticShape::onDestroyed(%this);
	%team = GameBase::getTeam(%this);
	$Radar[%team] = "";
  	$TeamItemCount[%team @ "SpyRadarPack"]--;
}


ItemImageData SpyRadarPackImage
{
	shapeFile = "sat_big";
	mountPoint = 2;
    	mountOffset = { 0, -0.1, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData SpyRadarPack
{
	description = "Multi-Radar";
	shapeFile = "sat_big";
	className = "Backpack";
    	heading = "dDefence";
	imageType = SpyRadarPackImage;
	shadowDetailMask = 4;
	mass = 3;
	elasticity = 1.01;
	price = 995;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function SpyRadarPack::onUse(%player,%item)
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

function SpyRadarPack::onDeploy(%player,%item,%pos)
{
	if(%player.outArea == "")
	{
		if (SpyRadarPack::deployShape(%player,%item))
		{
			messageall(1,"A \"MULTI-RADAR\" has been set-up " @ getTeamName(Client::getTeam(GameBase::getOwnerClient(%player))) @ "!!!!");
			Player::decItemCount(%player,%item);
		}
	}
	else
	{
		messageallExcept(GameBase::getOwnerClient(%player), 1, getTeamName(Client::getTeam(GameBase::getOwnerClient(%player))) @ " has tried to deploy a radar out of mission area!");
		Client::sendMessage(GameBase::getOwnerClient(%player), 1, "You CANNOT deploy this item out of Mission AREA!");
	}
}

function SpyRadarPack::deployShape(%player,%item)
{
	$Radar[Client::getTeam(GameBase::getOwnerClient(%player))] = deployable(%player,%item,"StaticShape","Spy Radar",True,False,False,False,"T",4,True,"SpyRadar","SpyRadarPack");
}

$packDiscription[SpyRadarPack] = "This is a Multi-Operating radar. It enables spying enemy transmission up to 300m! It also has various other functions....";

$TeamItemMax[SpyRadarPack] = 1;

$InvList[SpyRadarPack] = 1;
$RemoteInvList[SpyRadarPack] = 0;

$ItemMax[harmor, SpyRadarPack] = 1;
