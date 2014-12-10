//== This has FULL credit toward shifter!! horray!!! :')
RepairEffectData HackBolt
{
   bitmapName       = "lightningNew.bmp";
   boltLength       = 10.0;
   segmentDivisions = 2;
   beamWidth        = 0.5;

   updateTime   = 900;
   skipPercent  = 1.2;
   displaceBias = 0.3;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundELFFire;
};

function HackBolt::onRelease(%this, %player)
{
	%client = Player::getClient(%player);	
	$hacking[%client] = "false";
}

function HackBolt::onAcquire(%this, %player, %target)
{
	%client = Player::getClient(%player);	
	if($NovaMorpher::AllowHacking)
	{
		if (%target != %player)
		{
			%player.repairTarget = %target;
			%name = GameBase::getMapName(%target);
			%team = GameBase::getTeam(%target);
			%pTeam = GameBase::getTeam(%player);
			%pName = Client::getName(%client);
			%tName = getTeamName(%team);

			if(%name == "")
			{
				%name = (GameBase::getDataName(%player.repairTarget)).description;
			}
	
			%item = GameBase::getDataName(%player.repairTarget);

			if(checkHackable(%name, %item))
			{
				if(%team == %pTeam)
				{
					if (%target.infected == "true")
					{
						Client::sendMessage(%client,0," Your team's " @ %name @ " is already protected from hacking.");	
						return;
					}
					else if(getObjectType(%player.repairTarget) != "Player")
					{
						$hacking[%client] = "true";
						if($origTeam[%target] == "")
						{
							//echo ("No original team set. Marking.");
							$origTeam[%target] = %team;
						}
	
						Client::sendMessage(%client,0,"Infecting " @ %name @ ". Please wait...");

						hackingItem(%target, %pTeam, %pName, %tName, %name, %team, $NovaMorpher::HackTime, %client);
						return;
					}
					return;
				}
				else
				{
					if(getObjectType(%player.repairTarget) != "Player")
					{
						$hacking[%client] = "true";
						if($origTeam[%target] == "")
						{
							//echo ("No original team set. Marking.");
							$origTeam[%target] = %team;
						}
	
						Client::sendMessage(%client,0,"Hacking  " @ %name @ ". Please wait...");

						hackingItem(%target, %pTeam, %pName, %tName, %name, %team, $NovaMorpher::HackTime, %client);
						return;
					}
				}
			}
		
			if(!checkHackable(%name, %item) || getObjectType(%player.repairTarget) == "Player")
			{
				if(getObjectType(%player.repairTarget) == "Player")
				{
					Client::sendMessage(%client,0,"You can not hack another player.");
				}
				else
				{
					Client::sendMessage(%client,0,"It is not possible to hack in to a " @ %name);
					return;
				}
			}
		}	
	}
	else
	{
		Client::sendMessage(%client,0,"Hacking/Infecting disabled on this server");
	}
}

function checkHackable(%name, %item)
{
	if($NoHack[%item])
		return false;
	else
		return true;
}

function hackingItem(%target, %pTeam, %pName, %tName, %name, %team, %time, %client)
{
	%shape = (GameBase::getDataName(%target)).shapeFile;
	
	if ($debug) echo  ("Hacking");
	
	if(%time > 0)
	{
		if($hacking[%client] == "true")
		{
			schedule("hackingItem('" @ %target @ "','" @ %pTeam @ "','" @ %pName @ "','" @ %tName @ "','" @ %name @ "','" @ %team @ "','" @ %time -1 @ "','" @ %client @ "');", 1);
			return;
		}
	}
	else if ((%time < 0 || %time == 0) && $hacking[%client] == "true")
	{

		if($hacking[%client] == "true")
		{
			if(%team == %pTeam)
			{
				if ($debug) echo ("infecting");
				%target.infected = "true";
				schedule ("playSound(TargetingMissile,GameBase::getPosition(" @ %target @ "));",0.1);
				Client::sendMessage(%client,1,"Your " @ %name @ " is now protected by viral infection, from hacking.");
				return;
			}
			else
			{
			
				if (%target.infected == "true")
				{
					%rnd = floor(getRandom() * 100);	
					if (%rnd > 50)
					{
						schedule ("playSound(TargetingMissile,GameBase::getPosition(" @ %target @ "));",0.2);
						Client::sendMessage(%client,1,"You disarm the protection virus in the " @ %name @ ", but the device had shocked you until you were electricuteds!!!");
						%player = Client::getOwnedObject(%client);
						Player::blowUp(%this);
						GameBase::applyDamage(%player,$FlashDamageType,999.0,GameBase::getPosition(%player),"0 0 0","0 0 0",%target);
						%target.infected = "false";
						return;
					}
					else
					{
						schedule ("playSound(TargetingMissile,GameBase::getPosition(" @ %target @ "));",0.2);
						Client::sendMessage(%client,1,"You safely disarm the protection virus in the " @ %name @ ".");					
						%target.infected = "false";
						return;
					}
				}

				TeamMessages(1, %pTeam, %pName @ " hacked into the " @ %tName @ "'s " @ %name @ "!");
				TeamMessages(1, %team, %pName @ " hacked into your teams " @ %name @ "!");
				GameBase::setTeam(%target,%pTeam);
					
				if($NovaMorpher::HackedTime > 0)
				{
					schedule("GameBase::setTeam('" @ %target @ "','" @ $origTeam[%target] @ "');", $NovaMorpher::HackedTime);
				}

				if(%target < $minHacked || $minHacked == -1)
				{
					$minHacked = %target;
				}
				if(%target > $maxHacked || $maxHacked == -1)
				{
					$maxHacked = %target;
				}
			}
		}
	}
}
