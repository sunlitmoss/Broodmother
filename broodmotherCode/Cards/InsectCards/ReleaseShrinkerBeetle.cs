using broodmother.broodmotherCode.Powers;
using Broodmother.broodmotherCode.Powers;
using broodmother.broodmotherCode.Powers.InsectPowers;
using broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace broodmother.broodmotherCode.Cards.InsectCards;


public class ReleaseShrinkerBeetle() : BroodmotherInsectCard(TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IHoverTip InsectPowerTip => HoverTipFactory.FromPower<ShrinkerBeetlePower>();
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<ShrinkerBeetleShrinkPower>()];

    public static Task<CardModel?> CreateInHand(Player owner, ICombatState combatState)
    {
        return CreateInHand<ReleaseShrinkerBeetle>(owner, combatState);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var summon = await SummonInsect<ShrinkerBeetle>(choiceContext);
        (summon!.Monster as ShrinkerBeetle)!.Target = play.Target;
        await PowerCmd.Apply<MinionPower>(choiceContext, summon!, 1m, null, null);
        await PowerCmd.Apply<ShrinkerBeetlePower>(choiceContext, summon!, 1m, null, null);
        await PowerCmd.Apply<SacrificePower>(choiceContext, summon!, 1m, null, null);
    }
    
    protected override void OnUpgrade()
    {
    }
}