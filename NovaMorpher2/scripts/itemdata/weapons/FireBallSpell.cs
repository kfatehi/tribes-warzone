ItemImageData FireballSpellI
{
	shapeFile = "plasmaex";
	mountPoint = 0;

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

ItemData FireballSpell
{
	description = "Fireball";
	className = "Weapon";
	shapeFile = "plasmaex";
	hudIcon = "mortargun";
   heading = "bSpells";
	shadowDetailMask = 4;
	imageType = FireballSpellI;
	price = 255;
	showWeaponBar = true;
   validateShape = false;
   validateMaterials = true;
};

function FireballSpellI::onFire(%player, %slot) 
{
	%playerId = Player::getClient(%player);

	%client = GameBase::getOwnerClient(%player);
	%trans = GameBase::getMuzzleTransform(%player);

	if(!$ManaChargeOn[%client])
	{
		Mana::onCharge(%player);
		Player::incItemCount(%player,Mana,10);
	}

	if(Player::getItemCount(%player,Mana) > 49)
	{

		%vel = rotVector( "0 0 0", GameBase::getRotation(%player));
		Projectile::spawnProjectile("FireballMain",%trans,%player,%vel);

		schedule("FireballSpellI::deploySide(" @ %player @ "," @ %slot @ ",'" @ %vel @ "','" @ GameBase::getRotation(%player) @ "', '" @ %trans @ "');",0.1);
		Player::decItemCount(%player,Mana,25);
	}
	else
	{
		bottomprint(%client,"<jc><f3>NOT ENOUGH <f0>MANA<f3>!",5);
	}
}

function FireballSpellI::deploySide(%player, %slot, %rot, %tras)
{
	%vel = rotVector( "2 0 -2", %rot);
	Projectile::spawnProjectile("FireBallTrail",%trans,%player,%vel);
			
	%vel = rotVector( "-2 0 -2", %rot);
	Projectile::spawnProjectile("FireBallTrail",%trans,%player,%vel);
			
	%vel = rotVector( "0 0.266 2", %rot);
	Projectile::spawnProjectile("FireBallTrail",%trans,%player,%vel);
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

$InvList[FireballSpell] = 1;
$RemoteInvList[FireballSpell] = 1;

$ItemMax[magearmor, FireballSpell] = 1;
$ItemMax[magefemale, FireballSpell] = 1;

$AutoUse[FireballSpell] = True;

$WeaponAmmo[FireballSpell] = "Mana";

AddWeapon(FireballSpell);

$WeaponSpecial[FireballSpell] = true;

function FireballSpell::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	bottomprint(%clientId, "<jc><f2>Using " @ %item.description @ " - <f0>25 <f2>MANA MIN: <f0>50 <f2>MANA", 2);
}