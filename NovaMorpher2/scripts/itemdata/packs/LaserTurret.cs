TurretData LaserTurret
{
	className = "Turret";
	shapeFile = "camera";
	projectileType = SniperLaser;
	maxDamage = 0.75;
	maxEnergy = 75;
	minGunEnergy = 30;
	maxGunEnergy = 90;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 10.0;
	speed = 4.0;
	speedModifier = 1.5;
	range = 75;
	visibleToSensor = true;
	shadowDetailMask = 4;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundFireLaser;
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "Laser Turret";
	damageSkinData = "objectDamageSkins";
};

function LaserTurret::onAdd(%this)
{
	schedule("DeployableTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,20);
	%this.shieldStrength = 0.01;
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Laser");
	}
}

function LaserTurret::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function LaserTurret::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function LaserTurret::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "LaserTurretPack"]--;
}

// Override base class just in case.
function LaserTurret::onPower(%this,%power,%generator) {}
function LaserTurret::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}	








//----------------------------------------------------------------------------
																			
ItemImageData LaserTurretPackI
{
	shapeFile = "camera";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData LaserTurretPack
{
	description = "Laser Turret";
	shapeFile = "camera";
	className = "Backpack";
   heading = "eTurrets";
	imageType = LaserTurretPackI;
	shadowDetailMask = 4;
	mass = 0.5;
	elasticity = 0.2;
	price = 550;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function LaserTurretPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function LaserTurretPack::onDeploy(%player,%item,%pos)
{
	if (LaserTurretPack::deployShape(%player,%item)) {
		Player::decItemCount(%player,%item);
	}
}

function LaserTurretPack::deployShape(%player,%item)
{
	deployable(%player,%item,"Turret","Laser Turret","False","False","False","False","False","6","True", "LaserTurret", "LaserTurretPack");
}

$packDiscription[LaserTurretPack] = "This design is basically a remodeling of the Watcher Turret. Instead of shooting millions of bolts of light, It shoots 1 bolt of light to damage and even kill its enemy!";

Patch::AddReInit(LaserTurretPack);

$NeedPowerCheck[LaserTurret] = "false"; //== For future uses....
$CanControl[LaserTurret] = "true"; //== Can a player control this kind of turret?

$TeamItemMax[LaserTurretPack] = 6; //== Still with-in the "no-lag" zone...

$InvList[LaserTurretPack] = 1;
$RemoteInvList[LaserTurretPack] = 1;

$ItemMax[marmor, LaserTurretPack] = 1;
$ItemMax[mfemale, LaserTurretPack] = 1;

$ItemMax[harmor, LaserTurretPack] = 1;

Patch::AddReInit("LaserTurretPack");