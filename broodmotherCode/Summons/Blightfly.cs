using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace broodmother.broodmotherCode.Summons;

public class Blightfly : BroodmotherSummonModel
{
    public override int MinInitialHp => 3;
    public override int MaxInitialHp => 3;

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

    public override async Task OnPassive(ICombatState combatState)
    {
        if (_activeThisTurn)
        {
            var creature =
                combatState.RunState.Rng.CombatTargets.NextItem(
                    CombatState.HittableEnemies.Where(c => c != Creature));
            if (creature != null && !(creature.Monster is IBroodmotherSummon))
                await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), creature, 1m, Creature, null);
        }

        _activeThisTurn = !_activeThisTurn;
    }

    public override async Task OnDeath(PlayerChoiceContext choiceContext)
    {
        foreach (var hittableEnemy in CombatState.HittableEnemies)
            if (!(hittableEnemy.Monster is IBroodmotherSummon))
                await PowerCmd.Apply<WeakPower>(choiceContext, hittableEnemy, 1m, Creature, null);
    }
}