ItemImageData FlameThrowerI
{
	shapeFile = "grenadeL";
	mountPoint = 0;

	mountOffset		= { 0.3, 0, 0 };

	weaponType = 0; // Single Shot
	projectileType = Flame;
	accuFire = false;
	reloadTime = 0.099;
	fireTime = 0.001;

	minEnergy = 20;
	maxEnergy = 0.8;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = explosion4;
	sfxActivate = SoundPickUpWeapon;
//	sfxReload = SoundDryFire;
};

ItemData FlameThrower
{
	description = "Flame Thrower";
	className = "Weapon";
	shapeFile = "grenadeL";
	hudIcon = "grenade";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = FlameThrowerI;
	price = 333;
	showWeaponBar = true;
   validateShape = false;
};

ItemImageData FlameThrowerTankI
{
	shapeFile = "mortar";
	mountPoint = 0;
	mountOffset		= { 0.3, 0, 0 };
};

ItemData FlameThrowerTank
{
	description = "Flame Thrower Tank";
	className = "Weapon";
	shapeFile = "mortar";
	shadowDetailMask = 4;
	imageType = FlameThrowerTankI;
};

function FlameThrower::onMount(%player,%imageSlot) 
{ 
	Player::mountItem(%player,FlameThrowerTank,$ExtraWeaponSlotA); 
} 

function FlameThrower::onUnmount(%player,%imageSlot) 
{ 
	Player::unmountItem(%player,$ExtraWeaponSlotA);
} 


$InvList[FlameThrower] = 1;
$RemoteInvList[FlameThrower] = 1;

$ItemMax[marmor, FlameThrower] = 1;
$ItemMax[mfemale, FlameThrower] = 1;

$ItemMax[larmor, FlameThrower] = 1;
$ItemMax[lfemale, FlameThrower] = 1;

$AutoUse[FlameThrower] = False;

AddWeapon(FlameThrower);
