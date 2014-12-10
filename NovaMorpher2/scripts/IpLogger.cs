function ipDebug(%message)
{
	if($ipDebug)
		echo("IpDebug: "@%message);
}

function getIP(%clientId)
{
	ipDebug("Formatting IP");
	%ip = Client::getTransportAddress(%clientId);
	%ip = String::replace(%ip, ":", " ");
	%ip = getWord(%ip, 1);
	return %ip;
}

$backUpCount = 0;
function addUser(%clientId)
{
	ipDebug("Adding user");
	%ip = getIP(%clientId);
	%name = Client::getName(%clientId);
	%name = replaceNum(%name);
	%hashName = hashname(%name);
	%checkExist = checkExist(%name, %ip);
	%Exist = getWord(%checkExist, 0);

	if(%Exist)
		if(aquireNicks(%name) == '>>"Error"<<')
			%Exist = false;

	addSecurityCon(%ip, %name);

	if(%Exist != 10)
	{
		if(%Exist >= 8)
		{
			%mainId = getWord(%checkExist, 1);
			%mainName = $NovaMorpher::ConLog[%mainId];
			%mainHashName = hashname(%mainName, 1);
			if($NovaMorpher::ConLogName2Id[%hashName] != %mainId)
			{
				$NovaMorpher::ConLogNick[%mainHashName] = $NovaMorpher::ConLogNick[%mainHashName] $+ ", " $+ %name;
				$NovaMorpher::ConLogName2Id[%hashName] = %mainId;
				$NovaMorpher::ConLogTimes[%hashName] = 0;
			}

			$NovaMorpher::ConLogTimes[%hashName]++;
			$NovaMorpher::ConLogAllTimes[%mainHashName]++;
			addNickCon(%clientId);
		}
		else if(%Exist == "Imposter")
		{
			if($NovaMorpher::ConLogIp2Name[hashname(%ip)] != "")
			{
				%mainId = getWord(%checkExist, 1);
				%mainName = $NovaMorpher::ConLog[%mainId];
				%mainHashName = hashname(%mainName);
				%name = "(IMPOSTER)" $+ %name;
				%hashName = hashname(%name);
				$NovaMorpher::ConLogNick[%mainHashName] = $NovaMorpher::ConLogNick[%mainHashName] $+ ", " $+ %name;
				$NovaMorpher::ConLogName2Id[%hashName] = %mainId;
				$NovaMorpher::ConLogTimes[%hashName]++;
			}
		}
		else
		{
			%newId = getOpenPlacement();
			if(getWord(%checkExist, 1) == "Imposter")
				%name = "(Imposter) "@%name;

			%hashName = hashname(%name);

			$NovaMorpher::ConLog[%newId] = %name;
			$NovaMorpher::ConLogName2Id[%hashName] = %newId;
			$NovaMorpher::ConLogIp[%hashName] = %ip;
			$NovaMorpher::ConLogIp2Name[hashname(%ip)] = %name;
			$NovaMorpher::ConLogNick[%hashName] = %name;
			$NovaMorpher::ConLogTimes[%hashName]++;
			$NovaMorpher::ConLogAllTimes[%hashName]++;
			addNickCon(%clientId);
		}
	}
	else if(%Exist == 10)
	{
		$NovaMorpher::ConLogTimes[%hashName]++;
		$NovaMorpher::ConLogAllTimes[%hashName]++;
	}

	if(%mainHashName != "")
		$NovaMorpher::ConLogLastIp[%mainHashName] = %ip;
	else
		$NovaMorpher::ConLogLastIp[%hashName] = %ip;

	if($backUpCount == 10)
	{
		$backUpCount = 0;
		updateLogFile();
	}
	else
		$backUpCount++;

	return true;
}

function addSecurityCon(%ip, %name)
{
	ipDebug("Adding security connection reference");
	%ipLength = String::len(%ip);
	%whiteSpaces = 17 - %ipLength;
	for(%i = 1; %i <= %whiteSpaces; %i++)
		%Spaces = %Spaces @ " ";

	$IncomingConnection = %ip@%Spaces@"-->  "@%name;
	export("IncomingConnection", "temp\\ConnectionLog.txt", true);
	$IncomingConnection = ""; //== Erase inputString
}

function outPutSpecInfo(%finderCl, %name)
{
	ipDebug("Outputting USER DATA");
	%id = aquirePossibleId(%name);
	if(getWord(%id,0) == "TMIDF")
	{
		Client::sendMessage(%finderCl, 1, "Too many results, please narrow your search.");
		Client::sendMessage(%finderCl, 2, "Possible Names:");
		for(%i = 1; getWord(%id, %i) != "-1"; %i++)
		{
			if(%text == 3)
				%text = 1;
			else
				%text = 3;

			%string = %i$+": "$+String::Replace(getWord(%id, %i),"\"123456789123456789123456789AA\""," ");
			%out = "Client::sendMessage("@%finderCl@","@%text@",'"@%string@"~wButton4.wav');";
			schedule(%out,%i);
		}
	}
	else if(%id == "NIDF")
	{
		Client::sendMessage(%finderCl, 1, "No connections with the name of \""@%name@"\" was found.");
	}
	else
	{
		%name = $NovaMorpher::ConLogAll[%id];
		%hashName = hashname(%name);
		%mainId = $NovaMorpher::ConLogName2Id[%hashName];
		%mainName = $NovaMorpher::ConLog[%mainId];
		%mainHashName = hashname(%mainName);
		%usedNicks = $NovaMorpher::ConLogNick[%mainHashName];
		%connectTime = $NovaMorpher::ConLogAllTimes[%mainHashName];
		%connectMTime = $NovaMorpher::ConLogTimes[%hashName];
		%ip = $NovaMorpher::ConLogLastIp[%mainHashName];
		Client::sendMessage(%finderCl, 0, " ");
		Client::sendMessage(%finderCl, 0, " ");
		Client::sendMessage(%finderCl, 1, "Looking up information on: "@%name);
		Client::sendMessage(%finderCl, 2, "Total Times Connected: "@%connectTime);
		Client::sendMessage(%finderCl, 2, "Alias Times Connected: "@%connectMTime);
		Client::sendMessage(%finderCl, 3, "Main Name: "@%mainName);
		Client::sendMessage(%finderCl, 3, "TRIBES Aliases: "@%usedNicks);
		Client::sendMessage(%finderCl, 1, "Last seen IP: "@%ip);
	}
}

function updateLogFile()
{
	schedule('export("NovaMorpher::ConLogIp*", "config\\NovaMorpherConLog.cs", false);'@
		   'export("NovaMorpher::ConLogName2Id*", "config\\NovaMorpherConLog.cs", true);'@
		   'export("NovaMorpher::ConLogNick*", "config\\NovaMorpherConLog.cs", true);'@
		   'export("NovaMorpher::ConLogAllTimes*", "config\\NovaMorpherConLog.cs", true);'@
		   'export("NovaMorpher::ConLogTimes*", "config\\NovaMorpherConLog.cs", true);'@
		   'export("NovaMorpher::ConLogLastIp*", "config\\NovaMorpherConLog.cs", true);'@
		   'export("NovaMorpher::ConLogIp2Name*", "config\\NovaMorpherConLog.cs", true);'@
		   'export("NovaMorpher::ConLogAll*", "config\\NovaMorpherConLog.cs", true);'@
		   'flushExportText();'@
		   'exec("NovaMorpherConLog.cs");'@
		   'echo("Notice: Updated log file!");',0.1);
}


function aquireNicks(%name)
{
	ipDebug("Aquiring nickname(s)");
	%hashName = hashname(%name);
	%mainId = $NovaMorpher::ConLogName2Id[%hashName];
	if(%mainId != "")
	{
		%mainName = $NovaMorpher::ConLog[%mainId];
		%mainHashName = hashname(%mainName);
		%nicks = $NovaMorpher::ConLogNick[%mainHashName];
		return %nicks;
	}
	else
	{
		return '>>"Error"<<';
	}
}

function aquirePossibleId(%name)

{
	ipDebug("Returning possible refrence IDs");
	for(%i = 0; $NovaMorpher::ConLogAll[%i] != ""; %i++)
	{
		%check = replaceNum($NovaMorpher::ConLogAll[%i]);
		if((String::findSubStr(%check, %name) != "-1"))
		{
			%Id = %i;
			if(!String::CICompare(%check, %name))
				%count++;

			%total = String::Replace(%check," ","\"123456789123456789123456789AA\"")$+" "$+%total;
		}
		if((String::findSubStr($NovaMorpher::ConLogAll[%i],%name) != "-1") && (String::findSubStr(%name,$NovaMorpher::ConLogAll[%i]) != "-1"))
			return %i;
	}
	if(%count == 1)
		return %Id;

	else if(%count > 1)
		return "TMIDF "$+%total;

	return "NIDF";
}

function replaceNum(%name)
{
	String::Replace(%name,".1","");
	String::Replace(%name,".2","");
	String::Replace(%name,".3","");
	String::Replace(%name,".4","");
	return %name;
}

function getOpenPlacement()
{
	ipDebug("Aquiring open slow for placement");
	for(%i = 0; $NovaMorpher::ConLog[%i] != ""; %i++){}
	return %i;
}

function checkExist(%name, %ip)
{
	ipDebug("Checking for existance");

	for(%i = 0; $NovaMorpher::ConLog[%i] != ""; %i++)
	{
		%curName = hashname($NovaMorpher::ConLog[%i]);
		%clHashName = hashname(%name);
		%curIp = $NovaMorpher::ConLogIp[%curName];
		%ipComp = compareFinalIP(%ip, %curIp);

		if(%curName == %clHashName)
		{
			if(%ipComp > 18)
				return 10;
			else
				%wrong = true;
		}
		else if(%curIp == %ip && !%wrong)
			return "9 " $+ %i;
		else if(%ipComp == 18 && !%wrong)
			return "8 " $+ %i;
		else if(%ipComp > 18 && %wrong)
			return "Imposter "$+%i;
	}
	if(%wrong)
		return false @ " Imposter";
	else
		return false;

}

function addNickCon(%clientId)
{
	%name = Client::getName(%clientId);
	if(!checkNickExist(%name))
	{
		%id = getOpenNickPlacement();
		$NovaMorpher::ConLogAll[%id] = %name;
	}
}

function getOpenNickPlacement()
{
	ipDebug("Aquiring open placement for LOGALL");
	for(%i = 0; $NovaMorpher::ConLogAll[%i] != ""; %i++){}
	return %i;
}

function checkNickExist(%name)

{
	ipDebug("Checking existance for LOGALL");
	for(%i = 0; $NovaMorpher::ConLogAll[%i] != ""; %i++)
	{
		%curName = hashname($NovaMorpher::ConLogAll[%i]);
		%clHashName = hashname(%name);
		if(%curName == %clHashName)
		{
			return true;
		}
	}
	return false;
}

function compareFinalIP(%clientIP, %checkIP)
{
	ipDebug("Comparing IP");
	%clientIP = String::replace(%clientIP, ".", " ");
	for(%i = 0; %i < 3; %i++)
		%clientIPNode[%i] = "IPNODE"$+getWord(%clientIP, %i);

	%checkIP = String::replace(%checkIP, ".", " ");
	for(%i = 0; %i < 3; %i++)
		%checkIPNode[%i] = "IPNODE"$+getWord(%checkIP, %i);

	%levelCor = 20;
	for(%i = 0; %i < 3; %i++)
	{
		if(%clientIPNode[%i] != %checkIPNode[%i] && %checkIPNode[%i] != "*")
			%levelCor -= 8-((%i+1)*2);
	}

	return %levelCor;
}

exec(NovaMorpherConLog);