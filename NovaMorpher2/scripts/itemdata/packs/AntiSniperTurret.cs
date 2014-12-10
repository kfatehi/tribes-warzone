
TurretData AntiSniperTurret
{
	className = "Turret";
	shapeFile = "remoteturret";
	validateShape = false;
	validateMaterials = true;
	projectileType = SniperBullet;
	maxDamage = 25.0;
	maxEnergy = 50;
	minGunEnergy = 10;
	maxGunEnergy = 50;
	reloadDelay = 1.0;
	speed = 100.0;
	speedModifier = 10;
	range = 400; //== Ya ya!! =)
	visibleToSensor = false;
	shadowDetailMask = 4;
	dopplerVelocity = 0;
	castLOS = true;
	supression = true;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundRemoteTurretFire;
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "Anti-Sniper Turret";
	damageSkinData = "objectDamageSkins";
};


function AntiSniperTurret::onAdd(%this)
{
	schedule("DeployableTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Anti-Sniper Turret");
	}
}

function AntiSniperTurret::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function AntiSniperTurret::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function AntiSniperTurret::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "AntiSniperTurretPack"]--;
}

// Override base class just in case.
function AntiSniperTurret::onPower(%this,%power,%generator) {}
function AntiSniperTurret::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}	

function AntiSniperTurret::verifyTarget(%this,%target)
{
	%targetId = Player::getClient(%target);
	%targetArmor = Player::getArmor(%targetId);
	%curWeapon = %target.CurWeapon;

	if (%targetArmor == "sniperxarmor" || %targetArmor == "sniperxfemale" || $isSniperRifle[%curWeapon])
		return "True";
	else
		return "False";
}

$isSniperRifle[laserrifle] = true;






//----------------------------------------------------------------------------
																			
ItemImageData AntiSniperTurretPackI
{
	shapeFile = "remoteturret";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData AntiSniperTurretPack
{
	description = "Anti-Sniper Turret";
	shapeFile = "remoteturret";
	className = "Backpack";
   heading = "eTurrets";
	imageType = AntiSniperTurretPackI;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 550;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function AntiSniperTurretPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function AntiSniperTurretPack::onDeploy(%player,%item,%pos)
{
	if (AntiSniperTurretPack::deployShape(%player,%item)) {
		Player::decItemCount(%player,%item);
	}
}

function AntiSniperTurretPack::deployShape(%player,%item)
{
	deployable(%player,%item,"Turret","Anti-Sniper Turret","True","True","False","False","False","6","True", "AntiSniperTurret", "AntiSniperTurretPack");
}

$packDiscription[AntiSniperTurretPack] = "Can't help it if I hate snipers! Well this turret has no shields, it will only attack enemies with sniper armor. No one can excape it!";

$TeamItemMax[AntiSniperTurretPack] = 1;

$InvList[AntiSniperTurretPack] = 1;
$RemoteInvList[AntiSniperTurretPack] = 0;

$ItemMax[marmor, AntiSniperTurretPack] = 1;
$ItemMax[mfemale, AntiSniperTurretPack] = 1;

$ItemMax[harmor, AntiSniperTurretPack] = 1;