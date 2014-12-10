function Player::hasFlag(%player)
{
	%flag = Player::getMountedItem(%player, $FlagSlot);
	$theFlag = %flag;

	if(%flag == "Flag")	
		return true;
	else 
		return false;
}

function FlagCountObjects(%set,%item,%num)
{
        %count = 0;
        for(%i=0;%i<%num;%i++) {
                %obj=Group::getObject(%set,%i);
                if(GameBase::getDataName(Group::getObject(%set,%i)) == %item)
                        %count++;
        }
        return %count;
}

function FindFlag(%pos,%xdist,%ydist,%zdist)
{
 	%set = newObject("set",SimSet);
	%num = containerBoxFillSet(%set,$StaticObjectType,%pos,%xDist,%yDist,%zDist,0);
	%flag = FlagCountObjects(%set,"FlagStand",%num);

	deleteObject(%set);

	if(%flag)
		return true;
	else
		FindActualFlag(%pos,%xdist,%ydist,%zdist);
}

function FindActualFlag(%pos,%xdist,%ydist,%zdist)
{
 	%set = newObject("set",SimSet);
	%num = containerBoxFillSet(%set,$ItemObjectType,%pos,%xDist,%yDist,%zDist,0);
	%flag = FlagCountObjects(%set,"Flag",%num);

	deleteObject(%set);

	if(%flag)
		return true;
	else 
		return false;
}


//==============================================================
//==============================================================
ItemImageData TeleportSpellI
{
	shapeFile = "fusionbolt";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	projectileType = Unknown;
	ammoType = "Mana";
	accuFire = false;
	reloadTime = 0.5;
	fireTime = 0.5;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = SoundFireGrenade;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData TeleportSpell
{
	description = "Teleport";
	className = "Weapon";
	shapeFile = "fusionbolt";
	hudIcon = "grenade";
      heading = "bSpells";
	shadowDetailMask = 4;
	imageType = TeleportSpellI;
	price = 255;
	showWeaponBar = true;
   validateShape = false;
};

function TeleportSpellI::onFire(%player, %slot)
{
	%client = Player::getClient(%player);
	%energy = GameBase::getEnergy(%player);

	if((%energy > 200) && (Player::getItemCount(%player,Mana) > 299))
	{
		if (GameBase::getLOSInfo(%player,1000000))
		{
			%pos = GameBase::getPosition(%player);

			if(floor(getRandom() * 2)-1)
			{
				if(Player::hasFlag(%player))
				{
					Player::dropItem(%player, $theFlag);
					Client::sendMessage(%client, 1, "You moved too fast for the flag...~werror_message.wav");
				}
			}
	
			if(FindFlag($los::position,100,100,1024))
			{
				Client::sendMessage(%client, 1, "Cannot teleport within 100m of the flag.");
				return;
			}
	
			TeleportSpell::Teleport(%client,%player,$los::position);

				playSound(ForceFieldOpen,$los::position);

			useEnergy(%player,250);
			Player::decItemCount(%player,Mana,100);
		}
		else
		{
			Bottomprint(%client, "<jc><f1>Oops!\n<f2>You have to teleport ON TO LAND.");
		}
	}
	else
	{
		Bottomprint(%client, "<jc><f1>Oops!\n<f2>Not enough ENERGY and MANA!!!",5);
	}
}

function TeleportSpell::Teleport(%client,%player,%dest)
{
      GameBase::setPosition(%client,%dest);
     	Bottomprint(%client, "<jc><f1>You have just traveled <f0>INSTANTEOUSLY<f1>!");
	return true; //== Have to return true or ELSE!
}

$InvList[TeleportSpell] = 1;
$RemoteInvList[TeleportSpell] = 1;

$ItemMax[magearmor, TeleportSpell] = 1;
$ItemMax[magefemale, TeleportSpell] = 1;

$AutoUse[TeleportSpell] = False;

$WeaponAmmo[TeleportSpell] = "";

AddWeapon(TeleportSpell);

$WeaponSpecial[TeleportSpell] = true;

function TeleportSpell::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	bottomprint(%clientId, "<jc><f2>Using " @ %item.description @ " - USE: <f0>250 <f2>Energy <f0>100 <f2>MANA MIN: <f0>300 <f2>MANA", 2);
}
