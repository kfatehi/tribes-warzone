//== This TELEPORT Pad system will be chained up. The maximum amount of teleport pads would be 5.


//== Link, delete, reset functions
function AddTeleport(%this, %team)
{	// Create a New Teleport Index
	$TeleportID[$NTeleport[%team], %team]=%this;
	$TeleportThis[%this] = $NTeleports[%team];

	%PrevTeleport = 0;
	if($NTeleport[%team] > 0)
		%PrevTeleport = $NTeleport[%team]-1;
	while(true)
	{
		if($TeleportID[%PrevTeleport, %team] == "")
			%PrevTeleport = %PrevTeleport-1;
		else
			break;
	}
	$NextTeleport[$TeleportID[%PrevTeleport, %team]]	=	%this;			// Set the Prev Teleport to The New Teleport
	$NextTeleport[%this]						=	$TeleportID[0, %team];	// Set the Next Teleport to the First Teleport

	if($debug)
	{
		echo("Teleport Data Added");
		echo("$TeleportID[" @ $NTeleport[%team] @ ", " @ %team @"] = " @ $TeleportID[$NTeleport[%team], %team]);
	}

	$NTeleport[%team]++;
}

//== ARRGG So Complicated :'(
function DeleteTeleport(%this)
{
	echo("Deleteing Teleport Data");
	$TeleportID[$NTeleport[%team], %team]=%this;

	//======================= Check for previous teleport =================
	%PrevTeleport = 0;
	if($NTeleport[%team] > 0)
		%PrevTeleport = $NTeleport[%team]-1;
	while(true)
	{
		if($TeleportID[%PrevTeleport, %team] != "")
			%PrevTeleport = $NTeleport[%team]-1;
		else
			break;
	}

	//======================= Check for next teleport =================
	%NextTeleport = 0;
	if($NTeleport[%team] > 0)
		%NextTeleport = $NTeleport[%team]+1;
	while(true)
	{
		if($TeleportID[%NextTeleport, %team] != "")
			%NextTeleport = $NTeleport[%team]-1;
		else
			break;
	}

	$NextTeleport[$TeleportID[%PrevTeleport, %team]] = $NextTeleport[$TeleportID[%NextTeleport, %team]]; // Set the Prev Teleport to The Next Teleport

	$NextTeleport[%this] = "";
	$TeleportID[%this, %team] = "";
	$TeleportThis[%this] = "";
}

//== Easy peasy lemon squisy
function ResetTeleports()
{
	for(%i = -1; %i < 5; %i++)
		$NTeleport[%i] = 0;

	for(%i = -1; %i < 5; %i++)
	{
		for(%o = 0; %o < 6; %o++)
		{
			%this = $TeleportID[%o, %i];

			$NextTeleport[%this] = "";
			$TeleportID[%o, %i] = "";
			$TeleportThis[%this] = "";
		}
	}
}

//                                                  =-=-=-=-=- Teleporter =-=-=-=-
StaticShapeData DeployableTeleport
{
    	className = "DeployableTeleport";
	damageSkinData = "objectDamageSkins";

	shapeFile = "flagstand";
	maxDamage = 1.75;
	maxEnergy = 200;

   	mapFilter = 2;
	visibleToSensor = true;
    	explosionId = mortarExp;
    	debrisId = flashDebrisLarge;

	lightRadius = 12.0;
	lightType=2;
	lightColor = {1.0,0.2,0.2};
};
				
function RemoveBeam(%b)
{
	deleteObject(%b);
}				
														 
function DeployableTeleport::onDestroyed(%this)
{
	schedule("RemoveBeam("@%this.beam1@");",1);

	$TeamItemCount[GameBase::getTeam(%this) @ "TeleportPack"]--;

	%teleset = nameToID("MissionCleanup/Teleports");

	%count = 0;
	for(%i = 0; (%o = Group::getObject(%teleset, %i)) != -1; %i++)
	{
		if(GameBase::getTeam(%o) == GameBase::getTeam(%this) && %o != %this)
		{
			GameBase::applyDamage(%o,$DebrisDamageType,20,GameBase::getPosition(%o),"0 0 0","0 0 0",%this);
			%count++;
			if(%count == 5)
				break;
		}
	}

	DeleteTeleport(%this);
}

function DeployableTeleport::onCollision(%this,%obj)
{
	if(getObjectType(%obj) != "Player")
	{
		return;
	}

	if(Player::isDead(%obj))
	{
		return;
	}

	%c = Player::getClient(%obj);
	%playerTeam = GameBase::getTeam(%obj);
	%teleTeam = GameBase::getTeam(%this);

	if((Player::getArmor(%obj) == "nmarmor") || (Player::getArmor(%obj) == "nmfemale") || (Player::getArmor(%obj) == "nmlightarmor") || (Player::getArmor(%obj) == "nmlightfemale") || (Player::getArmor(%obj) == "nmheavyarmor"))
	{
		Client::SendMessage(%c,0,"Your molecular structure has been corrupted by the magnetic field...");
		%dlevel = GameBase::getDamageLevel(%obj) + 0.5;
		GameBase::setDamageLevel(%obj,%dlevel);

		%flash = %dlevel * 2;
		Player::setDamageFlash(%obj,%flash);
	}

	if(%teleTeam != %playerTeam)
	{
		if(Player::getArmor(%obj) == "amarmor")
		{
			Client::SendMessage(%c,0,"Recombining molecular structure... Passing through enemy teleport...");
		}
		else
		{ 
			Client::SendMessage(%c,0,"Wrong Team"); 
			return;
		}
	}

	if(%this.disabled == true)
	{
	        Client::SendMessage(%c,0,"Teleport Pad is recharging");
	        return;
	}

	if((Player::getArmor(%obj) == "magearmor") || (Player::getArmor(%obj) == "magefemale"))
	{
		Client::SendMessage(%c,0,"Cannot teleport non-armored personals.");
		return;
	}
	else
	{
		%o = $NextTeleport[%this];
		if(%o != %this)
		{
			GameBase::playSound(%o,ForceFieldOpen,0);
			GameBase::playSound(%this,ForceFieldOpen,0);
	           	GameBase::SetPosition(%obj,GameBase::GetPosition(%o));
			%this.Disabled = true;
			%o.Disabled = true;
			schedule("DeployableTeleport::Reenable("@%o@");",2.5);
			schedule("DeployableTeleport::Reenable("@%this@");",2.5);
			return;
		}
		else
		{
			Client::SendMessage(%c,0,"No other pad to teleport to.");
		}
	}
}

function DeployableTeleport::Reenable(%this)
{
	%this.disabled = false;
}


















//==================================================================================================== Teleport Pad

ItemImageData TeleportPackImage
{
	shapeFile = "flagstand";
	mountPoint = 2;
	mountOffset = { 0, 0, 0.1 };
	mountRotation = { 1.57, 0, 0 };
	firstPerson = false;
};

ItemData TeleportPack
{
	description = "Teleport Pad";
	shapeFile = "flagstand";
	className = "Backpack";
    	heading = "dDefence";
	imageType = TeleportPackImage;
	shadowDetailMask = 4;
	mass = 5.0;
	elasticity = 0.2;
	price = 3200;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function TeleportPack::onUse(%player,%item)
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

function TeleportPack::onDeploy(%player,%item,%pos)
{
	if (teleportPack::deployShape(%player,"Teleport Pad",DeployableTeleport,%item))
	{
		Player::decItemCount(%player,%item);
		$TeamItemCount[GameBase::getTeam(%player) @ "TeleportPack"]++;
	}
}

function CreateteleportSimSet()
{
    	%teleset = nameToID("MissionCleanup/Teleports");
	if(%teleset == -1)
	{
		newObject("Teleports",SimSet);
		addToSet("MissionCleanup","Teleports");
	}
}

function TeleportPack::deployShape(%player,%name,%shape,%item)
{
	%client = Player::getClient(%player);
	
	if($TeamItemCount[GameBase::getTeam(%player) @ "TeleportPack"] < $TeamItemMax[TeleportPack]) {
	
	if (GameBase::getLOSInfo(%player,5))
	{
		%obj = getObjectType($los::object);
			if (Vector::dot($los::normal,"0 0 1") > 0.7)
			{
				if(checkDeployArea(%client,$los::position))
				{
					%sensor = newObject("Teleport Pad","StaticShape",%shape,true);

					CreateteleportSimSet();
					addToSet("MissionCleanup/Teleports", %sensor);
					addToSet("MissionCleanup", %sensor);

					GameBase::setTeam(%sensor,GameBase::getTeam(%player));
					%pos = Vector::add($los::position,"0 0 1");
					GameBase::setPosition(%sensor,%pos);
					Gamebase::setMapName(%sensor,%name);
					Client::sendMessage(%client,0,%item.description @ " deployed");
					%sensor.disabled = false;
					playSound(SoundPickupBackpack,$los::position);

					%beam = newObject("","StaticShape",ElectricalBeam,true);
					addToSet("MissionCleanup", %beam);
					GameBase::setTeam(%beam,GameBase::getTeam(%player));
					GameBase::setPosition(%beam,%pos);

					%sensor.beam1 = %beam;

					AddTeleport(%sensor, GameBase::getTeam(%player));
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
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %name @ "s");
	return false;
}

$packDiscription[TeleportPack] = "There could be up to 5 teleports linked up together. The teleportation will be in a orderly fashion!";

$TeamItemMax[TeleportPack] = 5;

$InvList[TeleportPack] = 1;
$RemoteInvList[TeleportPack] = 1;

$ItemMax[marmor, TeleportPack] = 1;
$ItemMax[mfemale, TeleportPack] = 1;

$ItemMax[harmor, TeleportPack] = 1;