LightningData UnRepairBolt
{
   bitmapName       = "paintPulse.bmp";

   damageType       = $ImpactDamageType;
   boltLength       = 250.0;
   coneAngle        = 35.0;
   damagePerSec      = 0.05;
   energyDrainPerSec = 0;
   segmentDivisions = 1;
   numSegments      = 2;
   beamWidth        = 0.5;

   updateTime   = 450;
   skipPercent  = 0.6;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundRepairItem;
};









TurretData UnRepairTurret
{
	maxDamage = 1.0;
	maxEnergy = 150;
	minGunEnergy = 0;
	maxGunEnergy = 0;
	range = 175;
	gunRange = 250;
	visibleToSensor = true;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = defaultDebrisMedium;
	className = "Turret";
	shapeFile = "chainturret";
	shieldShapeName = "shield";
	speed = 5.0;
	speedModifier = 1.5;
	projectileType = UnRepairBolt;
	reloadDelay = 0;
	explosionId = LargeShockwave;
	description = "UnRepair Turret";

	fireSound        = SoundGeneratorPower;
	activationSound  = SoundChainTurretOn;
	deactivateSound  = SoundChainTurretOff;
	damageSkinData   = "objectDamageSkins";
	shadowDetailMask = 8;

   isSustained     = true;
   firingTimeMS    = 750;
   energyRate      = 3.0;
};

function UnRepairTurret::onAdd(%this)
{
	schedule("DeployableTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,20);
	%this.shieldStrength = 0.025;
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "De-Assembler");
	}
}

function UnRepairTurret::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function UnRepairTurret::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function UnRepairTurret::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "UnRepairTurretPack"]--;
}

// Override base class just in case.
function UnRepairTurret::onPower(%this,%power,%generator) {}
function UnRepairTurret::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}	








//----------------------------------------------------------------------------
																			
ItemImageData UnRepairTurretPackI
{
	shapeFile = "remoteturret";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData UnRepairTurretPack
{
	description = "De-Assembler";
	shapeFile = "remoteturret";
	className = "Backpack";
   heading = "eTurrets";
	imageType = UnRepairTurretPackI;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 550;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function UnRepairTurretPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function UnRepairTurretPack::onDeploy(%player,%item,%pos)
{
	if (UnRepairTurretPack::deployShape(%player,%item)) {
		Player::decItemCount(%player,%item);
	}
}

function UnRepairTurretPack::deployShape(%player,%item)
{
	deployable(%player,%item,"Turret","Repair Turret","False","False","False","False","False","6","True", "UnRepairTurret", "UnRepairTurretPack");
}

$packDiscription[UnRepairTurretPack] = "This elf-like turret was designed to take apart things in a factory.  Unfortunately war has found a new use to it...";

$TeamItemMax[UnRepairTurretPack] = 2;

$InvList[UnRepairTurretPack] = 1;
$RemoteInvList[UnRepairTurretPack] = 0;

$ItemMax[marmor, UnRepairTurretPack] = 1;
$ItemMax[mfemale, UnRepairTurretPack] = 1;

$ItemMax[harmor, UnRepairTurretPack] = 1;