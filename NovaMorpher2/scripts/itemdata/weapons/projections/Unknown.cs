//== Man! This damage is amazing! So is its returning speed!

RocketData UnknownBolt
{
   bulletShapeName = "dustplume.dts";
   explosionTag    = plasmaExp;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.1;
   damageType       = $PlasmaDamageType;

   explosionRadius  = 100.0;
   kickBackStrength = -600;

   muzzleVelocity   = 32.5;
   terminalVelocity = 40.0;

   totalTime        = 6.5;
   liveTime         = 8.0;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 1;
   soundId = SoundJetHeavy;

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "dustplume.dts";
   smokeDist   = 0.1; //== Hell long tail :-P
};