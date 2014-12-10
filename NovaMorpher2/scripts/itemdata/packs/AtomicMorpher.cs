//==================================================//
//== This is the only pack NovaMorpher can use... ==//
//==================================================//
exec("itemdata\\packs\\AtomicMorpherData");

function AtomicMorpher::DestroyPreBody(%this)
{
	$isGoner[%this] = true;
	schedule("$isGoner["@%this@"] = false;",2,%this);
        GameBase::setPosition(%this, "-1000 -1000 -1000");
        GameBase::applyRadiusDamage($MortarDamageType, "-1000 -1000 -1000", 500, 10000, 1000, %this);
        GameBase::applyRadiusDamage($LaserDamageType, "-1000 -1000 -1000", 500, 10000, 1000, %this);
        GameBase::applyRadiusDamage($ExplosionDamageType, "-1000 -1000 -1000", 500, 10000, 1000, %this);
}

function MorpherJet::onDestroy(%this)
{
	if(!$isGoner[%this])
		remoteKill(%this.clLastMount);
}

function MorpherJet::Dismount(%this, %cl)
{

	if($Morph::CannotMorph[%cl])
		return;

        %pl = Client::getOwnedObject(%cl);
        %pl.lastMount = %this;
        %pl.newMountTime = getSimTime() + 3.0;
        Player::setMountObject(%pl, -1, 0);
        %rot = GameBase::getRotation(%this);
        %rotZ = getWord(%rot,2);
        GameBase::setRotation(%pl, "0 0 " @ %rotZ);
        Client::setControlObject(%cl, %pl);

        %newPos = Vector::add(GameBase::getPosition(%cl), "0 0 0");
        GameBase::setPosition(%pl, %newPos);

        playSound (GameBase::getDataName(%this).dismountSound, GameBase::getPosition(%this));

        %pl.driver = "";
        %pl.vehicle = "";
        %damage = GameBase::getDamageLevel(%this);
        GameBase::setDamageLevel(%pl, %damage);

        Player::useItem(%cl, "AtomicMorpher");

	%armor = GameBase::getDataName(%pl);
	%type = GameBase::getDataName(%this);
	%damage = ((%armor.maxDamage / 100) * (%damage/(%type.maxDamage/100)));
	GameBase::setDamageLevel(%cl, %damage);

	AtomicMorpher::DestroyPreBody(%this);
}

ItemData MorpherJetVehicle
{
	description = "MorpherJet";
	className = "Vehicle";
   heading = "aVehicle";
	price = 0;
};

$DataBlockName[MorpherJetVehicle] = MorpherJet;
$VehicleToItem[MorpherJet] = MorpherJetVehicle;


function AtomicMorpher::TransformIntoVehicle(%player,%item)
{
	%client = Player::getClient(%player);

	if($Morph::CannotMorph[%client])
		return;

	%vel = Item::getVelocity(%player);
	%flag = Player::getMountedItem(%player, $FlagSlot);
	%pos = GameBase::getPosition(%player);
	%team = GameBase::getTeam(%player);
	%rot = GameBase::getRotation(%player);
	%damage = GameBase::getDamageLevel(%player);

	%weapon = Player::getMountedItem(%player,$WeaponSlot);
	%vehicle = newObject("MorpherJet",flier,MorpherJet,true);
  	%vehicle.clLastMount = %client;	
  	%vehicle.fading = 1;
	%player.driver = 1;
	%player.vehicle = %vehicle;
	%vehicle.clLastMount = %client;
	addToSet("MissionCleanup", %vehicle);
	GameBase::setMapName(%vehicle,"MorpherJet");
	GameBase::setTeam(%vehicle,%team);
	GameBase::setPosition(%vehicle, %pos);
	GameBase::setRotation(%vehicle,%rot);
	Player::setMountObject(%player, %vehicle, 1);
	Client::setControlObject(%client, %vehicle);

	%armor = GameBase::getDataName(%player);
	%type = GameBase::getDataName(%vehicle);
	%damage = ((%type.maxDamage / 100) * (GameBase::getDamageLevel(%player)/(%armor.maxDamage/100)));
	GameBase::setDamageLevel(%vehicle, %damage);

	if(%weapon != -1)			// Weapon Check
	{
		%player.lastWeapon = %weapon;
		Player::unMountItem(%player,$WeaponSlot);
	}
	return %vehicle;
}

//== Turret #1 - Plasma
function MorpherPlasmaTurret::jump(%this,%mom)
{
   MorpherPlasmaTurret::dismount(%this,%mom);
}

function MorpherPlasmaTurret::onDestroy(%this)
{
	if(!$isGoner[%this])
		remoteKill(%this.clLastMount);
}

function MorpherPlasmaTurret::onFire(%this)
{
	%trans = GameBase::getMuzzleTransform(%this); 
	%vel = Item::getVelocity(%this); 
	Projectile::spawnProjectile("plasmabolt",%trans,%this,%vel);
}

function MorpherPlasmaTurret::Dismount(%this, %mom)
{
       %cl = GameBase::getControlClient(%this);

	if($Morph::CannotMorph[%cl])
		return;

        %pl = Client::getOwnedObject(%cl);
        %pl.lastMount = %this;
        %pl.newMountTime = getSimTime() + 3.0;
        Player::setMountObject(%pl, -1, 0);
        %rot = GameBase::getRotation(%this);
        %rotZ = getWord(%rot,2);
        GameBase::setRotation(%pl, "0 0 " @ %rotZ);
        Client::setControlObject(%cl, %pl);

        %newPos = Vector::add(GameBase::getPosition(%cl), "0 0 0");
        GameBase::setPosition(%pl, %newPos);

        playSound (GameBase::getDataName(%this).dismountSound, GameBase::getPosition(%this));

        %pl.driver = "";
        %pl.vehicle = "";

        Player::useItem(%cl, "AtomicMorpher");

	  %armor = GameBase::getDataName(%pl);
	  %type = GameBase::getDataName(%this);
	  %damage = ((%armor.maxDamage / 100) * (%damage/(%type.maxDamage/100)));
	  GameBase::setDamageLevel(%cl, %damage);

	AtomicMorpher::DestroyPreBody(%this);
}

function MorpherPlasmaTurret::onDisable(%this)
{
	MorpherPlasmaTurret::Dismount(%this);
}

function AtomicMorpher::TransformIntoPlasmaTurret(%player,%item)
{
	%client = Player::getClient(%player);

	if($Morph::CannotMorph[%client])
		return;

	%vel = Item::getVelocity(%player);
	%flag = Player::getMountedItem(%player, $FlagSlot);
	%bpos = GameBase::getPosition(%player);
	%pos = Vector::add(%bpos,"0 0 0");
	%team = GameBase::getTeam(%player);
	%rot = GameBase::getRotation(%player);
	%damage = GameBase::getDamageLevel(%player);
	%weapon = Player::getMountedItem(%player,$WeaponSlot);

	%vehicle = newObject("MorpherPlasmaTurret",turret,MorpherPlasmaTurret,true);
	GameBase::setMapName(%vehicle,"MorpherPlasmaTurret");
	GameBase::setTeam(%vehicle,%team);
	GameBase::setPosition(%vehicle,%pos);
	GameBase::setRotation(%vehicle,%rot);

//	GameBase::setPosition(%player,vector::subtract(%pos, "0 0 1000"));
//	Player::setMountObject(%player, %vehicle, 1);

	Client::setControlObject(%client, %vehicle);
	GameBase::setDamageLevel(%vehicle, %damage);
	GameBase::setRechargeRate(%vehicle,1);

	%armor = GameBase::getDataName(%player);
	%type = GameBase::getDataName(%vehicle);
	%damage = ((%type.maxDamage / 100) * (%damage/(%armor.maxDamage/100)));
	GameBase::setDamageLevel(%vehicle, %damage);

	if(%weapon != -1)			// Weapon Check
	{
		%player.lastWeapon = %weapon;
		Player::unMountItem(%player,$WeaponSlot);
	}
	return %vehicle;
}


//========== Blast Wall Transformation :-P

function MorpherBlastWall::jump(%this,%mom)
{
	MorpherBlastWall::dismount(%this,%mom);
}

function MorpherBlastWall::onDisable(%this)
{
	MorpherBlastWall::dismount(%this);
}

function MorpherBlastWall::onDestroy(%this)
{
	if(!$isGoner[%this])
		remoteKill(%this.clLastMount);
}

function MorpherBlastWall::Dismount(%this, %mom)
{
        %cl = GameBase::getControlClient(%this);

	if($Morph::CannotMorph[%cl])
		return;

        %pl = Client::getOwnedObject(%cl);
        Player::setMountObject(%pl, -1, 0);
        %rot = GameBase::getRotation(%this);
        %rotZ = getWord(%rot,2);
        GameBase::setRotation(%pl, "0 0 " @ %rotZ);
        Client::setControlObject(%cl, %pl);

        %newPos = Vector::add(GameBase::getPosition(%this), "0 0 0");
        GameBase::setPosition(%pl, %newPos);

        playSound (GameBase::getDataName(%this).dismountSound, GameBase::getPosition(%this));

        %pl.driver = "";
        %pl.vehicle = "";

        Player::useItem(%cl, "AtomicMorpher");

	  %armor = GameBase::getDataName(%pl);
	  %type = GameBase::getDataName(%this);
	  %damage = ((%armor.maxDamage / 100) * (%damage/(%type.maxDamage/100)));
	  GameBase::setDamageLevel(%cl, %damage);

	AtomicMorpher::DestroyPreBody(%this);

}

function AtomicMorpher::TransformIntoMorpherBlastWall(%player,%item)
{
	%client = Player::getClient(%player);

	if($Morph::CannotMorph[%client])
		return;

	%vel = Item::getVelocity(%player);
	%flag = Player::getMountedItem(%player, $FlagSlot);
	%bpos = GameBase::getPosition(%player);
	%pos = Vector::add(%bpos,"0 0 0");
	%team = GameBase::getTeam(%player);
	%rot = GameBase::getRotation(%player);
	%damage = GameBase::getDamageLevel(%player);
	%weapon = Player::getMountedItem(%player,$WeaponSlot);

	%vehicle = newObject("MorpherBlastWall",turret,MorpherBlastWall,true);
	GameBase::setMapName(%vehicle,"MorpherBlastWall");
	GameBase::setTeam(%vehicle,%team);
	GameBase::setPosition(%vehicle,%pos);
	GameBase::setRotation(%vehicle,%rot);

	GameBase::setPosition(%player,vector::subtract(%pos, "0 0 1000"));
//	Player::setMountObject(%player, %vehicle, 1);

	Client::setControlObject(%client, %vehicle);
	GameBase::setDamageLevel(%vehicle, %damage);
	GameBase::setRechargeRate(%vehicle,1);

	%armor = GameBase::getDataName(%player);
	%type = GameBase::getDataName(%vehicle);
	%damage = ((%type.maxDamage / 100) * (%damage/(%armor.maxDamage/100)));
	GameBase::setDamageLevel(%vehicle, %damage);

	if(%weapon != -1)			// Weapon Check
	{
		%player.lastWeapon = %weapon;
		Player::unMountItem(%player,$WeaponSlot);
	}


	return %vehicle;
}



ItemImageData AtomicMorpherI
{
	shapeFile = "shieldPack";
	mountPoint = 2;
	weaponType = 2;  // Sustained
	minEnergy = 4;
	maxEnergy = 9;   // Energy/sec for sustained weapons
	sfxFire = SoundShieldOn;
	firstPerson = false;
};

ItemData AtomicMorpher
{
	description = "Atomic Morpher";
	shapeFile = "shieldPack";
	className = "Backpack";
   heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = AtomicMorpherI;
	price = 175;
	hudIcon = "shieldpack";
	showWeaponBar = true;
	hiliteOnActive = true;
   validateShape = false;
   validateMaterials = true;
};

function AtomicMorpher::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item)
	{
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else
	{
		if($Morph::CannotMorph[%clientId])
			return;

		%damage = GameBase::getDamageLevel(%player);
		%oldarmor = Player::getArmor(%pl);
		%olddamage = (%oldarmor.maxDamage / 100);

		if($debug)
		{
			echo("AtomicMorpherI::onActivate(); Called!");
		}
		%clientId = Player::getClient(%player);
		%checkDefault = ($Settings::AtomicMorpher[%clientId] == "" || $Settings::AtomicMorpher[%clientId] == "-1");

		if(%checkDefault)
		{
			if(%player.curMorph == "" || %player.curMorph == "5")
				%player.curMorph = 0;
			else
				%player.curMorph++;

			bottomprint(%clientId,"<jc><f3>Rotating <f0>MORPH<f3> : <f0>"@$AtomicMorpher::Set[%player.curMorph],5);
		}

		if($Settings::AtomicMorpher[%clientId] == "0" || (%player.curMorph == "0" && %checkDefault))
		{
			Player::setArmor(%clientId,nmlightarmor);
			%player.curFig = %player;
		}
		else if($Settings::AtomicMorpher[%clientId] == "1" || (%player.curMorph == "1" && %checkDefault))
		{
			Player::setArmor(%clientId,nmarmor);
			%player.curFig = %player;
		}
		else if($Settings::AtomicMorpher[%clientId] == "2" || (%player.curMorph == "2" && %checkDefault))
		{
			Player::setArmor(%clientId,nmheavyarmor);
			%player.curFig = %player;
		}
		else if($Settings::AtomicMorpher[%clientId] == "3" || (%player.curMorph == "3" && %checkDefault))
		{
			%player.curFig = AtomicMorpher::TransformIntoVehicle(%player,%item);
			bottomprint(%clientId,"Press jump to morph.",5);
		}
		else if($Settings::AtomicMorpher[%clientId] == "4" || (%player.curMorph == "4" && %checkDefault))
		{
			%player.curFig = AtomicMorpher::TransformIntoPlasmaTurret(%player,%item);
			bottomprint(%clientId,"Press jump to morph.",5);
		}
		else if($Settings::AtomicMorpher[%clientId] == "5" || (%player.curMorph == "5" && %checkDefault))
		{
			%player.curFig = AtomicMorpher::TransformIntoMorpherBlastWall(%player,%item);
			bottomprint(%clientId,"Press jump to morph.",5);
		}

		%armor = Player::getArmor(%player);
		%damage = (%olddamage * %damage);
		GameBase::setDamageLevel(%clientId, %damage);

		Player::trigger(%player,$BackpackSlot,false);
	}
}

function AtomicMorpherI::onDeactivate(%player,%imageSlot){}

function AtomicMorpher::onDrop(%player,%item)
{
	%client = Player::getClient(%player);
	Client::SendMessage(%client,0,"Unable to DROP this type of Pack. It is part of the armor.");
}	

$packDiscription[AtomicMorpher] = "This is the ultimate pack a player would want! You can morph into one of a few selections which would be extreamly useful!";

$InvList[AtomicMorpher] = 1;
$RemoteInvList[AtomicMorpher] = 0;

$ItemMax[nmarmor, AtomicMorpher] = 1;
$ItemMax[nmfemale, AtomicMorpher] = 1;
