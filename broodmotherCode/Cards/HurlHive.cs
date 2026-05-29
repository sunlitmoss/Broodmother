using broodmother.broodmotherCode.Cards.InsectCards;
using broodmother.broodmotherCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards;

public class HurlHive() : broodmotherCard
(1, CardType.Skill, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>(new DynamicVar[2]
    {
        new CardsVar("Cards", 1),
        new DamageVar(3m, ValueProp.Move)
    });

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        for (var i = 0; i < DynamicVars["Cards"].IntValue; i++)
        {
            await ReleaseWaspNest.CreateInHand(Owner, CombatState);
            await Cmd.Wait(0.25f);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Cards"].UpgradeValueBy(1m);
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new List<IHoverTip> { HoverTipFactory.FromCard<ReleaseWaspNest>(), HoverTipFactory.FromPower<WaspNestPower>() };
}