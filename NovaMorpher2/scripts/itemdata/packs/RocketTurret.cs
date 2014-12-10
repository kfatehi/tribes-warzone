TurretData DepRocketTurret
{
	maxDamage = 0.75;
	maxEnergy = 100;
	minGunEnergy = 30;
	maxGunEnergy = 30;
	range = 100;
	gunRange = 600;
	visibleToSensor = true;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = defaultDebrisLarge;
	className = "Turret";
	shapeFile = "missileturret";
	shieldShapeName = "shield_medium";
	speed = 2.0;
	speedModifier = 2.0;
	projectileType = TurretMissile;
	fireSound = SoundMissileTurretFire;
	activationSound = SoundMissileTurretOn;
	deactivateSound = SoundMissileTurretOff;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
   targetableFovRatio = 0.5;
	explosionId = LargeShockwave;
	description = "Rocket Turret";
};

function DepRocketTurret::onAdd(%this)
{
	schedule("DeployableTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	%this.shieldStrength = 0.025;
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Rocket Turret");
	}
}

function DepRocketTurret::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function DepRocketTurret::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function DepRocketTurret::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "DepRocketTurretPack"]--;
}

// Override base class just in case.
function DepRocketTurret::onPower(%this,%power,%generator) {}
function DepRocketTurret::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}	


function DepRocketTurret::verifyTarget(%this,%target)
{
   if (GameBase::virtual(%target, "getHeatFactor") >= 0.2 && %target.heatSync < 1)
      return "True";
   else
      return "False";
}







//----------------------------------------------------------------------------
																			
ItemImageData DepRocketTurretPackI
{
	shapeFile = "remoteturret";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData DepRocketTurretPack
{
	description = "Rocket Turret";
	shapeFile = "remoteturret";
	className = "Backpack";
   heading = "eTurrets";
	imageType = DepRocketTurretPackI;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 550;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function DepRocketTurretPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function DepRocketTurretPack::onDeploy(%player,%item,%pos)
{
	if (DepRocketTurretPack::deployShape(%player,%item)) {
		Player::decItemCount(%player,%item);
	}
}

function DepRocketTurretPack::deployShape(%player,%item)
{
// %player  = Player Id doing the deploy
// %item    = Item being deployed
// %type    = Type of item - Turret, StaticShape, Beacon - etc
// %name    = Name of item - Ion Turret
// %angle   = Check angel (to mount on walls, etc.) (True/False/Player) Checks angel - Does Not Check - Uses Players Rotation Reguardless
// %freq    = Check Frequency (True/False) = Too Many Of SAME Type Of Item
// %prox    = Check Proximity (True/False)
// %area    = Check Area (for objects in the way) (True/False)
// %surface = Check Surface Type  (True/False)
// %range   = Max deploy distance from player (number best between 3 and 5) meters from player.
// %limit   = Check limit (True/False)
// %flag    = Give Flag Defence Bonus 0 = None and higher for score ammount.
// %deploy  = The item to be deployed (actualy item data name)
// %count   = What item to count

	deployable(%player,%item,"Turret","Deployable Rocket Turret","True","True","False","False","False","6","True", "DepRocketTurret", "DepRocketTurretPack");
}

$TeamItemMax[DepRocketTurretPack] = 10;

$InvList[DepRocketTurretPack] = 1;
$RemoteInvList[DepRocketTurretPack] = 0;

$ItemMax[marmor, DepRocketTurretPack] = 1;
$ItemMax[mfemale, DepRocketTurretPack] = 1;

$ItemMax[harmor, DepRocketTurretPack] = 1;