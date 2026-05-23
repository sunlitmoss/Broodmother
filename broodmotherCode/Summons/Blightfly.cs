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

public class Blightfly : BroodmotherSummonModel
{
    public override int MinInitialHp => 3;
    public override int MaxInitialHp => 3;
    
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        
        MoveState moveState = new MoveState("NOTHING_MOVE", (IReadOnlyList<Creature> _) => Task.CompletedTask, new DebuffIntent());
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
    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (creature != base.Creature) return;
        
        BroodmotherInsectSlots.EmptySlot(SlotIndex);
        
        foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<WeakPower>(choiceContext, hittableEnemy, 1m, base.Creature, null);
        }
    }
    
    
}