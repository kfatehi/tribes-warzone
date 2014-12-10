StaticShapeData Octagon 
{ 
	shapeFile = "elevator16x16_octo"; 
	maxDamage = 40.0; 
	visibleToSensor = true; 
	debrisId = defaultDebrisLarge; 
	explosionId = debrisExpLarge; 
}; 

function Octagon::onDestroyed(%this) 
{ 
	StaticShape::onDestroyed(%this); 
	$TeamItemCount[GameBase::getTeam(%this) @ "LargeOctaPad"]--; 
}


ItemImageData LargeOctaPadImage 
{ 
	shapeFile = "discammo"; 
	mountPoint = 2; 
	mountOffset = { 0, 0, 0.125 };
	mountRotation = { 90, 0, 0 }; 
	mass = 2.5; 
	firstPerson = false; 
}; 

ItemData LargeOctaPad 
{ 
	description = "Large Octagon"; 
	shapeFile = "discammo"; 
	className = "Backpack"; 
	heading = "hPlatforms"; 
	imageType = LargeOctaPadImage; 
	shadowDetailMask = 4; 
	mass = 1.5; 
	elasticity = 0.2; 
	price = 420; 
	hudIcon = "deployable";
	showWeaponBar = true; 
	hiliteOnActive = true; 
}; 

function LargeOctaPad::onUse(%player,%item) 
{
	%client = Player::getClient(%player);
	%teamposa = ($teamFlag[0]).originalPosition;
	%teamposb = ($teamFlag[1]).originalPosition;
	%deplpos = GameBase::getPosition(%player);
	if (Player::getMountedItem(%player,$BackpackSlot) != %item)
		Player::mountItem(%player,%item,$BackpackSlot);
	else
	{
		if (Vector::GetDistance(%deplpos,%teamposa) < 60 || Vector::GetDistance(%deplpos,%teamposb) < 60 )
		{
			Client::sendMessage(%client,0,"You are too close to the flag!");
			Player::mountItem(%player,%item,$BackpackSlot);
			return;
		}
		else
			Player::deployItem(%player,%item);
	}
}

function LargeOctaPad::onDeploy(%player,%item,%pos) 
{ 
	if (LargeOctaPad::deployShape(%player,%item))  
		Player::decItemCount(%player,%item);  
} 

function LargeOctaPad::deployShape(%player,%item) 
{ 
	%client = Player::getClient(%player); 
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) 
	{ 
		%rot = GameBase::getRotation(%player); 
		%phase = newObject("","StaticShape",Octagon,true); 
		addToSet("MissionCleanup", %phase); 
		GameBase::setTeam(%phase,GameBase::getTeam(%player)); 

		%pos = GameBase::getPosition(%player);
		%pos = Vector::add(%pos,"0 0 -0.999999");

		GameBase::setPosition(%phase,%pos); 
		GameBase::setRotation(%phase,%rot); 

		Gamebase::setMapName(%phase,"Large Platform "@ Client::getName(%client)); 
		Client::sendMessage(%client,0,"Large Octagon Deployed"); 
		GameBase::startFadeIn(%phase); 
		playSound(SoundPickupBackpack,$los::position); 
		playSound(ForceFieldOpen,$los::position); 
		$TeamItemCount[GameBase::getTeam(%player) @ "LargeOctaPad"]++; 
		echo("MSG: ",%client," deployed a Large Platform");   
		return true;  
	} 
	else 
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s"); 
	return false; 
} 

$packDiscription[LargeOctaPad] = "A large 16 by 16 platform that is only allowed to be deployed 60m from your flag!.";

$TeamItemMax[LargeOctaPad] = 16;

$InvList[LargeOctaPad] = 1;
$RemoteInvList[LargeOctaPad] = 1;

$ItemMax[marmor, LargeOctaPad] = 1;
$ItemMax[mfemale, LargeOctaPad] = 1;

$ItemMax[harmor, LargeOctaPad] = 0;


