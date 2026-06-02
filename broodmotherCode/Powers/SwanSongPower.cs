using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace broodmother.broodmotherCode.Powers;


public class SwanSongPower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    private decimal _extraTurns;
    private bool _usedUp = false;

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        _extraTurns = Amount + 1;
        return  Task.CompletedTask;
    }

    public override bool ShouldDieLate(Creature creature)
    {
        if (_usedUp) return true;
        return creature != Owner;
    }
    
    public override async Task AfterPreventingDeath(Creature creature)
    {
        if (creature != Owner) return;
        await CreatureCmd.Heal(Owner, 1m);
    }

    public override async Task AfterSideTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Enemy) return;
        _extraTurns--;
        if (_extraTurns <= 0)
        {
            _usedUp = true;
            await CreatureCmd.Kill(Owner);
        }
    }
}