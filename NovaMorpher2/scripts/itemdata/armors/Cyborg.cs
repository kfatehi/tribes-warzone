//== A basic am armor....


  //----------------------------------------------------------------------------
  // SniperX Armor
  //----------------------------------------------------------------------------
//========================================================================
//========================================================================
  $DamageScale[borgarmor, $LandingDamageType] = 0.9;
  $DamageScale[borgarmor, $ImpactDamageType] = 0.9;
  $DamageScale[borgarmor, $CrushDamageType] = 0.9;
  $DamageScale[borgarmor, $BulletDamageType] = 0.9;
  $DamageScale[borgarmor, $PlasmaDamageType] = 0.5;
  $DamageScale[borgarmor, $EnergyDamageType] = 0.9;
  $DamageScale[borgarmor, $ExplosionDamageType] = 0.9;
  $DamageScale[borgarmor, $MissileDamageType] = 0.9;
  $DamageScale[borgarmor, $ShrapnelDamageType] = 0.9;
  $DamageScale[borgarmor, $DebrisDamageType] = 0.9;
  $DamageScale[borgarmor, $LaserDamageType] = 0.9;
  $DamageScale[borgarmor, $MortarDamageType] = 0.9;
  $DamageScale[borgarmor, $BlasterDamageType] = 0.9;
  $DamageScale[borgarmor, $ElectricityDamageType] = 1.5;
  $DamageScale[borgarmor, $MineDamageType] = 0.9;
//========================================================================
  $DamageScale[borgfemale, $LandingDamageType] = 0.9;
  $DamageScale[borgfemale, $ImpactDamageType] = 0.9;
  $DamageScale[borgfemale, $CrushDamageType] = 0.9;
  $DamageScale[borgfemale, $BulletDamageType] = 0.9;
  $DamageScale[borgfemale, $PlasmaDamageType] = 0.5;
  $DamageScale[borgfemale, $EnergyDamageType] = 0.9;
  $DamageScale[borgfemale, $ExplosionDamageType] = 0.9;
  $DamageScale[borgfemale, $MissileDamageType] = 0.9;
  $DamageScale[borgfemale, $ShrapnelDamageType] = 0.9;
  $DamageScale[borgfemale, $DebrisDamageType] = 0.9;
  $DamageScale[borgfemale, $LaserDamageType] = 0.9;
  $DamageScale[borgfemale, $MortarDamageType] = 0.9;
  $DamageScale[borgfemale, $BlasterDamageType] = 0.9;
  $DamageScale[borgfemale, $ElectricityDamageType] = 1.5;
  $DamageScale[borgfemale, $MineDamageType] = 0.9;

//========================================================================
//========================================================================
  $ItemMax[borgarmor, TargetingLaser] = 1;
  $ItemMax[borgarmor, Mineammo] = 3;
  $ItemMax[borgarmor, Grenade] = 3;
  $ItemMax[borgarmor, Beacon]  = 3;
//========================================================================
  $ItemMax[borgfemale, TargetingLaser] = 1;
  $ItemMax[borgfemale, Mineammo] = 3;
  $ItemMax[borgfemale, Grenade] = 3;
  $ItemMax[borgfemale, Beacon]  = 3;
  
//========================================================================
//========================================================================
  $ItemMax[borgarmor, EnergyPack] = 1;
  $ItemMax[borgarmor, SensorJammerPack] = 1;
  $ItemMax[borgarmor, RepairKit] = 2;
//========================================================================
  $ItemMax[borgfemale, EnergyPack] = 1;
  $ItemMax[borgfemale, SensorJammerPack] = 1;
  $ItemMax[borgfemale, RepairKit] = 2;

//========================================================================
//========================================================================
  $MaxWeapons[borgarmor] = 3;
  $MaxWeapons[borgfemale] = 3;



  PlayerData borgarmor
  {
     className = "Armor";
     shapeFile = "marmor";
     flameShapeName = "mflame";
     shieldShapeName = "shield";
     damageSkinData = "armorDamageSkins";
  	debrisId = playerDebris;
     shadowDetailMask = 1;
     validateShape = false;
  
     canCrouch = false;
     visibleToSensor = True;
  	mapFilter = 1;
  	mapIcon = "M_player";
  
     maxJetSideForceFactor = 0.6;
     maxJetForwardVelocity = 12.75;
     minJetEnergy = 1;
     jetForce = 400;
     jetEnergyDrain = 1.0;
  
  	maxDamage = 1.0;
     maxForwardSpeed = 5.5;
     maxBackwardSpeed = 4.4;
     maxSideSpeed = 4;
     groundForce = 35 * 13.0;
     mass = 13.0;
     groundTraction = 3.0;
  	
  	maxEnergy = 100;
     drag = 1.0;
     density = 1.5;
  
  	minDamageSpeed = 16.54321;
  	damageScale = 0.005;
  
     jumpImpulse = 55;
     jumpSurfaceMinDot = 0.2;
  
     // animation data:
     // animation name, one shot, exclude, direction
  	// firstPerson, chaseCam, thirdPerson, signalThread
  
     // movement animations:
     animData[0]  = { "root", none, 1, true, true, true, false, 0 };
//     animData[1]  = { "run", none, 1, true, false, true, false, 3 };
//     animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
//     animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
//     animData[4]  = { "side left", none, -1, true, false, true, false, 3 };

     animData[1]  = { "jump run", none, 1, true, false, true, false, 3 };
     animData[2]  = { "jump run", none, 1, true, false, true, false, 3 };
     animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
     animData[4]  = { "side left", none, -1, true, false, true, false, 3 };

     animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
     animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
     animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
     animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
     animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
     animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
     animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
     animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
     animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
     animData[14]  = { "run", none, 1, true, true, true, false, 3 };
     animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
     animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
     animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
     animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
     animData[19] = { "run", none, 1, true, true, true, false, 3 };
  
     // misc. animations:
     animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
     animData[21] = { "throw", none, 1, true, false, false, false, 3 };
     animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
     animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
     animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
     
     // death animations:
     animData[25] = { "crouch die", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[26] = { "die chest", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[27] = { "die head", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[28] = { "die grab back", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[29] = { "die right side", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[30] = { "die left side", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[31] = { "die leg left", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[32] = { "die leg right", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[33] = { "die blown back", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[34] = { "die spin", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[35] = { "die forward", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[36] = { "die forward kneel", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[37] = { "die back", SoundPlayerDeath, 1, false, false, false, false, 4 };
  
     // signal moves:
  	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
     animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
     animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
     animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
     animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 
  
      // celebraton animations:
     animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
     animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
     animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
  
      // taunt anmations:
     animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
     animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
  
      // poses:
     animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
     animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };
  
  	// Bonus wave
     animData[50] = { "wave", none, 1, true, false, false, true, 1 };
  
     jetSound = SoundJetLight;
  
     rFootSounds = 
     {
       SoundMFootRSoft,
       SoundMFootRHard,
       SoundMFootRSoft,
       SoundMFootRHard,
       SoundMFootRSoft,
       SoundMFootRSoft,
       SoundMFootRSoft,
       SoundMFootRHard,
       SoundMFootRSnow,
       SoundMFootRSoft,
       SoundMFootRSoft,
       SoundMFootRSoft,
       SoundMFootRSoft,
       SoundMFootRSoft,
       SoundMFootRSoft
    }; 
     lFootSounds =
     {
        SoundMFootLSoft,
        SoundMFootLHard,
        SoundMFootLSoft,
        SoundMFootLHard,
        SoundMFootLSoft,
        SoundMFootLSoft,
        SoundMFootLSoft,
        SoundMFootLHard,
        SoundMFootLSnow,
        SoundMFootLSoft,
        SoundMFootLSoft,
        SoundMFootLSoft,
        SoundMFootLSoft,
        SoundMFootLSoft,
        SoundMFootLSoft
     };
  
     footPrints = { 2, 3 };
  
     boxWidth = 0.7;
     boxDepth = 0.7;
     boxNormalHeight = 2.4;
  
     boxNormalHeadPercentage  = 0.83;
     boxNormalTorsoPercentage = 0.49;
  
     boxHeadLeftPercentage  = 0;
     boxHeadRightPercentage = 1;
     boxHeadBackPercentage  = 0;
     boxHeadFrontPercentage = 1;
  };


  PlayerData borgfemale
  {
     className = "Armor";
     shapeFile = "mfemale";
     flameShapeName = "mflame";
     shieldShapeName = "shield";
     damageSkinData = "armorDamageSkins";
  	debrisId = playerDebris;
     shadowDetailMask = 1;
     validateShape = false;
  
     canCrouch = false;
     visibleToSensor = True;
  	mapFilter = 1;
  	mapIcon = "M_player";
  
     maxJetSideForceFactor = 0.6;
     maxJetForwardVelocity = 12.75;
     minJetEnergy = 1;
     jetForce = 400;
     jetEnergyDrain = 1.0;
  
  	maxDamage = 1.0;
     maxForwardSpeed = 5.5;
     maxBackwardSpeed = 4.4;
     maxSideSpeed = 4;
     groundForce = 35 * 13.0;
     mass = 13.0;
     groundTraction = 3.0;
  	
  	maxEnergy = 100;
     drag = 1.0;
     density = 1.5;
  
  	minDamageSpeed = 16.54321;
  	damageScale = 0.005;
  
     jumpImpulse = 55;
     jumpSurfaceMinDot = 0.2;
  
     // animation data:
     // animation name, one shot, exclude, direction
  	// firstPerson, chaseCam, thirdPerson, signalThread
  
     // movement animations:
     animData[0]  = { "root", none, 1, true, true, true, false, 0 };
//     animData[1]  = { "run", none, 1, true, false, true, false, 3 };
//     animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
//     animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
//     animData[4]  = { "side left", none, -1, true, false, true, false, 3 };

     animData[1]  = { "jump run", none, 1, true, false, true, false, 3 };
     animData[2]  = { "jump run", none, 1, true, false, true, false, 3 };
     animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
     animData[4]  = { "side left", none, -1, true, false, true, false, 3 };

     animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
     animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
     animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
     animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
     animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
     animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
     animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
     animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
     animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
     animData[14]  = { "run", none, 1, true, true, true, false, 3 };
     animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
     animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
     animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
     animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
     animData[19] = { "run", none, 1, true, true, true, false, 3 };
  
     // misc. animations:
     animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
     animData[21] = { "throw", none, 1, true, false, false, false, 3 };
     animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
     animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
     animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
     
     // death animations:
     animData[25] = { "crouch die", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[26] = { "die chest", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[27] = { "die head", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[28] = { "die grab back", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[29] = { "die right side", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[30] = { "die left side", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[31] = { "die leg left", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[32] = { "die leg right", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[33] = { "die blown back", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[34] = { "die spin", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[35] = { "die forward", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[36] = { "die forward kneel", SoundPlayerDeath, 1, false, false, false, false, 4 };
     animData[37] = { "die back", SoundPlayerDeath, 1, false, false, false, false, 4 };
  
     // signal moves:
  	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
     animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
     animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
     animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
     animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 
  
      // celebraton animations:
     animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
     animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
     animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
  
      // taunt anmations:
     animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
     animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
  
      // poses:
     animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
     animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };
  
  	// Bonus wave
     animData[50] = { "wave", none, 1, true, false, false, true, 1 };
  
     jetSound = SoundJetLight;
  
     rFootSounds = 
     {
       SoundMFootRSoft,
       SoundMFootRHard,
       SoundMFootRSoft,
       SoundMFootRHard,
       SoundMFootRSoft,
       SoundMFootRSoft,
       SoundMFootRSoft,
       SoundMFootRHard,
       SoundMFootRSnow,
       SoundMFootRSoft,
       SoundMFootRSoft,
       SoundMFootRSoft,
       SoundMFootRSoft,
       SoundMFootRSoft,
       SoundMFootRSoft
    }; 
     lFootSounds =
     {
        SoundMFootLSoft,
        SoundMFootLHard,
        SoundMFootLSoft,
        SoundMFootLHard,
        SoundMFootLSoft,
        SoundMFootLSoft,
        SoundMFootLSoft,
        SoundMFootLHard,
        SoundMFootLSnow,
        SoundMFootLSoft,
        SoundMFootLSoft,
        SoundMFootLSoft,
        SoundMFootLSoft,
        SoundMFootLSoft,
        SoundMFootLSoft
     };
  
     footPrints = { 2, 3 };
  
     boxWidth = 0.7;
     boxDepth = 0.7;
     boxNormalHeight = 2.4;
  
     boxNormalHeadPercentage  = 0.83;
     boxNormalTorsoPercentage = 0.49;
  
     boxHeadLeftPercentage  = 0;
     boxHeadRightPercentage = 1;
     boxHeadBackPercentage  = 0;
     boxHeadFrontPercentage = 1;
  };


$ArmorType[Male, Borg] = borgarmor;
$ArmorType[Female, Borg] = borgfemale;

$ArmorName[borgarmor] = Borg;
$ArmorName[borgfemale] = Borg;

ItemData Borg
{
   heading = "aArmor";
	description = "Borg Drone";
	className = "Armor";
	price = 200;
};

//== Wether it is able to be bought or not at inventory stations and remote inv.
$InvList[Borg] = 1;
$RemoteInvList[Borg] = 0;

function Borg::Adapt(%player, %team, %damageType)
{
	if(!%player.isAdapting && !%damageType.adaptionType[%team] && %damageType != $ElectricityDamageType) //== Can't adapt to $ElectricityDamageType damage ;)
	{
		Borg::messageAll(%team, 3, "Attempting to adapt to damage type #"@(1234+%damageType)@"....");

		%timeLimit = $Server::timeLimit;
		if(%timeLimit < 1 || !%timeLimit)
			%timeLimit = 180;

		%adaptingTime = getRandom()*10;
		%adaptedTime = (getRandom()*3)+60+%adaptingTime;
		schedule("$Borg::Immune["@%team@","@%damageType@"] = 1;",%adaptingTime);
		schedule("Borg::messageAll("@%team@", 3, 'Damage type #"@(1234+%damageType)@" has been ADAPTED');",%adaptingTime);

		schedule("$Borg::Immune["@%team@","@%damageType@"] = 0;",%adaptedTime);

		%damageType.adaptionType = true;
		%player.isAdapting = true;
		schedule(%player@".isAdapting = false;",%adaptingTime+1);
		schedule(%damageType@".adaptionType["@%team@"] = false;",%adaptingTime+0.9999999);
	}
}

function Borg::messageAll(%team, %mtype, %message)
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%clTeam = Client::getTeam(%cl);
		if(%clTeam == %team)
		{
			%player = Client::getOwnedObject(%cl);
			%armor = Player::getArmor(%player);
			if(%armor == "borgarmor" || %armor == "borgfemale")
				Client::sendMessage(%cl, %mtype, "Collective: "@%message);
		}
	}

	if($dedicated)
		echo("Borg Message: " @ %message);
}
function Borg::HaveOtherTech(%clientId)
{
	%player = Client::getOwnedObject(%clientId);
	%armor = Player::getArmor(%player);

	%numweapon = Player::getItemClassCount(%clientId,"Weapon");
	%max = getNumItems(); 
	for (%i = 0; %i < %max; %i = %i + 1)
	{ 
		%item = getItemData(%i);
		%count = Player::getItemCount(%clientId,%item); // && %item != "Borg"
		if(%count && !$ItemMax[%armor, %item] && (%item.className == "Weapon" || %item.className == "Tool"))
			return true;
	}
	return false;
}

function Borg::Check(%clientId)
{
	%player = Client::getOwnedObject(%clientId);
	%armor = Player::getArmor(%player);
	if(%armor == "borgarmor" || %armor == "borgfemale")
		return true;
	else
		return false;
}

function Borg::shieldAffect(%team, %vec,%offsetZ)
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%clTeam = Client::getTeam(%cl);
		if(%clTeam == %team)
		{
			if(Borg::Check(%cl))
				GameBase::activateShield(Client::getOwnedObject(%cl),%vec,%offsetZ);
		}
	}
}
