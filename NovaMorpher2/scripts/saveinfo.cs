//==========================================================================================================
//== Save Profile
//==========================================================================================================
function SaveCharacter(%clientId)
{
	if ($Client::info[%clientId, 5] == "")
	{
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>You must specify a password in the OtherInfo field in your player profile.\", 5);", 0);
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>If you would like to save your profile.\", 5);", 5);	
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Disconnect, enter a password that you would like to use in the OtherInfo field.\", 5);", 10);
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Reconnect and you will be able to save your profile.\", 5);", 15);
		return;
	}
	else
	{
		%name = Client::getName(%clientId);
		%name = "NovaMorpherProfile_" @ hashname(%name);
		%filename = %name @ ".cs";
		if(isFile("config\\" @ %filename))
		{
			%exec = exec(%filename);
			if($funk::var[%name, password] != $Client::info[%clientId, 5] && %exec)
			{
				for(%i = 1; $funk::var["[\"" @ %name @ "\", " @ %i @ "]"] != ""; %i++)
					$funk::var["[\"" @ %name @ "\", " @ %i @ "]"] = "";
				$funk::var["[\"" @ %name @ "\", 1]"] = "";
				return;
			}
		}

		%name = Client::getName(%clientId);
		%name = hashname(%name);
		%name = ("NovaMorpherProfile_" @ %name);

		//clear $funk::var's

		for(%i = 1; $funk::var["[\"" @ %name @ "\", " @ %i @ "]"] != ""; %i++)
			$funk::var["[\"" @ %name @ "\", " @ %i @ "]"] = "";
		$funk::var["[\"" @ %name @ "\", 1]"] = "";

		echo("Saving player " @ %name @ " (" @ %clientId @ ")...");

		%playerId = %clientId;
		bottomprint(%clientId, "<jc><f1>Your profile was SAVED! Have fun....", 15);

		%clientId.TotalScore = %clientId.TotalScore + %clientId.score - %clientId.usedScore;
		%clientId.usedScore = %clientId.score;

		%count = 0;
		$funk::var["[\"" @ %name @ "\", "@%count++@"]"] = $Settings::spawn[%clientId];
		$funk::var["[\"" @ %name @ "\", "@%count++@"]"] = $Settings::AtomicMorpher[%clientId];
		$funk::var["[\"" @ %name @ "\", "@%count++@"]"] = $Settings::MorphControl[%clientId];
		$funk::var["[\"" @ %name @ "\", "@%count++@"]"] = $Settings::RocketLauncher[%clientId];
		$funk::var["[\"" @ %name @ "\", "@%count++@"]"] = $Settings::AirStrike[%clientId];
		$funk::var["[\"" @ %name @ "\", "@%count++@"]"] = $Settings::Mortar[%clientId];
		$funk::var["[\"" @ %name @ "\", "@%count++@"]"] = $Settings::VehicleAutoMount[%clientId];
		$funk::var["[\"" @ %name @ "\", "@%count++@"]"] = $Settings::Station[%clientId];
		$funk::var["[\"" @ %name @ "\", "@%count++@"]"] = $Settings::ShotGun[%clientId];
		$funk::var["[\"" @ %name @ "\", CurFav]"] = %clientId.CurFav;

		$funk::var["[\"" @ %name @ "\", TS]"] = %clientId.TotalScore;
		$funk::var["[\"" @ %name @ "\", TKS]"] = %clientId.TotalKills;
		$funk::var["[\"" @ %name @ "\", TDS]"] = %clientId.TotalDeaths;

		$funk::var["[\"" @ %name @ "\", TFT]"] = %clientId.FlagTouches;
		$funk::var["[\"" @ %name @ "\", TFC]"] = %clientId.FlagCaps;

		$funk::var["[\"" @ %name @ "\", TOC]"] = %clientId.ObjCaps;
		$funk::var["[\"" @ %name @ "\", TOH]"] = %clientId.ObjHolds;

		$funk::var["[\"" @ %name @ "\", TK]"] = %clientId.TKCount;
		$funk::var["[\"" @ %name @ "\", password]"] = $Client::info[%clientId, 5];

		//== RESET Score after save....
		File::delete("config\\" @ %name @ ".cs");
		export("funk::var[\"" @ %name @ "\",*", "config\\" @ %name @ ".cs", true);

		Game::refreshClientScore(%clientId);
	}
}

//==========================================================================================================
//== Load Profile
//==========================================================================================================
function LoadCharacter(%clientId)
{
	%name = Client::getName(%clientId);
	%name = hashname(%name);
	%name = ("NovaMorpherProfile_" @ %name);

	$Settings::isImposter[%clientId] = false;

	if($debug)
		echo("%name = " @ %name);
	
	%filename = %name @ ".cs";
	%playerId = %clientId;

	if($debug)
		echo("%filename = " @ %filename);

	%isConfig = false;
	if(isFile("config\\" @ %filename))
	{
		%isConfig = true;
		if(exec(%filename))
		{
			//=================================================================== clear $funk::var's
		
			for(%i = 1; $funk::var[%name, %i] != ""; %i++)
				$funk::var[%name, %i] = "";
			$funk::var[%name, 1] = "";
	
			//====================================================================== load character
			if ($debug) echo("Loading player... " @ %name @ " (" @ %clientId @ ")...");
	
			exec(%filename);
	
			if($debug)
				echo($funk::var[%name, password] @ " == " @ $Client::info[%clientId, 5]);
			if ($funk::var[%name, password] == $Client::info[%clientId, 5])
			{
				%count = 0;
				$Settings::Spawn[%clientId] = $funk::var[%name, %count++];
				$Settings::AtomicMorpher[%clientId] = $funk::var[%name, %count++];
				$Settings::MorphControl[%clientId] = $funk::var[%name, %count++];
				$Settings::RocketLauncher[%clientId] = $funk::var[%name, %count++];
				$Settings::AirStrike[%clientId] = $funk::var[%name, %count++];
				$Settings::Mortar[%clientId] = $funk::var[%name, %count++];
				$Settings::VehicleAutoMount[%clientId] = $funk::var[%name, %count++];
				$Settings::Station[%clientId] = $funk::var[%name, %count++];
				$Settings::ShotGun[%clientId] = $funk::var[%name, %count++];
				%clientId.CurFav = $funk::var[%name, CurFav];

				%clientId.TotalScore = $funk::var[%name, TS];
				%clientId.TKCount = $funk::var[%name, TK];

				%clientId.TotalKills = $funk::var[%name, TKS];
				%clientId.TotalDeaths = $funk::var[%name, TDS];

				%clientId.FlagTouches = $funk::var[%name, TFT];
				%clientId.FlagCaps = $funk::var[%name, TFC];

				%clientId.ObjCaps = $funk::var[%name, TOC];
				%clientId.ObjHolds = $funk::var[%name, TOH];


			
				if ($Debug)
					echo("Load complete.");
				%clientId.SavedInfo = "True";
	
				//== This is to tell that this person is NOT an imposter...
				$Settings::isImposter[%clientId] = "false";
			}
			else if($Client::info[%clientId, 5] != "" && ($funk::var[%name, password] != $Client::info[%clientId, 5]))
			{
				$Settings::isImposter[%clientId] = "true";
	
				schedule("messageall(1, \"" @ Client::getName(%clientId) @ " is an IMPOSTER\");", 0);
				schedule("messageall(1, \"" @ Client::getName(%clientId) @ " is an IMPOSTER\");", 5);
				schedule("messageall(1, \"" @ Client::getName(%clientId) @ " is an IMPOSTER\");", 10);
				schedule("messageall(1, \"" @ Client::getName(%clientId) @ " is an IMPOSTER\");", 15);
				schedule("messageall(1, " @ Client::getName(%clientId) @ " @ \" was auto-kicked because he was an IMPOSTER\");", 20);
				schedule("messageall(1, " @ Client::getName(%clientId) @ " @ \" is an idiot in disguise... Let us all remember that...\");", 22);

				schedule("Client::sendMessage("@%clientId@", 1,\"~wmale3.wbye.wav\");", 15);
				schedule("Client::sendMessage("@%clientId@", 1,\"~wmale3.wbye.wav\");", 16);
				schedule("Client::sendMessage("@%clientId@", 1,\"~wmale3.wdsgst2.wav\");", 17);
				schedule("Client::sendMessage("@%clientId@", 1,\"~wfemale1.wdsgst2.wav\");", 18);
				schedule("Client::sendMessage("@%clientId@", 1,\"~wmale2.wdsgst2.wav\");", 19);

				//== Make it flash on and off...
				for(%i = 0; %i < 20; %i++)
				{
					schedule("centerprint("@%clientId@", \"<jc><f3>Sorry! <f2>Imposters <f1>NOT ALLOWED!\", 0.5);",%i);
				}
	
				schedule("Net::kick(" @ %clientId @ ", \"Change your screen name or don't come back!\");",20); 
	
	
				for(%i = 1; $funk::var[%name, %i] != ""; %i++)
				{
					$funk::var[%name, %i] = "";
					$funk::var[%name, 1] = "";
				}
				//== This is to tell that this person IS an imposter...
			}
		}
		else
		{
			%isConfig = false;
		}
	}

	if(!%isConfig)
	{
		if($Client::info[%clientId, 5] != "")
		{
			schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>All profiles that were last updated before: " @ $NovaMorpher::LastDelDate @ " was removed. Sorry for the inconvenience this may have caused you.\", 15);", 25);
		}
		else
		{
			schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>You must specify a password in the OtherInfo field in your player profile.\", 5);", 25);
			schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>If you would like to save your profile.\", 5);", 30);
		}
	}
   	Game::refreshClientScore(%clientId);
}


function hashname(%name)
{
	%name = escapeString(%name);
	%name = String::replace(%name, "?", "A1");
	%name = String::replace(%name, "\\", "A2");
	%name = String::replace(%name, "/", "A3");
	%name = String::replace(%name, "!", "A4");
	%name = String::replace(%name, "@", "A5");
	%name = String::replace(%name, "#", "A6");
	%name = String::replace(%name, "$", "A7");
	%name = String::replace(%name, "%", "A8");
	%name = String::replace(%name, "^", "A9");
	%name = String::replace(%name, "&", "A0");
	%name = String::replace(%name, "*", "B1");
	%name = String::replace(%name, "(", "B2");
	%name = String::replace(%name, ")", "B3");
	%name = String::replace(%name, "+", "B4");
	%name = String::replace(%name, "=", "B5");
	%name = String::replace(%name, ":", "B6");
	%name = String::replace(%name, ";", "B7");
	%name = String::replace(%name, "<", "B8");
	%name = String::replace(%name, ">", "B9");
	%name = String::replace(%name, ",", "B0");
	%name = String::replace(%name, "|", "C1");
	%name = String::replace(%name, "`", "C2");
	%name = String::replace(%name, "~", "C3");
	%name = String::replace(%name, "]", "C3");
	%name = String::replace(%name, "[", "C4");
	%name = String::replace(%name, "}", "C5");
	%name = String::replace(%name, "{", "C6");
	%name = String::replace(%name, " ", "C7");
	%name = String::replace(%name, ".", "C8");
	%name = String::replace(%name, "-", "C9");
	%name = String::replace(%name, "_", "D1");
	return %name;
}
function String::len(%string)
{
	for(%i=0; String::getSubStr(%string, %i, 1) != ""; %i++)
		%length++;

	return %length;
}

function String::replace(%string, %search, %replace, %offSet, %safety)
{
	if(%offSet == "")
		%offSet = 0;
	%loc = String::findSubStr(%string, %search);
	for(%loc; %loc != -1; %i++)
	{
		if(%i > String::len(%string)+50 && %safety)
		{
			echo("FATAL ERROR: INFINITE LOOP SAFETY EXIT!!!");
			return;
		}

		%lenstr = String::len(%string);
		%lenser = String::len(%search);
		%part1 = String::getSubStr(%string, 0, %loc - %offSet);
		%part2 = String::getSubStr(%string, %loc + %lenser, %lenstr - %loc - %lenser);
		%string = %part1 @ "" @ %replace @ %part2;
		%loc = String::findSubStr(%string, %search);
	}
	return %string;
}

function saveall()
{
	%numClients = getNumClients();
	%numCl = ((2049 + %numClients) + 20);
	%linenum = 10;

	for(%k = 0 ; %k < %numClients; %k++) 
		%clientList[%k] = getClientByIndex(%k);

	for(%k= 0 ; %k < %numClients; %k++)
	{
		if($missionComplete == "True")
		{
			if ($Debug) echo ("Incrementing Mission For " @ %clientList[%k] @".");
			%clientList[%k].MissionComplete++;
		}
		SaveCharacter(%clientList[%k]);
	}
}

function loadall()
{
	%numClients = getNumClients();
	%numCl = ((2049 + %numClients) + 20);
	%linenum = 10;

	for(%k = 0 ; %k < %numClients; %k++) 
		%clientList[%k] = getClientByIndex(%k);

	for(%k= 0 ; %k < %numClients; %k++)
	{
		if($missionComplete == "True")
		{
			if ($Debug) echo ("Incrementing Mission For " @ %clientList[%k] @".");
			%clientList[%k].MissionComplete++;
		}
		LoadCharacter(%clientList[%k]);
	}
}

function Reload()
{
	if($debug)
		echo("*** RELOADING PROFILES!! **");

	saveall();
	loadall();
}