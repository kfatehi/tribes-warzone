//----------------------------------------------------------------------------

ItemData MortarAmmo
{
	description = "Mortar Ammo";
	className = "Ammo";
   heading = "xAmmunition";
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 5;
};

ItemImageData MortarImage
{
	shapeFile = "mortargun";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	ammoType = MortarAmmo;
	projectileType = "Undefined";
	accuFire = false;
	reloadTime = 0.25;
	fireTime = 1.0;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = SoundFireMortar;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundMortarReload;
	sfxReady = SoundMortarIdle;
};

function MortarImage::onFire(%player, %slot) 
{
	%Ammo = Player::getItemCount(%player, MortarAmmo);
	%armor = Player::getArmor(%player);
	%client = GameBase::getOwnerClient(%player);
	
	 if(%Ammo) 
	 {
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
	
		if ($Settings::Mortar[%client] == "" || $Settings::Mortar[%client] == 0)
		{	
			%client = GameBase::getOwnerClient(%player);
			Player::decItemCount(%player,MortarAmmo,1);
			%trans = GameBase::getMuzzleTransform(%player);
			%vel = Item::getVelocity(%player);

			Projectile::spawnProjectile("MortarShell",%trans,%player,%vel,%player);
		}
		else if ($Settings::Mortar[%client] == "1")
		{			
			%client = GameBase::getOwnerClient(%player);
			Player::decItemCount(%player,MortarAmmo,1);
			%trans = GameBase::getMuzzleTransform(%player);
			%vel = Item::getVelocity(%player);

			Projectile::spawnProjectile("IMortarShell",%trans,%player,%vel,%player);
		}
	}
	else
	{
		Client::sendMessage(Player::getClient(%player), 0,"You have no Ammo for the Mortar");
		bottomprint(Player::getClient(%player), "You have no Ammo for the Mortar",5);
	}
}

ItemData Mortar
{
	description = "Mortar";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "mortar";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = MortarImage;
	price = 375;
	showWeaponBar = true;
   validateShape = false;
};

$WeaponSpecial[Mortar] = "true"; //== Wether this weapon has multiple settings or not...

function Mortar::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	if($Settings::Mortar[%clientId] == "0" || $Settings::Mortar[%clientId] == "")
	{
		%mode = "Normal";
	}
	else if($Settings::Mortar[%clientId] == "1")
	{
		%mode = "Impact";
	}
	bottomprint(%clientId, "<jc><f2>Using " @ %item.description @ " - <f0>" @ %mode, 2);
}