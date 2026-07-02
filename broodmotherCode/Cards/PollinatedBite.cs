using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards;

public class PollinatedBite() : broodmotherCard(0,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BroodmotherKeywords.Bite];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(2, ValueProp.Move),
        new DynamicVar("Sticky", 2)
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "play.Target");
        
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Sticky"].UpgradeValueBy(1);
    }
}