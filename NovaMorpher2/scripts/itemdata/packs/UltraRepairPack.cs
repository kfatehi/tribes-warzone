//****************************************************
// Ultra Repair Pack based on: AAOD's SuperRepair Pack
//************************************

$InvList[UltraRepairPack]				= 0;
$RemoteInvList[UltraRepairPack]		= 0;

RepairEffectData UltraRepairBolt
{
	bitmapName			= "LightningNew.bmp";
	boltLength			= 200.0;
	segmentDivisions	      = 6;
	beamWidth			= 0.175;
	updateTime			= 450;
	skipPercent			= 0.6;
	displaceBias		= 0.15;
	lightRange			= 6.0;
	lightColor			= { 0.85, 0.25, 0.25 };
};

function UltraRepairBolt::onAcquire(%this, %player, %target)
{
      %client = Player::getClient(%player);


	if (%target == %player)
	{
      	%player.repairTarget = -1;
      	if (GameBase::getDamageLevel(%player) != 0)
		{
                        %player.repairRate = 1.0;
                        %player.repairTarget = %player;
                        Client::sendMessage(%client, 0, "AutoRepair On");
		}
		else
		{
			Client::sendMessage(%client,0,"Nothing in range");
 			Player::trigger(%player, $WeaponSlot, false);
			return;
		}
	}
      else
	{
      	%player.repairTarget = %target;
            %player.repairRate   = 1.5;
            if (getObjectType(%player.repairTarget) == "Player") {
            	%rclient = Player::getClient(%player.repairTarget);
            	%name = Client::getName(%rclient);
            }
            else {
            	%name = GameBase::getMapName(%target);
            	if(%name == "") {
				%name = (GameBase::getDataName(%player.repairTarget)).description;
            	}
            }
            if (GameBase::getDamageLevel(%player.repairTarget) == 0) {
            	Client::sendMessage(%client,0,%name @ " isn't damaged");
                  Player::trigger(%player,$WeaponSlot,false);
                  %player.repairTarget = -1;
                  return;
            }
            if (getObjectType(%player.repairTarget) == "Player") {
            	Client::sendMessage(%rclient,0,"Being repaired by " @ Client::getName(%client));
            }
            Client::sendMessage(%client,0,"Repairing " @ %name);
	}
      %rate = GameBase::getAutoRepairRate(%player.repairTarget) + %player.repairRate;
      GameBase::setAutoRepairRate(%player.repairTarget,%rate);
	schedule("UltraRepairBolt::checkDone(" @ %this @ "," @ %player @ ");",1);
}

function UltraRepairBolt::onRelease(%this, %player)
{
        %object = %player.repairTarget;
        if (%object != -1) {
                %client = Player::getClient(%player);
                if (%object == %player) {
                        Client::sendMessage(%client,0,"AutoRepair Off");
                }
                else {
                        if (GameBase::getDamageLevel(%object) == 0) {
                                Client::sendMessage(%client,0,"Repair Done");
                        }
                        else {
                                Client::sendMessage(%client,0,"Repair Stopped");
                        }
                }
                %rate = GameBase::getAutoRepairRate(%object) - %player.repairRate;
      if (%rate < 0)
         %rate = 0;

                GameBase::setAutoRepairRate(%object,%rate);
        }
}

function UltraRepairBolt::checkDone(%this, %player)
{
        if (Player::isTriggered(%player,$WeaponSlot) &&
       Player::getMountedItem(%player,$WeaponSlot) == UltraRepairGun &&
                 %player.repairTarget != -1) {
                %object = %player.repairTarget;
                if (%object == %player) {
                        if (GameBase::getDamageLevel(%player) == 0) {
                                Player::trigger(%player,$WeaponSlot,false);
                               return;
                        }
                }
                else {
                        if (GameBase::getDamageLevel(%object) == 0) {
                                Player::trigger(%player,$WeaponSlot,false);
                                return;
                        }
                }
        }
 	  schedule("UltraRepairBolt::checkDone(" @ %this @ "," @ %player @ ");",1);
}

ItemImageData UltraRepairPackImage
{	shapeFile		= "armorPack";
	mountPoint		= 2;
	weaponType		= 2;  // Sustained
	minEnergy		= 0;
	maxEnergy		= 0;   // Energy used/sec for sustained weapons
  	mountOffset		= { 0, -0.05, 0 };
  	mountRotation	= { 0, 0, 0 };
	firstPerson		= false;
};

ItemData UltraRepairPack
{	description		= "Ultra-Repair Pack";
	shapeFile		= "armorPack";
	className		= "Backpack";
	heading			= "kBackpacks";
	shadowDetailMask = 4;
	imageType		= UltraRepairPackImage;
	price			= 750;
	hudIcon			= "repairpack";
	showWeaponBar	= true;
	hiliteOnActive	= true;
};

ItemImageData SuperR2Image
{	shapeFile		= "armorPack";
	mountPoint		= 2;
	weaponType		= 2;  // Sustained
	minEnergy		= 0;
	maxEnergy		= 0;   // Energy used/sec for sustained weapons
  	mountOffset		= { 0, -0.05, -0.10 };
  	mountRotation	= { 0, 3.14159, -0.015 };
	firstPerson		= false;
};

ItemData SuperR2Pack
{	description		= "Super-Repair Pack";
	shapeFile		= "armorPack";
	className		= "Backpack";
	heading		= "cBackpacks";
	shadowDetailMask = 4;
	imageType		= SuperR2Image;
	hudIcon			= "repairpack";
	showWeaponBar	= true;
	hiliteOnActive	= true;
};

ItemImageData UltraRepairGunImage
{	shapeFile		= "repairgun";
	mountPoint		= 0;
	mountOffset		= { -0.15, 0, 0 };
	mountRotation	= { 0, 1.57, 0};
	weaponType		= 2;	// Sustained
	projectileType	= UltraRepairBolt;
	minEnergy		= 3;
	maxEnergy		= 13;	// Energy used/sec for sustained weapons
	lightType		= 3;	// Weapon Fire
	lightRadius		= 1;
	lightTime		= 1;
	lightColor		= { 0.25, 1, 0.25 };
	sfxActivate		= SoundPickUpWeapon;
	sfxFire			= SoundRepair;
};

ItemData UltraRepairGun
{	description		= "Ultra-Repair Gun";
	shapeFile		= "repairgun";
	className		= "Weapon";
	shadowDetailMask = 4;
	imageType		= UltraRepairGunImage;
	showInventory	= false;
	price			= 150;
};

function UltraRepairPack::onUnmount(%player,%item)
{	if (Player::getMountedItem(%player,$WeaponSlot) == UltraRepairGun) 
	{	Player::unmountItem(%player,$WeaponSlot);
	}
	Player::UnMountItem(%player,$FlagSlot);
}

function UltraRepairPack::onUse(%player,%item)
{	if (Player::getMountedItem(%player,$BackpackSlot) != %item) 
	{	Player::mountItem(%player,%item,$BackpackSlot);
	
	}
	else
	{	Player::mountItem(%player,UltraRepairGun,$WeaponSlot);
	}
}

function UltraRepairPack::onMount(%player,%item)
{	Player::mountItem(%player,SuperR2Pack,$FlagSlot);
}

function UltraRepairPack::onDrop(%player,%item)
{	%client = Player::getClient(%player);
	Client::SendMessage(%client,0,"Unable to DROP this type of pack. Must change armor to sell pack....");
}	

function UltraRepairGun::onMount(%player,%imageSlot)
{	Player::trigger(%player,$BackpackSlot,true);
	Player::trigger(%player,$FlagSlot,true);
}

function UltraRepairGun::onUnmount(%player,%imageSlot)
{	Player::trigger(%player,$BackpackSlot,false);
	Player::trigger(%player,$FlagSlot,false);
}

//******************************************************
//				End Super Repair Pack
//******************************************************
