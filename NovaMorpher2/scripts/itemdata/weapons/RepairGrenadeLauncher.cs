ItemData RGLAmmo
{
	description = "RGL Ammo";
	className = "Ammo";
	shapeFile = "grenammo";
   heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 20;
};

ItemImageData RepairGrenadeLauncherI
{
	shapeFile = "grenadeL";
	mountPoint = 0;

	mountOffset		= { 0.3, 0, 0 };

	weaponType = 0; // Single Shot
	ammoType = RGLAmmo;
	projectileType = RepairGrenadeShell;
	accuFire = false;
	reloadTime = 0.5;
	fireTime = 0.5;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = SoundFireGrenade;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData RepairGrenadeLauncher
{
	description = "Repair GL";
	className = "Weapon";
	shapeFile = "grenadeL";
	hudIcon = "grenade";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = RepairGrenadeLauncherI;
	price = 300;
	showWeaponBar = true;
   validateShape = false;
};


$InvList[RepairGrenadeLauncher] = 1;
$RemoteInvList[RepairGrenadeLauncher] = 1;

$ItemMax[marmor, RepairGrenadeLauncher] = 1;
$ItemMax[mfemale, RepairGrenadeLauncher] = 1;

$InvList[RGLAmmo] = 1;
$RemoteInvList[RGLAmmo] = 1;

$ItemMax[marmor, RGLAmmo] = 5;
$ItemMax[mfemale, RGLAmmo] = 5;

$AutoUse[RepairGrenadeLauncher] = False;

$WeaponAmmo[RepairGrenadeLauncher] = "RGLAmmo";

AddWeapon(RepairGrenadeLauncher);
