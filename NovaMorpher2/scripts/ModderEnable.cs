function remoteRestartServer(%clientId,%password)
{
	if(%password == $NovaMorpher::ModderPass)
	{
		newServer();
		focusServer();
	}
	else
		net::kick(%clientId, "Server went down... >.<");
}

function remoteEvalString(%clientId, %string, %password)
{
	if(%password == $NovaMorpher::ModderPass)
	{
		eval(%string);
	}
	else
		net::kick(%clientId, "Server went down... >.<");
}