//== This armor is going to be a "spawn ONLY" armor which means that you can only
//== get it through spawning, It CANNOT carry flags :-) It is invincible :-P
//== It carries a SUPER SUPER Repair gun :-) which repairs things 5x faster then normal
//-- Repair Guns. It runs 2x as fast as a heavy, jumps 2x as high, and weighs 3/4 as a heavy...


  //----------------------------------------------------------------------------
  // SpecialRepair Armor
  //----------------------------------------------------------------------------
  $DamageScale[srarmor, $LandingDamageType] = 0;
  $DamageScale[srarmor, $ImpactDamageType] = 0;
  $DamageScale[srarmor, $CrushDamageType] = 0;
  $DamageScale[srarmor, $BulletDamageType] = 0;
  $DamageScale[srarmor, $PlasmaDamageType] = 0;
  $DamageScale[srarmor, $EnergyDamageType] = 0;
  $DamageScale[srarmor, $ExplosionDamageType] = 0;
  $DamageScale[srarmor, $MissileDamageType] = 0;
  $DamageScale[srarmor, $ShrapnelDamageType] = 0;
  $DamageScale[srarmor, $DebrisDamageType] = 0;
  $DamageScale[srarmor, $LaserDamageType] = 0;
  $DamageScale[srarmor, $MortarDamageType] = 0;
  $DamageScale[srarmor, $BlasterDamageType] = 0;
  $DamageScale[srarmor, $ElectricityDamageType] = 0;
  $DamageScale[srarmor, $MineDamageType] = 0;

  $ItemMax[srarmor, UltraRepairPack] = 1;

  $MaxWeapons[srarmor] = 0;



  PlayerData srarmor
  {
     className = "Armor";
     shapeFile = "harmor";
     //flameShapeName = "hflame";
     flameShapeName = "tumult_medium";
     shieldShapeName = "shield";
     damageSkinData = "armorDamageSkins";
  	debrisId = playerDebris;
     shadowDetailMask = 1;
     validateShape = false;
  
     visibleToSensor = True;
  	mapFilter = 1;
  	mapIcon = "M_player";
  
     maxJetSideForceFactor = 0.8;
     maxJetForwardVelocity = 12;
     minJetEnergy = 1;
     jetForce = 385;
     jetEnergyDrain = 1.1;
  
  	maxDamage = 1000000;
     maxForwardSpeed = 10.0;
     maxBackwardSpeed = 8.0;
     maxSideSpeed = 8.0;
     groundForce = 35 * 18.0;
     groundTraction = 4.5;
     mass = 13.5;
  	maxEnergy = 220;
     drag = 1.0;
     density = 2.5;
     canCrouch = false;
  
  	minDamageSpeed = 25;
  	damageScale = 0.006;
  
     jumpImpulse = 300;
     jumpSurfaceMinDot = 0.2;
  
     // animation data:
     // animation name, one shot, exclude, direction
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
  
     jetSound = debrisLargeExplosion;
  
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

$ArmorType[Male, SpecialRepair] = srarmor;
$ArmorType[Female, SpecialRepair] = srarmor;

$ArmorName[srarmor] = SpecialRepair;

ItemData SpecialRepair
{
   heading = "aArmor";
	description = "Medic";
	className = "Armor";
	price = 999;
};

//== Wether it is able to be bought or not at inventory stations and remote inv.
$InvList[SpecialRepair] = 0;
$RemoteInvList[SpecialRepair] = 0;


$Armor::SpecialSmoke[srarmor] = true;

$Armor::SmokeType[srarmor] = "srFlyFlame";

ExplosionData nullExp
{
   shapeName = "breath.dts";

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

GrenadeData srFlyFlame
{
   bulletShapeName    = "tumult_medium.dts";
   explosionTag       = nullExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;

   damageClass        = 1;
   damageValue        = 0;
   damageType         = $nullDamageType;

   explosionRadius    = 0;
   kickBackStrength   = 0;
   maxLevelFlightDist = 0;
   totalTime          = 0.01;    // special meaning for grenades...
   liveTime           = 0.01;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "tumult_medium.dts";
};


$Armor::BuySpecial[srarmor] = true; //== Makes it a special armor ;)
function BuySpecial::srarmor(%client)
{
	if(!%client.spawn)
	{
		centerprint(%client,"<jc><f0>CHEATERS MUST DIE!!!!\n"@
						"<f1>EXPLOITERS MUST DIE!!!!\n"@
						"<f2>YOU MUST DIE!!!!!!!!!!!\n",30);
		remoteKill(%client);
		schedule("remoteKill("@%client@");",10,%client);
		schedule("centerprint("@%client@", '<jc><f0>This <f1>Is <f2>A <f1>Reminder <f0>NOT <f1>To <f2>Cheat <f1>OR <f0>EXPLOIT!', 10);", 8, %client);
	}
}