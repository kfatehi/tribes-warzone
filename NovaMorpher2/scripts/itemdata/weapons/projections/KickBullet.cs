//---------------------------------------------------------------------------------
// Player death messages - %1 = killer's name, %2 = victim's name
//       %3 = killer's gender pronoun (his/her), %4 = victim's gender pronoun
//---------------------------------------------------------------------------------

$deathMsg[$KickDamageType, 0]      = "%2 was playing a game with %1 called, \"KICK ME!!!\"";
$deathMsg[$KickDamageType, 1]      = "%1 shoves a bullet up %2's @$$ and so,  %4 got kicked.";
$deathMsg[$KickDamageType, 2]      = "%2 registered in %1's kicklist...";
$deathMsg[$KickDamageType, 3]      = "%2 discovered how being kicked really felt... With a bullet...";
$deathMsg[$KickDamageType, 4]      = "%2's head got discarded when kicked by %1...";
//--------------------------------------
//== HAHAHA!!! This is gonna be a funny show!
SeekingMissileData KickBullet
{
	bulletShapeName = "plasmaex.dts";
	explosionTag = mortarExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 0;       // 0 impact, 1, radius
	damageValue = 0.5;
	damageType = $KickDamageType;
	kickBackStrength = 0.0;
	muzzleVelocity = 50.0;
	terminalVelocity = 500.0;
	acceleration = 100.0;
	totalTime = 1000.0;
	liveTime = 1000.0;
	lightRange = 5.0;
	lightColor = { 1.0, 0.7, 0.5 };
	inheritedVelocityScale = 0.5;
	seekingTurningRadius = 3.6;
	nonSeekingTurningRadius = 3.6;
	proximityDist = 0.5;
	lightRange = 5.0;
	lightColor = { 0.4, 0.4, 1.0 };
	smokeDist = 4.5;
	inheritedVelocityScale = 0.5;
	soundId = SoundJetHeavy;
};
function KickBullet::updateTargetPercentage(%target)
{
	returb true;
}