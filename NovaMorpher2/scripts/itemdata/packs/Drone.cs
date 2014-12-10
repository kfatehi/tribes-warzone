//============================================================================ Functions
function Drone::playerSpawn(%clientId, %spawnPos)
{
	%chance = floor(getRandom() * 3)+1;
	if(%chance == "1")
	{
		%type = "default";
	}
	else if(%chance == "2")
	{
		%type = "Defence";
	}
	else
	{
		%type = "Offence";
	}
	%preArmor = $spawnBuyList[%type@"0"];
	if(!String::ICompare(Client::getGender(%clientId), "Male"))
		%armor = $ArmorType[Male, %preArmor];
	else
		%armor = $ArmorType[Female, %preArmor];

	%pl = spawnPlayer(%armor, %spawnPos, GameBase::getRotation(%clientId));
	if(%pl != -1)
	{
		%player = Client::getOwnedObject(%clientId);

		Client::setOwnedObject(%clientId, %pl);
		GameBase::setTeam(%pl, Client::getTeam(%clientId));
		Client::setOwnedObject(%clientId, %player);

		Client::setControlObject(%clientId, %pl);

		%clientId.spawn= 1;
		%max = getNumItems();
		for(%i = 0; %i == %i; %i++)
		{
			%ii = %type @ %i;
			%item = $spawnBuyList[%ii];

			if(%item == "")
				break;

			buyItemSec(%clientId,%pl,%item,%armor);	

			if(%item.className == Weapon) 
				%clientId.spawnWeapon = %item;
		}
		%clientId.spawn= 0;

	}

	GameBase::startFadein(%pl);

	%clientId.drone = %pl;
	%pl.controller = %clientId;
	return %pl;
}

function buyItemSec(%client,%player,%item,%armor)
{
	%clientId = %client;

	%checkCheats = $ServerCheats || $TestCheats;
	%checkA = %checkCheats || Client::isItemShoppingOn(%client,%item) || %client.spawn || %clientId.isSpecialFav;
	%checkB = $ItemMax[%armor, %item] || %item.className == Armor || %item.className == Vehicle || $TestCheats;
	if (%checkA && %checkB)
	{
		if (%item.className == Armor || %item == "SpecialRepair")
		{
			// Assign armor by requested type & gender 
			%buyarmor = $ArmorType[Client::getGender(%client), %item];
			if($debug)
				echo(%buyarmor);
			if(%armor != %buyarmor || Player::getItemCount(%player,%item) == 0)
			{
				teamEnergyBuySell(%player,$ArmorName[%armor].price);
				if(checkResources(%player,%item,1))
				{
					if($Armor::BuySpecial[%armor])
						eval("SellSpecial::"@%armor@"("@%client@");");

					teamEnergyBuySell(%player,$ArmorName[%buyarmor].price * -1);
					Player::setArmor(%player,%buyarmor);
					checkMax(%player,%buyarmor);
					armorChange(%player);

					if($Armor::BuySpecial[%buyarmor])
						eval("BuySpecial::"@%buyarmor@"("@%player@");");

     					Player::setItemCount(%player, %item, 1);  
					return 1;
				}

				teamEnergyBuySell(%player,$ArmorName[%armor].price * -1);
			}
		}
		else if (%item.className == Backpack)
		{
			// Only one backpack per armor.
			teamEnergyBuySell(%player,%item.price * -1);
			Player::incItemCount(%player,%item);
			Player::useItem(%player,%item);									 
			return 1;
		}
		else if(%item.className == Weapon)
		{
			Player::incItemCount(%player,%item);
			teamEnergyBuySell(%player,(%item.price * -1));
			%ammoItem =  %item.imageType.ammoType; 
			if(%ammoItem != "")
			{
				teamEnergyBuySell(%player,(%ammoItem.price * -1 * $ItemMax[%armor, %ammoitem]));
				Player::setItemCount(%player,%ammoitem,$ItemMax[%armor, %ammoitem]);
			}
			return 1;
		}
		else
		{
			%delta = checkResources(%player,%item,$ItemMax[%armor, %item],%armor);
			if(%delta || $testCheats)
			{
				teamEnergyBuySell(%player,(%item.price * -1 * %delta));
				Player::incItemCount(%player,%item,%delta);
				return 1;
			}
		}
		
 	}
	return 0;
}

//============ Actual Pack Function ============//

ItemImageData DronePackImage
{
	shapeFile = "AmmoPack";
	mountPoint = 2;
   	mountOffset = { 0, -0.03, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData DronePack
{
	description = "Telekinesis";
	shapeFile = "AmmoPack";
	className = "Backpack";
    	heading = "cBackpacks";
	imageType = DronePackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.5;
	price = 880;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function DronePack::onUse(%player,%item)
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

function DronePack::onDeploy(%player,%item,%pos)
{
	if (DronePack::deployShape(%player,%item)) 
	{
		Player::decItemCount(%player,%item);
	}
}

function DronePack::deployShape(%player,%item)
{
	%client = Player::getClient(%player);
	%team = GameBase::getTeam(%player);
	if($TeamItemCount[%team @ %item] < $TeamItemMax[%item])
	{
		if (GameBase::getLOSInfo(%player,5))
		{
			%obj = getObjectType($los::object);
			if (%obj == "SimTerrain")
			{
				if (Vector::dot($los::normal,"0 0 1") > 0.7)
				{
					%drone = Drone::playerSpawn(%client, $los::position);

					GameBase::setTeam(%drone, %team);
					GameBase::setPosition(%drone, $los::position);

					Client::sendMessage(%client,1,"You have summoned the dead from hell!");
					echo("MSG: ",%client," summoned the dead!",%name);
					$TeamItemCount[%team @ %item]++;

					return true;
				}
				else 
					Client::sendMessage(%client,0,"This is too steep for you to play your spell on.");
			}
			else 
				Client::sendMessage(%client,0,"You must use this on terrain.");
		}
		else 
			Client::sendMessage(%client,0,"You cannot summon at that far.");
	}
	else
	 	Client::sendMessage(%client,0,"Hell's gates have closed!");
	return false;
}

$packDiscription[DronePack] = "This pack will help you summon the dead for you to control and kill with! Use it wisely....";

$TeamItemMax[DronePack] = 5;

$InvList[DronePack] = 1;
$RemoteInvList[DronePack] = 1;

$ItemMax[magearmor, DronePack] = 1;
$ItemMax[magefemale, DronePack] = 1;


Patch::AddReInit("DronePack");