
//======================================================================== Sniper Rifle
ItemData SniperRifleAmmo 
{
	description = "SniperRifleAmmo";
	className = "Ammo";
	heading = "xAmmunition";
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 50;
};

ItemImageData SniperRifleImage
{
	shapeFile = "sniper";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	projectileType = SniperBullet;
	ammoType = SniperRifleAmmo;
	accuFire = true;
	reloadTime = 0.7;
	fireTime = 0.3;

	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 1, 0, 0 };

	sfxFire = SoundFireLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData SniperRifle
{
	description = "Sniper Rifle";
	className = "Weapon";
	shapeFile = "sniper";
	hudIcon = "sniper";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = SniperRifleImage;
	price = 444;
	showWeaponBar = true;
   validateShape = false;
   validateMaterials = true;
};

$isSniperRifle[SniperRifle] = true; //== Is this a sniper rifle? (In other words a gun that you can snipe people with)

$InvList[SniperRifle] = 1;
$RemoteInvList[SniperRifle] = 1;

$ItemMax[sniperxarmor, SniperRifle] = 1;
$ItemMax[sniperxfemale, SniperRifle] = 1;

$InvList[SniperRifleAmmo] = 1;
$RemoteInvList[SniperRifleAmmo] = 1;

$ItemMax[sniperxarmor, SniperRifleAmmo] = 30;
$ItemMax[sniperxfemale, SniperRifleAmmo] = 30;

$AutoUse[SniperRifle] = True;

$WeaponAmmo[SniperRifle] = "SniperRifleAmmo";

AddWeapon(SniperRifle);