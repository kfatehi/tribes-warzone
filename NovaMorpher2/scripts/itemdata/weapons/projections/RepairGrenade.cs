GrenadeData RepairGrenadeShell
{
   bulletShapeName    = "plasmaex.dts";
   explosionTag       = plasmaExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 3.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.01;
   damageType         = $HealDamageType;

   explosionRadius    = 20;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 9999;
   totalTime          = 60.0;    // special meaning for grenades...
   liveTime           = 3.0;
   projSpecialTime    = 0.05;

   inheritedVelocityScale = 0.5;

   smokeName              = "shotgunex.dts";
};