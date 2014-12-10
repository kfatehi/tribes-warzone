function processMenuMMenu(%clientId, %options)
{
	%curItem = 0;
	%first = getWord(%options, 0);

	if(%first > 1)
		%index = %first;
	else
		%index = 1;

	Client::buildMenu(%clientId, "Pick Mission Type", "CMMenu", true);

	%i = 0;
	for(%type = $TypeStart; %type < $MLIST::TypeCount; %type++)
	{
		if(%i > 6)
		{
			Client::addMenuItem(%clientId, %i++ @ "More mission types...", "more " @ %first + %i);
			break;
		}
		else if($MLIST::Type[%type] != "Training")
		{
			Client::addMenuItem(%clientId, %index @ $MLIST::Type[%type], %type @ " 0");
		}

		Client::addMenuItem(%clientId, %i+1 @ $MLIST::EName[%misIndex], %misIndex @ " " @ %option);

		%i++;
	}
}

function processMenuCMMenu(%clientId, %options)
{
	if(getWord(%option, 0) == "more")
	{
		%first = getWord(%option, 1);
		processMenuMMenu(%clientId, %first);
		return;
	}
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
