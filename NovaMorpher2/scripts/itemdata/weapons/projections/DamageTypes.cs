//== Put the new damage types here :-)

$MorphControlType = DamageTypes::AddReserve(); 
$SniperDamageType = DamageTypes::AddReserve();
$PistolDamageType = DamageTypes::AddReserve();
$HealDamageType   = DamageTypes::AddReserve();
$UTFDamageType   = DamageTypes::AddReserve();
$KickDamageType   = DamageTypes::AddReserve();
$FireDamageType   = DamageTypes::AddReserve();
$NoBurnFireDamageType   = DamageTypes::AddReserve();
$SpikeDamageType   = DamageTypes::AddReserve();
$NullDamageType   = DamageTypes::AddReserve();

//== And... New functions that involve damage types
function Heal(%clientId, %player)
{
	if($debug)
		echo("Heal DMG Type function called!");

	Client::sendMessage(%clientId,1,"You are being healed!");
	
	if(!$healTime[%player])
	{
		$healTime[%player] = 1000;
		checkHeal(%clientId, %player);
	}
	else
		$healTime[%player] = 1000;
}

function checkHeal(%clientId, %player)
{
	if($healTime[%player] > 0)
	{
		$healTime[%player] -= 1;  
		if(GameBase::getDamageLevel(%player) > 0)
		{
			%hrate = GameBase::getDamageLevel(%player) - 0.01;
			GameBase::setDamageLevel(%player, %hrate);  
			schedule("checkHeal(" @ %clientId @ ", " @ %player @ ");",0.1);
		}
		else
			$healTime[%player] = 0;
     	}
	else
	{
		Client::sendMessage(%clientId,1,"Heal potion has depleted...");
	}			
}

function round(%number, %nearest, %point)
{
	if(%point == "")
		%point = 0.5;
	%round = %point*%nearest;
	if(%round<=%number)
		%returnVal = floor(%number);
	else
		%returnVal = floor(%number)+%nearest;
	return %returnVal;
}

function FireDamageType::ChanceBurn(%player, %shooterClient)
{
	%clientId = Player::getClient(%player);
	%chance = round((getRandom() * 1)+0.25, 1, 0.75);
	%armor = Player::getArmor(%clientId);
	%dlevel = GameBase::getDamageLevel(%player);
	if(%clientId > 2048)
		%maxDamage = %armor.maxDamage;
	else
		%maxDamage = GameBase::getDataName(%player).maxDamage;

	%minBurn = (%maxDamage/100)*(floor(getRandom() * 49)+51); //== At least 50 to 70 % health to burn. Randomized.

	if(%chance && %dlevel >= %minBurn)
		FireDamageType::Burn(%player, %clientId, %shooterClient);
}

function FireDamageType::Burn(%player, %clientId, %shooterClient)
{
	%burnAdd = 10+floor(getRandom() * 50);
	if(%player.Burntime)
		%player.Burntime += %burnAdd;
	else
	{
		Client::sendMessage(%clientId,1,"Your on fire...");
		%player.Burntime = %burnAdd;
		FireDamageType::CheckBurn(%player, %clientId, %shooterClient);
	}
}

function FireDamageType::CheckBurn(%player, %clientId, %shooterClient)
{
	%armor = Player::getArmor(%clientId);
	%time = %player.Burntime;
	%checkA = %clientId > 2048 && !Player::isDead(%player) && $notBurnable[%armor] != "true";
	%checkB = %clientId > 2048;
	%checkC = GameBase::getDamageState(%player) == "Enabled" && !%checkB;
	%checkD = %checkA || %checkC;
	if(%time && %checkD)
	{

		%dlevel = GameBase::getDamageLevel(%player);

		%playerPos = GameBase::getPosition(%player);
		%zPos = floor(getRandom() * 6);
		%pos = getword(%playerPos, 0) @" "@ getword(%playerPos, 1)@" "@ %zPos;

		%pos = floor(getRandom() * 3.14) @" "@ floor(getRandom() * 3.14)@" "@ floor(getRandom() * 3.14);

		%data = GameBase::getDataName(%player);
		%maxDMG = %data.maxDamage;
		%cTime = %maxDMG - %dlevel;
		if(%cTime > 0)
			%cTime = 0.1;

		if(%clientId > 2048)
			Player::onDamage(%player,$NoBurnFireDamageType,0.005,%pos,%vec,"" , "torso", "back_left",%shooterClient);
		else
			StaticShape::onDamage(%player,$NoBurnFireDamageType,0.005,%pos,%vec,"" ,%shooterClient);

		if(%dlevel < %maxDMG)
			schedule("FireDamageType::CheckBurn("@%player@","@%clientId@","@%shooterClient@");",%cTime);

		DropFlame(%player);
	}
	else
		Client::sendMessage(%clientId,1,"Your armor stoped burning...");
}

function SelfHeal(%clientId, %player)
{
	$isSelfHealing[%clientId] = true;
	checkSelfHeal(%clientId, %player);
}

function checkSelfHeal(%clientId, %player)
{
	%damage = GameBase::getDamageLevel(%player);
	if  (!Player::isDead(%player) || %damage != "0") 

	{
		%hrate = %damage - 0.01; //== Go up to full health in 50 seconds
		GameBase::setDamageLevel(%player, %hrate);  
		schedule("checkHeal(" @ %clientId @ ", " @ %player @ ");",1,%player); //== Heal gradually
	}
	else
	{
		$isSelfHealing[%clientId] = false;
	}
}



//== Special function to morph clients....
function MorphControl::DoMorph(%player, %target)
{
      %client = Player::getClient(%player);
	%clientId = Player::getClient(%player);
	%targetId = Player::getClient(%target);
	%armor = Player::getArmor(%targetId);

	if (%target == %player){}
      if (%target = %target)
	{
		if(%armor != "nmlightarmor" && %armor != "nmarmor" && %armor != "nmfemale" && %armor != "nmheavyarmor")
		{
			Client::sendMessage(%client, 1, "Unable to modify molecular structure of current target!");
		}
		else
		{
			if(($Morph::CannotMorph[%targetId]) || (Client::getControlObject(%targetId) != Client::getOwnedObject(%targetId)))
				return; //== Taking the short way out :-P Besides, turning a person into a blastwall is nice.... :-)

			if($debug)
				echo("MorphControlBolt::onAcquire(); found target!");

			if($Morph::CannotMorph[%target]){} //== Just call me lazy ;)
			else
			{
				if($Settings::MorphControl[%clientId] == "")
				{
					bottomprint(%targetId,"You must select what to morph into vail tab menu first!",5);
				}
				else if($Settings::MorphControl[%clientId] == "0")
				{
					Player::setArmor(%targetId,nmlightarmor);
				}
				else if($Settings::MorphControl[%clientId] == "1")
				{
					Player::setArmor(%targetId,nmarmor);
				}
				else if($Settings::MorphControl[%clientId] == "2")
				{
					Player::setArmor(%targetId,nmheavyarmor);
				}
				else if($Settings::MorphControl[%clientId] == "3")
				{
					AtomicMorpher::TransformIntoMorpherBlastWall(%target);
				}
				else if($Settings::MorphControl[%clientId] == "4")
				{
					AtomicMorpher::TransformIntoVehicle(%target);
				}
				else if($Settings::MorphControl[%clientId] == "5")
				{
					AtomicMorpher::TransformIntoPlasmaTurret(%target);
				}
				MorphControl::DisableMorph(%target);
			}
		}
		return;
	}
}

function MorphControl::DisableMorph(%player)
{
	if($Morph::CannotMorph[%player])
	{
		bottomprint(%clientId,"Your MORPHING abilities have been DISABLED for 10 seconds!", 4.5);
		$Morph::CannotMorph[%player] = true;
		schedule("MorphControl::DisableMorph(" @ %player @ ");",10);
	}
	else
	{
		bottomprint(%clientId,"Your MORPHING abilities has recovered....", 5);
		$Morph::CannotMorph[%player] = "";
		return;
	}
}

//========================//
function DropFlame(%this)
{
	%vel = Item::getVelocity(%this);
	%pos = Vector::Add(getBoxCenter(%this), "0 0 1");
	%trans = "0 0 -1 0 0 0 0 0 -1 " @ %pos;
	Projectile::spawnProjectile(burnSmoke, %trans, %this, %vel); //transform, object, velocity vector, <projectile target (seeker)>
}



ExplosionData burnEx
{
   shapeName = "fiery.dts";
   faceCamera = true;
   randomSpin = true;
   hasLight   = true;

   lightRange = 10;
   timeZero = 0.100;
   timeOne  = 0.900;

   colors[0]  = { 0.0, 0.0, 0.0 };
   colors[1]  = { 1.0, 1.0, 1.0 };
   colors[2]  = { 1.0, 1.0, 1.0 };
   radFactors = { 0.0, 1.0, 0.0 };

   shiftPosition = True;
};

GrenadeData burnSmoke
{
   bulletShapeName    = "plasmaex.dts";
   explosionTag       = burnEx;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 1.3;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0;
   damageType         = $NullDamageType;

   explosionRadius    = 5;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 0;
   totalTime          = 0.01;    // special meaning for grenades...
   liveTime           = 0.01;
   projSpecialTime    = 2.5;

   inheritedVelocityScale = 0.5;
   smokeName              = "plasmaex.dts";
};

function EMP(%player, %clientId, %shooterClient, %msg, %time)
{
	if(!%time)
		%time = 400;
	if($EMPTime[%player] > %time/2) //== Avoid spam LOL
	{
		$EMPTime[%player] = %time;
		return;
	}

	if(%msg == "")
		%msg = "YOU WERE HIT BY EMP!";

	Client::sendMessage(%clientId,1,%msg);

	if($EMPTime[%player] == 0 || $EMPTime[%player] == "")
	{
		$EMPTime[%player] = %time;
		checkEMP(%clientId, %player, %shooterClient);
	}
	else
		$EMPTime[%player] = ($EMPTime[%player]+%time)/2;
}

function checkEMP(%clientId, %player, %shooterClient)
{
	if($EMPTime[%player] > 0)
	{
		$EMPTime[%player] -= 1;  
		if  (!Player::isDead(%player)) 
		{
			%energy = GameBase::getEnergy(%player);
			if(%energy > 2)
				GameBase::setEnergy(%player, %energy - 1);
			else
			{
				if(%clientId > 2048)
					Player::onDamage(%player,$ElectricityDamageType,0.0025,"","","" ,"torso" ,"front_right" ,%shooterClient);
				else
					StaticShape::onDamage(%player,$ElectricityDamageType,0.0025,"","","" , %shooterClient);

				GameBase::setEnergy(%player, %energy - 1);
			}
		}
		else
		{
			$EMPTime[%player] = 0;
		}
		schedule("checkEMP(" @ %clientId @ ", " @ %player @ ", " @ %shooterClient @ ");",0.03,%player);
     	}
	else
	{
		Client::sendMessage(%clientId,1,"EMP effect has faded away...");
	}			
}


