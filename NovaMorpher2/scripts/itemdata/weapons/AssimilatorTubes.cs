//======================================================================== ATProj
$HalfDamageType = DamageTypes::AddReserve();
$deathMsg[$HalfDamageType, 0]      = "%2 was assimulated by borgs.";
$deathMsg[$HalfDamageType, 1]      = "%1 said WE WILL ASSIMULATE YOU, and %1 did...";
$deathMsg[$HalfDamageType, 2]      = "%2 tried to make peace with the borgs but failed.";
$deathMsg[$HalfDamageType, 3]      = "%2 liked the assimulator tubes a bit too much.";
$deathMsg[$HalfDamageType, 4]      = "%1 used the TUBES OF MIGHT against %2";

$SpecialDamageType[$HalfDamageType] = "HalfDamageType";
function HalfDamageType::DoSpecialDMG(%this, %shooterPlayer)
{
	%shooterClient = Player::getClient(%shooterPlayer);
	%damagedClient = Player::getClient(%damagedPlayer);
	%damagedPlTeam = Client::getTeam(%damagedClient);
	%shooterClTeam = Client::getTeam(%shooterClient);
	if(%shooterClTeam != %damagedPlTeam)
	{
		%type = GameBase::getDataName(%this);
		%maxDamage = %type.maxDamage;

		%dmgLevel = GameBase::getDamageLevel(%this);
		%newdmgLevel = (%maxDamage-%dmgLevel)/2;

		schedule("StaticShape::shieldDamage("@%this@","@1000@","@%newdmgLevel@",'','','',"@%shooterPlayer@");",0.01);
	}
}


RocketData ATProj
{
   bulletShapeName = "breath.dts";
   explosionTag    = bulletExp1;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.01;
   damageType       = $HalfDamageType;

   explosionRadius  = 5;
   kickBackStrength = 100.0;

   muzzleVelocity   = 200.0;
   terminalVelocity = 2000.0;
   acceleration     = 50.0;

   totalTime        = 1.0;
   liveTime         = 3.5;

   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };

   inheritedVelocityScale = 0.5;
   // rocket specific
   trailType   = 1;
   trailLength = 15;
   trailWidth  = 5;

   soundId = SoundJetHeavy;
};

//======================================================================== AT Launcher
ItemData ATAmmo 
{
	description = "Nano Org";
	className = "Ammo";
	heading = "xAmmunition";
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 50;
};

ItemImageData SubATImage 
{
	shapeFile = "mrtwig";
	mountPoint = 0;
	mountRotation = { -1.5, 0, 0 };
	mountPoint = 0;
	weaponType = 0;
	ammoType = ATAmmo;
	projectileType = ATProj;
	accuFire = true;
	reloadTime = 0.9;
	fireTime = 0.1;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	//sfxFire = SoundMissileTurretFire;
	sfxReload = SoundMortarReload;
	sfxActivate = SoundPickUpWeapon;
	sfxReady = SoundMortarIdle;
};

ItemData SubAT
{
	description = "Assimulator Tubes";
	className = "Weapon";
	shapeFile = "mrtwig";
	hudIcon = "mortar";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = SubATImage;
	price = 375;
	showWeaponBar = true;
};


ItemImageData ATImage 
{
	shapeFile = "breath";
	mountPoint = 0;
	weaponType = 0;
	ammoType = ATAmmo;
	projectileType = ATProj;
	accuFire = true;
	reloadTime = 0.5;
	fireTime = 0.0;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	//sfxFire = SoundMissileTurretFire;
	sfxReload = SoundMortarReload;
	sfxActivate = SoundPickUpWeapon;
	sfxReady = SoundMortarIdle;
};

ItemData AT
{
	description = "Assimulator Tubes";
	className = "Weapon";
	shapeFile = "mrtwig";
	hudIcon = "mortar";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = ATImage;
	price = 375;
	showWeaponBar = true;
};

function AT::onMount(%player,%item,$WeaponSlot)
{
	%clientId = Player::getClient(%player);
	if(Borg::HaveOtherTech(%clientId))
	{
		Player::unmountItem(%player,$WeaponSlot);
		Client::sendMessage(%clientId,3,"Cannot use this with NONE-borg technology!");
		return;
	}

	Player::mountItem(%player,SubAT,$ExtraWeaponSlotA);
}

function AT::onUnmount(%player,%item,$WeaponSlot)
{
	Player::unmountItem(%player,$ExtraWeaponSlotA);
}
	
$InvList[AT] = 1;
$RemoteInvList[AT] = 1;

$ItemMax[borgarmor, AT] = 1;
$ItemMax[borgfemale, AT] = 1;

$InvList[ATAmmo] = 1;
$RemoteInvList[ATAmmo] = 1;

$ItemMax[borgfemale, ATAmmo] = 250;
$ItemMax[borgarmor, ATAmmo] = 250;

$AutoUse[AT] = True;

$WeaponAmmo[AT] = "ATAmmo";

AddWeapon(AT);