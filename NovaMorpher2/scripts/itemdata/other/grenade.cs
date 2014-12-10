$SpecialArmor::Grenade[0] = "larmor";  //== Male armors are differ from females :-)
$SpecialArmor::GrenadeInfo[0] = "cluster";

$SpecialArmor::Grenade[1] = "lfemale"; //== Male armors are differ from females :-)
$SpecialArmor::GrenadeInfo[1] = "cluster";

$SpecialArmor::Grenade[2] = "harmor"; //== Male armors are differ from females :-)
$SpecialArmor::GrenadeInfo[2] = "rocket";

$SpecialArmor::Grenade[3] = "amarmor"; //== Male armors are differ from females :-)
$SpecialArmor::GrenadeInfo[3] = "mortar";

$SpecialArmor::Grenade[4] = "marmor"; //== Male armors are differ from females :-)
$SpecialArmor::GrenadeInfo[4] = "needle";

$SpecialArmor::Grenade[5] = "mfemale"; //== Male armors are differ from females :-)
$SpecialArmor::GrenadeInfo[5] = "needle";

MineData Handgrenade
{
   mass = 0.3;
   drag = 1.0;
   density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
   description = "Handgrenade";
   shapeFile = "grenade";
   shadowDetailMask = 4;
   explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = 0.5;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

MineData Mortargrenade
{
   mass = 0.3;
   drag = 1.0;
   density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
   description = "Handgrenade";
   shapeFile = "mortar";
   shadowDetailMask = 4;
   explosionId = mortarExp;
	explosionRadius = 20.0;
	damageValue = 1.0;
	damageType = $MortarDamageType;
	kickBackStrength = 250;
	triggerRadius = 0.5;
	maxDamage = 0.01; //== Sensitivity guys!
};


function Mortargrenade::onAdd(%this)
{
	%data = GameBase::getDataName(%this);
	schedule("Mine::Detonate(" @ %this @ ");",10.0,%this);
}
ItemData Grenade
{
   description = "Grenade";
   shapeFile = "grenade";
   heading = "yMiscellany";
   shadowDetailMask = 4;
   price = 5;
	className = "HandAmmo";
   validateShape = false;
   validateMaterials = true;
};

function Grenade::onUse(%player,%item)
{
	%client = Player::getClient(%player);
	%armor = Player::getArmor(%client);
	for(%i = 0; $SpecialArmor::Grenade[%i] != ""; %i++)
	{
		if($SpecialArmor::Grenade[%i] == %armor)
		{
			%specialty = $SpecialArmor::GrenadeInfo[%i];
			%armorIsSpecial = True;
			break;
		}
	}
	if($matchStarted)
	{
		if(%player.throwTime < getSimTime() )
		{
			if(($Settings::Grenade[%client] == "" || $Settings::Grenade[%client] == "0") && !%armorIsSpecial) //== filter out the other kinds of armors
			{
				Grenade::useDefault(%player);
				Player::decItemCount(%player,%item);
			}
			else if(%specialty == "cluster")
			{
				ClusterGrenade::onUse(%player,%item);
				Player::decItemCount(%player,%item);
			}
			else if(%specialty == "rocket")
			{
				if(RocketMine::Fire(%client, %player, %item))
					Player::decItemCount(%player,%item);
			}
			else if(%specialty == "mortar")
			{
				Grenade::useMortar(%player);
				Player::decItemCount(%player,%item);
			}
			else if(%specialty == "needle")
			{
				Grenade::useNeedle(%player);
				Player::decItemCount(%player,%item);
			}
		}
	}
}

function Grenade::useDefault(%player)
{
	%client = Player::getClient(%player);
	%obj = newObject("","Mine","Handgrenade");
 	addToSet("MissionCleanup", %obj);
	%client = Player::getClient(%player);
	GameBase::throw(%obj,%player,9 * %client.throwStrength,false);
	%player.throwTime = getSimTime() + 0.5;
}

function Grenade::useMortar(%player)
{
	%client = Player::getClient(%player);
	%obj = newObject("","Mine","Mortargrenade");
 	addToSet("MissionCleanup", %obj);
	%client = Player::getClient(%player);
	GameBase::throw(%obj,%player,9 * %client.throwStrength,false);
	%player.throwTime = getSimTime() + 0.5;
}

function Handgrenade::onAdd(%this)
{
	%data = GameBase::getDataName(%this);
	schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}

function Mine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
   if (%type == $MineDamageType)
      %value = %value * 0.25;

	%damageLevel = GameBase::getDamageLevel(%this);
	GameBase::setDamageLevel(%this,%damageLevel + %value);
}

function Mine::Detonate(%this)
{
	%data = GameBase::getDataName(%this);
	GameBase::setDamageLevel(%this, %data.maxDamage);
}

function RocketMine::Fire(%client, %player, %item)
{
	if(Player::getMountedItem(%player, $ArmorSpecialSlotA) == CannonP1Pack)
	{
		%state = Player::getItemState(%player,$ArmorSpecialSlotA);
		if (%state != "Fire" && %state != "Reload")
		{
			Player::trigger(%player,$ArmorSpecialSlotA,true);
			Player::trigger(%player,$ArmorSpecialSlotB,true);
			Player::trigger(%player,$ArmorSpecialSlotA,false);
			Player::trigger(%player,$ArmorSpecialSlotB,false);

			%dmg = (0.5+GameBase::getDamageLevel(%player)/2);
			GameBase::applyDamage(%player,-5, %dmg,GameBase::getPosition(%player),"0 0 0","0 0 0",%player);
			Player::decItemCount(%player,%item); 

			ixApplyKickback(%player, 320*%dmg, 65*%dmg);
			schedule("playSound(SoundFlyerDismount, GameBase::getPosition("@%client@"));", 1.5, %player);
		}
		else
		{
			Client::sendMessage(%client,0, "Unable to fire - Missiles are reloading"); 	
			return false;			
		}				
	}
	else
	{
		Grenade::useDefault(%player);
	}
	return true;
}


function Grenade::useNeedle(%player)
{
	%client = Player::getClient(%player);
	%obj = newObject("","Mine","needleGrenade");
	%obj.thrower = %client;
 	addToSet("MissionCleanup", %obj);
	%client = Player::getClient(%player);
	GameBase::throw(%obj,%player,9 * %client.throwStrength,false);
	%player.throwTime = getSimTime() + 0.5;
}






RocketData grenadeBullet
{
   bulletShapeName = "bullet.dts";
   explosionTag    = bulletExp0;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.35;
   damageType       = $BulletDamageType;

   explosionRadius  = 3.0;
   kickBackStrength = 0.0;

   muzzleVelocity   = 100.0;
   terminalVelocity = 1000.0;
   acceleration     = 1.0;

   totalTime        = 8.5;
   liveTime         = 18.0;

   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };

   inheritedVelocityScale = 0.5;
   trailType   = 1;
   trailLength = 15;
   trailWidth  = 0.15;
   soundId = SoundJetHeavy;
};

MineData needleGrenade
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "discammo";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = 0;
	damageType = $NullDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 2;
};

StaticShapeData needleThrower
{
	className = "Beacon";
	damageSkinData = "objectDamageSkins";

	shapeFile = "discammo";
	maxDamage = 1;
	maxEnergy = 200;

	castLOS = true;
	supression = false;
	mapFilter = 2;
	visibleToSensor = true;
	explosionId = flashExpSmall;
	debrisId = flashDebrisSmall;
};

$DamageScale[needleThrower, $BulletDamageType] = 0;

function needleGrenade::onAdd(%this)
{
	%data = GameBase::getDataName(%this);
	schedule("needleGrenade::Detonate(" @ %this @ ");",1.0,%this);
}

function needleGrenade::Detonate(%this)
{
	%clientId = %this.thrower;
	%this.thrower = "";
	%thisPos = gamebase::getposition(%this);
	spawnNeedleThrower(%clientId, %thisPos);
	Mine::Detonate(%this);
}

function spawnNeedleThrower(%clientId, %pos)
{
	%player = Client::getOwnedObject(%clientId);
	%beacon = newObject("Needle Thrower", "StaticShape", "needleThrower", true);
	addToSet("MissionCleanup", %beacon);
	GameBase::setRotation(%beacon,%rot);
	GameBase::setPosition(%beacon,%pos);
	Gamebase::setMapName(%beacon,"Needle Thrower");
	%time = getBalancedNum(120);
	if(%time < 5)
		%time = 5;
   	spawnNeedleThrower::throwThings(%clientId, %beacon, %time);
}

function spawnNeedleThrower::throwThings(%clientId, %this, %count)
{
	%player = Client::getOwnedObject(%clientId);
	if(%count > 0)
	{
		%pos = gamebase::getposition(%this);
		%rot = getRandom()*200@" "@getRandom()*200@" "@getRandom()*200;
		%dir = Vector::getfromrot(%rot);
		%pos1 = Vector::add(%pos, "0 0 0.01");	
		%trans1 = %rot @ " " @ %dir @ " " @ %dir @ " " @ %pos1;
		Projectile::spawnProjectile("grenadeBullet", %trans1, %player, %rot, %player);

		%rot = getRandom()*-200@" "@getRandom()*-200@" "@getRandom()*-200;
		%dir = Vector::getfromrot(%rot);
		%pos1 = Vector::add(%pos, "0 0 -0.01");	
		%trans1 = %rot @ " " @ %dir @ " " @ %dir @ " " @ %pos1;
		Projectile::spawnProjectile("grenadeBullet", %trans1, %player, %rot, %player);

		schedule("spawnNeedleThrower::throwThings("@%clientId@","@%this@","@%count-2@");",0.3,%this);
	}
	else
		schedule("Mine::Detonate(" @ %this @ ");",1.0,%this);
}

