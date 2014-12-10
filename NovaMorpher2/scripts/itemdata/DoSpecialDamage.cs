function DoSpecialDamageCheck(%player, %target, %type, %shooterClient)
{
	if(%type == $MorphControlType)
		MorphControl::DoMorph(%player, %target);
	else if(%type == $HealDamageType)
		Heal(Player::getClient(%player), %player);
	else if(%type == $UTFDamageType)
		UTFLaser::CheckSequence(%player);
	else if(%type == $KickDamageType)
		Net::Kick(Player::getClient(%target),"You were kicked dieing!");
	else if(%type == $FireDamageType)
		FireDamageType::ChanceBurn(%target, %shooterClient);
	else if($SpecialDamageType[%type] != "")
	{
		%string = $SpecialDamageType[%type]@"::DoSpecialDMG("@%target@","@%player@");";
		eval(%string);
	}

	if($debug)
		echo("$FireDamageType = "@$FireDamageType@"  Cur = "@%type);
}

function DamageTypes::AddReserve()
{
	for(%i = 0; $DamageType::Reserve[%i]; %i++){}
	$DamageType::Reserve[%i] = true;
	return %i;
}

for(%i = 0; %i < 14; %i++)
	DamageTypes::AddReserve(); //== Reserve the first 14 Base MOD damage types