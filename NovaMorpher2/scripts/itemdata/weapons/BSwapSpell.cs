//======================================================================== ATProj
$BSwapDamageType = DamageTypes::AddReserve();
$SpecialDamageType[$BSwapDamageType] = "BSwapDamageType";
function BSwapDamageType::DoSpecialDMG(%this, %shooterPlayer)
{
	%shooterClient = Player::getClient(%shooterPlayer);
	%damagedClient = Player::getClient(%this);

	%type = GameBase::getDataName(%this);
	if(%damagedClient > 2048 && %shooterClient != %damagedClient)
	{
		Client::setControlObject(%shooterClient, %this);
		Client::setOwnedObject(%shooterClient, %this);

		Client::setControlObject(%damagedClient, %shooterPlayer);
		Client::setOwnedObject(%damagedClient, %shooterPlayer);

		GameBase::setTeam(%damagedClient, Client::getTeam(%damagedClient));
		GameBase::setTeam(%shooterClient, Client::getTeam(%shooterClient));

		BSwap::RejectBody(%this, 50, 400); //== Balance issues >)
	}
}

function BSwap::RejectBody(%player, %level, %count)
{
	if(!Player::isDead(%player) && %count)
	{
		%clientId = Player::getClient(%player);
		%level = floor(%level + (floor(getRandom()*6)-3));

		if(%level > 60)
		{
			remotekill(%clientId);
			CenterPrint(%clientId, "<jc><f2>Sorry, the body has mostly <f0>REJECTED <f2>your stay.");
		}
		else
		{
			CenterPrint(%clientId, "<jc>\n<f2>Body <f0>REJECTION <f2> level at:\n----<f1>"@%level@"%<f2>----\n",0.051);
			schedule("BSwap::RejectBody("@%player@","@%level@","@%count-1@");",0.05,%player);
		}
	}
}

RocketData BSwapBolt
{
   bulletShapeName = "zap.dts";
   explosionTag    = bulletExp1;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.0001;
   damageType       = $BSwapDamageType;

   explosionRadius  = 5;
   kickBackStrength = 0.0;

   muzzleVelocity   = 200.0;
   terminalVelocity = 2000.0;
   acceleration     = 100.0;

   totalTime        = 10.0;
   liveTime         = 9.9;

   lightRange       = 5.0;
   lightColor       = { 1.0, 0.7, 0.5 };

   // rocket specific
   trailType   = 2;                // smoke trail
   trailString = "zap.dts";
   smokeDist   = 1.8;

   soundId = SoundJetHeavy;
};

ItemImageData BSwapSpellI
{
	shapeFile = "rsmoke";
	mountPoint = 0;

//	mountRotation = { -1.5, 0, 0 };
	mountOffset		= { -0.15, 0, 0 };

	weaponType = 0; // Single Shot
	projectileType = "Unidentified";
	ammoType = "Mana";

	accuFire = false;
	reloadTime = 0.5;
	fireTime = 0.5;

	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = SoundFireGrenade;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData BSwapSpell
{

	description = "BSwap";
	className = "Weapon";
	shapeFile = "zap";
	hudIcon = "mortargun";
   heading = "bSpells";
	shadowDetailMask = 4;
	imageType = BSwapSpellI;
	price = 255;
	showWeaponBar = true;
   validateShape = false;
   validateMaterials = true;
};

function BSwapSpellI::onFire(%player, %slot) 
{
	%playerId = Player::getClient(%player);

	%client = GameBase::getOwnerClient(%player);
	%trans = GameBase::getMuzzleTransform(%player);

	if(!$ManaChargeOn[%client])
	{
		Mana::onCharge(%player);
		Player::incItemCount(%player,Mana,10);
	}

	if(Player::getItemCount(%player,Mana) > 99)
	{
		%vel = rotVector( "0 0 0", GameBase::getRotation(%player));
		Projectile::spawnProjectile("BSwapBolt",%trans,%player,%vel);
		Player::decItemCount(%player,Mana,200);
	}
	else
	{
		bottomprint(%client,"<jc><f3>NOT ENOUGH <f0>MANA<f3>!",5);
	}
}

$InvList[BSwapSpell] = 1;
$RemoteInvList[BSwapSpell] = 1;

$ItemMax[magearmor, BSwapSpell] = 1;
$ItemMax[magefemale, BSwapSpell] = 1;

$AutoUse[BSwapSpell] = True;

$WeaponAmmo[BSwapSpell] = "Mana";

AddWeapon(BSwapSpell);

$WeaponSpecial[BSwapSpell] = true;

function BSwapSpell::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	bottomprint(%clientId, "<jc><f2>Using " @ %item.description @ " - <f0>200  <f2>MANA MIN: <f0>100 <f2>MANA", 2);
}