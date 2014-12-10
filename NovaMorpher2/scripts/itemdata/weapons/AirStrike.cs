//== This should be more devistating then nukes!
//== Lets see... Wat should we bombard these people with... Ahh.... Mortar like things :-)
$TeamItemMax[AirStrike] = 5;

$InvList[AirStrike] = 1;
$RemoteInvList[AirStrike] = 0;

$ItemMax[harmor, AirStrike] = 1;

$AutoUse[AirStrike] = False;

$WeaponAmmo[AirStrike] = "";

AddWeapon(AirStrike);

$WeaponSpecial[AirStrike] = "true"; //== Wether this weapon has multiple settings or not...

function AirStrike::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	if($Settings::AirStrike[%clientId] == "0" || $Settings::AirStrike[%clientId] == "")
		%mode = "Random Dropping";
	else if($Settings::AirStrike[%clientId] == "1")
		%mode = "Visual Bombing";

	bottomprint(%clientId, "<jc><f2>Using " @ %item.description @ " - <f0>" @ %mode, 2);
}

//--------------------------------------
GrenadeData AirStrikeMortarShell
{
   bulletShapeName    = "liqcyl.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 12.0;
   damageType         = $MortarDamageType;

   explosionRadius    = 50.0;
   kickBackStrength   = 300.0;
   maxLevelFlightDist = 1;
   liveTime           = 0.01;
   totalTime          = 120.0;
   liveTime           = 0.001;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0;
   smokeName              = "shotgunex.dts";
};

GrenadeData MiniAirStrikeMortarShell
{
   bulletShapeName    = "liqcyl.dts";
   explosionTag       = LargeShockwave;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 12.0;
   damageType         = $MortarDamageType;

   explosionRadius    = 50.0;
   kickBackStrength   = 300.0;
   maxLevelFlightDist = 1;
   liveTime           = 0.01;
   totalTime          = 120.0;
   liveTime           = 0.001;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0;
   smokeName              = "shotgunex.dts";
};

MineData AirStrikeShell
{
	mass = 1.0;
	drag = 1.0;
	density = 1.0;
	elasticity = 0;
	friction = 1.0;
	className = "Handgrenade";
	description = "AirStrike Bomb";
	shapeFile = "liqcyl";
	shadowDetailMask = 4;
	explosionId = mortarExp;
	explosionRadius = 25.0;
	damageValue = 4.0;
	damageType = $MortarDamageType;
	kickBackStrength = 300;
	triggerRadius = 0.5;
	maxDamage = 9999.0;
};

function AirStrikeShell::onAdd(%this)
{
	AirStrikeShell::CheckRest(%this);
}

function AirStrikeShell::CheckRest(%this)
{
	if($debug)
		echo("Checking is Rest " @ %this);
	if(GameBase::isAtRest(%this))
	{
		AirStrikeShell::BOOM(%this);
	}
	else
		schedule("AirStrikeShell::CheckRest("@%this@");",0.5,%this);

	AirStrikeShell::scheduleDelete(%this);
}

function AirStrikeShell::scheduleDelete(%this)
{
	if($AirStrikeShell::scheduleDelete[%this])
		return;
	else
	{
		schedule("AirStrikeShell::BOOM("@%this@");", 60, %this); //== 1 minute to get down here!
		$AirStrikeShell::scheduleDelete[%this] = true;
	}
}

function AirStrikeShell::BOOM(%this)
{
	%data = GameBase::getDataName(%this);
	%class = %data.className;
	if(%class == "Handgrenade")
	{
		GameBase::setDamageLevel(%this, %data.maxDamage);
		$AirStrikeShell::scheduleDelete[%this] = false;
	}
}	

//----------------------------------------------------------------------------

ItemImageData AirStrikeImage
{
	shapeFile = "ammo1";
	mountPoint = 0;
	weaponType = 0; // Single Shot
	accuFire = true;
	reloadTime = 2.5;
	fireTime = 0;
	minEnergy = 0;
	maxEnergy = 0;


	lightType = 3;  // Weapon Fire
	lightRadius = 5;
	lightTime = 3;
	lightColor = { 1, 0, 0 };

	sfxFire = SoundDryFire;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData AirStrike
{
	heading = "bSpecial Weapons";
	description = "AirStrike";
	className = "Weapon";
	shapeFile  = "ammo1";
	hudIcon = "mortar";
	shadowDetailMask = 4;
	imageType = AirStrikeImage;
	price = 85;
	showWeaponBar = true;
};

function AirStrikeImage::onFire(%player, %slot)
{
	%count = 0;
	%client = Player::getClient(%player);
	%team = GameBase::getTeam(%player);
	if($radar[%team])
	{
		if(GameBase::getLOSInfo(%player,300))
		{
			%pos = $los::position;
			playsound(bigExplosion1,%pos);
			playsound(bigExplosion1,GameBase::getPosition(%player));
			%Xpos = getword(%pos, 0);
			%Ypos = getword(%pos, 1);
			%Zpos = getword(%pos, 2);
			%Zpos += floor(getRandom() * 1000);
			if(%Zpos < 50)
				%Zpos = 50;
	
			AirStrike::DropBombs(%player,%count, %Xpos, %Ypos, %Zpos, %pos);

			$TeamItemCount[%team @ "AirStrike"] = $TeamItemCount[GameBase::getTeam(%player) @ "AirStrike"] + 1;
		}
		else
			Client::SendMessage(%client,1,"Out of range for an AirStrike.");
	}
	else
		Client::SendMessage(%client,1,"Unable to lock! Please deploy a 'Multi-Radar'!");
}


//== AirStrike -- Based on chivalry's Brimstone: drop bomb:: So all credits to the CHIVALRY Dev. Group
function AirStrike::DropBombs(%player, %count, %Xpos, %Ypos, %Zpos, %pos)
{
	%client = Player::getClient(%player);
	%time = 0.5;

	%checkA = ($Settings::AirStrike[%client] == "3" && $SuperStrike < 1);
	%checkB = %checkA || $Settings::AirStrike[%client] != "3";

	if($TeamItemCount[GameBase::getTeam(%player) @ "AirStrike"] <= $TeamItemMax[AirStrike] && %checkB)
	{

		Player::setItemCount(%player, "AirStrike", 0);
	
		if(%count < 40)
		{
			if($debug)
				echo("BOMBS AWAY!!!!");
	
			%count++;

			if($Settings::AirStrike[%client] == "" || $Settings::AirStrike[%client] == "0") //== Standard Formation
			{
				%chance = floor(getRandom()*2)-1;
				if(%chance)
				{
					for(%cnt = 0;%cnt < 2;%cnt++)
					{
						%posneg = floor(getRandom() * 10);
						if($debug)
							echo("%posneg = " @ %posneg);
						if(%posneg  > 5.5555)
						{
							%coord = floor(getRandom() * (100 - 0.01));
							if(%cnt == 0)
								%newXpos = %Xpos + %coord + 10;
							else
								%newYpos = %Ypos + %coord0;
						}
						else
						{
							%coord = floor(getRandom() * (100 - 0.01));
							if(%cnt == 0)
								%newXpos = %Xpos - %coord  + 10;
							else
								%newYpos = %Ypos - %coord;
						}
					}
	
					for(%counts = -16; %counts < 16; %counts = %counts + 8)
					{
						//== Make it 1 row of drops per hit...
	
						%newXpos = %newXpos + %counts;
	
						%obj = newObject("","Mine","AirStrikeShell");
						addToSet("MissionCleanup", %obj);
						GameBase::throw(%obj,%player,-7.5,true);




						%newPos = %newXpos@" "@%newYpos@" "@%Zpos;
						GameBase::setPosition(%obj, %newPos);

					}
				}
				else
				{
					for(%counts = -4; %counts < 4; %counts = %counts + 2)
					{
						for(%cnt = 0;%cnt < 2;%cnt++)
						{
							%posneg = floor(getRandom() * 10);
							if($debug)
								echo("%posneg = " @ %posneg);
							if(%posneg > 5.5555)
							{
								%coord = floor(getRandom() * (100 - 0.01));
								if(%cnt == 0)
									%newXpos = %Xpos + %coord + 10;
								else
									%newYpos = %Ypos + %coord;
							}
							else
							{
								%coord = floor(getRandom() * (100 - 0.01));
								if(%cnt == 0)
									%newXpos = %Xpos - %coord + 10;
								else
	
									%newYpos = %Ypos - %coord;
							}
						}

						%obj = newObject("","Mine","AirStrikeShell");
						addToSet("MissionCleanup", %obj);
						GameBase::throw(%obj,%player,-7.5,true);

						%newPos = %newXpos@" "@%newYpos@" "@%Zpos;
						GameBase::setPosition(%obj, %newPos);
					}
				}
			}
			else if($Settings::AirStrike[%client] == "1") //== UNTESTED
			{
				if($SuperStrike > 1)
					Client::sendMessage(%client,1,"Located OTHER approching carrier, please try again later!!!!");
				else
				{
					AirStrike::Formation3(%player, %pos);
					$SuperStrike++;
				}
				break;
			}

			//== 2 second interval per strike....
			schedule("AirStrike::DropBombs("@%player@","@%count@","@%Xpos@","@%Ypos@","@%Zpos@");",%time);

		}
	}
	else
	{
		Client::sendMessage(%client,1,"Out of SeRvIcE! Try again in the NEXT Mission!");
	}
}

function CreateCircleVectors(%r)
{
	%y = 0;
	%CompString = (%r*%r);
	for(%x = (-1 * %r); (%CompString > (-1 * %r) && %r >= %x); %x=%x+8)
	{
		%y = sqrt(%r*%r-%x*%x);
		%OutPut = %OutPut @ " " @ %x @ " " @ %y;
		%CompString = (%r*%r-(%x*%x + %y*%y));
	}

	return %OutPut;
}

function AirStrike::Formation3(%player, %pos)
{
	%client = Player::getClient(%player); 

	%goPos = %pos;
	%obj = newObject("BasicDropShip1","InteriorShape", "cotpdrop.dis"); 
	%pos = Vector::add(%pos, MoveForward::rotVector( "0 500 0", GameBase::getRotation(%player)));
	%pos = Vector::add(%pos, "0 0 500");

	addToSet("MissionCleanup", %obj); 
	GameBase::setTeam(%objShip1,GameBase::getTeam(%player)); 
	GameBase::setPosition(%obj,%pos); 
	GameBase::setRotation(%obj,GameBase::getRotation(%player)); 
	Gamebase::setMapName(%obj,"BomberShip"); 
	GameBase::startFadeIn(%obj); 

	//%goPos = GameBase::getPosition(%player);
	AirStrike::MoveShip(%player, %obj, Vector::add(%goPos,"0 0 200"), false);
	return true; 
} 


function Vector::AddAllVar(%vec)
{
	%x = getWord(%vec,0)*100;
	%y = getWord(%vec,1)*10;
	%z = getWord(%vec,2);

	// new vector with z axis removed
	%basevec = %x + %y + %z;
	
	return %basevec;
}

function AirStrike::MoveShip(%player, %ship, %pos, %go, %fall)
{
	%curPos = Gamebase::getPosition(%ship);
	if(!%go)
	{
		%dest = Vector::add(%curPos, MoveForward::rotVector( "0 -2 0", GameBase::getRotation(%ship)));

		%dest = Vector::add(%dest, "0 0 -2");
		GameBase::setPosition(%ship,%dest); 

		%allA = Vector::AddAllVar(%dest);
		%allB = Vector::AddAllVar(%pos);
		%check = %allA-%allB;
		if(%check > -100 && %check < 100)
		{
			%go = true;
			schedule("deleteObject("@%ship@");",20,%ship);
			schedule("$SuperStrike--;",20);
		}
	}
	else
	{
		%rot = 0.01*getRandom();
		%newRot = Vector::add(GameBase::getRotation(%ship), "0 0 "@%rot);
		GameBase::setRotation(%ship,%newRot); 
		%dest = Vector::add(%curPos, MoveForward::rotVector( "0 -4 0", %newRot));
		%dest = Vector::add(%dest, "0 0 2");
		GameBase::setPosition(%ship,%dest); 

		%allA = Vector::AddAllVar(%dest);
		%allB = Vector::AddAllVar(%pos);
		%check = %allA-%allB;
		if(%check > 400 && %check < 600)
			%go = true;

	}

	if(%fall == 1)
	{
		%objRot = GameBase::getRotation(%ship);
		%objPos = Vector::add(%dest , "20 -10 -10");
		%dir = Vector::getfromrot(%objRot);
		%trans = %objRot @ " " @ %dir @ " " @ %dir @ " " @ %objPos;
		Projectile::spawnProjectile("AirStrikeMortarShell", %trans, %player, %objRot, %player);

		%fall++;
	}
	else if(%fall == 4)
	{
		%objRot = GameBase::getRotation(%ship);
		%objPos = Vector::add(%dest , "-20 10 -10");
		%dir = Vector::getfromrot(%objRot);
		%trans = %objRot @ " " @ %dir @ " " @ %dir @ " " @ %objPos;
		Projectile::spawnProjectile("AirStrikeMortarShell", %trans, %player, %objRot, %player);

		%fall = 0;
	}
	else
		%fall++;

	schedule("AirStrike::MoveShip("@%player@",'"@%ship@"','"@%pos@"','"@%go@"','"@%fall@"');",0.1,%ship);
}


//== Function found in MD, unsure of who wrote it
function MoveForward::rotVector(%vec,%rot)
{
	// this function rotates a vector about the z axis

	%vec_x = getWord(%vec,0);
	%vec_y = getWord(%vec,1);
	%vec_z = getWord(%vec,2);

	// new vector with z axis removed
	%basevec = %vec_x @ "  " @ %vec_y @ "  0";
	
	// change vector to distance and rotation
	%basedis = Vector::getDistance( "0 0 0", %basevec);
	%normvec = Vector::normalize( %basevec );
	%baserot = Vector::add( Vector::getRotation( %normvec ), "1.571 0 0" );

	// modify rotation and change back to vector (put z axis offset back)
	%newrot = Vector::add( %baserot, %rot );
	%newvec = Vector::getFromRot( %newrot, %basedis, %vec_z );

	return %newvec;




}

