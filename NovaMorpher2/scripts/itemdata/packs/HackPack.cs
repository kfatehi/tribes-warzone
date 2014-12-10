ItemImageData HackGunI
{
	shapeFile = "cmdpnl";	//== Hahahhaha... This is gonna be HELL funny!
	mountPoint = 0;

	mountRotation = { 0, 1.57, 0 }; 

	weaponType = 2;  // Sustained
	projectileType = HackBolt;
	minEnergy = 4;
	maxEnergy = 4;  // Energy used/sec for sustained weapons
	reloadTime = 4;
                        
	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 0.25, 0.25, 0.85 };

	sfxActivate = SoundPickUpWeapon;
	sfxFire     = SoundELFIdle;
};

ItemData HackGun
{
   description = "Hack Gun";
	shapeFile = "shotgun";
	hudIcon = "energyRifle";
   className = "Weapon";
   heading = "bWeapons";
   shadowDetailMask = 4;
   imageType = HackGunI;
	showWeaponBar = true;
   price = 125;
   validateShape = false;
};

function HackGun::onMount(%player,%imageSlot)
{
	Player::trigger(%player,$BackpackSlot,true);
}

function HackGun::onUnmount(%player,%imageSlot)
{
	Player::trigger(%player,$BackpackSlot,false);
}

ItemImageData HackPackImage
{
	shapeFile = "cmdpnl";
	mountPoint = 2;
	weaponType = 2;  // Sustained
	minEnergy = 4;
	maxEnergy = 4;   // Energy/sec for sustained weapons
	sfxFire = SoundShieldOn;
	firstPerson = false;
};

ItemData HackPack
{
	description = "Hack Pack";
	shapeFile = "cmdpnl";
	className = "Backpack";
   heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = HackPackImage;
	price = 175;
	hudIcon = "repairpack";
	showWeaponBar = true;
	hiliteOnActive = true;
   validateShape = false;
   validateMaterials = true;
};


function HackPack::onUnmount(%player,%item)
{	if (Player::getMountedItem(%player,$WeaponSlot) == HackGun) 
	{	Player::unmountItem(%player,$WeaponSlot);
	}
	Player::UnMountItem(%player,$FlagSlot);
}

function HackPack::onUse(%player,%item)
{	if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
	{	Player::mountItem(%player,%item,$BackpackSlot);
	
	}
	else
	{	Player::mountItem(%player,HackGun,$WeaponSlot);
	}
}

$packDiscription[HackPack] = "Hackers rule DA world! Thats all I can say! :P";

if($NovaMorpher::AllowHacking)
{
	$InvList[HackPack] = 1;
	$ItemMax[marmor, HackPack] = 1;
	$ItemMax[mfemale, HackPack] = 1;
}