//== A rather straight moving ball of fire....
$ParasiteDamageType = DamageTypes::AddReserve();

BulletData ParasiteBug
{
   bulletShapeName    = "enbolt.dts";
   validateShape      = false;
   explosionTag       = blasterExp;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.0001;
   damageType         = $ParasiteDamageType;

   muzzleVelocity     = 650.0;
   totalTime          = 2;
   inheritedVelocityScale = 1.0;
   isVisible          = True;

   tracerPercentage   = 1.0;
   tracerLength       = 30;

   trailType   = 2;                // smoke trail
   trailString = "mortar.dts";
   smokeDist   = 0.5;
};

$SpecialDamageType[$ParasiteDamageType] = "ParasiteDamageType";

function ParasiteDamageType::DoSpecialDMG(%damagedPlayer, %shooterPlayer)
{
	%shooterClient = Player::getClient(%shooterPlayer);
	%damagedClient = Player::getClient(%damagedPlayer);
	%damagedPlTeam = Client::getTeam(%damagedClient);
	%shooterClTeam = Client::getTeam(%shooterClient);
	if(%shooterClTeam != %damagedPlTeam)
	{
		$isParasiteSlave[%damagedPlayer] = true;
		$slaveMaster[%damagedClient] = %shooterClient;
		$MastersSlave[%shooterClient] = %damagedClient;

		//== Ouch, how evil ;)
		$ParasiteSlave::OldTeam[%damagedClient] = %damagedPlTeam;
		GameBase::setTeam(%damagedClient, %shooterClTeam);
		client::sendMessage(%damagedClient, 1, "YOU HAVE BEEN ENSLAVED BY A PARASITE!");
		client::sendMessage(%shooterClient, 1, Client::getName(%shooterClient)@" HAVE BEEN ENSLAVED BY A YOU!!!");
	}
}

function ParasiteDamageType::RemoveParasite(%clientId, %dump)
{
	%player = Client::getOwnedObject(%clientId);

	//== Aquire needed varibles
	%masterClient = $slaveMaster[%clientId];
	%oldTeam = $ParasiteSlave::OldTeam[%clientId];

	//== Clear global varibles
	$isParasiteSlave[%player] = "";
	$isParasiteSlave[%clientId] = "";

	$slaveMaster[%player] = "";
	$slaveMaster[%clientId] = "";

	$ParasiteSlave::OldTeam[%clientId] = "";
	$ParasiteSlave::OldTeam[%player] = "";

	$MastersSlave[%masterClient] = "";

	GameBase::setTeam(%clientId, %oldTeam);
	if(%dump)
		return;
	else
	{
		client::sendMessage(%clientId, 1, "You have been freed from the wrath of the parasite!");
		client::sendMessage(%masterClient, 1, "You pet "@Client::getName(%damagedClient)@" has broke out of your control!");
	}
}

function ParasiteDamageType::DumpSlave(%clientId)
{
	%player = Client::getOwnedObject(%clientId);
	%masterClient = $slaveMaster[%clientId];

	ParasiteDamageType::RemoveParasite(%clientId, true);

	for(%i = 0;(!Player::isDead(%player) && $i < 10); $i++)
	{
		%obj = newObject("","Mine","InstantExplosive");
		GameBase::throw(%obj, %masterClient, 0, false);
		GameBase::setPosition(%obj, gamebase::getposition(%player));
	}
	schedule("if(!Player::isDead("@%player@")) remoteKill("@%clientId@");",0.1,%player);

	client::sendMessage(%clientId, 1, "You have CONSUMED by your master!");
	client::sendMessage(%masterClient, 1, "You have CONSUMED your worthless pet "@Client::getName(%damagedClient)@"!");
	Player::incItemCount(%masterClient,Mana,50);
}