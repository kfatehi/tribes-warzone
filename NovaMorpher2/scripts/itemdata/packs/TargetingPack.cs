//== Targeting Pack:
//== AIM - Shoot - Use MORTAR/GRENADE weapon :')
$InvList[TargetingPack]				= 0;
$RemoteInvList[TargetingPack]		= 0;

TargetLaserData targetILaser
{
   laserBitmapName   = "Flash01.bmp";

   damageConversion  = 0.0;
   baseDamageType    = 0;

   lightRange        = 2.0;
   lightColor        = { 0.25, 1.0, 0.25 };

   detachFromShooter = true;
};

ItemImageData TargetingPackImage
{
	shapeFile = "shieldPack";
	mountPoint = 2;
	weaponType = 2;  // Sustained
	minEnergy = -2;
	maxEnergy = -2;   // Energy/sec for sustained weapons
	sfxFire = SoundShieldOn;
	firstPerson = false;
};

ItemData TargetingPack
{
	description = "Targeting Pack";
	shapeFile = "shieldPack";
	className = "Backpack";
   heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = TargetingPackImage;
	price = 175;
	hudIcon = "shieldpack";
	showWeaponBar = true;
	hiliteOnActive = true;
   validateShape = false;
   validateMaterials = true;
};

function TargetingPackImage::onActivate(%player,%imageSlot)
{
	Player::trigger(%player,$BackpackSlot,true);
	%player.packOn = true;
	TargetingPack::findTarget(%player);
}

function TargetingPackImage::onDeactivate(%player,%imageSlot)
{
	%player.packOn = false;
	Player::trigger(%player,$BackpackSlot,false);
}

function TargetingPack::findTarget(%player)
{
	%clientId = Player::getClient(%player);
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%clientId.closeEnemy == "")
			%clientId.closeEnemy = %cl;
		else
		{
			%team = Client::getTeam(%clientId);
			%curTeam = Client::getTeam(%cl);

			if(%team != %curTeam)
			{
				%clientPos = GameBase::getPosition(%player);
	
				%closeCl = %clientId.closeEnemy;
				%closePlayer = Client::getOwnedObject(%closeCl);
				%closePos = GameBase::getPosition(%closePlayer);
				%closeDistance = Vector::getDistance(%clientPos, %closePos);

				%curPlayer = Client::getOwnedObject(%cl);
				%curPos = GameBase::getPosition(%curPlayer);
				%curDistance = Vector::getDistance(%clientPos, %curPos);

				if(%curDistance < %closeDistance)
					%clientId.closeEnemy = %cl;
			}
		}
	}

	if(%clientId.closeEnemy == "")
		Client::sendMessage(%clientId, 1, "There are NO players near you!");
	else
	{
		%pos = GameBase::getPosition(%clientId);

		%targetClient = %clientId.closeEnemy;
		%targetName = Client::getName(%targetClient);
		%targetPos = GameBase::getPosition(%targetClient);

		%posX = getWord(%targetPos,0);
		%posY = getWord(%targetPos,1);
		%distance = Vector::getDistance(%pos, %targetPos);
		%string = "Player "@%targetName@" is detected " @ %distance @ "meters away.~wshell_click.wav";

		client::sendMessage(%clientId, 1, %string);
		issueCommand(%clientId, %clientId, 3,%string, %posX, %posY);
	}

	if(%player.packOn)
		schedule("TargetingPack::findTarget("@%player@");", 10, %player);
}

$InvList[TargetingPack] = 1;
$RemoteInvList[TargetingPack] = 1;

$ItemMax[marmor, TargetingPack] = 1;
$ItemMax[mfemale, TargetingPack] = 1;

$ItemMax[larmor, TargetingPack] = 1;
$ItemMax[lfemale, TargetingPack] = 1;

$ItemMax[harmor, TargetingPack] = 1;