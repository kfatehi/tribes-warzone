//== A rather straight moving ball of fire....

RocketData FireBallMain
{
   bulletShapeName = "plasmaex.dts";
   explosionTag    = plasmaExp;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
//   damageValue      = 0.6;
   damageValue      = 0.4;
   damageType       = $FireDamageType;

   explosionRadius  = 10.0;
   kickBackStrength = 20.0;

   muzzleVelocity   = 260.0;
   terminalVelocity = 320.0;
   acceleration     = 6.0;

   totalTime        = 6.5;
   liveTime         = 8.0;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   soundId = SoundJetHeavy;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "fiery.dts";
   smokeDist   = 0.5; //== Hell long tail :-P
};


RocketData FireBallTrail
{
   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.001;
   damageType       = $NoBurnFireDamageType;

   explosionRadius  = 10.0;
   kickBackStrength = 20.0;

   muzzleVelocity   = 260.0;
   terminalVelocity = 320.0;
   acceleration     = 6.0;

   totalTime        = 6.5;
   liveTime         = 8.0;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "plasmaex.dts";
   smokeDist   = 0.5; //== Hell long tail :-P

   soundId = SoundJetHeavy;
};


