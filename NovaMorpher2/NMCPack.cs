$NMCPack::Version = "0.01";			//== This pack version
$NMCPack::KeyBinds[Morpher] = "f12";	//== This specifies the key that toggles the morph pack option.
$NMCPack::KeyBinds[WepSet] = "f8";		//== This specifies the key that toggles the current weapon option


//=======================================================================//
//== Do not modify code below this unless you know what you are doing. ==//
//=======================================================================//
function NMCPack::CheckAll()
{
	for(%i=2049; %i<2100; %i++)
	{
		remoteEval(%i, CheckNMCPack);
	}

	NMCPack::SpecialSchedule("NMCPack::CheckAll", "NMCPack::CheckAll();", 600);
}

function remoteCheckNMCPack(%clientId)
{
	if(%clientId == 2048)
		echo("The server has requested your NMCPack version.");
	else
		echo("Client "@%clientId@" has requested your NMCPack version.");

	remoteEval(%clientId, hasNMCPack, $NMCPack::Version);
}

function remotehasNMCPack(%clientId)
{
	$NMCPack::ClientHasPack[%clientId] = true;
	NMCPack::SpecialSchedule(%clientId@"_DisablePack", "$NMCPack::ClientHasPack["@%clientId@"] = false;", 119.5);
	NMCPack::SpecialSchedule(%clientId@"_CheckPack", "remoteEval("@%clientId@", CheckNMCPack);", 120);
	NMCPack::SpecialSchedule(%clientId@"_StopChecks", "NMCPack::StopCheck("@%clientId@");", 130);
}

function NMC::StopCheck(%clientId)
{
	if(!$NMCPack::ClientHasPack[%clientId])
	{
		$NMCPack::SpecialSchedule[%clientId@"_DisablePack"] = false;
		$NMCPack::SpecialSchedule[%clientId@"_CheckPack"] = false;
		$NMCPack::SpecialSchedule[%clientId@"_StopChecks"] = false;
	}
}

function NMCPack::SpecialSchedule(%id, %command, %time)
{
	$NMCPack::SpecialSchedule[%id] = true;
	schedule("NMCPack::CarryOutCMD("@%id@","@%command@");",%time);
}

function NMCPack::CarryOutCMD(%id, %command)
{
	if($NMCPack::SpecialSchedule)
	{
		$NMCPack::SpecialSchedule[%id] = false;
		eval(%command);
	}
}

EditActionMap("actionMap.sae");
bindCommand(keyboard0, make, $NMCPack::KeyBinds[Morpher], TO, "remoteEval(2048, ToggleMorph);");
bindCommand(keyboard0, make, $NMCPack::KeyBinds[WepSet], TO, "remoteEval(2048, ToggleWepSetting);");