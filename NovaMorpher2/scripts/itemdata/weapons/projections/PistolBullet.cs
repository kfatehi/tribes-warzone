//---------------------------------------------------------------------------------
// Player death messages - %1 = killer's name, %2 = victim's name
//       %3 = killer's gender pronoun (his/her), %4 = victim's gender pronoun
//---------------------------------------------------------------------------------

$deathMsg[$PistolDamageType, 0]      = "%2 was known as the slowest dueler in the west....";
$deathMsg[$PistolDamageType, 1]      = "%2 felt like god and started dueling %1...";
$deathMsg[$PistolDamageType, 2]      = "%2 found a bullet in %4 chest...";
$deathMsg[$PistolDamageType, 3]      = "%2 discovered how death really felt... With a bullet that is...";
$deathMsg[$PistolDamageType, 4]      = "%2 danced with a bullet, but the bullet steped on %4's heart...";
//--------------------------------------
BulletData PistolBullet
{
   bulletShapeName    = "bullet.dts";
   validateShape      = false;
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 1;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.65;
   damageType         = $PistolDamageType;

   aimDeflection      = 0;
   muzzleVelocity     = 100.0; // Holy Shit.....
   totalTime          = 0.28125;
   inheritedVelocityScale = 2.0;
   isVisible          = true;

   tracerPercentage   = 5.0;
   tracerLength       = 150;
};
