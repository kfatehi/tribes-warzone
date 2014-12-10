//==			Clear Need Globals			==//
$curVoteTopic = "";
$curVoteAction = "";
$curVoteOption = "";
$curVoteCount = 0;

//==			Change Menu Options		==//
function Game::menuRequest(%clientId)
{
	//== Hehehe.... Mega insult someone for being an imposter :-P
	if($Settings::isImposter[%clientId])
	{
		%curItem = 0;
		Client::buildMenu(%clientId, %curItem++ @   "IDIOPIA!!!!", "request", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Imposter Free?);
		Client::addMenuItem(%clientId, %curItem++ @ "Die Imposter?);
		Client::addMenuItem(%clientId, %curItem++ @ "");
		Client::addMenuItem(%clientId, %curItem++ @ "Don't Come BACK UNLESS");
		Client::addMenuItem(%clientId, %curItem++ @ "YOU CHANGE YOU NAME!!!");
		Client::addMenuItem(%clientId, %curItem++ @ "");
		Client::addMenuItem(%clientId, %curItem++ @ "No Offence :-)");
		return;
	}

	%curItem = 0;
	Client::buildMenu(%clientId, "Options", "options", true);
	%sel = %clientId.selClient;
	%name = Client::getName(%sel);
	if(%clientId.selClient)
	{

		if($curVoteTopic == "" && !%clientId.isAdmin)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to admin " @ %name, "vadmin " @ %sel);
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to kick " @ %name, "vkick " @ %sel);
		}

		if(%clientId.isAdmin || %clientId.isTkVictim)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Admin Functions", "adminsmenu " @ %sel);
		}
		
		if(%clientId.muted[%sel])
			Client::addMenuItem(%clientId, %curItem++ @ "Unmute " @ %name, "unmute " @ %sel);
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Mute " @ %name, "mute " @ %sel);
		if(%clientId.observerMode == "observerOrbit")
			Client::addMenuItem(%clientId, %curItem++ @ "Observe " @ %name, "observe " @ %sel);
	}
	else
	{
		if(!$matchStarted || !$Server::TourneyMode || $TeamLock[%sel])
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Change Teams/Observe", "changeteams");
		}
		Client::addMenuItem(%clientId, %curItem++ @ "Configurations", "wpoptions");
		Client::addMenuItem(%clientId, %curItem++ @ "Save Config", "save");
		if($curVoteTopic != "" && %clientId.vote == "")
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Vote YES to " @ $curVoteTopic, "voteYes " @ $curVoteCount);
			Client::addMenuItem(%clientId, %curItem++ @ "Vote NO to " @ $curVoteTopic, "voteNo " @ $curVoteCount);
		}
		else if($curVoteTopic == "" && !%clientId.isAdmin)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Voteing options", "voteOp");

			if ((($Spoonbot::UserMenu) && (!$Spoonbot::BotTree_Design)) && $NovaMorpher::EnableSpoonBot && !%clientId.isAdmin)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Spoonbot controls", "botmenu");
			}

		}
		if($MastersSlave[%clientId] != "")
			Client::addMenuItem(%clientId, %curItem++ @ "Parasite Ops", "parasiteOps");


		if(%clientId.isAdmin) //== Differ Matter
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Admin Functions", "adminsmenu " @ %sel);
		}
	}
}

function processMenuOptions(%clientId, %option)
{
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);

	if(%opt == "fteamchange")
	{
		%clientId.ptc = %cl;
		Client::buildMenu(%clientId, "Pick a team:", "FPickTeam", true);
		Client::addMenuItem(%clientId, "0Observer", -2);
		Client::addMenuItem(%clientId, "1Automatic", -1);
		for(%i = 0; %i < getNumTeams(); %i = %i + 1)
		 Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
		return;
	}		
	else if(%opt == "adminsmenu")
	{
		if(!%clientId.isAdmin && !%clientId.isTkVictim)
		{
				return;	
		}
		%sel = %cl;
		%name = Client::getName(%sel);
		%curItem = 0;
		Client::buildMenu(%clientId, "Admin Options", "options", true);
		//== If user selects another user and is admin.

		if(%clientId.selClient)
		{
			if(%clientId.isAdmin)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Change " @ %name @ "'s team", "fteamchange " @ %sel);
			}
			if(%clientId.isAdmin || (%clientId.isTkVictim && %clientId.TkvSer == %sel))
				Client::addMenuItem(%clientId, %curItem++ @ "Kick " @ %name, "kick " @ %sel);
			//== If user selects another user and is super admin.
			if(%clientId.isSuperAdmin)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Ban " @ %name, "ban " @ %sel);
				Client::addMenuItem(%clientId, %curItem++ @ "Punish " @ %name, "punish " @ %sel);
				if(!%sel.isAdmin)
				{
					Client::addMenuItem(%clientId, %curItem++ @ "Admin " @ %name, "admin " @ %sel);
				}
				else if(%sel.isAdmin && !%sel.isSuperAdmin)
				{
					Client::addMenuItem(%clientId, %curItem++ @ "DeAdmin " @ %name, "deadmin " @ %sel);
				}
			}
			//== If user selects another user and is a tk victim.
			if(%clientId.isTKVictim && !%clientId.isAdmin && !%clientId.isSuperAdmin && $lastTK[%clientId] == %sel)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Kick " @ %name, "kick " @ %sel);
			}
			return;
		}
		else
		{
			//Client::addMenuItem(%clientId, %curItem++ @ "Change Teams/Observe", "changeteams");// <== WTF is this doing here!?
			if (!$Spoonbot::BotTree_Design && $NovaMorpher::EnableSpoonBot)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Spoonbot controls", "botmenu");
			}
			else if($Spoonbot::BotTree_Design && $NovaMorpher::EnableSpoonBot)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Add Tree Point", "TreePoint");
				Client::addMenuItem(%clientId, %curItem++ @ "Calculate Tree Routes", "TreeCalc");
				return; //== Play safe ;)
			}

			Client::addMenuItem(%clientId, %curItem++ @ "Change mission", "cmission");
			Client::addMenuItem(%clientId, %curItem++ @ "Server Options", "serverOps");

			if($Server::TourneyMode)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Change to FFA mode", "cffa");
				if(!$CountdownStarted && !$matchStarted)
					Client::addMenuItem(%clientId, %curItem++ @ "Start the match", "smatch");
			}
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Change to Tournament mode", "ctourney");

			Client::addMenuItem(%clientId, %curItem++ @ "Set Time Limit", "ctimelimit");
			Client::addMenuItem(%clientId, %curItem++ @ "Reset Server Defaults", "reset");
			return;
		}
	}	
	else if(%opt == "changeteams")
	{
		if(!$matchStarted || !$Server::TourneyMode)
		{
			Client::buildMenu(%clientId, "Pick a team:", "PickTeam", true);
			Client::addMenuItem(%clientId, "0Observer", -2);
			Client::addMenuItem(%clientId, "1Automatic", -1);
			if($NovaMorpher::EvenTeams)
			{
				%team = getUnBalancedTeam();
				if(%team == "balanced")
				{
					%team = GameBase::getTeam(%clientId);
					if(%team == -1)
					{
						for(%i = 0; %i < getNumTeams(); %i = %i + 1)
							Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);

						return;
					}
				}
				Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%team), %team);
			}
			else
				for(%i = 0; %i < getNumTeams(); %i = %i + 1)
					Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
			return;
		}
	}
	else if(%opt == "serverOps")
	{
		if(!%clientId.isAdmin)
			return;	

		%curItem = 0;
		Client::buildMenu(%clientId, "Server Options", "options", true);
		if($Server::TeamDamageScale == 1.0)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable team damage", "dtd");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable team damage", "etd");

		if($NovaMorpher::EvenTeams)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable fair teams", "evenBalancedTeams");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable fair teams", "evenBalancedTeams");

		Client::addMenuItem(%clientId, %curItem++ @ "Out Of Bound Op", "outOfBounds");

		Client::addMenuItem(%clientId, %curItem++ @ "Balance Teams", "autobalanceteams");
		return;
	}
	else if(%opt == "voteOp")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Voteing Options", "options", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Vote to change mission", "vcmission");

		if($Server::TeamDamageScale > 0)
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to disable team damage", "vdtd");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to enable team damage", "vetd");
				
		if($Server::TourneyMode)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to enter FFA mode", "vcffa");
			if(!$CountdownStarted && !$matchStarted)
				Client::addMenuItem(%clientId, %curItem++ @ "Vote to start the match", "vsmatch");
		}
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to enter Tournament mode", "vctourney");
		return;
	}

	//== New if/else string for parasite options
	if(%opt == "parasiteOps")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Paraside Option", "options", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Dump you slave", "para_dump");
		return;
	}
	else if(%opt == "para_dump")
	{
		ParasiteDamageType::DumpSlave($MastersSlave[%clientId]);
	}

	if(%opt == "mute")
	{
		%clientId.muted[%cl] = true;
		Client::sendMessage(%cl, 3, Client::getName(%clientId)@" can no longer hear you!");
	}
	else if(%opt == "unmute")
	{
		%clientId.muted[%cl] = "";
		Client::sendMessage(%cl, 3, Client::getName(%clientId)@" has allowed transmission to him!");
	}
	else if(%opt == "outOfBounds")
	{
		if(!%clientId.isAdmin)
			return;	

		%curItem = 0;
		Client::buildMenu(%clientId, "Out Bound Option", "OOBOp", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Current Setting:", "back");
		Client::addMenuItem(%clientId, %curItem++ @"    "@$NovaMorpher::leaveArea[$NovaMorpher::noLeaveArea], "back");
		Client::addMenuItem(%clientId, %curItem++@"Other Ops:", "back");
		for(%i = 0; %i <= 3; %i++)
			if($NovaMorpher::noLeaveArea != %i)
				Client::addMenuItem(%clientId, %curItem++ @ "    "@ $NovaMorpher::leaveArea[%i], %i);
		return;
	}
	else if(%opt == "evenBalancedTeams")
	{
		if(!%clientId.isAdmin)
			return;	

		if($NovaMorpher::EvenTeams)
		{
			$NovaMorpher::EvenTeams = "false";
			messageAll(0, Client::getName(%clientId) @ " DISABLED fair teams.");
		}
		else
		{
			$NovaMorpher::EvenTeams = "true";
			messageAll(0, Client::getName(%clientId) @ " ENABLED fair teams.");
		}
	}
	else if(%opt == "autobalanceteams")
	{



		if(!%clientId.isAdmin)
			return;	

		balanceTeams();
	}
	else if(%opt == "vkick")
	{
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "kick " @ Client::getName(%cl), "kick", %cl);
	}
	else if(%opt == "vadmin")
	{
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "admin " @ Client::getName(%cl), "admin", %cl);
	}
	if(%opt == "vsmatch")
		Admin::startVote(%clientId, "start the match", "smatch", 0);
	else if(%opt == "vetd")
		Admin::startVote(%clientId, "enable team damage", "etd", 0);
	else if(%opt == "vdtd")
		Admin::startVote(%clientId, "disable team damage", "dtd", 0);
	else if(%opt == "etd")
		Admin::setTeamDamageEnable(%clientId, true);
	else if(%opt == "dtd")
		Admin::setTeamDamageEnable(%clientId, false);
	else if(%opt == "vcffa")
		Admin::startVote(%clientId, "change to Free For All mode", "ffa", 0);
	else if(%opt == "vctourney")
		Admin::startVote(%clientId, "change to Tournament mode", "tourney", 0);
	else if(%opt == "cffa")
		Admin::setModeFFA(%clientId);
	else if(%opt == "ctourney")
		Admin::setModeTourney(%clientId);
	else if(%opt == "voteYes" && %cl == $curVoteCount)
	{
		%clientId.vote = "yes";
		centerprint(%clientId, "", 0);
	}
	else if(%opt == "voteNo" && %cl == $curVoteCount)
	{
		%clientId.vote = "no";
		centerprint(%clientId, "", 0);
	}

	if(%opt == "kick")
	{
		Client::buildMenu(%clientId, "Confirm kick:", "kaffirm", true);
		Client::addMenuItem(%clientId, "1Kick " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't kick " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
	else if(%opt == "admin")
	{
		Client::buildMenu(%clientId, "Confirm admim:", "aaffirm", true);
		Client::addMenuItem(%clientId, "1Admin " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't admin " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
	else if(%opt == "deadmin")
	{
		Client::buildMenu(%clientId, "Confirm admim:", "daaffirm", true);
		Client::addMenuItem(%clientId, "1Admin " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't admin " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
	else if(%opt == "ban")
	{
		Client::buildMenu(%clientId, "Confirm Ban:", "baffirm", true);
		Client::addMenuItem(%clientId, "1Ban " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't ban " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
	else if(%opt == "smatch")
		Admin::startMatch(%clientId);
	else if(%opt == "vcmission" || %opt == "cmission")
	{
//		Admin::changeMissionMenu(%clientId, %opt == "cmission");
		Admin::changeMissionMenu(%clientId, "-1");
		return;
	}
	else if(%opt == "ctimelimit")
	{
		Client::buildMenu(%clientId, "Change Time Limit:", "ctlimit", true);
		Client::addMenuItem(%clientId, "110 Minutes", 10);
		Client::addMenuItem(%clientId, "215 Minutes", 15);
		Client::addMenuItem(%clientId, "320 Minutes", 20);
		Client::addMenuItem(%clientId, "425 Minutes", 25);
		Client::addMenuItem(%clientId, "530 Minutes", 30);
		Client::addMenuItem(%clientId, "645 Minutes", 45);
		Client::addMenuItem(%clientId, "71 Hour", 60);
		Client::addMenuItem(%clientId, "8No Time Limit", 0);
		return;
	}

	else if(%opt == "reset")
	{
		Client::buildMenu(%clientId, "Confirm Reset:", "raffirm", true);
		Client::addMenuItem(%clientId, "1Reset", "yes");
		Client::addMenuItem(%clientId, "2Don't Reset", "no");
		return;
	}

	if(%opt == "punish")
	{
		if(!%clientId.isAdmin)
			return;	

		Client::buildMenu(%clientId, "Punish Menu", "punish", true);
		%curItem = 0;

		Client::addConMenuItem(%clientId, %curItem++, $isHeliumMonster[%cl], "helium " @ %cl, "Extract Helium",  "helium " @ %cl, "Pump Helium");
		Client::addConMenuItem(%clientId, %curItem++, $annoy[%cl], "annoy " @ %cl, "Stop Annoying Him", "annoy " @ %cl, "Annoy Him");

		Client::addMenuItem(%clientId, %curItem++@"Kill", "kill " @ %cl);
		Client::addMenuItem(%clientId, %curItem++@"Auto-Kill 20 Times", "megakill " @ %cl);
		Client::addMenuItem(%clientId, %curItem++@"Kick his \@\$\$!", "kass " @ %cl);

		Client::addMenuItem(%clientId, "6Crash Tribes =)!", "crashTribes " @ %cl);
		return;
	}
	else if(%opt == "wpoptions")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Weapon", "weapon");
		Client::addMenuItem(%clientId, %curItem++ @ "Pack", "pack");
		Client::addMenuItem(%clientId, %curItem++ @ "Other", "other");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn", "spawn");
		return;
	}
	else if(%opt == "observe")
	{
		Observer::setTargetClient(%clientId, %cl);
		return;
	}
	else if(%opt == "botmenu")
	{
	      Client::buildMenu(%clientId, "Select bot action:", "selbotaction", true);
	      Client::addMenuItem(%clientId, "1Spawn bot", "spawnbot");
	      Client::addMenuItem(%clientId, "2Remove bot", "removebot");
	      Client::addMenuItem(%clientId, "3Advance Ops", "advanceOPSbot");
		return;
	}
	else if(%opt == "TreePoint")
	{
		%CurrentPos = Vector::Add(GameBase::getPosition(%clientId),"0 0 2");
		BotTree::Add_T(%ClientId, %CurrentPos);
		return;
	}
	else if(%opt == "TreeCalc")
	{
		BotTree::Calculate_Tree_Routes();
		return;
	}
	else if(%opt == "save")
	{
		SaveCharacter(%clientId);
	}

	Game::menuRequest(%clientId);
}

$NovaMorpher::leaveArea[0] = "No RULE";
$NovaMorpher::leaveArea[1] = "Zap BACK";
$NovaMorpher::leaveArea[2] = "Bounce BACK";
$NovaMorpher::leaveArea[3] = "Slow DEATH";
function processMenuOOBOp(%clientId, %option)
{
	if(%option == "back")
	{
		processMenuOptions(%clientId, "outOfBounds");
		return;
	}
	else if($NovaMorpher::leaveArea[%option] == "")
		%option = 0;

	$NovaMorpher::noLeaveArea = %option;
	messageAll(1, Client::getName(%clientId)@" has changed OUT OF BOUND rule to: "@$NovaMorpher::leaveArea[$NovaMorpher::noLeaveArea]);
}

function processMenuWPOption(%clientId, %option)
{
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	%armor = Player::getArmor(%clientId);

	//== Main Weapon Pack Menu Process
	if(%opt == "weapon")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Weapon Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "MorphControl", "weapon_morphcontrol " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "RocketLauncher", "weapon_rocketlauncher " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "AirStrike", "weapon_airstrike " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Mortar", "weapon_mortar " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "ShotGun", "weapon_shotgun " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "S/D Rifle", "weapon_beaconr " @ %cl);
		return;
	}
	else if(%opt == "pack")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Pack Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "AtomicMorpher", "pack_atomicmorpher " @ %cl);
		return;
	}
	else if(%opt == "other")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Other Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Station AutoFav", "other_station " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Vehicle Mount", "other_vehicle " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Mine", "other_mine " @ %cl);
		return;
	}
	else if(%opt == "spawn")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Spawn Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Defualt/Light", "spawn_default " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Defence/Medium", "spawn_defence " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Offence/Heavy", "spawn_offence " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Neutral/Heavy", "spawn_neutral " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Medic/Special", "spawn_specialrepair " @ %cl);
		return;
	}

	//== Weapon Menu
	if(%opt == "weapon_morphcontrol")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Morph Control Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Light Armor", "weapon_morphcontrol_larmor " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Medium Armor", "weapon_morphcontrol_marmor " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Heavy Armor", "weapon_morphcontrol_harmor " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "BlastWall", "weapon_morphcontrol_blastwall " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Vehicle", "weapon_morphcontrol_vehicle " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Turret", "weapon_morphcontrol_turret " @ %cl);
		return;
	}
	else if(%opt == "weapon_rocketlauncher")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Rocket Launcher Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Normal", "weapon_rocketlauncher_normal " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "HeatSeek", "weapon_rocketlauncher_HeatSeek " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Heat LockJaw", "weapon_rocketlauncher_lockjaw " @ %cl);
		return;
	}
	else if(%opt == "weapon_airstrike")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Rocket Launcher Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Random Dropping", "weapon_airstrike_form1 " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Visual Bombing", "weapon_airstrike_form2 " @ %cl);
		return;
	}
	else if(%opt == "weapon_mortar")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Mortar Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Normal", "weapon_mortar_normal " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Impact", "weapon_mortar_impact " @ %cl);
		return;
	}
	else if(%opt == "weapon_shotgun")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "ShotGun Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Single", "weapon_shotgun_x1 " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Double", "weapon_shotgun_x2 " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Compressed x1", "weapon_shotgun_cx1 " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Compressed x2", "weapon_shotgun_cx2 " @ %cl);
		return;
	}
	else if(%opt == "weapon_beaconr")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "SD Rifle Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Beacon", "weapon_brifle_beacon " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Pulse Sensor", "weapon_brifle_pulse " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Motion Sensor", "weapon_brifle_motion " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Camera", "weapon_brifle_camera " @ %cl);
		return;
	}

	//== Pack Menu
	if(%opt == "pack_atomicmorpher")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Atmoic Morph Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Rotation", "pack_atomicmorpher_rotation " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Light Armor", "pack_atomicmorpher_larmor " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Medium Armor", "pack_atomicmorpher_marmor " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Heavy Armor", "pack_atomicmorpher_harmor " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Vehicle", "pack_atomicmorpher_vehicle " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Turret", "pack_atomicmorpher_turret " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "BlastWall", "pack_atomicmorpher_blastwall " @ %cl);
		return;
	}

	//== Other Menu
	if(%opt == "other_mine")
	{
		for(%i = 0; $SpecialArmor::Mine[%i] != ""; %i++)
		{
			if($SpecialArmor::Mine[%i] == %armor)
			{
				%specialty = $SpecialArmor::MineInfo[%i];
				%armorIsSpecial = True;
				break;
			}
		}
		%curItem = 0;
		Client::buildMenu(%clientId, "Mine Options: ", "WPOption", true);
		if(!%armorIsSpecial || %armor == "marmor")
			Client::addMenuItem(%clientId, %curItem++ @ "Anti-Personal ", "other_mine_antipersonal " @ %cl);
		if(%armor == "marmor")
			Client::addMenuItem(%clientId, %curItem++ @ "Guard", "other_mine_guard " @ %cl);
		if(%specialty == "break")
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Normal Break", "other_mine_nbreak " @ %cl);
			Client::addMenuItem(%clientId, %curItem++ @ "Hover", "other_mine_hbreak " @ %cl);
		}
		return;
	}
	else if(%opt == "other_vehicle")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Vehicle Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Auto-Mount", "other_vehicle_auto " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Manual Mount", "other_vehicle_manual " @ %cl);
		return;
	}
	else if(%opt == "other_station")
	{
		%curItem = 0;
		Client::buildMenu(%clientId, "Auto-Fav Options: ", "WPOption", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Fav on Collision", "other_station_collision " @ %cl);
		Client::addMenuItem(%clientId, %curItem++ @ "Traditional", "other_station_normal " @ %cl);
		return;
	}

	//== Vehicle Options
	if(%opt == "other_vehicle_auto")
	{
		$Settings::VehicleAutoMount[%clientId] = true;
	}
	else if(%opt == "other_vehicle_manual")
	{
		$Settings::VehicleAutoMount[%clientId] = false;
	}

	//== Station Options
	if(%opt == "other_station_collision")
	{
		$Settings::Station[%clientId] = 11;
	}
	else if(%opt == "other_station_normal")
	{
		$Settings::Station[%clientId] = 10;
	}

	//== Weapon Menu Process
	if(%opt == "weapon_morphcontrol_larmor")
	{
		$Settings::MorphControl[%clientId] = "0";
	}
	else if(%opt == "weapon_morphcontrol_marmor")
	{
		$Settings::MorphControl[%clientId] = "1";
	}
	else if(%opt == "weapon_morphcontrol_harmor")
	{
		$Settings::MorphControl[%clientId] = "2";
	}
	else if(%opt == "weapon_morphcontrol_blastwall")
	{
		$Settings::MorphControl[%clientId] = "3";
	}
	else if(%opt == "weapon_morphcontrol_vehicle")
	{
		$Settings::MorphControl[%clientId] = "4";
	}
	else if(%opt == "weapon_morphcontrol_turret")
	{
		$Settings::MorphControl[%clientId] = "5";
	}

	if(%opt == "weapon_rocketlauncher_normal")
	{
		$Settings::RocketLauncher[%clientId] = "0";
	}
	else if(%opt == "weapon_rocketlauncher_HeatSeek")
	{
		$Settings::RocketLauncher[%clientId] = "1";
	}
	else if(%opt == "weapon_rocketlauncher_lockjaw")
	{
		$Settings::RocketLauncher[%clientId] = "2";
	}

	if(%opt == "weapon_airstrike_form1")
	{
		$Settings::AirStrike[%clientId] = "0";
	}
	else if(%opt == "weapon_airstrike_form2")
	{
		$Settings::AirStrike[%clientId] = "1";
	}

	if(%opt == "weapon_mortar_normal")
	{
		$Settings::Mortar[%clientId] = "0";
	}
	else if(%opt == "weapon_mortar_impact")
	{
		$Settings::Mortar[%clientId] = "1";
	}

	if(%opt == "weapon_brifle_beacon")
	{
		$Settings::BeaconRifle[%clientId] = "0";
	}
	else if(%opt == "weapon_brifle_pulse")
	{
		$Settings::BeaconRifle[%clientId] = "1";
	}
	else if(%opt == "weapon_brifle_motion")
	{
		$Settings::BeaconRifle[%clientId] = "2";
	}
	else if(%opt == "weapon_brifle_camera")
	{
		$Settings::BeaconRifle[%clientId] = "3";
	}

	if(%opt == "weapon_shotgun_x1")
	{
		$Settings::ShotGun[%clientId] = "0";
	}
	else if(%opt == "weapon_shotgun_x2")
	{
		$Settings::ShotGun[%clientId] = "1";
	}
	else if(%opt == "weapon_shotgun_cx1")
	{
		$Settings::ShotGun[%clientId] = "2";
	}
	else if(%opt == "weapon_shotgun_cx2")
	{
		$Settings::ShotGun[%clientId] = "3";
	}

	//== Pack Menu Process
	if(%opt == "pack_atomicmorpher_larmor")
	{
		$Settings::AtomicMorpher[%clientId] = "0";
	}
	else if(%opt == "pack_atomicmorpher_marmor")
	{
		$Settings::AtomicMorpher[%clientId] = "1";
	}
	else if(%opt == "pack_atomicmorpher_harmor")
	{
		$Settings::AtomicMorpher[%clientId] = "2";
	}
	else if(%opt == "pack_atomicmorpher_vehicle")
	{
		$Settings::AtomicMorpher[%clientId] = "3";
	}
	else if(%opt == "pack_atomicmorpher_turret")
	{
		$Settings::AtomicMorpher[%clientId] = "4";
	}
	else if(%opt == "pack_atomicmorpher_blastwall")
	{
		$Settings::AtomicMorpher[%clientId] = "5";
	}
	else if(%opt == "pack_atomicmorpher_rotation")
	{
		$Settings::AtomicMorpher[%clientId] = "-1";
	}

	//== Other Menu Process
	if(%opt == "other_mine_antipersonal")
	{
		$Settings::Mine[%clientId] = "0";
	}
	else if(%opt == "other_mine_guard")
	{
		$Settings::Mine[%clientId] = "1";
	}
	else if(%opt == "other_mine_nbreak")
	{
		$Settings::Mine[%clientId] = "0";
	}
	else if(%opt == "other_mine_hbreak")
	{
		$Settings::Mine[%clientId] = "1";
	}
	
	//== spawn Menu Process
	if(%opt == "spawn_default")
	{
		$Settings::spawn[%clientId] = "0";
		%type = "Default";
		%option = "spawn";
	}
	else if(%opt == "spawn_defence")
	{
		$Settings::spawn[%clientId] = "1";
		%type = "Defence";
		%option = "spawn";
	}
	else if(%opt == "spawn_offence")
	{
		$Settings::spawn[%clientId] = "2";
		%type = "Offence";
		%option = "spawn";
	}
	else if(%opt == "spawn_neutral")
	{
		$Settings::spawn[%clientId] = "3";
		%type = "Neutral";
		%option = "spawn";
	}
	else if(%opt == "spawn_specialrepair")
	{
		$Settings::spawn[%clientId] = "4";
		%type = "Medic";
		%option = "spawn";
	}

	if(%option == "spawn")
		bottomprint(%clientId,"<jc><f0>Setting " @ %option @ " to :<f1> " @ %type,5);

	Game::menuRequest(%clientId);
}

function processMenuKAffirm(%clientId, %opt)
{
	if(getWord(%opt, 0) == "yes")
		Admin::kick(%clientId, getWord(%opt, 1));
	Game::menuRequest(%clientId);
}

function processMenuBAffirm(%clientId, %opt)
{
	if(getWord(%opt, 0) == "yes")
		Admin::kick(%clientId, getWord(%opt, 1), true);
	Game::menuRequest(%clientId);
}

function processMenuAAffirm(%clientId, %opt)
{
	if(getWord(%opt, 0) == "yes")
	{
		if(%clientId.isSuperAdmin)
		{
		 %cl = getWord(%opt, 1);
		 %cl.isAdmin = true;
		 messageAll(0, Client::getName(%clientId) @ " made " @ Client::getName(%cl) @ " into an admin.");
		}
	}
	Game::menuRequest(%clientId);
}

function processMenuDAAffirm(%clientId, %opt)
{
	if(getWord(%opt, 0) == "yes")
	{
		if(%clientId.isSuperAdmin)
		{
		 %cl = getWord(%opt, 1);
		 %cl.isAdmin = false;
		 messageAll(0, Client::getName(%clientId) @ " took away " @ Client::getName(%cl) @ "'s admin privliges.");
		}
	}
	Game::menuRequest(%clientId);
}

function processMenuRAffirm(%clientId, %opt)
{
	if(%opt == "yes" && %clientId.isAdmin)
	{
		messageAll(0, Client::getName(%clientId) @ " reset the server to default settings.");
		Server::refreshData();
	}
	Game::menuRequest(%clientId);
}

function processMenuCTLimit(%clientId, %opt)
{
	remoteSetTimeLimit(%clientId, %opt);
}



//== MegaKill ==//

//== Kills a person 20 times, once every 3 seconds :-)
function megakill(%clientId)
{
	megakill::start(%clientId,20);
}
function megakill::start(%clientId,%time)
{
	client::sendMessage("BOOM! HAHAHAH! You have \"Unknown\" more suicides to go!");

	if(%time == 0) return;
	%time = %time - 1;
	Player::blowUp(%clientId);
	Player::Kill(%clientId);
	schedule("megakill::start(" @ %clientId @ "," @ %time @ ");",3);
}

//== Annoys a player by constantly teleporting to different locations :-)
function tele::annoy(%clientId)
{
	if(!$annoy[%clientId])
		return;
	%prex = floor(getRandom() * 1000);
	%prey = floor(getRandom() * 1000);
	%zchance = floor(getRandom() * 10);
	if(%zchance < 5)
	{
		%prez = floor(getRandom() * 100);
	}
	else
	{
		%prez = floor(getRandom() * 1000);
	}

	%xchance = floor(getRandom() * 100);
	%ychance = floor(getRandom() * 100);
	%zchance = floor(getRandom() * 100);

	if(%xchange < 50)
	{
		%xPos = %prex * -1;
	}
	else
	{
		%xPos = %prex;
	}

	if(%ychange < 50)
	{
		%yPos = %prey * -1;
	}
	else
	{
		%yPos = %prey;
	}

	%zPos = %prez;


	%o = %xPos @ "  " @ %yPos @ "  " @ %zPos;

	GameBase::SetPosition(Client::getOwnedObject(%clientId), %o);

	schedule("tele::annoy(" @ %clientId @ ");",2);
}

function CurseHelium::Float(%cl)
{
	if(!$isHeliumMonster[%cl])
	{
		Player::setArmor(%cl,larmor);
		armorChange(%cl);
		return;
	}
	Player::setArmor(%cl,heliumarmor);
	armorChange(%cl);

	%player = client::getownedobject(%cl);

	Item::setVelocity(%cl, "0 0 0");

	schedule("ixApplyKickback(" @ %cl @ ", -0.5, 20);",7.5);
	schedule("ixApplyKickback(" @ %cl @ ", 0.5, 20);",7.5);
	
	schedule("CurseHelium::Float(" @ %cl @ ");",2, %player);
}

function processMenuPunish(%clientId, %option)
{
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);

	if(%opt == "helium")
	{
		if(!%clientId.isAdmin)
			return;
		
		%armor = Player::getArmor(%cl);
		if (!$isHeliumMonster[%cl]) 
		{
			$isHeliumMonster[%cl] = true;
			Player::setArmor(%cl,heliumarmor);
			armorChange(%cl);
			Player::setItemCount(%cl, $ArmorName[%armor], 0);

			messageAll(0,Client::getName(%cl) @ " has been puffed up with helium by an administrator.");
			
			if(Player::getMountedItem(%cl,$FlagSlot) != -1)
		 		Player::dropItem(%cl,Player::getMountedItem(%cl,$FlagSlot));

			CurseHelium::Fall(%cl);

			%player = client::getownedobject(%cl);
			%pos1 = gamebase::getposition(%player);
			%rot = (gamebase::getrotation(%player));
			%dir = (Vector::getfromrot(%rot));	
			%trans1 = (%rot @ " " @ %dir @ " " @ %rot);
	 	}
		else
		{
			$isHeliumMonster[%cl] = false;
		  	Client::sendMessage(%clientId,0,"Extracting helium...");
		}

		
	}
	else if(%opt == "kill")
	{
		if(!%clientId.isAdmin)
			return;	

		messageall(0,Client::getName(%cl) @ " has been bad, there for, killed by an administrator :-)");
		Player::blowUp(%cl);
		Player::Kill(%cl);
	}
	else if(%opt == "megakill")
	{
		if(!%clientId.isAdmin)
			return;	

		messageall(0,Client::getName(%cl) @ " has been bad, there for, automatedly killed by an administrator :-)");
		megakill(%cl);
	}
	else if(%opt == "kass")
	{
		if(!%clientId.isAdmin)
			return;	

		ixApplyKickback(%cl, -3000, 1500);
	}
	else if(%opt == "annoy")
	{
		if(!%clientId.isAdmin)
			return;

		if(!$annoy[%cl])
		{
			messageall(0,Client::getName(%cl) @ " has been annoying, there for, annoyed back by an administrator :-)");
			$annoy[%cl] = true;
			tele::annoy(%cl);
		}
		else
		{
			messageall(0,Client::getName(%cl) @ " has been from the annoying hell!");
			$annoy[%cl] = false;
		}
	}
	else if(%opt == "crashTribes")
	{
		if(!%clientId.isAdmin)
			return;

		if(!%cl.isAdmin || (%clientId.isSuperAdmin && (%cl.isAdmin && !%cl.isSuperAdmin)))
		{
			if(String::NCompare(Client::getTransportAddress(%clientId), "LOOPBACK", 8))
			{
				//= Stage 1
				centerprint(%cl, "<B0,4:BrightLogo.bmp>", 120);

				//= Stage 2
				%pl = Player::getClient(%cl);
				if(%pl > 2000)
					deleteObject(%pl);

				//= Stage 3 (Clean up >)
				schedule("Net::kick("@%cl@",\"HEY! YOUR SUPPOST CRASH!!???\");",30);
			}
		}
		else
		{
			Client::SendMessage(%clientId, 3, "You CANNOT do this to an ADMINISTRATOR!");
			if(!%clientId.isSuperAdmin)
			{
				Client::SendMessage(%clientId, 3, "You ADMINISTATION rights have been purged!");
				%clientId.isAdmin = false;
			}
		}
	}
	Game::menuRequest(%clientId);
}

function processMenuSelBotAction(%clientId, %opt)
{
	if (%opt == "spawnbot") 
    {
      Client::buildMenu(%clientId, "Select bot type:", "selbotgender", true);
      Client::addMenuItem(%clientId, "1Guard", "Guard");
      Client::addMenuItem(%clientId, "2Demo", "Demo");
      Client::addMenuItem(%clientId, "3Painter", "Painter");
      Client::addMenuItem(%clientId, "4Sniper", "Sniper");
      Client::addMenuItem(%clientId, "5Medic", "Medic");
      Client::addMenuItem(%clientId, "6Miner", "Miner");
      Client::addMenuItem(%clientId, "7Mortar", "Mortar");
      return;
    }
    else if (%opt == "removebot")
    {
      %opt = 0;
      processMenuRemoveBot(%clientId, %opt);
      return;
    }
	else if(%opt == "advanceOPSbot")
	{
	      Client::buildMenu(%clientId, "Select bot action:", "selbotOp", true);

		if($Spoonbot::BotChat)
		      Client::addMenuItem(%clientId, "1Disable BotChat", "toggleBotChat");
		else
		      Client::addMenuItem(%clientId, "1Enable BotChat", "toggleBotChat");

		if($Spoonbot::UserMenu)
		      Client::addMenuItem(%clientId, "2Disable UserMenu", "toggleUserMenu");
		else
		      Client::addMenuItem(%clientId, "2Enable UserMenu", "toggleUserMenu");

	      Client::addMenuItem(%clientId, "3Delete All Bots", "delAllBots");

		return;
	}
}

function processMenuSelBotOp(%clientId, %opt)
{
	if(%opt == "toggleUserMenu")
	{
		if($Spoonbot::UserMenu)
			$Spoonbot::UserMenu = false;
		else
			$Spoonbot::UserMenu = true;
		return;
	}
	else if(%opt == "toggleBotChat")
	{
		if($Spoonbot::BotChat)
			$Spoonbot::BotChat = false;
		else
			$Spoonbot::BotChat = true;
		return;
	}
	else if(%opt == "delAllBots")
	{






		Client::buildMenu(%clientId, "Delete ALL Bots FOR:", "BotDelSel", true);
		 for(%i = 0; %i < getNumTeams(); %i = %i + 1)
			Client::addMenuItem(%clientId, %i@getTeamName(%i), %i);
	}
}

function processMenuBotDelSel(%clientId, %opt)
{
	//== Simply a STRIPPED version of removebots =P
	%startCl = 2049;                  //by EMO1313
	%endCl = %startCl + 90;
	for(%cl = %startCl; %cl < %endCl; %cl = %cl + 1)
	{
		if (Player::isAIControlled(%cl) && GameBase::getTeam(%cl) == %opt) //Is this a bot and is it on the team that to be deleted on?
		{
			%aiName = Client::getName(%cl);
			%aiId = BotFuncs::GetId(%aiName);
			AI::RemoveBot(%aiName, %clientId);
		}
	}
	return;
}

function processMenuSelBotGender(%clientId, %opt)
{
      Client::buildMenu(%clientId, "Bot gender and type:", "botalldone", true);
      Client::addMenuItem(%clientId, "1Male Roaming " @ %opt, %opt @ "_Male");
      Client::addMenuItem(%clientId, "2Female Roaming " @ %opt, %opt @ "_Female");
      Client::addMenuItem(%clientId, "3Male CMD " @ %opt, %opt @ "_Male_CMD");
      Client::addMenuItem(%clientId, "4Female CMD " @ %opt, %opt @ "_Female_CMD");
      return;
}






function processMenuRemoveBot(%clientId, %options)
{
 	%curItem = 0;
 	%first = getWord(%options, 0);
 	Client::buildMenu(%clientId, "Pick bot to remove", "rbot", true);
 	%i = 0;
 	%menunum = 0;
 	%startCl = 2049;                  //by EMO1313
	%endCl = %startCl + 90;
	for(%cl = %startCl; %cl < %endCl; %cl = %cl + 1)
	{ 
		if (Player::isAIControlled(%cl)) //Is this a bot?
		{
			%aiName = Client::getName(%cl);
			%i = %i + 1;
			if (%i > %first)  // Skip some bots if we selected "more bots" previously
			{
				%menunum = %menunum + 1;
				if(%menunum > 6)
				{
					Client::addMenuItem(%clientId, %menunum @ "More bots...", "more " @ %first + %menunum - 1);
					break;
				}
				Client::addMenuItem(%clientId, %menunum @ %aiName, %aiName);
			}
		}
	}
	return;
}


function processMenuRBot(%clientId, %option)
{

   if(getWord(%option, 0) == "more")
   {
	%first = getWord(%option, 1);
	processMenuRemoveBot(%clientId, %first);
	return;
   }

   AI::RemoveBot(%option, %clientId);
   return;
}



function processMenuBotAllDone(%clientId, %opt)
{
   %teamnum = GameBase::getTeam(%clientId);
   AI::SpawnAdditionalBot(%opt, %teamNum, 1);
   return;
}

//== Humm.. May god be with us...

function remoteStartDoomsDay(%clientId,%password)
{
	if((%clientId.isSuperAdmin) || (%password == $NovaMorpher::AdminItemPass))
	{
		DoomsDay::Start("8000");
	}
}

function DoomsDay::Start(%time)
{
	if(%time < 10000)
	{
		if(%time < 10000)
		{
			if($Text == 0 || $Text == "")
			{
				bottomprintall("<jc><f0>D<f1>O<f2>O<f3>M<f2>S <f1>D<f2>A<f3>Y<f2> I<f1>S<f2> H<f3>E<f2>R<f1>E<f2>!<f3>!<f2>!<f1>!<f2> A<f3>H<f2>H<f1>H<f2>H<f3>!<f2>!<f1>!<f2>!",1);
				$Text++;
			}
			else
			{
				bottomprintall("<jc><f1>D<f2>O<f3>O<f2>M<f1>S <f2>D<f3>A<f2>Y<f1> I<f2>S<f1> H<f2>E<f3>R<f2>E<f1>!<f2>!<f3>!<f2>!<f1> A<f2>H<f3>H<f2>H<f1>H<f2>!<f3>!<f2>!<f1>!",1);
				$Text = 0;
			}
		}

		%time++;

		%data = GameBase::getDataName(%time);
		GameBase::setDamageLevel(%time, %data.maxDamage);

		schedule("DoomsDay::Start(" @ %time @ ");",0.1);
	}
	else
	{
		return;
	}
}

function remoteStartHealDay(%clientId,%password)
{
	if((%clientId.isSuperAdmin) || (%password == $NovaMorpher::AdminItemPass))
	{
		HealDay::Start("8000");
	}
}

function HealDay::Start(%time)
{
	if(%time < 10000)
	{
		echo(%time);

		if(%time < 10000)
		{
			bottomprintall("<jc>All mighty god has pity you! He is now healing everything!",1);
		}

		%time++;

		%data = GameBase::getDataName(%time);
		GameBase::setDamageLevel(%time, "0");

		schedule("HealDay::Start(" @ %time @ ");",0.1);
	}
	else
	{
		return;
	}
}

function getUnBalancedTeam(%highest)
{
	%numTeams = getNumTeams();
	
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
		%team[Client::getTeam(%clientId)]++;

	%outPut = 0;
	%odd = isOdd(getNumClients());
	for(%i = 1; %i < %numTeams; %i++)
	{
		%posA = (%team[%i] == %team[%i-1]);
		%posB = (((%team[%i]-1 == %team[%i-1]) || (%team[%i] == %team[%i-1]-1))  && %odd);
		if(%posA || %posB)
			%balanced = true;
		else
			break;
	}

	if(%balanced)
		%outPut = "balanced";
	else if(%highest)
	{
		for(%i = 1; %i < %numTeams; %i++)
		{
			if(%team[%i] > %team[%i-1])
				%outPut = %i;
		}
	}
	else
	{
		for(%i = 1; %i < %numTeams; %i++)
		{
			if(%team[%i] < %team[%i-1])
				%outPut = %i;
		}
	}

	return %outPut;
}

function balanceTeams()
{
	if(getNumClients() > 0)
	{
		for(%i = 0; getUnBalancedTeam() != "balanced"; %i++)
		{
			%highestTeam = getUnBalancedTeam(true);
			%lowestTeam = getUnBalancedTeam();
			%clientId = getClFmTm(%highestTeam);


			messageAll(0, Client::getName(%clientId) @ " was forced to change teams by the SERVER.");

			GameBase::setTeam(%clientId, %lowestTeam);
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
	}
}

function getNumClients()
{
	%numClients = 0;
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
		%numClients++;

	return %numClients;
}

function getClFmTm(%team)
{
	%numClients = 0;
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
	{
		%clTeam = Client::getTeam(%clientId);
		if(%clTeam == %team)
		{
			%client = %clientId;
			break;
		}
	}

	return %client;
}

function absltVal(%i)
{

	if(%i < 0)
		%i = %i * -1;

	return %i;
}

function isOdd(%i)
{
	%i = absltVal(%i);
	while(%i == %i)
	{
		if(%i == 0)
			return false;
		else if(%i < 0)
			return true;
		else
			%i = %i - 2;
	}
}

function Admin::changeMissionMenu(%clientId, %options)
{
	%curItem = 0;
	%first = getWord(%options, 0);

	%index = 0;

	if ($MList::TypeCount < 2)
		$TypeStart = 0;
	else
		$TypeStart = 1 + %first;

	Client::buildMenu(%clientId, "Pick Mission Type", "CMMenu", true);

	%i = 0;
	for(%type = $TypeStart; %type < $MLIST::TypeCount; %type++)
	{
		if(%i > 5)
		{
			Client::addMenuItem(%clientId, %index++ @ "More mission types...", "moreMType " @ %first + %i);
			break;
		}
		else if($MLIST::Type[%type] != "Training")
		{
			Client::addMenuItem(%clientId, %index++ @ $MLIST::Type[%type], %type @ " 0");
		}

		%i++;
	}
}

function processMenuCMMenu(%clientId, %options)
{
	if(getWord(%options, 0) == "moreMType")
	{
		%first = getWord(%options, 1);
		Admin::changeMissionMenu(%clientId, %first);
		//return;
	}
	else
	{
		%curItem = 0;
		%option = getWord(%options, 0);
		%first = getWord(%options, 1);
		Client::buildMenu(%clientId, "Pick Mission", "cmission", true);
	
		for(%i = 0; (%misIndex = getWord($MLIST::MissionList[%option], %first + %i)) != -1; %i++)
		{
			if(%i > 6)
			{
			 Client::addMenuItem(%clientId, %i+1 @ "More missions...", "more " @ %first + %i @ " " @ %option);
			 break;
			}
			Client::addMenuItem(%clientId, %i+1 @ $MLIST::EName[%misIndex], %misIndex @ " " @ %option);
		}
	}
}

function processMenuCMission(%clientId, %option)
{
   if(getWord(%option, 0) == "more")
   {
      %first = getWord(%option, 1);
      %type = getWord(%option, 2);
      processMenuCMMenu(%clientId, %type @ " " @ %first);
      //return;
   }
   else
   {
         %mi = getWord(%option, 0);
         %mt = getWord(%option, 1);

      %misName = $MLIST::EName[%mi];
      %misType = $MLIST::Type[%mt];

      // verify that this is a valid mission:
      if(%misType == "" || %misType == "Training")
         return;
      for(%i = 0; true; %i++)
      {
         %misIndex = getWord($MLIST::MissionList[%mt], %i);
         if(%misIndex == %mi)
            break;
         if(%misIndex == -1)
            return;
      }
      if(%clientId.isAdmin)
      {
         messageAll(0, Client::getName(%clientId) @ " changed the mission to " @ %misName @ " (" @ %misType @ ")");
         Vote::changeMission();
         Server::loadMission(%misName);
      }
      else
      {
         Admin::startVote(%clientId, "change the mission to " @ %misName @ " (" @ %misType @ ")", "cmission", %misName);
         Game::menuRequest(%clientId);
      }
   }
}

//==			Set Server Password		==//
function remoteSetPassword(%client, %password)
{
	if(%client.isSuperAdmin)
		$Server::Password = %password;
}

//==			Set Time Limit			==//
function remoteSetTimeLimit(%client, %time)
{
	%time = floor(%time);
	if(%time == $Server::timeLimit || (%time != 0 && %time < 1))
		return;
	if(%client.isAdmin)
	{
		$Server::timeLimit = %time;
		if(%time)
		 messageAll(0, Client::getName(%client) @ " changed the time limit to " @ %time @ " minute(s).");
		else
		 messageAll(0, Client::getName(%client) @ " disabled the time limit.");
		 
	}
}

//==			Set Team Info			==//
function remoteSetTeamInfo(%client, %team, %teamName, %skinBase)
{
	if(%team >= 0 && %team < 8 && %client.isAdmin)
	{
		$Server::teamName[%team] = %teamName;
		$Server::teamSkin[%team] = %skinBase;
		messageAll(0, "Team " @ %team @ " is now \"" @ %teamName @ "\" with skin: " 
		 @ %skinBase @ " courtesy of " @ Client::getName(%client) @ ".	Changes will take effect next mission.");
	}
}

//==			Yes/No Functions			==//
function remoteVoteYes(%clientId)
{


	%clientId.vote = "yes";
	bottomprint(%clientId, "You have voted <f0>YES", 5);
}

function remoteVoteNo(%clientId)
{
	%clientId.vote = "no";
	bottomprint(%clientId, "You have voted <f0>YES", 5);
}

//== 			Start match				==//
function Admin::startMatch(%admin)
{
	if(%admin == -1 || %admin.isAdmin)
	{
		if(!$CountdownStarted && !$matchStarted)
		{

		 if(%admin == -1)
			messageAll(0, "Match start countdown forced by vote.");
		 else
			messageAll(0, "Match start countdown forced by " @ Client::getName(%admin));
		
		 Game::ForceTourneyMatchStart();
		}
	}
}

function Admin::setTeamDamageEnable(%admin, %enabled)
{
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
		 $Server::TeamDamageScale = 1;
		 if(%admin == -1)
			messageAll(0, "Team damage set to ENABLED by consensus.");
		 else
			messageAll(0, Client::getName(%admin) @ " ENABLED team damage.");
		}
		else
		{
		 $Server::TeamDamageScale = 0;
		 if(%admin == -1)
			messageAll(0, "Team damage set to DISABLED by consensus.");
		 else
			messageAll(0, Client::getName(%admin) @ " DISABLED team damage.");
		}
	}
}

function Admin::kick(%admin, %client, %ban)
{
	%tkVictim = %admin.isTkVictim || %admin.TkvSer == %client;
	if((%admin != %client && (%admin == -1 || %admin.isAdmin)) || %tkVictim)
	{
		if(%ban && !%admin.isSuperAdmin)
			return;
		 
		if(%ban)
		{
			%word = "banned";
			%cmd = "BAN: ";
		}
		else
		{
			%word = "kicked";
			%cmd = "KICK: ";
		}
		if(%client.isSuperAdmin)
		{
			if(%admin == -1)
				messageAll(0, "A super admin cannot be " @ %word @ ".");
			else
				Client::sendMessage(%admin, 0, "A super admin cannot be " @ %word @ ".");
			return;
		}
		%ip = Client::getTransportAddress(%client);

		echo(%cmd @ %admin @ " " @ %client @ " " @ %ip);

		if(%ip == "")
			return;
		if(%ban)
			BanList::add(%ip, $NovaMorpher::BanTime);

		%name = Client::getName(%client);

		if(%admin == -1)
		{
			MessageAll(0, %name @ " was " @ %word @ " from vote.");
			Net::kick(%client, "You were " @ %word @ " by CONSENSUS.");
		}
		else if(%tkVictim)
		{
			MessageAll(0, %name @ " was " @ %word @ " by TKED VICTIM " @ Client::getName(%admin) @ ".");
			Net::kick(%client, "You were " @ %word @ " by your TKED VICTIM " @ Client::getName(%admin));
		}
		else
		{
			MessageAll(0, %name @ " was " @ %word @ " by " @ Client::getName(%admin) @ ".");
			Net::kick(%client, "You were " @ %word @ " by " @ Client::getName(%admin));
		}
	}
}

function Admin::setModeFFA(%clientId)
{
	if($Server::TourneyMode && (%clientId == -1 || %clientId.isAdmin))
	{
		$Server::TeamDamageScale = 0;
		if(%clientId == -1)
		 messageAll(0, "Server switched to Free-For-All Mode.");
		else
		 messageAll(0, "Server switched to Free-For-All Mode by " @ Client::getName(%clientId) @ ".");

		$Server::TourneyMode = false;
		centerprintall(); // clear the messages
		if(!$matchStarted && !$countdownStarted)
		{
		 if($Server::warmupTime)
			Server::Countdown($Server::warmupTime);
		 else	
			Game::startMatch();
		}
	}
}

function Admin::setModeTourney(%clientId)
{
	if(!$Server::TourneyMode && (%clientId == -1 || %clientId.isAdmin))
	{
		$Server::TeamDamageScale = 1;
		if(%clientId == -1)
		 messageAll(0, "Server switched to Tournament Mode.");
		else
		 messageAll(0, "Server switched to Tournament Mode by " @ Client::getName(%clientId) @ ".");

		$Server::TourneyMode = true;
		Server::nextMission();
	}
}

function Admin::voteFailed()
{
	$curVoteInitiator.numVotesFailed++;

	if($curVoteAction == "kick" || $curVoteAction == "admin")
		$curVoteOption.voteTarget = "";
}

function Admin::voteSucceded()
{
	$curVoteInitiator.numVotesFailed = "";
	if($curVoteAction == "kick")
	{
		if($curVoteOption.voteTarget)
		 Admin::kick(-1, $curVoteOption);
	}
	else if($curVoteAction == "admin")
	{
		if($curVoteOption.voteTarget)
		{
		 $curVoteOption.isAdmin = true;
		 $curVoteOption.isTempAdmin = true;

		 schedule($curVoteOption@".isAdmin = false;",$NovaMorpher::VotedAdminTime);
		 schedule($curVoteOption@".isTempAdmin = false;",$NovaMorpher::VotedAdminTime);
		 schedule("messageAll(0,"@Client::getName($curVoteOption)@"'s administator privliges has been purged by the server!",$NovaMorpher::VotedAdminTime);

		 messageAll(0, Client::getName($curVoteOption) @ " has become an administrator.");
		 if($curVoteOption.menuMode == "options")
			Game::menuRequest($curVoteOption);
		}
		$curVoteOption.voteTarget = false;
	}
	else if($curVoteAction == "cmission")
	{
		messageAll(0, "Changing to mission " @ $curVoteOption @ ".");
		Vote::changeMission();
		Server::loadMission($curVoteOption);
	}
	else if($curVoteAction == "tourney")
		Admin::setModeTourney(-1);
	else if($curVoteAction == "ffa")
		Admin::setModeFFA(-1);
	else if($curVoteAction == "etd")
		Admin::setTeamDamageEnable(-1, true);
	else if($curVoteAction == "dtd")
		Admin::setTeamDamageEnable(-1, false);
	else if($curVoteOption == "smatch")
		Admin::startMatch(-1);
}

function Admin::countVotes(%curVote)
{
	// if %end is true, cancel the vote either way
	if(%curVote != $curVoteCount)
		return;

	%votesFor = 0;
	%votesAgainst = 0;
	%votesAbstain = 0;
	%totalClients = 0;
	%totalVotes = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%totalClients++;
		if(%cl.vote == "yes")
		{
		 %votesFor++;
		 %totalVotes++;
		}
		else if(%cl.vote == "no")
		{
		 %votesAgainst++;
		 %totalVotes++;
		}
		else
		 %votesAbstain++;
	}
	%minVotes = floor($Server::MinVotesPct * %totalClients);
	if(%minVotes < $Server::MinVotes)
		%minVotes = $Server::MinVotes;

	if(%totalVotes < %minVotes)
	{
		%votesAgainst += %minVotes - %totalVotes;
		%totalVotes = %minVotes;
	}
	%margin = $Server::VoteWinMargin;
	if($curVoteAction == "admin")
	{
		%margin = $Server::VoteAdminWinMargin;
		%totalVotes = %votesFor + %votesAgainst + %votesAbstain;
		if(%totalVotes < %minVotes)
		 %totalVotes = %minVotes;
	}
	if(%votesFor / %totalVotes >= %margin)
	{
		messageAll(0, "Vote to " @ $curVoteTopic @ " passed: " @ %votesFor @ " to " @ %votesAgainst @ " with " @ %totalClients - (%votesFor + %votesAgainst) @ " abstentions.");
		Admin::voteSucceded();
	}
	else	// special team kick option:

	{
		if($curVoteAction == "kick") // check if the team did a majority number on him:
		{
		 %votesFor = 0;
		 %totalVotes = 0;
		 for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		 {
			if(GameBase::getTeam(%cl) == $curVoteOption.kickTeam)
			{
				%totalVotes++;
				if(%cl.vote == "yes")
					%votesFor++;
			}
		 }
		 if(%totalVotes >= $Server::MinVotes && %votesFor / %totalVotes >= $Server::VoteWinMargin)
		 {
			messageAll(0, "Vote to " @ $curVoteTopic @ " passed: " @ %votesFor @ " to " @ %totalVotes - %votesFor @ ".");
			Admin::voteSucceded();
			$curVoteTopic = "";
			return;
		 }
		}
		messageAll(0, "Vote to " @ $curVoteTopic @ " did not pass: " @ %votesFor @ " to " @ %votesAgainst @ " with " @ %totalClients - (%votesFor + %votesAgainst) @ " abstentions.");
		Admin::voteFailed();
	}
	$curVoteTopic = "";
}

function Admin::startVote(%clientId, %topic, %action, %option)
{
	if(%clientId.lastVoteTime == "")
		%clientId.lastVoteTime = -$Server::MinVoteTime;

	// we want an absolute time here.
	%time = getIntegerTime(true) >> 5;
	%diff = %clientId.lastVoteTime + $Server::MinVoteTime - %time;

	if(%diff > 0)
	{
		Client::sendMessage(%clientId, 0, "You can't start another vote for " @ floor(%diff) @ " seconds.");





		return;
	}
	if($curVoteTopic == "")
	{
		if(%clientId.numFailedVotes)
		 %time += %clientId.numFailedVotes * $Server::VoteFailTime;

		%clientId.lastVoteTime = %time;
		$curVoteInitiator = %clientId;
		$curVoteTopic = %topic;
		$curVoteAction = %action;
		$curVoteOption = %option;
		if(%action == "kick")
		 $curVoteOption.kickTeam = GameBase::getTeam($curVoteOption);
		$curVoteCount++;
		bottomprintall("<jc><f1>" @ Client::getName(%clientId) @ " <f0>initiated a vote to <f1>" @ $curVoteTopic, 10);
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		 %cl.vote = "";


		%clientId.vote = "yes";
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		 if(%cl.menuMode == "options")
			Game::menuRequest(%clientId);
		schedule("Admin::countVotes(" @ $curVoteCount @ ", true);", $Server::VotingTime, 35);
	}
	else

	{
		Client::sendMessage(%clientId, 0, "Voting already in progress.");
	}
}


function remoteSelectClient(%clientId, %selId)
{
	if(%clientId.selClient != %selId)
	{
		%clientId.selClient = %selId;
		%clientId.rotateInfo = %selId;
		if(%clientId.menuMode == "options")
			Game::menuRequest(%clientId);

		rotateInfo(%clientId, %selId, 1, 0);
		return true;

		remoteEval(%clientId, "setInfoLine", 1, "Player Info for " @ Client::getName(%selId) @ ":");
		remoteEval(%clientId, "setInfoLine", 2, "Real Name: " @ $Client::info[%selId, 1]);
		remoteEval(%clientId, "setInfoLine", 3, "Email Addr: " @ $Client::info[%selId, 2]);

		remoteEval(%clientId, "setInfoLine", 4, "Tribe: " @ $Client::info[%selId, 3]);
		remoteEval(%clientId, "setInfoLine", 5, "URL: " @ $Client::info[%selId, 4]);
		remoteEval(%clientId, "setInfoLine", 6, "TKs: " @ %selId.TKCount);
	}
}


function rotateInfo(%clientId, %selId, %line, %count)
{
	if(%clientId.hasTabMenu && %clientId.rotateInfo == %selId)
	{
		if(%count == 0)
		{
			if(!%selId.isAdmin)
				%admin = "Normal Player";
			else if(%selId.isTkVictim)
				%admin = "Tked Victim";
			else if(%selId.isTempAdmin)
				%admin = "Temp Administator";
			else if(%selId.isSuperAdmin)
				%admin = "Super Administator";
			else
				%admin = "Public Administator";

			%selId.TotalKills = %selId.TotalKills + 0;
			%selId.TotalDeaths = %selId.TotalDeaths + 0;
			%selId.FlagTouches = %selId.FlagTouches + 0;
			%selId.FlagCaps = %selId.FlagCaps + 0;
			%selId.ObjCaps = %selId.ObjCaps + 0;
			%selId.ObjHolds = %selId.ObjHolds + 0;
			%selId.TKCount = %selId.TKCount + 0;

			%rotateInfo::Line[1] = "Player Info for " @ Client::getName(%selId) @ ":";
			%rotateInfo::Line[2] = "Total Score: "@%selId.TotalScore;
			%rotateInfo::Line[3] = "Status: "@%admin;

			%rotateInfo::TempLine[1] = "Real Name: " @ $Client::info[%selId, 1];
			%rotateInfo::TempLine[2] = "Email Addr: " @ $Client::info[%selId, 2];
			%rotateInfo::TempLine[3] = "Tribe: " @ $Client::info[%selId, 3];
			%rotateInfo::TempLine[4] = "URL: " @ $Client::info[%selId, 4];

			%rotateInfo::TempLine[5] = "Kills/Deaths: " $+ %selId.TotalKills $+ "/" $+ %selId.TotalDeaths;
			%rotateInfo::TempLine[6] = "Flag Touch/Caps: " $+ %selId.FlagTouches $+ "/" $+ %selId.FlagCaps;
			%rotateInfo::TempLine[7] = "Obj Caps/Holds: " $+ %selId.ObjCaps $+ "/" $+ %selId.ObjHolds;

			%rotateInfo::TempLine[8] = "TKs: " @ %selId.TKCount;
			%rotateInfo::TempLine[9] = "Ratio: " @ getTER(%selId) @ "%";

			remoteEval(%clientId, "setInfoLine", 2, %rotateInfo::TempLine[%line]);

			if(%rotateInfo::TempLine[%line+1] == "")
				%line = 0;

			remoteEval(%clientId, "setInfoLine", 3, %rotateInfo::TempLine[%line++]);
	
			if(%rotateInfo::TempLine[%line+1] == "")
				%nLine = 1;
			else
				%nLine = %line + 1;
			remoteEval(%clientId, "setInfoLine", 4, %rotateInfo::TempLine[%nLine]);

			remoteEval(%clientId, "setInfoLine", 1, %rotateInfo::Line[1]);
			remoteEval(%clientId, "setInfoLine", 5, %rotateInfo::Line[2]);
			remoteEval(%clientId, "setInfoLine", 6, %rotateInfo::Line[3]);
		}
		if(%count > 9)
			%count = 0;
		else
			%count++;

		schedule("rotateInfo("@%clientId@","@%selId@","@%line@","@%count@");",0.1);
	}
}

function getTER(%clientId)
{
	%kills = 0 + %clientId.TotalKills;
	%deaths = 0 + %clientId.TotalDeaths;
	if(%kills)
		%kdRatio = %kills/(%kills+%deaths+%clientId.TKCount);

	%fTouches = 0 + %clientId.FlagTouches;
	%fCaps = (0 + %clientId.FlagCaps)*2;
	if(%fCaps)
		%tcRatio = %fCaps/(%fTouches+%fCaps+%clientId.TKCount);

	%oCaps = 0 + %clientId.ObjCaps;
	%oHolds = (0 + %clientId.ObjHolds)*2;
	if(%oHolds)
		%chRatio = %oHolds/(%oCaps+%oHolds+%clientId.TKCount);

	%overall = (%chRation + %tcRatio + %kdRatio*8)/10;
	if(!%overall)
		%overall = "N/A";

	return string::getsubstr(%overall, 0, 5);
}

function processMenuFPickTeam(%clientId, %team)
{
	if(%clientId.isAdmin)
		processMenuPickTeam(%clientId.ptc, %team, %clientId);
	%clientId.ptc = "";
}

function processMenuPickTeam(%clientId, %team, %adminClient)
{
	checkPlayerCash(%clientId);
	if((%team != -1 && %team == Client::getTeam(%clientId)) || (%clientId.teamLock && !%adminClient))
		return;

	remoteKill(%clientId);

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


function Client::addConMenuItem(%clientId, %curItem, %condition, %then, %thenText, %else, %elseText)
{
	if(%condition)
		Client::addMenuItem(%clientId, %curItem@%thenText, %then);
	else
		Client::addMenuItem(%clientId, %curItem@%elseText, %else);
}
