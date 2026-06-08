using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Powers;

public class FesteringFormPower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    private readonly Dictionary<Creature, decimal> _festeringTracker = new();
    
    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer != null &&
            (dealer == Owner ||
             (dealer is
              {
                  IsMonster: true,
                  Monster: IBroodmotherSummon summon
              } && summon.Summoner == Owner.Player)) &&
            props.IsPoweredAttack() &&
            result.TotalDamage > 0)
        {
            var currentInfestation = target.GetPowerAmount<InfestationPower>();
            if (currentInfestation > 0)
            {
                var addedAmount = currentInfestation * (Amount / 100m);
                _festeringTracker.TryAdd(target, 0m);
                _festeringTracker[target] += addedAmount;
                var amountAdded = (int)Math.Floor(_festeringTracker[target]);
                if (amountAdded > 0)
                {
                    _festeringTracker[target] -= amountAdded;
                    await PowerCmd.Apply<InfestationPower>(choiceContext, target, amountAdded, dealer, null);
                }
            }
            else
            {
                await PowerCmd.Apply<InfestationPower>(choiceContext, target, 1m, dealer, null);
            }
        }
    }
    
    public override Task AfterCombatEnd(CombatRoom room)
    {
        _festeringTracker.Clear();
        return base.AfterCombatEnd(room);
    }
}