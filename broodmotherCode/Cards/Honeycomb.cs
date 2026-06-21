using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards;


public class Honeycomb() : broodmotherCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(5m, ValueProp.Move),
        new DynamicVar("CardAmount", 1m),
        new DynamicVar("StickyAmount", 2m)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        var cards = await CardSelectCmd.FromHand(choiceContext,
            Owner,
            new CardSelectorPrefs(SelectionScreenPrompt,
                DynamicVars["CardAmount"].IntValue),
            c => !c.Keywords.Contains(BroodmotherKeywords.Sticky),
            this);
        foreach (var card in cards)
        {
            await HelperMethods.StickyCard(card, DynamicVars["StickyAmount"].IntValue);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}