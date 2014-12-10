LightningData MorphControlBolt
{
   bitmapName       = "warp.bmp";

   damageType       = $MorphControlType;
   boltLength       = 25.0;
   coneAngle        = 35.0;
   damagePerSec      = 0.001;
   energyDrainPerSec = 0.001;
   segmentDivisions = 4;
   numSegments      = 5;
   beamWidth        = 0.125;

   updateTime   = 120;
   skipPercent  = 0.5;
   displaceBias = 0.15;

   lightRange = 3.0;
   lightColor = { 0.25, 0.25, 0.85 };

   soundId = SoundELFFire;
};

function MorphControlBolt::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId)
{
	Player::trigger(%player,$WeaponSlot,false);
	MorphControl::DoMorph(Client::getOwnedObject(%shooterId), %target);
}