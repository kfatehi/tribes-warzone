BulletData UTFBomb
{
   bulletShapeName    = "mortar.dts";
   validateShape      = false;
   explosionTag       = mortarExp;
   expRandCycle       = 3;
   mass               = 0.001;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 100;
   damageType         = $MortarDamageType;

   aimDeflection      = 0;
   muzzleVelocity     = 9999.0; // Holy Shit.....
   totalTime          = 2;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 0.0;
};

TurretData UTFTurret
{
	className = "Turret";
	shapeFile = "camera";
   validateShape = false;
   validateMaterials = true;
	projectileType = UTFBomb;
	maxDamage = 2;
	maxEnergy = 60;
	minGunEnergy = 30;
	maxGunEnergy = 60;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 2;
	speed = 20.0;
	speedModifier = 1.5;
	range = 100;
	visibleToSensor = true;
	shadowDetailMask = 4;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundRemoteTurretFire;
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "Ultimate Flag Killer";
	damageSkinData = "objectDamageSkins";
};

function UTFTurret::onAdd(%this)
{
	schedule("DeployableTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,20);
	%this.shieldStrength = 0.01;
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Ultimate Flag Assassin");
	}
}

function UTFTurret::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function UTFTurret::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function UTFTurret::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "UTFTurretPack"]--;
}

// Override base class just in case.
function UTFTurret::onPower(%this,%power,%generator) {}
function UTFTurret::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}	

function UTFTurret::verifyTarget(%this,%target)
{
	if (Player::getItemCount(%target, "Flag") == "1" )
		return "True";
	else
		return "False";
}

$CanControl[UTFTurret] = "false"; //== Can a player control this kind of turret?





//----------------------------------------------------------------------------
																			
ItemImageData UTFTurretPackI
{
	shapeFile = "camera";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData UTFTurretPack
{
	description = "UFA Pack";
	shapeFile = "camera";
	className = "Backpack";
   heading = "eTurrets";
	imageType = UTFTurretPackI;
	shadowDetailMask = 4;
	mass = 0.5;
	elasticity = 0.2;
	price = 550;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function UTFTurretPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function UTFTurretPack::onDeploy(%player,%item,%pos)
{
	if (UTFTurretPack::deployShape(%player,%item)) {
		Player::decItemCount(%player,%item);
	}
}

function UTFTurretPack::deployShape(%player,%item)
{
	deployable(%player,%item,"Turret","UFA Turret","False","False","False","False","True","6","True", "UTFTurret", "UTFTurretPack");
}

$packDiscription[UTFTurretPack] = "This baby can kill any enemies with a flag in 1 hit! Great Flag D, don't try it for anything else ;)";

$NeedPowerCheck[UTFTurret] = "false"; //== For future uses....
$CanControl[UTFTurret] = "false"; //== Can a player control this kind of turret?

$TeamItemMax[UTFTurretPack] = 2; //== Still with-in the "no-lag" zone...

$InvList[UTFTurretPack] = 1;
$RemoteInvList[UTFTurretPack] = 1;

$ItemMax[marmor, UTFTurretPack] = 1;
$ItemMax[mfemale, UTFTurretPack] = 1;

$ItemMax[harmor, UTFTurretPack] = 1;