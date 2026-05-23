using broodmother.broodmotherCode.Character;
using Broodmother.broodmotherCode.Summons;
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

public class WaspNest : BroodmotherSummonModel
{
    public override int MinInitialHp => 6;
    public override int MaxInitialHp => 6;
    
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {

        MoveState moveState = new MoveState("NOTHING_MOVE", (IReadOnlyList<Creature> _) => Task.CompletedTask,
            new SingleAttackIntent(3));
        moveState.FollowUpState = moveState;
        return new MonsterMoveStateMachine(new List<MonsterState> {moveState}, moveState);
    }
    
    public override CreatureAnimator GenerateAnimator(MegaSprite controller)
    {
        AnimState animState = new AnimState("idle_loop", isLooping: true);
        AnimState state = new AnimState("die");
        CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
        creatureAnimator.AddAnyState("Dead", state);
        return creatureAnimator;
    }

    public override async Task BeforeDeath(Creature creature)
    {
        if (creature != base.Creature) return;

        BroodmotherInsectSlots.EmptySlot(SlotIndex);

        foreach (Creature c in base.CombatState.HittableEnemies.ToList())
        {
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), new[] { c }, 6m, ValueProp.Move, base.Creature);
        }
    }
}