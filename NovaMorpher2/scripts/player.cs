$PlayerAnim::Crouching = 25;
$PlayerAnim::DieChest = 26;
$PlayerAnim::DieHead = 27;
$PlayerAnim::DieGrabBack = 28;
$PlayerAnim::DieRightSide = 29;
$PlayerAnim::DieLeftSide = 30;
$PlayerAnim::DieLegLeft = 31;
$PlayerAnim::DieLegRight = 32;
$PlayerAnim::DieBlownBack = 33;
$PlayerAnim::DieSpin = 34;
$PlayerAnim::DieForward = 35;
$PlayerAnim::DieForwardKneel = 36;
$PlayerAnim::DieBack = 37;

//----------------------------------------------------------------------------
$CorpseTimeoutValue = 22;
//----------------------------------------------------------------------------

// Player & Armor data block callbacks

function Player::onAdd(%this)
{
	GameBase::setRechargeRate(%this,8);
	if($NovaMorpher::JetSmoke)
		Player::jetpackLoop(%this);

	%client = Player::getClient(%this);
	%armor = Player::getArmor(%client);
}

function SRArmor::doDeathCount(%clientId)
{
	%player = Client::getOwnedObject(%clientId);
	Client::sendMessage(%clientId,1,"Please do what you need to do in 1 minute and change armor, or you will be executed! So hurry up!");
	schedule("SRArmor::doDeath("@%clientId@","@%player@");",60,%player);
}

function SRArmor::doDeath(%client,%player)
{
	if(Player::isDead(%player))
		return;

	%armor = Player::getArmor(%player);
	if(%armor == "srarmor")
		remoteKill(%client);
}

//== Compete credit to WarCan and his mod Meltdown )]-[(olocoust ;) And where ever he may have got that from...
function velToSingle(%vel, %bool) // Converts 3-vector velocity to a single number
{
	%x = getWord(%vel, 0);
	%y = getWord(%vel, 1);
	%z = getWord(%vel, 2);

	if(%bool)
		return floor(sqrt((%x*%x)+(%y*%y)+(%z*%z)));
	else
		return sqrt((%x*%x)+(%y*%y)+(%z*%z));
}

function Player::jetpackLoop(%this)
{
	if(Player::isDead(%this))
		return;

	%client = Player::getClient(%this);

	if((%checkRate = $NovaMorpher::JetSmokeRate) == 0)
		%checkRate = 1;

	%armor = Player::getArmor(%client);

	%heat = GameBase::virtual(%this, "getHeatFactor");

	%plyHimSelf = (Client::getControlObject(%client) == Client::getOwnedObject(%client));
	%plyJet = Player::isJetting(%this);
	%plyOvHt = (%heat >= 0.5);
	%plyHtSync = %this.heatSync;

	%armor = Player::getArmor(%this);
	if(((%plyJet || %plyOvHt) && !%plyHtSync && %plyHimSelf) && $Armor::SpecialSmoke[%armor] != "NoSmoke")
	{
		$JPrate = 10;
		%vel = Item::getVelocity(%this);
		%JetVel = velToSingle(%vel);
		%trans = "0 0 -1 0 0 0 0 0 -1 " @ getBoxCenter(%this);

#		%pos1 = vector::Add(getBoxCenter(%this), "-0.5 -0.01 -1.25");
#		%pos2 = vector::Add(getBoxCenter(%this), "0.5 -0.01 -1.25");

		%rot = GameBase::getRotation(%this);
		%pos = vector::Add(getBoxCenter(%this), "-0.5 -0.01 -1.25");
		%pos1 = Vector::add(%pos, Vector::rotVector("-0.5 -0.01 -1.25",%rot));
		%pos2 = Vector::add(%pos, Vector::rotVector("0.5 -0.01 -1.25",%rot));

		%trans2 = "0 0 -1 0 0 0 0 0 -1 " @ %pos1;
		%trans3 = "0 0 -1 0 0 0 0 0 -1 " @ %pos2;

		%armorShape = %armor.shapeFile;
		%isntHeavy = (%armorShape == "larmor" || %armorShape == "lfemale" || %armorShape == "marmor" || %armorShape == "mfemale");
#		%isntHeavy = (%armorShape =! "harmor");

		if($Armor::SpecialSmoke[%armor])
		{
			%smoke = $Armor::SmokeType[%armor];
			if(%isntHeavy || !$Armor::SmokeUseHeavy)
			{
				Projectile::spawnProjectile(%smoke, %trans, %this, "0 0 -5"); //transform, object, velocity vector, <projectile target (seeker)>
			}
			else
			{
				Projectile::spawnProjectile(%smoke, %trans2, %this, "0 0 -2.5");
				Projectile::spawnProjectile(%smoke, %trans3, %this, "0 0 -2.5");
			}
		}
		else if(%isntHeavy)
		{
			Projectile::spawnProjectile(SFXSmokeLight, %trans, %this, "0 0 -2.5"); //transform, object, velocity vector, <projectile target (seeker)>
		}
		else
		{
			if(%this.hasBooster)
				%proj = smokeBooster;
			else
				%proj = SFXSmoke;

			Projectile::spawnProjectile(%proj, %trans2, %this, "0 0 -5");
			Projectile::spawnProjectile(%proj, %trans3, %this, "0 0 -5");
 		}
		%nextCheckT = (1/$JPrate)/%checkRate;
		schedule("Player::jetpackloop(" @ %this @ ");", %nextCheckT, %this);
	}
	else
	{
		schedule("Player::jetpackloop(" @ %this @ ");", 0.5/%checkRate, %this);
	}
}

//=====================//

ExplosionData smExp
{
 shapeName = "rsmoke.dts";
 faceCamera = true;
 randomSpin = true;
 hasLight = true;
// lightRange = 1.0;

 lightRange = 0;
 timeScale = 10;

 timeZero = 0.100;
 timeOne= 0.900;

 colors[0]= { 0.0, 0.0, 0.0 };
 colors[1]= { 1.0, 1.0, 1.0 };
 colors[2]= { 1.0, 1.0, 1.0 };
 radFactors = { 0.0, 1.0, 0.0 };

 shiftPosition = True;
};

GrenadeData SFXSmoke
{
 bulletShapeName= "breath.dts";
 explosionTag = smExp;
 collideWithOwner = True;
 ownerGraceMS = 250;
 collisionRadius= 1.3;
 mass = 5.0;
 elasticity = 0.1;

 damageClass= 1; // 0 impact, 1, radius
 damageValue= 0.3;
 damageType = $NullDamageType;

 explosionRadius= 0;
 kickBackStrength = 0.0;
 maxLevelFlightDist = 0;
 totalTime= 0.01;// special meaning for grenades...
 liveTime = 0.01;
// projSpecialTime= 0.01;
 projSpecialTime= 2.5;

 inheritedVelocityScale = 0.5;
 smokeName= "rsmoke.dts";
};

//========================//
ExplosionData smExpLight
{
 shapeName = "smoke.dts";
 faceCamera = true;
 randomSpin = true;
 hasLight = true;
// lightRange = 1.0;

 lightRange = 0;
 timeScale = 10;

 timeZero = 0.100;
 timeOne= 0.900;

 colors[0]= { 0.0, 0.0, 0.0 };
 colors[1]= { 1.0, 1.0, 1.0 };
 colors[2]= { 1.0, 1.0, 1.0 };
 radFactors = { 0.0, 1.0, 0.0 };

 shiftPosition = True;
};

GrenadeData SFXSmokeLight
{
 bulletShapeName= "breath.dts";
 explosionTag = smExpLight;
 collideWithOwner = True;
 ownerGraceMS = 250;
 collisionRadius= 1.3;
 mass = 5.0;
 elasticity = 0.1;

 damageClass= 1; // 0 impact, 1, radius
 damageValue= 0.3;
 damageType = $NullDamageType;

 explosionRadius= 0;
 kickBackStrength = 0.0;
 maxLevelFlightDist = 0;
 totalTime= 0.01;// special meaning for grenades...
 liveTime = 0.01;
// projSpecialTime= 0.01;
 projSpecialTime= 2.5;

 inheritedVelocityScale = 0.5;
 smokeName= "smoke.dts";
};

//========================//
ExplosionData smokeBoosterEx
{
 shapeName = "plasmaex.dts";
 faceCamera = true;
 randomSpin = true;
 hasLight = true;
// lightRange = 1.0;

 lightRange = 0;
 timeScale = 2;

 timeZero = 0.100;
 timeOne= 0.900;

 colors[0]= { 0.0, 0.0, 0.0 };
 colors[1]= { 1.0, 1.0, 1.0 };
 colors[2]= { 1.0, 1.0, 1.0 };
 radFactors = { 0.0, 1.0, 0.0 };

 shiftPosition = True;
};

GrenadeData smokeBooster
{
 bulletShapeName= "plasmaex.dts";
 explosionTag = smokeBoosterEx;
 collideWithOwner = True;
 ownerGraceMS = 250;
 collisionRadius= 1.3;
 mass = 5.0;
 elasticity = 0.1;

 damageClass= 1; // 0 impact, 1, radius
 damageValue= 0;
 damageType = $NullDamageType;

 explosionRadius= 5;
 kickBackStrength = 15.0;
 maxLevelFlightDist = 0;
 totalTime= 0.01;// special meaning for grenades...
 liveTime = 0.01;
 projSpecialTime= 2.5;

 inheritedVelocityScale = 0.5;
 smokeName= "plasmaex.dts";
};
//============================================================================================//

function Player::onRemove(%this)
{
	// Drop anything left at the players pos
	for (%i = 0; %i < 8; %i = %i + 1) {
		%type = Player::getMountedItem(%this,%i);
		if (%type != -1) {
			// Note: Player::dropItem ius not called here.
			%item = newObject("","Item",%type,1,false);
 schedule("Item::Pop(" @ %item @ ");", $ItemPopTime, %item);

 addToSet("MissionCleanup", %item);
			GameBase::setPosition(%item,GameBase::getPosition(%this));
		}
	}
}

function Player::onNoAmmo(%player,%imageSlot,%itemType)
{
	//echo("No ammo for weapon ",%itemType.description," slot(",%imageSlot,")");
}

function Player::onKilled(%this)
{
	%cl = GameBase::getOwnerClient(%this);
	%cl.dead = 1;
	if($AutoRespawn > 0)
		schedule("Game::autoRespawn(" @ %cl @ ");",$AutoRespawn,%cl);
	if(%this.outArea==1)	
		leaveMissionAreaDamage(%cl);
	Player::setDamageFlash(%this,0.75);
	for (%i = 0; %i < 8; %i = %i + 1) {
		%type = Player::getMountedItem(%this,%i);
		if (%type != -1) {
			if (%i != $WeaponSlot || !Player::isTriggered(%this,%i) || getRandom() > "0.2") 
				Player::dropItem(%this,%type);
		}
	}

 if(%cl != -1)
 {
	if(%this.vehicle != "")	{
		if(%this.driver != "")
 		{
			%this.driver = "";
		Client::setControlObject(Player::getClient(%this), %this);
		Player::setMountObject(%this, -1, 0);
		}
		else
 		{











			%this.vehicle.Seat[%this.vehicleSlot-2] = "";
			%this.vehicleSlot = "";
		}
		%this.vehicle = "";		
	}
 	schedule("GameBase::startFadeOut(" @ %this @ ");", $CorpseTimeoutValue, %this);
 	Client::setOwnedObject(%cl, -1);
 	Client::setControlObject(%cl, Client::getObserverCamera(%cl));
 	Observer::setOrbitObject(%cl, %this, 5, 5, 5);

	if((Player::isAiControlled(%this) && $Spoonbot::DelOnSpawn) && $SpoonBot::DoNotRespawnAI[%this] != %this)
		return;
	else
	 	schedule("deleteObject(" @ %this @ ");", $CorpseTimeoutValue + 2.5, %this);

 	%cl.observerMode = "dead";
 	%cl.dieTime = getSimTime();

	if($C4Bomb[%this] != "")
		$C4::BlowUp[%player] = true;
 }
}

function Player::onDamage(%this,%type,%value,%pos,%vec,%mom,%vertPos,%quadrant,%object)
{
	if(%this.isGraceTime)
		return;

	if(!%object && Player::getClient(%this).whichBot)
		%object = Player::getClient(%this).whichBot;

	%orgVal=%value;
	$oldArmor[Player::getClient(%this)] = Player::getArmor(Player::getClient(%this));
	$oldArmor[%object] = Player::getArmor(%object);
	$oldPos[Player::getClient(%this)] = GameBase::getPosition(Player::getClient(%this));
	$oldPos[Player::getClient(%object)] = GameBase::getPosition(Player::getClient(%object));

	if (Player::isExposed(%this)) {
%damagedClient = Player::getClient(%this);
%shooterClient = %object;

 	 %damagedPlayer = Client::getOwnedObject(%damagedClient);
	 %shooterPlayer = Client::getOwnedObject(%object);
	 %shooterarmor = Player::getArmor(%shooterPlayer);
	 Player::applyImpulse(%this,%mom);
		if($teamplay && %damagedClient != %shooterClient && Client::getTeam(%damagedClient) == Client::getTeam(%shooterClient) ) {
			if (%shooterClient != -1) {
				%curTime = getSimTime();
			 if ((%curTime - %this.DamageTime > 3.5 || %this.LastHarm != %shooterClient) && %damagedClient != %shooterClient && $Server::TeamDamageScale > 0) {
					if(%type != $MineDamageType)
					{
						Client::sendMessage(%shooterClient,0,"You just harmed Teammate " @ Client::getName(%damagedClient) @ "!");
						Client::sendMessage(%damagedClient,0,"You took Friendly Fire from " @ Client::getName(%shooterClient) @ "!");
					}
					else
					{
						Client::sendMessage(%shooterClient,0,"You just harmed Teammate " @ Client::getName(%damagedClient) @ " with your mine!");
						Client::sendMessage(%damagedClient,0,"You just stepped on Teammate " @ Client::getName(%shooterClient) @ "'s mine!");
					}
					%this.LastHarm = %shooterClient;
					%this.DamageStamp = %curTime;
				}
			}
			%friendFire = $Server::TeamDamageScale;
		}
		else if(%type == $ImpactDamageType && Client::getTeam(%object.clLastMount) == Client::getTeam(%damagedClient)) 
			%friendFire = $Server::TeamDamageScale;
		else
			%friendFire = 1.0;	

		//=======================================================================================//
		//=======================Start TD Bounce Back CODE!======================================//
		//=======================================================================================//

		if(($NovaMorpher::TDBackDamage && Client::getTeam(%damagedClient) == Client::getTeam(%shooterClient) && %damagedClient != %shooterClient) && %shooterClient != "0")
		{
			if($debug)
				echo("Client " @ %shooterClient @ " hurted " @ %damagedClient @ ". Deciding status...");

			%damagedPos = GameBase::getPosition(%shooterPlayer);
			%shooterPos = GameBase::getPosition(%damagedPlayer);
			%distance = Vector::getDistance(%shooterPos, %damagedPos);

			if(%distance <$NovaMorpher::AccidentRange)
			{
				if($debug)
					echo("Client " @ %shooterClient @ " is not guilty team damage...");
			}
			else
			{
				if($debug)
					echo("Client " @ %shooterClient @ " is guilty of team damage...");
	
				%tdvalue = $DamageScale[%shooterarmor, %type] * %value;
	 		%dlevel = GameBase::getDamageLevel(%shooterPlayer) + (%tdvalue * %friendFire);
	 		GameBase::setDamageLevel(%shooterPlayer,%dlevel);
				%flash = Player::getDamageFlash(%object) + %tdvalue * 2;
				if (%flash > 0.75) 



					%flash = 0.75;
	
				Player::setDamageFlash(%shooterPlayer,%flash);
				if(!Player::isDead(%shooterPlayer))
				{ 
					if(%shooterClient.lastDamage < getSimTime())
					{
						%sound = radnomItems(3,injure1,injure2,injure3);
						playVoice(%damagedClient,%sound);
						%damagedClient.lastdamage = getSimTime() + 1.5;
					}
				}
				else
				{
					%curDie = $PlayerAnim::DieChest;
					Player::setAnimation(%shooterPlayer, %curDie);

					if(%type == $ImpactDamageType && %object.clLastMount != "")
						%shooterClient = %object.clLastMount;
					Client::onKilled(%shooterClient,%shooterClient, %type);

				}
			}
			if($debug)
				echo("Shootting range: " @ %distance);
		}

		//=======================================================================================//
		//=========================END TD Bounce Back CODE!======================================//
		//=======================================================================================//

		if (!Player::isDead(%this)) {
			%armor = Player::getArmor(%this);
			//More damage applyed to head shots
			if(%vertPos == "head")
			{
				if (%type == $LaserDamageType || %type == $SniperDamageType)
				{
					%value += (%value * 2);
				}
				else
				{
					if(%armor == "harmor")
					{ 
						if(%quadrant == "middle_back" || %quadrant == "middle_front" || %quadrant == "middle_middle")
						{
							%value += (%value * 1.5);
						}
					}
					else
					{
						%value += (%value * 1);
					}
				}
			}
			//If Shield Pack is on
			%plasmaCheck =(%type != $FireDamageType && %type != $PlasmaDamageType);
			if (%type != -1 && %plasmaCheck && %this.shieldStrength > 0)
			{
				%energy = GameBase::getEnergy(%this);
				%strength = %this.shieldStrength;
				if (%type == $ShrapnelDamageType || %type == $MortarDamageType)
					%strength *= 0.75;

				%absorb = %energy * %strength;
				if (%value < %absorb)
				{
					GameBase::setEnergy(%this,%energy - ((%value / %strength)*%friendFire));
					%thisPos = getBoxCenter(%this);
					%offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
					GameBase::activateShield(%this,%vec,%offsetZ);
					%value = 0;

					return; //== Aviod aftermath of this.....
				}
				else
				{
					GameBase::setEnergy(%this,0);
					%value = %value - %absorb;
				}
			}

			if (%type != -1 && %this.holyShield)
			{
				%caster = %this.holyShield.caster; if($debug){echo("%caster :"@%caster);}
				%casterCl = Player::getClient(%caster); if($debug){echo("%casterCl :"@%casterCl);}
				%casterAr = Player::getArmor(%casterCl); if($debug){echo("%casterAr :"@%casterAr);}

				%strength = %this.holyShield.strength;
				%energy = Player::getItemCount(%caster, mana) + GameBase::getEnergy(%caster) + (0.375 - GameBase::getDamageLevel())*100;

				if(%casterAr == "magearmor" || %casterAr == "magefemale")
				{
					if (%type == $ShrapnelDamageType || %type == $MortarDamageType)
						%strength *= 0.75;

					%absorb = %energy * %strength;
					if (%value < %absorb)
					{
						%energyPercent = %energy/100;
						%energy = %energy - ((%value / %strength)*%friendFire);
						%energyPLeft = %energy/%energyPercent;

						%perPercent = %energy/100;
						%manaRatio = Player::getItemCount(%caster, mana)/%perPercent;
						%energyRatio = GameBase::getEnergy(%caster)/%perPercent;
						%dmgRatio = ((0.375 - GameBase::getDamageLevel())*100)/%perPercent;


						Player::setItemCount(%caster, mana, %manaRatio*%energyPLeft);
						GameBase::setEnergy(%caster, %energyRatio*%energyPLeft);
						GameBase::setDamageLevel(%caster, %dmgRatio*%energyPLeft);

						%thisPos = getBoxCenter(%this);
						%offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
						GameBase::activateShield(%this,%vec,%offsetZ);

						%bonus = %value * 10;
						%casterCl.score += %bonus;
						bottomprint(%killerId,"<jc><f1>"@%casterCl.score-%bonus@" + "@%bonus@" = <f0>"@%casterCl.score@"\n<f1>Damage Taken: <f0>"@%value,5);

						%value = 0;
						return; //== Aviod aftermath of this..... :O
					}
					else
					{
						remoteKill(%casterCl);
						ShieldDamageType::RemoveShield(%this);
						%value = %value - %absorb;

						%bonus = %value * 10;
						%casterCl.score += %bonus;
						bottomprint(%killerId,"<jc><f1>"@%casterCl.score-%bonus@" + "@%bonus@" = <f0>"@%casterCl.score@"\n<f1>Damage Taken: <f0>"@%value,5);

						client::sendMessage(%damagedClient, 1, "Your enchanted warrior(s) has killed you!");
					}
				}
				else
					ShieldDamageType::RemoveShield(%this);
			}

			if (%type != -1 && Player::getMountedItem(%damagedPlayer, 0) == "ShieldWep")
			{
				%stringA = String::replace(%quadrant, "_", " ");
				%side = getWord(%stringA, 0);
				if(%side == "front" || %side == "middle")
				{
					if(%side == "front")
						%multi = 1;
					else
						%multi = 0.6;

					%energy = GameBase::getEnergy(%this) * 0.2 * %multi; //== No, your not god
					if(%energy > 0)
					{
						if(%energy > %value)
						{
							GameBase::setEnergy(%damagedPlayer, %energy - %value);
							%value = 0;
						}
						else
						{
							GameBase::setEnergy(%damagedPlayer, 0);
							Player::unmountItem(%damagedPlayer, 0);
							%value -= %energy;

							client::Sendmessage(%damagedClient, 1, "Your particle shield has been over powered!");
						}
					}
				}
			}

			if ((%type == $FireDamageType || %type == $PlasmaDamageType) && %this.heatSync > 0)
			{
				%energy = GameBase::getEnergy(%this);
				%MaxEnergy = %armor.maxEnergy;
				%strength = %this.heatSync;
				if((%absorb = (%energy/(%MaxEnergy/100)) * %strength * 0.01)>=0.75)
				{
					%fire2energyTrans = 1/2*%absorb*%value;
					GameBase::setEnergy(%this,%energy + %fire2energyTrans);
					%value = 0;

					%thisPos = getBoxCenter(%this);
					%offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
					GameBase::activateShield(%this,%vec,%offsetZ);
					%offsetZ =((getWord(%thisPos,2))-(getWord(%pos,2)));
					GameBase::activateShield(%this,%vec,%offsetZ);



				}
				else
					%value = %value - (%value * %absorb);
			}

			if(%armor == "borgarmor" || %armor == "borgfemale")
			{
				%borgTeam = Client::getTeam(%damagedClient);
				if($Borg::Immune[%borgTeam, %type])
				{
					%thisPos = getBoxCenter(%this);
					%offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
					Borg::shieldAffect(%borgTeam,%vec,%offsetZ);
					return;
				}
				else
					Borg::Adapt(%this, %borgTeam, %type);
			}

			if (%value > "0")
			{
				%DamageScale = $DamageScale[%armor, %type];
				if($DamageScale[%armor, %type] == "")
					%DamageScale = 1.0;
				%value = %DamageScale * %value * %friendFire;
		%dlevel = GameBase::getDamageLevel(%this) + %value;
		%spillOver = %dlevel - %armor.maxDamage;
				if(%armor.maxDamage <= %dlevel && (%armor == "magearmor" || %armor == "magefemale"))
				{
					Player::blowUp(%damagedClient);
					%obj = newObject("","Mine","InstantExplosive");
					GameBase::throw(%obj, %damagedClient, 0, false);	
					GameBase::setPosition(%obj, gamebase::getposition(%this));
				}
				GameBase::setDamageLevel(%this,%dlevel);
				%flash = Player::getDamageFlash(%this) + %value * 2;
				if (%flash > 0.75) 
					%flash = 0.75;
				Player::setDamageFlash(%this,%flash);
				//If player not dead then play a random hurt sound
				if(!Player::isDead(%this))
				{ 
					if(%damagedClient.lastDamage < getSimTime())
					{
						%sound = radnomItems(3,injure1,injure2,injure3);
						playVoice(%damagedClient,%sound);
						%damagedClient.lastdamage = getSimTime() + 1.5;
					}

					DoSpecialDamageCheck(%shooterPlayer, %damagedPlayer, %type, %shooterClient);
				}
				else
				{
 			if(%spillOver > 0.5 && (%type== $ExplosionDamageType || %type == $ShrapnelDamageType || %type== $MortarDamageType|| %type == $MissileDamageType|| %type == $MeteoroidDamageType))
					{
		 				Player::trigger(%this, $WeaponSlot, false);
						%weaponType = Player::getMountedItem(%this,$WeaponSlot);
						if(%weaponType != -1)
							Player::dropItem(%this,%weaponType);
			Player::blowUp(%this);
					}

					else
					{
						if ((%value > 0.40 && (%type== $ExplosionDamageType || %type == $ShrapnelDamageType || %type== $MortarDamageType || %type == $MissileDamageType )) || (Player::getLastContactCount(%this) > 6) )
						{
							if(%quadrant == "front_left" || %quadrant == "front_right") 
								%curDie = $PlayerAnim::DieBlownBack;
							else
								%curDie = $PlayerAnim::DieForward;
						}
						else if( Player::isCrouching(%this) ) 
							%curDie = $PlayerAnim::Crouching;							
						else if(%vertPos=="head")
						{
							if(%quadrant == "front_left" ||	%quadrant == "front_right"	) 
								%curDie = radnomItems(2, $PlayerAnim::DieHead, $PlayerAnim::DieBack);
							else 
								%curDie = radnomItems(2, $PlayerAnim::DieHead, $PlayerAnim::DieForward);
						}
						else if (%vertPos == "torso")
						{
							if(%quadrant == "front_left" ) 
								%curDie = radnomItems(3, $PlayerAnim::DieLeftSide, $PlayerAnim::DieChest, $PlayerAnim::DieForwardKneel);
							else if(%quadrant == "front_right") 
								%curDie = radnomItems(3, $PlayerAnim::DieChest, $PlayerAnim::DieRightSide, $PlayerAnim::DieSpin);
							else if(%quadrant == "back_left" ) 
								%curDie = radnomItems(4, $PlayerAnim::DieLeftSide, $PlayerAnim::DieGrabBack, $PlayerAnim::DieForward, $PlayerAnim::DieForwardKneel);
							else if(%quadrant == "back_right") 
								%curDie = radnomItems(4, $PlayerAnim::DieGrabBack, $PlayerAnim::DieRightSide, $PlayerAnim::DieForward, $PlayerAnim::DieForwardKneel);
						}
						else if (%vertPos == "legs")
						{
							if(%quadrant == "front_left" ||	%quadrant == "back_left") 
								%curDie = $PlayerAnim::DieLegLeft;
							if(%quadrant == "front_right" ||	%quadrant == "back_right") 
								%curDie = $PlayerAnim::DieLegRight;
						}
						Player::setAnimation(%this, %curDie);
					}

					if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient))
						awardPoints(%damagedClient,%shooterClient, %type, %vertPos, %orgVal);

					if(%type == $ImpactDamageType && %object.clLastMount != "")
						%shooterClient = %object.clLastMount;

					//== Special "death" checking
					if($isParasiteSlave[%damagedPlayer])
						ParasiteDamageType::RemoveParasite(%damagedClient);
					else if($MastersSlave[%damagedClient] > 2048)
					{
						ParasiteDamageType::RemoveParasite($MastersSlave[%damagedClient]);
						remoteKill($MastersSlave[%damagedClient]); //== MEWHAHHAAH
					}

					if(%damagedPlayer.curFig != "")
					{
						GameBase::setDamageLevel(%damagedPlayer.curFig, 10000000);
						%damagedPlayer.curFig = "";
					}

					if(%this.controller)
					{
 						schedule("GameBase::startFadeOut(" @ %this @ ");", $CorpseTimeoutValue-1);
					 	schedule("deleteObject(" @ %this @ ");", $CorpseTimeoutValue + 1.5);

						Client::setControlObject(%this.controller, Client::getOwnedObject(%this.controller));
						%this.controller.drone = "";
						%this.controller = "";
					}
					else if(%damagedClient.drone)
					{
 						schedule("GameBase::startFadeOut(" @ %damagedClient.drone @ ");", $CorpseTimeoutValue-1);
					 	schedule("deleteObject(" @ %damagedClient.drone @ ");", $CorpseTimeoutValue + 1.5);

						%damagedClient.drone.controller = "";
						Player::blowUp(%damagedClient.drone);
						GameBase::setDamageLevel(%damagedClient.drone, 10000000);
						%damagedClient.drone = "";
					}

					Client::onKilled(%damagedClient,%shooterClient, %type, %vertPos, %orgVal);
 				}
			}
		}
	}
}

function awardPoints(%playerId, %killerId, %damageType, %vertPos, %dmgDealt)
{
	if(%playerId != %killerId)
	{
		if(%vertPos == "head")
		{
			if (%damageType == $LaserDamageType || %damageType == $SniperDamageType)
			{
				%rand = floor(getRandom() * 100)/10;
				bottomprint(%killerId,"<jc><f1>Head Shot! <f3>+" @ %rand,5);
				bottomprint(%playerId,"<jc><f1>Head Shot! Ouchi!\n" @ Client::getName(%killerId) @ " got <f3>+" @ %rand @ "<f1> points for that kill!\nThink about <f2>REVENGE<f1> :-) Hehehehe...",15);
				%killerId.score = %killerId.score + %rand;
				%playerId.sniperKill = true;
			}
		}
		else
		{
			%plPos = $oldPos[%playerId];
			%klPos = GameBase::getPosition(%killerId);
			%distance = Vector::getDistance(%klPos, %plPos);
	
			if(%dmgDealt < 0)
				%dmgDealt = 0;
	
			%bonus = %distance/%dmgDealt;
			while(%bonus > 25) //== I don't want people to start getting thousands!
			{
				%bonus = floor(%bonus/2);
			}

			%oldScore = floor(%killerId.score);
			%killerId.score = %oldScore + %bonus;
			if((%killerId.score*0) != 0 || %killerId.score == "+INF")
				%killerId.score = 0;
	
			bottomprint(%killerId,"<jc><f1>"@%oldScore@" + "@%bonus@" = <f0>"@%killerId.score@"\n<f1>Distance: <f0>"@%distance,5);
			Game::refreshClientScore(%playerId);
			Game::refreshClientScore(%killerId);
		}
	}
}


function radnomItems(%num, %an0, %an1, %an2, %an3, %an4, %an5, %an6, %an7, %an8, %an9, 
				   %an10, %an11, %an12, %an13, %an14, %an15, %an16, %an17, %an18, %an19,
				   %an20, %an21, %an22, %an23, %an24, %an25, %an26, %an27, %an28, %an29)
{
	return %an[floor(getRandom() * (%num - 0.01))];
}

function Player::onCollision(%this,%object)
{
	if($debug)
		echo("Player " @ %this @ " collision with " @ %object);

	%armor = Player::getArmor(%this);
	%isBorg = %armor == "borgarmor" || %armor == "borgfemale";
	%objTeam = GameBase::getTeam(%object);
	%thisTeam = GameBase::getTeam(%this);

	if (getObjectType(%object) == "Player")
	{
		%objArmor = Player::getArmor(%object);
		if (Player::isDead(%this))
		{
			// Transfer all our items to the player
			%sound = false;
			%max = getNumItems();
			for (%i = 0; %i < %max; %i = %i + 1)
			{
				%count = Player::getItemCount(%this,%i);
				if (%count)
				{
					%delta = Item::giveItem(%object,getItemData(%i),%count);
					if (%delta > 0)
					{
						Player::decItemCount(%this,%i,%delta);
						%sound = true;
					}
				}
			}
			if (%sound)
			{
				// Play pickup if we gave him anything
				playSound(SoundPickupItem,GameBase::getPosition(%this));

			}
		}
		else if((%armor == "undeadmagearmor" || %armor == "undeadmagefemale") && (%objArmor != "undeadmagearmor" && %objArmor != "undeadmagefemale"))
		{
			%clientId = Player::getClient(%this);
			Client::sendMessage(%clientId, 1, "Satan: The living has touched you! You are now punished to be the DEAD!");
			remoteKill(%clientId);

			%clientId = Player::getClient(%object);
			Client::sendMessage(%clientId, 3, "Satan: You have touched the living DEAD, hence you killed it!");
		}
	}
}

function Player::getHeatFactor(%this)
{
	//------------- Original By Dynamix -------------------//
	// Hack to avoid turret turret not tracking vehicles.  //
	// Assumes that if we are not in the player we are     //
	// controlling a vechicle, which is not always correct // <-- More like almost ALWAYS
	// but should be OK for now.                           //
	//-----------------------------------------------------//
	//if(Client::getControlObject(%client) != %this)       //
	//	return 1.0;                                      //
	//-----------------------------------------------------//


	%client = Player::getClient(%this);
	%player = Client::getOwnedObject(%client);
	if(%player.driver == 1 || Player::isAiControlled(%player) || %player.Burntime > 0)
		return 1.0;

	%time = getIntegerTime(true) >> 5;
	%lastTime = Player::lastJetTime(%this) >> 10;

	if((%lastTime + 1.5) < %time)
		return 0.0;
	else
	{
		%diff = %time - %lastTime;
		%heat = 1.0 - (%diff / 1.5);
		return %heat;
	}
}

function Player::jump(%this,%mom)
{
	%cl = GameBase::getControlClient(%this);
	if(%cl != -1)
	{
		%vehicle = Player::getMountObject (%this);
		%this.lastMount = %vehicle;
		%this.newMountTime = getSimTime() + 3.0;
		Player::setMountObject(%this, %vehicle, 0);
		Player::setMountObject(%this, -1, 0);
		Player::applyImpulse(%pl,%mom);
		playSound (GameBase::getDataName(%this).dismountSound, GameBase::getPosition(%this));
	}

}


//----------------------------------------------------------------------------

function remoteKill(%client)
{
	if(!$matchStarted)
		return;
	%player = Client::getOwnedObject(%client);

	if($isHeliumMonster[%client])
	{
		%pos = GameBase::getPosition(%player);
		Player::onDamage(%player,$FireDamageType,250,pos,"","" ,"torso" ,"front_right" ,%client);
		bottomprint(%client,"<jc>It is not that easy to die :-). Don't worry!\nIf you have full health, it will take about 400 suicides!",10);
		return;
	}
	else if($isParasiteSlave[%player])
	{
		bottomprint(%client,"<jc>\n<f2>You are a SLAVE, not a human\n\n<f3>You have <f0>NO RIGHT<f3> to commit this act!\n",10);
		return;
	}

	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
	{
		for(%i = 0; $DamageType::Reserve[%i]; %i++){}
		%type = floor(getRandom()*(%i-1))+1;

		Player::trigger(%player,$BackpackSlot,false);
		Player::onDamage(%player,$FireDamageType,999999,pos,"","" ,"torso" ,"front_right" ,%client); //=== Bahahahhaa, over kill!
		if(!Player::isDead(%player))
		{
			playNextAnim(%client);
			Player::kill(%client);
			Client::onKilled(%client,%client);
		}
	}
}

$animNumber = 25;	 
function playNextAnim(%client)
{
	if($animNumber > 36) 
		$animNumber = 25;		
	Player::setAnimation(%client,$animNumber++);
}

function Client::takeControl(%clientId, %objectId)
{
	// remote control
	if(%objectId == -1)
	{
		//echo("objectId = " @ %objectId);
		return;
	}

	%pl = Client::getOwnedObject(%clientId);
	// If mounted to a vehicle then can't mount any other objects
	if(%pl.driver != "" || %pl.vehicleSlot != "")
		return;

	if(GameBase::getTeam(%objectId) != Client::getTeam(%clientId))
	{
		//echo(GameBase::getTeam(%objectId) @ " " @ Client::getTeam(%clientId));
		return;
	}
	if(GameBase::getControlClient(%objectId) != -1)
	{
		echo("Ctrl Client = " @ GameBase::getControlClient(%objectId));
		return;
	}
	%name = GameBase::getDataName(%objectId);
	if((%name == PlasmaTurret && %name == ELFTurret && %name == RocketTurret && %name == MortarTurret) || $NeedPowerCheck[%name])
	{
		if(!GameBase::isPowered(%objectId)) 
		{
			return;
		}
	}
	else if(%name == AntiSniperTurret || (!$CanControl[%name] && $CanControl[%name] != ""))
	{
		Client::SendMessage(%clientId,0,"Cannot control this type of turret!");
		return;
 	}

	if(!(Client::getOwnedObject(%clientId)).CommandTag && GameBase::getDataName(%objectId) != CameraTurret && !$TestCheats && (!$NoComStation[%name] || $NoComStation[%name] == ""))
	{
		Client::SendMessage(%clientId,0,"Must be at a Command Station to control turrets");
 		return;
	}
	if(GameBase::getDamageState(%objectId) == "Enabled")
	{
 		Client::setControlObject(%clientId, %objectId);
 		Client::setGuiMode(%clientId, $GuiModePlay);
	}

}

function remoteCmdrMountObject(%clientId, %objectIdx)
{
	Client::takeControl(%clientId, getObjectByTargetIndex(%objectIdx));
}

function checkControlUnmount(%clientId)
{
	%ownedObject = Client::getOwnedObject(%clientId);
	%ctrlObject = Client::getControlObject(%clientId);
	if(%ownedObject != %ctrlObject)
	{
		%drone = %clientId.drone;
		if(%drone != "")
		{
			Player::blowUp(%drone);
			GameBase::setDamageLevel(%drone, 10000000);
		 	schedule("GameBase::startFadeOut(" @ %drone @ ");", 0);
		 	schedule("deleteObject(" @ %drone @ ");", 2.5);

			%clientId.drone.controller = "";
			%clientId.drone = "";
		}

		if(%ownedObject == -1 || %ctrlObject == -1)
			return;
		if(getObjectType(%ownedObject) == "Player" && Player::getMountObject(%ownedObject) == %ctrlObject)
			return;

		Client::setControlObject(%clientId, %ownedObject);
	}
}
