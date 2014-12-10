//== If there is a patch .CS file, execute it....
if($NovaMorpher::PatchFileDirName != "")
{
	exec($NovaMorpher::PatchFileDirName);
}

function Patch::AddReInit(%name)
{
	for(%i = 0; $init[%i] != ""; %i++){}
	$init[%i] = %name;
}

function ReInitItems()
{
	for(%i = 0; $Init[%i] != ""; %i++)
	{
		for(%cnt = 0; %cnt < 7; %cnt++)
		{
			$TeamItemCount[%cnt @ $Init[%i]] = 0;
		}
	}

	for(%cnt = 0; %cnt < 7; %cnt++)
	{
		$Radar[%cnt] = 0;
	}

	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
	{
		%clientId.TkvSer = '';
		%clientId.isTkVictim = '';
	}

	ResetTeleports();

	return True;
}
//--------------------------------------------------------//
//=== DO NOT MODIFY ANYTHING ABOVE THIS LINE!!!!!!!!! ====//
//--------------------------------------------------------//


Patch::AddReInit("AirStrike");
Patch::AddReInit("AntiSniperTurretPack");
Patch::AddReInit("DepRocketTurretPack");
Patch::AddReInit("UnRepairTurretPack");
Patch::AddReInit("SpyRadarPack");
Patch::AddReInit("MineBot");
Patch::AddReInit("WatcherTurretPack");
Patch::AddReInit("LargeForceFieldPack");
Patch::AddReInit("MediumForceFieldPack");
Patch::AddReInit("SmallForceFieldPack");
Patch::AddReInit("UTFTurretPack");
Patch::AddReInit("LargeOctaPad");
Patch::AddReInit("BlastWallPack");
Patch::AddReInit("MobileInvPack");
Patch::AddReInit("FlashLightning");
Patch::AddReInit("KamakaziAirliner");
Patch::AddReInit("BlastFloorPack");
