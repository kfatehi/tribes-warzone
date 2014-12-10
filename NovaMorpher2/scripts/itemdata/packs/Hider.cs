//========================================================================//
//== This is the weapon that controls the morph of a novamorpher :-)... ==//
//========================================================================//

ItemImageData HiderPackImage
{
	shapeFile = "shield";
	mountPoint = 2;
	weaponType = 2;  // Sustained
	minEnergy = 8;
	maxEnergy = 8;   // Energy/sec for sustained weapons
	sfxFire = SoundShieldOn;
	firstPerson = false;
};

ItemData HiderPack
{
	description = "Terrain Shifter";
	shapeFile = "shield";
	className = "Backpack";
   heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = HiderPackImage;
	price = 175;
	hudIcon = "repairpack";
	showWeaponBar = true;
	hiliteOnActive = true;
   validateShape = false;
   validateMaterials = true;
};


function HiderPackImage::onActivate(%player,%imageSlot)
{
	%clientId = Player::getClient(%player);

	Client::sendMessage(%clientId,0,"Activating Terrain Shifter");

	if(Player::hasFlag(%player))
	{
		Player::dropItem(%player, $theFlag);
		Client::sendMessage(%client, 1, "You materialized without the flag!~werror_message.wav");
	}

	$HiderPack::OldPos[%player] = GameBase::getPosition(%player);
	%xPos = getword($HiderPack::OldPos[%player], 0);
	%yPos = getword($HiderPack::OldPos[%player], 1);
	%Pos = %xPos @ " " @ %yPos @ " -100000";
	GameBase::setPosition(%player, %Pos);

	%player.Sub = newObject("Player Subsitution", "StaticShape", AcceleratorZap, true);
	GameBase::setPosition(%player.Sub, $HiderPack::OldPos[%player]);

	Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
	Observer::setOrbitObject(%clientId, %player.Sub, 3, 3, 3);

	%clientId.guiLock = true;

	bottomprint(%clientId, "<jc><f0>Press the <f4>JUMP KEY <f0> when you want to return!", 5);
}

function HiderPackImage::onDeactivate(%player,%imageSlot)
{
	%clientId = Player::getClient(%player);

	Client::sendMessage(%clientId,0,"Deactivating Terrain Shifter");

	Item::setVelocity(%player, 0);
	GameBase::setPosition(%player, $HiderPack::OldPos[%player]);
	Item::setVelocity(%player, 0);

	Client::setControlObject(%clientId, %player);

	deleteObject(%player.Sub);
	%player.Sub = "";

	%clientId.guiLock = false;
}

$packDiscription[HiderPack] = "Who ever said you couldn't hide from the pure existance of life? Unfortunately you some of your life seems to go when you hide...";

$InvList[HiderPack] = 1;
$ItemMax[marmor, HiderPack] = 1;
$ItemMax[mfemale, HiderPack] = 1;
