$AutoUse[RepairKit] = false;

ItemData RepairKit
{
   description = "Repair Kit";
   shapeFile = "armorKit";
   heading = "yMiscellany";
   shadowDetailMask = 4;
   price = 35;
   validateShape = false;
   validateMaterials = true;
};

function RepairKit::onUse(%player,%item)
{
	%client = Player::getClient(%player);
	%armor = Player::getArmor(%client);

	%maxRepair = %armor.maxDamage*100000000000;

	Player::decItemCount(%player,%item);
	//== Get random amount....
	%repair = floor(getRandom() * %maxRepair);

	//== Now make it so that min:0 max:The full health
	%amount = %repair/100000000000;
	%perPercent = %armor.maxDamage / 100;

	if((%amount/%perPercent) < 10)
		%amount = %perPercent*10;

	%percentRepair = %amount / %perPercent;
	//== Now actually repair....
	GameBase::repairDamage(%player,%amount);

	Client::SendMessage(%client, 3, "The repair kit has repaired " @ floor(%percentRepair) @ "% equivilent of your full health!");
}