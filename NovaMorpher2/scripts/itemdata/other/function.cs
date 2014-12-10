//===================================================================================================== Start Shielding

function Shield::Start(%clientId, %player, %strength, %time, %msg1, %msg2)
{
	if(%player.shieldStrength < 0)
		%player.shieldStrength = 0;

	if(%msg != "")
		Client::sendMessage(%clientId,0,%msg1);
	GameBase::playSound(%player,ForceFieldOpen,0);
	%armor = Player::getArmor(%player);

	%player.shieldStrength = %strength;

	if($shieldTime[%clientId] == 0)
	{
		$shieldTime[%clientId] = %time;
		Shield::Check(%clientId, %player, %strength, %msg2);
	}
	else
		$shieldTime[%clientId] += %time;
}


function Shield::Check(%clientId, %player, %strength, %msg)
{
	if(%player.shieldStrength < 0)
		%player.shieldStrength = 0;

	%armor = Player::getArmor(%player);
	if($shieldTime[%clientId] > 0)
	{
		$shieldTime[%clientId] -= 2;  

		if  (Player::isDead(%player))
		{
			$shieldTime[%clientId] = 0;
		}
		

		schedule("checkPlayerShield(" @ %clientId @ ", " @ %player @ ", " @ %strength @ ", " @ %msg @ ");",2,%player);
    	}
	else
	{
		if(%msg != "")
			Client::sendMessage(%clientId,0,%msg);

		%player.shieldStrength = %player.shieldStrength - %strength;
		GameBase::playSound(%player,ForceFieldOpen,0);	
	}			

		
}