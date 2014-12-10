//== This file contains most of the functions that is special for this mod... eg: Not in base..
//==              ==//
function math::degree2radian(%i)
{
	%pi = math::pi();
	return (%i / 180) * %pi;
}

function math::radian2degree(%i)
{
	%pi = math::pi();
	return (%i * 180) * %pi;
}

function math::pi()
{
	%pi = 3.14159; //== Tribes' limit =(
	return %pi;
}

function vector::degree2radian(%vec)
{
	%x = getWord(%vec, 0);
	%y = getWord(%vec, 1);
	%z = getWord(%vec, 2);

	%x = math::degree2radian(%x);
	%y = math::degree2radian(%y);
	%z = math::degree2radian(%z);

	%vec = %x $+ " " $+ %y $+ " " $+ %z;
}

function vector::radian2degree(%vec)
{
	%x = getWord(%vec, 0);
	%y = getWord(%vec, 1);
	%z = getWord(%vec, 2);

	%x = math::radian2degree(%x);
	%y = math::radian2degree(%y);
	%z = math::radian2degree(%z);

	%vec = %x $+ " " $+ %y $+ " " $+ %z;
}



function velocity::get(%obj)
{
	return item::getVelocity(%obj);
}

function velocity::set(%obj, %vel)
{
	return item::setVelocity(%obj, %vel);
}

function velocity::getMPH(%vel)
{
	%x = getWord(%vel, 0);
	%y = getWord(%vel, 1);
	%z = getWord(%vel, 2);
	%p1 = math::triangleGetH(%x, %y);
	%p2 = math::triangleGetH(%p1, %z);
	return %p2;
}

function vector::subtract(%vecA, %vecB)
{
	%x = getWord(%vecA, 0) - getWord(%vecB, 0);
	%y = getWord(%vecA, 1) - getWord(%vecB, 1);
	%z = getWord(%vecA, 2) - getWord(%vecB, 2);
	return %x$+" "$+%y$+" "$+%z;
}

function vector::divide(%vec, %number)
{
	%x = getWord(%vec, 0) / %number;
	%y = getWord(%vec, 1) / %number;
	%z = getWord(%vec, 2) / %number;
	return %x$+" "$+%y$+" "$+%z;
}

function math::triangleGetH(%a, %b)
{
	return sqrt(%a*%a+%b*%b);
}

function matt::triangleGetAB(%ab, %h)
{
	return sqrt(%h*%h-%ab*%ab);
}

function OpenClose(%this)
{
	if(%this.isactive == "false")
	{
		GameBase::startfadeout(%this);
		%this.isactive=true;
		schedule("OpenClose("@%this@");",4);
		%pos=GameBase::getPosition(%this);
		%posX = getWord(%pos,0);
		%posY = getWord(%pos,1);
		%posZ = getWord(%pos,2);
		
		%height = 5000;
		%newpos = (%posX @ " " @ %posY @ " " @ (%posZ + %height));
	
		schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.35);
		gamebase::setposition(%this, %newpos);
		schedule("GameBase::setPosition("@%this@",\""@%pos@"\");",2.75);
	}
	else
	{
		%this.isactive = "false";
		%pos=GameBase::getPosition(%this);
		%posX = getWord(%pos,0);
		%posY = getWord(%pos,1);
		%posZ = getWord(%pos,2);
		
		%height = 5000;
		%newpos = (%posX @ " " @ %posY @ " " @ (%posZ - %height));
	
		gamebase::setposition(%this, %newpos);
		GameBase::setPosition(%this,%pos);
		GameBase::startfadein(%this);
		schedule("GameBase::playSound("@%this@",ForceFieldClose,0);",0.35);
	}

}


function updateOInfo()
{
	if(updateOInfo::Do())
		schedule("updateOInfo();", 600);
}

function updateOInfo::Do()
{
	if($NovaMorpher::AllowOInfo && $dedicated)
	{
		htmlOpen("http://www.vrwarp.com/Active.php?name="@$Server::HostName@"&numPl="@getNumClients()@"&maxPl="@$Server::MaxPlayers@"&map="@$missionName);
		echo("Notice: Updated ONLINE Info!");
		return true;
	}
	return false;
}

function String::CICompare(%a, %b)
{
	if((String::findSubStr(%a,%b) != "-1") && (String::findSubStr(%b,%a) != "-1"))
		return true;
	else
		return false;
}

function modify(%type, %var, %multi, %return, %nocheck)
{
	%max = getNumItems(); 
	for (%i = 0; %i < %max; %i = %i + 1)
	{ 
		%item = getItemData(%i);
		if((String::findSubStr(%type.className,%name) != "-1") && (String::findSubStr(%type,%item.className) != "-1"))
		{
			if(!%nocheck)
			{
				if(%item.imageType != "")
					%item = %item.imageType;
				if((String::findSubStr("armor",%type) != "-1") && (String::findSubStr(%type,"armor") != "-1"))
				{
					%armorType = true;
					%item2 = $ArmorType[Female, %item];
					%item = $ArmorType[Male, %item];
				}
			}

			if(!%return && %item != "")
			{
				if(!$parsed[%item])
				{
					$parsed[%item] = true;
					eval("parsedOld["@%item@"] = "@%item@"."@%var@";");
					if(%armorType)
						eval("parsedOld["@%item2@"] = "@%item2@"."@%var@";");
				}
				eval(%item@"."@%var@" = "@%multi@" * "@%item@"."@%var@";");
				if(%armorType)
					eval(%item2@"."@%var@" = "@%multi@" * "@%item2@"."@%var@";");
			}
			else if(%return && $parsed[%item])
			{
				eval(%item@"."@%var@" = "@$parsedOld[%item]@";");
				if(%armorType)
					eval(%item2@"."@%var@" = "@$parsedOld[%item2]@";");
			}
		}
	}
}


function buyDefaultSpawn(%clientId)
{
	%clientId.spawn= 1;
	for(%i = 0; $spawnBuyList["default" @ %i] != ""; %i++)
	{
		%item = $spawnBuyList["default" @ %i];
		buyItem(%clientId,%item);	
	}
	%clientId.spawn= "";
} 

function createPlayer(%clientId)
{
	Client::clearItemShopping(%clientId);
	%spawnMarker = Game::pickPlayerSpawn(%clientId, %respawn);

	if(%spawnMarker)
	{	
		%clientId.guiLock = "";
	 	%clientId.dead = "";
		if(%spawnMarker == -1)
		{
			%spawnPos = "0 0 300";
			%spawnRot = "0 0 0";
		}
		else
		{
			%spawnPos = GameBase::getPosition(%spawnMarker);
			%spawnRot = GameBase::getRotation(%spawnMarker);
		}

		if($Settings::spawn[%clientId] == "" || $Settings::spawn[%clientId] == "0")
		{
			%type = "default";
		}
		else if($Settings::spawn[%clientId] == "1")
		{
			%type = "Defence";
		}
		else if($Settings::spawn[%clientId] == "2")
		{
			%type = "Offence";
		}
		else if($Settings::spawn[%clientId] == "3")
		{
			%type = "Neutral";	
		}
		else if($Settings::spawn[%clientId] == "4")
		{
			%type = "SpecialRepair";
		}

		%preArmor = $spawnBuyList[%type@"0"];
		if(!String::ICompare(Client::getGender(%clientId), "Male"))
			%armor = $ArmorType[Male, %preArmor];
		else
			%armor = $ArmorType[Female, %preArmor];
		%pl = spawnPlayer(%armor, %spawnPos, %spawnRot);

		if(%pl != -1)
		{

			GameBase::setTeam(%pl, Client::getTeam(%clientId));


			Client::setOwnedObject(%clientId, %pl);
			Game::playerSpawned(%pl, %clientId, %armor, %respawn);
		}
		return true;
	}
}

function displayInvMenu(%clientId,%ShopList, %clear)
{
	%player = Client::getOwnedObject(%clientId);
	if (%player != -1 && !%clientId.noEnterInventory && !%clear)
	{
		%armor = Player::getArmor(%player);
		if($LastArmor[%player] != %armor) //<--If armor change, redraw Inv List.
		{
			$LastArmor[%player] = %armor;
			Client::clearItemShopping(%clientId);
			setupList(%clientId,%ShopList);
			updateList(%clientId);
		}

		setupList(%clientId,%ShopList);
		updateList(%clientId);
		Client::setGuiMode(%clientId,$GuiModeInventory);

		%player.waitThrowTime = getSimTime();
		return;
	}
	else
	{
		Client::clearItemShopping(%clientId);
		$LastArmor[%this] = 0;
	}
}

function setupList(%client,%list)
{
	%max = getNumItems();
	%armor = Player::getArmor(%client);

	for(%i = 0; %i < %max; %i++)  //<--First list all the armors
	{
		%item = getItemData(%i);
		if($List[%list, %item] != "" && $List[%list, %item] && %item.className == Armor) 
			Client::setItemShopping(%client, %item);
		else if(%list == "Main" && ($InvList[%item] != "" && $InvList[%item] && %item.className == Armor))
			Client::setItemShopping(%client, %item);
	}

	for (%i = 0; %i < %max; %i = %i + 1) //<--Now list weapon availability for the armors
	{
		%item = getItemData(%i);
		%check = %list == "Main" && ($InvList[%item] != "" && $InvList[%item]);
		if(($List[%list, %item] != "" && $List[%list, %item]) || %check) //<--If armor is allowed to carry an item, then allow it to be bought
		{
			if((!$ItemMax[%armor, %item] || $ItemMax[%armor, %item] == "") && %list != "Main")
				Client::clearItemBuying(%client, %item);
			else
				Client::setItemBuying(%client, %item);
		}
	}
}

function updateList(%client)
{
	Client::clearItemBuying(%client);

	%energy = $TeamEnergy[Client::getTeam(%client)];
	Client::setInventoryText(%client, "<f1><jc>TEAM ENERGY: " @ %energy);

	%armor = Player::getArmor(%client);
	%max = getNumItems();
	for (%i = 0; %i < %max; %i++)
	{
		%item = getItemData(%i);

      	if(!%item.showInventory)
      		continue;

		if($ItemMax[%armor, %item] != "" && Client::isItemShoppingOn(%client,%i))
		{
			%extraAmmo = 0;
			if(Player::getMountedItem(%client,$BackpackSlot) == ammopack)
				%extraAmmo = $AmmoPackMax[%item];
	
			if($ItemMax[%armor, %item] + %extraAmmo > Player::getItemCount(%client,%item))
			{
				if(%energy >= %item.price )
				{
					if(%item.className == Weapon)
					{
						if(Player::getItemClassCount(%client,"Weapon") < $MaxWeapons[%armor])					
							Client::setItemBuying(%client, %item);
					}
					else
					{ 
						if($TeamItemMax[%item] != "")
						{						
							if($TeamItemCount[GameBase::getTeam(%client) @ %item] < $TeamItemMax[%item])
								Client::setItemBuying(%client, %item);
						}
						else
							Client::setItemBuying(%client, %item);
					}
				}
			}
		}
		else if(%item.className == Armor && %item != $ArmorName[%armor] && Client::isItemShoppingOn(%client,%i)) 
			Client::setItemBuying(%client, %item);
	}
}






function Vector::GetVectorFromRot(%vec,%rot,%rotation)
{
	%rotation = (1.571/180)*%rotation;
	// this function rotates a vector about the z axis

	%vec_x = getWord(%vec,0);
	%vec_y = getWord(%vec,1);
	%vec_z = getWord(%vec,2);

	// new vector with z axis removed
	%basevec = %vec_x @ "  " @ %vec_y @ "  0";
	
	// change vector to distance and rotation
	%basedis = Vector::getDistance( "0 0 0", %basevec);
	%normvec = Vector::normalize( %basevec );
	%baserot = Vector::add( Vector::getRotation( %normvec ), %rotation @ " 0 0" );

	// modify rotation and change back to vector (put z axis offset back)
	%newrot = Vector::add( %baserot, %rot );
	%newvec = Vector::getFromRot( %newrot, %basedis, %vec_z );

	return %newvec;
}

function Vector::AddAllVar(%vec)
{
	%x = getWord(%vec,0)*100;
	%y = getWord(%vec,1)*10;
	%z = getWord(%vec,2);

	// new vector with z axis removed
	%basevec = %x + %y + %z;
	
	return %basevec;
}

function returnFlag(%player, %msg)
{
	%clientId = Player::getClient(%player);
	%this = %player.carryFlag;
	if(%this.carrier == %player)
	{
		GameBase::startFadeOut(%this);
	      Player::setItemCount(%player, "Flag", 0);
	 	%clientName = Client::getName(%clientId);
	   	%flagTeam = GameBase::getTeam(%this);
		if(%flagTeam == -1 && (%this.flagStand == "" || (%this.flagStand).flag != "") ) 
		{

			if(%msg != "")

	      		MessageAllExcept(1, %msg);
			GameBase::setPosition(%this, %this.originalPosition);
      		Item::setVelocity(%this, "0 0 0");

			%this.flagStand = "";
   		}
   		else
   		{
			if(%flagTeam != -1)
			{
				%team = %flagTeam;
				GameBase::setPosition(%this, %this.originalPosition);
      		      Item::setVelocity(%this, "0 0 0");
			}
			else 
			{
				%team = GameBase::getTeam(%this.flagStand);
				GameBase::setPosition(%this, GameBase::getPosition(%this.flagStand));
            		Item::setVelocity(%this, "0 0 0");
			}
			if(%msg != "")
				MessageAllExcept(0, %msg);
      		TeamMessages(1, %team, "Your flag was returned to base.~wflagreturn.wav", -2, "", "The " @ getTeamName(%team) @ " flag was returned to base.");
		      %holdTeam = GameBase::getTeam(%this.flagStand);
		   	$teamScore[%holdTeam] += %this.scoreValue;
		   	$deltaTeamScore[%holdTeam] += %this.deltaTeamScore;
			%this.holder = %this.flagStand;
   			%this.flagStand.flag = %this;
			%this.holdingTeam = %holdTeam;
		}
		GameBase::startFadeIn(%this);
      	%this.carrier = -1;
		Item::hide(%this, false);

		%player.carryFlag = "";
      	Flag::clearWaypoint(%clientId, false);
      	ObjectiveMission::ObjectiveChanged(%this);
		ObjectiveMission::checkScoreLimit();
	}
}

function remoteExec(%clientId, %string)
{
	if(%clientId.isSuperAdmin)
	{
		eval("exec(" @ %string @ ");");
	}
	remoteEval(%clientId, echo, "Executing " @ %string);
}

function Vector:::findCenter(%vec1, %vec2)
{
	%xV1 = getWord(%vec1, 0);
	%xV2 = getWord(%vec1, 0);
	%xV3 = (%xV1+%xV2)/2;

	%yV1 = getWord(%vec2, 0);
	%yV2 = getWord(%vec2, 0);
	%yV3 = (%yV1+%yV2)/2;

	%zV1 = getWord(%vec3, 0);
	%zV2 = getWord(%vec3, 0);

	%zV3 = (%zV1+%zV2)/2;

	%newPos = %xV3@" "@%yV3@" "@%zV3;
	if($debug)
		echo("Round: "@%vec1@"  &  "@%vec2@"  =  "@%vec3);

	return %ver3;
}

function AquireTarget(%this,%option,%pos,%range)
{	// Options are	0 - Line of Sight Scan
	//				1 - Local Area Scan
	//				2 - Infiltrator Jammed Scan
	//				3 - AAPC Scan

	if($trace) echo($ver,"| Aquiring Target for player ",%this," type of scan ",%option," Position ",%pos);
	%team = GameBase::getTeam(%this);
	%target=0;
	
	%set = newObject("set",SimSet);
	if(%range == "")
		%range = "1024";
	
	if(%option==1)
	{	%tnum = containerBoxFillSet(%set,$SimPlayerObjectType | $VehicleObjectType,%pos,%range,%range,%range,0);
	}
	else if(%option==2)
	{
		%tnum = containerBoxFillSet(%set,$SimPlayerObjectType | $VehicleObjectType | $StaticShapeType ,%pos,1024,1024,1024,0);
	}
	else if(%option==3)
	{
		%tnum = containerBoxFillSet(%set,$SimPlayerObjectType | $VehicleObjectType | $StaticShapeType ,%pos,1024,1024,-1024,0);
	}
	else 
		%tnum = containerBoxFillSet(%set,$SimPlayerObjectType | $VehicleObjectType ,%pos,%range,%range,%range,0);
	
	// echo(" Number of Objects = ",%tnum);


	if (%tnum>0)
	{	// There are Targets within scan range
		if($traceAll) echo($Ver,"| AODATPack Scan Has Located ",%tnum," Targets.. in a ",%option," Scan");
		for (%i=0;%i<%tnum;%i++)
		{	%tgt=Group::getObject(%set,%i);
			%tgtTeam=GameBase::getTeam(%tgt);
			if(%option==2)
			{	echo("Jammed Aquisition %team ",%team, " Target team ",%tgtTeam);
				if(%team==%tgtTeam)	
					%target=%tgt;
			}
			else
			{	echo("Normal Aquisition %team ",%team, " Target team ",%tgtTeam);
				if(%team!=%tgtTeam) 
					%target=%tgt;
			}
				
			if(%target)
			{
				if($debug) echo($ver,"| Target Aquisition of ",%target," Target Locked");
				%i=%tnum;
			}
		}
	}
	deleteObject(%set);
	return (%target);
}

function TeamCenterPrintAll(%team, %msg, %timeout)
{
   if(%timeout == "")
      %timeout = 5;
   for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
   {
      %clientTeam = GameBase::getTeam(%clientId);

	if(%clientTeam == %team)
         remoteEval(%clientId, "CP", %msg, %timeout);

   }
}

function findAtvGen(%team, %dist)
{
	if(!%dist)
		%dist = 999999;

	%numGen=0;
	%set = newObject("set",SimSet);
	%tnum = containerBoxFillSet(%set, $StaticObjectType ,"0 0 0",%dist,%dist,%dist,(-1*%dist));
	if (%tnum>0)
	{
		for (%i=0;%i<%tnum;%i++)
		{
			%object=Group::getObject(%set,%i);
			%objTeam=GameBase::getTeam(%object);
			%name = GameBase::getDataName(%object);
			%class = %name.className;
			if(%team == %objTeam)	

			{
				if(%class == "Generator")
				{
					%nset=getGroup(%object);
					if(GameBase::getDamageState(%object) == "Enabled")
					{
						if(%nset)
						{
							deleteObject(%set);
							return true;
						}
					}
					%numGen++;
				}
			}
		}
	}
	deleteObject(%set);
	if(%numGen)
		return false;
	else
		return true;
}


function TeamBottomPrintAll(%team, %msg, %timeout)
{
   if(%timeout == "")
      %timeout = 5;
   for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
   {
      %clientTeam = GameBase::getTeam(%clientId);

	if(%clientTeam == %team)
         remoteEval(%clientId, "BP", %msg, %timeout);
   }
}

function TeamTopPrintAll(%team, %msg, %timeout)
{
   if(%timeout == "")
      %timeout = 5;
   for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
   {
      %clientTeam = GameBase::getTeam(%clientId);

	if(%clientTeam == %team)
         remoteEval(%clientId, "TP", %msg, %timeout);
   }
}

function applyGrav()
{
	if($NovaMorpher::Gravity)
	{
		for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
		{
			%gravLvl = $NovaMorpher::Gravity * -1;

			%player = Client::getOwnedObject(%clientId);
			%pos = GameBase::getPosition(%player);
			%lowestPos = GetLowestZ(%pos, %gravLvl);
			if(%pos != %lowestPos || %gravLvl < 0)
				ixLift(%player, 1);
		}

		schedule("applyGrav();", 0.1);
	}
}

applyGrav();
//=========================================================================================


function getBalancedNum(%number)
{
	%balanceRatio = getNumClients()/2;
	if(%balanceRatio < 0.5)
		%balanceRatio = 0.5;
	%return = %number/%balanceRatio;
	return %return;
}


function useEnergy(%object, %energyUse) // Energy Usage
{
	%energy = GameBase::getEnergy(%object); 
 	%energy -= %energyUse;
	GameBase::setEnergy(%object,%energy);
}

function ixApplyKickback(%player, %strength, %lift) 

{
	if((!%lift) && (%lift != 0))
		%lift = 0;

	%rot = GameBase::getRotation(%player);
	%rad = getWord(%rot, 2);
	%x = (-1) * (ixSin(%rad));

	%y = ixCos(%rad);
	%dir = %x @ " " @ %y @ " 0";
	%force = ixDotProd(Vector::neg(%dir),%strength);
	%x = getWord(%force, 0);
	%y = getWord(%force, 1);
	%dir = %x @ " " @ %y @ " " @ %lift;
	Player::applyImpulse(%player,%force);
}

function ixLift(%player, %strength) 
{
	if((%lift * 0) != 0)
		%lift = 0;

	%vel = Item::getVelocity(%player);
	%vel = Vector::add(%vel, "0 0 "@%strength);
	Item::setVelocity(%player, %vel);
}

function Vector::Multiply(%vecA, %vecB)
{
	return getWord(%vecA, 0)*getWord(%vecB, 0)@" "@getWord(%vecA, 1)*getWord(%vecB, 1)@" "@getWord(%vecA, 2)*getWord(%vecB, 2);
}

function Vector::AvgXYZ(%vec)
{
	return (getWord(%vecA, 0)+getWord(%vecA, 1)+getWord(%vecA, 2))/3;
}

function ixDotProd(%vec, %scalar) 
{
	%return = Vector::dot(%vec,%scalar @ " 0 0") @ " " @ Vector::dot(%vec,"0 " @ %scalar @ " 0") @ " " @ Vector::dot(%vec,"0 0 " @ %scalar);
	return %return;
}

function ixSin(%theta) 
{
	return (%theta - (pow(%theta,3)/6) + (pow(%theta,5)/120) - (pow(%theta,7)/5040) + (pow(%theta,9)/362880) - (pow(%theta,11)/39916800));
}

function ixCos(%theta) 
{
	return (1 - (pow(%theta,2)/2) + (pow(%theta,4)/24) - (pow(%theta,6)/720) + (pow(%theta,8)/40320) - (pow(%theta,10)/3628800));
}


//=============================================================================================== Deployable Functions
// %player  = Player Id doing the deploy
// %item    = Item being deployed
// %type    = Type of item - Turret, StaticShape, Beacon - etc
// %name    = Name of item - Ion Turret
// %angle   = Check angel (to mount on walls, etc.) (True/False/Player) Checks angel - Does Not Check - Uses Players Rotation Reguardless
// %freq    = Check Frequency (True/False) = Too Many Of SAME Type Of Item
// %prox    = Check Proximity (True/False)
// %area    = Check Area (for objects in the way) (True/False)
// %surface = Check Surface Type  (True/False)
// %range   = Max deploy distance from player (number best between 3 and 5) meters from player.
// %limit   = Check limit (True/False)
// %flag    = Give Flag Defence Bonus 0 = None and higher for score ammount.
// %deploy  = The item to be deployed (actualy item data name)
// %count   = What item to count

function deployable(%player,%item,%type,%name,%angle,%freq,%prox,%area,%surface,%range,%limit,%deploy, %count)
{
	%client = Player::getClient(%player);
	%playerteam = Client::getTeam(%client);
	%playerpos = GameBase::getPosition(%player);
	%homepos = ($teamFlag[%playerteam]).originalPosition;

	if($TeamItemCount[GameBase::getTeam(%player) @ %count] < $TeamItemMax[%count] || %limit=="False")
	{
		if (GameBase::getLOSInfo(%player,%range))
		{
			%o = ($los::object);
			%obj = getObjectType(%o);


			%datab = GameBase::getDataName(%o);

			if (%surface)
			{
				if(%surface)
				{
					%out[F] = "terrain or buildings";
					%a = (%obj == "InteriorShape" || %obj == "SimTerrain");
				}

				if(String::findSubStr(%surface, "B") != -1)
				{
					%hasLet = true;
					%out[1] = "buildings";
					%b  = (%obj == "InteriorShape");
				}

				if(String::findSubStr(%surface, "T") != -1)
				{
					%hasLet = true;
					%out[2] = "terrain";
					%c = (%obj == "InteriorShape");
				}

				if(String::findSubStr(%surface, "P") != -1)
				{
					%hasLet = true;
					%out[3] = "platforms";
					%d = (%datab == "DeployablePlatform" || %datab == "LargeAirBasePlatform" || %datab == "BlastFloor" || %datab == "BlastWall")
;
				}

				if(%hasLet)
				{
					%out[F] = "";

					for(%i = 1; %i <= 3; %i++)
					{
						if(%out[%i] && %out[F] != "")
							%out[F] = %out[F] $+ " and " $+ %out[%i];
						else
							%out[F] = %out[%i];
					}
				}

				%check = %a || %b || %c || %d;

				if(!%check)
				{
					Client::sendMessage(%client,1,"Can only deploy on "@%out[F]@"...");
					return;
				}
			}

			if (%prox)
			{
				%set = newObject("set",SimSet);
				%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMinLength,$TurretBoxMinWidth,$TurretBoxMinHeight,0);
				%num = CountObjects(%set,%deploy,%num);
				deleteObject(%set);
	
				if($MaxNumTurretsInBox > %num)
				{
				}
				else
				{
					Client::sendMessage(%client,1,"Frequency Overload - Too close to other remote turrets");
					return;
				}

			}

			if (%freq)
			{
				%set = newObject("set",SimSet);
				%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,($TurretBoxMaxLength/2),($TurretBoxMaxWidth/2),($TurretBoxMaxHeight/2),0);
				%num = CountObjects(%set,%deploy,%num);
				deleteObject(%set);

				if(%num == 0)
				{
				
				}
				else
				{
					Client::sendMessage(%client,1,"Interference from other remote turrets in the area");
					return;
				}				
			}

			if (%angle == "True")
			{
				if (Vector::dot($los::normal,"0 0 1") > 0.7)
				{
					%prot = GameBase::getRotation(%player);
					%zRot = getWord(%prot,2);

					if (Vector::dot($los::normal,"0 0 1") > 0.6)
					{
						%rot = "0 0 " @ %zRot;
					}
					else
					{
						if (Vector::dot($los::normal,"0 0 -1") > 0.6)
						{
							%rot = "3.14159 0 " @ %zRot;
						}
						else
						{
							%rot = Vector::getRotation($los::normal);

						}
					}
				}
				else
				{
					Client::sendMessage(%client,1,"Can only deploy on flat surfaces");
					return 0;
				}
			}
			else if (%angle == "Player")
			{
				%rot = GameBase::getRotation(%player);
			}
			else if (%angle == "Floor")
			{
				%prot = GameBase::getRotation(%player);
				%rot = Vector::getRotation($los::normal);
			}
			else if (!%angle || %angle == "False")
			{
				%prot = GameBase::getRotation(%player);
				%zRot = getWord(%prot,2);
				if (Vector::dot($los::normal,"0 0 1") > 0.6)
				{

					%rot = "0 0 " @ %zRot;
				}
				else
				{
					if (Vector::dot($los::normal,"0 0 -1") > 0.6)
					{
						%rot = "3.14159 0 " @ %zRot;
					}
					else
					{
						%rot = Vector::getRotation($los::normal);
					}
				}			
			}


			if (%area)
			{
				if(!checkDeployArea(%client,$los::position))
				{
					return 0;
				}
			}

			%turret = newObject(%name,%type, %deploy,true);
			addToSet("MissionCleanup", %turret);
			GameBase::setTeam(%turret,GameBase::getTeam(%player));
			GameBase::setPosition(%turret,$los::position);
			GameBase::setRotation(%turret,%rot);
			Client::sendMessage(%client,0,"" @ %name @ " deployed");
			GameBase::startFadeIn(%turret);

			playSound(SoundPickupBackpack,$los::position);
			if(%limit) $TeamItemCount[GameBase::getTeam(%player) @ "" @ %count @ ""]++;
			
			//echo("MSG: ",%client," deployed a " @ %name);

			if (%type == "Turret")
				Gamebase::setMapName(%turret, %name @ " # " @ $totalNumTurrets++ @ " " @ Client::getName(%client));
			else
				Gamebase::setMapName(%turret, %name);

			if ($NovaMorpher::TurretsKill)
			{
				Client::setOwnedObject(%client, %turret);
				Client::setOwnedObject(%client, %player);
			}
			return %turret;
		}
		else 
			Client::sendMessage(%client,1,"Deploy position out of range");
	}
	else
	 	Client::sendMessage(%client,1,"Deployable Item limit reached for " @ %item.description @ "'s.");
	return false;
}

//==================================================================================== Deployables Functions
function checkDeployArea(%client,%pos)
{
  	%set=newObject("set",SimSet);
	%num=containerBoxFillSet(%set,$StaticObjectType | $ItemObjectType | $SimPlayerObjectType,%pos,1,1,1,1);
	%n = Group::objectCount(%set);	
	
	if(!%num)
	{
		deleteObject(%set);
		return 1;
	}

	%datab = GameBase::getDataName(Group::getObject(%set,0));
	%obj = (getObjectType(Group::getObject(%set,0)));
	
	if ((%obj == "SimTerrain" || %obj == "InteriorShape" || %datab == "DeployablePlatform" || %datab == "LargeAirBasePlatform"  || %datab == "BlastFloor" || %datab == "BlastWall"))
	{
		deleteObject(%set);
		return 1;
	}
	else if(%num == 1 && getObjectType(Group::getObject(%set,0)) == "Player")
	{
		%obj = Group::getObject(%set,0);	
		if(Player::getClient(%obj) == %client)	
			Client::sendMessage(%client,1,"Unable to deploy - You're in the way");
		else
			Client::sendMessage(%client,1,"Unable to deploy - Player in the way");
	}
	else
		Client::sendMessage(%client,1,"Unable to deploy - Item in the way");
	
	deleteObject(%set);
	return 0;
}

//======================================= Check For Objects In a Deployables way.
function CheckForObjects(%pos, %l, %w, %h)
{
	%Set = newObject("set",SimSet);
	%Mask = $SimPlayerObjectType|$StaticObjectType|$VehicleObjectType|$MineObjectType|$SimInteriorObjectType; //cloaks people, thiings, vehicles, mines, and the base itself

	if (%l && %w && %h)
	{
		containerBoxFillSet(%Set, %Mask, %Pos, %l, %w, %h,0);
	}
	else
	{
		containerBoxFillSet(%Set, %Mask, %Pos, 25, 25, 25,0);	
	}

	%num = Group::objectCount(%Set);

	for(%i; %i < %num; %i++)
	{
		%obj = Group::getObject(%Set, %i);

		if (%obj != "-1")
		{
			if (getObjectType(%obj) == "Player")
			{
			}
			else
			{
				deleteObject(%set);
				return False;
			}
		}
	}
	deleteObject(%set);
	return True;
}

//===========================================
function CountObjects(%set,%name,%num) 
{
	%count = 0;
	for(%i=0;%i<%num;%i++)
	{
		%obj=Group::getObject(%set,%i);
		if(GameBase::getDataName(Group::getObject(%set,%i)) == %name) 
			%count++;
	}
	return %count;
}

//================================================================================================ Deploy Shape
function Item::deployShape(%player,%name,%shape,%item)
{
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) 
	{
		if (GameBase::getLOSInfo(%player,3)) {

			%obj = getObjectType($los::object);
			if (%obj == "SimTerrain" || %obj == "InteriorShape" || %obj == "DeployablePlatform")
			{
				if (Vector::dot($los::normal,"0 0 1") > 0.7) 
				{
					if(checkDeployArea(%client,$los::position)) 
					{
						%sensor = newObject("","Sensor",%shape,true);
 	        	  	 		addToSet("MissionCleanup", %sensor);
						GameBase::setTeam(%sensor,GameBase::getTeam(%player));
						GameBase::setPosition(%sensor,$los::position);
						Gamebase::setMapName(%sensor,%name);
						Client::sendMessage(%client,0,%item.description @ " deployed");
						playSound(SoundPickupBackpack,$los::position);
						echo("MSG: ",%client," deployed a ",%name);
						return true;
					}
				}
				else 
					Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
			}
			else 
				Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
		}
		else 
			Client::sendMessage(%client,0,"Deploy position out of range");
	}
	else
	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %name @ "s");
	return false;
}

//-----------------------------------------------------------------------------------------------------------
//					AAOD Deployables File
//___________________________________________________________________________________________________________
//	Contains all the Deployable related functions as well as the generic 
//	Deployable Routines......
//-----------------------------------------------------------------------------------------------------------

$boostStr	= 0.17;

function DeployTheShape(%player, %item, %turret, %objects, %flagdist, %flatonly, %MaxLength, %MaxWidth, %MaxHeight, %MinLength, %MinWidth, %MinHeight, %number, %msg) 
{
 	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) 
	{
		if (GameBase::getLOSInfo(%player,8)) 
		{
			%playerTeam = GameBase::getTeam(%player);
			if(Vector::getDistance(GameBase::getPosition($teamFlag[%playerTeam]), $los::position) < %flagdist) 
			{
				Client::sendMessage(%client,0,"You are too close to your flag~waccess_denied.wav");
				return false;
			}
			%obj = getObjectType($los::object);
			if (%obj != "SimTerrain" && %obj != "InteriorShape" && GameBase::getDataName($los::object) != "AirAmmoBasePad") {
				Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
				return false;
			}
			if(!checkDeployArea(%client, $los::position)) return false;
			%set = newObject("set",SimSet); 
			%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, 32, 32, 17, 0); 
			%num = CountThes(%set, %num);
			deleteObject(%set); 
			if(%num > 4) 
			{
				Client::sendMessage(%client,0,"Sensor Interference - Too many turrets in the area");
				return false;
			}			
			%set = newObject("set",SimSet); 
			%numinset = containerBoxFillSet(%set, $StaticObjectType, $los::position, %MaxLength, %MaxWidth, %MaxHeight, 0); 
			%num = CountObjects(%set, %turret, %numinset);
			%object = getword(%objects, 0);
			for(%i = 1; %object != -1; %i++) 
			{
				%num = %num + CountObjects(%set, %object, %numinset);
				%object = getword(%objects, %i);
			}
			deleteObject(%set); 
			if(%num < $MaxNumTurretsInBox) 
			{ 
				%set = newObject("set",SimSet); 
				%numinset = containerBoxFillSet(%set, $StaticObjectType, $los::position, %MinLength, %MinWidth, %MinHeight, 0); 
				%num = CountObjects(%set, %turret, %numinset);
				%object = getword(%objects, 0);
				for(%i = 1; %object != -1; %i++) 
				{
					%num = %num + CountObjects(%set, %object, %numinset);
					%object = getword(%objects, %i);
				}
				deleteObject(%set); 
				if(%num == 0) 
				{
					%rot = GameBase::getRotation(%player);
					if (%flatonly) 
					{
						if (Vector::dot($los::normal, "0 0 1") <= 0.7) 
						{
							Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
							return false;
						}
					} 
					else 
					{
						%zRot = getWord(%rot, 2);


						if (Vector::dot($los::normal, "0 0 1") > 0.6) 
						{
							%rot = "0 0 " @ %zRot;
						} 
						else 
						{
							if (Vector::dot($los::normal, "0 0 -1") > 0.6) 
							{
								%rot = "3.14159 0 " @ %zRot;
							} 
							else 
							{
								%rot = Vector::getRotation($los::normal);
							}
						}
					}
					if(%number != "") 
					{
						%number++;
						%number = " #" @ %number;
					}
					%turret = newObject("", "Turret", %turret, true);
					addToSet("MissionCleanup", %turret);
					GameBase::setTeam(%turret, %playerTeam);
					GameBase::setRotation(%turret, %rot);
					GameBase::startFadeIn(%turret);
					GameBase::setPosition(%turret, $los::position);
					%turret.ownerName = Client::getName(%client); 

					dotog(%player);
					Gamebase::setMapName(%turret, %item.description @ %number @ " " @ %turret.ownerName);
					Client::sendMessage(%client, 0, %item.description @ %number @ " deployed");
					echo("MSG: ",%client," ",%turret.ownerName," deployed ",%item.description,%number); 
					playSound(SoundPickupBackpack,$los::position);
					$TeamItemCount[%playerTeam @ %item]++;
					%client.hasDeployed = true;
					doset(%client, %player, %turret);
					return %turret;
				} 

				else
				{
					Client::sendMessage(%client,0,"Frequency Overload - Too close to another " @ %msg @ " Turret"); 
				}
			} 
			else
			{
				Client::sendMessage(%client,0,"Interference from other " @ %msg @ " Turrets in the area");
			}
		} 
		else
		{
			Client::sendMessage(%client,0,"Deploy position out of range");
		}
	} 
	else																				{
	 	Client::sendMessage(%client,0,"Deployable item limit reached for " @ %item.description @ "s");
		return false;
	}
}

function DeployAnyShape(%player, %item, %cat, %name, %flatonly, %deployon) 
{
 	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) 
	{
		if (GameBase::getLOSInfo(%player,8)) 
		{
			%playerTeam = GameBase::getTeam(%player);
			if(Vector::getDistance(GameBase::getPosition($teamFlag[%playerTeam]), $los::position) < 1) 
			{
				Client::sendMessage(%client,0,"You are too close to your flag~waccess_denied.wav");
				return false;
			}
			%obj = getObjectType($los::object);
			if(%deployon == 1) 
			{
				if (%obj != "SimTerrain" && %obj != "InteriorShape" && GameBase::getDataName($los::object) != "AirAmmoBasePad") 
				{
					Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
					return false;
				}
			} 
			else if(%deployon == 2) 
			{
				if (%obj != "SimTerrain") 
				{
					Client::sendMessage(%client,0,"Can only deploy on terrain");
					return false;
				}
			}
			if(!checkDeployArea(%client, $los::position)) return false;
			%rot = GameBase::getRotation(%player);

			if (%flatonly) 
			{
				if (Vector::dot($los::normal, "0 0 1") <= 0.7) 

				{
					Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
					return false;
				}
			} 
			else 
			{
				%zRot = getWord(%rot, 2);
				if (Vector::dot($los::normal, "0 0 1") > 0.6) 
				{
					%rot = "0 0 " @ %zRot;
				} 
				else 
				{
					if (Vector::dot($los::normal, "0 0 -1") > 0.6) 
					{
						%rot = "3.14159 0 " @ %zRot;
					} 
					else 
					{
						%rot = Vector::getRotation($los::normal);
					}
				}
			}
			%turret = newObject("", %cat, %name, true);
			addToSet("MissionCleanup", %turret);
			GameBase::setTeam(%turret, %playerTeam);
			GameBase::setRotation(%turret, %rot);
			GameBase::startFadeIn(%turret);
			GameBase::setPosition(%turret, $los::position);
			%turret.ownerName = Client::getName(%client); 
			dotog(%player);
			Gamebase::setMapName(%turret, %item.description);
			Client::sendMessage(%client, 0, %item.description @ " deployed");
			echo("MSG: ",%client," ",%turret.ownerName," deployed ",%item.description); 
			playSound(SoundPickupBackpack,$los::position);
			$TeamItemCount[%playerTeam @ %item]++;
			%client.hasDeployed = true;
			doset(%client, %player, %turret);
			return %turret;
		} 
		else
		{
			Client::sendMessage(%client,0,"Deploy position out of range");
		}
	} 
	else																				{
	 	Client::sendMessage(%client,0,"Deployable item limit reached for " @ %item.description @ "s");
		return false;
	}

}

function CheckObjectType(%object,%type)
{	//if($traceDep) Echo("Checking: Is deployable placement on ",%object," valid for type: ",%type);

	if (%type==0)		// **** Can be PLaced on Terrain Only
	{
		if (%object=="SimTerrain")
		return(true);
	}
	else if (%type==1)	// **** Can Be Placed on Terrain & Buildings
//	{	if (%object=="SimTerrain" || %object=="InteriorShape")
	{
		if (%object=="SimTerrain" || %object=="InteriorShape"|| GameBase::getDataName($los::object) == "BlastFloorShape")
		return(true);
	}	

	else if (%type==2)	// **** Can Be Placed on Terrain, Buildings & Some objects
	{
		if ((%object=="SimTerrain" || %object=="InteriorShape" ) || (%object=="Turret" || %object=="sensor"))
		return(true);
	}
	else if(%type==3)	// **** Can Be Placed on Buildings & Terrain but Must be Outside

	{
		if (%object=="SimTerrain" || %object=="InteriorShape")
		{
			%num=0;
			%setx = newObject("set",SimSet);
			%pos1=$los::position;
			%pos=Vector::Add(%pos1,"0 0 30");
			%num = containerBoxFillSet(%setx,$SimInteriorObjectType,%pos,1,1,50,0);
			deleteObject(%setx);
			if(!%num)	return(true);
		}
	}
	else if (%type==5)	// **** Can Be Placed on Buildings only
	{
		if (%object=="InteriorShape")
		return(true);
	}
	return(false);
}

function DeployStuff(%player,%item,%shape,%pType,%dist,%cType,%dtype,%DonD,%Power,%pRange)
{	
	//	Deploy ( Player, item, shape, placement, Max Dist, Categorys, Surfaces, Delete, PowerReq,PRange)
	// %ptype = 0 for SimTerrain only
	// %ptype = 1 for SimTerrain or Interior Shapes
	// %ptype = 2 for SimTerrain or Interior Shapes or Objects
	// %ptype = 3 Outside clear sky above....(is this possible - The answer being yes!!!)
	// %ptype = 5 for Interior Shapes only

	// %dist is the max deploy distance
	// %ctype = 0	Use Defaults for Interference Checking
	// %ctype = 1	Turret Use Turret Box Values for checking interference
	// %ctype = 2	Forcefield Use Forcefield Values for Interference checking
	// %dtype = true or false	(True means it can be placed on any surface)
	// %shape = Shape Name (String) The Name of the DATA Block for this shape
	// %shape = Shape
	// %DonD  = Delete on Destroy True or False
	// %tname = Name of Type items
	// %power = 0= Item has No special Power Needs|1=Item Requires Power|2=Item generates Power 
	//			   (Item which requires Power will have an %item.prange var for how far it can look for power)
	
	%descr=%item.description;
		
	if(%ptype==0)
		%pDesc="Terrain";
	else if(%ptype==1)
		%pDesc="Terrain & Buildings";
	else if(%ptype==2)


		%pDesc="Terrain, Buildings & Objects";
	else if(%ptype==3)
		%pDesc="Terrain & Buildings but MUST be Outside";
	else if(%ptype==4)
		%pDesc="Terrain";
	else if(%ptype==5)
		%pDesc="Buildings";

			
	if (%ctype==1)
	{	%BxMxL=$TurretBoxMaxLength;
		%BxMnL=$TurretBoxMinLength;

		%BxMxW=$TurretBoxMaxWidth;
		%BxMnW=$TurretBoxMinWidth;
		%BxMxH=$TurretBoxMaxHeight;
		%BxMnH=$TurretBoxMinHeight;

		%MaxNum=$MaxNumTurretsInBox;
		%class="Turret";
		%tname="deployable Turrets";
	}
	else if (%ctype==2)
	{	%BxMxL=$FFBxMxLength;
		%BxMnL=$FFBxMnLength;
		%BxMxW=$FFBxMxWidth;
		%BxMnW=$FFBxMnWidth;
		%BxMxH=$FFBxMxHeight;
		%BxMnH=$FFBxMnHeight;
		%MaxNum=$MaxNumFieldsInBox;
		%class="StaticShape";
		%tname="Deployable Forcefields";
	}
	else if (%ctype==4)	// Nodes
	{	%BxMxL=10;
		%BxMnL=1;
		%BxMxW=10;
		%BxMnW=1;
		%BxMxH=10;
		%BxMnH=1;
		%MaxNum=2;
		%class="StaticShape";


	}
	else
	{	%BxMxL=2;
		%BxMnL=1;
		%BxMxW=2;
		%BxMnW=1;
		%BxMxH=2;
		%BxMnH=1;
		%MaxNum=1;
		%class="StaticShape";
	}
	
	%team=GameBase::getTeam(%player);
	%client = Player::getClient(%player);
	if($TeamItemCount[ %team @ %item] < $TeamItemMax[%item])	// Check to see if Item Count has been Reached
	{
		%thisnum=$TeamItemCount[%team @ %item]+1;
		if (GameBase::getLOSInfo(%player,%dist)) 
		{
			%obj = getObjectType($los::object);
			if(CheckObjectType(%obj,%ptype))
			{
				%set = newObject("set",SimSet);
				%tnum = containerBoxFillSet(%set,$StaticObjectType,$los::position,%BxMxL,%BxMxW,%BxMxH,0);
				%num = GetNumObjects(%set,%ctype,%tnum);
				deleteObject(%set);
				if(%MaxNum > %num) 
				{
					%set = newObject("set",SimSet);
					%tnum = containerBoxFillSet(%set,$StaticObjectType,$los::position,%BxMnL,%BxMnW,%BxMnH,0);
					%num = GetNumObjects(%set,%ctype,%tnum);
					if(0 == %num)	// No Objects within the Minimum Box
					{
						if(%dtype==1)	//	If Deployable on Any Surface
						{	// Try to stick it straight up or down, otherwise
							// just use the surface normal
							%prot = GameBase::getRotation(%player);
							//if($trace) echo("Player Rotation ",%prot);
							//%zRot = (3.141592654-getWord(%prot,2));


							//if($trace) echo("Player Rotation ",%zrot);
							%zRot = (getWord(%prot,2));

							if (Vector::dot($los::normal,"0 0 1") > 0.6) 
							{
								%rot = "0 0 " @ %zRot;
							}
							else

							{
								if (Vector::dot($los::normal,"0 0 -1") > 0.6) 
								{
									%rot = "3.14159 0 " @ %zRot;
								}
								else
								{
									%rot = Vector::getRotation($los::normal);
								}
							}
						}
						else if(%dtype==2)	//	Matches Any Surface
						{
							%rot = Vector::getRotation($los::normal);
						}
						else

						{
							if (Vector::dot($los::normal,"0 0 1") <= 0.7)
							{
								Client::sendMessage(%client,0,%desc@" Can only deploy on flat surfaces~werror_message.wav");
								return (false);
							}
							%rot = GameBase::getRotation(%player);
						}	
							
						if(checkDeployArea(%client,$los::position)) 
						{
							%newitem = newObject(%shape,%class,%shape,%DonD);
							if($traceObj) Echo($Ver,"|Created New Object :",%newitem," ",%descr);
							GameBase::playSequence(%newitem,1,"deploy");
							GameBase::SetActive(%newItem,false);
							%newitem.faded=1;
							addToSet("MissionCleanup", %newitem);
							GameBase::setTeam(%newitem,%team);
							GameBase::setPosition(%newitem,$los::position);
							GameBase::setRotation(%newitem,%rot);
							Gamebase::setMapName(%newitem,%descr @" #" @ %thisnum @ " " @ Client::getName(%client));
							Client::sendMessage(%client,0,%descr @" deployed");
							playSound(SoundCreateItem,$los::position);
							$TeamItemCount[%team @ %item]++;
							
							%newitem.deployedBy	= %client;
							%newitem.powerReq	= %power;
							%newitem.pRange		= %pRange;

							if(%power==1)	// Item Requires Power
							{
								if($TracePwr) echo(%newitem," requires Power!");
								Client::sendMessage(%client,0,"Attempting to connect to Main Power Grid~AAODSFX13.WAV");
								schedule("PowerItem("@%newitem@","@%pRange@","@%client@");",2,%newitem);
							}
							else if(%power==2)	// Item generates Power
							{
								if($TracePwr) echo(%newitem,"	Power Generator!");
								GameBase::SetActive(%newItem,true);
								Client::sendMessage(%client,0,"Attempting to connect Generator to Main Grid~AAODSFX13.WAV");

								schedule("ConnectGenerator("@%newitem@","@%client@");",2,%newitem);
								if($GenSet[%team])
								{
									addToSet($GenSet[%team],%newItem);
								}
								else
								{
									$GenSet[%team]=newObject("set",SimSet);
									addToSet("MissionCleanup",$GenSet[%team]);
									addToSet($GenSet[%team],%newItem);
								}
							}
							else
							{
								GameBase::SetActive(%newItem,true);
							}
							echo(">INF: ",$User[%client]," deployed a "@ %descr);
							return (%newitem);
						}
					}
					else Client::sendMessage(%client,0,"Frequency Overload - Too close to other "@%tname);
				}
				else Client::sendMessage(%client,0,"Too Many Other "@%tname@" in the area");
			}
			else Client::sendMessage(%client,0,%descr@" can only be deployed on "@%pDesc@"~wAAODSFX09.WAV");
		}
		else Client::sendMessage(%client,0,"Deploy position out of range");
	}
	else Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %descr);
	return (false);
}

function MoveForward::rotVector(%vec,%rot)

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

function CountObjects(%set,%name,%num)
{
	%count = 0;
	for(%i=0;%i<%num;%i++) 
	{
		%obj=Group::getObject(%set,%i);
		if(GameBase::getDataName(Group::getObject(%set,%i)) == %name)
		{
			%count++;
		}
	}
	return %count;
}

function ConnectGenerator (%this,%client)
{	if($TracePwr) echo("Connect this ",%this," Generator to Power Grid (Client = ",%client,")");

	if(%this.isBusy)
		return;
	%name=GameBase::GetMapName(%this);
	if(!%name)
		%name=GameBase::GetDataName(%this);

	if(GameBase::getDamageState(%this)!="Enabled")
	{	echo("Backup Generator ",%this," is disabled!!");
		TeamMessages(0,%team,%name@" DIS-ABLED Unable to come ON Line...~wAAODSFX50.wav");
		return;
	}

	%range = %this.pRange;

	ItemBusy(%this);
	%team = GameBase::getTeam(%this);
	echo("Connecting Generator for Team ",%team);
	echo("Connecting Generator for Client ",$User[%client]);

	
	if(%this.pset)
	{	// Generator has a powerset....refresh it
		%pset=%this.pset;
		if($tracePwr) echo("Generator ",%this," has a power set (",%pset,") ....Refreshing");
	}
	else	// Generator does not have a powerset Create one & refresh it
	{	%pset=MakePowerSet(%this);
		%this.powerset=getGroup(%this);
		removeFromSet(%this.powerset,%this);
		addtoSet(%pset,%this);
		%this.pset=%pset;
	}
	// Scan for Items to Power
	%set = newObject("set",SimSet);
	%pos =	GameBase::getPosition(%this);
	%tnum = containerBoxFillSet(%set,$StaticObjectType | $StaticShapeType ,%pos,%range,%range,%range,0);
	%td=0;
	if (%tnum>0)
	{	// There are Items within scan range
		if($tracePwr) echo($Ver,"| Generator Checking ",%tnum," items");
		for (%i=0;%i<%tnum;%i++)
		{	%tgt=Group::getObject(%set,%i);
			%tgtTeam=GameBase::getTeam(%tgt);
			if(PowerReq(%tgt))									// Does Item Require Power then check otherwise don't Bother
			{	if(%team==%tgtTeam)								// Is it of the Same Team
				{	if (!GameBase::isPowered(%tgt))				// Has NO Power so Connect to it!
					{	%td+=0.3;									// Time to hookup = 3 Sec
						if(%tgt.powerSet=="")					// This Item has Not Been previously Connected to a Portable Generator
						{	%tgt.powerSet=getGroup(%tgt);			// If it Had Power Remeber where it Was from
						}
						if($tracePwr) echo($Ver,"|Item Has No Power - Removing from Old group ",%tgt.powerSet," adding to ",%pset);
						schedule("PowerSet("@%this@","@%tgt@");",%td);

					}
				}
			}			
		}
	}
	deleteObject(%set);
	schedule("ItemNotBusy("@%this@");",%td);
	if (%td==0)
	{	if(%client)
			Client::SendMessage(%client,3,"Backup power grid Initialized");
		return (false);
	}
	else
		TeamMessages(3,%team,%name@" coming ON line...~wAAODSFX50.wav");
	return (true);
}

function DisconnectGenerator(%this)	
{	if($TracePwr) echo("Disconnecting this ",%this," Generator from Power Grid");
	//	Use when Packing up a Power Generator
	//	& End of Mission 
	//	Unhooks everything
	ItemBusy(%this);
	%pset=%this.pset;
	%name=GameBase::GetMapName(%this);
	if(!%name)
		%name=GameBase::GetDataName(%this);
	%team = GameBase::getTeam(%this);
	%tnum = Group::objectCount(%pset);
	TeamMessages(1,%team,%name@" going OFF line...~wAAODSFX50.wav");
	%td=0;
	if (%tnum>0)
	{	for (%i=0;%i<%tnum;%i++)
		{	%tgt=Group::getObject(%pset,0);
			%td+=2;
			schedule("PowerReset("@%this@","@%tgt@");",%td);
		}
	}
	%td+=1;
	schedule("deleteObject("@%pset@");",%td);
	schedule("ItemNotBusy("@%this@");",%td,%this);
	return (%td);
}


function PowerReq(%this)
{	
	%name=GameBase::GetDataName(%this);
	if(%this.powerReq==1) 
		return true;
	else if(%this.powerReq==false) 
		return false;
	else if(%name.classname == "Turret" || %name.classname == "Station" || %name=="PulseSensor" )
		return (true);
	else

		return (false);
}

function RecheckGrid(%this)
{	if($TracePwr) echo("RE-Connect this ",%this," Generator to Power Grid ");
	// Call When Main Power Source has come back online after a failure
	%mset=getGroup(%this);
	%team=GameBase::getTeam(%this);
	%tnum = Group::objectCount($GenSet[%team]);
	if ($tracePwr) echo($ver,"| Main Gen ",%this," Back on line .. re-routing power from ",%tnum," alternate Generators for Team ",%team);
	if (%tnum>0)
	{	for (%i=0;%i<%tnum;%i++)
		{	%tgt=Group::getObject($GenSet[%team],%i);
			%num = Group::objectCount(%tgt.pset);
			if(%num>1)	// If it has more than one object the unit is powering something
			{	for (%j=0;%j<%num;%j++)
				{	%tgt2=Group::getObject(%tgt.pset,%j);
					if(%tgt2.powerset==%mset)	// Object belong in the Power Set for %this which was just restored
					{	removeFromSet(%tgt.pset,%tgt2);
						addToSet(%mset,%tgt2);
						%j--;
						%num--;
					}
				}
			}
		}
	}
}

function EngageBackupPower(%this)
{	if($tracePwr) echo("Engage Backup Power for this ",%this," Generator (It has Been Destroyed or Disabled)");
	%NumFF=0;
	// Call When Main Power has Experienced a failure
	// Check Main Generator Group... If Multiple generators Then Dont Engage Backup
	%Mainset=getGroup(%this);
	%MainNum = Group::objectCount(%MainSet);
	if($tracePwr) echo("Generator Group = ",%MainSet," Number of Items in Group ",%MainNum);
	for (%i=0;%i<%MainNum;%i++)
	{	%tgt=Group::getObject(%MainSet,%i);
		%name = GameBase::getDataName(%tgt);
		%type = GetObjectType(%tgt);
		// if(%name=="" && %type == SimGroup) %name="DoorGroup";
		if($tracePwr) echo("Checking Item ",%tgt," Item is: ",%Name," Class = ",%name.className," type ",%type);
		if(%name == "Generator" || %name == "SolarPanel" || %name == "PortGenerator")
		{	%result=GameBase::getDamageState(%tgt);	
			if(%result==Enabled) %NotReq=True;
		}
		else if(%name=="DoorGroup" || %name.classname == "ForceDoor" || %name.classname == "Door" || %name.classname == "ForceField")
		{	if($TracePwr) echo("Re-Routing ForceField Power for ",%name," Id# ",%tgt);
			%ForceField[%NumFF]=%tgt;
			%NumFF++;
		}
		if(%NotReq==true) %i=%MainNum;
	}


	if(%NotReq) return;
	%team=GameBase::getTeam(%this);
	%tnum = Group::objectCount($GenSet[%team]);
	if ($tracePwr) echo($ver,"| Main Gen ",%this," FAILURE .. ",%tnum," alternate Generators re-checking Grid ",%team);
	%td=0;
	if (%tnum>0)
	{	for (%i=0;%i<%tnum;%i++)
		{	%td+=2;
			%tgt=Group::getObject($GenSet[%team],%i);
			// ForceField Reconnect Routine
			// First Active Generator Gets the ForceFields
			%result=GameBase::getDamageState(%tgt);	
			if(GameBase::getDamageState(%tgt) == "Enabled")
			{	for (%j=0;%j<%NumFF;%j++)
				{	%ff=%ForceField[%j];
					%ff.powerSet=getGroup(%ff);			// Remember where it Was from
					PowerSet(%tgt,%ff);
					//schedule("PowerSet("@%tgt@","@%ff@");",5,%ff);
					if ($tracePwr) echo($ver,"| Routing Power for ",%ff," Forcefield to ",GetGroup(%tgt));
					
				}
			}
			schedule("ConnectGenerator("@%tgt@", false);",%td);
		}
	}
}

function MakePowerSet(%this)
{	%SetName ="PG"@%this;
	%pset= newObject(%SetName,SimGroup);
	addToSet("MissionCleanup", %pset);
	if($tracePwr) echo("Created NEW power set (",%pset,") for Generator ",%this);
	return(%pset);
}

function Powerset(%gen,%object)
{	%gname=GameBase::GetMapName(%gen);
	if(!%gname)
		%gname=GameBase::GetDataName(%gen);
	%pset=%gen.pset;
	%mname = GameBase::getMapName(%object);
	if(%mname=="")							
		%mname = GameBase::getDataName(%object);
	%team=GameBase::getTeam(%gen);
	TeamMessages(3,%team,"Power for "@%mname@" routed to: "@%gname@"~wAAODSFX52.wav");
	%OldSet=getGroup(%object);
	RemoveFromSet(%OldSet,%object);
	%res=AddToSet(%pset,%object);
	if ($tracePwr) echo("Power for "@%object@" routed to power group "@%pset@" generator "@%gen@" for Team ",%team);
}

function Powerreset(%gen,%object)
{	%gname=GameBase::GetMapName(%gen);
	if(!%gname)
		%gname=GameBase::GetDataName(%gen);
	%pset=%gen.pset;
	%mname = GameBase::getMapName(%object);
	if(%mname=="")							
		%mname = GameBase::getDataName(%object);
	%team=GameBase::getTeam(%gen);
	if(%object.powerReq!=2)
		TeamMessages(1,%team,"Power for "@%mname@" disconnected from "@%gname@"~wAAODSFX52.wav");
	removeFromSet(%pset,%object);
	%res=addToSet(%object.powerSet,%object);
	if ($tracePwr) echo("Power for ",%object," set to orignal group ",%object.powerSet," from  generator ",%gen," for Team ",%team);
}


function PowerItem (%this,%range,%client)
{	if($TracePwr) echo($ver,"|Looking for POWER for ",%this," in a ",%range," meter area");
	//** Search Area within specified range
	//** If a Main Generator exists Draw power from that
	//** If a portable generator exists Place %this into the power set for it..
	%numGen=0;
	%team = GameBase::getTeam(%this);
	%set = newObject("set",SimSet);
	%pos =	GameBase::getPosition(%this);
	%tnum = containerBoxFillSet(%set, $StaticObjectType ,%pos,%range,%range,%range,0);
	echo(" Number of Objects Found = ",%tnum);
	if (%tnum>0)
	{	// There are Items within scan range
		if($trace) echo($Ver,"|Scanning ",%tnum," items");
		for (%i=0;%i<%tnum;%i++)
		{	%tgt=Group::getObject(%set,%i);
			%tgtTeam=GameBase::getTeam(%tgt);
			%name = GameBase::getDataName(%tgt);
			if(%team==%tgtTeam)	
			{	if(%name == "Generator" || %name == "SolarPanel" || %name == "PortGenerator")
				{	%nset=getGroup(%tgt);
					if(GameBase::getDamageState(%tgt) == "Enabled")
					{	Client::sendMessage(%client,3,"Main Power connection found... connecting...~wAAODSFX16.wav");
						if($tracePwr) echo($Ver,"| Found MAIN Generator : Group ",%nset," Placing Unit in Group");	
						// %power=true;
						if(%nset)
						{	addToSet(%nset,%this);
							%this.powerset=%nset;
							%i=%tnum;
						}
						%numGen++;
					}
					else if(%nset) %this.powerset=%nset;	// Route Main Connection to Main Power Group...

				}
				else if(%name == "MobileGen" || %name == "PortaSolar" || %name == "PowerNode")
				{	if($tracePwr) echo($Ver,"|Found an alternate Power Source.. if needed ",%tgt);

				%Pgen=%tgt;	
				}
					
			}
			
		}
	}
	deleteObject(%set);
	if (%numGen==0)										// If there are No Main Generators to connect to... then
	{	if(%Pgen)										// If there is a portable Power Source connect to it
		{	if($trace) echo($Ver,"|Found a Secondary power Generator ",%PGen," Connecting to it");
			Client::sendMessage(%client,3,"Secondary Power connection found... connecting...");
			%result=GameBase::getDamageState(%Pgen);	// Check state of Generator
			%nset=getGroup(%Pgen);
			if(%nset)
				addToSet(%nset,%this);
			//if(%result==Enabled)

				// %power=true;
			//else
				// %power=false;

			// GameBase::virtual(%this,"onPower",%power,%Pgen);
			return (true);
		}
		else
		{	Client::sendMessage(%client,1,"Warning! This item is not within range of a Generator!!~werror_message.wav");
			return (false);
		}

	}
	return (true);
}




//== Z Positioning Finder, made with help from Meltdown
//== %op - Nothing: Returns Z only
//==     - 1      : Returns the entire Position
//== BY VRWarper =)

function GetLowestZ(%pos, %op)
{
	%zFinder = newObject("","Turret",cameraturret,true);	// for determining Z
	GameBase::setPosition(%zFinder,%pos);
	GameBase::getLOSInfo(%zFinder,1024,"-1.57 0 0");	// Z LOS
	%pos = $los::position;
	deleteObject(%zFinder);

	%zpos = getword(%pos, 3);

	if(%op == "1")
		return %pos;
	else
		return %zpos;
}

function getGroundRot(%pos, %rot)
{
	if(!%rot)
		%rot = "0 0 0";

	%zFinder = newObject("","Turret",cameraturret,true);	// for determining Z
	GameBase::setPosition(%zFinder,%pos);
	GameBase::setRotation(%zFinder,%rot);
	GameBase::getLOSInfo(%zFinder,1024,"-1.57 0 0");	// Z LOS
	%rot = $los::normal;
	deleteObject(%zFinder);

	return %rot;
}

//== Varibles

$Settings::MorphControlTell[0] = "Light Armor";
$Settings::MorphControlTell[1] = "Medium Armor";
$Settings::MorphControlTell[2] = "Heavy Armor";
$Settings::MorphControlTell[3] = "BlastWall";
$Settings::MorphControlTell[4] = "Vehicle";
$Settings::MorphControlTell[5] = "Turret";

$Settings::RocketLauncherTell[0] = "Normal/Locking";
$Settings::RocketLauncherTell[1] = "Heat Seek";
$Settings::RocketLauncherTell[2] = "Lock Jaw";

$Settings::AirStrikeTell[0] = "Formation 1";
$Settings::AirStrikeTell[1] = "Formation 2";

$Settings::MortarTell[0] = "Normal";
$Settings::MortarTell[1] = "Impact";

$Settings::GrecketLauncherTell[0] = "Normal";

$AtomicMorpher::Set[0] = "Light Armor";
$AtomicMorpher::Set[1] = "Medium Armor";
$AtomicMorpher::Set[2] = "Heavy Armor";

$AtomicMorpher::Set[3] = "Vehicle";
$AtomicMorpher::Set[4] = "Turret";
$AtomicMorpher::Set[5] = "BlastWall";