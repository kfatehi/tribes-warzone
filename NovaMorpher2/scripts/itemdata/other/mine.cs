$SpecialArmor::Mine[0] = "marmor";  //== Male armors are differ from females :-)
$SpecialArmor::MineInfo[0] = "partner";

$SpecialArmor::Mine[1] = "mfemale"; //== Male armors are differ from females :-)
$SpecialArmor::MineInfo[1] = "partner";

$SpecialArmor::Mine[2] = "magearmor";
$SpecialArmor::MineInfo[2] = "break";

$SpecialArmor::Mine[3] = "magefemale";
$SpecialArmor::MineInfo[3] = "break";

$SpecialArmor::Mine[4] = "larmor";
$SpecialArmor::MineInfo[4] = "spikes";

$SpecialArmor::Mine[5] = "lfemale";
$SpecialArmor::MineInfo[5] = "spikes";

ItemData MineAmmo
{
   description = "Mine";
   shapeFile = "mineammo";
   heading = "yMiscellany";
   shadowDetailMask = 4;
   price = 10;
	className = "HandAmmo";
};

function MineAmmo::onUse(%player,%item)
{
	%client = Player::getClient(%player);
	%armor = Player::getArmor(%client);
	for(%i = 0; $SpecialArmor::Mine[%i] != ""; %i++)
	{
		if($SpecialArmor::Mine[%i] == %armor)
		{
			%specialty = $SpecialArmor::MineInfo[%i];
			%armorIsSpecial = True;
			break;
		}
	}
	if($matchStarted)
	{
		if(%player.throwTime < getSimTime())
		{
			if(($Settings::Mine[%client] == "" || $Settings::Mine[%client] == "0") && !%armorIsSpecial) //== filter out the other kinds of armors
			{
				MineAmmo::AntiPersonelMine(%player,%item);
			}
			else if(($Settings::Mine[%client] == "1") && %specialty == "partner") //== Make sure ONLY this kind of armor can use this....
			{
				MineAmmo::SpwanPartner(%player,%item);
			}
			else if(($Settings::Mine[%client] == "0" || $Settings::Mine[%client] == "") && %specialty == "break") //== Only kind of mines mages can use, it is very useful if you find out a way to use it...
			{
				MineAmmo::applyBreaks(%player,%item);
				Player::setItemCount(%client,%item,1); //== Makes sure this person has 1 mine all the time, as long as he doesn't drop it
				return;
			}
			else if(($Settings::Mine[%client] == "1") && %specialty == "break") //== Only kind of mines mages can use, it is very useful if you find out a way to use it...
			{
				if(!$MineAmmo::Hover[%client] || $MineAmmo::Hover[%client] == "false")
				{
					if(Player::getItemCount(%player,Mana) > 100)
					{
						Client::SendMessage(%client,1,"Engaging Hover!");
						$MineAmmo::Hover[%client] = "True";
						MineAmmo::Hover(%player,%item);
						Player::decItemCount(%player,Mana,100);
					}
					else
					{
						Client::SendMessage(%client,1,"Not enough mana to cast the spell! You need at least 100 mana to cast this spell");
					}
				}
				else
				{
					$MineAmmo::Hover[%client] = "False";
				}
				Player::setItemCount(%client,%item,1); //== Makes sure this person has 1 mine all the time, as long as he doesn't drop it
				return;
			}
			else if(%specialty == "spikes")
			{
				MineAmmo::SpikeMine(%player,%item);
			}

			Player::decItemCount(%player,%item);
		}
	}
}


//===== Actual Functions =====//

function MineAmmo::AntiPersonelMine(%player,%item)
{
	%obj = newObject("","Mine","antipersonelMine");
	addToSet("MissionCleanup", %obj);
	%client = Player::getClient(%player);
	GameBase::throw(%obj,%player,15 * %client.throwStrength,false);
	%player.throwTime = getSimTime() + 0.5;
	GameBase::setTeam(%obj,GameBase::getTeam(%player));
}

SoundProfileData ProfileApplyBreaks
{
   baseVolume = 1;
   minDistance = 100.0;
   maxDistance = 300.0;
   flags = SFX_IS_HARDWARE_3D;
};

SoundData SoundApplyBreaks
{
   wavFileName = "crash.wav";
   profile = ProfileApplyBreaks;
};

function MineAmmo::applyBreaks(%player,%item)
{
	%client = Player::getClient(%player);
	if(Player::getItemCount(%player,Mana) > 10)
	{
		Item::setVelocity(%client, "0 0 2");
		GameBase::playSound(%player,SoundApplyBreaks,0);
		Client::SendMessage(%client,1,"Applying BREAKS!");
		Player::decItemCount(%player,Mana,10);
	}
	else
	{
		Client::SendMessage(%client,1,"Not enough mana to cast the spell! You need at least 10 mana to cast this spell");
	}
}

function MineAmmo::Hover(%player,%item)
{
	%client = Player::getClient(%player);
	Item::setVelocity(%client, "0 0 1.1085");
	if(Player::isDead(%player) || !$MineAmmo::Hover[%client])
	{
		Client::SendMessage(%client,1,"Disengaging Hover!");
		$MineAmmo::Hover[%client] = "false"; //== Second round check
		return;
	}
	schedule("MineAmmo::Hover(" @ %player @ "," @ %item @ ");",0.01); //== 100 times per second...
}

$TeamItemMax[MineBot] = 10;

function MineAmmo::SpwanPartner(%player,%item)
{
	%clientId = GameBase::getOwnerClient(%player);
	%teamnum = GameBase::getTeam(%clientId);
	%name = "DefenderBot"; //== Instead of normal name because the name of the player may have < or > or something else the screws it.
	%time = floor(getRandom() * 100);
	%count = $TeamItemCount[GameBase::getTeam(%player) @ MineBot];
	%fullbotname = %name @ "_Guard_Roam_" @ Client::getGender(%clientId) @ "_" @ %count;

	if($TeamItemCount[GameBase::getTeam(%player) @ MineBot] < $TeamItemMax[MineBot])
	{
		//== Spwan Bot
		AI::spawnAdditionalBot(%fullbotname, %teamnum, 0);

		//== Move the bot to player
		schedule("MineAmmo::MovePartner(" @ %fullbotname @"," @ %clientId @ ");",0.1);

		//== Set bot's life line
		schedule("AI::RemoveBot(" @ %fullbotname @ ", %clientId);",%time);

		//== Alert Player of Bot and Its Life Line
		bottomprint(%clientId, "<jc><f2>Guard <f3>SPWANED<f2> for " @ %time @ " seconds!", %time);

		//== Increase MineBot limit count....
		$TeamItemCount[GameBase::getTeam(%player) @ MineBot]++;
	}
	else
	{
		bottomprint(%clientId, "<jc><f2>Unable to draft defenderbots due to limited resources.\nPlease try again in the NEXT mission.", 15);
	}
}

function MineAmmo::MovePartner(%fullbotname, %clientId)
{
		%aiId = BotFuncs::GetId(%fullbotname);
		%aiPlayer = Client::GetOwnedObject(%aiId);

		%name = Client::getName(%clientId);

		if($debug)
			echo("%aiId : " @ %aiId @ "\n%aiPlayer" @ %aiPlayer);

		%spawnMarker = Vector::Add(GameBase::getPosition(%clientId),"1 1 5");
		//%spawnMarker = GameBase::getPosition(%clientId);
		%xPos = getWord(%spawnMarker, 0);
		%yPos = getword(%spawnMarker, 1);
		%zPos = getWord(%spawnMarker, 2);
		%o = %xPos @ "  " @ %yPos @ "  " @ %zPos;

		GameBase::SetPosition(%aiPlayer, %o);

		if($debug)
			echo("Sending bot " @ %fullbotname @ " to " @ %o);

		//== Set the bot to follow the creator, second priority.
		AI::DirectiveFollow(%fullbotname, %clientId, 0, 2);

		//== Set the bot status to guarding and following creator...
		$Spoonbot::BotStatus[%aiId] = "Guarding and Following " @ %name;
}

//----------------------------------------------------------------------------
// MINE DYNAMIC DATA

MineData AntipersonelMine
{
	className = "Mine";
   description = "Antipersonel Mine";
   shapeFile = "mine";
   validateShape = true;
   validateMaterials = true;
   shadowDetailMask = 4;
   explosionId = mineExp;
	explosionRadius = 10.0;
	damageValue = 0.65;
	damageType = $MineDamageType;
	kickBackStrength = 150;
	triggerRadius = 2.5;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};

function AntipersonelMine::onAdd(%this)
{
	%this.damage = 0;
	AntipersonelMine::deployCheck(%this);
}

function AntipersonelMine::onCollision(%this,%object)
{
	%type = getObjectType(%object);
	%data = GameBase::getDataName(%this);
	if ((%type == "Player" || %data == AntipersonelMine || %data == Vehicle || %type == "Moveable") && GameBase::isActive(%this)) 
		if(!$NovaMorpher::FriendlyMines || GameBase::getTeam(%this) != GameBase::getTeam(%object))
			GameBase::setDamageLevel(%this, %data.maxDamage);
}

function AntipersonelMine::deployCheck(%this)
{
	if (GameBase::isAtRest(%this)) {
		GameBase::playSequence(%this,1,"deploy");
	 	GameBase::setActive(%this,true);
		%set = newObject("set",SimSet);
		if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) {
			%data = GameBase::getDataName(%this);
			GameBase::setDamageLevel(%this, %data.maxDamage);
		}
		deleteObject(%set);
	}
	else 
		schedule("AntipersonelMine::deployCheck(" @ %this @ ");", 3, %this);
}	

function AntipersonelMine::onDestroyed(%this)
{
	$TeamItemCount[GameBase::getTeam(%this) @ "mineammo"]--;
}

function AntipersonelMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
   if (%type == $MineDamageType)
      %value = %value * 0.25;

	%data = GameBase::getDataName(%this);
	if((%data.maxDamage/1.5) < %this.damage+%value) 
		GameBase::setDamageLevel(%this, %data.maxDamage);
	else 
		%this.damage += %value;
}

//----------------------------------------------------------------------------
// MINE DYNAMIC DATA
$deathMsg[$SpikeDamageType, 0]      = "%2 fell into a pitt full of nails.";
$deathMsg[$SpikeDamageType, 1]      = "%2 participated in Spike-O-Rama!";
$deathMsg[$SpikeDamageType, 2]      = "%1 places nails on the floor for %2.";
$deathMsg[$SpikeDamageType, 3]      = "%2 learns that spikes have sharp edges...";
$deathMsg[$SpikeDamageType, 4]      = "%1 uses Spike-a-nisis on %2.";

MineData SpikeMine
{
	className = "Mine";
	description = "Spikes";
	shapeFile = "bullet";
	validateShape = true;
	validateMaterials = true;
	shadowDetailMask = 4;
	explosionId = bulletExp0;
	explosionRadius = 4.0;
	damageValue = 0.1;
	damageType = $SpikeDamageType;
	kickBackStrength = 2;
	triggerRadius = 0.1;
	maxDamage = 0.5;
	shadowDetailMask = 0;
	destroyDamage = 0.1;
	damageLevel = {1.0, 1.0};
};

function SpikeMine::onAdd(%this)
{
	%this.damage = 0;
	%data = GameBase::getDataName(%this);
	schedule("GameBase::setDamageLevel("@%this@","@%data.maxDamage@");", 120, %this);

}

function SpikeMine::onCollision(%this,%object)
{
	%type = getObjectType(%object);
	%data = GameBase::getDataName(%this);
	if((%type == "Player" || %data == Vehicle || %type == "Moveable")) 
		if(!$NovaMorpher::FriendlyMines || GameBase::getTeam(%this) != GameBase::getTeam(%object))
			GameBase::setDamageLevel(%this, %data.maxDamage);
}

function SpikeMine::onDestroyed(%this)
{
	$TeamItemCount[GameBase::getTeam(%this) @ "mineammo"]--;
}

function SpikeMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
     	%value = %value * 0.001;

	%data = GameBase::getDataName(%this);
	if((%data.maxDamage/1.5) < %this.damage+%value) 
		GameBase::setDamageLevel(%this, %data.maxDamage);
	else 
		%this.damage += %value;
}

function MineAmmo::SpikeMine(%player,%item)
{
	for(%i=1;%i<4;%i++)
	{
		%obj = newObject("","Mine","SpikeMine");
		addToSet("MissionCleanup", %obj);
		%client = Player::getClient(%player);
		GameBase::throw(%obj,%player,%i * 15 * %client.throwStrength,false);
		%player.throwTime = getSimTime() + 0.5;
		GameBase::setTeam(%obj,GameBase::getTeam(%player));
	}
}
