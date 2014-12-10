ItemImageData ParasiteSpellI
{
	shapeFile = "rsmoke";
	mountPoint = 0;

	mountOffset		= { -0.15, 0, 0 };

	weaponType = 0; // Single Shot
	projectileType = "Unidentified";
	ammoType = "Mana";

	accuFire = false;
	reloadTime = 0.001;
	fireTime = 0;

	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };

	sfxFire = SoundFireGrenade;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData ParasiteSpell
{
	description = "Parasite";
	className = "Weapon";
	shapeFile = "plasmaex";
	hudIcon = "mortargun";
   heading = "bSpells";
	shadowDetailMask = 4;
	imageType = ParasiteSpellI;
	price = 500;
	showWeaponBar = true;
   validateShape = false;
   validateMaterials = true;
};

function ParasiteSpellI::onFire(%player, %slot) 
{
	%playerId = Player::getClient(%player);

	%client = GameBase::getOwnerClient(%player);
	%trans = GameBase::getMuzzleTransform(%player);

	if(!$ManaChargeOn[%client])
	{
		Mana::onCharge(%player);
		Player::incItemCount(%player,Mana,10);
	}

	if(Player::getItemCount(%player,Mana) > 499)
	{
		%vel = rotVector( "0 0 0", GameBase::getRotation(%player));
		Projectile::spawnProjectile("ParasiteBug",%trans,%player,%vel);
		Player::decItemCount(%player,Mana,300);
	}
	else
	{
		bottomprint(%client,"<jc><f3>NOT ENOUGH <f0>MANA<f3>!",5);
	}
}

function rotVector(%vec,%rot)
{
	// this function rotates a vector about the z axis

	%vec_x = getWord(%vec,0);
	%vec_y = getWord(%vec,1);
	%vec_z = getWord(%vec,2);

	// new vector with z axis removed
	%basevec = %vec_x @ "  " @ %vec_y @ "  0";
	
	// change vector to distance and rotation
	%basedis = Vector::getDistance( "0 0 0", %basevec);
	%normvec = Vector::normalize( %basevec );
	%baserot = Vector::add( Vector::getRotation( %normvec ), "1.571 0 0" );

	// modify rotation and change back to vector (put z axis offset back)
	%newrot = Vector::add( %baserot, %rot );
	%newvec = Vector::getFromRot( %newrot, %basedis, %vec_z );

	return %newvec;
}

$InvList[ParasiteSpell] = 1;
$RemoteInvList[ParasiteSpell] = 1;

$ItemMax[magearmor, ParasiteSpell] = 1;
$ItemMax[magefemale, ParasiteSpell] = 1;

$AutoUse[ParasiteSpell] = True;

$WeaponAmmo[ParasiteSpell] = "Mana";

AddWeapon(ParasiteSpell);

$WeaponSpecial[ParasiteSpell] = true;

function ParasiteSpell::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	bottomprint(%clientId, "<jc><f2>Using " @ %item.description @ " - <f0>300 <f2>MANA MIN: <f0>500 <f2>MANA", 2);
}