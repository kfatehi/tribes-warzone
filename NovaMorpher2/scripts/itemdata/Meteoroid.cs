//--------------------------------------
$MeteoroidDamageType   = DamageTypes::AddReserve();

GrenadeData MeteoroidV2
{
   bulletShapeName    = "shockwave_large.dts";
   explosionTag       = LargeShockwave;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 0.001;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 10.0;
   damageType         = $MeteoroidDamageType;

   explosionRadius    = 25.0;
   kickBackStrength   = 3000.0;
   maxLevelFlightDist = 10000;
   totalTime          = 300.0;
   liveTime           = 0.001;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0;
   smokeName              = "shockwave_large.dts";
};

MineData Meteoroid
{
	mass = 10.0;
	drag = 1.0;
	density = 1.0;
	elasticity = 0;
	friction = 10000;
	className = "Handgrenade";
	description = "Meteoroid";
	shapeFile = "shockwave_large";
	shadowDetailMask = 4;
	explosionId = mortarExp;
	explosionRadius = 25.0;
	damageValue = 10.0;
	damageType = $MeteoroidDamageType;
	kickBackStrength = 30000;
	triggerRadius = 5;
	maxDamage = 0.001;
};

function Meteoroid::onAdd(%this)
{
	schedule("AirStrikeShell::CheckRest("@%this@");",1.0);
}

function Meteoroid::onStart()
{
	if($Meteoroid::Stop)
	{
		$Meteoroid::Stop = false;
		return;
	}
		
	%numClients = getNumClients();
	%numCl = ((2049 + %numClients) + 20);
	%linenum = 10;

	for(%k = 0 ; %k < %numClients; %k++) 
		%clientList[%k] = getClientByIndex(%k);

	for(%k= 0 ; %k < %numClients; %k++)
	{
		%player = Client::getOwnedObject(%clientList[%k]);

		%checkChance = floor(getRandom() * 10);
		if(%checkChance <= 5)
		{
			%msg = " but, was destroyed or bounced off in the atmosphere...";
		}
		else if(%player != "-1")
		{
			%type = getObjectType(%player);
			if(Player::isDead(%player) || %type != "Player")
			{
				%msg = ", if you were still alive that is....";
			}
			else
			{
				%pos = GameBase::getPosition(%player);
				%Xpos = getword(%pos, 0) + ((getRandom() * 200) - (getRandom() * 200)); //== Wanna be as random
				%Ypos = getword(%pos, 1) + ((getRandom() * 200) - (getRandom() * 200)); //== as possible =)
				%Zpos = getword(%pos, 2);
				%NZpos = floor(getRandom() * 1000) + 1000; //== Make it AT LEAST 1000m above that dude =)
				%NZpos += %Zpos;

				%pos = %Xpos @ " " @ %Ypos @ " " @ %NZpos;

				%obj = newObject("","Mine","Meteoroid");
				GameBase::setPosition(%obj, %pos);

				%msg = "! They are " @ %NZpos @ "m high above you!!";

				if($debug)
					KeepHeightTrack(%clientList[%k],%obj);
			}
		}
		else
			%msg = ", if you had even existed...";
		bottomprint(%clientList[%k],"<jc>A <f3>Meteoroid<f1> falling close to your position" @ %msg,10);
	}

	%waitTime = floor(getRandom() * 60 + $NovaMorpher::MeteoroidAddTime);
	schedule("Meteoroid::onStart();",%waitTime);
	if($debug)
		echo("Meteoriod Dropping again in: " @ %waitTime @ " seconds");
}

function KeepHeightTrack(%clientId, %this)
{
	%pos = GameBase::getPosition(%this);
	if((%Zpos = getword(%pos, 2)) == 0)
		return;
	bottomprint(%clientId,"<jc><f3>Meteoroid<f1> Status: HEIGHT --> " @ %Zpos,1);
	schedule("KeepHeightTrack("@%clientId@","@%this@");",0.1);
}