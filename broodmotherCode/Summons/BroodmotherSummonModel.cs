using BaseLib.Abstracts;
using broodmother.broodmotherCode.Character;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace broodmother.broodmotherCode.Summons;

public abstract class BroodmotherSummonModel : CustomMonsterModel, IBroodmotherSummon
{
    public int SlotIndex { get; set; } = -1;
    public override LocString Title => new LocString("monsters", "BROODMOTHER-" + GetType().Name.ToUpper() + ".name");
    
    public override CreatureAnimator GenerateAnimator(MegaSprite controller)
    {
        AnimState animState = new AnimState("idle_loop", isLooping: true);
        AnimState state = new AnimState("die");
        CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
        creatureAnimator.AddAnyState("Dead", state);
        return creatureAnimator;
    }
    protected abstract Task OnPassive(ICombatState combatState);
    protected abstract Task OnDeath(PlayerChoiceContext choiceContext);

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side == CombatSide.Player)
            await OnPassive(combatState);
    }

    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature,
        bool wasRemovalPrevented, float deathAnimLength)
    {
        if (creature != base.Creature) return;
        BroodmotherInsectSlots.EmptySlot(SlotIndex);
        await OnDeath(choiceContext);
    }
    
    
}