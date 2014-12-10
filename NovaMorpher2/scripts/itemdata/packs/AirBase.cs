$packDiscription[airbase] = "This enhancer has the ability to speed you up when to pass through it!";

$TeamItemMax[airbase] = 5;

$InvList[airbase] = 1;
$RemoteInvList[airbase] = 1;

$ItemMax[marmor, airbase] = 1;
$ItemMax[mfemale, airbase] = 1;

Patch::AddReInit("airbase");
//==================================================================================================
/// Created by TriCon Team C3 & graphfx
/// http://www.planetstarsiege.com/tricon
/// Rewritten By Emo1313 - Per placement

ItemImageData airbasePackImage
{
        shapeFile = "ammopack";
        mountPoint = 2;
        mountOffset = { 0, 0, 0 };
        mountRotation = { 0, 0, 0 };
        firstPerson = false;
};

ItemData airbase
{
	description = "AirBase";
	shapeFile = "shieldpack";
	className = "Backpack";
	heading =  "jDeployable Base";
	imageType = airbasePackImage;
	shadowDetailMask = 4;
	mass = 5.0;
	elasticity = 0.2;
	price = 2000;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function airbase::onUse(%player,%item)
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

function airbase::onDeploy(%player,%item,%pos)
{
        if (airbase::deployShape(%player,%item))
        {
                Player::decItemCount(%player,%item);
        }
}

function CreateAirBaseSet(%client)
{
    	%teleset = nameToID("MissionCleanup/AirBase" @ %client);
	if(%teleset == -1)
	{
		newObject("AirBase" @ %client,SimSet);
		addToSet("MissionCleanup","AirBase" @ %client);
		gamebase::setteam(%teleset, gamebase::getteam(%client));
	}
}

function airbase::deployshape(%player,%item)
{
        GameBase::getLOSInfo(%player,3);
        
        %client = Player::getClient(%player);
	%playerPos = GameBase::getPosition(%player);
	%deploypos = Vector::add(GameBase::getPosition(%player), "-0 -0 50.50");

	%number = $TeamItemCount[GameBase::getTeam(%player) @ "airbase"];

	if (!CheckForObjects(%deploypos,45,45,25))
	{
		Client::sendMessage(%client,1,"Objects In The Way, Can not deploy.");
		return false;
	}
	
	CreateAirBaseSet(%client);

	//================== Airbase platforms
	%name1 = "Sensor" @ Client::getName(%client) @ "" @ %number;
	%name2 = "StationGenerator" @ Client::getName(%client) @ "" @ %number;
	%name3 = "CommandStation" @ Client::getName(%client) @ "" @ %number;
	%name4 = "InventoryStation" @ Client::getName(%client) @ "" @ %number;
	%name5 = "VehiclePad" @ Client::getName(%client) @ "" @ %number;
	%name6 = "VehicleStation" @ Client::getName(%client) @ "" @ %number;
	
	
	%plat1 = "Platform1";
	%plat2 = "Platform2";
	%plat3 = "Platform3";
	%plat4 = "Platform4";

	instant StaticShape %plat1
	{
		dataBlock = "LargeAirBasePlatform";
		name = %plat1;
		position = Vector::add(GameBase::getPosition(%player), "-6.75 -5.0 58.00");
		rotation = Vector::add(GameBase::getRotation(%player), "0 0 0");
		destroyable = "False";
		deleteOnDestroy = "True";
		VehiclePad = %name5;
		team = GameBase::getTeam(%player);
	};

	instant StaticShape %plat2
	{
		dataBlock = "LargeAirBasePlatform";
		name = %plat2;
		position = Vector::add(GameBase::getPosition(%player), "6.75 -0 50.00");
		rotation = Vector::add(GameBase::getRotation(%player), "0 0 0");
		destroyable = "False";
		deleteOnDestroy = "True";
		VehiclePad = %name5;
		team = GameBase::getTeam(%player);
	};

	instant StaticShape %plat3
	{
		dataBlock = "LargeAirBasePlatform";
		name = %plat3;
		position = Vector::add(GameBase::getPosition(%player), "-6.75 -5.0 50.00");
		rotation = Vector::add(GameBase::getRotation(%player), "0 0 0");
		destroyable = "False";
		deleteOnDestroy = "True";
		VehiclePad = %name5;
		team = GameBase::getTeam(%player);
	};

	instant StaticShape %plat4
	{
		dataBlock = "LargeAirBasePlatform";
		name = %plat4;
		position = Vector::add(GameBase::getPosition(%player), "6.75 -0 58.00");
		rotation = Vector::add(GameBase::getRotation(%player), "0 0 0");
		destroyable = "False";
		deleteOnDestroy = "True";
		VehiclePad = %name5;
		team = GameBase::getTeam(%player);
	};

	//=================== Airbase Radar
	instant Sensor %name1
	{
		dataBlock = "PulseSensor";
		name = %name1;
		position = Vector::add(GameBase::getPosition(%player), "-0 -2.0 58.50");
		rotation = Vector::add(GameBase::getRotation(%player), "0 0 0");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);
	};

	//=================== Air base Gen
	instant StaticShape %name2
	{
		dataBlock = "ABGenerator";
		name = %name2;
		position = Vector::add(GameBase::getPosition(%player), "-3 -5 50.40");
		rotation = Vector::add(GameBase::getRotation(%player), "0 0 4.71339");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);
	};

	//=================== Command Station
	instant StaticShape %name3
	{
		dataBlock = "CommandStation";
		name = %name3;
		position = Vector::add(GameBase::getPosition(%player), "-7 -5 50.40");
		rotation = Vector::add(GameBase::getRotation(%player), "0 0 4.71339");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);
	};

	//=================== Invo Station
	instant StaticShape %name4
	{
		dataBlock = "InventoryStation";
		name = %name4;
		position = Vector::add(GameBase::getPosition(%player), "5 0 50.40");
		rotation = Vector::add(GameBase::getRotation(%player), "0 0 4.71339");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);
	};
	//==================== Vehicle Stations
	instant StaticShape %name5
	{
		dataBlock = "VehiclePad";
		name = %name5;
		position = Vector::add(GameBase::getPosition(%player), "8 0 59.40");
		rotation = Vector::add(GameBase::getRotation(%player), "0 0 4.71339");
		destroyable = "True";
		deleteOnDestroy = "True";
		team = GameBase::getTeam(%player);






	};

	instant StaticShape %name6
	{
		dataBlock = "VehicleStation";
		name = %name6;
		position = Vector::add(GameBase::getPosition(%player), "-8 -5 58.50");
		rotation = Vector::add(GameBase::getRotation(%player), "0 0 4.71339");
		destroyable = "True";
		deleteOnDestroy = "True";
		VehiclePad = %name5;
		team = GameBase::getTeam(%player);
	};

	addToSet("MissionCleanup/AirBase" @ %client,%name1);
	addToSet("MissionCleanup/AirBase" @ %client,%name2);
	addToSet("MissionCleanup/AirBase" @ %client,%name3);
	addToSet("MissionCleanup/AirBase" @ %client,%name4);
	addToSet("MissionCleanup/AirBase" @ %client,%name5);
	addToSet("MissionCleanup/AirBase" @ %client,%name6);
	addToSet("MissionCleanup/AirBase" @ %client,%plat1);
	addToSet("MissionCleanup/AirBase" @ %client,%plat2);
	addToSet("MissionCleanup/AirBase" @ %client,%plat3);
	addToSet("MissionCleanup/AirBase" @ %client,%plat4);

	addToSet("MissionCleanup",%name1);
	addToSet("MissionCleanup",%name2);
	addToSet("MissionCleanup",%name3);
	addToSet("MissionCleanup",%name4);
	addToSet("MissionCleanup",%name5);
	addToSet("MissionCleanup",%name6);
	addToSet("MissionCleanup",%plat1);
	addToSet("MissionCleanup",%plat2);
	addToSet("MissionCleanup",%plat3);
	addToSet("MissionCleanup",%plat4);

	%n1 = (nametoId("MissionCleanup/AirBase" @ %client @ "/" @ %name1));
	%n2 = (nametoId("MissionCleanup/AirBase" @ %client @ "/" @ %name2));
	%n3 = (nametoId("MissionCleanup/AirBase" @ %client @ "/" @ %name3));
	%n4 = (nametoId("MissionCleanup/AirBase" @ %client @ "/" @ %name4));
	%n5 = (nametoId("MissionCleanup/AirBase" @ %client @ "/" @ %name5));
	%n6 = (nametoId("MissionCleanup/AirBase" @ %client @ "/" @ %name6));
	
	%n6.vehiclePad = %n5;
	
	%n7 = (nametoId("MissionCleanup/AirBase" @ %client @ "/" @ %plat1));
	%n8 = (nametoId("MissionCleanup/AirBase" @ %client @ "/" @ %plat2));
	%n9 = (nametoId("MissionCleanup/AirBase" @ %client @ "/" @ %plat3));
	%n10 = (nametoId("MissionCleanup/AirBase" @ %client @ "/" @ %plat4));

	gamebase::setteam(%n1, gamebase::getteam(%client));
	gamebase::setteam(%n2, gamebase::getteam(%client));
	gamebase::setteam(%n3, gamebase::getteam(%client));
	gamebase::setteam(%n4, gamebase::getteam(%client));
	gamebase::setteam(%n5, gamebase::getteam(%client));
	gamebase::setteam(%n6, gamebase::getteam(%client));

	playSound(SoundPickupBackpack,$los::position);
	$TeamItemCount[GameBase::getTeam(%player) @ "airbase"]++;
	Client::sendMessage(%client,1,"Air Base Deployed.");
	return true;
}

StaticShapeData LargeAirBasePlatform
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

StaticShapeData ABGenerator
{
	description = "AirBase Generator";
	shapeFile = "generator_p";
	className = "Generator";
	debrisId = flashDebrisSmall;
 	sfxAmbient = SoundGeneratorPower;
	maxDamage = 1.6;
	mapIcon = "M_generator";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpMedium;
	visibleToSensor = true;
	mapFilter = 4;


	explosionRadius = 60.0;
	damageValue = 73;
	damageType = $DebrisDamageType;
};

function ABGenerator::onDestroyed(%this)
{
	StaticShape::onDestroyed(%this);

	//== These should kill it!

	%pos = GameBase::getPosition(%this);
	%obj = newObject("","Mine","Meteoroid");
	GameBase::setPosition(%obj, %pos);
	GameBase::setDamageLevel(%obj,10000000);

	%pos = GameBase::getPosition(%this);
	%obj = newObject("","Mine","Meteoroid");
	GameBase::setPosition(%obj, %pos);
	GameBase::setDamageLevel(%obj,10000000);

	%pos = GameBase::getPosition(%this);
	%obj = newObject("","Mine","Meteoroid");
	GameBase::setPosition(%obj, %pos);
	GameBase::setDamageLevel(%obj,10000000);
}