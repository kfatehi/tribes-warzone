//==============================================================
//==============================================================
ItemImageData UndeadSpellI
{
	shapeFile = "fusionbolt";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	projectileType = Unknown;
	ammoType = "Mana";
	accuFire = false;
	reloadTime = 0.5;
	fireTime = 0.5;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = SoundFireGrenade;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData UndeadSpell
{
	description = "Undead";
	className = "Weapon";
	shapeFile = "fusionbolt";
	hudIcon = "grenade";
      heading = "bSpells";
	shadowDetailMask = 4;
	imageType = UndeadSpellI;
	price = 255;
	showWeaponBar = true;
   validateShape = false;
};

function UndeadSpellI::onFire(%player, %slot)
{
	%client = Player::getClient(%player);
	%energy = GameBase::getEnergy(%player);

	if((%energy > 200) && (Player::getItemCount(%player,Mana) > 299))
	{
		if (!Player::hasFlag(%player))
		{
			%armor = $ArmorType[Client::getGender(%client), UndeadMage];
			Client::sendMessage(%client, 1, "You feel dead, yet your alive! Walk through WALLS AND RUN FOR YOUR LIFE!!");
			useEnergy(%player,200);
			Player::decItemCount(%player,Mana,200);
			Player::SetArmor(%player, %armor);
		}
		else
			Bottomprint(%client, "<jc><f1>Oops!\n<f2>Cannot transform with the flag!!!",5);
	}
	else
	{
		Bottomprint(%client, "<jc><f1>Oops!\n<f2>Not enough ENERGY and MANA!!!",5);
	}
}

$InvList[UndeadSpell] = 1;
$RemoteInvList[UndeadSpell] = 1;

$ItemMax[magearmor, UndeadSpell] = 1;
$ItemMax[magefemale, UndeadSpell] = 1;

$AutoUse[UndeadSpell] = False;

$WeaponAmmo[UndeadSpell] = "Mana";

AddWeapon(UndeadSpell);

$WeaponSpecial[UndeadSpell] = true;

function UndeadSpell::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	bottomprint(%clientId, "<jc><f2>Using " @ %item.description @ " - USE: <f0>200 <f2>Energy <f0>200 <f2>MANA MIN: <f0>300 <f2>MANA", 2);
}
