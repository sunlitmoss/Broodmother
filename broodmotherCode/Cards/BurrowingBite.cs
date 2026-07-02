using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards;


public class BurrowingBite() : broodmotherCard(0,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(2m, ValueProp.Move),
        new DynamicVar("Cards", 1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BroodmotherKeywords.Bite];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "play.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        await CardPileCmd.Add(await CardSelectCmd.FromCombatPile(choiceContext,
                PileType.Discard.GetPile(base.Owner),
                base.Owner,
                new CardSelectorPrefs(base.SelectionScreenPrompt,
                    0,
                    DynamicVars["Cards"].IntValue)),
            PileType.Draw,
            CardPilePosition.Top);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Cards"].UpgradeValueBy(1m);
    }
}