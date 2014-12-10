//== Watch out for the great big smoke coming towards ya!

RocketData WindBolt
{
   bulletShapeName  = "breath.dts";
   explosionTag     = Shockwave;
   collisionRadius  = 0.0;
   mass             = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0;
   damageType       = $NullDamageType;

   explosionRadius  = 40;
   kickBackStrength = 60.0;
   muzzleVelocity   = 1000.0;
   terminalVelocity = 1400.0;
   totalTime        = 20.0;
   liveTime         = 21.0;
   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };
   inheritedVelocityScale = 5;

   soundId = SoundWindGust;
};
