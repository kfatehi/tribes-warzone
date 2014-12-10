//---------------------------------------------------------------------------------
// Player death messages - %1 = killer's name, %2 = victim's name
//       %3 = killer's gender pronoun (his/her), %4 = victim's gender pronoun
//---------------------------------------------------------------------------------

$deathMsg[$SniperDamageType, 0]      = "%2 took the wrong turn into Sniper Street.";
$deathMsg[$SniperDamageType, 1]      = "%1 shoves a bullet up %2's @$$.";
$deathMsg[$SniperDamageType, 2]      = "%2 stayed in %1's crosshairs for too long.";
$deathMsg[$SniperDamageType, 3]      = "%2 discovered how death really felt... With a bullet that is...";
$deathMsg[$SniperDamageType, 4]      = "%2 choose the bullet instead of the mortar...";
//--------------------------------------
BulletData SniperBullet
{
   bulletShapeName    = "bullet.dts";
   validateShape      = false;
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 1;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.55;
   damageType         = $SniperDamageType;

   aimDeflection      = 0;
   muzzleVelocity     = 4000.0; // Holy Shit.....
   totalTime          = 2;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 1.0;
   tracerLength       = 30;
};
