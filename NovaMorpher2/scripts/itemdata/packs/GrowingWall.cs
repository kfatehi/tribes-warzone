//============================================================================ GrowingWall
StaticShapeData GrowingWall
{
	shapeFile = "discb";
	maxDamage = 1.0;
	debrisId = defaultDebrisLarge;
	explosionId = debrisExpLarge;
	description = "GrowingWall";
	damageSkinData = "objectDamageSkins";
};

function GrowingWall::check4Spaces(%this)
{
	%orgPos = GameBase::getPosition(%this);
	%orgRot = GameBase::getRotation(%this);

	//== Start checking!
	%pos =  Vector::rotPos(%orgPos, %orgRot, "4 0 0");
	%check = CheckForObjects(%pos, 1, 1, 0);
	if(%check)
		GrowingWall::mitosis(%this, %pos);
	else
	{
		%pos =  Vector::rotPos(%orgPos, %orgRot, "-4 0 0");
		%check = CheckForObjects(%pos, 1, 1, 0);
		if(%check)
			GrowingWall::mitosis(%this, %pos);
		else
		{
			%pos =  Vector::add(%orgPos, "0 0 4");
			%check = CheckForObjects(%pos, 1, 1, 0);
			if(%check)
				GrowingWall::mitosis(%this, %pos);
			else
			{
				%pos =  Vector::add(%orgPos, "0 0 -4");
				%check = CheckForObjects(%pos, 1, 1, 0);
				if(%check)
					GrowingWall::mitosis(%this, %pos);
			}
		}
	}
	//== Multiply again if wall had openings
	%time = 60+floor(getRandom()*10);
	if(%check)
		%time = 3+floor(getRandom()*100);
	//== Schedule for next "mitosis"
	schedule("GrowingWall::check4Spaces('"@%this@"');",%time,%this);
}

function GrowingWall::onAdd(%this)
{
	schedule("GrowingWall::check4Spaces('"@%this@"');",30,%this);
}

function GrowingWall::mitosis(%this, %dest)
{
	%team = GameBase::getTeam(%this);
	%rot = GameBase::getRotation(%this);

	%inv = newObject("GrowingWall","StaticShape","GrowingWall",true);
         addToSet("MissionCleanup", %inv);
	%rot = GameBase::getRotation(%player); 
	GameBase::setTeam(%inv,GameBase::getTeam(%player));
	GameBase::setPosition(%inv, %dest);
	GameBase::setRotation(%inv,%rot);
	Gamebase::setMapName(%inv,%name);
	playSound(ForceFieldOpen,%dest);
}

function Vector::rotPos(%center, %rot, %dist)
{
	return Vector::add(%center, Vector::rotVector(%dist,%rot));
}

//== from shifter
function Vector::rotVector(%vec,%rot)
{
	// this function rotates a vector about the z axis

	%vec_x = getWord(%vec,0);
	%vec_y = getWord(%vec,1);
	%vec_z = getWord(%vec,2);

	// new vector with z axis removed
	%basevec = %vec_x @ "  " @ %vec_y @ "  0";
	
	// change vector to distance and rotation
	%basedis = Vector::getDistance( "0 0 0", %basevec);
	%normvec = Vector::normalize( %basevec );
	%baserot = Vector::add( Vector::getRotation( %normvec ), "1.571 0 0" );

	// modify rotation and change back to vector (put z axis offset back)
	%newrot = Vector::add( %baserot, %rot );
	%newvec = Vector::getFromRot( %newrot, %basedis, %vec_z );

	return %newvec;
}

//======================================= Check For Objects In a Deployables way.
function CheckForObjects(%pos, %l, %w, %h)
{
	%Set = newObject("set",SimSet);
	%Mask = $SimPlayerObjectType|$StaticObjectType|$VehicleObjectType|$MineObjectType|$SimInteriorObjectType;
	if (%l && %w && %h)
		containerBoxFillSet(%Set, %Mask, %Pos, %l, %w, %h,0);
	else
		containerBoxFillSet(%Set, %Mask, %Pos, 25, 25, 25,0);	

	%num = Group::objectCount(%Set);
	for(%i; %i < %num; %i++)
	{
		%obj = Group::getObject(%Set, %i);
		if (%obj != "-1")
		{
			if (getObjectType(%obj) != "Player")
			{
				deleteObject(%set);
				return False;
			}
		}
	}
	deleteObject(%set);
	return True;
}
//============ Actual Pack Function ============//

ItemImageData GrowingWallPackImage
{
	shapeFile = "AmmoPack";
	mountPoint = 2;
   	mountOffset = { 0, -0.03, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData GrowingWallPack
{
	description = "Growing Wall";
	shapeFile = "AmmoPack";
	className = "Backpack";
    	heading = "hPlatforms";
	imageType = GrowingWallPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 880;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function GrowingWallPack::onUse(%player,%item)
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

function GrowingWallPack::onDeploy(%player,%item,%pos)
{
	if (GrowingWallPack::deployShape(%player,%item)) 
	{
		Player::decItemCount(%player,%item);
	}
}

function GrowingWallPack::deployShape(%player,%item)
{
	deployable(%player,%item,"StaticShape","Growing Wall",False,False,False,False,False,4,True,"GrowingWall", "GrowingWallPack");
}

$packDiscription[GrowingWallPack] = "This special wall literally 'grows' into the space in which it was confined.";

//== Testing uses
$TeamItemMax[GrowingWallPack] = 3000;

$InvList[GrowingWallPack] = 1;
$RemoteInvList[GrowingWallPack] = 0;

$ItemMax[marmor, GrowingWallPack] = 1;
$ItemMax[mfemale, GrowingWallPack] = 1;

