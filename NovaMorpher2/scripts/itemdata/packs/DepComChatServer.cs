//== This will allow people to chat that are 100 m from it when their gens are down! Very useful!

StaticShapeData PComChatServ
{
	description = "Portable ComChat Server";
	shapeFile = "generator_p";
	className = "radar";
	debrisId = flashDebrisSmall;
	sfxAmbient = SoundGeneratorPower;
	maxDamage = 1.6;
	mapIcon = "M_generator";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpMedium;
	visibleToSensor = true;
	mapFilter = 4;
};







//----------------------------------------------------------------------------
																			
ItemImageData PComChatServPackI
{
	shapeFile = "generator_p";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData PComChatServPack
{
	description = "Dep ComChat Sev";
	shapeFile = "camera";
	className = "Backpack";
   heading = "dDefence";
	imageType = PComChatServPackI;
	shadowDetailMask = 4;
	mass = 0.5;
	elasticity = 0.2;
	price = 1000;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function PComChatServPack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::deployItem(%player,%item);
	}
}

function PComChatServPack::onDeploy(%player,%item,%pos)
{
	if (PComChatServPack::deployShape(%player,%item))
	{
		Player::decItemCount(%player,%item);
	}
}

function PComChatServPack::deployShape(%player,%item)
{
	%team = Client::getTeam(GameBase::getOwnerClient(%player));
	for(%i = 0; $PComChatServ[%team, %i] != ""; %i++){}

	$PComChatServ[%team, %i] = deployable(%player,%item,"StaticShape","Deployable ComChat Server","False","False","False","False","False","6","True", "PComChatServ", "PComChatServPack");
}

Patch::AddReInit(PComChatServPack);
$packDiscription[PComChatServPack] = "Ever wanted to chat when your gens are down? Well we got the solution! Just deploy one of these babies and you can chat when your at most, 100m from it!";

$TeamItemMax[PComChatServPack] = 4; //== Still with-in the "no-lag" zone...

$InvList[PComChatServPack] = 1;
$RemoteInvList[PComChatServPack] = 1;

$ItemMax[marmor, PComChatServPack] = 1;
$ItemMax[mfemale, PComChatServPack] = 1;

$ItemMax[harmor, PComChatServPack] = 1;

Patch::AddReInit("PComChatServPack");
