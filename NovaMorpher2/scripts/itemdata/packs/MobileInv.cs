ExplosionData MInvEx
{
   shapeName = "shockwave_large.dts";
   soundId   = shockExplosion;

   faceCamera = false;
   randomSpin = true;
   hasLight   = true;
   lightRange = 10.0;

   timeZero = 1.000;
   timeOne  = 3.000;

   colors[0]  = { 0.5, 1.0, 0.0 };
   colors[1]  = { 0.0, 0.5, 1.0 };
   colors[2]  = { 1.0, 0.0, 0.5 };
   radFactors = { 1.0, 1.0, 1.0 };
};

//== $DenyBuy[MobileInvList, %item] = 1; == Makes this item NOT show in mobile inventory.
$DenyBuy[MobileInv , MobileInvPack] = 1; //== Prevents this pack from deploying itself :'P

StaticShapeData MobileInv 
{ 
	description = "Mobile Suppy Unit"; 
	//shapeFile = "inventory_sta"; 
	//shapeFile = "vehi_pur_pnl";
	shapefile = "mainpad";
	className = "DeployableStation"; 
	maxDamage = 0.6; 
	sequenceSound[0] = { "activate", SoundActivateInventoryStation }; 
	sequenceSound[1] = { "power", SoundInventoryStationPower }; 
	sequenceSound[2] = { "use", SoundUseInventoryStation };
	visibleToSensor = true; 
	shadowDetailMask = 4; 
	castLOS = true; 
	supression = false; 
	supressable = false; 
	mapFilter = 4; 
	mapIcon = "M_station"; 
	debrisId = flashDebrisMedium; 
	damageSkinData = "objectDamageSkins"; 
	explosionId = MInvEx; 
}; 

function MobileInv::onAdd(%this) 
{ 
	if (GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "M-Inv Station"); 
	%this.Energy = $RemoteInvEnergy; 
} 

function MobileInv::onEnabled(%this) 
{ 
	GameBase::playSequence(%this,0,"power");
 	GameBase::playSequence(%this,1);
} 

function MobileInv::onDestroyed(%this)
{
	MobileInv::onDisabled(%this);
	%stationName = GameBase::getDataName(%this);

    	$TeamItemCount[GameBase::getTeam(%this) @ "MobileInvPack"]--;
	calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.30, 	0.1, 200, 100); 
	Station::weaponCheck(%this);
}

function MobileInv::onDisabled(%this) 
{ 
	GameBase::stopSequence(%this,0); 
	GameBase::setSequenceDirection(%this,1,0); 
	GameBase::pauseSequence(%this,1); 
	GameBase::stopSequence(%this,2); 
	Station::checkTarget(%this);
} 

function MobileInv::onActivate(%this) 
{
	%obj = Station::getTarget(%this);
	%this.lastPlayer = %obj;
	if (%obj != -1) 
	{ 
	 	GameBase::playSequence(%this,1,"activate");
		GameBase::setSequenceDirection(%this,1,1);
		InventoryStation::onResupply(%this,"InvList"); 
	} 
	else 
	{
		dbecho(3, "%obj is -1 which is invalid");
		GameBase::setActive(%this,false); 
	}
} 

function MobileInv::onDeactivate(%this) 
{ 
	GameBase::stopSequence(%this,2);
 	GameBase::setSequenceDirection(%this,1,0);
} 

function MobileInv::onCollision(%this, %object) 
{
	if($debug)
		echo("Collision (" @ %this @ ", " @ %object @ ")");
	%obj = getObjectType(%object);
	if (%obj == "Player" && isPlayerBusy(%object) == 0)
	{
  	 	%client = Player::getClient(%object);
		if(GameBase::getTeam(%object) == GameBase::getTeam(%this) || GameBase::getTeam(%this) == -1)
		{
			if (GameBase::getDamageState(%this) == "Enabled")
			{
				if(%this.enterTime == "") 
					%this.enterTime = getSimTime();

				GameBase::setActive(%this,true);
			}
			else 
				Client::sendMessage(%client,0,"Unit is disabled");
		}
	      else if(Station::getTarget(%this) == %object)
		{
			%curTime = getSimTime();
			if(%curTime - %object.stationDeniedStamp > 3.5 && GameBase::getDamageState(%this) == "Enabled")
			{
				%object.stationDeniedStamp = %curTime;
				Client::sendMessage(%client,0,"--ACCESS DENIED-- Wrong Team ~waccess_denied.wav");
			}
			else if($debug)
				echo("Failiar at if/else statement 3b on function MobileInv::onCollision");

		}
		else
		{
			if($debug)
				echo("Failiar at if/else statement 2 on function MobileInv::onCollision");
		}
	}
	else
	{
		if($debug)
			echo("Failiar at if/else statement 1 on function MobileInv::onCollision");
	}
}
//----------------------------------------------------------------------------

ItemImageData MobileInvPackImage
{
	shapeFile = "panel_set";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.3 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData MobileInvPack
{
	description = "M-Inv Station";
	shapeFile = "panel_set";
	className = "Backpack";
   heading = "fStations";
	shadowDetailMask = 4	;
	imageType = MobileInvPackImage;
	mass = 4.0;
	elasticity = 0.2;
	price = 5821;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function MobileInvPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function MobileInvPack::onDeploy(%player,%item,%pos)
{
	if (MobileInvPack::deployShape(%player,%item)) {
		Player::decItemCount(%player,%item);
	}
}	

function MobileInvPack::deployShape(%player,%item)
{
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) {
		if (GameBase::getLOSInfo(%player,3)) {
			%obj = getObjectType($los::object);
			if (Vector::dot($los::normal,"0 0 1") > 0.7) {
				if(checkDeployArea(%client,$los::position)) {
					%inv = newObject("ammounit_remote","StaticShape","MobileInv",true);
 	 		     		addToSet("MissionCleanup", %inv);
					%rot = GameBase::getRotation(%player); 
					GameBase::setTeam(%inv,GameBase::getTeam(%player));

					%pos = Vector::add($los::position,"0 0 0.01");

					GameBase::setPosition(%inv,%pos);
					GameBase::setRotation(%inv,%rot);
					Gamebase::setMapName(%inv,%name);
					Client::sendMessage(%client,0,"Inventory Station deployed");
					playSound(SoundPickupBackpack,$los::position);
					$TeamItemCount[GameBase::getTeam(%inv) @ "MobileInvPack"]++;
					echo("MSG: ",%client," deployed an Inventory Station");
					return true;
				}
			}
			else
				Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
		}
		else
			Client::sendMessage(%client,0,"Deploy position out of range");
	}
	else																						  
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
	return false;
}



$packDiscription[MobileInvPack] = "This is a degraded version of the Inventory Station. Not to be confused with Deployable Inventory Station. It can buy anything the Inventory Station buys BUT itself....";

$TeamItemMax[MobileInvPack] = 2;

$InvList[MobileInvPack] = 1;
$RemoteInvList[MobileInvPack] = 0;

$ItemMax[marmor, MobileInvPack] = 1;
$ItemMax[mfemale, MobileInvPack] = 1;

$ItemMax[harmor, MobileInvPack] = 1;

Patch::AddReInit("MobileInvPack");