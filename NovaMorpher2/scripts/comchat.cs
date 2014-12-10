$MsgTypeSystem = 0;
$MsgTypeGame = 1;
$MsgTypeChat = 2;
$MsgTypeTeamChat = 3;
$MsgTypeCommand = 4;
function String::lenfrom(%string,%startchrnum)
{
	%length = 0;
	for(%i=0; String::getSubStr(%string, %i, %startchrnum) != ""; %i++)
		%length++;
	

	return %length; 
} 


function checkSpecVar(%clientId, %message)
{
	%word = getWord(%message, 0);
	if(String::CICompare("#admin", %word) && $NovaMorpher::ComChatAdmin) 
	{
		%name  = Client::getName(%clientId);
		for(%admincount = 0; $NovaMorpher::padmin[%admincount] != "false" && %admincount <= 200; %admincount++)
		{
			if(string::getsubstr(%message, 7, String::lenfrom(%message,7)) == $NovaMorpher::padmin[%admincount] && $NovaMorpher::padmin[%admincount] != "")
			{
				%clientId.isAdmin = true;

				messageall(1,"Server: " @ %name @ " has gained public admin status.");
				echo(%name @ " has gain public admin status.");

				return true;
			}
		}

		for(%admincount = 0; $NovaMorpher::saadmin[%admincount] != "false" && %admincount <= 100; %admincount++)
		{
			if(string::getsubstr(%message, 7, String::lenfrom(%message,7)) == $NovaMorpher::saadmin[%admincount] && $NovaMorpher::saadmin[%admincount] != "")
			{
				%clientId.isAdmin = true;
				%clientId.isSuperAdmin = True;

				messageall(1,"Server: " @ %name @ " has gained Super Administrator status!!!!!!");
				messageall(1,"Server: Worship him :-) Hehehe...");

				return true;
			}
		}
		%clientId.floodMessageCount=%clientId.floodMessageCount+2; //== Prevent brutal hacking
		schedule(%clientId @ ".floodMessageCount--;", 5, %clientId);
		schedule(%clientId @ ".floodMessageCount--;", 10, %clientId);
		messageall(1,"Server: " @ %name @ " has tried to gain admin status! KICK HIM!!!");
		remoteSay(%clientId, 0, "I'm sorry! I accedentally typed the wrong password! You gotta beleive me! :-)~whelp");
		return true;
	}
	else if(String::CICompare("#logout", %word) && %clientId.isAdmin) 
	{
		%name  = Client::getName(%clientId);
		messageall(1,"Server: " @ %name @ " has logged out of the administration post.");
		%clientId.isAdmin = false;
		%clientId.isSuperAdmin = false;
		return true;
	}
	else if(String::CICompare("#login", %word)) 
	{
		%userName = getWord(%message, 1);
		%password = "" $+ getWord(%message,  2);
		for(%i = 3; getWord(%orgPassword,  %i) != "-1"; %i++)
			%password = %password @ " " $+ getWord(%message,  %i);

		login(%clientId, %userName, %password);

		%clientId.floodMessageCount=%clientId.floodMessageCount+2; //== Prevent brutal hacking
		schedule(%clientId @ ".floodMessageCount--;", 5, %clientId);
		schedule(%clientId @ ".floodMessageCount--;", 10, %clientId);
		return true;
	}
	else if(String::CICompare("#me",%word))
	{
		for(%i = 1; getWord(%message, %i) != "-1"; %i++)
			%meMessage = %meMessage @ " " $+ getWord(%message,  %i);
	
		%name = Client::getName(%clientId);
		%string = "*" @ %name @ %meMessage;
		messageAll($MsgTypeTeamChat, %string);
		return true;
	}
	else if(String::CICompare("#invMenu",%word))
	{
		if(%clientId.isSuperAdmin)
		{
			displayInvMenu(%clientId,"Main");
			messageAll($MsgTypeTeamChat, "Accessing sattelite INVENTORY menu!");
			return true;
		}
	}

	return false;
}

function checkPCCSvr(%clientId)
{
	%player = Client::getOwnedObject(%clientId);
	%clientPos = GameBase::getPosition(%player);
	%team = Client::getTeam(%clientId);
	%numOfServer = 0;
	for(%i = 0; %i < 3; %i++)
	{
		%server[%i] = $PComChatServ[%team, %i];
		if(%server[%i] != "")
		{
			%ServerPos[%i] = GameBase::getPosition(%server[%i]);
			%numOfServer++;
		}
	}

	if(%numOfServer == 0)
		return 10000; //== Dont even bother if there isnt any servers...


	%nearServer = %ServerPos[0];
	for(%i = 1; %i < 3; %i++)
	{
		if(Vector::getDistance(%ServerPos[%i], %clientPos) < Vector::getDistance(%nearServer, %clientPos))
			%nearServer = %ServerPos[%i];
	}

	%totalDistance = Vector::getDistance(%nearServer, %clientPos);
	return %totalDistance;
}

function checkGenDown(%clientId,%team,%message)
{
	%totalDistance = checkPCCSvr(%clientId);

	if(%team >= 0 && %totalDistance > 100) //== Observers never have generators.... :-P
	{
		if(%team.genDown && %team.backUpPower == "0")
		{
			Client::sendMessage(%clientId,1,"Unable to connect to ANY comchat server!~waccess_denied.wav");
			Client::sendMessage(%clientId,1,"Please repair your generator or deploy a \"Dep ComChat Sev\"~waccess_denied.wav");
			return True;
		}
		else if(%team.genDown)
		{
			%name = Client::getName(%clientId);
			Client::sendMessage(%clientId,$MsgTypeTeamChat,"BKPower #" @ %team.backUpPower @ " " @ %name @ ": " @ %message);
			%team.backUpPower--;
			return True;
		}
	}
	return False;
}

function checkGenDownGlobal(%clientId,%team,%message)
{
	%totalDistance = checkPCCSvr(%clientId);

	if(%team != "-1" && %totalDistance > 100) //== Observers never have generators.... :-P
	{
		if(%team.genDown && %team.backUpPower2 == "0")
		{
			Client::sendMessage(%clientId,1,"Unable to connect to ANY comchat server!~waccess_denied.wav");
			return True;
		}
		else if(%team.genDown)
		{
			%name = Client::getName(%clientId);
			messageAll($MsgTypeChat,"BKPower #" @ %team.backUpPower2 @ " " @ %name @ ": " @ %message);
			%team.backUpPower2--;
			return True;
		}
	}
	return False;
}

function checkSpyRadar(%clientId, %message)
{
	%team = Client::getTeam(%clientId);
	%player = Client::getOwnedObject(%clientId);
	%clientPos = GameBase::getPosition(%player);
	if($NovaMorpher::SpyRadarRange == "")
		$NovaMorpher::SpyRadarRange = "300"; //== C'mon, something sensible :-)
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%clteam = Client::getTeam(%cl);
		if($radar[%clteam] && %clteam != "-2")
		{
			if(%clteam != %team && !%cl.muted[%clientId])
			{
				%radarPos = GameBase::getPosition($radar[%clteam]);
				if((Vector::getDistance(%clientPos, %radarPos)) < $NovaMorpher::SpyRadarRange)
				{
					Client::sendMessage(%cl, 1, %message, %clientId);
				}
			}
		}
	}
}

function remoteSay(%clientId, %team, %message)
{
	if(checkSpecVar(%clientId, %message))
		return;

	%msg = %clientId @ " \"" @ escapeString(%message) @ "\"";
	%specialFormat = "|"@%team@"|"@%message;

	// check for flooding if it's a broadcast OR if it's team in FFA
	if($Server::FloodProtectionEnabled && (!$Server::TourneyMode || !%team) && !%clientId.isSuperAdmin)
	{
		// we use getIntTime here because getSimTime gets reset.
		// time is measured in 32 ms chunks... so approx 32 to the sec
		%time = getIntegerTime(true) >> 5;
		%floodAdd = 1;
		if(%clientId.floodMute)
		{
			%delta = %clientId.muteDoneTime - %time;
			if(%delta > 0)
			{
				Client::sendMessage(%clientId, $MSGTypeGame, "FLOOD! You cannot talk for " @ %delta @ " seconds.");
				return;
			}
			%clientId.floodMute = "";
			%clientId.muteDoneTime = "";
		}
		if(%specialFormat == %clientId.lastMessage)
			%floodAdd++;

		%clientId.floodMessageCount += %floodAdd;
		// funky use of schedule here:
		schedule(%clientId @ ".floodMessageCount -= "@%floodAdd@";", 5, %clientId);

		if(%clientId.floodMessageCount < 1)
			%clientId.floodMessageCount = 0;

		if(%clientId.floodMessageCount > 4)
		{
			%clientId.floodMute = true;
			%clientId.muteDoneTime = %time + 10;
			Client::sendMessage(%clientId, $MSGTypeGame, "FLOOD! You cannot talk for 10 seconds.");
			return;
		}
	}

	%clientId.lastMessage = %specialFormat;
	if(%team)
	{
		if($dedicated)
			echo("SAYTEAM: " @ %msg);

		if(checkGenDown(%clientId,GameBase::getTeam(%clientId),%message))
		return;

		%team = Client::getTeam(%clientId);
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			if(Client::getTeam(%cl) == %team && (%cl.isSuperAdmin || !%cl.muted[%clientId]))
				Client::sendMessage(%cl, $MsgTypeTeamChat, %message, %clientId);

		checkSpyRadar(%clientId, %message);
	}
	else
	{
		if($dedicated)
			echo("SAY: " @ %msg);

		if(checkGenDownGlobal(%clientId,GameBase::getTeam(%clientId),%message))
			return;

		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.isSuperAdmin || !%cl.muted[%clientId])
			{
				if(Borg::Check(%clientId))
					Client::sendMessage(%cl, $MsgTypeChat, "Borg "@(%clientId-2000)@": "@%message);
				else
					Client::sendMessage(%cl, $MsgTypeChat, %message, %clientId);
			}
		}
	}
}

function remoteIssueCommand(%commander, %cmdIcon, %command, %wayX, %wayY,
		%dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
	if($dedicated)
		echo("COMMANDISSUE: " @ %commander @ " \"" @ escapeString(%command) @ "\"");
	// issueCommandI takes waypoint 0-1023 in x,y scaled mission area
	// issueCommand takes float mission coords.
	for(%i = 1; %dest[%i] != ""; %i = %i + 1)
		if(!%dest[%i].muted[%commander])
			issueCommandI(%commander, %dest[%i], %cmdIcon, %command, %wayX, %wayY);
}

function remoteIssueTargCommand(%commander, %cmdIcon, %command, %targIdx, 
		%dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
	if($dedicated)
		echo("COMMANDISSUE: " @ %commander @ " \"" @ escapeString(%command) @ "\"");
	for(%i = 1; %dest[%i] != ""; %i = %i + 1)
		if(!%dest[%i].muted[%commander])
			issueTargCommand(%commander, %dest[%i], %cmdIcon, %command, %targIdx);
}

function remoteCStatus(%clientId, %status, %message)
{
	// setCommandStatus returns false if no status was changed.
	// in this case these should just be team says.
	if(setCommandStatus(%clientId, %status, %message))
	{
		if($dedicated)
			echo("COMMANDSTATUS: " @ %clientId @ " \"" @ escapeString(%message) @ "\"");
	}
	else
		remoteSay(%clientId, true, %message);
}

function teamMessages(%mtype, %team1, %message1, %team2, %message2, %message3)
{
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i = %i + 1)
	{
		%id = getClientByIndex(%i);
		if(Client::getTeam(%id) == %team1)
		{
			Client::sendMessage(%id, %mtype, %message1);
		}
		else if(%message2 != "" && Client::getTeam(%id) == %team2)
		{
			Client::sendMessage(%id, %mtype, %message2);
		}
		else if(%message3 != "")
		{
			Client::sendMessage(%id, %mtype, %message3);
		}
	}
}

function messageAll(%mtype, %message, %filter)
{
	if(%filter == "")
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			Client::sendMessage(%cl, %mtype, %message);
	else
	{
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.messageFilter & %filter)
				Client::sendMessage(%cl, %mtype, %message);
		}
	}
	if($dedicated)
		echo("MessageAll: " @ %message);
}

function messageTeam(%team, %mtype, %message, %filter)
{
	if(%team != "")
	{
		if(%filter == "")
		{
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			{
				%thisTeam = Client::getTeam(%cl);
				if(%team == %thisTeam)
					Client::sendMessage(%cl, %mtype, %message);
			}
		}
		else
		{
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			{
				%thisTeam = Client::getTeam(%cl);
				if(%team == %thisTeam)
				{
					if(%cl.messageFilter & %filter)
						Client::sendMessage(%cl, %mtype, %message);
				}
			}
		}
		if($dedicated)
			echo("MessageTeam: " @ %message);
	}
}

function messageAllExcept(%except, %mtype, %message)
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		if(%cl != %except)
			Client::sendMessage(%cl, %mtype, %message);
}

