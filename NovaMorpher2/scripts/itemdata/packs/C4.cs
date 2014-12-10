//== A superbomb capable of killing almost anything... I think... Except blastwall... :'(

StaticShapeData C4Dep
{
	className = "Beacon";
	damageSkinData = "objectDamageSkins";

	shapeFile = "grenammo";
	maxDamage = 2.5;
	maxEnergy = 200;

	castLOS = true;
	supression = false;
	mapFilter = 2;
	visibleToSensor = true;
	explosionId = flashExpSmall;
	debrisId = flashDebrisSmall;
};

function C4Dep::CheckDetonate(%this)
{
	%player = $C4BombDep[%this];
	if($C4::BlowUp[%player])
		C4Dep::addBomb(%this);
	else
		schedule("C4Dep::CheckDetonate("@%this@");",5,%this);
}

function C4Dep::onDestroyed(%this)
{
	C4Dep::addBomb(%this);
}

function C4Dep::addBomb(%this)
{
	ShockZap(%this);

	%obj = newObject("","Mine","C4MineCore");
	%player = $C4BombDep[%this];
	$C4BombDep[%this] = "";
	GameBase::throw(%obj,%player,0.1,true);

	addToSet("MissionCleanup", %obj);
	%padd = "0 0 0";
	%pos = Vector::add(GameBase::getPosition(%this), %padd);
	deleteObject(%this); //== Oh, bye bye!
	GameBase::setPosition(%obj, %pos);
	%player.C4Bomb--;
}

MineData C4MineCore
{
	mass = 0.01;
	drag = 0.0;
	density = 2.0;
        elasticity = 0.0;
        friction = 0.0;
        className = "Handgrenade";
	description = "Shock Grenade";
	shapeFile = "grenammo";
	shadowDetailMask = 4;
	explosionId = GrenadeExp;
        explosionRadius = 25.0;
        damageValue = 3.0;
        damageType = $MineDamageType;
        kickBackStrength = 100;
        triggerRadius = 0.5;
        maxDamage = 5;
};

function C4MineCore::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");",0.1,%this);
}

//== Needed Mine Funcs ==//

function ShockZap(%this)
{
	%obj = newObject("","Mine","shockadd");
	addToSet("MissionCleanup", %obj);
	%padd = "0 0 0";
	%pos = Vector::add(GameBase::getPosition(%this), %padd);
	GameBase::setPosition(%obj, %pos);
}


MineData Shockadd
{
           mass = 5.0;
           drag = 1.0;
           density = 2.0;
        elasticity = 0.15;
        friction = 1.0;
        className = "Handgrenade";
        description = "End of the line";
        shapeFile = "force";
        shadowDetailMask = 4;
        explosionId = LargeShockwave;
        explosionRadius = 37.5;
        damageValue = 0.25;
        damageType = $MortarDamageType;
        kickBackStrength = 100;
        triggerRadius = 0.5;
        maxDamage = 0.5;
};

function shockadd::onAdd(%this)
{
        schedule("Mine::Detonate(" @ %this @ ");",0.1,%this);
}

//===== The actual pack data starts here =====//

ItemImageData C4PackImage
{
	shapeFile = "grenammo";
	mountPoint = 2;
    	mountOffset = { 0, -0.1, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData C4Pack
{
	description = "C4 Explosive";
	shapeFile = "grenammo";
	className = "Backpack";
    	heading = "dDefence";
	imageType = C4PackImage;
	shadowDetailMask = 4;
	mass = 3;
	elasticity = 1.01;
	price = 995;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function C4Pack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item)
	{
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else
	{
		Player::deployItem(%player,%item);
	}
}

function C4Pack::onDeploy(%player,%item,%pos)
{
	%clientId = Player::getClient(%player);
	if(%player.C4Bomb < 6)
	{
 		%client = Player::getClient(%player);
		if (GameBase::getLOSInfo(%player,2)) //= You have to be close to set it
		{
			%obj = getObjectType($los::object);
	
			if (Vector::dot($los::normal,"0 0 1") > 0.6)
			{
				%rot = "0 0 0";
			}
			else
			{
				if (Vector::dot($los::normal,"0 0 -1") > 0.6)
				{
					%rot = "3.14159 0 0";
				}
				else
				{
					%rot = Vector::getRotation($los::normal);
				}
			}
	
			$C4::BlowUp[%player] = "";
			%team = GameBase::getTeam(%player);

			%beacon = newObject("C4 Explosive", "StaticShape", "C4Dep", true);
			addToSet("MissionCleanup", %beacon);
			GameBase::setTeam(%beacon,GameBase::getTeam(%player));
			GameBase::setRotation(%beacon,%rot);
			GameBase::setPosition(%beacon,$los::position);
			Gamebase::setMapName(%beacon,"C4 - "@Client::getName(%clientId));
			Client::sendMessage(%client,0,"C4 deployed... Waiting for detonation command....");

			Player::setItemCount(%player,"C4Activator",1);
			Player::mountItem(%player,C4Activator,$WeaponSlot);

			$C4BombDep[%beacon] = %player;

			C4Dep::CheckDetonate(%beacon);
			if(%player.C4Bomb < 1 || !%player.C4Bomb)
				%player.C4Bomb = 0;
			else
				%player.C4Bomb++;
		}
		else
		{
			Client::sendMessage(%client,0,"Must literally touch the wall to deploy it.");
		}
	}
	else
	{
		Client::sendMessage(%client,0,"Hey! You still have a bomb waiting for you!");
	}
}

$packDiscription[C4Pack] = "This is a bomb that you get to set off! Just deploy it and press fire when you want to make it explode! Remember, you can only deploy 1 at a time...";

$InvList[C4Pack] = 1;
$RemoteInvList[C4Pack] = 0;

$ItemMax[marmor, C4Pack] = 1;
$ItemMax[mfemale, C4Pack] = 1;

Patch::AddReInit("C4Pack");
//==== C4 Activator Data ====//
ItemImageData C4ActivatorImage
{
	shapeFile = "paintgun";
	mountPoint = 0;
	weaponType = 0; // Single Shot
	accuFire = true;
	reloadTime = 2.5;
	fireTime = 0;
	minEnergy = 0;
	maxEnergy = 0;

	mountRotation = { 3.14, 3.14, 0 };	

	lightType = 3;  // Weapon Fire
	lightRadius = 5;
	lightTime = 3;
	lightColor = { 1, 0, 0 };

	sfxFire = SoundDryFire;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData C4Activator
{
	heading = "bWeapons";
	description = "C4Activator";
	className = "Weapon";
	shapeFile  = "paintgun";
	hudIcon = "mortar";
	shadowDetailMask = 4;
	imageType = C4ActivatorImage;
	price = 85;
	showWeaponBar = true;
};

function C4ActivatorImage::onFire(%player, %slot)
{
	Player::decItemCount(%player,"C4Activator");
	$C4::BlowUp[%player] = true;
}


//==== Add/Delete/Reset bomb code ====//
function AddC4(%this, %team, %clientId)
{
	$C4ID[$NC4[%clientId], %clientId, %team] = %this;
	$C4Owner[$NC4[%team]] = %clientId;

	$C4This[%this] = $NC4[%team];

	$NC4[%team]++;
}

function ResetC4()
{
	for(%i = -1; %i < 5; %i++)
		$NC4[%i] = 0;

	for(%i = -1; %i < 5; %i++)
	{
		for(%o = 0; %o < 6; %o++)
		{
			%this = $C4ID[%o, %i];

			$NextC4[%this] = "";
			$C4ID[%o, %i] = "";
			$C4This[%this] = "";
		}
	}
}
