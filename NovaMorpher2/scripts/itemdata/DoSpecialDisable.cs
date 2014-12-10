function DoSpecial::Disable(%this)
{
	for(%i = 0; $Patch::Turret[%i] != ""; %i++)
	{
		if($Patch::Turret[%i] == %name)
		{
			%string = $Patch::Turret[%i] @ "::onDisable(" @ %this @ ");";

			if(!(eval(%string)))
			{
				if($debug)
					echo("Error: No such disable function for " @ %name @ "!!!");
				return false;
			}

			return true;
		}
	}
	return false;
}

DoSpecial::Disable("MorpherPlasmaTurret");
DoSpecial::Disable("MorpherBlastWall");
