using broodmother.broodmotherCode.Cards;
using broodmother.broodmotherCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace broodmother.broodmotherCode.Cards;

public class Infest() : broodmotherCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<InfestationPower>(),
        HoverTipFactory.FromPower<ResistancePower>()
    ];

    private DynamicVar InfestationAmount = new DynamicVar("InfestationAmount", 7m);

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        InfestationAmount
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<InfestationPower>(choiceContext, play.Target!, InfestationAmount.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["InfestationAmount"].UpgradeValueBy(3m);
    }
}