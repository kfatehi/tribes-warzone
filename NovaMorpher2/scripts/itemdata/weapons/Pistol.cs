
//======================================================================== Pistol
ItemData PistolAmmo 
{
	description = "PistolAmmo";
	className = "Ammo";
	heading = "xAmmunition";
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 1;
};

ItemImageData PistolImage
{
	shapeFile = "energygun";
	mountPoint = 0;

	mountOffset		= { -0.15, 0, 0 };

	weaponType = 0; // Single Shot
	projectileType = PistolBullet;
	ammoType = PistolAmmo;
	accuFire = true;
	reloadTime = 0.199;
	fireTime = 0.001;

	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 1, 0, 0 };

	sfxFire = SoundFireLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Pistol
{
	description = "Pistol";
	className = "Weapon";
	shapeFile = "energygun";
	hudIcon = "energygun";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = PistolImage;
	price = 20;
	showWeaponBar = true;
   validateShape = false;
   validateMaterials = true;
};

$InvList[Pistol] = 1;
$RemoteInvList[Pistol] = 1;

$InvList[PistolAmmo] = 1;
$RemoteInvList[PistolAmmo] = 1;

$ItemMax[larmor, Pistol] = 1;
$ItemMax[lfemale, Pistol] = 1;

$ItemMax[larmor, PistolAmmo] = 200;
$ItemMax[lfemale, PistolAmmo] = 200;

$ItemMax[sniperxarmor, Pistol] = 1;
$ItemMax[sniperxfemale, Pistol] = 1;

$ItemMax[sniperxarmor, PistolAmmo] = 100;
$ItemMax[sniperxfemale, PistolAmmo] = 100;

$AutoUse[Pistol] = True;

$WeaponAmmo[Pistol] = "PistolAmmo";

AddWeapon(Pistol);