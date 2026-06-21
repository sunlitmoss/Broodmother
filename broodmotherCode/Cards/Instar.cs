using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace broodmother.broodmotherCode.Cards;


public class Instar() : broodmotherCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        foreach (var card in PileType.Hand.GetPile(Owner).Cards
                     .Where(c => c.Keywords.Contains(BroodmotherKeywords.Shift)).ToList())
        {
            await HelperMethods.ShiftCard(card);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}