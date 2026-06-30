using broodmother.broodmotherCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace Broodmother.broodmotherCode.Powers;

public class ShrinkerBeetleShrinkPower : broodmotherPower
{
	public const decimal damageDecrease = 15m;

	private const string _damageDecreaseKey = "DamageDecrease";

	private const string _applierNameKey = "ApplierName";

	public override PowerType Type => PowerType.Debuff;

	public override PowerStackType StackType => PowerStackType.Single;

	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DynamicVar("DamageDecrease", 15m),
		new StringVar("ApplierName")
	];

	public override Task AfterApplied(Creature? applier, CardModel? cardSource)
	{
		if (base.Owner.Monster is Vantom vantom)
		{
			vantom.ScaleTo(0.75f, 0.5f);
		}
		else
		{
			NCombatRoom.Instance?.GetCreatureNode(base.Owner)?.ScaleTo(0.75f, 0.5);
		}
		Creature applier2 = Applier!;
		if (applier2.IsMonster)
		{
			((StringVar)base.DynamicVars["ApplierName"]).StringValue = Applier!.Monster!.Title.GetFormattedText();
		}
		return Task.CompletedTask;
	}

	public override Task AfterRemoved(Creature oldOwner)
	{
		if (oldOwner.Monster is Vantom vantom)
		{
			vantom.ScaleTo(1f, 0.5f);
		}
		else
		{
			NCombatRoom.Instance?.GetCreatureNode(oldOwner)?.ScaleTo(1f, 0.5);
		}
		return Task.CompletedTask;
	}

	public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
	{
		if (!wasRemovalPrevented && creature == Applier)
		{
			await PowerCmd.Remove(this);
		}
	}

	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (Owner != dealer)
		{
			return 1m;
		}
		if (!props.IsPoweredAttack())
		{
			return 1m;
		}
		return (100m - DynamicVars["DamageDecrease"].BaseValue) / 100m;
	}
}
