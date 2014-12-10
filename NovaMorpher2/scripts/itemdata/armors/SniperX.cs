//== A basic am armor....


  //----------------------------------------------------------------------------
  // SniperX Armor
  //----------------------------------------------------------------------------
//========================================================================
//========================================================================
  $DamageScale[sniperxarmor, $LandingDamageType] = 0.9;
  $DamageScale[sniperxarmor, $ImpactDamageType] = 0.9;
  $DamageScale[sniperxarmor, $CrushDamageType] = 0.9;
  $DamageScale[sniperxarmor, $BulletDamageType] = 0.9;
  $DamageScale[sniperxarmor, $PlasmaDamageType] = 0.5;
  $DamageScale[sniperxarmor, $EnergyDamageType] = 0.9;
  $DamageScale[sniperxarmor, $ExplosionDamageType] = 0.9;
  $DamageScale[sniperxarmor, $MissileDamageType] = 0.9;
  $DamageScale[sniperxarmor, $ShrapnelDamageType] = 0.9;
  $DamageScale[sniperxarmor, $DebrisDamageType] = 0.9;
  $DamageScale[sniperxarmor, $LaserDamageType] = 0.9;
  $DamageScale[sniperxarmor, $MortarDamageType] = 0.9;
  $DamageScale[sniperxarmor, $BlasterDamageType] = 0.9;
  $DamageScale[sniperxarmor, $ElectricityDamageType] = 0.9;
  $DamageScale[sniperxarmor, $MineDamageType] = 0.9;
//========================================================================
  $DamageScale[sniperxfemale, $LandingDamageType] = 0.9;
  $DamageScale[sniperxfemale, $ImpactDamageType] = 0.9;
  $DamageScale[sniperxfemale, $CrushDamageType] = 0.9;
  $DamageScale[sniperxfemale, $BulletDamageType] = 0.9;
  $DamageScale[sniperxfemale, $PlasmaDamageType] = 0.5;
  $DamageScale[sniperxfemale, $EnergyDamageType] = 0.9;
  $DamageScale[sniperxfemale, $ExplosionDamageType] = 0.9;
  $DamageScale[sniperxfemale, $MissileDamageType] = 0.9;
  $DamageScale[sniperxfemale, $ShrapnelDamageType] = 0.9;
  $DamageScale[sniperxfemale, $DebrisDamageType] = 0.9;
  $DamageScale[sniperxfemale, $LaserDamageType] = 0.9;
  $DamageScale[sniperxfemale, $MortarDamageType] = 0.9;
  $DamageScale[sniperxfemale, $BlasterDamageType] = 0.9;
  $DamageScale[sniperxfemale, $ElectricityDamageType] = 0.9;
  $DamageScale[sniperxfemale, $MineDamageType] = 0.9;

//========================================================================
//========================================================================
  $ItemMax[sniperxarmor, Blaster] = 1;
  $ItemMax[sniperxarmor, LaserRifle] = 1;
  $ItemMax[sniperxarmor, Disclauncher] = 1;
  $ItemMax[sniperxarmor, PlasmaGun] = 1;
  $ItemMax[sniperxarmor, TargetingLaser] = 1;
  $ItemMax[sniperxarmor, Mineammo] = 3;
  $ItemMax[sniperxarmor, Grenade] = 3;
  $ItemMax[sniperxarmor, Beacon]  = 3;
//========================================================================
  $ItemMax[sniperxfemale, Blaster] = 1;
  $ItemMax[sniperxarmor, LaserRifle] = 1;
  $ItemMax[sniperxfemale, Disclauncher] = 1;
  $ItemMax[sniperxfemale, PlasmaGun] = 1;
  $ItemMax[sniperxfemale, TargetingLaser] = 1;
  $ItemMax[sniperxfemale, Mineammo] = 3;
  $ItemMax[sniperxfemale, Grenade] = 3;
  $ItemMax[sniperxfemale, Beacon]  = 3;
  
//========================================================================
//========================================================================
  $ItemMax[sniperxarmor, Plasmaammo] = 50;
  $ItemMax[sniperxarmor, Bulletammo] = 50;
  $ItemMax[sniperxarmor, Discammo] = 50;
//========================================================================
  $ItemMax[sniperxfemale, Plasmaammo] = 50;
  $ItemMax[sniperxfemale, Bulletammo] = 50;
  $ItemMax[sniperxfemale, Discammo] = 50;
  
//========================================================================
//========================================================================
  $ItemMax[sniperxarmor, EnergyPack] = 1;
  $ItemMax[sniperxarmor, SensorJammerPack] = 1;
  $ItemMax[sniperxarmor, RepairKit] = 2;
//========================================================================
  $ItemMax[sniperxfemale, EnergyPack] = 1;
  $ItemMax[sniperxfemale, SensorJammerPack] = 1;
  $ItemMax[sniperxfemale, RepairKit] = 2;

//========================================================================
//========================================================================
  $MaxWeapons[sniperxarmor] = 3;
  $MaxWeapons[sniperxfemale] = 3;



  PlayerData sniperxarmor
  {
     className = "Armor";
     shapeFile = "larmor";
     damageSkinData = "armorDamageSkins";
  	debrisId = playerDebris;
     flameShapeName = "lflame";
     shieldShapeName = "shield";
     shadowDetailMask = 1;
     validateShape = false;
  
     visibleToSensor = True;
  	mapFilter = 1;
  	mapIcon = "M_player";
     canCrouch = true;
  
     maxJetSideForceFactor = 0.8;
     maxJetForwardVelocity = 22;
     minJetEnergy = 1;
     jetForce = 236;
     jetEnergyDrain = 1.0;
  
  	maxDamage = 0.7;
     maxForwardSpeed = 15; //== Not very impressive :(
     maxBackwardSpeed = 10;
     maxSideSpeed = 20; //== Snipers have great side speed cuz they snipe :)
     groundForce = 40 * 9.0;
     mass = 9.0;
     groundTraction = 3.0;
  	maxEnergy = 250;
     drag = 1.0;
     density = 1.2;
  
  	minDamageSpeed = 35;
  	damageScale = 0.005;
  
     jumpImpulse = 75;
     jumpSurfaceMinDot = 0.2;
  
     // animation data:
     // animation name, one shot, direction
  	// firstPerson, chaseCam, thirdPerson, signalThread
     // movement animations:
     animData[0]  = { "root", none, 1, true, true, true, false, 0 };
     animData[1]  = { "run", none, 1, true, false, true, false, 3 };
     animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
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
     animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
     animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
     animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
     animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
     animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
     animData[19] = { "jet", none, 1, true, true, true, false, 3 };
  
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
  
  
      // celebration animations:
     animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
     animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
     animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
   
      // taunt animations:
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
       SoundLFootRSoft,
       SoundLFootRHard,
       SoundLFootRSoft,
       SoundLFootRHard,
       SoundLFootRSoft,
       SoundLFootRSoft,
       SoundLFootRSoft,
       SoundLFootRHard,
       SoundLFootRSnow,
       SoundLFootRSoft,
       SoundLFootRSoft,
       SoundLFootRSoft,
       SoundLFootRSoft,
       SoundLFootRSoft,
       SoundLFootRSoft
    }; 
     lFootSounds =
     {
        SoundLFootLSoft,
        SoundLFootLHard,
        SoundLFootLSoft,
        SoundLFootLHard,
        SoundLFootLSoft,
        SoundLFootLSoft,
        SoundLFootLSoft,
        SoundLFootLHard,
        SoundLFootLSnow,
        SoundLFootLSoft,
        SoundLFootLSoft,
        SoundLFootLSoft,
        SoundLFootLSoft,
        SoundLFootLSoft,
        SoundLFootLSoft
     };
  
     footPrints = { 0, 1 };
  
     boxWidth = 0.5;
     boxDepth = 0.5;
     boxNormalHeight = 2.3;
     boxCrouchHeight = 1.8;
  
     boxNormalHeadPercentage  = 0.83;
     boxNormalTorsoPercentage = 0.53;
     boxCrouchHeadPercentage  = 0.6666;
     boxCrouchTorsoPercentage = 0.3333;
  
     boxHeadLeftPercentage  = 0;
     boxHeadRightPercentage = 1;
     boxHeadBackPercentage  = 0;
     boxHeadFrontPercentage = 1;
  };

  PlayerData sniperxfemale
  {
     className = "Armor";
     shapeFile = "lfemale";
     damageSkinData = "armorDamageSkins";
  	debrisId = playerDebris;
     flameShapeName = "lflame";
     shieldShapeName = "shield";
     shadowDetailMask = 1;
     validateShape = false;
  
     visibleToSensor = True;
  	mapFilter = 1;
  	mapIcon = "M_player";
     canCrouch = true;
  
     maxJetSideForceFactor = 0.8;
     maxJetForwardVelocity = 22;
     minJetEnergy = 1;
     jetForce = 236;
     jetEnergyDrain = 1.0;
  
  	maxDamage = 0.7;
     maxForwardSpeed = 18;
     maxBackwardSpeed = 15;
     maxSideSpeed = 8;
     groundForce = 40 * 9.0;
     mass = 9.0;
     groundTraction = 3.0;
  	maxEnergy = 250;
     drag = 1.0;
     density = 1.2;
  
  	minDamageSpeed = 35;
  	damageScale = 0.005;
  
     jumpImpulse = 75;
     jumpSurfaceMinDot = 0.2;
  
     // animation data:
     // animation name, one shot, direction
  	// firstPerson, chaseCam, thirdPerson, signalThread
     // movement animations:
     animData[0]  = { "root", none, 1, true, true, true, false, 0 };
     animData[1]  = { "run", none, 1, true, false, true, false, 3 };
     animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
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
     animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
     animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
     animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
     animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
     animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
     animData[19] = { "jet", none, 1, true, true, true, false, 3 };
  
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
  
  
      // celebration animations:
     animData[43] = { "celebration 1",none, 1, true, false, false, false, 2 };
     animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
     animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
   
      // taunt animations:
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
       SoundLFootRSoft,
       SoundLFootRHard,
       SoundLFootRSoft,
       SoundLFootRHard,
       SoundLFootRSoft,
       SoundLFootRSoft,
       SoundLFootRSoft,
       SoundLFootRHard,
       SoundLFootRSnow,
       SoundLFootRSoft,
       SoundLFootRSoft,
       SoundLFootRSoft,
       SoundLFootRSoft,
       SoundLFootRSoft,
       SoundLFootRSoft
    }; 
     lFootSounds =
     {
        SoundLFootLSoft,
        SoundLFootLHard,
        SoundLFootLSoft,
        SoundLFootLHard,
        SoundLFootLSoft,
        SoundLFootLSoft,
        SoundLFootLSoft,
        SoundLFootLHard,
        SoundLFootLSnow,
        SoundLFootLSoft,
        SoundLFootLSoft,
        SoundLFootLSoft,
        SoundLFootLSoft,
        SoundLFootLSoft,
        SoundLFootLSoft
     };
  
     footPrints = { 0, 1 };
  
     boxWidth = 0.5;
     boxDepth = 0.5;
     boxNormalHeight = 2.3;
     boxCrouchHeight = 1.8;
  
     boxNormalHeadPercentage  = 0.83;
     boxNormalTorsoPercentage = 0.53;
     boxCrouchHeadPercentage  = 0.6666;
     boxCrouchTorsoPercentage = 0.3333;
  
     boxHeadLeftPercentage  = 0;
     boxHeadRightPercentage = 1;
     boxHeadBackPercentage  = 0;
     boxHeadFrontPercentage = 1;
  };


$ArmorType[Male, SniperX] = sniperxarmor;
$ArmorType[Female, SniperX] = sniperxfemale;

$ArmorName[sniperxarmor] = SniperX;
$ArmorName[sniperxfemale] = SniperX;

ItemData SniperX
{
   heading = "aArmor";
	description = "SnipeX";
	className = "Armor";
	price = 200;
};

//== Wether it is able to be bought or not at inventory stations and remote inv.
$InvList[SniperX] = 1;
$RemoteInvList[SniperX] = 0;

//== Can the armor fly a vehicle?
$ArmorFly[sniperxarmor]  = "True";
$ArmorFly[sniperxfemale] = "True";