$packDiscription[AcceleratorDevicePack] = "This enhancer has the ability to speed you up when to pass through it!";

$TeamItemMax[AcceleratorDevicePack] = 5;

$InvList[AcceleratorDevicePack] = 1;
$RemoteInvList[AcceleratorDevicePack] = 1;

$ItemMax[marmor, AcceleratorDevicePack] = 1;
$ItemMax[mfemale, AcceleratorDevicePack] = 1;

Patch::AddReInit("AcceleratorDevicePack");
 //-=-=-=-=-=-=-=- Pack -=-=-=-=-=-=-

ItemImageData AcceleratorDevicePackImage
{
	shapeFile = "ammopack";
	mountPoint = 2;
	mountOffset = { 0, 0, 0.1 };
	mountRotation = { 1.57, 0, 0 };
	firstPerson = false;
};

ItemData AcceleratorDevicePack
{
	description = "Velocity Enhancer";
	shapeFile = "ammopack";
	className = "Backpack";
	heading = "iArea Effect Items";
	imageType = AcceleratorDevicePackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 600;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function AcceleratorDevicePack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function AcceleratorDevicePack::onDeploy(%player,%item,%pos)
{
	if (AcceleratorDevicePack::deployShape(%player,%item)) {
		Player::decItemCount(%player,%item);
	}
}

function AcceleratorDevicePack::deployShape(%player,%item)
{
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ "AcceleratorDevicePack"] >= $TeamItemMax[AcceleratorDevicePack]) 
	{ Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s"); return false; }

	if (!GameBase::getLOSInfo(%player,3)) 
	{ Client::sendMessage(%client,0,"Deploy position out of range"); return false; }

	 //
	 // Passed validation, create the object
	 //
	%objDevice = newObject("Accelerator Device", "Item", AcceleratorDevice, 1, true, true);
	addToSet(MissionCleanup, %objDevice);

	%objDevice.objSide1 = newObject("Accelerator Device mount Alpha", "StaticShape", AcceleratorDeviceSide, true);
	%objDevice.objSide1.objParent = %objDevice;
	addToSet(MissionCleanup, %objDevice.objSide1);

	%objDevice.objMid = newObject("Accelerator Device", "StaticShape", AcceleratorZap, true);
	%objDevice.objMid.objParent = %objDevice;
	addToSet(MissionCleanup, %objDevice.objMid);

	%objDevice.objSide2 = newObject("Accelerator Device mount Beta", "StaticShape", AcceleratorDeviceSide, true);
	%objDevice.objSide2.objParent = %objDevice;
	addToSet(MissionCleanup, %objDevice.objSide2);

	 // Make sure all objects are cleaned up

	 // Make sure it's on our team (not sure this is necessary)
	GameBase::setTeam(%objDevice, GameBase::getTeam(%player));

	 // Place the object wherever you're looking
	%pos = $los::position;

	//== Rotate Zap ==//
	%rot = "1.56 0 " @ getWord(GameBase::getRotation(%player), 2) + 1.56;
	%vec = Vector::getFromRot(%rot, 225);
	%xPos = getWord(%vec, 0) + getWord(%pos, 0) + 0.5;
	%yPos = getWord(%vec, 1) + getWord(%pos, 1);
	%zPos = getWord(%pos, 2)+1;
	%posBeam = %xPos @ " " @ %yPos @ " " @ %zPos;
	%rot = "0 1.56 " @ getWord(GameBase::getRotation(%player), 2);
	GameBase::setPosition(%objDevice.objMid, %posBeam);
	GameBase::setRotation(%objDevice.objMid, %rot);
	//== Rotate Zap ==//
	//== Rotate Zap ==//

	%posBeam = Vector::add(%pos,"0 0 1");
	GameBase::setPosition(%objDevice, %posBeam);

	 // Set values for the sides
	%rot = "1.56 0 " @ getWord(GameBase::getRotation(%player), 2) + 1.56;
	%vec = Vector::getFromRot(%rot, 500);
	%xPos = getWord(%vec, 0) + getWord(%pos, 0);
	%yPos = getWord(%vec, 1) + getWord(%pos, 1);
	%zPos = getWord(%pos, 2)+1;
	%posSide = %xPos @ " " @ %yPos @ " " @ %zPos;
	GameBase::setPosition(%objDevice.objSide1, %posSide);
	GameBase::setRotation(%objDevice.objSide1, %rot);

	%rot = "1.56 0 " @ getWord(GameBase::getRotation(%player), 2) - 1.56;
	%vec = Vector::getFromRot(%rot, 500);
	%xPos = getWord(%vec, 0) + getWord(%pos, 0);
	%yPos = getWord(%vec, 1) + getWord(%pos, 1);
	%zPos = getWord(%pos, 2)+1;
	%posSide = %xPos @ " " @ %yPos @ " " @ %zPos;
	GameBase::setPosition(%objDevice.objSide2, %posSide);
	GameBase::setRotation(%objDevice.objSide2, %rot);

	 // 
	Gamebase::setMapName(%objDevice,"Accelerator Device");
	Client::sendMessage(%client,0,"Accelerator Device Deployed");
	GameBase::startFadeIn(%objDevice);
	playSound(SoundPickupBackpack,$los::position);
	$TeamItemCount[GameBase::getTeam(%player) @ "AcceleratorDevicePack"]++;
        reportDeploy(%objDevice, %client);
	return true;
}

 //-=-=-=-=-=-=-=- Object =-=-=-=-=-=-=-

StaticShapeData AcceleratorZap
{
	shapeFile = "zap";
	maxDamage = 10000.0;
	isTranslucent = true;
	description = "Electrical Beam";
	disableCollision = true;
};

ItemImageData AcceleratorBeamImage
{
	shapeFile = "snowplume";

	mountPoint = 2;
	mountOffset = { 0, 0, 0 };
	mountRotation = { 0, 0, 0.1 };

	lightType = 2;   // Pulsing
	lightRadius = 4;
	lightTime = 1.5;
	lightColor = { 1, 1, 1};	

	firstperson = false;
};

ItemData AcceleratorDevice
{
   	description = "Accelerator Beam";
	shapeFile = "snowplume";
	imageType = AcceleratorBeamImage;
	showInventory = false;
	shadowDetailMask = 4;
};


function AcceleratorDevice::DestroyPad(%this, %pad)
{
	if (%this.objSide1 == %pad)
		%this.objSide1 = "";
	else if (%this.objSide2 == %pad)
		%this.objSide2 = "";
	AcceleratorDevice::onDestroyed(%this);
	deleteObject(%this);
}

function AcceleratorDevice::onDestroyed(%this)
{
	$TeamItemCount[GameBase::getTeam(%this) @ "AcceleratorDevice"]--;
	if (%this.objSide1 != "") schedule("deleteObject(" @ %this.objSide1 @ ");", 1, %this.objSide1);
	if (%this.objSide2 != "") schedule("deleteObject(" @ %this.objSide2 @ ");", 1, %this.objSide2);
	if (%this.objMid != "") schedule("deleteObject(" @ %this.objMid @ ");", 1, %this.objMid);
}

function AcceleratorDevice::onCollision(%this,%obj)
{
    %c = Player::getClient(%obj);

     // See if the accelerator mis-fired
    if (floor(getRandom() * 40) == 0)
    {
         GameBase::playSound(%this, debrisLargeExplosion, 0);
         Client::SendMessage(%c, 0, "Accelerator Malfunction!");
         %velocity = 12;
    }
     // Normal fire
    else
    {
         GameBase::playSound(%this, SoundFireMortar, 0);
         %velocity = 5;
    }

    Item::hide(%this,True);
    schedule("Item::hide(" @ %this @ ",false); GameBase::startFadeIn(" @ %this @ ");",3,%this);

    %vel = Item::getVelocity(%obj);
    %vX = GetWord(%vel, 0) * %velocity;
    %vY = GetWord(%vel, 1) * %velocity;
    %vz = GetWord(%vel, 2) * %velocity;
    %vel = %vX @ " " @ %vY @ " " @ %vZ;
    Item::setVelocity(%obj, %vel);
}

//-=-=-=-=-=-=-=- Accelerator Device Side (staticshape) =-=-=-=-=-=-=-

StaticShapeData AcceleratorDeviceSide
{
        shapeFile = "bridge";
	debrisId = defaultDebrisSmall;
	maxDamage = 4.00;
	visibleToSensor = true;
	isTranslucent = false;
   	description = "Accelerator Device";
};

function AcceleratorDeviceSide::onDestroyed(%this)
{
	StaticShape::onDestroyed(%this);
	AcceleratorDevice::DestroyPad(%this.objParent, %this);
}

