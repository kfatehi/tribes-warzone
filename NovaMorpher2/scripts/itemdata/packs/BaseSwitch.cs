//============================================================================ BaseSwitch
StaticShapeData BaseSwitch
{
	description = "Base Control Switch";
	shapeFile = "tower";
	maxDamage = 0.01;
	debrisId = defaultDebrisLarge;
	explosionId = debrisExpLarge;
	description = "BaseSwitch";
	damageSkinData = "objectDamageSkins";
};

function BaseSwitch::onDestroyed(%this)
{
	BaseSwitch::returnObjects(%this);
	StaticShape::onDestroyed(%this);

	MessageAll(3, "Base Switch #"@%this@" was destroyed, all objects now retain their original team!");
}

function BaseSwitch::onCollision(%this, %object)
{
	BaseSwitch::aquireObjects(%this);

	%thisTeam = GameBase::getTeam(%this);
	%team = GameBase::getTeam(%object);
	if(%thisTeam != %team)
	{
		BaseSwitch::setObjectTeam(%this, %team);

		%clientId = Player::getClient(%object);

		MessageAllExcept(%clientId, 0, Client::getName(%clientId) @ " captured BaseSwitch #" @ %this @ " from the " @ getTeamName(%thisTeam) @ " team!");
		Client::sendMessage(%clientId, 0, "You captured BaseSwitch #" @ %this @ " from the " @ getTeamName(%thisTeam) @ " team!");
	}
}

function BaseSwitch::addObject(%this, %object)
{
	for(%i=0; $BaseSwitch[%this, %i] != ""; %i++){}
	$BaseSwitch[%this, %i] = %object;
	$BaseSwitch[%this, team, %i] = GameBase::getTeam(%object);
	$BaseSwitch[%this, type, %i] = GameBase::getDataName(%object);
}

function BaseSwitch::checkObject(%this, %add)
{
	for(%i=0; $BaseSwitch[%this, %i] != ""; %i++)
	{
		%object = $BaseSwitch[%this, %i];
		if(%add == %object)
			return true;
	}
	return false;
}

function BaseSwitch::eraseArray(%this)
{
	for(%i=0; $BaseSwitch[%this, %i] != ""; %i++)
	{
		$BaseSwitch[%this, %i] = "";
		$BaseSwitch[%this, team, %i] = "";
		$BaseSwitch[%this, type, %i] = "";
	}
	return false;
}

function BaseSwitch::returnObjects(%this)
{
	for(%i=0; $BaseSwitch[%this, %i] != ""; %i++)
	{
		%object = $BaseSwitch[%this, %i];
		%team = $BaseSwitch[%this, team, %i];
		if(GameBase::getDataName(%object) == $BaseSwitch[%this, type, %i])
			GameBase::setTeam(%object, %team);

		$BaseSwitch[%this, %i] = "";
		$BaseSwitch[%this, team, %i] = "";
		$BaseSwitch[%this, type, %i] = "";
	}
}

function BaseSwitch::setObjectTeam(%this, %team)
{
	for(%i=0; $BaseSwitch[%this, %i] != ""; %i++)
	{
		%object = $BaseSwitch[%this, %i];
		GameBase::setTeam(%object, %team);
	}


}

function BaseSwitch::aquireObjects(%this)
{
	if(GameBase::getDamageState(%this) != "Enabled")
		return;


	%Set = newObject("set",SimSet); 
	%Pos = GameBase::getPosition(%this); 
	%Mask = $StaticObjectType|$SimInteriorObjectType;
	containerBoxFillSet(%Set, %Mask, %Pos, 75, 75, 75, 75);
	%num = Group::objectCount(%Set);
	for(%i; %i < %num; %i++)
	{
		%obj = Group::getObject(%Set, %i);
		if(!BaseSwitch::checkObject(%this, %obj))
			BaseSwitch::addObject(%this, %obj);
	}
	deleteObject(%set);
}


//============ Actual Pack Function ============//

ItemImageData BaseSwitchPackImage
{
	shapeFile = "ammopack";
	mountPoint = 2;
   	mountOffset = { 0, -0.03, 0 };
	mass = -5;
	firstPerson = false;
};

ItemData BaseSwitchPack
{
	description = "BaseSwitch";
	shapeFile = "tower";
	className = "Backpack";
    	heading = "iArea Effect";
	imageType = BaseSwitchPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.6;
	price = 880;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function BaseSwitchPack::onUse(%player,%item)
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

function BaseSwitchPack::onDeploy(%player,%item,%pos)
{
	if (BaseSwitchPack::deployShape(%player,%item)) 
	{
		Player::decItemCount(%player,%item);
	}
}

function BaseSwitchPack::deployShape(%player,%item)
{
	%this = deployable(%player,%item,"StaticShape","Base Switch",False,False,False,False,False,4,True,"BaseSwitch", "BaseSwitchPack");
	BaseSwitch::eraseArray(%this);
	BaseSwitch::aquireObjects(%this);
	BaseSwitch::setObjectTeam(%this, GameBase::getTeam(%this));
	return true;
}

$packDiscription[BaseSwitchPack] = "Too lazy to hack but still steal things from the other team? Well we got just the right thing for you! PS: This is one heck of a light object ;)";

$TeamItemMax[BaseSwitchPack] = 2;

$InvList[BaseSwitchPack] = 1;
$RemoteInvList[BaseSwitchPack] = 0;


$ItemMax[borgarmor, BaseSwitchPack] = 1;
$ItemMax[borgfemale, BaseSwitchPack] = 1;
Patch::AddReInit("BaseSwitchPack");
