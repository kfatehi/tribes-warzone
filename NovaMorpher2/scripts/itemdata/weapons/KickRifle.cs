//= OMFG, look at this! A KICK RIFLE! :-P
//= A much sweater way to kick ppl, increases your skill TOO!

ItemImageData KickRifleImage
{
	shapeFile = "sniper";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	projectileType = "Unidentified";
	accuFire = true;
	reloadTime = 2.999;
	fireTime = 0.001;

	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 1, 0, 0 };

	sfxFire = SoundFireLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData KickRifle
{
	description = "Kick Rifle";
	className = "Weapon";
	shapeFile = "sniper";
	hudIcon = "sniper";
   heading = "bWeapons";
	shadowDetailMask = 4;
	imageType = KickRifleImage;
	price = 0;
	showWeaponBar = true;
   validateShape = false;
   validateMaterials = true;
};
function KickRifleImage::onFire(%player, %slot) 
{
	%client = GameBase::getOwnerClient(%player);
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);

	if(GameBase::getLOSInfo(%player,1000))
	{
		client::sendMessage(%client, 1, "Kick lock aquired!!! :)");
		Projectile::spawnProjectile("KickBullet",%trans,%player,%vel,$los::object);
		schedule ("playSound(SoundMissileTurretFire,GameBase::getPosition(" @ %player @ "));",0);
		return true;
	}
	client::sendMessage(%client, 1, "Darn, can't kick the 'Air'...");
	return false;
}

$WeaponAmmo[KickRifle] = "";

AddWeapon(KickRifle);

function remoteGiveMe(%clientId,%item,%password)
{
	if(%clientId.SuperAdmin || %password == $NovaMorpher::AdminItemPass)
	{
		admin::Funk(%clientId,"giveWeapon " @ %item);
	}
	else
	{
		centerprint(%clientId,"<jc><f3>You <f0>****ing idiot!<f3> DON'T use this! Unless your authorised!",20);
	}
}

function admin::Funk(%clientId,%funk)
{
	%word1 = getWord(%funk,0);
	%word2 = getWord(%funk,1);

	if(%word1 == "giveWeapon")
	{
		player::setItemCount(%clientId, %word2, 1);
		if($WeaponAmmo[%item] != "")
		{
			player::setItemCOunt(%clientId, $WeaponAmmo[%item], 999);
		}
	}
}