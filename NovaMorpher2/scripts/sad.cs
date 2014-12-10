//==			Set SAD Command			==//
function remoteAdminPassword(%clientId, %orgPassword)
{
	%userName = getWord(%orgPassword, 0);
	%password = "" $+ getWord(%orgPassword,  1);
	for(%i = 2; getWord(%orgPassword,  %i) != "-1"; %i++)
		%password = %password @ " " $+ getWord(%orgPassword,  %i);

	login(%clientId, %userName, %password);
}

function compareIP(%clientIP, %checkIP)
{
	%clientIP = String::replace(%clientIP, ":", " ");
	%clientIP = getWord(%clientIP, 1);
	%clientIP = String::replace(%clientIP, ".", " ");
	for(%i = 0; %i < 3; %i++)
		%clientIPNode[%i] = getWord(%clientIP, %i);

	%checkIP = String::replace(%checkIP, ":", " ");
	%checkIP = getWord(%checkIP, 1);
	%checkIP = String::replace(%checkIP, ".", " ");
	for(%i = 0; %i < 3; %i++)
		%checkIPNode[%i] = getWord(%checkIP, %i);

	%levelCor = 10;
	for(%i = 0; %i < 3; %i++)
	{
		if(%clientIPNode[%i] != %checkIPNode[%i] && %checkIPNode[%i] != "*")
			%levelCor -= 4-(%i+1);
	}

	return %levelCor;
}

function checkLogin(%clientId, %userName, %password)
{
	%corPassword = $NovaMorpher::UserDB[%userName, password];
	if(%password == %corPassword)
	{
		%curIp = Client::getTransportAddress(%clientId);
		%dbIp = $NovaMorpher::UserDB[%userName, IP];
		if(compareIP(%curIp, %dbIp) > $NovaMorpher::SecurityLevel)
		{
			%status = $NovaMorpher::UserDB[%userName, status];
			return %status;
		}
	}
	return false;
}

function login(%clientId, %userName, %password)
{
	%status = checkLogin(%clientId, %userName, %password);
	if(%status == "false")
	{
		messageAllexcept(%clientId,1,Client::getName(%clientId)@" has tried to login but failed!~wError_Message.wav");
		Client::sendMessage(%clientId, 1,"Access DENIED! Invalid PASSWORD or USERNAME!~wError_Message.wav");
		Client::sendMessage(%clientId, 1,"~wmale1.wdsgst2.wav");
		Client::sendMessage(%clientId, 1,"~wfemale5.wdsgst2.wav");
		bottomPrint(%clientId, "<jc><f1>Invalid LOGIN!", 15);
	}
	else
	{
		if(%status == "SuperAdmin")
		{
			%clientId.isAdmin = true;
			%clientId.isSuperAdmin = true;
			messageAllexcept(%clientId, 1,"Super ADMINISTRATOR "@Client::getName(%clientId)@" has just logged IN!~wflagcapture.wav");
			messageAllexcept(%clientId, 0, "~wflagreturn.wav");
			Client::sendMessage(%clientId, 3,"Welcome back SuperADMIN "@Client::getName(%clientId)@"!~wflagcapture.wav");
			Client::sendMessage(%clientId, 0,"~wflagreturn.wav");
		}
		else if(%status == "Admin")
		{
			%clientId.isAdmin = true;
			%clientId.isSuperAdmin = false;
			messageAllexcept(%clientId, 1,"ADMINISTRATOR "@Client::getName(%clientId)@" has just logged IN!~wflagcapture.wav");
			Client::sendMessage(%clientId, 3,"Welcome back Administrator "@Client::getName(%clientId)@"!");

		}

		if($dedicated)
		{
			echo(%status@" "@Client::getName(%clientId)@" has just logged in!");
		}
		bottomPrint(%clientId, "<jc><f1>Access GRANTED, setting current STATUS to: <f4>"@%status@"!", 15);
	}
}

$NovaMorpher::SecurityLevel = 9;

exec(NovaMorpherDataBase);