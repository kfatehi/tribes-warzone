function remoteToggleMorph(%clientId)
{
	if($Settings::AtomicMorpher[%clientId] < 5)
		$Settings::AtomicMorpher[%clientId]++;
	else
		$Settings::AtomicMorpher[%clientId] = "0";

	bottomprint(%clientId, "<jc><f0>AtomicMorpher<f1> is now set to: <f3>"@$AtomicMorpher::Set[$Settings::AtomicMorpher[%clientId]]);
}

//== Keybinding for this command is:==//
//====================================//
//== EditActionMap("actionMap.sae");
//== bindCommand(keyboard0, make, "f12", TO, "remoteEval(2048, ToggleMorph);");

//==================================================================================//

function remoteToggleWepSetting(%clientId)
{
	//%CurWeapon = Client::getOwnedObject(%clientId).CurWeapon; //== Old 'n stupid method...
	%CurWeapon = Player::getMountedItem(%clientId, $WeaponSlot);
	if($debug)
		echo("Client "@%clientId@" used the NOVAMORPHER key-bing to change: "@%CurWeapon@"'s setting!");
	if($WeaponSpecial[%CurWeapon])
	{
		eval("$Settings::"@%CurWeapon@"[%clientId]++;");
		eval("%num = $Settings::"@%CurWeapon@"[%clientId];");
		%string = "$Settings::"@%CurWeapon@"Tell["@%num@"]";
		eval('%check = '@%string@';');

		if(%check == "")
			eval("$Settings::"@%CurWeapon@"[%clientId]=0;");

		eval("%Mode = $Settings::"@%CurWeapon@"Tell[$Settings::"@%CurWeapon@"[%clientId]];");

		bottomprint(%clientId, "<jc><f0>"@%CurWeapon@"<f1> is now set to: <f3>"@%Mode,1);
	}
}

//== Keybinding for this command is:==//
//====================================//
//== EditActionMap("actionMap.sae");
//== bindCommand(keyboard0, make, "f8", TO, "remoteEval(2048, ToggleWepSetting);");
