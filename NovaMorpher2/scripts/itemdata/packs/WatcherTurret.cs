LaserData WatcherLaser
{
   laserBitmapName   = "paintPulse.bmp";
   hitName           = "shotgunex.dts";

   damageConversion  = 0;
   baseDamageType    = $LaserDamageType;

   beamTime          = 0.05;

   lightRange        = 2.0;
   lightColor        = { 1.0, 0.25, 0.25 };

   detachFromShooter = false;
//   hitSoundId        = SoundLaserHit; // Take out the annoyance hehehehe....
};










TurretData WatcherTurret
{
	className = "Turret";
	shapeFile = "camera";
   validateShape = false;
   validateMaterials = true;
	projectileType = WatcherLaser;
	maxDamage = 2;
	maxEnergy = 60;
	minGunEnergy = 0;
	maxGunEnergy = 0.6;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 0.01;
	speed = 20.0;
	speedModifier = 1.5;
	range = 50;
	gunRange = 300;
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
	description = "Watcher";
	damageSkinData = "objectDamageSkins";
};

function WatcherTurret::onAdd(%this)
{
	schedule("DeployableTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,20);
	%this.shieldStrength = 0.1;
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Watcher");
	}
}

function WatcherTurret::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function WatcherTurret::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function WatcherTurret::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
  	$TeamItemCount[GameBase::getTeam(%this) @ "WatcherTurretPack"]--;
}

// Override base class just in case.
function WatcherTurret::onPower(%this,%power,%generator) {}
function WatcherTurret::onEnabled(%this) 
{
	%this.isUpdating=false;
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}	

function WatcherTurret::verifyTarget(%this,%target)
{
	if(!%this.isUpdating)
	{
		%thisTeam = GameBase::getTeam(%this);

		%player = %target;
		%clientId = Player::getClient(%target);

		%armor = Player::getArmor(%clientId);
		%armorName = $ArmorName[%armor];
		%armorDiscription = %armorName.description;

		%name = Client::getName(%clientId);
		%weapon = Player::getMountedItem(%clientId, $WeaponSlot);
		%hp = 100-(GameBase::getDamageLevel(%player)/(%armor.MaxDamage/100));
		%range = floor(Vector::getDistance(GameBase::getPosition(%this), GameBase::getPosition(%player)));

		%thisName = "<jc><f3>Universal Identification System #"@floor(%this-((floor(%this/100))*100))@" <f1>REPORT<f3>:\n";
		%infoA = "<f3>Player Detected: <f1>"@%name@"\n";
		%infoB = "<f3>Player Armor: <f1>"@%armorDiscription@"\n";
		%infoC = "<f3>Player Current Weapon: <f1>"@%weapon.description@"\n";
		%infoD = "<f3>Player Current Health: <f1>"@%hp@"%\n";
		%string = %thisName@%infoA@%infoB@%infoC;

		%this.isUpdating = true;
		schedule(%this@".isUpdating=false;",0.99);

		TeamBottomPrintAll(%thisTeam, %string, 2.5);
	}

	return "True";
}






//----------------------------------------------------------------------------
																			
ItemImageData WatcherTurretPackI
{
	shapeFile = "camera";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData WatcherTurretPack
{
	description = "Watcher";
	shapeFile = "camera";
	className = "Backpack";
   heading = "eTurrets";
	imageType = WatcherTurretPackI;
	shadowDetailMask = 4;
	mass = 0.5;
	elasticity = 0.2;
	price = 550;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function WatcherTurretPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function WatcherTurretPack::onDeploy(%player,%item,%pos)
{
	if (WatcherTurretPack::deployShape(%player,%item)) {
		Player::decItemCount(%player,%item);
	}
}

function WatcherTurretPack::deployShape(%player,%item)
{
	deployable(%player,%item,"Turret","Watcher Turret","False","False","False","False","False","6","True", "WatcherTurret", "WatcherTurretPack");
}

$packDiscription[WatcherTurretPack] = "Roof! Roof! This turret is like a targeting laser without a number. It is an annoying thing for enemies!.";

$NeedPowerCheck[WatcherTurret] = "false"; //== For future uses....
$CanControl[WatcherTurret] = "false"; //== Can a player control this kind of turret?

$TeamItemMax[WatcherTurretPack] = 4; //== Still with-in the "no-lag" zone...

$InvList[WatcherTurretPack] = 1;
$RemoteInvList[WatcherTurretPack] = 1;

$ItemMax[marmor, WatcherTurretPack] = 1;
$ItemMax[mfemale, WatcherTurretPack] = 1;

$ItemMax[harmor, WatcherTurretPack] = 1;