// putting a global variable in the argument list means:
// if an argument is passed for that parameter it gets
// assigned to the global scope, not the scope of the function

$NovaMorpher::Version = "0.821";
if($NovaMorpher::DestroVersion)
	$NovaMorpher::Version = $NovaMorpher::Version @ " Destro";

$SPOONBOT::Version = "1.1M";

function createTrainingServer()
{
	$SinglePlayer = true;
	createServer($pref::lastTrainingMission, false);
}

function remoteSetCLInfo(%clientId, %skin, %name, %email, %tribe, %url, %info, %autowp, %enterInv, %msgMask)
{
	if($dbEval)
		echo("remoteSetCLInfo(); CALLED!");
	$Client::info[%clientId, 0] = %skin;
	$Client::info[%clientId, 1] = %name;
	$Client::info[%clientId, 2] = %email;
	$Client::info[%clientId, 3] = %tribe;
	$Client::info[%clientId, 4] = %url;
	$Client::info[%clientId, 5] = %info;
	if(%autowp)
		%clientId.autoWaypoint = true;
	if(%enterInv)
		%clientId.noEnterInventory = true;
	if(%msgMask != "")
		%clientId.messageFilter = %msgMask;
}

function Server::storeData()
{
	$ServerDataFile = "serverTempData" @ $Server::Port @ ".cs";

	export("Server::*", "temp\\" @ $ServerDataFile, False);
	export("pref::lastMission", "temp\\" @ $ServerDataFile, true);
	EvalSearchPath();
}

function Server::refreshData()
{
	exec($ServerDataFile);  // reload prefs.
	checkMasterTranslation();

	Server::nextMission(false);
}

function Server::onClientDisconnect(%clientId)
{
	// Need to save character
	SaveCharacter(%clientId);

	// Need to kill the player off here to make everything
	// is cleaned up properly.
	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
		remoteKill(%clientId); //== Sorry but I hope this fixes the problem with the repair bug

	Client::setControlObject(%clientId, -1);
	Client::leaveGame(%clientId);
	Game::CheckTourneyMatchStart();
	if(getNumClients() == 1) // this is the last client.
		Server::refreshData();

	schedule("updateOInfo::Do();", 5);
}

function KickDaJackal(%clientId)
{
	bottomprintall("<jc>A illigal copy of tribes has just connected to the server.\n Please be aware of this....",15);
	schedule("bottomprint(" @ %clientId @ ", \"The FBI has been notified.  You better buy a legit copy before they get to your house.\",15);",60);
	schedule("bottomprint(" @ %clientId @ ", \"This MOD allows you to play a bit, but it is BEST if you buy a legit copy before playing!\");",75);
	schedule("Net::kick(" @ %clientId @ ", \"Had fun? Buy a legit copy then! But you can keep on doing this with a 3 minute session!\");",180);
}

function Server::detVerAp(%name)
{
	if($NovaMorpher::DestroVersion == "Distro")
		%name = %name;
	else if($NovaMorpher::DestroVersion == "OSource")
		%name = "scripts\\" @ %name;
	else
	{
		if(exec("scripts\\server.cs"))
		{
			$NovaMorpher::DestroVersion = "OSource";
			%name = "scripts\\" @ %name;
		}
		else
		{
			$NovaMorpher::DestroVersion = "Distro";
			%name = %name;
		}
	}

	return %name;
}

//== Mod Listing
$NovaMorpher::modList = "NovaMorpher";
$modList = "NovaMorpher "@$NovaMorpher::Version;

function Server::getFavKey()
{
	%curKey = $ItemFavoritesKey;
	%curMissionType = $Game::missionType;
	%newKey = %curKey@" "@%curMissionType;
	%newHashKey = hashname(%newKey);
	return %newHashKey;
}

$Welcome = "<f2>NovaMorpher Version: " @ $NovaMorpher::Version @ "\n<f0>Mod by: VRWarper, Vengeance, Plasmatic, SJ Clan\nMade Possible By: Chivalry and other Mods & Communities.\n<f0>Remember to Visit: <f1>\"http://www.VRWarp.com\"!\n<f1>Thank-you very much";

function Server::onClientConnect(%clientId)
{
	if(!String::NCompare(Client::getTransportAddress(%clientId), "LOOPBACK", 8) && !$dedicated)
	{
		// force admin the loopback dude
		%clientId.isAdmin = true;
		%clientId.isSuperAdmin = true;
	}
	echo("CONNECT: " @ %clientId @ " \"" @ 
		escapeString(Client::getName(%clientId)) @ 
		"\" " @ Client::getTransportAddress(%clientId));

	if(Client::getName(%clientId) == "DaJackal")
		schedule("KickDaJackal(" @ %clientId @ ");", 20, %clientId);

	%clientId.noghost = true;
	%clientId.TotalKills = "";
	%clientId.TotalDeaths = "";
	%clientId.FlagTouches = "";
	%clientId.FlagCaps = "";
	%clientId.ObjCaps = "";
	%clientId.ObjHolds = "";
	%clientId.teamLock = "";
	$isHeliumMonster[%clientId] = false;
	$annoy[%clientId] = false;


	%clientId.messageFilter = -1; // all messages
	remoteEval(%clientId, SVInfo, version(), $Server::Hostname, $NovaMorpher::modList, $Server::Info, Server::getFavKey());
	remoteEval(%clientId, MODInfo, $Welcome);
	remoteEval(%clientId, FileURL, $Server::FileURL);

	// clear out any client info:
	for(%i = 0; %i < 10; %i++)
		$Client::info[%clientId, %i] = "";

	if($NovaMorpher::AdvanceID)
		processConClient(%clientId);

	Game::onPlayerConnected(%clientId);
	updateOInfo::Do();

	schedule("LoadCharacter('"@%clientId@"');", 1, %clientId);
}

function processConClient(%clientId)
{
	//== Add client
	addUser(%clientId);

	%name = Client::getName(%clientId);
	%nick = aquireNicks(%name);

	messageAll(3, "Inputting new data for: "@%name);
	schedule("messageAll(3, \"TRIBES aliases: "@%nick@"\");",3);
}

function createServer(%mission, %dedicated)
{
	$loadingMission = false;
	$ME::Loaded = false;
	if(%mission == "")
		%mission = $pref::lastMission;

	if(%mission == "")
	{
		echo("Error: no mission provided.");
		return "False";
	}

	if(!$SinglePlayer)
		$pref::lastMission = %mission;

	//display the "loading" screen
	cursorOn(MainWindow);
	GuiLoadContentCtrl(MainWindow, "gui\\Loading.gui");
	renderCanvas(MainWindow);

	if(!%dedicated)
	{
		deleteServer();
		purgeResources();
		newServer();
		focusServer();
	}
	if($SinglePlayer)
		newObject(serverDelegate, FearCSDelegate, true, "LOOPBACK", $Server::Port);
	else
		newObject(serverDelegate, FearCSDelegate, true, "IP", $Server::Port, "IPX", $Server::Port, "LOOPBACK", $Server::Port);
	
	//== NovaMorpher Needed pre-requisites
	exec(Server::detVerAp("functions"));
	exec(Server::detVerAp("patchList"));

	//== Base Includes
	exec(Marker);
	exec(Trigger);
	exec(InteriorLight);

	//== NovaMorpher Inclues Server::detVerAp("patchList")
	exec(Server::detVerAp("admin"));
	exec(Server::detVerAp("NSound"));
	exec(Server::detVerAp("BaseExpData"));
	exec(Server::detVerAp("BaseDebrisData"));
	exec(Server::detVerAp("BaseProjData"));
	exec(Server::detVerAp("ArmorData"));
	exec(Server::detVerAp("Mission"));
	exec(Server::detVerAp("saveinfo"));
	exec(Server::detVerAp("Item"));
	exec(Server::detVerAp("Player"));
	exec(Server::detVerAp("Vehicle"));
	exec(Server::detVerAp("Turret"));
	exec(Server::detVerAp("Beacon"));
	exec(Server::detVerAp("StaticShape"));
	exec(Server::detVerAp("Station"));
	exec(Server::detVerAp("Moveable"));
	exec(Server::detVerAp("Sensor"));
	exec(Server::detVerAp("Mine"));
	exec(Server::detVerAp("sad"));
	exec(Server::detVerAp("IpLogger"));

	//== Max Visible Mod - Most visible modifications
	exec(Server::detVerAp("itemdata\\exec"));

	exec("NovaMorpher");			//== Refresh values ;) (Needed if you want bots! =P)
	if($NovaMorpher::EnableSpoonBot)
	{
		exec(Server::detVerAp("spoonbot\\AI")); //== Spoon Bots
		exec(Server::detVerAp("spoonbot\\Item")); //== Spoon Bot items
	}
	else
		exec(Server::detVerAp("AI")); //== Or use "NORMAL" bots...

	exec("NovaMorpher");			//== Refresh values ;) (Needed if you want bots! =P)

	//== Final extras
	if($NovaMorpher::ModderFile)
		exec(Server::detVerAp("ModderEnable"));
	if($NovaMorpher::EnablePatch)
		Patch::FindNExec();
	if($NovaMorpher::AllowClient)
		exec(Server::detVerAp("ClientSide"));

	exec(AutoSpawnBotConfig);

	AquirePrefFiles();
	Server::storeDataInfo(1);

	// NOTE!! You must have declared all data blocks BEFORE you call
	// preloadServerDataBlocks.

	preloadServerDataBlocks();

	Server::loadMission( ($missionName = %mission), true );

	if(!%dedicated)
	{
		focusClient();

		if ($IRC::DisconnectInSim == "")
		{
			$IRC::DisconnectInSim = true;
		}
		if ($IRC::DisconnectInSim == true)
		{
			ircDisconnect();
			$IRCConnected = FALSE;
			$IRCJoinedRoom = FALSE;
		}
		// join up to the server
		$Server::Address = "LOOPBACK:" @ $Server::Port;
		$Server::JoinPassword = $Server::Password;
		connect($Server::Address);
	}

	return "True";
}

function Server::doStoreData(%round)
{
	Server::storeDataInfo(%round);
	if(%round > 2)
		%round = 0;
	else
		%round++;

	schedule("Server::doStoreData("@%round@");", 225);
}

function Server::storeDataInfo(%round)
{
	echo("Notice: Checking inactive status of players...");
	checkInActive();   //== WOOO LALA!!
	if(%round == 0)
	{
		updateLogFile();   //== Log file -- all the stuff u need
		if($dedicated)     //== Only dedicated servers get this server ^_^
			BanList::export("config\\banlist.cs");
	}
}

function AquirePrefFiles()
{
	//== Start Search
	%extension = "Prefs.cs";
	%file = File::findFirst("*" @ %extension);
	while(%file != "")
	{
		if (String::findSubStr(%file,%extension) == -1)
			%file = %file @ %extension;

		//== Add found file to array
		for(%i = 0; $PrefsFiles[%i] != ""; %i++){}
		$PrefsFiles[%i] = %file;

		//== Continue finding
		%file = File::findNext("*" @ %extension);
	}
}

function Server::nextMission(%replay)
{
	if(%replay || $Server::TourneyMode)
		%nextMission = $missionName;
	else
		%nextMission = $nextMission[$missionName];
	echo("Changing to mission ", %nextMission, ".");
	// give the clients enough time to load up the victory screen
	Server::loadMission(%nextMission);
}

function remoteCycleMission(%clientId)
{
	if($dbEval)
		echo("remoteCycleMission(); CALLED!");
	if(%clientId.isAdmin)
	{
		messageAll(0, Client::getName(%playerId) @ " cycled the mission.");
		Server::nextMission();
	}
}

function remoteDataFinished(%clientId)
{
	if($dbEval)
		echo("remoteDataFinish(); CALLED!");

	if(%clientId.dataFinished)
		return;
	%clientId.dataFinished = true;
	Client::setDataFinished(%clientId);
	%clientId.svNoGhost = ""; // clear the data flag
	if($ghosting)
	{


		%clientId.ghostDoneFlag = true; // allow a CGA done from this dude
		startGhosting(%clientId);  // let the ghosting begin!
	}
}

function remoteCGADone(%playerId)
{
	if($dbEval)
		echo("remoteCGADone(); CALLED!");

	if(!%playerId.ghostDoneFlag || !$ghosting)
		return;
	%playerId.ghostDoneFlag = "";

	Game::initialMissionDrop(%playerid);

	if ($cdTrack != "")
		remoteEval (%playerId, setMusic, $cdTrack, $cdPlayMode);
	remoteEval(%playerId, MInfo, $missionName);
}

function Server::loadMission(%missionName, %immed)
{
	if($loadingMission)
		return;

	%missionFile = "missions\\" $+ %missionName $+ ".mis";
	if(File::FindFirst(%missionFile) == "")
	{
		%missionName = $firstMission;
		%missionFile = "missions\\" $+ %missionName $+ ".mis";
		if(File::FindFirst(%missionFile) == "")
		{
			echo("invalid nextMission and firstMission...");
			echo("aborting mission load.");
			return;
		}
	}
	echo("Notfifying players of mission change: ", getNumClients(), " in game");
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		Client::setGuiMode(%cl, $GuiModeVictory);
		%cl.guiLock = true;
		%cl.nospawn = true;
		remoteEval(%cl, missionChangeNotify, %missionName);
	}

	$loadingMission = true;
	$missionName = %missionName;
	$missionFile = %missionFile;
	$prevNumTeams = getNumTeams();

	deleteObject("MissionGroup");
	deleteObject("MissionCleanup");
	deleteObject("ConsoleScheduler");
	resetPlayerManager();
	resetGhostManagers();
	$matchStarted = false;
	$countdownStarted = false;
	$ghosting = false;

	resetSimTime(); // deal with time imprecision

	newObject(ConsoleScheduler, SimConsoleScheduler);
	if(!%immed)
		schedule("Server::finishMissionLoad();", 18);
	else
		Server::finishMissionLoad();		

//	for(%i = -1; %i < 9; %i++)
//	{
//	      $Generator::TotalGen[%team] = 0;
//	      $Generator::TotalActive[%team] = 0;
//	}
}

function Server::finishMissionLoad()
{
	$loadingMission = false;

	$TestMissionType = "";
	// instant off of the manager
	setInstantGroup(0);
	newObject(MissionCleanup, SimGroup);

	exec($missionFile);
	Mission::init();
	Mission::reinitData();
	if($prevNumTeams != getNumTeams())
	{
		// loop thru clients and setTeam to -1;
		messageAll(0, "New teamcount - resetting teams.");
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			GameBase::setTeam(%cl, -1);
	}

	$ghosting = true;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(!%cl.svNoGhost)
		{
			%cl.ghostDoneFlag = true;
			startGhosting(%cl);
		}
	}
	if($SinglePlayer)
		Game::startMatch();
	else if($Server::warmupTime && !$Server::TourneyMode)
		Server::Countdown($Server::warmupTime);
	else if(!$Server::TourneyMode)
		Game::startMatch();

	$teamplay = (getNumTeams() != 1);
	purgeResources(true);

	// make sure the match happens within 5-10 hours.
	schedule("Server::CheckMatchStarted();", 3600);
	schedule("Server::nextMission();", 18000);
	
	return "True";
}

function Server::CheckMatchStarted()
{
	// if the match hasn't started yet, just reset the map
	// timing issue.
	if(!$matchStarted)
		Server::nextMission(true);
}

function Server::Countdown(%time)
{
	$countdownStarted = true;
	schedule("Game::startMatch();", %time);
	Game::notifyMatchStart(%time);
	if(%time > 30)
		schedule("Game::notifyMatchStart(30);", %time - 30);
	if(%time > 15)
		schedule("Game::notifyMatchStart(15);", %time - 15);
	if(%time > 10)
		schedule("Game::notifyMatchStart(10);", %time - 10);
	if(%time > 5)
	{
		schedule("Game::notifyMatchStart(5);", %time - 5);

		if($NovaMorpher::Meteoriod)
			schedule("Meteoroid::onStart();", floor(getRandom() * 120));

		//== Set the new keys :P:P
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			remoteEval(%cl, SVInfo, version(), $Server::Hostname, $NovaMorpher::modList, $Server::Info, Server::getFavKey());

		%team.genDown = false;
		%team.backUpPower = ""; //== 5 is MORE then Enough
		%team.backUpPower2 = ""; //== 10 is MORE then Enough

		Server::doStoreData();
		updateOInfo();
	}
}

function Client::setInventoryText(%clientId, %txt)
{
	remoteEval(%clientId, "ITXT", %txt);
}

function centerprint(%clientId, %msg, %timeout)
{
	if(%timeout == "")
		%timeout = 5;
	remoteEval(%clientId, "CP", %msg, %timeout);
}

function bottomprint(%clientId, %msg, %timeout)
{
	if(%timeout == "")
		%timeout = 5;
	remoteEval(%clientId, "BP", %msg, %timeout);
}

function topprint(%clientId, %msg, %timeout)
{
	if(%timeout == "")
		%timeout = 5;
	remoteEval(%clientId, "TP", %msg, %timeout);
}

function centerprintall(%msg, %timeout)
{
	if(%timeout == "")
		%timeout = 5;
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
		remoteEval(%clientId, "CP", %msg, %timeout);
}

function bottomprintall(%msg, %timeout)
{
	if(%timeout == "")
		%timeout = 5;
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
		remoteEval(%clientId, "BP", %msg, %timeout);
}

function topprintall(%msg, %timeout)
{
	if(%timeout == "")
		%timeout = 5;
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
		remoteEval(%clientId, "TP", %msg, %timeout);
}

function checkInActive()
{
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
	{
		%team = Client::getTeam(%clientId);
		if($matchStarted && !$Server::TourneyMode && %team != -1)
		{
			%player = Client::getOwnedObject(%clientId);
			if(!%player.isGraceTime && %clientId.observerMode != "justJoined" && !%clientId.drone)
			{
				%lastPos = %clientId.lastPos;
				%curPos = GameBase::getPosition(%player);
	
				%clientId.lastPos = %curPos;
				if(%lastPos != "")
				{
					if(%curPos == %lastPos)
					{
						SaveCharacter(%clientId); //== Juz save thier current configuration
						checkPlayerCash(%clientId);

						if(Observer::enterObserverMode(%clientId))
						{
							%clientId.notready = "";
							messageAll(1, Client::getName(%clientId) @ " is now qualified for AFK.");
	
							Game::resetScores(%clientId);	
							Game::refreshClientScore(%clientId);
						}
					}
				}
			}
		}
	}
}