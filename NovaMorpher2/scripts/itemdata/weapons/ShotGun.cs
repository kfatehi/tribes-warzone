//======================================================================== ShotGun Launcher
BulletData ShotgunBullet
{
   bulletShapeName    = "bullet.dts";
   validateShape      = false;
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.045;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.075;
   muzzleVelocity     = 425.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 1.0;
   tracerLength       = 30;
};

BulletData CompressedShotgunBullet
{
   bulletShapeName    = "bullet.dts";
   validateShape      = false;
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.045;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.015;
   muzzleVelocity     = 425.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 1.0;
   tracerLength       = 30;
};

ItemData ShotGunAmmo 
{
	description = "ShotGunAmmo";
	className = "Ammo";
	heading = "xAmmunition";
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 50;
};

ItemImageData ShotGunImage 
{
	shapeFile = "shotgun";
	mountPoint = 0;
	mountOffset = { 0.0, 0.0, -0.2 };
	mountRotation = { 0, 0, 0 };	
	weaponType = 0;
	ammoType = ShotGunAmmo;
	projectileType = "Undefined";
	accuFire = false;
	reloadTime = 1;
	fireTime = 0.5;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	sfxFire = SoundFireMortar;
	sfxReload = SoundDiscReload;
	sfxActivate = SoundPickUpWeapon;
	sfxReady = SoundMortarIdle;
};
	
function ShotGunImage::onFire(%player, %slot) 
{
	%Ammo = Player::getItemCount(%player, $WeaponAmmo[ShotGun]);
	%armor = Player::getArmor(%player);
	%client = GameBase::getOwnerClient(%player);
	
	 if(%Ammo) 
	 {
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
	
		%client = GameBase::getOwnerClient(%player);
		%clientId = %client;

		Player::decItemCount(%player,$WeaponAmmo[ShotGun],1);
		if($Settings::ShotGun[%clientId] == "1")
		{
			for(%i = 0; %i < 50; %i++) //== HAHAHAHA SUPER CHAINGUN!!! ;)
				Projectile::spawnProjectile("ShotgunBullet",%trans,%player,%vel,%player);
 
			Player::decItemCount(%player,$WeaponAmmo[ShotGun],1);
		}
		else if($Settings::ShotGun[%clientId] == "2")
		{
			for(%i = 0; %i < 25; %i++)
				Projectile::spawnProjectile("CompressedShotgunBullet",%trans,%player,%vel,%player);
 
			Player::decItemCount(%player,$WeaponAmmo[ShotGun],3);
		}
		else if($Settings::ShotGun[%clientId] == "3")
		{
			for(%i = 0; %i < 50; %i++)
				Projectile::spawnProjectile("CompressedShotgunBullet",%trans,%player,%vel,%player);
 
			Player::decItemCount(%player,$WeaponAmmo[ShotGun],7);
		}
		else
		{
			for(%i = 0; %i < 25; %i++)
				Projectile::spawnProjectile("ShotgunBullet",%trans,%player,%vel,%player);
		}
	}
	else
	{
		Client::sendMessage(Player::getClient(%player), 0,"You have no Ammo for the ShotGun");
		bottomprint(Player::getClient(%player), "You have no Ammo for the ShotGun",5);
	}
}

ItemData ShotGun
{
	description = "ShotGun";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "mortar";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = ShotGunImage;
	price = 375;
	showWeaponBar = true;
};

$InvList[ShotGun] = 1;
$RemoteInvList[ShotGun] = 0;

$ItemMax[amarmor, ShotGun] = 1;
$ItemMax[harmor, ShotGun] = 1;
$ItemMax[marmor, ShotGun] = 1;
$ItemMax[mfemale, ShotGun] = 1;

$InvList[ShotGunAmmo] = 1;
$RemoteInvList[ShotGunAmmo] = 1;

$ItemMax[amarmor, ShotGunAmmo] = 40;
$ItemMax[harmor, ShotGunAmmo] = 32;
$ItemMax[marmor, ShotGunAmmo] = 18;
$ItemMax[mfemale, ShotGunAmmo] = 18;

$AutoUse[ShotGun] = False;

$WeaponAmmo[ShotGun] = "ShotGunAmmo";

AddWeapon(ShotGun);

$WeaponSpecial[ShotGun] = "true"; //== Wether this weapon has multiple settings or not...

function ShotGun::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	if($Settings::ShotGun[%clientId] == "0" || $Settings::ShotGun[%clientId] == "")
	{
		%mode = "Single Barrel";
	}
	else if($Settings::ShotGun[%clientId] == "1")
	{
		%mode = "Double Barrel";
	}
	else if($Settings::ShotGun[%clientId] == "2")
	{
		%mode = "Compressed Single Barrel";
	}
	else if($Settings::ShotGun[%clientId] == "3")
	{
		%mode = "Compressed Double Barrel";
	}
	bottomprint(%clientId, "<jc><f2>Using " @ %item.description @ " - <f0>" @ %mode, 2);
}