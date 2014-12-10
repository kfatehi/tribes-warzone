//======================================================================== Lock-Jaw Missle 
SeekingMissileData MiniMissileTracker 
{
	bulletShapeName = "rocket.dts";
	explosionTag = rocketExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1;
	damageValue = 0.25;
	damageType = $MissileDamageType;
	explosionRadius = 10.0;
	kickBackStrength = 100.0;
	muzzleVelocity = 100.0;
	terminalVelocity = 1000.0;
	acceleration = 200.0;
	totalTime = 15.0;
	liveTime = 15.0;
	lightRange = 5.0;
	lightColor = { 1.0, 0.7, 0.5 };
	inheritedVelocityScale = 0.5;
	seekingTurningRadius = 3.6;
	nonSeekingTurningRadius = 3.6;
	proximityDist = 0.5;
	lightRange = 5.0;
	lightColor = { 0.4, 0.4, 1.0 };
	smokeDist = 4.5;
	inheritedVelocityScale = 0.5;
	soundId = SoundJetHeavy;
};

function MiniMissileTracker::updateTargetPercentage(%target)
{
	return true;
}

//======================================================================== HeatSeek Missle 
SeekingMissileData HeatSeekMissle 
{
	bulletShapeName = "rocket.dts";
	explosionTag = rocketExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1;
	damageValue = 0.65;
	damageType = $MissileDamageType;
	explosionRadius = 5.0;
	kickBackStrength = 100.0;
	muzzleVelocity = 100.0;
	terminalVelocity = 1000.0;
	acceleration = 50.0;
	totalTime = 15.0;
	liveTime = 15.0;
	lightRange = 5.0;
	lightColor = { 1.0, 0.7, 0.5 };
	inheritedVelocityScale = 0.5;
	seekingTurningRadius = 179;
	nonSeekingTurningRadius = 179;
	proximityDist = 0.5;
	lightRange = 5.0;
	lightColor = { 0.4, 0.4, 1.0 };
	smokeDist = 4.5;
	inheritedVelocityScale = 0.5;

	trailType = 2;
	trailString = "plasmatrail.dts";
	smokeDist = 18;
	inheritedVelocityScale = 0.5;


	soundId = SoundJetHeavy;
};
function HeatSeekMissle::updateTargetPercentage(%target)
{
	if(GameBase::virtual(%target, "getHeatFactor") > 0.75)
		return true;

	return false;
}

//======================================================================== Locking Missle 
SeekingMissileData StingerMissileTracker 
{
	bulletShapeName = "rocket.dts";
	explosionTag = rocketExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1;
	damageValue = 0.75;
	damageType = $MissileDamageType;
	explosionRadius = 8.5;
	kickBackStrength = 220.0;

	muzzleVelocity   = 100.0;
	terminalVelocity = 1000.0;
	acceleration     = 25.0;

	totalTime = 4.0;
	liveTime = 21.0;
	lightRange = 5.0;
	lightColor = { 1.0, 0.7, 0.5 };
	inheritedVelocityScale = 0.5;
	seekingTurningRadius = 179;
	nonSeekingTurningRadius = 179;
	proximityDist = 1.5;
	lightRange = 5.0;
	lightColor = { 0.4, 0.4, 1.0 };
	trailType = 2;
	trailString = "plasmatrail.dts";
	smokeDist = 5;
	inheritedVelocityScale = 0.5;
	soundId = SoundJetHeavy;
};

//======================================================================== Stinger Missile
RocketData StingerMissile
{
   bulletShapeName = "rocket.dts";
   explosionTag    = rocketExp;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.75;
   damageType       = $ExplosionDamageType;

   explosionRadius  = 12.5;
   kickBackStrength = 100.0;

   muzzleVelocity   = 200.0;
   terminalVelocity = 2000.0;
   acceleration     = 50.0;

   totalTime        = 8.5;
   liveTime         = 18.0;

   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };

   inheritedVelocityScale = 0.5;
   trailType   = 2;
   trailString = "rsmoke.dts";
   smokeDist   = 1.8;
   soundId = SoundJetHeavy;
};


//===============================================================================================================
//													 Lock Jaw
//===============================================================================================================

RepairEffectData LockJaw
{
   bitmapName       = "lightningNew.bmp";
   boltLength       = 700.0;
   segmentDivisions = 2;
   beamWidth        = 0.05;
   updateTime   = 150;
   skipPercent  = 1.0;
   displaceBias = 0.25;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };
   soundId = SoundELFFire;
};

function LockJaw::onAcquire(%this, %player, %target)
{
	%client = Player::getClient(%player);
	if ((!%client.target || %client.target == "-1" || %client.target == %player))
		%client.target = %target;

	%name = GameBase::getDataName(%target);
	%team = GameBase::getTeam(%target);
	%pTeam = GameBase::getTeam(%player);
	%pName = Client::getName(%client);

	if (!%client.target || %client.target == "-1" || %client.target == %player || (GameBase::virtual(%target, "getHeatFactor") < 0.5 && %target.heatSync > 0.5))
	{
		schedule("bottomprint(" @ %client @ ", \"<jc><f1>No Lock, Attempting To Acquire!\", 3);", 0);
		schedule("Client::sendMessage(" @ %client @ ",1,\"No Lock, Attempting To Acquire!~waccess_denied.wav\");",0);
		%client.target = -1;
		return;
	}
	if (%client.target != "-1")
	{
		if (gamebase::getteam(%client.target) == gamebase::getteam(%player))
		{
			schedule("bottomprint(" @ %client @ ", \"<jc><f1>Lock Failed, Can Not Target Friendlies!\", 3);", 0);
			schedule("Client::sendMessage(" @ %client @ ",1,\"Lock Failed, Can Not Target Friendlies!~waccess_denied.wav\");",0);
			%client.target = -1;
			return;
		}
		else if(%target.heatSync < 0.5)
		{
			schedule("Client::sendMessage(" @ %client @ ",1,\"** LockJaw Aquired - Active For Next 7 Seconds!!!~wmine_act.wav\");",0);			
			schedule("Client::sendMessage(" @ %client @ ",1,\"** LockJaw Jammed - Lost target!!!~wmine_act.wav\");",0);			
			schedule("bottomprint(" @ %client @ ", \"<jc><f1>LockJaw Jammed!\", 3);", 0);
			%client.target = -1;
			return;
		}
		schedule("Client::sendMessage(" @ %client @ ",1,\"** LockJaw Aquired - Active For Next 7 Seconds!!!~wmine_act.wav\");",0);			
		schedule("bottomprint(" @ %client @ ", \"<jc><f1>LockJaw Aquired - Active For Next 7 Seconds! - Fire Now To Launch!\", 3);", 0);
		schedule("bottomprint(" @ %client @ ", \"<jc><f1>LockJaw Lost Lock!\", 3);", 7);
		schedule("" @ %client @ ".target = \"-1\";", 7);
		schedule ("playSound(TargetingMissile,GameBase::getPosition(" @ %player @ "));",0);
		LockJaw(%client,%targetId);			
	}
}
function LockJaw(%clientId, %targetId) 
{
	if(%targetId) 
	{
		%name = Client::getName(%clientId);
		 Client::sendMessage(%targetId,0,"** WARNING ** - " @ %name @ " has you in LockJaw!!!~waccess_denied.wav");
		 schedule("Client::sendMessage(" @ %targetId @ ",0,\"~waccess_denied.wav\");",0.25);
		 schedule("Client::sendMessage(" @ %targetId @ ",0,\"~waccess_denied.wav\");",0.5);
		 schedule("Client::sendMessage(" @ %targetId @ ",0,\"~waccess_denied.wav\");",0.75);
	}
} 
function LockJawFire(%player, %target)
{
	if ($debug) echo ("Firing Locker");
	
	%client = Player::getClient(%player);

	Player::decItemCount(%player,$WeaponAmmo[RocketLauncher],1);
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);	
	
	$targetingmissile = %client.target;
	Projectile::spawnProjectile("MiniMissileTracker",%trans,%player,%vel,%client.target);
	schedule ("playSound(SoundMissileTurretFire,GameBase::getPosition(" @ %player @ "));",0);
	schedule("Client::sendMessage(" @ %target @ ",0,\"~waccess_denied.wav\");",0.5);

}

function LockJaw::onRelease(%this, %player)
{
	%client = Player::getClient(%player);
	%object = %player.target;
	
	if ($debug) echo ("Object Target " @ %player.target);
	if ($debug) echo ("Shootr Client " @ %client);
}

function Tracker(%clientId, %targetId, %delay) 
{
	if(%targetId) 
	{
		%name = Client::getName(%clientId);
		 Client::sendMessage(%targetId,0,"** WARNING ** - " @ %name @ " has a Missile Lock!~waccess_denied.wav");
		 schedule("Client::sendMessage(" @ %targetId @ ",0,\"~waccess_denied.wav\");",0.25);
		 schedule("Client::sendMessage(" @ %targetId @ ",0,\"~waccess_denied.wav\");",0.5);
		 schedule("Client::sendMessage(" @ %targetId @ ",0,\"~waccess_denied.wav\");",0.75);
	}
} 










//======================================================================== Rocket Launcher
ItemData RocketAmmo 
{
	description = "RocketAmmo";
	className = "Ammo";
	heading = "xAmmunition";
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 50;
};

ItemImageData RocketImage 
{
	shapeFile = "mortargun";
	mountPoint = 0;
	mountOffset = { 0.0, 0.0, -0.2 };
	mountRotation = { 0, 0, 0 };	
	weaponType = 0;
	ammoType = RocketAmmo;
	projectileType = "Undefined";
	accuFire = false;
	reloadTime = 1.0;
	fireTime = 0.5;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	//sfxFire = SoundMissileTurretFire;
	sfxReload = SoundMortarReload;
	sfxActivate = SoundPickUpWeapon;
	sfxReady = SoundMortarIdle;
};
	
function RocketImage::onFire(%player, %slot) 
{
	%Ammo = Player::getItemCount(%player, $WeaponAmmo[RocketLauncher]);
	%armor = Player::getArmor(%player);
	%client = GameBase::getOwnerClient(%player);
	
	 if(%Ammo) 
	 {
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
	
		if ($Settings::RocketLauncher[%client] == "" || $Settings::RocketLauncher[%client] == 0)	//== Normal Rocket Launcher
		{	
			%client = GameBase::getOwnerClient(%player);
			%trans = GameBase::getMuzzleTransform(%player);
			%vel = Item::getVelocity(%player);
			%fired = false;
			if(GameBase::getLOSInfo(%player,450))
			{

				%object = getObjectType($los::object);
				%targetId = GameBase::getOwnerClient($los::object);

				%armor = Player::getArmor(%targetId);
				%name = Client::getName(%targetId);

				if(%name != "" && %targetId != %client && %target.heatSync < 1) 
				{
					Tracker(%client,%targetId);

					Client::sendMessage(%client,0,"** Lock Aquired - " @ %name @ "~wmine_act.wav");
					Projectile::spawnProjectile("StingerMissileTracker",%trans,%player,%vel,$los::object);
					schedule ("playSound(SoundMissileTurretFire,GameBase::getPosition(" @ %player @ "));",0);
					%fired = true;
				}
			}
			if(!%fired)
			{
				Projectile::spawnProjectile("StingerMissile",%trans,%player,%vel,%player);
				schedule ("playSound(SoundMissileTurretFire,GameBase::getPosition(" @ %player @ "));",0);
			}
			Player::decItemCount(%player,$WeaponAmmo[RocketLauncher],1);
		}
		else if ($Settings::RocketLauncher[%client] == "1")
		{
			%client = GameBase::getOwnerClient(%player);
			GameBase::getLOSInfo(%player,400);
			%tpos=($los::position);
			%target=AquireTarget(%player,0,%tpos,75);
			if(!(%target > 8000))
				%target=AquireTarget(%player,1,GameBase::getPosition(%player),75);

			%hasHeatSink = %target.heatSync;
			%hasHeat = (GameBase::virtual(%target, "getHeatFactor") >= 0.75);
			%heat = %hasHeat && !%hasHeatSink;
			%notSelf = %target != %player;
			%isTrue = (%heat && %notSelf && %target);

			if(%isTrue)
			{	
				%targetcl=Player::GetClient(%target);	 

				Projectile::spawnProjectile("HeatSeekMissle",%trans,%player,%vel,%target);
				Player::decItemCount(%player,$WeaponAmmo[RocketLauncher],1);
				%name = Client::getName(%target);
				SendLockWarning(%TargetCl,1);
				Client::sendMessage(%client,0,"~wmine_act.wav");
			}
			else
			{	
				schedule("bottomprint(" @ %client @ ", \"<jc><f1>Unable To Acquire Target!\", 3);", 0);		
				schedule("Client::sendMessage(" @ %client @ ",1,\"Unable acquire target!~waccess_denied.wav\");",0);
			}

		}
		else if ($Settings::RocketLauncher[%client] == "2")			//== Lock-Jaw Rocket
		{			
			if (!%client.target || %client.target == "-1" || %client.target == %player)
			{
				if ($debug) echo ("Spawning Lock Jaw");
				Projectile::spawnProjectile("LockJaw",%trans,%player,%vel);
			}
			else if (%client.target != "-1")
			{
				if ($debug) echo ("Spawning Missile");			
				if (gamebase::getteam(%client.target) == gamebase::getteam(%player))
				{
					schedule("bottomprint(" @ %client @ ", \"<jc><f1>Lock Failed, Can Not Target Friendlies!\", 3);", 0);
					%client.target = -1;
					return;
				}
				else
				{
					$targeting = %player;
					LockJawFire(%player, %target);
				}
			}	
		}
	}
	else
	{
		Client::sendMessage(Player::getClient(%player), 0,"You have no Ammo for the Rocket Launcher");
		bottomprint(Player::getClient(%player), "You have no Ammo for the Rocket Launcher",5);
	}
}

ItemData RocketLauncher
{
	description = "Rocket Launcher";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "mortar";
	heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = RocketImage;
	price = 375;
	showWeaponBar = true;
};

$InvList[RocketLauncher] = 1;
$RemoteInvList[RocketLauncher] = 0;

$ItemMax[amarmor, RocketLauncher] = 1;
$ItemMax[harmor, RocketLauncher] = 1;
$ItemMax[marmor, RocketLauncher] = 1;
$ItemMax[mfemale, RocketLauncher] = 1;

$InvList[RocketAmmo] = 1;
$RemoteInvList[RocketAmmo] = 1;

$ItemMax[amarmor, RocketAmmo] = 30;
$ItemMax[harmor, RocketAmmo] = 200;
$ItemMax[marmor, RocketAmmo] = 10;
$ItemMax[mfemale, RocketAmmo] = 10;

$AutoUse[RocketLauncher] = False;

$WeaponAmmo[RocketLauncher] = "RocketAmmo";

AddWeapon(RocketLauncher);

$WeaponSpecial[RocketLauncher] = "true"; //== Wether this weapon has multiple settings or not...

function RocketLauncher::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	if($Settings::RocketLauncher[%clientId] == "0" || $Settings::RocketLauncher[%clientId] == "")
		%mode = "Normal/Mini-Locking";
	else if($Settings::RocketLauncher[%clientId] == "1")
		%mode = "Heat Seeking";
	else if($Settings::RocketLauncher[%clientId] == "2")
		%mode = "LockJaw";

	bottomprint(%clientId, "<jc><f2>Using " @ %item.description @ " - <f0>" @ %mode, 2);
}