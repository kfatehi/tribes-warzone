FlierData AdaptorJet
{
	explosionId = flashExpLarge;
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "flyer";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 0.5;
	maxPitch = 0.5;
	maxSpeed = 50;
	minSpeed = -2;
	lift = 0.75;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.6;
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = "Undefined";
	reloadDelay = 2.0;
	repairRate = 0;
	fireSound = SoundFireFlierRocket;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;

	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;

	visibleDriver = true;
	driverPose = 22;
	description = "Adaptor";
};

function AdaptorJet::OnFire(%this)
{
	%client = gamebase::getcontrolclient(%this);
	%player = Client::getOwnedObject(%client);

	%CurWeapon = Client::getOwnedObject(%client).CurWeapon;
	%CurWeaponImg = %CurWeapon.imageType;
	%projection = $WeaponProjection[%CurWeapon];

	if($isFire[%this]){}
	else
	{
		if(%projection == "")
		{
			Client::sendMessage(%client,1,"Can not adapt to this kind of weaponary, please switch your current weapon. ~wError_Message.wav");
			%time = 1.5;
		}
		else
		{
			%rot = gamebase::getrotation(%this);
			%dir = (Vector::getfromrot(%rot));
			%pos = gamebase::getposition(%this);
			%y1 = Vector::getFromRot(%rot, 17);
			%pos1 = Vector::add(%pos, %y1);

			%trans =  %rot @ " " @ %dir @ " 0 0 0 " @ %pos1;

			%vel = Item::getVelocity(%this);

			Projectile::spawnProjectile(%projection, %trans ,%player,%vel);
			playSound(%CurWeaponImg.sfxFire,GameBase::getPosition(%this));
			%time = %CurWeaponImg.reloadTime + %CurWeaponImg.fireTime;
		}

		$isFire[%this] = true;
		if(%time<=0)
			%time = 0.01;

		if($debug)
			echo("%time = " @ %time);

		schedule("AdaptorJet::enableFire("@%this@");", %time, %this);
	}
	return;
}

function AdaptorJet::enableFire(%this)
{
	$isFire[%this] = false;
}


ItemData Adaptor
{
	description = "*-|-Adaptor-|-*";
	className = "Vehicle";
	heading = "aVehicle";
	price = 3251;
};

$TeamItemMax[Adaptor] = 2;

$VehicleInvList[Adaptor] = 1;
$DataBlockName[Adaptor] = AdaptorJet;
$VehicleToItem[AdaptorJet] = Adaptor;

function AdaptorJet::addAdapt(%weapon, %projection)
{
	$WeaponProjection[%weapon] = %projection;
}

$Vehicle::canSwitchWeapons[AdaptorJet] = true;

//== Adding adaption:
AdaptorJet::addAdapt(blaster, BlasterBolt);
AdaptorJet::addAdapt(Chaingun, ChaingunBullet);
AdaptorJet::addAdapt(PlasmaGun, PlasmaBolt);
AdaptorJet::addAdapt(GrenadeLauncher, GrenadeShell);
AdaptorJet::addAdapt(Mortar, MortarShell);
AdaptorJet::addAdapt(DiscLauncher, DiscShell);
AdaptorJet::addAdapt(RocketLauncher, StingerMissile);
AdaptorJet::addAdapt(Pistol, PistolBullet);
AdaptorJet::addAdapt(SniperRifle, SniperBullet);


