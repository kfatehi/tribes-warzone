function ipLog::saveList()
{
	messageAll(3,"Saving user/ip connection log. Please excuse the brief lag.~waccess_denied.wav");
	export("ipLog[*", "config\\userLog.cs");
}

function ipLog::addUser(%clientId)
{
	%id = $ipLog[count];
	if(!%id || %id < 1)
		%id = 0;

	%name = Client::getName(%clientId);
	%ip = ipLog::getIP(%clientId);
	//== General
	$ipLog[General@%id] = %name $+ "@" $+ %ip;
	//== Complex
	$ipLog[Cip@%id] = %ip;
	$ipLog[Cname@%id] = %name;
	//== Complex Alias
	%hIP = String::replace(%ip, ".", " ");
	%cIP = getWord(%hIP, 0) $+ "D" $+ getWord(%hIP, 1) $+ "D" $+ getWord(%hIP, 2) $+ "D" $+ getWord(%hIP, 3);
	%fIP = getWord(%hIP, 0) $+ "D" $+ getWord(%hIP, 1) $+ "D" $+ getWord(%hIP, 2);

	//== Far (2 nodes)
	for(%i = 0; %i < $ipLog[CFAliasCount@%fIP]; %i++)
	{
		if($ipLog[Cname@$ipLog[CFAlias@%fIP@"C"@%i]] == %name)
		{
			%has = true;
			break;
		}
	}
	if(!%has)
	{
		$ipLog[CFAlias@%fIP@"C"@$ipLog[CFAliasCount@%fIP]] = %id;
		$ipLog[CFAliasCount@%fIP]++;
	}
	//== Close (3 nodes)
	if(%has) //== If this person doesn't exist in a strict check, dont even bother ;)
	{
		%has = false;
		for(%i = 0; %i < $ipLog[CCAliasCount@%cIP]; %i++)
		{
			if($ipLog[Cname@$ipLog[CCAlias@%cIP@"C"@%i]] == %name)
			{
				%has = true;
				break;
			}
		}
	}
	if(!%has)
	{
		$ipLog[CCAlias@%cIP@"C"@$ipLog[CCAliasCount@%cIP]] = %id;
		$ipLog[CCAliasCount@%cIP]++;
	}
	//== Finish, increase ID count
	$ipLog[count]++;
}

function ipLog::getIP(%clientId)
{
	%ip = Client::getTransportAddress(%clientId);
	%ip = String::replace(%ip, ":", " ");
	%ip = getWord(%ip, 1);
	return %ip;
}

function ipLog::compIP(%a, %b)
{
	%aN = String::replace(%a, ".", " ");
	%bN = String::replace(%b, ".", " ");
	for(%i = 0; %i < 4; %i++)
	{
		%aC = getWord(%aN, %i);
		%bC = getWord(%bN, %i);
		if(%aC != "*" && %bC != "*" && %aC != %bC) //== Accept wild cards
			return %i+1;
	}
	return %i+1;
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
