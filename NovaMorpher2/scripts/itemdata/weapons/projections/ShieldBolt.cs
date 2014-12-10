//== A rather straight moving ball of fire....
$ShieldDamageType = DamageTypes::AddReserve();

BulletData ShieldBolt
{
   bulletShapeName    = "shield.dts";
   validateShape      = false;
   explosionTag       = blasterExp;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.0001;
   damageType         = $ShieldDamageType;

   muzzleVelocity     = 650.0;
   totalTime          = 2;
   inheritedVelocityScale = 1.0;
   isVisible          = True;

   tracerPercentage   = 1.0;
   tracerLength       = 30;
};

$SpecialDamageType[$ShieldDamageType] = "ShieldDamageType";

function ShieldDamageType::DoSpecialDMG(%damagedPlayer, %shooterPlayer)
{
	%shooterClient = Player::getClient(%shooterPlayer);
	%damagedClient = Player::getClient(%damagedPlayer);
	%damagedPlTeam = Client::getTeam(%damagedClient);
	%shooterClTeam = Client::getTeam(%shooterClient);

	%damagedPlayer.holyShield = true;
	%damagedPlayer.holyShield.strength = 0.01;
	%damagedPlayer.holyShield.caster = %shooterPlayer;

	client::sendMessage(%damagedClient, 1, "You have enhanced with the shield spell by "@Client::getName(%shooterClient)@"!");
	client::sendMessage(%shooterClient, 1, "You have enhanced "@Client::getName(%damagedClient)@" with a shield spell that is not to be broken till death!");

	if($debug)
	{
		echo("%damagePlayer: "@%damagePlayer);
		echo("%shooterPlayer: "@%shooterPlayer);
		echo("%shooterClient: "@%shooterClient);
		echo("%damagedClient: "@%damagedClient);
	}
}

function ShieldDamageType::RemoveShield(%player)
{
	%clientId = Player::getClient(%player);

	%player.holyShield = false;
	%player.holyShield.strength = 0;
	%player.holyShield.caster = "";

	client::sendMessage(%damagedClient, 1, "You have shield's source been eliminated!");
}