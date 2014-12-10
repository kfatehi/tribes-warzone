exec(objectives);
exec(JailRecords);

if($server::jailskin[0] == "")
	$server::jailskin[0] = "purple";
if($server::jailskin[1] == "")
	$server::jailskin[1] = "green";
if($server::jailskin[2] == "")
	$server::jailskin[2] = "orange";
if($server::jailskin[3] == "")
	$server::jailskin[3] = "blue";

if($server::teamjaildamage == "")
	$server::teamjaildamage = 0;



//---------------------------------------------------------------------------------
// Player death messages - %1 = killer's name, %2 = victim's name
//       %3 = killer's gender pronoun (his/her), %4 = victim's gender pronoun
//---------------------------------------------------------------------------------

$deathMsg[$BlasterDamageType, 0]     = "%2 gets knocked out by %1's blaster and wakes up in prison.";
$deathMsg[$BlasterDamageType, 1]     = "%2 get a real blast out of meeting %1 and the long ride to prison that follows.";
$deathMsg[$BlasterDamageType, 2]     = "%1 holds %2 up with %3 blaster and says 'Go directly to jail.  Do not pass Go.  Do not collect $200'.";
$deathMsg[$BlasterDamageType, 3]     = "%1's master blaster puts an end to %2's days on the run.";
$deathMsg[$ElectricityDamageType, 0] = "%2 gets tazed by %1's ELF gun and winds up in prison.";
$deathMsg[$ElectricityDamageType, 1] = "%2 gets a nasty jolt out of learning that %1 represents the long arm of the law.";
$deathMsg[$ElectricityDamageType, 2] = "%1's ELF gun acts as a nice warmup for %2's trip to the chair.";
$deathMsg[$ElectricityDamageType, 3] = "%1 shows %2 just how electrifying justice can be.";

// "you just killed yourself" messages
//   %1 = player name,  %2 = player gender pronoun

$deathMsg[-2,0]						 = "%1 gets sent away for a long time.";
$deathMsg[-2,1]						 = "%1 confesses to %2 own murder and is sentenced to life without parole.";
$deathMsg[-2,2]						 = "%1 has proven to be a danger to %2 own self and to others...well, %2 own self anyway.";
$deathMsg[-2,3]						 = "%1 decides to see what prison life is like.";


function Mission::init()
{
   
   %numPlayers = getnumclients();
	
	for(%i = 0; %i < %numPlayers; %i = %i + 1)
	{
		%pl = getclientbyindex(%i);
		%pl.captures = 0;
		%pl.timescaptured = 0;
		%pl.murders = 0;
		%pl.escapes = 0;
		%pl.inJail = false;
		%pl.totaltimeserved = 0;
		%pl.term = 0;
		%pl.sentence = 0;
		%pl.jailteam = "";

	}
    
    
	for(%team = 0; %team < getNumTeams(); %team = %team + 1)	
	{
		$InOurJail[%team] = 0;
		for(%i = 0; %i < getNumTeams(); %i = %i + 1)	
		{
   			if(%i == %team)
   				continue;
   			$InTheirJail[%i, %team] = 0;
   			
   		}
   		
   		
   	}	
   
   setClientScoreHeading("Player Name\t\x6FTeam\t\xA6Score\t\xCFPing\t\xEFPL");
//   setClientScoreHeading("Player Name\t\x6FTeam\t\xD6Score");//\t\xFFPing\t\xFFPL");
   setTeamScoreHeading("Team Name\t\xD6Score");

   $firstTeamLine = 7;
   $firstObjectiveLine = $firstTeamLine + getNumTeams() + 1;
   for(%i = -1; %i < getNumTeams(); %i++)
   {
      $teamFlagStand[%i] = "";
		$teamFlag[%i] = "";
      Team::setObjective(%i, $firstTeamLine - 1, " ");
      Team::setObjective(%i, $firstObjectiveLine - 1, " ");
      Team::setObjective(%i, $firstObjectiveLine, "<f5>Mission Objectives: ");
      $firstObjectiveLine++;
      
		$deltaTeamScore[%i] = 0;
      $teamScore[%i] = 0;
      
      
      newObject("TeamDrops" @ %i, SimSet);
      addToSet(MissionCleanup, "TeamDrops" @ %i);
      %dropSet = nameToID("MissionGroup/Teams/Team" @ %i @ "/DropPoints/Random");
      for(%j = 0; (%dropPoint = Group::getObject(%dropSet, %j)) != -1; %j++)
         addToSet("MissionCleanup/TeamDrops" @ %i, %dropPoint);
   }
   
   $firstJailLine = $firstObjectiveLine;
   $firstObjectiveLine = $firstObjectiveLine + getnumteams();
   UpdateJailObjectives();
   
   
   Team::setObjective(%i, $firstObjectiveLine, "<f5>Mission Objectives: ");
   $numObjectives = 0;
   newObject(ObjectivesSet, SimSet);
   addToSet(MissionCleanup, ObjectivesSet);
   
   Group::iterateRecursive(MissionGroup, ObjectiveMission::initCheck);
   %group = nameToID("MissionCleanup/ObjectivesSet");

	ObjectiveMission::setObjectiveHeading();
   for(%i = 0; (%obj = Group::getObject(%group, %i)) != -1; %i++)
   {
      %obj.objectiveLine = %i + $firstObjectiveLine;
      ObjectiveMission::objectiveChanged(%obj);
   }
   
   $firstPlayerLine = $firstObjectiveLine + %i + 1;
   
   UpdateLeaderBoard();
   
   ObjectiveMission::refreshTeamScores();
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      %cl.score = 0;
      Game::refreshClientScore(%cl);
   }
   schedule("ObjectiveMission::checkPoints();", 5);
   schedule("CheckJailSentence();", 5);

	if($TestMissionType == "") {
		if($NumTowerSwitchs) 
			$TestMissionType = "C&H";
		else 
			$TestMissionType = "NONE";		
		$NumTowerSwitchs = "";
	}
   AI::setupAI();
}

function UpdateLeaderBoard()
{
	%infractions = 0;
	%infractionName = "None";
	%escapes = 0;
	%escapeName = "None";
	%captures = 0;
	%captureName = "None";
	%served = 0;
	%servedName = "None";
	
	%numPlayers = getnumclients();
	
	for(%i = 0; %i < %numPlayers; %i = %i + 1)
	{
		%pl = getclientbyindex(%i);
		
		
		if(%pl.captures > %captures)
		{
			%captures = %pl.captures;
			%captureName = client::getname(%pl);
		}
		else if(%pl.captures == %captures && %pl.captures > 0)
			%captureName = %captureName @ " & " @ client::getname(%pl);
			
		if(%pl.escapes > %escapes)
		{
			%escapes = %pl.escapes;
			%escapeName = client::getname(%pl);
		}
		else if(%pl.escapes == %escapes && %pl.escapes > 0)
			%escapeName = %escapeName @ " & " @ client::getname(%pl);
			
		if(%pl.murders > %infractions)
		{
			%infractions = %pl.murders;
			%infractionName = client::getname(%pl);
		}
		else if(%pl.infractions == %infractions && %pl.murders > 0)
			%infractionName = %infractionName @ " & " @ client::getname(%pl);
			
		if(%pl.totaltimeserved > %served)
		{
			%served = %pl.totaltimeserved;
			%servedName = client::getname(%pl);
		}
		else if(%pl.totaltimeserved == %served && %pl.totaltimeserved > 0)
			%servedName = %servedName @ " & " @ client::getname(%pl);	
			
	}
	
	
	if(%infractions == 0)
		%infractionsUpdate = "None";
	else
		%infractionsUpdate = %infractionName @ " with " @ %infractions @ " infractions.";
		
	if(%captures == 0)
		%capturesUpdate = "None";
	else
		%capturesUpdate = %captureName @ " with " @ %captures @ " fugitives captured.";
		
	if(%escapes == 0)
		%escapesUpdate = "None";
	else
		%escapesUpdate = %escapeName @ " with " @ %escapes @ " escapes from prison.";
		
	if(%served == 0)
		%servedUpdate = "None";
	else
		%servedUpdate = %servedName @ " with " @ getMinSecs(%served) @ " spent in prison.";

	for(%i = 0; %i < getNumTeams(); %i = %i + 1)	
	{
		Team::setObjective(%i, $firstPlayerLine, "<f5>Most Wanted");
		Team::setObjective(%i, $firstPlayerLine + 1, "<f1>Current Mission: " @ %infractionsUpdate);
		if($recInf[$missionName] > 0)
			Team::setObjective(%i, $firstPlayerLine + 2, "<f1>Record: " @ $recInfName[$missionName] @ " with " @ $recInf[$missionName] @ " infractions.");
		else
			Team::setObjective(%i, $firstPlayerLine + 2, "<f1>Record: None");
		Team::setObjective(%i, $firstPlayerLine + 3, "<f1>");
		Team::setObjective(%i, $firstPlayerLine + 4, "<f5>Most Captures");
		Team::setObjective(%i, $firstPlayerLine + 5, "<f1>Current Mission: " @ %capturesUpdate);
		if($recCap[$missionName] > 0)
			Team::setObjective(%i, $firstPlayerLine + 6, "<f1>Record: " @ $recCapName[$missionName] @ " with " @ $recCap[$missionName] @ " fugitives captured.");
		else
			Team::setObjective(%i, $firstPlayerLine + 6, "<f1>Record: None");
		Team::setObjective(%i, $firstPlayerLine + 7, "<f1>");
		Team::setObjective(%i, $firstPlayerLine + 8, "<f5>Most Escapes");
		Team::setObjective(%i, $firstPlayerLine + 9, "<f1>Current Mission: " @ %escapesUpdate);
		if($recEsc[$missionName] > 0)
			Team::setObjective(%i, $firstPlayerLine + 10, "<f1>Record: " @ $recEscName[$missionName] @ " with " @ $recEsc[$missionName] @ " escapes from prison.");
		else
			Team::setObjective(%i, $firstPlayerLine + 10, "<f1>Record: None");

		Team::setObjective(%i, $firstPlayerLine + 11, "<f1>");
		Team::setObjective(%i, $firstPlayerLine + 12, "<f5>Most Time Served");
		Team::setObjective(%i, $firstPlayerLine + 13, "<f1>Current Mission: " @ %servedUpdate);
		if($recSrv[$missionName] > 0)
			Team::setObjective(%i, $firstPlayerLine + 14, "<f1>Record: " @ $recSrvName[$missionName] @ " with " @ getminsecs($recSrv[$missionName]) @ " spent in prison.");
		else
			Team::setObjective(%i, $firstPlayerLine + 14, "<f1>Record: None");

		
		
		
	}
}

function UpdateLeaderRecords()
{
	
	%numPlayers = getnumclients();
	
	for(%i = 0; %i < %numPlayers; %i = %i + 1)
	{
		%pl = getclientbyindex(%i);
		
		
		if(%pl.captures > $recCap[$missionName] && %pl.captures > 0)
		{
			$recCap[$missionName] = %pl.captures;
			$recCapName[$missionName] = client::getname(%pl);
		}
		else if(%pl.captures == $recCap[$missionName] && %pl.captures > 0 && $recCapName[$missionName] != client::getname(%pl))
			$recCapName[$missionName] = $recCapName[$missionName] @ " & " @ client::getname(%pl);
			
		if(%pl.escapes > $recEsc[$missionName] && %pl.escapes > 0)
		{
			$recEsc[$missionName] = %pl.escapes;
			$recEscName[$missionName] = client::getname(%pl);
		}
		else if(%pl.escapes == $recEsc[$missionName] && %pl.escapes > 0 && $recEscName[$missionName] != client::getname(%pl))
			$recEscName[$missionName] = $recEscName[$missionName] @ " & " @ client::getname(%pl);
			
		if(%pl.murders > $recInf[$missionName] && %pl.murders > 0)
		{
			$recInf[$missionName] = %pl.murders;
			$recInfName[$missionName] = client::getname(%pl);
		}
		else if(%pl.infractions == $recInf[$missionName] && %pl.murders > 0 && $recInfName[$missionName] != client::getname(%pl))
			$recInfName[$missionName] = $recInfName[$missionName] @ " & " @ client::getname(%pl);
			
		if(%pl.totaltimeserved > $recSrv[$missionName] && %pl.totaltimeserved > 0)
		{
			$recSrv[$missionName] = %pl.totaltimeserved;
			$recSrvName[$missionName] = client::getname(%pl);
		}
		else if(%pl.totaltimeserved == $recSrv[$missionName] && %pl.totaltimeserved > 0 && $recSrvName[$missionName] != client::getname(%pl))
			$recSrvName[$missionName] = $recSrvName[$missionName] @ " & " @ client::getname(%pl);	
			
	}
	

	       export("$rec*", "config\\JailRecords.cs", False);

}


function UpdateJailObjectives()
{
	
if(!$missioncomplete)
{
	for(%team = 0; %team < getNumTeams(); %team = %team + 1)	
	{
		%l = 0;
		if($InOurJail[%team] == 0)
			Team::setObjective(%team, $firstJailLine, " <f1><Btowers_neutral.bmp>\nCapture and detain fugitives to gain 12 points per minute.");
		else
			Team::setObjective(%team, $firstJailLine, " <f1><Btower_teamcontrol.bmp>\nPrevent your " @ $InOurJail[%team] @ " prisoners from escaping to retain " @ 12 * $InOurJail[%team] @ " points per minute.");
		%l++;
		
		for(%i = 0; %i < getNumTeams(); %i = %i + 1)	
		{	
			if(%i == %team)
				continue;
		
			if($InTheirJail[%i, %team] == 0)
				Team::setObjective(%team, $firstJailLine + %l, " <f1><Btowers_neutral.bmp>\nAvoid being captured and detained by the " @ getteamname(%i) @ " team.");
			else
				Team::setObjective(%team, $firstJailLine + %l, " <f1><Btower_enemycontrol.bmp>\nRescue your " @ $InTheirJail[%i, %team] @ " teammates who are being detained within the " @ getteamname(%i) @ " prison.");
			
			%l++;
		}
	}
}
else
{
	for(%team = 0; %team < getNumTeams(); %team = %team + 1)	
	{
		%l = 0;
		if($InOurJail[%team] == 0)
			Team::setObjective(%team, $firstJailLine, " <f1><Btower_enemycontrol.bmp>\nNo prisoners were detained within your prison at the end of the mission.");
		else
			Team::setObjective(%team, $firstJailLine, " <f1><Btower_teamcontrol.bmp>\nThere were " @ $InOurJail[%team] @ " prisoners detained within your prison at the end of the mission.");
		%l++;
		
		for(%i = 0; %i < getNumTeams(); %i = %i + 1)	
		{	
			if(%i == %team)
				continue;
		
			if($InTheirJail[%i, %team] == 0)
				Team::setObjective(%team, $firstJailLine + %l, " <f1><Btower_teamcontrol.bmp>\nNone of your teammates were detained by the " @ getteamname(%i) @ " team at the end of the mission.");
			else
				Team::setObjective(%team, $firstJailLine + %l, " <f1><Btower_enemycontrol.bmp>\nThere were " @ $InTheirJail[%i, %team] @ " of your teammates detained by the " @ getteamname(%i) @ " team at the end of the mission.");
			%l++;
		}
	}
}


}
function CheckJailSentence()
{
	%numPlayers = getnumclients();
	
	for(%i = 0; %i < %numPlayers; %i = %i + 1)
		{	
			%pl = getclientbyindex(%i);
			
			if(!%pl.inJail)
				continue;
			// he's not considered in jail, so it doesn't matter...
			
						
			
			
			if(%pl.jailteam != client::getteam(%pl))
			{
				%pl.sentence = %pl.sentence + 5;
				%pl.totaltimeserved = %pl.totaltimeserved + 5; 
				$teamScore[%pl.jailteam] = $teamScore[%pl.jailteam] + 1;
			}
			else
				continue;
				
	
			
			if(%pl.sentence >= %pl.term && %pl.term != "LIFE")
			{
				//his term is up, let him go
				
				%pl.inJail = false;
				%pl.term = 0;			
				%pl.sentence = 0;	
				%jailteam = %pl.jailteam;
				%team = client::Getteam(%pl);
				%pl.jailteam = "";
						
				player::blowup(%pl);
				player::kill(%pl);
				game::playerspawn(%pl, true);
				
				$InOurJail[%jailteam]--;
		 		$InTheirJail[%jailteam, %team]--;
		 		UpdateJailObjectives();
				
				messageAll(0, client::getname(%pl) @ " has been paroled from the " @ getteamname(%jailteam) @ " prison!~wflagreturn.wav");
		 		bottomprint(%pl, "<jc><f0>Your prison term is over, you have been released from the " @ getteamname(%jailteam) @ " prison!", 5);
			}
			else if (%pl.term != "LIFE")
			{
				
				%minremaining = floor((%pl.term - %pl.sentence) / 60);
				%secremaining = (%pl.term - %pl.sentence) % 60;
				
				if(%minremaining == 1)
					%remaining = "1 minute";
				else if(%minremaining > 1)
					%remaining = %minremaining @ " minutes";
				
				if(%secremaining > 0 && %minremaining >= 1)
					%remaining = %remaining @ ", " @ %secremaining @ " seconds";
				else if(%secremaining > 0)
					%remaining =  %secremaining @ " seconds";
				
				bottomprint(%pl, "<jc><f0>You have " @ %remaining @ " until your prison term is complete.", 5);
			}
			else	
					bottomprint(%clientId, "<jc><f0>You have been sentenced to life in prison!  Just hope someone breaks you out...!", 5);
		}
	updateLeaderBoard();
	schedule("CheckJailSentence();", 5);
}

function GroupTrigger::onEnter(%this, %object)
{
	
	%client = Player::getClient(%object);
	//echo("Enter " @ %client);
	
	if(%this.jail)
	{
		if(!%client.inJail) //he's not in jail to begin with so this does not apply
		 	return;
		 	
		 client::setskin(%client, $Server::teamSkin[Client::getTeam(%client)]);
		 %jailteam = %client.jailteam;
		 %team = client::getteam(%client);
		 
		 %client.inJail = false;
		 %client.term = 0;
		 %client.sentence = 0;
		 %client.jailteam = "";
		 
		 if(%jailteam == %team)
		  	return;
		 	
		 %client.escapes = %client.escapes + 1;
		 %client.score = %client.score +5;
		 Game::refreshClientScore(%client);
		 
		 $InOurJail[%jailteam]--;
		 $InTheirJail[%jailteam, %team]--;
		 UpdateJailObjectives();
		 
		 messageAll(0, client::getname(%client) @ " has escaped from the " @ getteamname(%jailteam) @ " prison!~wflag1.wav");
		 bottomprint(%client, "<jc><f0>You have escaped from the " @ getteamname(%jailteam) @ " prison!", 10);
		 schedule("StatusMessage(" @ %client @ ");", 10);
		 
	}
}

function InOwnJailRelease(%clientID)
{
	if(%clientID.injail == true && %clientID.jailteam == client::getteam(%clientID))
	{
		%clientID.inJail = false;
		 %clientID.term = 0;
		 %clientID.sentence = 0;
		 %clientID.jailteam = "";
		 client::setskin(%clientID, $Server::teamSkin[Client::getTeam(%clientID)]);
	}
}

function StatusMessage(%clientID)
{
	if(%clientID.escapes == "")
		%clientID.escapes = 0;
	if(%clientID.murders == "")
		%clientID.murders = 0;
	if(%clientID.captures == "")
		%clientID.captures = 0;
	
	%term = getMinSecs(GetSentenceLength(%clientID));
	%escapes = %clientID.escapes;
	%murders = %clientID.murders;
	%captures = %clientID.captures;
	%outstanding = (%murders + (%escapes * 2)) - %captures;
	if(%outstanding < 0)
		%outstanding = 0;
	
	
		
	bottomprint(%clientId, "<jc><f0>You now have " @ %outstanding @ " infractions (" @ %murders @ " murders, " @ %captures @ " captures, " @ %escapes @ " escapes).  Your current sentence is " @ %term @ ".", 5);
}
	

function GetMinSecs(%term)
{
	%minremaining = floor(%term / 60);
	%secremaining = %term % 60;
				
	if(%minremaining == 1)
		%remaining = "1 minute";
	else if(%minremaining > 1)
		%remaining = %minremaining @ " minutes";
				
	if(%secremaining > 0 && %minremaining > 0)
		%remaining = %remaining @ ", " @ %secremaining @ " seconds";
	else if(%secremaining > 0)
		%remaining = %secremaining @ " seconds";
		
	return %remaining;
}

function GetSentenceLength(%clientID)
{

  	
  	if(%clientID.murders - %clientId.captures > 15)
  	{
		return "LIFE";  		
  	}
  	else
  	{
  	%modifier = 1;
  	%murders = %clientId.murders + (%clientId.escapes * 2);
  	
  	
  	if(%murders - %clientId.captures <= 10 && %murders - %clientId.captures > 0)
  		%modifier = %modifier + (%murders - %clientId.captures) * 0.25;
  	else if(%murders - %clientId.captures <= 15 && %murders - %clientId.captures > 10)
  		%modifier = %modifier + (((%murders - %clientId.captures) - 10) * 0.5) + 2.5;
  	
  	//echo(%modifier);

	if($server::baseterm <= 0)
		%baseterm = 2;
	else
		%baseterm = $server::baseterm;

  	%term = %baseterm * %modifier * 60;
  	
  	return(%term);
  	 	
  	}
  	
  	
}

function Game::pickPlayerSpawn(%clientId, %respawn)
{
  	
  if(%clientID.inJail)
  	return Game::pickTeamSpawn(%clientID.jailteam, true, true);
  else
  	return Game::pickTeamSpawn(client::getteam(%clientId), %respawn, false);
 
}

function Game::pickTeamSpawn(%team, %respawn, %jail)
{
   if(%respawn)
   {
   	if(%jail)
      		return Game::pickJailSpawn(%team);
      	else
      		return Game::pickRandomSpawn(%team);
   }
   else
   {
      %spawn = Game::pickStartSpawn(%team);
      if(%spawn == -1)
         return Game::pickRandomSpawn(%team);
      return %spawn;
   }
}


function Game::pickJailSpawn(%team)
{
   %group = nameToID("MissionGroup/Teams/team" @ %team @ "/DropPoints/Jail");
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

function remoteKill(%client)
{
   
   if(!$matchStarted)
      return;

%player = Client::getOwnedObject(%client);

  if(%client.inJail)
  {
  	bottomprint(%client, "<jc><f0>It's not that easy!  Wait out your term!", 0);
  	return;
  }
  	

   %player = Client::getOwnedObject(%client);
   if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
   {
		playNextAnim(%client);
	   Player::kill(%client);
	   Client::onKilled(%client,%client);
   }
}


function Game::playerSpawned(%pl, %clientId, %armor)
{						  
	if(%clientId.injail)
		client::setskin(%clientId, $Server::jailSkin[%clientId.jailteam]);
	else
		client::setskin(%clientID, $Server::teamSkin[Client::getTeam(%clientId)]);
	%clientId.spawn= 1;
	%max = getNumItems();
   for(%i = 0; (%item = $spawnBuyList[%i]) != ""; %i++)
   {
		buyItem(%clientId,%item);	
		if(%item.className == Weapon) 
			%clientId.spawnWeapon = %item;
	}
	%clientId.spawn= "";
	if(%clientId.spawnWeapon != "") {
		Player::useItem(%pl,%clientId.spawnWeapon);	
   	%clientId.spawnWeapon="";
	}
} 

// Add new SPAWN list:
$spawnBuyList[jail0] = LightArmor;
$spawnBuyList[jail1] = Blaster;
$spawnBuyList[jail2] = RepairKit;

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

		if(%clientID.inJail)
		{
			%type = "jail";
		}
		else if($Settings::spawn[%clientId] == "" || $Settings::spawn[%clientId] == "0")
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
		if(%type != "jail")
		{
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
		}
		return true;
	}
	else {
		Client::sendMessage(%clientId,0,"Sorry No Respawn Positions Are Empty - Try again later ");
		return false;
	}
}

function Client::onKilled(%playerId, %killerId, %damageType)
{
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


   if(!%killerId)
   {
      messageAll(0, strcat(%victimName, " dies."), $DeathMessageMask);
      %playerId.scoreDeaths++;
  }
   else if(%killerId == %playerId)
   {
      %oopsMsg = sprintf($deathMsg[-2, %ridx], %victimName, %playerGender);
      messageAll(0, %oopsMsg, $DeathMessageMask);
      %playerId.scoreDeaths++;
      %playerId.score--;
      Game::refreshClientScore(%playerId);
      
      if(!%playerid.inJail)
      {
      		%playerID.jailteam = "";
      		if(%playerID.lastCaptureDamage > "" && getsimtime() - %playerID.lastCaptureTime < 20)
      		{
     				%captureID = %playerID.lastcaptureDamage;
     				%captureid.captures++;
				%captureId.score++;
				Game::refreshClientScore(%captureID);
				StatusMessage(%captureID);
				%playerID.jailteam = client::getteam(%captureID);
				//echo("Last Capture: " @ %playerid.jailteam);
		}
		
		if(%playerid.jailteam == "" || %playerid.jailteam == client::getteam(%playerID))
		{
			%playerid.jailteam = floor(getRandom() * (getnumteams() - 1));
			if(%playerid.jailteam == client::getteam(%playerID))
				%playerID.jailteam++;
				//echo("Random: " @ %playerid.jailteam);
		
		}
		
		//echo(%playerid.jailteam);
		
		
		%playerid.timescaptured++;
      		   		
      		%playerid.inJail = true;
      		%playerid.term = getsentencelength(%playerId);
		%playerid.sentence = 0;
		
				%jailteam = %playerID.jailteam;
				%team = client::getteam(%playerID);
				
				$teamscore[%jailteam]++;
				$InOurJail[%jailteam]++;
		 		$InTheirJail[%jailteam, %team]++;
		 		UpdateJailObjectives();
				
				if(%playerid.term == "LIFE")
					bottomprint(%playerId, "<jc><f0>You have been sentenced to life in prison!  Just hope someone breaks you out...", 10);
				else
				{
					%term = getminsecs(%playerid.term);
					bottomprint(%playerId, "<jc><f0>You have been sentenced to " @ %term @ " in prison!  Make the best of it!", 10);
				}
				
 
      }
      		
      
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
		if(!String::ICompare(Client::getGender(%playerId), "Male"))
		{
			%victimGender = "him";
		}
		else
		{
			%victimGender = "her";
		}
      if($teamplay && (Client::getTeam(%killerId) == Client::getTeam(%playerId)))
      {
		if(%damageType == $blasterdamagetype || %damageType == $electricitydamagetype)
		messageAll(0, strcat(Client::getName(%killerId), 
   	        " mistakes ", %killerGender, " teammate, ", %victimName, " for a fugitive, and sends ", %victimGender, " to prison!"), $DeathMessageMask);
		else if(%damageType != $MineDamageType) 
	    	messageAll(0, strcat(Client::getName(%killerId), 
   	        " mows down ", %killerGender, " teammate, ", %victimName), $DeathMessageMask);
		else 
	         messageAll(0, strcat(Client::getName(%killerId), 
   	     	" killed ", %killerGender, " teammate, ", %victimName ," with a mine."), $DeathMessageMask);
		 %killerId.scoreDeaths++;
       %killerId.score--;
       Game::refreshClientScore(%killerId);
      }
      else
      {
	     %obitMsg = sprintf($deathMsg[%damageType, %ridx], Client::getName(%killerId),
	       %victimName, %killerGender, %playerGender);
         messageAll(0, %obitMsg, $DeathMessageMask);
         %killerId.scoreKills++;
         %playerId.scoreDeaths++;  // test play mode
         %killerId.score++;
         Game::refreshClientScore(%killerId);
         Game::refreshClientScore(%playerId);
      }
      
      if(!%killerId.injail)
      {
      	if(%damageType == $blasterdamagetype || %damageType == $electricitydamagetype)
	{
		if(!%playerid.injail)
		{
      			
      			%playerid.injail = true;
      			%playerid.jailteam = client::getteam(%killerId);
      			
      		     	if(client::getteam(%killerID) != client::getteam(%playerID))
			{
				%playerid.term = getsentencelength(%playerId);
				%playerid.sentence = 0;
				
				if(%playerid.term == "LIFE")
					bottomprint(%playerId, "<jc><f0>You have been sentenced to life in prison!  Just hope someone breaks you out...", 10);
				else
				{
					%term = getminsecs(%playerid.term);
					bottomprint(%playerId, "<jc><f0>You have been sentenced to " @ %term @ " in prison!  Make the best of it!", 10);
				}
				
				%killerid.captures++;
				%playerid.timescaptured++;
				%killerId.score++;
				Game::refreshClientScore(%killerID);
				StatusMessage(%killerID);
				
				%jailteam = client::getteam(%killerID);
				%team = client::getteam(%playerID);
				
				$teamscore[%jailteam]++;
				$InOurJail[%jailteam]++;
		 		$InTheirJail[%jailteam, %team]++;
		 		UpdateJailObjectives();
				
			}
			else

			{
				%playerID.jailteam = "";
      				if(%playerID.lastCaptureDamage > "" && getsimtime() - %playerID.lastCaptureTime < 20)
      				{
     					%captureID = %playerID.lastcaptureDamage;
     					%captureid.captures++;
					%captureId.score++;
					Game::refreshClientScore(%captureID);
					StatusMessage(%captureID);
					%playerID.jailteam = client::getteam(%captureID);
					//echo("Last Capture: " @ %playerid.jailteam);
				}
		
				if(%playerid.jailteam == "" || %playerid.jailteam == client::getteam(%playerID))
				{
					%playerid.jailteam = floor(getRandom() * (getnumteams() - 1));
					if(%playerid.jailteam == client::getteam(%playerID))
					%playerID.jailteam++;
					//echo("Random: " @ %playerid.jailteam);
		
				}
				
				%playerid.timescaptured++;
      		   		
      				%playerid.inJail = true;
      				%playerid.term = getsentencelength(%playerId);
				%playerid.sentence = 0;
		
				%jailteam = %playerID.jailteam;
				%team = client::getteam(%playerID);
				
				$teamscore[%jailteam]++;
				$InOurJail[%jailteam]++;
		 		$InTheirJail[%jailteam, %team]++;
		 		UpdateJailObjectives();
				
				if(%playerid.term == "LIFE")
					bottomprint(%playerId, "<jc><f0>You have been sentenced to life in prison!  Just hope someone breaks you out...", 10);
				else
				{
					%term = getminsecs(%playerid.term);
					bottomprint(%playerId, "<jc><f0>You have been sentenced to " @ %term @ " in prison!  Make the best of it!", 10);
				}
  			}
			
		}	
     
	}
	else if(client::getteam(%killerID) != client::getteam(%playerId))
	{	
		%killerid.murders++;
		StatusMessage(%killerID);
	}
      }
   }
   Game::clientKilled(%playerId, %killerId);
}

function Server::finishMissionLoad()
{
      	
   
   $loadingMission = false;
	$TestMissionType = "";
   // instant off of the manager
   setInstantGroup(0);
   newObject(MissionCleanup, SimGroup);

   exec($missionFile);
   
   if($game::missiontype != "Jail")
   	{
   	exec(station);
   	exec(admin);
   	exec(observer);
   	exec(player);
   	exec(server);
   	exec(moveable);
   	}
   
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


function processMenuPickTeam(%clientId, %team, %adminClient)
{
	if(%clientId.injail)
		return;
	checkPlayerCash(%clientId);
   if(%team != -1 && %team == Client::getTeam(%clientId))
      return;

   if(%clientId.observerMode == "justJoined")
   {
      %clientId.observerMode = "";
      centerprint(%clientId, "");
   }

   if((!$matchStarted || !$Server::TourneyMode || %adminClient) && %team == -2)
   {
      if(Observer::enterObserverMode(%clientId))
      {
         %clientId.notready = "";
         if(%adminClient == "") 
            messageAll(0, Client::getName(%clientId) @ " became an observer.");
         else
            messageAll(0, Client::getName(%clientId) @ " was forced into observer mode by " @ Client::getName(%adminClient) @ ".");
			Game::resetScores(%clientId);	
		   Game::refreshClientScore(%clientId);
		}
      return;
   }

   %player = Client::getOwnedObject(%clientId);
   if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)) {
		playNextAnim(%clientId);
	   Player::kill(%clientId);
	}
   %clientId.observerMode = "";
   if(%adminClient == "")
      messageAll(0, Client::getName(%clientId) @ " changed teams.");
   else
      messageAll(0, Client::getName(%clientId) @ " was teamchanged by " @ Client::getName(%adminClient) @ ".");

   if(%team == -1)
   {
      Game::assignClientTeam(%clientId);
      %team = Client::getTeam(%clientId);
   }
   GameBase::setTeam(%clientId, %team);
   %clientId.teamEnergy = 0;
	Client::clearItemShopping(%clientId);
	if(Client::getGuiMode(%clientId) != 1)
		Client::setGuiMode(%clientId,1);		


	Client::setControlObject(%clientId, -1);

   Game::playerSpawn(%clientId, false);
	%team = Client::getTeam(%clientId);
	if($TeamEnergy[%team] != "Infinite")
		$TeamEnergy[%team] += $InitialPlayerEnergy;
   if($Server::TourneyMode && !$CountdownStarted)
   {
      bottomprint(%clientId, "<f1><jc>Press FIRE when ready.", 0);
      %clientId.notready = true;
   }
}

function Server::onClientDisconnect(%clientId)
{
	if(%clientID.injail && %clientID.jailteam != client::getteam(%clientID))
	{
		if(%clientID.term != "LIFE")
			%timeremaining = %clientID.term - %clientID.sentence;
		else
			%timeremaining = 600; // 10 min for lifers
		
		if($server::timeLimit)
		{
			%curTimeLeft = ($Server::timeLimit * 60) + $missionStartTime - getSimTime();
			if(%timeremaining > %curTimeLeft)
				%timeremaining = %curTimeLeft;
		}
		
		if(%timeremaining > 600)
			%timeremaining = 600; //don't award points for more than 10 minutes
		
		%score = %timeremaining / 5;
		$teamScore[%clientID.jailteam] = $teamScore[%clientID.jailteam] + %score;
		$InOurJail[%clientID.jailteam]--;
		$InTheirJail[%clientID.jailteam, client::getteam(%clientID)]--;
		messageAll(0, client::getname(%clientID) @ " tried to skip out on his jail term!  " @ %score @ " points have been added to the " @ getteamname(%clientID.jailteam) @ " team's score for the rest of his sentence!");
		%clientID.injail = false;
		
		if($server::BanDroppers != false && !%clientID.isSuperAdmin)
		{
			BanList::add(Client::getTransportAddress(%clientId),%timeremaining);
			if(!String::ICompare(Client::getGender(%clientId), "Male"))
   			{
      			%pronoun = "his";
   			}
			else
			{
				%pronoun = "her";
			}
			messageAll(0, client::getname(%clientID) @ " has been BANNED for the rest of " @ %pronoun @ " term. (" @ getminsecs(%timeremaining) @")");
			return;
		}			
	}	
		
		
	// Need to kill the player off here to make everything
	// is cleaned up properly.
   %player = Client::getOwnedObject(%clientId);
   if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)) {
		playNextAnim(%player);
	   Player::kill(%player);
	}

   Client::setControlObject(%clientId, -1);
   Client::leaveGame(%clientId);
   Game::CheckTourneyMatchStart();
   if(getNumClients() == 1) // this is the last client.
      Server::refreshData();
}

function Vote::changeMission()
{
   $missionComplete = true;
   UpdateJailObjectives();
   updateLeaderBoard();
   updateLeaderRecords();
   ObjectiveMission::refreshTeamScores();
   
   %group = nameToID("MissionCleanup/ObjectivesSet");
	%lineNum = "";
   for(%i = 0; (%obj = Group::getObject(%group, %i)) != -1; %i++)
   {
      ObjectiveMission::objectiveChanged(%obj);
	}
   for(%i = 0; %i < getNumTeams(); %i++) { 
	   Team::setObjective(%i, $firstJailLine-2, " ");
	   Team::setObjective(%i, $firstJailLine-1, "<f5>Mission Summary:");
	}
	ObjectiveMission::setObjectiveHeading();
   $missionComplete = false;
}

function ObjectiveMission::missionComplete()
{
   $missionComplete = true;
   UpdateJailObjectives();
   updateLeaderBoard();
   updateLeaderRecords();
   %group = nameToID("MissionCleanup/ObjectivesSet");
   for(%i = 0; (%obj = Group::getObject(%group, %i)) != -1; %i++)
   {
      ObjectiveMission::objectiveChanged(%obj);
	}
   for(%i = 0; %i < getNumTeams(); %i++) { 
	   Team::setObjective(%i, $firstJailLine-4, " ");
	   Team::setObjective(%i, $firstJailLine-3, "<f5>Mission Summary:");
	   Team::setObjective(%i, $firstJailLine-2, " ");
	}
	ObjectiveMission::setObjectiveHeading();
   ObjectiveMission::refreshTeamScores();
	%lineNum = "";
   $missionComplete = false;

   // back out of all the functions...
   schedule("Server::nextMission();", 0);
}

function Door::onCollision(%this, %object)
{
	if(!Player::isDead(%object) && getObjectType(%object) == "Player") 
		if (GameBase::isActive(%this) && GameBase::isPowered(%this) && %this.faded == "" && !%this.NoOpen)  
			if(GameBase::getTeam(%this) == GameBase::getTeam(%object) || GameBase::getTeam(%this) == -1 || %this.noTeam != "" ) 
				if((%this.triggerOpen == "" || %this.triggerTrigger) ) 
					Door::trigger(%this);
}