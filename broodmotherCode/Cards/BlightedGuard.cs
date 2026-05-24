using broodmother.broodmotherCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards;


public class BlightedGuard() : BroodmotherCard
(2, CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>(new DynamicVar[2]
    {
        new CardsVar("Cards", 1),
        new BlockVar(7m, ValueProp.Move)
    });
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, play);
        for (int i = 0; i < base.DynamicVars["Cards"].IntValue; i++)
        {
            await ReleaseBlightfly.CreateInHand(base.Owner, base.CombatState);
            await Cmd.Wait(0.25f);
        }
    }
    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        new List<IHoverTip> { HoverTipFactory.FromCard<ReleaseBlightfly>(), HoverTipFactory.FromPower<BlightflyPower>(), HoverTipFactory.FromPower<WeakPower>() };
}