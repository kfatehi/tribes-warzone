  //------------------------------------------------------------------
  // Heavy Armor data:
  //------------------------------------------------------------------
  
  PlayerData harmor
  {
     className = "Armor";
     shapeFile = "harmor";
     flameShapeName = "hflame";
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
     jetForce = 595;
     jetEnergyDrain = 1.5;
  
  	maxDamage = 3.5;
     maxForwardSpeed = 5.5;
     maxBackwardSpeed = 5.5;
     maxSideSpeed = 4.0;
     groundForce = 35 * 18.0;
     groundTraction = 4.5;
     mass = 19.0;
  	maxEnergy = 110;
     drag = 1.0;
     density = 2.5;
     canCrouch = false;
  
  	minDamageSpeed = 25;
  	damageScale = 0.006;
  
     jumpImpulse = 150;
     jumpSurfaceMinDot = 0.2;
  
     // animation data:
     // animation name, one shot, exclude, direction,
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
  
     jetSound = SoundJetHeavy;
  
     rFootSounds = 
     {
       SoundHFootRSoft,
       SoundHFootRHard,
       SoundHFootRSoft,
       SoundHFootRHard,
       SoundHFootRSoft,
       SoundHFootRSoft,
       SoundHFootRSoft,
       SoundHFootRHard,
       SoundHFootRSnow,
       SoundHFootRSoft,
       SoundHFootRSoft,
       SoundHFootRSoft,
       SoundHFootRSoft,
       SoundHFootRSoft,
       SoundHFootRSoft
    }; 
     lFootSounds =
     {
        SoundHFootLSoft,
        SoundHFootLHard,
        SoundHFootLSoft,
        SoundHFootLHard,
        SoundHFootLSoft,
        SoundHFootLSoft,
        SoundHFootLSoft,
        SoundHFootLHard,
        SoundHFootLSnow,
        SoundHFootLSoft,
        SoundHFootLSoft,
        SoundHFootLSoft,
        SoundHFootLSoft,
        SoundHFootLSoft,
        SoundHFootLSoft
     };
  
     footPrints = { 4, 5 };
  
     boxWidth = 0.8;
     boxDepth = 0.8;
     boxNormalHeight = 2.6;
  
     boxNormalHeadPercentage  = 0.70;
     boxNormalTorsoPercentage = 0.45;
  
     boxHeadLeftPercentage  = 0.48;
     boxHeadRightPercentage = 0.70;
     boxHeadBackPercentage  = 0.48;
     boxHeadFrontPercentage = 0.60;
  };

$ArmorSpecialSlotA = 6; //== Note* This may overlap some weapons using
$ArmorSpecialSlotB = 7; //== Note* $ExtraWeaponSlot C and D so be careful!

$Armor::BuySpecial[harmor] = true; //== Makes it a special armor ;)
function BuySpecial::harmor(%client)
{
	Player::setItemCount(%client, CannonP1Pack,1);
	Player::mountItem(%client, CannonP1Pack, 6);
	Player::setItemCount(%client, CannonP2Pack,1);
	Player::mountItem(%client, CannonP2Pack, 7);

	%player.hasArmorCannons = true;
}

function SellSpecial::harmor(%client)
{
	%player = Client::getOwnedObject(%client);
	if(Player::getMountedItem(%player, 6) == CannonP1Pack)
	{
		Player::unmountItem(%player, 6);
		Player::setItemCount(%player, CannonP1Pack,0);
	}
	if(Player::getMountedItem(%player, 7) == CannonP2Pack)
	{
		Player::unmountItem(%player, 7);
		Player::setItemCount(%player, CannonP2Pack,0);
	}

	%player.hasArmorCannons = false;
}

//=================================================//
//=======---------------NOTE---------------========//
//=================================================//
//== All credits toward this special enhancement ==//
//== is given to the HaVoC modder ;)             ==//
//=================================================//

ExplosionData nappyExp
{
   shapeName = "tumult_large.dts";
   soundId   = debrisLargeExp1osion;

   faceCamera = true;
   randomSpin = true;
   hasLight   = true;
   lightRange = 35.0;

   timeScale = 9;

   timeZero = 0.200;
   timeOne  = 0.950;

   colors[0]  = { 1.0, 1.0,  0.0 };
   colors[1]  = { 1.0, 0.4, 0.0 };
   colors[2]  = { 1.0, 0.0, 0.0 };
   radFactors = { 0.8, 0.9, 0.5 };
};

RocketData PimpinMissile 
{ 
	bulletShapeName = "rocket.dts"; 
	explosionTag = nappyExp; 
	collisionRadius = 0.0; 
	mass = 2.0; 
	damageClass = 1; 
	damageValue = 0.75; 
	damageType = $ExplosionDamageType; 
	explosionRadius = 20; 
	kickBackStrength = 200.0; 
	muzzleVelocity = 30.0; 
	terminalVelocity = 42.5; 
	acceleration = 2.5; 
	totalTime = 60.0; 
	liveTime = 60.1; 
	lightRange = 10.0; 
	lightColor = { 1.0, 0.0, 0.0 }; 
	inheritedVelocityScale = 0.5; 
	trailType = 2; 
	trailString = "plasmatrail.dts"; 
	smokeDist = 1.8; 
	soundId = SoundJetHeavy; 
}; 

ItemImageData CannonP1PackImage 
{
	shapeFile = "mortargun"; 
	mountPoint = 3; 
	mountOffset = { -0.27, 0.1, 2.175 }; 
	mountRotation = { 0, -1.57, 0 }; 
	mass = 4;

	weaponType = 0; 
	projectileType = PimpinMissile; 
	accuFire = true; 
	reloadTime = 3.0; 
	fireTime = 0.0; 
	lightType = 3; 
	lightRadius = 5; 
	lightTime = 2; 
	lightColor = { 1, 1, 1.0 }; 
	sfxFire = explosion3; 
}; 

function CannonP1PackImage::onActivate(%player,%imageSlot)
{
	%player.CannonFireing = true;
} 

function CannonP1PackImage::onDeactivate(%player,%imageSlot) 
{ 
	schedule(%player@".CannonFireing = false;",1);
	Player::trigger(%player,%imageSlot,false); 
} 

ItemData CannonP1Pack
{
	description = "Cannon"; 
	shapeFile = "mortargun"; 	
	heading = "cBackpacks"; 
	shadowDetailMask = 4; 
	imageType = CannonP1PackImage; 
	mass = 4;

	price = 350; 
	hudIcon = "mortar"; 
	showWeaponBar = false; 
	hiliteOnActive = true; 
	showInventory = false; 
}; 

ItemImageData CannonP2PackImage 
{ 
	shapeFile = "mortargun"; 
	mountPoint = 3; 
	mountOffset = { 0.46, 0.1, 2.175 }; 
	mountRotation = { 0, 1.57, 0 }; 
	mass = 4;

	weaponType = 0; 
	projectileType = PimpinMissile; 
	accuFire = true; 
	reloadTime = 3.0; 
	fireTime = 0.0; 
	lightType = 3; 
	lightRadius = 5; 
	lightTime = 2; 
	lightColor = { 1.0, 1, 1.0 }; 
	sfxFire = explosion3; 
}; 

function CannonP2PackImage::onActivate(%player,%imageSlot)
{

} 

function CannonP2PackImage::onDeactivate(%player,%imageSlot) 
{ 
	Player::trigger(%player,%imageSlot,false); 
} 

ItemData CannonP2Pack 
{ 
	description = "Cannon"; 
	shapeFile = "mortargun"; 
	heading = "cBackpacks"; 
	shadowDetailMask = 4; 
	imageType = CannonP2PackImage; 
	mass = 4;

	price = 350; 
	hudIcon = "mortar"; 
	showWeaponBar = false; 
	hiliteOnActive = true; 
	showInventory = false; 
}; 

