$InvList[VortexC] = 1;
$RemoteInvList[VortexC] = 1;

$InvList[VortexCAmmo] = 1;
$RemoteInvList[VortexCAmmo] = 1;

$ItemMax[amarmor, VortexC] = 1;
$ItemMax[amarmor, VortexC] = 1;

$ItemMax[amarmor, VortexCAmmo] = 250;
$ItemMax[amarmor, VortexCAmmo] = 250;

$ItemMax[harmor, VortexC] = 1;
$ItemMax[harmor, VortexC] = 1;

$ItemMax[harmor, VortexCAmmo] = 500;
$ItemMax[harmor, VortexCAmmo] = 500;

$WeaponAmmo[VortexC] = "VortexCAmmo";

AddWeapon(VortexC);


$VortexDamageType = DamageTypes::AddReserve();
$deathMsg[$VortexDamageType , 0]      = "%2 tried sky diving into a black hole!";
$deathMsg[$VortexDamageType , 1]      = "%1 calls in the ghost busters to suck the life out of %2";
$deathMsg[$VortexDamageType , 2]      = "%2 finds a vortex very interesting, while it burned %4 into dust.";
$deathMsg[$VortexDamageType , 3]      = "%2 loved the joy ride into the vortex of death >)!";
$deathMsg[$VortexDamageType , 4]      = "%2 couldn't RESIST the vortex's love for %4!";

$SpecialDamageType[$VortexDamageType] = "VortexDamageType";

function VortexDamageType::DoSpecialDMG(%damagedPlayer, %shooterPlayer)
{
	%shooterClient = Player::getClient(%shooterPlayer); if($debug){echo("%shooterClient: "@%shooterClient);}
	%damagedClient = Player::getClient(%damagedPlayer); if($debug){echo("%damagedClient: "@%damagedClient);}
	%damagedPlTeam = Client::getTeam(%damagedClient);
	%shooterClTeam = Client::getTeam(%shooterClient);

	%chance = floor(getRandom() * 100);
	if(%chance > 90)
		EMP(%damagedPlayer, %damagedClient, %shooterClient, "Your energy has been curropted! Prepareing to repair damages!");
}

TargetLaserData vortexCLaserA
{
   laserBitmapName   = "laserPulse.bmp";
   damageConversion  = 0.0;
   baseDamageType    = $LaserDamageType;
   detachFromShooter = false;
};

TargetLaserData vortexCLaserB
{
   laserBitmapName   = "paintPulse.bmp";
   damageConversion  = 0.0;
   baseDamageType    = $LaserDamageType;
   detachFromShooter = false;
};

TargetLaserData vortexCLaserC
{
   laserBitmapName   = "lightningNew.bmp";
   damageConversion  = 0.0;
   baseDamageType    = $LaserDamageType;
   detachFromShooter = false;
};

ExplosionData shieldExp
{
   shapeName = "shield_large.dts";
   soundId   = SoundFlierCrash;

   faceCamera = true;
   randomSpin = true;
   hasLight   = true;
   lightRange = 8.0;

   timeScale = 9;

   timeZero = 0.200;
   timeOne  = 0.950;

   colors[0]  = { 0.4, 0.4,  1.0 };
   colors[1]  = { 1.0, 1.0,  1.0 };
   colors[2]  = { 1.0, 0.95, 1.0 };
   radFactors = { 0.5, 1.0, 1.0 };
};

BulletData VortexEBolt
{
   bulletShapeName    = "shield.dts";
   explosionTag       = shieldExp;

   damageClass        = 1;
   damageValue        = 0.01;
   damageType         = $VortexDamageType;
   kickBackStrength   = -400;

   explosionRadius    = 9.0;
   aimDeflection      = 0.006;

   muzzleVelocity     = 300.0;
   totalTime          = 10.1;
   liveTime           = 10.0;
   lightRange         = 3.0;
   lightColor         = { 1, 1, 0 };
   inheritedVelocityScale = 0.3;
   isVisible          = True;

   soundId = SoundJetLight;
};

RocketData VortexMBolt
{
   bulletShapeName  = "shockwave_large.dts";
   explosionTag     = shieldExp;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.01;
   damageType       = $VortexDamageType;

   explosionRadius  = 9;
   kickBackStrength = -250;
   muzzleVelocity   = 310; //== Arrives just a bit quicker
   terminalVelocity = 310;
   acceleration     = 0.0;
   totalTime        = 10.0;
   liveTime         = 11.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "shield_large.dts";
   smokeDist   = 10;

   soundId = SoundJetHeavy;
};


ItemData VortexCAmmo
{
	description = "Vortex Charge";
	className = "Ammo";
	shapeFile = "mortarammo";
	heading = "xAmmunition";
	shadowDetailMask = 4;
	price = 4;
};

ItemImageData VortexCImage
{
	shapeFile = "mortargun";
	mountPoint = 0;
	mountOffset = { 0, -0.1, 0 };

	weaponType = 0; // Single Shot
	ammoType = VortexCAmmo;
	accuFire = false;
	reloadTime = 2.5;
	fireTime = 1.5;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = SoundFireSeeking;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundMortarReload;
};

ItemData VortexC
{
	description = "Vortex Cannon";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "grenade";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = VortexCImage;
	price = 16384;
	showWeaponBar = true;
};

function VortexC::onMount(%player,%item,$WeaponSlot)
{
	%client = Player::getclient(%player);
	Player::mountItem(%player,ExtraVortexCB,$ExtraWeaponSlotA);
	Player::mountItem(%player,ExtraVortexCC,$ExtraWeaponSlotB);

	Player::trigger(%player,$ExtraWeaponSlotA,true);
	Player::trigger(%player,$ExtraWeaponSlotB,true);
}

function VortexC::onUnmount(%player,%item,$WeaponSlot)
{
	Player::trigger(%player,$ExtraWeaponSlotA,false);
	Player::trigger(%player,$ExtraWeaponSlotB,false);

	Player::unmountItem(%player,$ExtraWeaponSlotA);
	Player::unmountItem(%player,$ExtraWeaponSlotB);
}

function VortexCImage::onFire(%player,%slot)
{
	%rawCharge = Player::getItemCount(%player,VortexCAmmo);
	%charge = %rawCharge *= 0.2;
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);

	if(%charge > 0)
	{
		Projectile::spawnProjectile("VortexMBolt",%trans,%player,%vel);
		for(%i=1; %i<%charge; %i++)
		{
			Projectile::spawnProjectile("VortexEBolt",%trans,%player,%vel);
		}
		Player::setItemCount(%player,VortexCAmmo,0);
	}
		else Client::sendMessage(Player::getClient(%player),0,"VortexC is out of energy.");

	if(%player.VortexCRecharging != True)
	{
		%player.VortexCRecharging = True;
		%guntype = VortexC;
		schedule("RechargeAmmo(" @ %player @ ", " @ %guntype @ ");",0.075,%player);
	}
}

ItemImageData VortexCImageA
{
	shapeFile = "paintgun";
	mountPoint = 0;
	mountOffset = { 0, 0, 0.2 };

	weaponType = 2; // Single Shot
	projectileType = vortexCLaserA;
	accuFire = true;
	reloadTime = 0.5;
	fireTime = 0.2;
	minEnergy = 0;
	maxEnergy = 0;

};

ItemData ExtraVortexCA
{
	description = "VortexC";
	className = "Weapon";
	shapeFile = "paintgun";
	hudIcon = "grenade";
   heading = "cSpecialWeapons";
	shadowDetailMask = 4;
	imageType = VortexCImageA;
	price = 0;
	showWeaponBar = true;
	showInventory = false;
};


ItemImageData VortexCImageB
{
	shapeFile = "paintgun";
	mountPoint = 0;
	mountOffset = { 0.3, 0, 0 };

	weaponType = 2; // Single Shot
	projectileType = vortexCLaserB;
	accuFire = true;
	reloadTime = 0.5;
	fireTime = 0.2;
	minEnergy = 0;
	maxEnergy = 0;

};

ItemData ExtraVortexCB
{
	description = "VortexC";
	className = "Weapon";
	shapeFile = "paintgun";
	hudIcon = "grenade";
   heading = "cSpecialWeapons";
	shadowDetailMask = 4;
	imageType = VortexCImageB;
	price = 0;
	showWeaponBar = true;
	showInventory = false;
};
ItemImageData VortexCImageC
{
	shapeFile = "paintgun";
	mountPoint = 0;
	mountOffset = { -0.3, 0, 0 };

	weaponType = 2; // Single Shot
	projectileType = vortexCLaserC;
	accuFire = true;
	reloadTime = 0.5;
	fireTime = 0.2;
	minEnergy = 0;
	maxEnergy = 0;
};

ItemData ExtraVortexCC
{
	description = "VortexC";
	className = "Weapon";
	shapeFile = "paintgun";
	hudIcon = "grenade";
   heading = "cSpecialWeapons";
	shadowDetailMask = 4;
	imageType = VortexCImageC;
	price = 0;
	showWeaponBar = true;
	showInventory = false;
};


function RechargeAmmo(%player,%gunType)
{
	%armor = Player::GetArmor(%player);
	if(%gunType == VortexC && (Player::getItemCount(%player,VortexC) > 0))
	{
		if(Player::getItemCount(%player,VortexCAmmo) < $ItemMax[%armor,VortexCAmmo])
		{

		Player::incItemCount(%player,VortexCAmmo,1);
			schedule("RechargeAmmo(" @ %player @ ", " @ %gunType @ ");",0.1,%player);
		}
		else
			%player.VortexCRecharging = False;
	}
}
