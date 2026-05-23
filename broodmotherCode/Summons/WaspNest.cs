using broodmother.broodmotherCode.Character;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
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

public class WaspNest : BroodmotherSummonModel
{
    public override int MinInitialHp => 6;
    public override int MaxInitialHp => 6;
    
    readonly DamageVar _damage = new DamageVar(3m, ValueProp.Move);
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {

        MoveState moveState = new MoveState("NOTHING_MOVE", (IReadOnlyList<Creature> _) => Task.CompletedTask,
            new SingleAttackIntent(3));
        moveState.FollowUpState = moveState;
        return new MonsterMoveStateMachine(new List<MonsterState> {moveState}, moveState);
    }
    


    protected override async Task OnPassive(ICombatState combatState)
    {
        Creature? target = combatState.RunState.Rng.CombatTargets.NextItem(
            combatState.HittableEnemies.Where(c => !(c.Monster is IBroodmotherSummon)));
        if (target != null)
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, _damage, base.Creature);

    }
    protected override async Task OnDeath(PlayerChoiceContext choiceContext)
    {
        foreach (Creature c in base.CombatState.HittableEnemies.ToList())
        {
            if (!(c.Monster is IBroodmotherSummon))
                await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), new[] { c }, 6m, ValueProp.Move, base.Creature);
        }
    }
}