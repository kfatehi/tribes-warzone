$packDiscription[BoxBase] = "This enhancer has the ability to speed you up when to pass through it!";

$TeamItemMax[BoxBase] = 5;

$InvList[BoxBase] = 1;
$RemoteInvList[BoxBase] = 1;

$ItemMax[marmor, BoxBase] = 1;
$ItemMax[mfemale, BoxBase] = 1;

Patch::AddReInit("BoxBase");
//==================================================================================================
/// Created by TriCon Team C3 & graphfx
/// http://www.planetstarsiege.com/tricon
/// Rewritten By Emo1313 - Per placement

ItemImageData BoxBasePackImage
{
        shapeFile = "ammopack";
        mountPoint = 2;
        mountOffset = { 0, 0, 0 };
        mountRotation = { 0, 0, 0 };
        firstPerson = false;
};

ItemData BoxBase
{
	description = "BoxBase";
	shapeFile = "shieldpack";
	className = "Backpack";
	heading =  "jDeployable Base";
	imageType = BoxBasePackImage;
	shadowDetailMask = 4;
	mass = 5.0;
	elasticity = 0.2;
	price = 2000;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function BoxBase::onUse(%player,%item)
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

function BoxBase::onDeploy(%player,%item,%pos)
{
        if (BoxBase::deployShape(%player,%item))
        {
                Player::decItemCount(%player,%item);
        }
}

function CreateBoxBaseSet(%client, %number)
{
    	%teleset = nameToID("MissionCleanup/BoxBase" @ %client @ %number);
	if(%teleset == -1)
	{
		newObject("BoxBase" @ %client @ %number,SimSet);
		addToSet("MissionCleanup","BoxBase" @ %client @ %number);
		gamebase::setteam(%teleset, gamebase::getteam(%client));
	}
}

//== Heavily based on Shifter's airbase which originated from the tricon team, credits to them
function BoxBase::deployshape(%player,%item)
{
        GameBase::getLOSInfo(%player,3);
        
        %client = Player::getClient(%player);
	%playerPos = GameBase::getPosition(%player);
	%deploypos = Vector::add(GameBase::getPosition(%player), "-0 -0 -0");

	%number = $TeamItemCount[GameBase::getTeam(%player) @ "BoxBase"];

	if (!CheckForObjects(%deploypos,20,20,20))
	{
		Client::sendMessage(%client,1,"Objects In The Way, Can not deploy.");
		return false;
	}
	
	CreateBoxBaseSet(%client, %number);

	%rot = GameBase::getRotation(%player);
	//================== BoxBase platforms
	%name1 = "Sensor" @ Client::getName(%client) @ "" @ %number;
	%name2 = "StationGenerator" @ Client::getName(%client) @ "" @ %number;
	%name4 = "InventoryStation" @ Client::getName(%client) @ "" @ %number;
	
	
	%plat1 = "Bottem";
	%plat2 = "Top";
	%plat3 = "SideA";
	%plat4 = "SideB";
	%plat5 = "Back";
	%plat6 = "FF";

	instant StaticShape %plat1
	{
		dataBlock = "LargeBoxBasePlatform";
		name = %plat1;
		position = Vector::add(GameBase::getPosition(%player), Vector::rotVector("1 0 0",%rot));
		rotation = Vector::add(%rot, "0 0 0");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);
	};

	instant StaticShape %plat2
	{
		dataBlock = "LargeBoxBasePlatform";
		name = %plat2;
		position = Vector::add(GameBase::getPosition(%player), Vector::rotVector("1 -0 5.5",%rot));
		rotation = Vector::add(%rot, "0 0 0");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);
	};

	instant StaticShape %plat3
	{
		dataBlock = "LargeBoxBasePlatform";
		name = %plat3;
		position = Vector::add(GameBase::getPosition(%player), Vector::rotVector("-4 0 0",%rot));
		rotation = Vector::add(%rot, "0 1.55 0");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);
	};

	instant StaticShape %plat4
	{
		dataBlock = "LargeBoxBasePlatform";
		name = %plat4;
		position = Vector::add(GameBase::getPosition(%player), Vector::rotVector("6 0 0",%rot));
		rotation = Vector::add(%rot, "0 1.55 0");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);
	};

	instant StaticShape %plat5
	{
		dataBlock = "LargeBoxBasePlatform";
		name = %plat5;
		position = Vector::add(GameBase::getPosition(%player), Vector::rotVector("1 -5 0",%rot));
		rotation = Vector::add(%rot, "1.55 0 0");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);
	};

	instant StaticShape %plat6
	{
		dataBlock = "DeployableMediumForceField";
		name = %plat6;
		position = Vector::add(GameBase::getPosition(%player), Vector::rotVector("1 5 0",%rot));
		rotation = Vector::add(%rot, "0 0 0");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);
	};

	//=================== BoxBase Radar
	instant Sensor %name1
	{
		dataBlock = "MediumPulseSensor";
		name = %name1;
		position = Vector::add(GameBase::getPosition(%player), Vector::rotVector("1 -0 5.5",%rot));
		rotation = Vector::add(%rot, "0 0 0");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);
	};

	//=================== Air base Gen
	instant StaticShape %name2
	{
		dataBlock = "SolarPanel";
		name = %name2;
		position = Vector::add(GameBase::getPosition(%player), Vector::rotVector("1 4 6",%rot));
		rotation = Vector::add(%rot, "0 0 3.15");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);
	};

	//=================== Invo Station
	instant StaticShape %name4
	{
		dataBlock = "InventoryStation";
		name = %name4;
		position = Vector::add(GameBase::getPosition(%player), Vector::rotVector("1 -3 0.5",%rot));
		rotation = Vector::add(%rot, "0 0 3.15");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);
	};

	addToSet("MissionCleanup/BoxBase" @ %client @ %number,%name1);
	addToSet("MissionCleanup/BoxBase" @ %client @ %number,%name2);
	addToSet("MissionCleanup/BoxBase" @ %client @ %number,%name4);
	addToSet("MissionCleanup/BoxBase" @ %client @ %number,%plat1);
	addToSet("MissionCleanup/BoxBase" @ %client @ %number,%plat2);
	addToSet("MissionCleanup/BoxBase" @ %client @ %number,%plat3);
	addToSet("MissionCleanup/BoxBase" @ %client @ %number,%plat4);
	addToSet("MissionCleanup/BoxBase" @ %client @ %number,%plat5);
	addToSet("MissionCleanup/BoxBase" @ %client @ %number,%plat6);

	addToSet("MissionCleanup",%name1);
	addToSet("MissionCleanup",%name2);
	addToSet("MissionCleanup",%name4);
	addToSet("MissionCleanup",%plat1);
	addToSet("MissionCleanup",%plat2);
	addToSet("MissionCleanup",%plat3);
	addToSet("MissionCleanup",%plat4);
	addToSet("MissionCleanup",%plat5);
	addToSet("MissionCleanup",%plat6);

	%n1 = (nametoId("MissionCleanup/BoxBase" @ %client @ %number @ "/" @ %name1));
	%n2 = (nametoId("MissionCleanup/BoxBase" @ %client @ %number @ "/" @ %name2));
	%n4 = (nametoId("MissionCleanup/BoxBase" @ %client @ %number @ "/" @ %name4));
	
	%n7 = (nametoId("MissionCleanup/BoxBase" @ %client @ %number @ "/" @ %plat1));
	%n8 = (nametoId("MissionCleanup/BoxBase" @ %client @ %number @ "/" @ %plat2));
	%n9 = (nametoId("MissionCleanup/BoxBase" @ %client @ %number @ "/" @ %plat3));
	%n10 = (nametoId("MissionCleanup/BoxBase" @ %client @ %number @ "/" @ %plat4));
	%n11 = (nametoId("MissionCleanup/BoxBase" @ %client @ %number @ "/" @ %plat5));
	%n12 = (nametoId("MissionCleanup/BoxBase" @ %client @ %number @ "/" @ %plat6));

	gamebase::setteam(%n1, gamebase::getteam(%client));
	gamebase::setteam(%n2, gamebase::getteam(%client));
	gamebase::setteam(%n4, gamebase::getteam(%client));

	gamebase::setteam(%n7, gamebase::getteam(%client));
	gamebase::setteam(%n8, gamebase::getteam(%client));
	gamebase::setteam(%n9, gamebase::getteam(%client));
	gamebase::setteam(%n10, gamebase::getteam(%client));
	gamebase::setteam(%n11, gamebase::getteam(%client));
	gamebase::setteam(%n12, gamebase::getteam(%client));

	playSound(SoundPickupBackpack,$los::position);
	$TeamItemCount[GameBase::getTeam(%player) @ "BoxBase"]++;
	Client::sendMessage(%client,1,"Mini-Base Deployed.");
	return true;
}

//== from shifter
function Vector::rotVector(%vec,%rot)
{
	// this function rotates a vector about the z axis

	%vec_x = getWord(%vec,0);
	%vec_y = getWord(%vec,1);
	%vec_z = getWord(%vec,2);

	// new vector with z axis removed
	%basevec = %vec_x @ "  " @ %vec_y @ "  0";
	
	// change vector to distance and rotation
	%basedis = Vector::getDistance( "0 0 0", %basevec);
	%normvec = Vector::normalize( %basevec );
	%baserot = Vector::add( Vector::getRotation( %normvec ), "1.571 0 0" );

	// modify rotation and change back to vector (put z axis offset back)
	%newrot = Vector::add( %baserot, %rot );
	%newvec = Vector::getFromRot( %newrot, %basedis, %vec_z );

	return %newvec;
}

StaticShapeData LargeBoxBasePlatform
{
        shapeFile = "elevator16x16_octo";
        debrisId = defaultDebrisLarge;
        maxDamage = 36.0;
        damageSkinData = "objectDamageSkins";
        shadowDetailMask = 16;
        explosionId = debrisExpLarge;
        visibleToSensor = false;
        mapFilter = 4;
        description = "Air Base";
};