using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace broodmother.broodmotherCode.Summons;

public class ShrinkerBeetle : BroodmotherSummonModel
{
    protected override AbstractIntent GetIntent()
    {
        return new SleepIntent();
    }

    public override Task OnPassive(ICombatState combatState, PlayerChoiceContext? choiceContext = null)
    {
        return Task.CompletedTask;
    }

    public override Task OnDeath(PlayerChoiceContext choiceContext)
    {
        return Task.CompletedTask;
    }

    public Creature? Target { get; set; }
    
    public override int MinInitialHp => 3;
    public override int MaxInitialHp => 3;
}