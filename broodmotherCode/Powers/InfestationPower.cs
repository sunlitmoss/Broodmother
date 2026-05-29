using BaseLib.Cards.Variables;
using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Powers;

public sealed class InfestationPower : broodmotherPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    private DynamicVar InfestationMultiplier = new("InfestationMultiplier", 1.5m);
    private DynamicVar ResistanceMultiplier = new("ResistanceMultiplier", 100m);

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        InfestationMultiplier,
        ResistanceMultiplier,
        new DisplayVar<InfestationPower>("CalculatedDamage", p => p.CalculateDamage().ToString())
    ];

    public override LocString Description
    {
        get
        {
            var loc = base.Description;
            loc.Add("ResistanceMultiplier", ResistanceMultiplier.BaseValue);
            loc.Add("InfestationMultiplier", InfestationMultiplier.BaseValue);
            return loc;
        }
    }

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (!Owner.HasPower<ResistancePower>())
            await PowerCmd.Apply<ResistancePower>(new ThrowingPlayerChoiceContext(), Owner, Owner.MaxHp / 10m, applier,
                null, true);
    }

    public int CalculateDamage()
    {
        decimal damage = 0;

        damage = Amount * InfestationMultiplier.BaseValue;

        return (int)damage;
    }

    private int ActualDamage()
    {
        if (Amount > Owner.GetPowerAmount<ResistancePower>())
            return CalculateDamage();
        return 0;
    }

    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return [new HealthBarForecastSegment(ActualDamage(), Colors.Goldenrod, HealthBarForecastDirection.FromRight)];
    }

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participant)
    {
        if (side == CombatSide.Enemy)
            if (Amount > Owner.GetPowerAmount<ResistancePower>())
            {
                await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner, CalculateDamage(),
                    ValueProp.Unblockable | ValueProp.Unpowered, null, null);
                await PowerCmd.Apply<ResistancePower>(new ThrowingPlayerChoiceContext(), Owner,
                    (ResistanceMultiplier.BaseValue / 50 - 1) * Owner.GetPowerAmount<ResistancePower>(), Owner, null);
                await PowerCmd.Remove(this);
            }
    }
}