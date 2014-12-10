MineData Clustergrenade
{
   mass = 0.3;
   drag = 1.0;
   density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
   description = "Clustergrenade";
   shapeFile = "grenade";
   shadowDetailMask = 4;
   explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = 0.3;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

MineData ClusterMinigrenade
{
   mass = 0.3;
   drag = 1.0;
   density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
   description = "Clustergrenade";
   shapeFile = "armorKit";
   shadowDetailMask = 4;
   explosionId = mineExp;
	explosionRadius = 15.0;
	damageValue = 0.25;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

function ClusterGrenade::onUse(%player,%item)
{
	%obj = newObject("","Mine","Clustergrenade");
 	addToSet("MissionCleanup", %obj);
	%client = Player::getClient(%player);
	GameBase::throw(%obj,%player,9 * %client.throwStrength,false);
	%player.throwTime = getSimTime() + 0.5;
}

function Clustergrenade::onAdd(%this)
{
	%data = GameBase::getDataName(%this);
	schedule("ThrowClusters(" @ %this @ ");",1.999,%this);
	schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}

function ClusterMinigrenade::onAdd(%this)
{
	%data = GameBase::getDataName(%this);
	schedule("Mine::Detonate(" @ %this @ ");",1.0,%this);
}

function ThrowClusters(%this)
{
	%obj = newObject("","Mine","ClusterMinigrenade");
	addToSet("MissionCleanup", %obj);
	GameBase::throw(%obj,%this,-2,true);

	%obj = newObject("","Mine","ClusterMinigrenade");
	addToSet("MissionCleanup", %obj);
	GameBase::throw(%obj,%this,-1.5,false);

	%obj = newObject("","Mine","ClusterMinigrenade");
	addToSet("MissionCleanup", %obj);
	GameBase::throw(%obj,%this,-0.5,true);

	%obj = newObject("","Mine","ClusterMinigrenade");
	addToSet("MissionCleanup", %obj);
	GameBase::throw(%obj,%this,0.5,false);

	%obj = newObject("","Mine","ClusterMinigrenade");
	addToSet("MissionCleanup", %obj);
	GameBase::throw(%obj,%this,1.0,true);

	%obj = newObject("","Mine","ClusterMinigrenade");
	addToSet("MissionCleanup", %obj);
	GameBase::throw(%obj,%this,1.5,false);
}
