//==================================================//
//== This is the only pack NovaMorpher can use... ==//
//==================================================//
RocketData MorphJetShell
{
   bulletShapeName = "enbolt.dts";
   explosionTag    = energyExp;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.25;
   damageType       = $ExplosionDamageType;

   explosionRadius  = 3;
   kickBackStrength = 150.0;

   muzzleVelocity   = 65.0;
   terminalVelocity = 80.0;
   acceleration     = 5.0;

   totalTime        = 6.5;
   liveTime         = 8.0;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 1;

   // rocket specific
   trailType   = 1;
   trailLength = 15;
   trailWidth  = 0.3;

   soundId = SoundDiscSpin;
};


FlierData MorpherJet
{
        explosionId = flashExpLarge;
        debrisId = flashDebrisLarge;
        className = "Vehicle";
	shapeFile = "flyer";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 0.5;
	density = 1.2;
	maxBank = 64;
	maxPitch = 64;
	maxSpeed = 80;
	minSpeed = -20;
        maxSideSpeed = 40;
        lift = 1;
        maxAlt = 25;
        maxVertical = 10;
        maxDamage = 0.66;
        damageLevel = {1.0, 1.0};
        maxEnergy = 30;
        accel = 0.8;

        groundDamageScale = 0.01;

        projectileType = MorphJetShell;
        reloadDelay = 0.05;
        repairRate = 0.1;
        fireSound = SoundMissileTurretFire;
        damageSound = SoundFlierCrash;
        ramDamage = 2;
        ramDamageType = -1;
        mapFilter = 2;
        mapIcon = "M_vehicle";
        visibleToSensor = true;
        shadowDetailMask = 2;

        idleSound = SoundFlyerIdle;
        moveSound = SoundFlyerActive;

        visibleDriver = false;
        driverPose = 22;
        description = "MorpherJet";
};

$Patch::DeathVehicles[MorpherJet] = true;
function MorpherJet::onDestroy(%this,%cl)
{
	%shooterClient = %this.lastDamager;
	%type = %this.lastDamageType;
	Player::onDamage(Client::getOwnedObject(%cl),%type,999999,"","","" ,"torso" ,"front_right" ,%shooterClient);
}

//== Turret #1 - Plasma

TurretData MorpherPlasmaTurret
{
	maxDamage = 1.0;
	maxEnergy = 200;
	minGunEnergy = 1.4;
	maxGunEnergy = 1.5;
	reloadDelay = 0.1;
	fireSound = SoundPlasmaTurretFire;
	activationSound = SoundPlasmaTurretOn;
	deactivateSound = SoundPlasmaTurretOff;
	whirSound = SoundPlasmaTurretTurn;
	range = 100;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	visibleToSensor = true;
	debrisId = defaultDebrisMedium;
	className = "Turret";
	shapeFile = "hellfiregun";
	shieldShapeName = "shield_medium";
	speed = 2.0;
	speedModifier = 2.0;
	projectileType = FusionBolt;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
	explosionId = LargeShockwave;
	description = "Plasma Turret";
};


//========== Blast Wall Transformation :-P
TurretData MorpherBlastWall
{
	maxDamage = 12.0;
	maxEnergy = 200;
	minGunEnergy = 1000;
	maxGunEnergy = 1000;
	reloadDelay = 1000;
	fireSound = SoundPlasmaTurretFire;
	activationSound = SoundPlasmaTurretOn;
	deactivateSound = SoundPlasmaTurretOff;
	whirSound = SoundPlasmaTurretTurn;
	range = 0;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	visibleToSensor = true;
	debrisId = defaultDebrisMedium;
	className = "Turret";
	shapeFile = "newdoor5";
	shieldShapeName = "shield_medium";
	speed = 0;
	speedModifier = 0;
	projectileType = "";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
	explosionId = LargeShockwave;
	description = "Blast Wall Morph";
};