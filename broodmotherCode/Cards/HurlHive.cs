using broodmother.broodmotherCode.Cards.InsectCards;
using broodmother.broodmotherCode.Powers.InsectPowers;
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
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>([
        new CardsVar("Cards", 1),
        new DamageVar(3m, ValueProp.Move)
    ]);

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        for (var i = 0; i < DynamicVars["Cards"].IntValue; i++)
        {
            await ReleaseRazorwasp.CreateInHand(Owner, CombatState!);
            await Cmd.Wait(0.25f);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new List<IHoverTip> { HoverTipFactory.FromCard<ReleaseRazorwasp>(), HoverTipFactory.FromPower<RazorwaspPower>() };
}