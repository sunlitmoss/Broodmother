using broodmother.broodmotherCode.Cards.InsectCards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace broodmother.broodmotherCode.Cards;

public class KheprisAssurance() : broodmotherCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust, CardKeyword.Ethereal};
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int num = CardPile.MaxCardsInHand - CardPile.GetCards(Owner, PileType.Hand).Count() - 1;
        await CardPileCmd.Shuffle(choiceContext, Owner);
        for (int i = 0; i < num; i++)
        {
            await CardPileCmd.Draw(choiceContext, Owner);
        }
        await ReleaseScarabBeetle.CreateInHand(Owner, CombatState!);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}