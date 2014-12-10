function DoSpecial::Add(%vehicle, %type)
{
	%i=0;
	eval("%check=($Patch::"@%type@"Vehicles["@%i@"] != '');");
	while(%check)
	{
		%i++;
		eval("%check=($Patch::"@%type@"Vehicles["@%i@"] != '');");
	}
	eval("$Patch::"@%type@"Vehicles["@%i@"]=\""@%vehicle@"\";");
}

function DoSpecial::Dismount(%name, %this, %mom, %cl)
{
	if(%name != "")
	{
		for(%i = 0; $Patch::DismountVehicles[%i] != ""; %i++)
		{
			if($Patch::DismountVehicles[%i] == %name)
			{
				%string = $Patch::DismountVehicles[%i] @ "::onDismount(\"" @ %this @ "\",\"" @ %mom @ "\",\"" @ %cl @ "\");";

				if($debug)
					echo("%string = " @ %string);

				eval(%string);

				return false;
			}
		}
	}
	else
	{
		if($debug)
			echo("Error: No vehicle name!");
	}
	return true;
}

function DoSpecial::passengerJump(%this,%passenger,%mom)
{
	%name = GameBase::getDataName(%this);
	if(%name != "")
	{
		for(%i = 0; $Patch::pJumpVehicles[%i] != ""; %i++)
		{
			if($Patch::pJumpVehicles[%i] == %name)
			{
				%string = $Patch::pJumpVehicles[%i] @ "::onPJump(\"" @ %this @ "\",\"" @ %passenger @ "\",\"" @ %mom @ "\");";

				if($debug)
					echo("%string = " @ %string);

				eval(%string);

				return false;
			}
		}
	}
	else
	{
		if($debug)
			echo("Error: No vehicle name!");
	}
	return true;
}

function DoSpecial::Collision(%this, %object)
{
	%name = GameBase::getDataName(%this);
	if(%name != "")
	{
		for(%i = 0; $Patch::CollisionVehicles[%i] != ""; %i++)
		{
			if($Patch::CollisionVehicles[%i] == %name)
			{
				%string = $Patch::CollisionVehicles[%i] @ "::onCollision(\"" @ %this @ "\",\"" @ %mom @ "\",\"" @ %cl @ "\");";

				if($debug)
					echo("%string = " @ %string);

				eval(%string);

				return false;
			}
		}
	}
	else
	{
		if($debug)
			echo("Error: No vehicle name!");
	}
	return true;
}

function DoSpecial::OnDestroy(%name, %this, %cl)
{
	if(%name != "")
	{
		if($Patch::DeathVehicles[%name])
		{
			%string = %name @ "::onDestroy(" @ %this @ "," @ %cl @ ");";

			if($debug)
				echo("%string = " @ %string);

			eval(%string);
		}
	}
	else
	{
		if($debug)
			echo("Error: No vehicle name!");
	}
}