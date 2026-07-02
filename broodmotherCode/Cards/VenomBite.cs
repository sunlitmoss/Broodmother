using broodmother.broodmotherCode.Powers;
using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards;


public class VenomBite() : broodmotherCard(0,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BroodmotherKeywords.Bite];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<InfestationPower>(),
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(2m, ValueProp.Move),
        new DynamicVar("InfestationAmount", 2m)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        
        if (cardPlay.Target.HasPower<InfestationPower>())
        {
            await PowerCmd.Apply<InfestationPower>(choiceContext,
                cardPlay.Target,
                DynamicVars["InfestationAmount"].IntValue,
                Owner.Creature,
                cardPlay.Card);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["InfestationAmount"].UpgradeValueBy(1);
    }
}