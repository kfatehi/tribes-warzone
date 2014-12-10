ItemImageData ShieldSpellI
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

ItemData ShieldSpell
{
	description = "Shield Spell";
	className = "Weapon";
	shapeFile = "rsmoke";
	hudIcon = "mortargun";
   heading = "bSpells";
	shadowDetailMask = 4;
	imageType = ShieldSpellI;
	price = 500;
	showWeaponBar = true;
   validateShape = false;
   validateMaterials = true;
};

function ShieldSpellI::onFire(%player, %slot) 
{
	%playerId = Player::getClient(%player);

	%client = GameBase::getOwnerClient(%player);
	%trans = GameBase::getMuzzleTransform(%player);

	if(!$ManaChargeOn[%client])
	{
		Mana::onCharge(%player);
		Player::incItemCount(%player,Mana,10);
	}

	if(Player::getItemCount(%player,Mana) > 249)
	{
		%vel = rotVector( "0 0 0", GameBase::getRotation(%player));
		Projectile::spawnProjectile("ShieldBolt",%trans,%player,%vel);
		Player::decItemCount(%player,Mana,150);
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

$InvList[ShieldSpell] = 1;
$RemoteInvList[ShieldSpell] = 1;

$ItemMax[magearmor, ShieldSpell] = 1;
$ItemMax[magefemale, ShieldSpell] = 1;

$AutoUse[ShieldSpell] = True;

$WeaponAmmo[ShieldSpell] = "Mana";

AddWeapon(ShieldSpell);

$WeaponSpecial[ShieldSpell] = true;

function ShieldSpell::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	bottomprint(%clientId, "<jc><f2>Using " @ %item.description @ " - <f0>150 <f2>MANA MIN: <f0>350 <f2>MANA", 2);
}