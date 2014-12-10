ExplosionData FlashLightningExp
{
   shapeName = "fusionex.dts";
   soundId   = turretExplosion;

   faceCamera = true;
   randomSpin = true;
   hasLight   = true;
   lightRange = 3.0;

   timeZero = 0.450;
   timeOne  = 1.00;

   colors[0]  = { 0.25, 0.25, 1.0 };
   colors[1]  = { 0.25, 0.25, 1.0 };
   colors[2]  = { 1.0,  1.0,  1.0 };
   radFactors = { 1.0, 1.0, 1.0 };
};

FlierData FlashLightningJet
{
	explosionId						= FlashLightningExp;
	debrisId						= FlashLightningDebris;
	className						= "Vehicle";
	shapeFile						= "flyer";
	shieldShapeName					= "shield_medium";
	mass							= 9.0;
	drag							= 1.0;
	density						= 1.2;
	maxBank						= 32;
	maxPitch						= 32;
	maxSpeed						= 400;
	maxSideSpeed					= 10;
	minSpeed						= -5;
	lift							= 1;
	maxAlt						= 25;
	maxVertical						= 10;
	maxDamage						= 1.75;
	damageLevel						= {1.0, 1.0};
	accel							= 0.6;

	groundDamageScale					= 1;

	reloadDelay						= 1.5;
	maxEnergy						= 30;
	repairRate						= 0;
	damageSound						= SoundFlierCrash;
	ramDamage						= 1.5;
	ramDamageType					= -1;
	mapFilter						= 2;
	mapIcon						= "M_marker";
	visibleToSensor					= true;
	shadowDetailMask					= 2;

	mountSound						= ForceFieldOpen;
	dismountSound					= ForceFieldClose;
	idleSound						= SoundElevatorBlocked;
	moveSound						= SoundElevatorRun;

	visibleDriver					= false;
	driverPose						= 22;
	description						= "Flash Lightning";
};

DebrisData FlashLightningDebris
{
   type      = 0;
   imageType = 0;
   
   mass       = 100.0;
   elasticity = 0.25;
   friction   = 0.5;
   center     = { 0, 0, 0 };

   //collisionMask = 0;    // default is Interior | Terrain, which is what we want
   //knockMask     = 0;

   animationSequence = -1;

   minTimeout = 30.0;
   maxTimeout = 60.0;

   explodeOnBounce = 0.6;

   damage          = 1000.0;
   damageThreshold = 100.0;

   spawnedDebrisMask     = 1;
   spawnedDebrisStrength = 90;
   spawnedDebrisRadius   = 0.2;

   spawnedExplosionID = FlashLightningExp;

   p = 1;

   explodeOnRest   = false;
   collisionDetail = 0;
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
