<?php
echo "====----DataBlock Counter by VRWarper----====\n\n";

$total = 0;
$Explosion = 0;
$Player = 0;
$Item = 0;
$ItemData = 0;
$BulletData = 0;
$Grenade = 0;
$Rocket = 0;
$SeekingRocket = 0;
$Laser = 0;
$TargetingLaser = 0;
$Lightning = 0;
$Repair = 0;
$Unknown = 0;
$numFiles = 0;

function CountScripts($dir)
{
	global $total, $Explosion, $Player, $Item, $ItemData, $BulletData, $Grenade, $Rocket, $SeekingRocket, $Laser, $TargetingLaser, $Lightning, $Repair, $Unknown, $numFiles;

	$count = 0;
	if($dir == "")
	$dir = "./";

	$dp = opendir($dir);
	$toRead;
	$toReadDir;
	
	while($file = readdir($dp)) $filenames[] = $file;
	
	sort($filenames);
	
	for($i = 0; $i < count($filenames); $i++)
	{
		$file = $filenames[$i];
	
		if(is_dir("$dir/$file") && $file != "." && $file != "..")
		{
	
			$toRead[$count]="$dir/$file";
			$toReadDir[$count]=$dir;
			$count++;
		}
		else if($file != "." && $file != ".." && !eregi(".txt",$file) && !eregi(".exe",$file)  && !eregi(".old",$file) && !eregi(".zip",$file) && !eregi(".bat",$file) && !eregi(".php",$file))
		{
			if(!(@$fp = fopen("$dir/$file", "r")))
				die ("Cannot open $dir/$file FILE!!!!!!!!!!\n\n\n");

//			echo "Opening $dir/$file....";
			echo "Opening $file....";

			$CurCount = 0;
			for($Indata = fgets($fp, 2000); !feof($fp); $Indata = fgets($fp, 2000))
			{
				if(eregi("ExplosionData ", $Indata))
				{
					$Explosion++;
					$CurCount++;
				}
				else if(eregi("PlayerData ", $Indata))
				{
					$Player++;
					$CurCount++;
				}
				else if(eregi("ItemData ", $Indata))
				{
					$Item++;
					$CurCount++;
				}
				else if(eregi("ItemImageData ", $Indata))
				{
					$ItemData++;
					$CurCount++;
				}
				else if(eregi("BulletData ", $Indata))
				{
					$BulletData++;
					$CurCount++;
				}
				else if(eregi("GrenadeData ", $Indata))
				{
					$Grenade++;
					$CurCount++;
				}
				else if(eregi("RocketData ", $Indata))
				{
					$Rocket++;
					$CurCount++;
				}
				else if(eregi("SeekingMissileData ", $Indata))
				{
					$SeekingRocket++;
					$CurCount++;
				}
				else if(eregi("LaserData ", $Indata))
				{
					$Laser++;
					$CurCount++;
				}
				else if(eregi("TargetLaserData ", $Indata))
				{
					$TargetingLaser++;
					$CurCount++;
				}
				else if(eregi("LightningData ", $Indata))
				{
					$Lightning++;
					$CurCount++;
				}
				else if(eregi("RepairEffectData ", $Indata))
				{
					$Repair++;
					$CurCount++;
				}
				else if(eregi("Data ", $Indata))
				{
					$Unknown++;
					$CurCount++;
				}
			}

			$total += $CurCount;
			$numFiles++;

			echo " This: $CurCount  Total: $total\n";

			fclose($fp);
		}
	}
	for($i = 0; $toRead[$i] != ""; $i++)
	{
		if(CountScripts($toRead[$i]))
			continue;
	}
}

CountScripts("./scripts");


echo "\n\nThis program concludes that you have:\n";
echo "Number of Files:   $numFiles\n\n";
echo "\n----------------------   " . $total . " DATA BLOCKS!!!!\n\n";
echo "\$Explosion:        $Explosion\n";
echo "\$Player:           $Player\n";
echo "\$Item:             $Item\n";
echo "\$ItemData:         $ItemData\n";
echo "\$BulletData:       $BulletData\n";
echo "\$Grenade:          $Grenade\n";
echo "\$Rocket:           $Rocket\n";
echo "\$SeekingRocket:    $SeekingRocket\n";
echo "\$Laser:            $Laser\n";
echo "\$TargetingLaser:   $TargetingLaser\n";
echo "\$Lightning:        $Lightning\n";
echo "\$Repair:           $Repair\n";
echo "\$Unknown:          $Unknown\n";
?>