FlierData DroneBot
{
	explosionId = LargeShockwave;
	debrisId = defaultDebrisSmall;
	className = "Vehicle";
	shapeFile = "mortar";
	shieldShapeName = "shield_medium";
	mass = 0.0001;
	drag = 0.0001;
	density = 1.2;
	maxBank = 0.4;
	maxPitch = 0.4;
	maxSpeed = 40;
	minSpeed = -100;
	lift = 0.25;
	maxAlt = 25;
	maxVertical = 10;
	maxDamage = 0.01;
	damageLevel = {5.0, 5.0};
	maxEnergy = 100;
	accel = 0.4;

	groundDamageScale = 1.0;

	projectileType = BlasterBolt;
	reloadDelay = 0.1;
	repairRate = 1;
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
	description = "Scout";
};


ItemData FlashLightning
{
	description = "Flash Lightning";
	className = "Vehicle";
	heading = "aVehicle";
	price = 3251;
};

$TeamItemMax[FlashLightning] = 2;

$VehicleInvList[FlashLightning] = 1;
$DataBlockName[FlashLightning] = FlashLightningJet;
$VehicleToItem[FlashLightningJet] = FlashLightning;
