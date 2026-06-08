using broodmother.broodmotherCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace broodmother.broodmotherCode.Cards;

public class FesteringForm() : broodmotherCard(3,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("InfestationMultiplier", 25)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<FesteringFormPower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
        await PowerCmd.Apply<FesteringFormPower>(choiceContext, Owner.Creature, DynamicVars["InfestationMultiplier"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["InfestationMultiplier"].UpgradeValueBy(25);
    }
}