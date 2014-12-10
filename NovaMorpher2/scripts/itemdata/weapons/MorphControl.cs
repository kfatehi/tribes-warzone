//========================================================================//
//== This is the weapon that controls the morph of a novamorpher :-)... ==//
//========================================================================//


ItemImageData MorphControlI
{
	shapeFile = "shotgun";
	mountPoint = 0;

	mountRotation = { 0, 1.57, 0 }; 

	weaponType = 2;  // Sustained
	projectileType = MorphControlBolt;
	minEnergy = 20;
	maxEnergy = 20;  // Energy used/sec for sustained weapons
	reloadTime = 0.2;
                        
	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 0.25, 0.25, 0.85 };

	sfxActivate = SoundPickUpWeapon;
	sfxFire     = SoundELFIdle;
};

ItemData MorphControl
{
   description = "Morph Control";
	shapeFile = "shotgun";
	hudIcon = "energyRifle";
   className = "Tool";
   heading = "wToolz";
   shadowDetailMask = 4;
   imageType = MorphControlI;
	showWeaponBar = true;
   price = 125;
   validateShape = false;
};




$InvList[MorphControl] = 1;
$RemoteInvList[MorphControl] = 0;

$ItemMax[amarmor, MorphControl] = 1;

$AutoUse[MorphControl] = False;

$WeaponAmmo[MorphControl] = "";

AddWeapon(MorphControl);

$WeaponSpecial[MorphControl] = "true"; //== Wether this weapon has multiple settings or not...

function MorphControl::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	if($Settings::MorphControl[%clientId] == "")
	{
		%mode = "No Setting! Use the tab menu to set this weapon...";
	}
	else if($Settings::MorphControl[%clientId] == "0")
	{
		%mode = "Light Armor";
	}
	else if($Settings::MorphControl[%clientId] == "1")
	{
		%mode = "Medium Armor";
	}
	else if($Settings::MorphControl[%clientId] == "2")
	{
		%mode = "Heavy Armor";
	}
	else if($Settings::MorphControl[%clientId] == "3")
	{
		%mode = "BlastWall";
	}
	else if($Settings::MorphControl[%clientId] == "4")
	{
		%mode = "Vehicle";
	}
	else if($Settings::MorphControl[%clientId] == "5")
	{
		%mode = "Turret";
	}

	bottomprint(%clientId, "<jc><f2>Using " @ %item.description @ " - <f0>" @ %mode, 2);
}
