using Broodmother.broodmotherCode.Summons;
using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace broodmother.broodmotherCode.Cards;

public class Cannibalize() : broodmotherCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
        BroodmotherKeywords.Harvest
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
        new EnergyVar(1)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var insects = BroodmotherInsectSlots.Occupants
            .Where(c => c is { Monster: IBroodmotherSummon summon } && summon.Summoner == Owner)
            .ToList();

        foreach (var insect in insects)
        {
            if (!IsUpgraded)
                await HelperMethods.ConsumeInsect(insect!, choiceContext);
            else
                await CreatureCmd.Kill(insect!);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
    }

    protected override void OnUpgrade() =>
        RemoveKeyword(BroodmotherKeywords.Harvest);
}