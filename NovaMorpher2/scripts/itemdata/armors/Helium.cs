//== This armor is just plain for a punishment :-P

  $MaxWeapons[heliumarmor] = 0;

  PlayerData heliumarmor
  {
     className = "Armor";
     shapeFile = "harmor";
     flameShapeName = "hflame";
     shieldShapeName = "shield";
     damageSkinData = "armorDamageSkins";
  	debrisId = playerDebris;
     shadowDetailMask = 1;
     validateShape = false;
  
     canCrouch = false;
     visibleToSensor = True;
  	mapFilter = 1;
  	mapIcon = "M_player";
  
     maxJetSideForceFactor = 0;
     maxJetForwardVelocity = 0;
     minJetEnergy = 0;
     jetForce = 0;
     jetEnergyDrain = 2.0;
  
  	maxDamage = 100000.0;
     maxForwardSpeed = 0.01;
     maxBackwardSpeed = 0.01;
     maxSideSpeed = 0.01;
     groundForce = 35 * 13.0;
     mass = 0.005;
     groundTraction = 0;
  	
  	maxEnergy = 1;
     drag = 0.005;
     density = 0.005;
  
  	minDamageSpeed = 60;
  	damageScale = 0.005;
  
     jumpImpulse = 0.0001;
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