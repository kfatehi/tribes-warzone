ItemData MMinigunAmmo 
{ 
	description = "MMinigun Ammo"; 

	ammoType = MMinigunAmmo; 
	className = "Ammo"; 
	shapeFile = "ammo1"; 
	heading = "xAmmunition"; 
	shadowDetailMask = 4; 
	price = 1; 
}; 

ItemImageData MMinigunImage
{ 
	shapeFile = "paintgun"; 
	mountPoint = 0; 
	mountOffset = { -1.21, -0.351, 0 }; 
	mountRotation = { 0, 1.01, 0}; 
	weaponType = 1; 
	reloadTime = 0; 
	ammoType = MMinigunAmmo; 
	spinUpTime = 0.1; 
	spinDownTime = 0.1; 
	fireTime = 0.1; 
	lightType = 3; 
	lightRadius = 3; 
	lightTime = 1; 
	lightColor = { 0.6, 1, 1 }; 
}; 

function MMinigunImage::onFire(%player, %slot) 
{ 
	%client = GameBase::getOwnerClient(%player); 

	%ammo = $WeaponAmmo[MMinigun];

	if(!$MDFiringMMinigun[%player]) MDCheckMMinigun(%client, %player); 
} 
ItemData MMinigun 
{ 
	description = "MECHMiniGun ICPU"; 
	className = "Weapon"; 
	shapeFile = "chaingun"; 
	hudIcon = "chain"; 
   heading = "bWeapons";
	shadowDetailMask = 4; 
	imageType = MMinigunImage; 
	price = 7500; 
	showWeaponBar = true; 
}; 

function MMinigun::onDrop(%player,%item) 
{ 
	%state = Player::getItemState(%player,$WeaponSlot); 
	if (%state != "Fire" && %state != "Reload") 
	{ 
		Player::setItemCount(%player, MMinigun1, 0); 
		Player::setItemCount(%player, MMinigun2, 0); 
		Player::setItemCount(%player, MMinigun3, 0); 
		Player::setItemCount(%player, MMinigun4, 0); 
		Item::onDrop(%player,%item); 
	}
} 

function MMinigun::onMount(%player,%imageSlot) 
{ 
	%client = GameBase::getOwnerClient(%player); 
	%armor = Player::getArmor(%player);

	%maxWeps = Player::getItemClassCount(%client,"Weapon")-5;
	if(%maxWeps++ > $MaxWeapons[%armor] || Player::getItemCount(%player, MMinigun4) > 0) 
	{
		Player::setItemCount(%player, MMinigun4, 1);
		Player::mountItem(%player,MMinigun4,$ExtraWeaponSlotB); 
		if(%maxWeps++ > $MaxWeapons[%armor] || Player::getItemCount(%player, MMinigun3) > 0) 
		{
			Player::setItemCount(%player, MMinigun3, 1);
			Player::mountItem(%player,MMinigun3,$ExtraWeaponSlotC); 
			if(%maxWeps++ > $MaxWeapons[%armor] || Player::getItemCount(%player, MMinigun2) > 0) 
			{
				Player::setItemCount(%player, MMinigun2, 1);
				Player::mountItem(%player,MMinigun2,$ExtraWeaponSlotD); 
				if(%maxWeps++ > $MaxWeapons[%armor] || Player::getItemCount(%player, MMinigun1) > 0) 
				{
					Player::setItemCount(%player, MMinigun1, 1);
					Player::mountItem(%player,MMinigun1,$ExtraWeaponSlotA); 
					return true;
				}
			}
		}
	}

	client::Sendmessage(%client, 1, "Problem, couldn't find some part(s) of the gun!");
	return false;
} 

function MMinigun::onUnmount(%player,%imageSlot) 
{ 
	Player::unmountItem(%player,$ExtraWeaponSlotA);
	Player::unmountItem(%player,$ExtraWeaponSlotB);	 
	Player::unmountItem(%player,$ExtraWeaponSlotC);
	Player::unmountItem(%player,$ExtraWeaponSlotD);
} 

ItemImageData MMinigun1Image 
{ 
	shapeFile = "chaingun"; 
	mountPoint = 0; 
	mountOffset = { -1.21, -0.351, 0 }; 
	mountRotation = { 0, 1.01, 0}; 
	weaponType = 1; 
	reloadTime = 0; 
	spinUpTime = 0.1; 
	spinDownTime = 0.1; 
	fireTime = 0.1; 
	ammoType = MMinigunAmmo; 
	projectileType = ChaingunBullet; 
	accuFire = true; 
	lightType = 3; 
	lightRadius = 3; 
	lightTime = 1; 
	lightColor = { 0.6, 1, 1 }; 
	sfxFire = SoundFireChaingun; 
}; 

ItemData MMinigun1
{ 
	description = "MECHM PRT 1"; 
	className = "Weapon"; 
	shapeFile = "chaingun"; 
	hudIcon = "chain"; 
	heading = "bWeapons"; 
	shadowDetailMask = 4; 
	imageType = MMinigun1Image; 
	price = 0; 
	showWeaponBar = true; 
	showInventory = true; 
}; 

ItemImageData MMinigun2Image 
{ 
	shapeFile = "chaingun"; 
	mountPoint = 0; 
	mountOffset = { -1.21, -0.351, 0.4 }; 
	mountRotation = { 0, 1.01, 0}; 
	weaponType = 1; 
	reloadTime = 0; 
	spinUpTime = 0.1; 
	spinDownTime = 0.1; 
	fireTime = 0.1; 
	ammoType = MMinigunAmmo; 
	projectileType = ChaingunBullet; 
	accuFire = true; 
	lightType = 3; 
	lightRadius = 3; 
	lightTime = 1; 
	lightColor = { 0.6, 1, 1 }; 
	sfxFire = SoundFireChaingun; 
}; 

ItemData MMinigun2 
{ 
	description = "MECHM PRT 2"; 
	className = "Weapon"; 
	shapeFile = "chaingun"; 
	hudIcon = "chain"; 
	heading = "bWeapons"; 
	shadowDetailMask = 4; 
	imageType = MMinigun2Image; 
	price = 0; 
	showWeaponBar = true; 
	showInventory = true; 
}; 

ItemImageData MMinigun3Image 
{ 
	shapeFile = "chaingun"; 
	mountPoint = 0; 
	mountOffset = { 0, -0.351, 0 }; 
	mountRotation = { 0, -1.01, 0 }; 
	weaponType = 1; 
	reloadTime = 0; 
	spinUpTime = 0.1; 
	spinDownTime = 0.1; 
	fireTime = 0.1; 
	ammoType = MMinigunAmmo; 
	projectileType = ChaingunBullet; 
	accuFire = true; 
	lightType = 3; 
	lightRadius = 3; 
	lightTime = 1; 
	lightColor = { 0.6, 1, 1 }; 
	sfxFire = SoundFireChaingun; 
}; 

ItemData MMinigun3 
{ 
	description = "MECHM PRT 3"; 
	className = "Weapon"; 
	shapeFile = "chaingun"; 
	hudIcon = "chain"; 
	heading = "bWeapons"; 
	shadowDetailMask = 4; 
	imageType = MMinigun3Image; 
	price = 0; 
	showWeaponBar = true; 
	showInventory = true; 
}; 
ItemImageData MMinigun4Image 
{ 
	shapeFile = "chaingun"; 
	mountPoint = 0;
	mountOffset = {0.101, -0.201, 0.4 }; 
	mountRotation = { 0, -1.501, 0};  
	weaponType = 1; 
	reloadTime = 0; 
	spinUpTime = 0.1; 
	spinDownTime = 0.1; 
	fireTime = 0.1; 
	ammoType = MMinigunAmmo; 
	projectileType = ChaingunBullet; 
	accuFire = true; 
	lightType = 3; 
	lightRadius = 3; 
	lightTime = 1; 
	lightColor = { 0.6, 1, 1 }; 
	sfxFire = SoundFireChaingun; 
}; 

ItemData MMinigun4 
{ 
	description = "MECHM PRT 4"; 
	className = "Weapon"; 
	shapeFile = "chaingun"; 
	hudIcon = "chain"; 
	heading = "bWeapons"; 
	shadowDetailMask = 4; 
	imageType = MMinigun4Image; 
	price = 0; 
	showWeaponBar = true; 
	showInventory = true; 
}; 

function MDCheckMMinigun(%client, %player) 
{ 
	if(Player::isTriggered(%player,$WeaponSlot) && (Player::getMountedItem(%player,$WeaponSlot) == "MMinigun")) 
	{ 
		schedule("Player::trigger("@%player@","@$ExtraWeaponSlotA@",true);",0.1);
		schedule("Player::trigger("@%player@","@$ExtraWeaponSlotB@",true);",0.2);
		schedule("Player::trigger("@%player@","@$ExtraWeaponSlotC@",true);",0.3);
		schedule("Player::trigger("@%player@","@$ExtraWeaponSlotD@",true);",0.4);
		schedule("MDCheckMMinigun(" @ %client @ "," @ %player @ ");",0.1); 
		$MDFiringMMinigun[%player] = true; 
	} 
	else 
	{ 
		schedule("Player::trigger("@%player@","@$ExtraWeaponSlotA@",false);",0.1);
		schedule("Player::trigger("@%player@","@$ExtraWeaponSlotB@",false);",0.2);
		schedule("Player::trigger("@%player@","@$ExtraWeaponSlotC@",false);",0.3);
		schedule("Player::trigger("@%player@","@$ExtraWeaponSlotD@",false);",0.4);
		$MDFiringMMinigun[%player] = false; 
	} 
} 

function MMinigun::giveItem(%player,%item,%delta)
{
	return False;
}

$TeamItemMax[MMinigun] = 3;

$InvList[MMinigun]  = 1;
$InvList[MMinigun1] = 1;
$InvList[MMinigun2] = 1;
$InvList[MMinigun3] = 1;
$InvList[MMinigun4] = 1;

$WeaponAmmo[MMinigun]  = MMinigunAmmo; 
$WeaponAmmo[MMinigun1] = MMinigunAmmo; 
$WeaponAmmo[MMinigun2] = MMinigunAmmo; 
$WeaponAmmo[MMinigun3] = MMinigunAmmo; 
$WeaponAmmo[MMinigun4] = MMinigunAmmo; 

$WeaponSlotOccupation[MMinigun] = 5;

$InvList[MMinigunAmmo]=1;
$RemoteInvList[MMinigunAmmo]=1;

$ItemMax[harmor, MMinigun ] = 1;
$ItemMax[harmor, MMinigunAmmo] = 500;

$AutoUse[MMinigun] = False;

AddWeapon(MMinigun);

$WeaponSpecial[MMinigun] = "true"; //== Wether this weapon has multiple settings or not...

function MMinigun::TellMode(%clientId,%item) //== The function that TELLS the MODE if there is...
{
	bottomprint(%clientId, "<jc><f2>Using " @ %item.description, 2);
	schedule("bottomprint("@%clientId@", \"<jc><f2>Amazing! 4x the number of chain-guns! 8x the effeciency of ammo!\", 5);",2);
	schedule("bottomprint("@%clientId@", \"<jc><f2>AND A WOOPING 5x the number of slots it takes up LOL!!!\", 5);",7);
	schedule("bottomprint("@%clientId@", \"<jc><f2>Great D-Killer, bad to use at real combat ;)!!!\", 5);",13);
}