//======================================================================== Sounds
SoundData SoundFireBorgLaser
{
   wavFileName = "energyexp.wav";
   profile = Profile3dNear;
};

SoundData SoundBorgLaserHit
{
   wavFileName = "energyexp.wav";
   profile = Profile3dMedium;
};

//======================================================================== Borg Laser Projection
LaserData borgLaserP
{
   laserBitmapName   = "paintPulse.bmp";
   hitName           = "laserhit.dts";

   damageConversion  = 0.04;
   baseDamageType    = $LaserDamageType;

   beamTime          = 1;

   lightRange        = 2.0;
   lightColor        = { 1.0, 0.25, 0.25 };

   detachFromShooter = true;
   hitSoundId        = SoundFireBorgLaser;
};

//======================================================================== Borg Laser
ItemImageData BorgLaserImage
{
	shapeFile = "paintgun";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	projectileType = borgLaserP;
	accuFire = true;
	reloadTime = 1;
	fireTime = 0;
	minEnergy = 3.25;
	maxEnergy = 10;

	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 1, 0, 0 };

	sfxFire = SoundBorgLaserHit;
	sfxActivate = SoundPickUpWeapon;
};

ItemData BorgLaser
{
	description = "Borg Laser";
	className = "Weapon";
	shapeFile = "paintgun";
	hudIcon = "sniper";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = BorgLaserImage;
	price = 200;
	showWeaponBar = true;
   validateShape = false;
   validateMaterials = true;
};
	
$InvList[BorgLaser] = 1;
$RemoteInvList[BorgLaser] = 1;

$ItemMax[borgarmor, BorgLaser] = 1;
$ItemMax[borgfemale, BorgLaser] = 1;

$AutoUse[BorgLaser] = True;

AddWeapon(BorgLaser);