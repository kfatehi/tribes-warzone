RocketData GrecketMissile
{
	bulletShapeName = "rocket.dts";
	explosionTag = rocketExp;

	collisionRadius = 0.0; //== Hopefully never crash hHAHAHAHA... Damn... It crashed!
	mass = 14.0;

	damageClass = 1;		 // 0 impact, 1, radius
	damageValue = 0.25;
	damageType = $ExplosionDamageType;

	explosionRadius = 10.5;
	kickBackStrength = 10.0;

	muzzleVelocity = 50.0;
	terminalVelocity = 50.0;
	acceleration = 1.0;

	totalTime = 8.5;
	liveTime = 18.0;

	lightRange = 5.0;
	lightColor = { 1.0, 0.7, 0.5 };

	inheritedVelocityScale = 0.5;
	trailType = 2;
	trailString = "rsmoke.dts";
	smokeDist = 50;
	soundId = SoundJetHeavy;
};


function GrecketMissile::OnRemove(%this)
{
	%player = $grecketOwner[%this];
	GrecketLauncher::Clear(%player);
}

MineData GrecketGrenade
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = 0.5;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

function GrecketGrenade::onAdd(%this)
{
	AirStrikeShell::CheckRest(%this);
}

//======================================================================== Grecket Launcher
ItemData GrecketAmmo
{
	description = "GrecketAmmo";
	className = "Ammo";
	heading = "xAmmunition";
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 50;
};

ItemImageData GrecketImage 
{
	shapeFile = "mortargun";
	mountPoint = 0;
	mountOffset = { 0.0, 0.0, 0.0};
	mountRotation = { 0, 0, 0 };	
	weaponType = 0;
	ammoType = GrecketAmmo;
	projectileType = "Undefined";
	accuFire = false;
	reloadTime = 0.5;
	fireTime = 0.5;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	//sfxFire = SoundMissileTurretFire;
	sfxReload = SoundMortarReload;
	sfxActivate = SoundPickUpWeapon;
	sfxReady = SoundMortarIdle;
};

function GrecketImage::onFire(%player, %slot) 
{
	%Ammo = Player::getItemCount(%player, $WeaponAmmo[GrecketLauncher]);
	%armor = Player::getArmor(%player);
	%client = GameBase::getOwnerClient(%player);
	
	if($hasGrecket[%player])
	{
		if(!$grecketIsDropping[%player])
		{
			if(GameBase::GetPosition($hasGrecket[%player]) != "0 0 0")
			{
				%hasShot = true;
				for(%i=0; %i<24; %i++)
					schedule("GrecketLauncher::DropBomb("@%player@","@%i@");",(%i*0.1),$hasGrecket[%player]);
			}
		}
	}
	else
	{
		if(%Ammo) 
		{
			Player::decItemCount(%player,$WeaponAmmo[GrecketLauncher],1);
			%trans = GameBase::getMuzzleTransform(%player);
			%vel = Item::getVelocity(%player);
			%fireId = Projectile::spawnProjectile("GrecketMissile",%trans,%player,%vel,%player);
			$hasGrecket[%player] = %fireId;
			$grecketOwner[%fireId] = %player;

			playSound("SoundMissileTurretFire",GameBase::getPosition(%player));
			playSound("SoundMissileTurretFire",GameBase::getPosition(%player));
		}
		else
		{
			Client::sendMessage(Player::getClient(%player), 0,"You have no Ammo for the Grecket Launcher");
			bottomprint(Player::getClient(%player), "You have no Ammo for the Grecket Launcher",5);
		}
	}
}

function GrecketLauncher::Clear(%player)
{
	if($debug)
		echo("Cur: "@$hasGrecket[%player]);

	%this = $grecketOwner[%player];

	$grecketOwner[%this]="";
	$hasGrecket[%player]="";
	$grecketIsDropping[%player]="";

	if($debug)
		echo("After: "@$hasGrecket[%player]);
}

function GrecketLauncher::DropBomb(%player, %num)
{
	$grecketIsDropping[%player] = true;
	%client = GameBase::getOwnerClient(%player);
	if($hasGrecket[%player])
	{

		%object = $hasGrecket[%player];
		%pos = GameBase::GetPosition(%object);
		%newPos = Vector::Add(%pos, "0 0 -0.5");
		%obj = newObject("","Mine","GrecketGrenade");
		GameBase::throw(%obj,%player,0,true);
		if(%num>22)
		{
			GameBase::setPosition(%obj, %pos);
			GrecketLauncher::Clear(%player);
		}
		else
		{
			GameBase::setPosition(%obj, %newPos);
		}
	}
}

ItemData GrecketLauncher
{
	description = "Grecket Launcher";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "mortar";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = GrecketImage;
	price = 375;
	showWeaponBar = true;
};

$InvList[GrecketLauncher] = 1;
$RemoteInvList[GrecketLauncher] = 0;

$ItemMax[amarmor, GrecketLauncher] = 1;
$ItemMax[harmor, GrecketLauncher] = 1;

$InvList[GrecketLauncher] = 1;
$RemoteInvList[GrecketLauncher] = 1;

$InvList[GrecketAmmo] = 1;
$RemoteInvList[GrecketAmmo] = 1;

$ItemMax[amarmor, GrecketAmmo] = 15;
$ItemMax[harmor, GrecketAmmo] = 30;

$AutoUse[GrecketLauncher] = False;

$WeaponAmmo[GrecketLauncher] = "GrecketAmmo";

AddWeapon(GrecketLauncher);

$WeaponSpecial[GrecketLauncher] = "true"; //== Wether this weapon has multiple settings or not...

function GrecketLauncher::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	bottomprint(%clientId, "<jc><f2>Using " @ %item.description @ " - Usage: Press fire once to fire the rocket, then a second time to let out the grenades.", 5);
}