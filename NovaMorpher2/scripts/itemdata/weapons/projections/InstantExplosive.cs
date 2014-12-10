//Mimic of a mortar except it explodes right away..

ItemData InstantExplosionProjection
{
   description = "InstantExplosive";
   shapeFile = "mortar";
   heading = "yMiscellany";
   shadowDetailMask = 4;
   price = 0;
   className = "HandAmmo";
   validateShape = false;
   validateMaterials = true;
};

function InstantExplosionProjection::onUse(%player,%item)
{
	if($matchStarted)
	{
		%obj = newObject("","Mine","InstantExplosive");
 	 	addToSet("MissionCleanup", %obj);
		%client = Player::getClient(%player);
	}
}

MineData InstantExplosive
{
   mass = 0.3;
   drag = 1.0;
   density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
   description = "Handgrenade";
   shapeFile = "mortar";
   shadowDetailMask = 4;
   explosionId = mortarExp;
	explosionRadius = 10.0;
	damageValue = 1.0;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

function InstantExplosive::onAdd(%this)
{
	%data = GameBase::getDataName(%this);
	schedule("Mine::Detonate(" @ %this @ ");",0.001,%this);
}