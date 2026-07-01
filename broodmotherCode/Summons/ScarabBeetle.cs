using broodmother.broodmotherCode.Cards.InsectCards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace broodmother.broodmotherCode.Summons;

public class ScarabBeetle : BroodmotherSummonModel
{
    protected override AbstractIntent GetIntent()
    {
        return new SleepIntent();
    }
    public override Task CreateReleaseCard(ICombatState combatState, Player owner) =>
        ReleaseScarabBeetle.CreateInHand(owner, combatState);
    public override Task OnPassive(ICombatState combatState, PlayerChoiceContext? choiceContext = null)
    {
        return Task.CompletedTask;
    }

    public override Task OnDeath(PlayerChoiceContext choiceContext)
    {
        return Task.CompletedTask;
    }
    
    
    public override int MinInitialHp => 10;
    public override int MaxInitialHp => 10;
}