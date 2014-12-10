//----------------------------------------------------------------------------

ItemImageData HeatSSPackImage
{
	shapeFile = "shieldPack";
	mountPoint = 2;
	weaponType = 2;  // Sustained
	minEnergy = 2;
	maxEnergy = 4;   // Energy/sec for sustained weapons
	sfxFire = SoundShieldOn;
	firstPerson = false;
};

ItemData HeatSSPack
{
	description = "Heat Shield Pack";
	shapeFile = "shieldPack";
	className = "Backpack";
   heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = HeatSSPackImage;
	price = 175;
	hudIcon = "shieldpack";
	showWeaponBar = true;
	hiliteOnActive = true;
   validateShape = false;
   validateMaterials = true;
};

function HeatSSPackImage::onActivate(%player,%imageSlot)
{
	if(%player.shieldStrength < 0)
		%player.shieldStrength = 0;

	Client::sendMessage(Player::getClient(%player),0,"Heat Shield On");
	%player.heatSync = %player.heatSync + 1;
	%player.shieldStrength = %player.shieldStrength + 0.0075;
}

function HeatSSPackImage::onDeactivate(%player,%imageSlot)
{
	Client::sendMessage(Player::getClient(%player),0,"Heat Shield Off");
	Player::trigger(%player,$BackpackSlot,false);
	%player.heatSync = %player.heatSync - 1;
	%player.shieldStrength = %player.shieldStrength - 0.0075;

	if(%player.shieldStrength < 0)
		%player.shieldStrength = 0;
}


$packDiscription[HeatSSPack] = "Protect your self against the hot plasma or fire type weapons! Special Ability: Heat Sync & Fire to Energy Conversion! Note: More energy, the stronger the shield.";

$InvList[HeatSSPack] = 1;
$RemoteInvList[HeatSSPack] = 1;

$ItemMax[marmor, HeatSSPack] = 1;
$ItemMax[mfemale, HeatSSPack] = 1;

$ItemMax[harmor, HeatSSPack] = 1;
$ItemMax[hfemale, HeatSSPack] = 1;

$ItemMax[sniperXarmor, HeatSSPack] = 1;
$ItemMax[sniperXfemale, HeatSSPack] = 1;
