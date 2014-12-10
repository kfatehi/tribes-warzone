
//======================================================================== Beacon Rifle
ItemImageData BeaconImage
{
	shapeFile = "sniper";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	projectileType = "Unidentified";
	ammoType = BeaconRifle;
	accuFire = true;
	reloadTime = 0.8;
	fireTime = 0.2;

	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 1, 0, 0 };

	sfxFire = SoundFireLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData BeaconRifle
{
	description = "S/D Rifle";
	className = "Tool"; //== :O
	shapeFile = "sniper";
	hudIcon = "sniper";
   heading = "wToolz";
	shadowDetailMask = 4;
	imageType = BeaconImage;
	price = 444;
	showWeaponBar = true;
   validateShape = false;
   validateMaterials = true;
};

function BeaconImage::onFire(%player, %slot) 
{
	%client = GameBase::getOwnerClient(%player);
	if($TeamItemMax[BeaconRifle] < $TeamItemCount[GameBase::getTeam(%player) @ BeaconRifle])
	{
		Client::sendMessage(%client,0,"Special Item limit reached");
		return;
	}

	if(Player::getItemCount(%player, BeaconRifle) > 0)
	{
		if (GameBase::getLOSInfo(%player,999999))
		{

		if($Settings::BeaconRifle[%client] == "" || $Settings::BeaconRifle[%client] == "0")
		{
				// GetLOSInfo sets the following globals:
				// 	los::position
				// 	los::normal
				// 	los::object
				%obj = getObjectType($los::object);
					// Try to stick it straight up or down, otherwise
					// just use the surface normal
					if (Vector::dot($los::normal,"0 0 1") > 0.6)
					{
						%rot = "0 0 0";
					}
					else
					{
						if (Vector::dot($los::normal,"0 0 -1") > 0.6)
						{
							%rot = "3.14159 0 0";
						}
						else
						{
							%rot = Vector::getRotation($los::normal);
						}
					}
				  	%set=newObject("set",SimSet);
					%num=containerBoxFillSet(%set,$StaticObjectType | $ItemObjectType | $SimPlayerObjectType,$los::position,0.3,0.3,0.3,1);
					deleteObject(%set);
					if(!%num)
					{
						%team = GameBase::getTeam(%player);
						if($TeamItemMax[Beacon] > $TeamItemCount[%team @ Beacon] || $TestCheats)
						{
							%beacon = newObject("Target Beacon", "StaticShape", "DefaultBeacon", true);
							addToSet("MissionCleanup", %beacon);
							//, CameraTurret, true);
							GameBase::setTeam(%beacon,GameBase::getTeam(%player));
							GameBase::setRotation(%beacon,%rot);
							GameBase::setPosition(%beacon,$los::position);
							Gamebase::setMapName(%beacon,"Target Beacon");
   					   		Beacon::onEnabled(%beacon);
							Client::sendMessage(%client,0,"Beacon deployed");
							//playSound(SoundPickupBackpack,$los::position);
							$TeamItemCount[GameBase::getTeam(%beacon) @ "Beacon"]++;
							$TeamItemCount[GameBase::getTeam(%camera) @ "BeaconRifle"]++;

							Player::decItemCount(%player, BeaconRifle, 1);
						}
						else
							Client::sendMessage(%client,0,"Deployable Item limit reached");
					}
					else
						Client::sendMessage(%client,0,"Unable to deploy - Item in the way");
		}
		else if($Settings::BeaconRifle[%client] == 1)
		{
			if (Item::deployShape(%player,"Pulse Sensor",DeployablePulseSensor,PulseSensorPack, 999)) {
				Player::decItemCount(%player, BeaconRifle, 1);
				$TeamItemCount[GameBase::getTeam(%player) @ "PulseSensorPack"]++;
				$TeamItemCount[GameBase::getTeam(%camera) @ "BeaconRifle"]++;
			}
		}
		else if($Settings::BeaconRifle[%client] == 2)
		{
			if($TeamItemCount[GameBase::getTeam(%player) @ MotionSensorPack] < $TeamItemMax[MotionSensorPack]) {
					// GetLOSInfo sets the following globals:
					// 	los::position
					// 	los::normal
					// 	los::object
					%obj = getObjectType($los::object);
						// Try to stick it straight up or down, otherwise
						// just use the surface normal
						%prot = GameBase::getRotation(%player);
						%zRot = getWord(%prot,2);
						if (Vector::dot($los::normal,"0 0 1") > 0.6) {
								%rot = "0 0 " @ %zRot;
						}
							else {
							if (Vector::dot($los::normal,"0 0 -1") > 0.6) {
								%rot = "3.14159 0 " @ %zRot;
							}
							else {
								%rot = Vector::getRotation($los::normal);
							}
						}
						if(checkDeployArea(%client,$los::position)) {
							%mSensor = newObject("","Sensor",DeployableMotionSensor,true);
				   		      addToSet("MissionCleanup", %mSensor);
							GameBase::setTeam(%mSensor,GameBase::getTeam(%player));
							GameBase::setRotation(%mSensor,%rot);
							GameBase::setPosition(%mSensor,$los::position);
							Gamebase::setMapName(%mSensor,"Motion Sensor");
							Client::sendMessage(%client,0,"Motion Sensor deployed");
								playSound(SoundPickupBackpack,$los::position);
							echo("MSG: ",%client," deployed a Motion Sensor");
							$TeamItemCount[GameBase::getTeam(%camera) @ "BeaconRifle"]++;

							Player::decItemCount(%player, BeaconRifle, 1);
						}
			}
			else																						  
			 	Client::sendMessage(%client,0,"Deployable Item limit reached for MotionSensorPack");
		}
		else if($Settings::BeaconRifle[%client] == 3)
		{
			if($TeamItemCount[GameBase::getTeam(%player) @ CameraPack] < $TeamItemMax[CameraPack]) {
					// GetLOSInfo sets the following globals:
					// 	los::position
					// 	los::normal
					// 	los::object
					%obj = getObjectType($los::object);
						// Try to stick it straight up or down, otherwise
						// just use the surface normal
								%prot = GameBase::getRotation(%player);
							%zRot = getWord(%prot,2);
							if (Vector::dot($los::normal,"0 0 1") > 0.6) {
							%rot = "0 0 " @ %zRot;
						}
						else {
							if (Vector::dot($los::normal,"0 0 -1") > 0.6) {
									%rot = "3.14159 0 " @ %zRot;
							}
								else {
								%rot = Vector::getRotation($los::normal);
							}
						}
						if(checkDeployArea(%client,$los::position)) {
							%camera = newObject("Camera","Turret",CameraTurret,true);
				   	      	addToSet("MissionCleanup", %camera);
							GameBase::setTeam(%camera,GameBase::getTeam(%player));
							GameBase::setRotation(%camera,%rot);
							GameBase::setPosition(%camera,$los::position);
							Gamebase::setMapName(%camera,"Camera#"@ $totalNumCameras++ @ " " @ Client::getName(%client));
							Client::sendMessage(%client,0,"Camera deployed");
							playSound(SoundPickupBackpack,$los::position);
							$TeamItemCount[GameBase::getTeam(%camera) @ "CameraPack"]++;
							$TeamItemCount[GameBase::getTeam(%camera) @ "BeaconRifle"]++;
							echo("MSG: ",%client," deployed a Camera");

							Player::decItemCount(%player, BeaconRifle, 1);
						}
			}
			else																						  
			 	Client::sendMessage(%client,0,"Deployable Item limit reached for CameraPacks");

		}
		}
	}
}

Patch::AddReInit("BeaconRifle");

$InvList[BeaconRifle] = 1;
$RemoteInvList[BeaconRifle] = 1;

$ItemMax[marmor, BeaconRifle] = 1;
$ItemMax[mfemale, BeaconRifle] = 1;

$InvList[BeaconRifle] = 1;
$RemoteInvList[BeaconRifle] = 1;

$ItemMax[marmor, BeaconRifle] = 1;
$ItemMax[mfemale, BeaconRifle] = 1;

$AutoUse[BeaconRifle] = True;

$WeaponAmmo[BeaconRifle] = "BeaconRifle";

$TeamItemMax[BeaconRifle] = 10;

AddWeapon(BeaconRifle);