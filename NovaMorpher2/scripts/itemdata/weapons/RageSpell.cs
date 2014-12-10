//--------------------------------------
BulletData RageBolt
{
   bulletShapeName    = "plasmatrail.dts";
   explosionTag       = turretExp;
   mass               = 0.05;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.075;
   damageType         = $EnergyDamageType;

   explosionRadius    = 3.0;
   aimDeflection      = 0.075;
   muzzleVelocity     = 50.0;
   totalTime          = 6.0;
   liveTime           = 4.0;
   isVisible          = True;

   rotationPeriod = 1.5;
};

ItemImageData RageSpellI
{
	shapeFile = "plasmaex";
	mountPoint = 0;

	mountOffset		= { -0.15, 0, 0 };

	weaponType = 0; // Single Shot
	projectileType = "Unidentified";
	ammoType = "Mana";

	accuFire = false;
	reloadTime = 0.015;
	fireTime = 0;

	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = SoundFireGrenade;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData RageSpell
{
	description = "Rage";
	className = "Weapon";
	shapeFile = "plasmaex";
	hudIcon = "mortargun";
   heading = "bSpells";
	shadowDetailMask = 4;
	imageType = RageSpellI;
	price = 255;
	showWeaponBar = true;
   validateShape = false;
   validateMaterials = true;
};

function RageSpellI::onFire(%player, %slot) 
{
	%playerId = Player::getClient(%player);

	%client = GameBase::getOwnerClient(%player);

	%energy = GameBase::getEnergy(%player);
	if(%energy > 0)
	{
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = GameBase::getRotation(%player);
		Projectile::spawnProjectile("RageBolt",%trans,%player,%vel);

		useEnergy(%player,2.5);
	}
	else
	{
		bottomprint(%client,"<jc><f3>NOT ENOUGH <f0>ENERGY<f3>!",5);
	}
}

$InvList[RageSpell] = 1;
$RemoteInvList[RageSpell] = 1;

$ItemMax[magearmor, RageSpell] = 1;
$ItemMax[magefemale, RageSpell] = 1;

$AutoUse[RageSpell] = True;

$WeaponAmmo[RageSpell] = "Mana";

AddWeapon(RageSpell);

$WeaponSpecial[RageSpell] = true;

function RageSpell::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	bottomprint(%clientId, "<jc><f2>Using " @ %item.description @ " - <f0>2.5 <f2>ENERGY PER BOLT", 2);
}