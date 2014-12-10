//== Hehhehe, I  wonder who besides me figured to use the same
//== function calls that built the mission list to use to make
//== an auto patch finder ;) A genius idea isnt it?

//== Okay... This i how the importance works, it compares the
//== File type which would be .<importance>.NMPatch.cs and execute
//== the files that are MORE important then the next.
//== This way I can use the original tree and let it execute in the
//== Semi-Right order....

//== Special Required Files ==//
$Patch::Importance[1]	= Special;

//== Weaponary Files ==//
$Patch::Importance[2]	= DamageType;
$Patch::Importance[3]	= Explosion;
$Patch::Importance[4]	= Projection;
$Patch::Importance[5]	= Weapon;
$Patch::Importance[6]	= Spell;

//== Armor Files ==//
$Patch::Importance[7]	= Armor;

//== Pack Files ==//
$Patch::Importance[8]	= Pack;

//== Vehicle Files ==//
$Patch::Importance[9]	= Vehicle;

//== Other Files ==//
$Patch::Importance[10]	= Other;


function Patch::FindNExec()
{
	echo("Running Test...");
	if(Patch::Test())
	{
		%numPatch = 0;
		for(%i = 1; $Patch::Importance[%i] != ""; %i++)
		{
			%extension = "." @ $Patch::Importance[%i] @ ".NMPatch.cs";
			%file = File::findFirst("*" @ %extension);
			while(%file != "")
			{
				if (String::findSubStr(%file,%extension) == -1)
					%file = %file @ %extension;
		
				execOnce(%file);
				%file = File::findNext("*" @ %extension);

				%numPatch++;
			}
		}

		if(%numPatch)
			Patch::SetServerPatched();
	}
	else
	{
		echo("Unable to continue.");
		echo("Error: Failed Test");
	}	
}

function Patch::SetServerPatched()
{
	$ItemFavoritesKey = "NovaMorpherPatched";
	$NovaMorpher::Version = $NovaMorpher::Version @ " *Patched*";
	$NovaMorpher::modList = $NovaMorpher::modList @ " *Patched*";
}

$executionCount = 0;
function execOnce(%file)
{
	if($execOnce[%file])
		return;
	else
		exec(%file);

	$execOnce[%file] = true;
	$execFile[$executionCount] = %file;
}

//== This test function finds AT LEAST 3 files 
//== with cs extension (eg. client.cs) then
//== accepts the technology as working.

function Patch::Test()
{
	echo("Searching FILES");

	%count = 0;
	%file = File::findFirst("*.cs");
	while(%file != "")
	{
		if(%count < 3) //== Ehhhhhh 3 files is enough -- VRWarper
		{
			if (String::findSubStr(%file,".cs") == -1)
				%file = %file @ ".cs";
			echo(%file);
			%count++;

			%file = File::findNext("*.cs");
		}
		else
			%file = ""; //== Does the same... -- VRWarper	
	}
	echo("Total Files Counted: " @ %count);

	if(%count > 0)
		return True;
	else
		return False;
}

//---------------------------------------------------------//

function Patch::AddRI(%ItemA, %ItemB, %Speed)
{
	for(%i = 0; ($Patch::ResupplySpeed[%i] != ""); %i++){}

	$Patch::ResupplyItemA[%i] = %ItemA;
	$Patch::ResupplyItemB[%i] = %ItemB;
	$Patch::ResupplySpeed[%i] = %Speed;
}

function Patch::Resupply(%player,%cnt)
{
	for(%i = 0; ($Patch::ResupplySpeed[%i] != ""); %i++)
	{
		if($Patch::ResupplyItemA[%i] == "null")
			%ReSupplyA = "";
		else
			%ReSupplyA = $Patch::ResupplyItemA[%i];

		if($Patch::ResupplyItemSpeed[%i] > 0)
			%cnt = %cnt + AmmoStation::resupply(%player,%ReSupplyA,$Patch::ResupplyItemB[%i],$Patch::ResupplySpeed[%i]);
	}
	return %cnt;
}
