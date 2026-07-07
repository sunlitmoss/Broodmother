using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace broodmother.broodmotherCode.Cards;


public class Reclamation() : broodmotherCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("CardAmount", 1m),
        new DynamicVar("DrawAmount", 3m)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CardPileCmd.ShuffleIfNecessary(choiceContext, Owner);
        await CardPileCmd.Add(await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner), Owner, new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars["CardAmount"].IntValue)), PileType.Discard);
        await CardPileCmd.Draw(choiceContext, DynamicVars["DrawAmount"].IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DrawAmount"].UpgradeValueBy(1m);
    }
}