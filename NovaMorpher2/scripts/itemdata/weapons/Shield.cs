//== This should be more devistating then nukes!
//== Lets see... Wat should we bombard these people with... Ahh.... Mortar like things :-)
$TeamItemMax[ShieldWep] = 3;

$InvList[ShieldWep] = 1;
$RemoteInvList[ShieldWep] = 1;

$ItemMax[harmor, ShieldWep] = 1;
$ItemMax[amarmor, ShieldWep] = 1;

$AutoUse[ShieldWep] = True;

$WeaponAmmo[ShieldWep] = "";

AddWeapon(ShieldWep);

ItemImageData ShieldWepImage
{
	shapeFile = "shield_medium";
	mountPoint = 0;
	weaponType = 0; // Single Shot

	sfxActivate = SoundPickUpWeapon;
};

ItemData ShieldWep
{
	heading = "wToolz";
	description = "Particle Shield";
	className = "Tool";
	shapeFile  = "shield_large";
	hudIcon = "shieldpack";
	shadowDetailMask = 4;
	imageType = ShieldWepImage;
	price = 10;
	showWeaponBar = false;
};
