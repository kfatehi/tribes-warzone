//== A low health powerful armor, max damage of 0.5, it can withstand great speed


  //----------------------------------------------------------------------------
  // mage Armor
  //----------------------------------------------------------------------------
//========================================================================
//========================================================================
  $DamageScale[magearmor, $EnergyDamageType] = -0.1;      //== .....
  $DamageScale[magearmor, $LaserDamageType] = 99999999.0; //== Dont even think about it
  $DamageScale[magearmor, $ElectricityDamageType] = -0.1; //== Hey! This armor is a energy proof!
//========================================================================
  $DamageScale[magefemale, $EnergyDamageType] = -0.1;      //== .....
  $DamageScale[magefemale, $LaserDamageType] = 99999999.0; //== Dont even think about it
  $DamageScale[magefemale, $ElectricityDamageType] = -0.1; //== Hey! This armor is a energy proof!

//========================================================================
// No normal weapons for mage! It is all new! I hope... :'(
//========================================================================
  $ItemMax[magearmor, Mineammo] = 1;
  $ItemMax[magearmor, Mana] = 75;
//========================================================================
  $ItemMax[magefemale, Mineammo] = 1;
  $ItemMax[magefemale, Mana] = 75;
  
//========================================================================
//========================================================================
//== The mages SHOULD be all energy based, probably some mana based (self recharging ammo)
//========================================================================
//== The mages SHOULD be all energy based, probably some mana based (self recharging ammo)
  
//========================================================================
//========================================================================
  $ItemMax[magearmor, EnergyPack] = 1;
  $ItemMax[magearmor, ShieldPack] = 1;
  $ItemMax[magearmor, SensorJammerPack] = 1;
  $ItemMax[magearmor, RepairKit] = 5;
//========================================================================
  $ItemMax[magefemale, EnergyPack] = 1;
  $ItemMax[magefemale, ShieldPack] = 1;
  $ItemMax[magefemale, SensorJammerPack] = 1;
  $ItemMax[magefemale, RepairKit] = 1;

//========================================================================
//========================================================================
  $MaxWeapons[magearmor] = 3;
  $MaxWeapons[magefemale] = 3;



  PlayerData magearmor
  {
     className = "Armor";
     shapeFile = "larmor";
     damageSkinData = "armorDamageSkins";
  	debrisId = playerDebris;
     flameShapeName = "plasmatrail";
     shieldShapeName = "shield";
     shadowDetailMask = 1;
     validateShape = false;
  
     visibleToSensor = True;
  	mapFilter = 1;
  	mapIcon = "M_player";
     canCrouch = true;
  
     maxJetSideForceFactor = 1.6;
     maxJetForwardVelocity = 44;
     minJetEnergy = 1.5;
     jetForce = 137;
     jetEnergyDrain = 0.9; //== The only (so far) long lasting non-infinite flight armor
  
  	maxDamage = 0.5;
     maxForwardSpeed = 26;
     maxBackwardSpeed = 20;
     maxSideSpeed = 20;
     groundForce = 40 * 9.0;
     mass = 6.0; //== This armor supposivly HAS no armor
     groundTraction = 3.0;
  	maxEnergy = 250;
     drag = 0.85;
     density = 1.2;
  
  	minDamageSpeed = 40;
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

  PlayerData magefemale
  {
     className = "Armor";
     shapeFile = "lfemale";
     damageSkinData = "armorDamageSkins";
  	debrisId = playerDebris;
     flameShapeName = "plasmatrail";
     shieldShapeName = "shield";
     shadowDetailMask = 1;
     validateShape = false;
  
     visibleToSensor = True;
  	mapFilter = 1;
  	mapIcon = "M_player";
     canCrouch = true;
  
     maxJetSideForceFactor = 1.6;
     maxJetForwardVelocity = 44;
     minJetEnergy = 1.5;
     jetForce = 137;
     jetEnergyDrain = 0.9;
  
  	maxDamage = 0.5;
     maxForwardSpeed = 26;
     maxBackwardSpeed = 20;
     maxSideSpeed = 20;
     groundForce = 40 * 9.0;
     mass = 6.0;
     groundTraction = 3.0;
  	maxEnergy = 250;
     drag = 0.85;
     density = 1.2;
  
  	minDamageSpeed = 40;
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



$ArmorType[Male, Mage] = magearmor;
$ArmorType[Female, Mage] = magefemale;

$ArmorName[magearmor] = Mage;
$ArmorName[magefemale] = Mage;

ItemData Mage
{
	heading = "aArmor";
	description = "Mage (Magical)";
	className = "Armor";
	price = 200;
};

//== Wether it is able to be bought or not at inventory stations and remote inv.
$InvList[mage] = 1;
$RemoteInvList[mage] = 1; //== This armor is like... magical, so remote inventory stations can summon them

//== Can the armor fly a vehicle?
$ArmorFly[magearmor]  = "True";
$ArmorFly[magefemale] = "True";

$Armor::SpecialSmoke[magearmor] = true;
$Armor::SpecialSmoke[magefemale] = true;

$Armor::SmokeType[magearmor] = "mageFlyFlame";
$Armor::SmokeType[magefemale] = "mageFlyFlame";

ExplosionData mageFlyFlameExp
{
   shapeName = "plasmatrail.dts";

   faceCamera = false;
   randomSpin = false;
   hasLight   = false;
   lightRange = 3.0;

   timeZero = 0.450;
   timeOne  = 0.750;

   colors[0]  = { 0.25, 0.25, 1.0 };
   colors[1]  = { 0.25, 0.25, 1.0 };
   colors[2]  = { 1.0, 1.0,  1.0 };
   radFactors = { 1.0, 1.0,  1.0 };

   shiftPosition = true;
};

GrenadeData mageFlyFlame
{
   bulletShapeName    = "plasmatrail.dts";
   explosionTag       = mageFlyFlameExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;
   damageValue        = 0;
   damageType         = false;

   explosionRadius    = 0;
   kickBackStrength   = 0;
   maxLevelFlightDist = 0;
   totalTime          = 0.01;    // special meaning for grenades...
   liveTime           = 0.01;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "plasmatrail.dts";
};















//== Undead armor for undaed spell

  PlayerData undeadmagearmor
  {
     className = "Armor";
     shapeFile = "larmor";
     damageSkinData = "armorDamageSkins";
  	debrisId = playerDebris;
     flameShapeName = "plasmatrail";
     shieldShapeName = "shield";
     shadowDetailMask = 1;
     validateShape = false;
  
     visibleToSensor = True;
  	mapFilter = 1;
  	mapIcon = "M_player";
     canCrouch = true;
  
     maxJetSideForceFactor = 1.6;
     maxJetForwardVelocity = 44;
     minJetEnergy = 1.5;
     jetForce = 137;
     jetEnergyDrain = 0.09; //== The only (so far) long lasting non-infinite flight armor
  
  	maxDamage = 0.0001; //== HAH!
     maxForwardSpeed = 26;
     maxBackwardSpeed = 20;
     maxSideSpeed = 20;
     groundForce = 40 * 9.0;
     mass = 6.0; //== This armor supposivly HAS no armor
     groundTraction = 3.0;
  	maxEnergy = 25.0;
     drag = 0.85;
     density = 1.2;
  
  	minDamageSpeed = 40;
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
  
     boxWidth = 0;


     boxDepth = 0;
     boxNormalHeight = 0.5;
     boxCrouchHeight = 0.1;

     boxNormalHeadPercentage  = 0.83;
     boxNormalTorsoPercentage = 0.53;
     boxCrouchHeadPercentage  = 0.6666;
     boxCrouchTorsoPercentage = 0.3333;
  
     boxHeadLeftPercentage  = 0;
     boxHeadRightPercentage = 1;
     boxHeadBackPercentage  = 0;
     boxHeadFrontPercentage = 1;

  };

  PlayerData undeadmagefemale
  {
     className = "Armor";
     shapeFile = "lfemale";
     damageSkinData = "armorDamageSkins";
  	debrisId = playerDebris;
     flameShapeName = "plasmatrail";
     shieldShapeName = "shield";
     shadowDetailMask = 1;
     validateShape = false;
  
     visibleToSensor = True;
  	mapFilter = 1;
  	mapIcon = "M_player";
     canCrouch = true;
  
     maxJetSideForceFactor = 1.6;
     maxJetForwardVelocity = 44;
     minJetEnergy = 1.5;
     jetForce = 137;
     jetEnergyDrain = 0.09; //== 0.678
  
  	maxDamage = 0.0001;
     maxForwardSpeed = 26;
     maxBackwardSpeed = 20;
     maxSideSpeed = 20;
     groundForce = 40 * 9.0;
     mass = 6.0;
     groundTraction = 3.0;
  	maxEnergy = 25.0;
     drag = 0.85;
     density = 1.2;
  
  	minDamageSpeed = 40;
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
  
     boxWidth = 0;
     boxDepth = 0;
     boxNormalHeight = 0.5;
     boxCrouchHeight = 0.1;
  
     boxNormalHeadPercentage  = 0.83;
     boxNormalTorsoPercentage = 0.53;
     boxCrouchHeadPercentage  = 0.6666;
     boxCrouchTorsoPercentage = 0.3333;
  
     boxHeadLeftPercentage  = 0;
     boxHeadRightPercentage = 1;
     boxHeadBackPercentage  = 0;
     boxHeadFrontPercentage = 1;
  };


$ArmorType[Male, UndeadMage] = undeadmagearmor;
$ArmorType[Female, UndeadMage] = undeadmagefemale;

$ArmorName[undeadmagearmor] = UndeadMage;
$ArmorName[undeadmagefemale] = UndeadMage;

ItemData UndeadMage
{
	heading = "aArmor";
	description = "Mage (Dead)";
	className = "Armor";
	price = 200;
};

//== Wether it is able to be bought or not at inventory stations and remote inv.
$InvList[UndeadMage] = 0;
$RemoteInvList[UndeadMage] = 0;

//== Can the armor fly a vehicle?
$ArmorFly[undeadmagearmor]  = false;
$ArmorFly[undeadmagefemale] = false;

$Armor::SpecialSmoke[undeadmagearmor] = "NoSmoke";
$Armor::SpecialSmoke[undeadmagefemale] = "NoSmoke";