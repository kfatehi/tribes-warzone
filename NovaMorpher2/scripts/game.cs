exec(Server::detVerAp("comchat.cs"));
$SensorNetworkEnabled = true;

$GuiModePlay = 1;
$GuiModeCommand = 2;
$GuiModeVictory = 3;
$GuiModeInventory = 4;
$GuiModeObjectives = 5;
$GuiModeLobby = 6;


//  Global Variables

//---------------------------------------------------------------------------------
// Energy each team is given at beginning of game
//---------------------------------------------------------------------------------
$DefaultTeamEnergy = "Infinite";

//---------------------------------------------------------------------------------
// Team Energy variables
//---------------------------------------------------------------------------------
$TeamEnergy[-1] = $DefaultTeamEnergy; 
$TeamEnergy[0]  = $DefaultTeamEnergy; 
$TeamEnergy[1]  = $DefaultTeamEnergy; 
$TeamEnergy[2]  = $DefaultTeamEnergy; 
$TeamEnergy[3]  = $DefaultTeamEnergy; 
$TeamEnergy[4]  = $DefaultTeamEnergy; 
$TeamEnergy[5]  = $DefaultTeamEnergy; 
$TeamEnergy[6]  = $DefaultTeamEnergy; 				
$TeamEnergy[7]  = $DefaultTeamEnergy; 

//---------------------------------------------------------------------------------
// Time in sec player must wait before he can throw a Grenade or Mine after leaving
//	a station.
//---------------------------------------------------------------------------------
$WaitThrowTime = 2;

//---------------------------------------------------------------------------------
// If 1 then Team Spending Ignored -- Team Energy is set to $MaxTeamEnergy every
// 	$secTeamEnergy.
//---------------------------------------------------------------------------------
$TeamEnergyCheat = 0;

//---------------------------------------------------------------------------------
// MAX amount team energy can reach
//---------------------------------------------------------------------------------
$MaxTeamEnergy = 700000;

//---------------------------------------------------------------------------------
//  Time player has to put flag in flagstand before it gets returned to its last
//  location. 
//---------------------------------------------------------------------------------
$flagToStandTime = 180;	  

//---------------------------------------------------------------------------------
// Amount to inc team energy every ($secTeamEnergy) seconds
//---------------------------------------------------------------------------------
$incTeamEnergy = 700;

//---------------------------------------------------------------------------------
// (Rate is sec's) Set how often TeamEnergy is incremented
//---------------------------------------------------------------------------------
$secTeamEnergy = 30;

//---------------------------------------------------------------------------------
// (Rate is sec's) Items respawn
//---------------------------------------------------------------------------------
$ItemRespawnTime = 30;

//---------------------------------------------------------------------------------
//Amount of Energy remote stations start out with
//---------------------------------------------------------------------------------
$RemoteAmmoEnergy = 2500; 
$RemoteInvEnergy = 3000;

//---------------------------------------------------------------------------------
// TEAM ENERGY -  Warn team when teammate has spent x amount - Warn team that 
//				  energy level is low when it reaches x amount 
//---------------------------------------------------------------------------------
$TeammateSpending = -4000;  //Set = to 0 if don't want the warning message
$WarnEnergyLow = 4000;		 //Set = to 0 if don't want the warning message

//---------------------------------------------------------------------------------
// Amount added to TeamEnergy when a player joins a team
//---------------------------------------------------------------------------------
$InitialPlayerEnergy = 5000;

//---------------------------------------------------------------------------------
// REMOTE TURRET
//---------------------------------------------------------------------------------
$MaxNumTurretsInBox = 2;	  //Number of remote turrets allowed in the area
$TurretBoxMaxLength = 50;	 //Define Max Length of the area
$TurretBoxMaxWidth =  50;	 //Define Max Width of the area
$TurretBoxMaxHeight = 25;	 //Define Max Height of the area

$TurretBoxMinLength = 10;	  //Define Min Length from another turret
$TurretBoxMinWidth =  10;	  //Define Min Width from another turret
$TurretBoxMinHeight = 10;	 //Define Min Height from another turret

//---------------------------------------------------------------------------------
//	Object Types	
//---------------------------------------------------------------------------------
$SimTerrainObjectType	 = 1 << 1;
$SimInteriorObjectType	= 1 << 2;
$SimPlayerObjectType	  = 1 << 7;

$MineObjectType			 = 1 << 26;	
$MoveableObjectType		 = 1 << 22;
$VehicleObjectType	 	 = 1 << 29;  
$StaticObjectType			 = 1 << 23;		
$ItemObjectType			 = 1 << 21;	  

//---------------------------------------------------------------------------------
// CHEATS
//---------------------------------------------------------------------------------
$ServerCheats = 0;
$TestCheats = 0;

//---------------------------------------------------------------------------------
//Respawn automatically after X sec's -  If 0..no respawn
//---------------------------------------------------------------------------------
$AutoRespawn = 0;

//---------------------------------------------------------------------------------
// Player death messages - %1 = killer's name, %2 = victim's name
//		 %3 = killer's gender pronoun (his/her), %4 = victim's gender pronoun
//---------------------------------------------------------------------------------
$deathMsg[$LandingDamageType, 0]		= "%2 falls to %3 death.";
$deathMsg[$LandingDamageType, 1]		= "%2 forgot to tie %3 bungie cord.";
$deathMsg[$LandingDamageType, 2]		= "%2 bites the dust in a forceful manner.";
$deathMsg[$LandingDamageType, 3]		= "%2 fall down go boom.";
$deathMsg[$ImpactDamageType, 0]		= "%1 makes quite an impact on %2.";
$deathMsg[$ImpactDamageType, 1]		= "%2 becomes the victim of a fly-by from %1.";
$deathMsg[$ImpactDamageType, 2]		= "%2 leaves a nasty dent in %1's fender.";
$deathMsg[$ImpactDamageType, 3]		= "%1 says, 'Hey %2, you scratched my paint job!'";
$deathMsg[$BulletDamageType, 0]		= "%1 ventilates %2 with %3 chaingun.";
$deathMsg[$BulletDamageType, 1]		= "%1 gives %2 an overdose of lead.";
$deathMsg[$BulletDamageType, 2]		= "%1 fills %2 full of holes.";
$deathMsg[$BulletDamageType, 3]		= "%1 guns down %2.";
$deathMsg[$EnergyDamageType, 0]		= "%2 dies of turret trauma.";
$deathMsg[$EnergyDamageType, 1]		= "%2 is chewed to pieces by a turret.";
$deathMsg[$EnergyDamageType, 2]		= "%2 walks into a stream of turret fire.";
$deathMsg[$EnergyDamageType, 3]		= "%2 ends up on the wrong side of a turret.";
$deathMsg[$PlasmaDamageType, 0]		= "%2 feels the warm glow of %1's plasma.";
$deathMsg[$PlasmaDamageType, 1]		= "%1 gives %2 a white-hot plasma injection.";
$deathMsg[$PlasmaDamageType, 2]		= "%1 asks %2, 'Got plasma?'";
$deathMsg[$PlasmaDamageType, 3]		= "%1 gives %2 a plasma transfusion.";
$deathMsg[$ExplosionDamageType, 0]	= "%2 catches a Frisbee of Death thrown by %1.";
$deathMsg[$ExplosionDamageType, 1]	= "%1 blasts %2 with a well-placed disc.";
$deathMsg[$ExplosionDamageType, 2]	= "%1's spinfusor caught %2 by surprise.";
$deathMsg[$ExplosionDamageType, 3]	= "%2 falls victim to %1's Stormhammer.";
$deathMsg[$ShrapnelDamageType, 0]	 = "%1 blows %2 up real good.";
$deathMsg[$ShrapnelDamageType, 1]	 = "%2 gets a taste of %1's explosive temper.";
$deathMsg[$ShrapnelDamageType, 2]	 = "%1 gives %2 a fatal concussion.";
$deathMsg[$ShrapnelDamageType, 3]	 = "%2 never saw it coming from %1.";
$deathMsg[$LaserDamageType, 0]		 = "%1 adds %2 to %3 list of sniper victims.";
$deathMsg[$LaserDamageType, 1]		 = "%1 fells %2 with a sniper shot.";
$deathMsg[$LaserDamageType, 2]		 = "%2 becomes a victim of %1's laser rifle.";
$deathMsg[$LaserDamageType, 3]		 = "%2 stayed in %1's crosshairs for too long.";
$deathMsg[$MortarDamageType, 0]		= "%1 mortars %2 into oblivion.";
$deathMsg[$MortarDamageType, 1]		= "%2 didn't see that last mortar from %1.";
$deathMsg[$MortarDamageType, 2]		= "%1 inflicts a mortal mortar wound on %2.";
$deathMsg[$MortarDamageType, 3]		= "%1's mortar takes out %2.";
$deathMsg[$BlasterDamageType, 0]	  = "%2 gets a blast out of %1.";
$deathMsg[$BlasterDamageType, 1]	  = "%2 succumbs to %1's rain of blaster fire.";
$deathMsg[$BlasterDamageType, 2]	  = "%1's puny blaster shows %2 a new world of pain.";
$deathMsg[$BlasterDamageType, 3]	  = "%2 meets %1's master blaster.";
$deathMsg[$ElectricityDamageType, 0] = "%2 gets zapped with %1's ELF gun.";
$deathMsg[$ElectricityDamageType, 1] = "%1 gives %2 a nasty jolt.";
$deathMsg[$ElectricityDamageType, 2] = "%2 gets a real shock out of meeting %1.";
$deathMsg[$ElectricityDamageType, 3] = "%1 short-circuits %2's systems.";
$deathMsg[$CrushDamageType, 0]		 = "%2 didn't stay away from the moving parts.";
$deathMsg[$CrushDamageType, 1]		 = "%2 is crushed.";
$deathMsg[$CrushDamageType, 2]		 = "%2 gets smushed flat.";
$deathMsg[$CrushDamageType, 3]		 = "%2 gets caught in the machinery.";
$deathMsg[$DebrisDamageType, 0]		 = "%2 is a victim among the wreckage.";
$deathMsg[$DebrisDamageType, 1]		 = "%2 is killed by debris.";
$deathMsg[$DebrisDamageType, 2]		 = "%2 becomes a victim of collateral damage.";
$deathMsg[$DebrisDamageType, 3]		 = "%2 got too close to the exploding stuff.";
$deathMsg[$MissileDamageType, 0]		 = "%2 takes a missile up the keister.";
$deathMsg[$MissileDamageType, 1]		 = "%2 gets shot down.";
$deathMsg[$MissileDamageType, 2]		 = "%2 gets real friendly with a rocket.";
$deathMsg[$MissileDamageType, 3]		 = "%2 feels the burn from a warhead.";
$deathMsg[$MineDamageType, 0]			 = "%1 blows %2 up real good.";
$deathMsg[$MineDamageType, 1]			 = "%2 gets a taste of %1's explosive temper.";
$deathMsg[$MineDamageType, 2]			 = "%1 gives %2 a fatal concussion.";
$deathMsg[$MineDamageType, 3]			 = "%2 never saw it coming from %1.";

// "you just killed yourself" messages
//	%1 = player name,  %2 = player gender pronoun

$deathMsg[-2,0]						 = "%1 ends it all.";
$deathMsg[-2,1]						 = "%1 takes %2 own life.";
$deathMsg[-2,2]						 = "%1 kills %2 own dumb self.";
$deathMsg[-2,3]						 = "%1 decides to see what the afterlife is like.";

$numDeathMsgs = 4;
//---------------------------------------------------------------------------------
exec(Server::detVerAp(spawnlist));

function remotePlayMode(%clientId)
{
	if(!%clientId.guiLock)
	{
		remoteSCOM(%clientId, -1);
		Client::setGuiMode(%clientId, $GuiModePlay);
	}
}

function remoteCommandMode(%clientId)
{
	// can't switch to command mode while a server menu is up
	if(!%clientId.guiLock)
	{
		remoteSCOM(%clientId, -1);  // force the bandwidth to be full command
		if(%clientId.observerMode != "pregame")
			checkControlUnmount(%clientId);
		Client::setGuiMode(%clientId, $GuiModeCommand);
	}
}

function remoteInventoryMode(%clientId)
{
	if(!%clientId.guiLock && !Observer::isObserver(%clientId))
	{
		remoteSCOM(%clientId, -1);
		Client::setGuiMode(%clientId, $GuiModeInventory);
	}
}

function remoteObjectivesMode(%clientId)
{
	if(!%clientId.guiLock)
	{
		remoteSCOM(%clientId, -1);
		Client::setGuiMode(%clientId, $GuiModeObjectives);
	}
}

function remoteScoresOn(%clientId)
{
	if(!%clientId.menuMode && (!$isHeliumMonster[%clientId] || %clientId.isAdmin)) //== HAHAHAHHAAHAHAHHA
		Game::menuRequest(%clientId);

	%clientId.hasTabMenu = true;
}

function remoteScoresOff(%clientId)
{
	Client::cancelMenu(%clientId);

	%clientId.hasTabMenu = false;
}

function remoteToggleCommandMode(%clientId)
{
	if (Client::getGuiMode(%clientId) != $GuiModeCommand)
		remoteCommandMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function remoteToggleInventoryMode(%clientId)
{
	if (Client::getGuiMode(%clientId) != $GuiModeInventory)
		remoteInventoryMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function remoteToggleObjectivesMode(%clientId)
{
	if (Client::getGuiMode(%clientId) != $GuiModeObjectives)
		remoteObjectivesMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function Time::getMinutes(%simTime)
{
	return floor(%simTime / 60);
}

function Time::getSeconds(%simTime)
{
	return %simTime % 60;
}

function Game::pickRandomSpawn(%team)
{
	%group = nameToID("MissionGroup/Teams/team" @ %team @ "/DropPoints/Random");
	%count = Group::objectCount(%group);
	if(!%count)
		return -1;
  	%spawnIdx = floor(getRandom() * (%count - 0.1));
  	%value = %count;
	for(%i = %spawnIdx; %i < %value; %i++) {
		%set = newObject("set",SimSet);
		%obj = Group::getObject(%group, %i);
		if(containerBoxFillSet(%set,$SimPlayerObjectType|$VehicleObjectType,GameBase::getPosition(%obj),2,2,4,0) == 0) {
			deleteObject(%set);
			return %obj;		
		}
		if(%i == %count - 1) {
			%i = -1;
			%value = %spawnIdx;
		}
		deleteObject(%set);
	}
	return false;
}

function Game::pickStartSpawn(%team)
{
	%group = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\DropPoints\\Start");
	%count = Group::objectCount(%group);
	if(!%count)
		return -1;

	%spawnIdx = $lastTeamSpawn[%team] + 1;
	if(%spawnIdx >= %count)
		%spawnIdx = 0;
	$lastTeamSpawn[%team] = %spawnIdx;
	return Group::getObject(%group, %spawnIdx);
}

function Game::pickTeamSpawn(%team, %respawn)
{
	if(%respawn)
		return Game::pickRandomSpawn(%team);
	else
	{
		%spawn = Game::pickStartSpawn(%team);
		if(%spawn == -1)
			return Game::pickRandomSpawn(%team);
		return %spawn;
	}
}

function Game::pickObserverSpawn(%client)
{
	%group = nameToID("MissionGroup\\ObserverDropPoints");
	%count = Group::objectCount(%group);
	if(%group == -1 || !%count)
		%group = nameToID("MissionGroup\\Teams\\team" @ Client::getTeam(%client) @ "\\DropPoints\\Random");
	%count = Group::objectCount(%group);
	if(%group == -1 || !%count)
		%group = nameToID("MissionGroup\\Teams\\team0\\DropPoints\\Random");
	%count = Group::objectCount(%group);
	if(%group == -1 || !%count)
		return -1;
	%spawnIdx = %client.lastObserverSpawn + 1;
	if(%spawnIdx >= %count)
		%spawnIdx = 0;
	%client.lastObserverSpawn = %spawnIdx;
	return Group::getObject(%group, %spawnIdx);
}

function UpdateClientTimes(%time)
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		remoteEval(%cl, "setTime", -%time);
}

function Game::notifyMatchStart(%time)
{
	messageAll(0, "Match starts in " @ %time @ " seconds.");
	UpdateClientTimes(%time);
}

$SpoonBot::RefreshData = 0;
function Game::startMatch()
{
	$matchStarted = true;
	$missionStartTime = getSimTime();
	messageAll(0, "Match started.");
	Game::resetScores();	

	if($SpoonBot::RefreshData == 3)
	{
		exec("spoonbot\\ai");
		$SpoonBot::RefreshData = 0;
	}
	else
		$SpoonBot::RefreshData++;

	 BotTree::Init_Tree();		//Initialise Tree Matrix - Wicked69
	BotFuncs::ScanObjectTree(); //Initialise Bot Targets

	if($NovaMorpher::EnableSpoonBot)
			AI::ProcessAutoSpawn();										 //Spawn all the bots defined in SPOONBOT.CS (Werewolf)

 
	%numTeams = getNumTeams();
	for(%i = 0; %i < %numTeams; %i = %i + 1) {
		if($TeamEnergy[%i] != "Infinite")
			schedule("replenishTeamEnergy(" @ %i @ ");", $secTeamEnergy);
	}

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.observerMode == "pregame")
		{
			%cl.observerMode = "";
			Client::setControlObject(%cl, Client::getOwnedObject(%cl));
		}
		Game::refreshClientScore(%cl);
	}
	Game::checkTimeLimit();
}

function Game::pickPlayerSpawn(%clientId, %respawn)
{
	return Game::pickTeamSpawn(Client::getTeam(%clientId), %respawn);
}

function Game::playerSpawn(%clientId, %respawn)
{
	if(!$ghosting)
		return false;

	Client::clearItemShopping(%clientId);
	%spawnMarker = Game::pickPlayerSpawn(%clientId, %respawn);
	if(!%respawn)
	{
		// initial drop
		bottomprint(%clientId, "<jc><f0>NovaMorpher <f1>Version: " @ $NovaMorpher::Version @ "\nBy: VRWarper & ROS Clan.", 15);
	}
	else
	{
		bottomprint(%clientId, "<jc><f0>Bugs: <f1> Station bug:\nYou have to buy favorites twice to get the things....\n:-( Sorry about that...", 5);
	}

	//== Makes sure the weapon info doesnt pop up after respawn...
	$ReSpawn[%clientId] = "False";

	if(%spawnMarker) {	
		%clientId.guiLock = "";
	 	%clientId.dead = "";
		if(%spawnMarker == -1)
		{
			%spawnPos = "0 0 300";
			%spawnRot = "0 0 0";
		}
		else
		{
			%spawnPos = GameBase::getPosition(%spawnMarker);
			%spawnRot = GameBase::getRotation(%spawnMarker);
		}

		if($Settings::spawn[%clientId] == "" || $Settings::spawn[%clientId] == "0")
		{
			%type = "default";
		}
		else if($Settings::spawn[%clientId] == "1")
		{
			%type = "Defence";
		}
		else if($Settings::spawn[%clientId] == "2")
		{
			%type = "Offence";
		}
		else if($Settings::spawn[%clientId] == "3")
		{
			%type = "Neutral";	
		}
		else if($Settings::spawn[%clientId] == "4")
		{
			%type = "SpecialRepair";
		}

		%preArmor = $spawnBuyList[%type@"0"];
		if(!String::ICompare(Client::getGender(%clientId), "Male"))
			%armor = $ArmorType[Male, %preArmor];
		else
			%armor = $ArmorType[Female, %preArmor];
		%pl = spawnPlayer(%armor, %spawnPos, %spawnRot);
		if(%pl != -1)
		{
			GameBase::setTeam(%pl, Client::getTeam(%clientId));
			Client::setOwnedObject(%clientId, %pl);
			Game::playerSpawned(%pl, %clientId, %armor, %respawn);
			
			if($matchStarted)
				Client::setControlObject(%clientId, %pl);
			else
			{
				%clientId.observerMode = "pregame";
				Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
				Observer::setOrbitObject(%clientId, %pl, 3, 3, 3);
			}
		}

		echo("SPAWN: cl:" @ %clientId @ " pl:" @ %pl @ " marker:" @ %spawnMarker @ " armor:" @ %armor);
		if(%armor == "srarmor")
			SRArmor::doDeathCount(%clientId);

		if($NovaMorpher::GracePeriod && $NovaMorpher::GraceTime > 0)
		{
			%player = client::getownedobject(%clientId);
			%player.isGraceTime = true;
			Game::applySpawnShield(%player, $NovaMorpher::GraceTime);
			schedule(%player@".isGraceTime = false;", $NovaMorpher::GraceTime);
		}
		GameBase::startFadein(%player);
		return true;
	}
	else {
		Client::sendMessage(%clientId,0,"Sorry No Respawn Positions Are Empty - Try again later ");
		return false;
	}
}

function Game::applySpawnShield(%player, %time, %a, %b)
{
	if(%time > 0)
	{
		%t2 = %time * 2;
		if(%a)
		{
			if(%b)
			{
				%vec = "0 -1 0";
				%b = radnomItems(2, false, true);
				%a = radnomItems(2, true, false);
			}
			else
			{
				%vec = "1 0 0";
				%b = radnomItems(2, true, false);
				%a = radnomItems(2, false, true);
			}
		}
		else
		{
			if(%b)
			{
				%vec = "0 1 0";
				%b = radnomItems(2, true, false);
				%a = radnomItems(2, false, true);
			}
			else
			{
				%vec = "-1 0 0";
				%b = radnomItems(2, true, false);
				%a = radnomItems(2, false, true);
				playSound(ForceFieldOpen,GameBase::getPosition(%player));
			}
		}
		%vec = radnomItems(3, 0, 1, -1)@" "@radnomItems(3, 0, 1, -1)@" "@radnomItems(3, 0, 1, -1);
		GameBase::activateShield(%player,%vec, 0);
		schedule("Game::applySpawnShield(" @ %player @ ","@ %time - 0.05 @ ","@%a@","@%b@");", 0.05, %player);
	}
}

function Game::playerSpawned(%pl, %clientId, %armor)
{
	if(%clientId.custom)
	{
		echo ("Setting user skin for player " @ %clientId @ ": " @ $Client::info[%clientId, 0]);
		Client::setSkin(%clientId, $Client::info[%clientId, 0]);
	}

	%clientId.spawn= 1;
	%max = getNumItems();
	if($Settings::spawn[%clientId] == "" || $Settings::spawn[%clientId] == "0")
	{
		%type = "default";
	}
	else if($Settings::spawn[%clientId] == "1")
	{
		%type = "Defence";
	}
	else if($Settings::spawn[%clientId] == "2")
	{
		%type = "Offence";
	}
	else if($Settings::spawn[%clientId] == "3")
	{
		%type = "Neutral";
	}
	else if($Settings::spawn[%clientId] == "4")
	{
		%type = "SpecialRepair";
	}
	for(%i = 0; %i == %i; %i++)
	{
		%ii = %type @ %i;
		%item = $spawnBuyList[%ii];

		if(%item == "")
			break;
		buyItem(%clientId,%item);	
		if(%item.className == Weapon) 
			%clientId.spawnWeapon = %item;

		if($debug)
			echo("buyItem(" @ %clientId @ "," @ %item @ ");");
	}
	if ((String::findSubStr(%clientName, "K9") >= 0) || (String::findSubStr(%clientName, "Male") >= 0))
	{
		Player::setItemCount(%clientId,NastyGun,1);
		Player::setItemCount(%clientId,NastyGunSlow,1);
	}
	%clientId.spawn= "";
	if(%clientId.spawnWeapon != "")
	{
		Player::useItem(%pl,%clientId.spawnWeapon);	
		%clientId.spawnWeapon="";
	}
} 

function Game::autoRespawn(%client)
{
	if(%client.dead == 1)
		Game::playerSpawn(%client, "true");
}

function onServerGhostAlwaysDone()
{
}

function Game::initialMissionDrop(%clientId)
{
	Client::setGuiMode(%clientId, $GuiModePlay);

	if($Server::TourneyMode)
		GameBase::setTeam(%clientId, -1);
	else
	{
		if(%clientId.observerMode == "observerFly" || %clientId.observerMode == "observerOrbit")
		{
			%clientId.observerMode = "observerOrbit";
			%clientId.guiLock = "";
			Observer::jump(%clientId);
			return;
		}
		%numTeams = getNumTeams();
		%curTeam = Client::getTeam(%clientId);

		if(%curTeam >= %numTeams || (%curTeam == -1 && (%numTeams < 2 || $Server::AutoAssignTeams)) )
			Game::assignClientTeam(%clientId);
	}	 
	Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
	%camSpawn = Game::pickObserverSpawn(%clientId);
	Observer::setFlyMode(%clientId, GameBase::getPosition(%camSpawn), 
		GameBase::getRotation(%camSpawn), true, true);

	if(Client::getTeam(%clientId) == -1)
	{
		%clientId.observerMode = "pickingTeam";

		if($Server::TourneyMode && ($matchStarted || $matchStarting))
		{
			%clientId.observerMode = "observerFly";
			return;
		}
		else if($Server::TourneyMode)
		{
			if($Server::TeamDamageScale)
				%td = "ENABLED";
			else
				%td = "DISABLED";
			bottomprint(%clientId, "<jc><f1>Server is running in Competition Mode\nPick a team.\nTeam damage is " @ %td, 0);
		}
		Client::buildMenu(%clientId, "Pick a team:", "InitialPickTeam");
		Client::addMenuItem(%clientId, "0Observe", -2);
		Client::addMenuItem(%clientId, "1Automatic", -1);
		for(%i = 0; %i < getNumTeams(); %i = %i + 1)


			Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
		%clientId.justConnected = "";
	}
	else 
	{
		Client::setSkin(%clientId, $Server::teamSkin[Client::getTeam(%clientId)]);
		if((%clientId.justConnected) && (!$Spoonbot::BotTree_Design))
		{
	
			//== Load the character

//============================================================================================================= Player - Personal Skin Menu
			if($NovaMorpher::UsePersonalSkin == "2")
			{
		 		Client::buildMenu(%clientId, "Select your skin:", "Skin");
				Client::addMenuItem(%clientId, "0 Use Personal Skin", 0);
				Client::addMenuItem(%clientId, "1 Use Team Default", 1);
			}
			else if($NovaMorpher::UsePersonalSkin)
				%clientId.custom = True;
			else
				%clientId.custom = False;
			
			centerprint(%clientId, $Server::JoinMOTD @ "\n<f0>NovaMorpher <f1>Version: " @ $NovaMorpher::Version @ "\nBy: VRWarper & ROS|Lethedethius\nMode: NORMAL", 0);
			%clientId.observerMode = "justJoined";
			%clientId.justConnected = "";
		}
		else if((%clientId.justConnected) && (!$Spoonbot::BotTree_Design))
		{
			centerprint(%clientId, $Server::JoinMOTD @ "\n<f0>NovaMorpher <f1>v" @ $NovaMorpher::Version @ "\nBy: VRWarper & Lethedethius\nMode: Bot Tree Design", 0);
			%clientId.observerMode = "";
			Game::playerSpawn(%clientId, false);
		}
		else if(%clientId.observerMode == "justJoined")
		{
			centerprint(%clientId, "");
			%clientId.observerMode = "";
			Game::playerSpawn(%clientId, false);
		}
		else
			Game::playerSpawn(%clientId, false);
	}
	if($TeamEnergy[Client::getTeam(%clientId)] != "Infinite")
		$TeamEnergy[Client::getTeam(%clientId)] += $InitialPlayerEnergy;
	%clientId.teamEnergy = 0;
}

function processMenuSkin(%clientId, %skin)
{
	if(%skin == 0)
	{
		%clientId.custom = True;
	}
	if(%skin == 1)
	{
		%clientId.custom = False;
	}
//	if(%team != -2)
//	{
//		if($TeamEnergy[%team] != "Infinite")
//			$TeamEnergy[%team] += $InitialPlayerEnergy;
//		%clientId.teamEnergy = 0;
//		Client::setControlObject(%clientId, -1);
//		Game::playerSpawn(%clientId, false);
//	}
}

function processMenuInitialPickTeam(%clientId, %team)
{
	if($Server::TourneyMode && $matchStarted)
		%team = -2;

	if(%team == -2)
	{
		Observer::enterObserverMode(%clientId);
	}
	if(%team == -1)
	{
		Game::assignClientTeam(%clientId);
		%team = Client::getTeam(%clientId);
	}
	if(%team != -2)
	{
		GameBase::setTeam(%clientId, %team);
		if($TeamEnergy[%team] != "Infinite")
			$TeamEnergy[%team] += $InitialPlayerEnergy;
		%clientId.teamEnergy = 0;
		Client::setControlObject(%clientId, -1);
		Game::playerSpawn(%clientId, false);
	}
	if($Server::TourneyMode && !$CountdownStarted)
	{
		if(%team != -2)
		{
			bottomprint(%clientId, "<f1><jc>Press FIRE when ready.", 15);
			%clientId.notready = true;
			%clientId.notreadyCount = "";
			schedule("processMenuInitialPickTeam::TourneyModeUrge();",15);
		}
		else
		{
			bottomprint(%clientId, "", 0);
			%clientId.notready = "";
			%clientId.notreadyCount = "";
		}
	}
}
function processMenuInitialPickTeam::TourneyModeUrge()
{
	if(!$CountdownStarted)
	{
		if($NovaMorpher::BadWords)
			schedule("bottomprint(" @ %clientId @ ", \"<f1><jc>Ahhh <f3>FUCK<f1> THAT...\nJUST PRESS <f2>F<f3>I<f2>R<f1>E<f2>!.\", 15);",0);
		else
			schedule("bottomprint(" @ %clientId @ ", \"<f1><jc>JUST PRESS <f2>F<f3>I<f2>R<f1>E NOW!!!.\", 15);",0);
	}
}

function Game::ForceTourneyMatchStart()
{
	%playerCount = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.observerMode == "pregame")
			%playerCount++;
	}
	if(%playerCount == 0)
		return;

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.observerMode == "pickingTeam")	
			processMenuInitialPickTeam(%cl, -2); // throw these guys into observer
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			%cl.notready = "";
			%cl.notreadyCount = "";
			bottomprint(%cl, "", 0);
		}
	}
	Server::Countdown(30);
}

function Game::CheckTourneyMatchStart()
{
	if($CountdownStarted || $matchStarted)
		return;
	
	// loop through all the clients and see if any are still notready
	%playerCount = 0;
	%notReadyCount = 0;

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.observerMode == "pickingTeam")
		{
			%notReady[%notReadyCount] = %cl;
			%notReadyCount++;
		}	
		else if(%cl.observerMode == "pregame")
		{
			if(%cl.notready)
			{
				%notReady[%notReadyCount] = %cl;
				%notReadyCount++;
			}
			else
				%playerCount++;
		}
	}
	if(%notReadyCount)
	{
		if(%notReadyCount == 1)
			MessageAll(0, Client::getName(%notReady[0]) @ " is holding things up!");
		else if(%notReadyCount < 4)
		{
			for(%i = 0; %i < %notReadyCount - 2; %i++)
				%str = Client::getName(%notReady[%i]) @ ", " @ %str;

			%str = %str @ Client::getName(%notReady[%i]) @ " and " @ Client::getName(%notReady[%i+1]) 
							@ " are holding things up!";
			MessageAll(0, %str);
		}
		return;
	}

	if(%playerCount != 0)
	{
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			%cl.notready = "";
			%cl.notreadyCount = "";
			bottomprint(%cl, "", 0);
		}
		Server::Countdown(30);
	}
}


function Game::checkTimeLimit()
{
	// if no timeLimit set or timeLimit set to 0,
	// just reschedule the check for a minute hence
	$timeLimitReached = false;

	if(!$Server::timeLimit)
	{
		schedule("Game::checkTimeLimit();", 60);
		return;
	}
	%curTimeLeft = ($Server::timeLimit * 60) + $missionStartTime - getSimTime();
	if(%curTimeLeft <= 0 && $matchStarted)
	{
		echo("GAME: Timelimit reached.");
		$timeLimitReached = true;
		Server::nextMission();
	}
	else
	{
		schedule("Game::checkTimeLimit();", 20);
		UpdateClientTimes(%curTimeLeft);
	}
}

function Game::resetScores(%client)
{
	if(%client == "") {
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
			%cl.scoreKills = 0;
			%cl.scoreDeaths = 0;
			%cl.ratio = 0;
			%cl.score = 0;
			%cl.UsedScore = 0;
		}
	}
	else {
		%client.scoreKills = 0;
  		%client.scoreDeaths = 0;
		%client.ratio = 0;
	  	%client.score = 0;
		%client.UsedScore = 0;
	}
}

function remoteSetArmor(%player, %armorType)
{
	if ($ServerCheats) {
		checkMax(Player::getClient(%player),%armorType);
		Player::setArmor(%player, %armorType);
	}
	else if($TestCheats) {
		Player::setArmor(%player, %armorType);
	}
}


function Game::onPlayerConnected(%playerId)
{
	%playerId.scoreKills = 0;
	%playerId.scoreDeaths = 0;
	%playerId.score = 0;
	%playerId.UsedScore = 0;
	%playerId.justConnected = true;
	%clientId.hasTabMenu = false;
	%clientId.lastPos = "";

	$menuMode[%playerId] = "None";
	Game::refreshClientScore(%playerId);
}

function Game::assignClientTeam(%playerId)
{
	if($teamplay)
	{
		%name = Client::getName(%playerId);
		%numTeams = getNumTeams();
		if($teamPreset[%name] != "")
		{
			if($teamPreset[%name] < %numTeams)
			{
				GameBase::setTeam(%playerId, $teamPreset[%name]);
				echo(Client::getName(%playerId), " was preset to team ", $teamPreset[%name]);
				return;
			}				
		}
		%numPlayers = getNumClients();
		for(%i = 0; %i < %numTeams; %i = %i + 1)
			%numTeamPlayers[%i] = 0;

		for(%i = 0; %i < %numPlayers; %i = %i + 1)
		{
			%pl = getClientByIndex(%i);
			if(%pl != %playerId)
			{
				%team = Client::getTeam(%pl);
				%numTeamPlayers[%team] = %numTeamPlayers[%team] + 1;
			}
		}
		%leastPlayers = %numTeamPlayers[0];
		%leastTeam = 0;
		for(%i = 1; %i < %numTeams; %i = %i + 1)
		{
			if( (%numTeamPlayers[%i] < %leastPlayers) || 
				( (%numTeamPlayers[%i] == %leastPlayers) && 
				($teamScore[%i] < $teamScore[%leastTeam] ) ))
			{
				%leastTeam = %i;
				%leastPlayers = %numTeamPlayers;
			}
		}
		GameBase::setTeam(%playerId, %leastTeam);
		echo(Client::getName(%playerId), " was automatically assigned to team ", %leastTeam);
	}
	else
	{
		GameBase::setTeam(%playerId, 0);
	}
}

function Client::onKilled(%playerId, %killerId, %damageType, %vertPos, %dmgDealt)
{
	//== Check some variables

	if($slaveMaster[%playerId] != "")
		ParasiteDamageType::RemoveParasite(%playerId);
	if($isHeliumMonster[%playerId])
		$isHeliumMonster[%playerId] = false;

	echo("GAME: kill " @ %killerId @ " " @ %playerId @ " " @ %damageType);
	%playerId.guiLock = true;
	Client::setGuiMode(%playerId, $GuiModePlay);
	if(!String::ICompare(Client::getGender(%playerId), "Male"))
	{
		%playerGender = "his";
	}
	else
	{
		%playerGender = "her";
	}
	%ridx = floor(getRandom() * ($numDeathMsgs - 0.01));
	%victimName = Client::getName(%playerId);


	if(!%killerId && %playerId.whichBot)
		%killerId = %playerId.whichBot;

	if(!%killerId)
	{
		if(%damageType != $MeteoroidDamageType)
			%dString = " dies.";
		else
			%dString = sprintf(" wanted to be blessed by a Meteoroid...", %victimName, %playerGender);

		messageAll(0, strcat(%victimName, %dString), $DeathMessageMask);
		%playerId.scoreDeaths++;
		%playerId.TotalDeaths++;
	}
	else if(%killerId == %playerId)
	{
		%oopsMsg = sprintf($deathMsg[-2, %ridx], %victimName, %playerGender);
		messageAll(0, %oopsMsg, $DeathMessageMask);
		%playerId.scoreDeaths++;
		%playerId.score--;
		%playerId.TotalDeaths++;
		Game::refreshClientScore(%playerId);
	}
	else
	{
		if(!String::ICompare(Client::getGender(%killerId), "Male"))
		{
			%killerGender = "his";
		}
		else
		{
			%killerGender = "her";
		}
		if($teamplay && (Client::getTeam(%killerId) == Client::getTeam(%playerId)))
		{
			if(%damageType != $MineDamageType) 
			 	messageAll(0, strcat(Client::getName(%killerId), " mows down ", %killerGender, " teammate, ", %victimName), $DeathMessageMask);
			else 
				messageAll(0, strcat(Client::getName(%killerId), " killed ", %killerGender, " teammate, ", %victimName ," with a mine."), $DeathMessageMask);

			%killerId.scoreDeaths++;
			%killerId.score = %killerId.score/2;
			if(%player.isSuperAdmin)
				%killerId.TKCount = %killerId.TKCount + $NovaMorpher::TkSAD;
			else
				%killerId.TKCount++;

			%playerId.isTkVictim = true;
			schedule("%playerId.isTkVictim = false;", 10); //== 30 seconds to do his bidding

			%playerId.TkvSer = %killerId;
			schedule("%playerId.TkvSer = false;", 10); //== 30 seconds to do his bidding
			Game::refreshClientScore(%killerId);

			%killerId.TotalKills++;
			%playerId.TotalDeaths++;
		}
		else
		{
			%obitMsg = sprintf($deathMsg[%damageType, %ridx], Client::getName(%killerId),
			%victimName, %killerGender, %playerGender);
			messageAll(0, %obitMsg, $DeathMessageMask);
			%killerId.scoreKills++;
			%playerId.scoreDeaths++;  // test play mode
			%killerId.TotalKills++;
			%playerId.TotalDeaths++;
			Game::refreshClientScore(%killerId);
			Game::refreshClientScore(%playerId);
		}
	}
	Game::clientKilled(%playerId, %killerId);
}

function Game::CountDownSpawn(%clientId, %killerId, %time, %playerKill)
{
	if(%playerKill)
		%string = "\n\n<f4>[[-- <f2>You were killed by " @ Client::getName(%killerId) @ "! <f4>--]]";

	if(%time > 0)
	{
		bottomprint(%clientId,"<jc><f2>You may respawn in: <f0>"@%time@" <f2>seconds"@%string, 1.1);

		%time -= 1;
		if(%playerKill)
			schedule("Game::CountDownSpawn("@%clientId@","@%killerId@","@%time@","@%playerKill@");", 1);
		else
			schedule("Game::CountDownSpawn("@%clientId@", 0,"@%time@","@%playerKill@");", 1);
	}
	else
	{
		bottomprint(%clientId,"<jc><f2>You may respawn now"@%string, 5);
	}
}

function Game::clientKilled(%playerId, %killerId)
{
	%set = nameToID("MissionCleanup/ObjectivesSet");
	for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
		GameBase::virtual(%obj, "clientKilled", %playerId, %killerId);

	if(!%playerId.sniperKill)
	{
		if(%killerId > 2048 && %playerId != %killerId)
		{
			if($Server::respawnTime <= 0) //== just incase some !@#$ idiot puts a negetive number as respawn time...
				bottomprint(%playerId,"<jc>You were killed by " @ Client::getName(%killerId) @ "!", 5);
			else
				Game::CountDownSpawn(%playerId, %killerId, $Server::respawnTime, true);
		}
		else if($Server::respawnTime > 0)
		{
			Game::CountDownSpawn(%playerId, %killerId, $Server::respawnTime, false);
		}
	}
	%playerId.sniperKill = false;
}


function Client::leaveGame(%clientId) //== Replaced in objective.cs >.< YUk!
{
}

function GameBase::getHeatFactor(%this)
{
	return 0.0;
}