using BaseLib.Hooks;
using Broodmother.broodmotherCode.Summons;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Powers;

public sealed class InfestationPower : broodmotherPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    private Creature _applier;
    private bool _firstApplication = true;
    private DynamicVar InfestationMultiplier = new("InfestationMultiplier", 33m);
    private DynamicVar DeathExplosion = new("DeathExplosion", 25m);
    private decimal _addedAmount = 0m;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        InfestationMultiplier,
        DeathExplosion
    ];

    
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (!_firstApplication || applier == null) return Task.CompletedTask;
        _applier = applier;
        _firstApplication = false;
        return Task.CompletedTask;
    }

    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return [new HealthBarForecastSegment(Amount, Colors.Goldenrod, HealthBarForecastDirection.FromRight)];
    }

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participant)
    {
        if (side == CombatSide.Enemy)
        {
            await CreatureCmd.Damage(choiceContext,
                Owner, 
                    Amount,
                ValueProp.Unblockable | ValueProp.Unpowered,
                null,
                null);
            _addedAmount += (InfestationMultiplier.BaseValue / 100m) * Amount;
            int delta = (int)Math.Floor(_addedAmount);
            if (delta > 0)
            {
                _addedAmount -= delta;
                await PowerCmd.ModifyAmount(choiceContext, 
                    this, 
                    delta, 
                    null, 
                    null);
            }
        }    
    }

    public override async Task BeforeDeath(Creature creature)
    {
        if (creature != Owner || _applier == null) return;
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(),
            CombatState.HittableEnemies.Where(c => c != Owner && c.Monster is not IBroodmotherSummon),
            new DamageVar(Owner.MaxHp * (DeathExplosion.IntValue / 100m),
            ValueProp.Move), _applier);
        await PowerCmd.Apply<InfestationPower>(new ThrowingPlayerChoiceContext(),
            CombatState.HittableEnemies.Where(c => c != Owner && c.Monster is not IBroodmotherSummon),
            1m,
            null,
            null);
    }
}