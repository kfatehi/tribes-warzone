$packDiscription[SpringBoard] = "AHHHH!!! B--O--I--N--G!!! It's a spring board that bounces you off of it!";

$TeamItemMax[SpringBoard] = 25;

$InvList[SpringBoard] = 1;
$RemoteInvList[SpringBoard] = 1;

$ItemMax[marmor, SpringBoard] = 1;
$ItemMax[mfemale, SpringBoard] = 1;

$ItemMax[harmor, SpringBoard] = 1;

Patch::AddReInit("SpringBoard");


//-=-=-=-=-=-=-=- Pack -=-=-=-=-=-=-
ItemImageData SpringBoardImage
{
	shapeFile = "ammopack";
	mountPoint = 2;
	mountOffset = { 0, 0, 0.1 };
	mountRotation = { 1.57, 0, 0 };
	firstPerson = false;
};

ItemData SpringBoard
{
	description = "Spring Pad";
	shapeFile = "ammopack";
	className = "Backpack";
	heading = "dDefence";
	imageType = SpringBoardImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 600;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function SpringBoard::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function SpringBoard::onDeploy(%player,%item,%pos)
{
	if (SpringBoard::deployShape(%player,%item)) {
		Player::decItemCount(%player,%item);
	}
}

function SpringBoard::deployShape(%player,%item)
{
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ "SpringBoard"] < $TeamItemMax[SpringBoard]) 
	{
		if(GameBase::getLOSInfo(%player,3)) 
		{
			deployable(%player,%item,"StaticShape","SpringPad",'Floor',False,False,False,False,10,True,"SpringPad", "SpringBoard");
			return true;
		}
		else
			Client::sendMessage(%client,0,"Deploy position out of range");
	}
	else
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");

	return false;
}

 //-=-=-=-=-=-=-=- Object =-=-=-=-=-=-=-

StaticShapeData SpringPad
{
	shapeFile = "plasmawall";
	debrisId = defaultDebrisSmall;
	maxDamage = 2.00;
	isTranslucent = true;
   	description = "Energy Altitude Pad";
	visibleToSensor = true;
};

function SpringPad::onDestroyed(%this)
{
	StaticShape::onDestroyed(%this);
	$TeamItemCount[GameBase::getTeam(%this) @ "SpringPad"]--;
}

function SpringPad::onCollision(%this,%obj)
{
	%c = Player::getClient(%obj);
	%vecVelocity = Item::getVelocity(%obj);
	%rnd = getRandom() * 100;

	 // Check misfires
	if (%rnd <= 50 && %rnd >= 43)
	{
		GameBase::playSound(%this, debrisLargeExplosion, 0);
		Client::SendMessage(%c, 0, "TO THE MOON!");		
		%HMult = 50;
		%ZMax = 200;

		%rnd = floor(getRandom() * 3); 
		if (%rnd == 0) 
		{ MessageAll(0,strcat(Client::getName(%c), " suffers a Launch malfunction.")); } 
		else if (%rnd == 1) 
		{ MessageAll(0,strcat(Client::getName(%c), " hits orbit.")); } 
		else if (%rnd == 2) 
		{ MessageAll(0,strcat(Client::getName(%c), " falls off the edge of the planet.")); }
	}
	else if (%rnd > 80)  // 81-100
	{
		GameBase::playSound(%this, debrisLargeExplosion, 0);
		Client::SendMessage(%c, 0, "K-E-R-S-P-R-O-I-N-G-g-g-g-!-!");
		%HMult = 2;
		%ZMax = 150;
	}
	else
	{
		GameBase::playSound(%this, SoundFireMortar, 0);
		Client::SendMessage(%c, 0, "SPROING!");
		%HMult = 2;
		%ZMax = 45;
	}
	%armor = GameBase::getDataName(%obj);
	%mass = %armor.mass;
	%ratio = %mass/27;
	echo(%ratio);
	%vecNewVelocity = GetWord(%vecVelocity, 0) * %HMult @ " " @ 
	                  GetWord(%vecVelocity, 1) * %HMult @ " " @
	                  %ZMax*%ratio+10;

	Item::setVelocity(%obj, %vecNewVelocity);
}
