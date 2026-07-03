using BaseLib.Abstracts;
using Broodmother.broodmotherCode.Summons;
using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Summons;

public abstract class BroodmotherSummonModel : CustomMonsterModel, IBroodmotherSummon
{
    public int SlotIndex { get; set; } = -1;
    public Player? Summoner { get; set; }
    public bool WasConsumed { get; set; } =  false;
    public PlayerChoiceContext? ChoiceContext { get; set; }
    public abstract Task CreateReleaseCard(ICombatState combatState, Player owner);
    
    public override LocString Title => new("monsters", "BROODMOTHER-" + GetType().Name.ToUpper() + ".name");
    
    protected virtual AbstractIntent GetIntent()
    {
        return new SleepIntent();
    }

    public override CreatureAnimator GenerateAnimator(MegaSprite controller)
    {
        var animState = new AnimState("idle_loop", true);
        var state = new AnimState("die");
        var creatureAnimator = new CreatureAnimator(animState, controller);
        creatureAnimator.AddAnyState("Dead", state);
        return creatureAnimator;
    }

    public abstract Task OnPassive(ICombatState combatState, PlayerChoiceContext? choiceContext = null);

    public abstract Task OnDeath(PlayerChoiceContext choiceContext);

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var moveState = new MoveState("NOTHING_MOVE", (IReadOnlyList<Creature> _) => Task.CompletedTask, GetIntent());
        moveState.FollowUpState = moveState;
        return new MonsterMoveStateMachine(new List<MonsterState> { moveState }, moveState);
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == CombatSide.Player)
        {
            if (Creature.CurrentHp > 1)
            {
                await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(),
                    this.Creature,
                    new DamageVar(1m,
                        ValueProp.Move),
                    this.Creature);
                await OnPassive(combatState);
            }
            else
                await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(),
                    this.Creature,
                    new DamageVar(1m,
                        ValueProp.Move),
                    this.Creature);
        }

    }

    public override  async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (creature != Creature) return;
        if (!WasConsumed) await OnDeath(choiceContext);
    }

    public override async Task BeforeDeath(Creature creature)
    {
        if (creature != Creature) return;
        BroodmotherInsectSlots.EmptySlot(SlotIndex);
    }
}