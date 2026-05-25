using broodmother.broodmotherCode.Character;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace broodmother.broodmotherCode.Summons;

using broodmother.broodmotherCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

public class Blightfly : BroodmotherSummonModel
{
    public override int MinInitialHp => 3;
    public override int MaxInitialHp => 3;

protected override MonsterMoveStateMachine GenerateMoveStateMachine()
{
    MoveState activeState = new MoveState("ACTIVE", (IReadOnlyList<Creature> _) => Task.CompletedTask, new DebuffIntent());
    MoveState dormantState = new MoveState("DORMANT", (IReadOnlyList<Creature> _) => Task.CompletedTask, new SleepIntent());
    
    activeState.FollowUpState = dormantState;
    dormantState.FollowUpState = activeState;
    
    return new MonsterMoveStateMachine(new List<MonsterState> { activeState, dormantState }, activeState);
}
    
    private bool _activeThisTurn = true;
    public override async Task OnPassive(ICombatState combatState)
    {
        if (_activeThisTurn)
        {
            Creature? creature =
                combatState.RunState.Rng.CombatTargets.NextItem(
                    base.CombatState.HittableEnemies.Where(c => c != base.Creature));
            if (creature != null && !(creature.Monster is IBroodmotherSummon))
            {
                await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), creature, 1m, base.Creature, null);
            }
        }
        _activeThisTurn = !_activeThisTurn;
        
    }

    public override async Task OnDeath(PlayerChoiceContext choiceContext)
    {
        foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
        {
            if (!(hittableEnemy.Monster is IBroodmotherSummon))
                await PowerCmd.Apply<WeakPower>(choiceContext, hittableEnemy, 1m, base.Creature, null);
        }
    }
    
}