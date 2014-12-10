//== This is the stuff most spells use... IF they done use energy.

ItemData Mana
{
	description = "Mana";
	className = "Ammo";
	heading = "xEnergy";
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 1;
};

function Mana::onCharge(%player)
{
	%armor = Player::getArmor(%player);
	%client = GameBase::getOwnerClient(%player);
	%checkA = %armor == "magearmor" || %armor == "magefemale";
	%checkAExcept = %armor == "undeadmagearmor" || %armor == "undeadmagefemale";

	if((%checkAExcept || %checkA) && $ManaChargeDisabled[%player] != "true")
	{
		$ManaCount[%client]++;
		if($ManaCount[%client] == 30 && Player::getItemCount(%player,Mana) != "500")
		{
			bottomprint(%client,"<jc><f3>Mana Level: <f1>" @ Player::getItemCount(%player,Mana),5);
			$ManaCount[%client] = "0";
		}

		if(Player::getItemCount(%player,Mana) < 500 && Player::getItemCount(%player, "Flag") < "1") //== Mana doesn't recharge with a flag for some mysterious reason LOL
			Player::incItemCount(%player,Mana,1);
		else if(Player::getItemCount(%player, "Flag") == "1" && Player::getItemCount(%player,Mana) > 0) //== Instead it decreases :O
			Player::decItemCount(%player,Mana,1);

		if(!$ManaChargeDisabled[%player])
		{
			$ManaChargeDisabled[%player] = true;
			if(%checkAExcept)
			{	//== Major SLOWDOWN!!! =D
				schedule("$ManaChargeDisabled["@%player@"] = false;",0.7456789);
				schedule("Mana::onCharge("@%player@");",0.75);
			}
			else
			{
				schedule("$ManaChargeDisabled["@%player@"] = false;",0.2456789);
				schedule("Mana::onCharge("@%player@");",0.25);
	 		}
		}
		$ManaChargeOn[%client] = "true";
	}
	else if(!%checkA)
	{
		$ManaChargeOn[%client] = "false";
		$ManaCount[%client] = "0";
	}
}