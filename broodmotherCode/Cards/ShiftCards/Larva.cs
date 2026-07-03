using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace broodmother.broodmotherCode.Cards.ShiftCards;

[Pool(typeof(TokenCardPool))]

public class LarvaEnergy() : ShiftCard<LarvaCard>(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<TokenCardPool>();
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    protected override IEnumerable<CardKeyword> AdditionalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }
}


[Pool(typeof(TokenCardPool))]
public class LarvaCard() : ShiftCard<LarvaEnergy>(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<TokenCardPool>();
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override IEnumerable<CardKeyword> AdditionalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}