using broodmother.broodmotherCode.Powers;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace broodmother.broodmotherCode.Summons;

public class Plaguefly : BroodmotherSummonModel
{
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var activeState =
            new MoveState("ACTIVE", (IReadOnlyList<Creature> _) => Task.CompletedTask, new DebuffIntent());
        var dormantState =
            new MoveState("DORMANT", (IReadOnlyList<Creature> _) => Task.CompletedTask, new SleepIntent());

        activeState.FollowUpState = dormantState;
        dormantState.FollowUpState = activeState;

        return new MonsterMoveStateMachine(new List<MonsterState> { activeState, dormantState }, activeState);
    }

    private bool _activeThisTurn = true;
    public override async Task OnPassive(ICombatState combatState, PlayerChoiceContext? choiceContext = null)
    {
        if (_activeThisTurn)
        {
            var creature =
                combatState.RunState.Rng.CombatTargets.NextItem(
                    CombatState.HittableEnemies.Where(c => c != Creature));
            if (creature != null && creature.Monster is not IBroodmotherSummon)
                await PowerCmd.Apply<InfestationPower>(new ThrowingPlayerChoiceContext(), creature, 1m, Creature, null);
        }

        _activeThisTurn = !_activeThisTurn;
        
    }

    public override async Task OnDeath(PlayerChoiceContext choiceContext)
    {
        foreach (var target in CombatState.HittableEnemies.Where(c =>
                     c.Monster is not IBroodmotherSummon && c.HasPower<InfestationPower>()))
        {
            await PowerCmd.Apply<InfestationPower>(choiceContext,
                target,
                target.GetPowerAmount<InfestationPower>(),
                null,
                null);
        }
    }


    public override int MinInitialHp => 5;
    public override int MaxInitialHp => 5;
}