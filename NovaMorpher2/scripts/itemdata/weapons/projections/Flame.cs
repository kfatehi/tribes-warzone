//---------------------------------------------------------------------------------
// Player death messages - %1 = killer's name, %2 = victim's name
//       %3 = killer's gender pronoun (his/her), %4 = victim's gender pronoun
//---------------------------------------------------------------------------------

$deathMsg[$FireDamageType, 0]      = "%2 went swimming in the volcano...";
$deathMsg[$FireDamageType, 1]      = "%1 will serve roast %2 tonight.";
$deathMsg[$FireDamageType, 2]      = "%2 had an overdose of DEHYDRATION pills.";
$deathMsg[$FireDamageType, 3]      = "%2 started to play with fire...";
$deathMsg[$FireDamageType, 4]      = "%1 said,\"Burn! Baby, Burn!\" to %2";

$deathMsg[$NoBurnFireDamageType, 0]      = "%2 went swimming in the volcano...";
$deathMsg[$NoBurnFireDamageType, 1]      = "%1 will serve roast %2 tonight.";
$deathMsg[$NoBurnFireDamageType, 2]      = "%2 had an overdose of DEHYDRATION pills.";
$deathMsg[$NoBurnFireDamageType, 3]      = "%2 started to play with fire...";
$deathMsg[$NoBurnFireDamageType, 4]      = "%1 said,\"Burn! Baby, Burn!\" to %2";

//---------------------------------------------------------------------------------

GrenadeData Flame
{
   bulletShapeName    = "fiery.dts";
   explosionTag       = plasmaExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 0.001;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.01;
   damageType         = $FireDamageType;

   aimDeflection      = 0.01;

   explosionRadius    = 10.0;
   kickBackStrength   = 0.0;
   maxLevelFlightDist = 175;
   totalTime          = 0.5;
   liveTime           = 0.001;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0.5;
   smokeName              = "plasmaex.dts";
};