using broodmother.broodmotherCode.Cards.InsectCards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace broodmother.broodmotherCode.Summons;

public class WaspNest : BroodmotherSummonModel
{
    protected override AbstractIntent GetIntent()
    {
        return new SleepIntent();
    }

    public override Task CreateReleaseCard(ICombatState combatState, Player owner) =>
        ReleaseWaspNest.CreateInHand(owner, combatState);

    public override async Task OnPassive(ICombatState combatState, PlayerChoiceContext? choiceContext = null)
    {
        await ReleaseDronewasp.CreateInHand(Summoner!,  combatState);
    }

    public override async Task OnDeath(PlayerChoiceContext choiceContext)
    {
        int openSlots = CardPile.MaxCardsInHand - CardPile.GetCards(Summoner!, PileType.Hand).Count();
        for (int i = 0; i < openSlots; i++)
        {
            await ReleaseDronewasp.CreateInHand(Summoner!, Summoner!.Creature.CombatState!);
        }
    }

    public override int MinInitialHp => 10;
    public override int MaxInitialHp => 10;
}