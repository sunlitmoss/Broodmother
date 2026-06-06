using broodmother.broodmotherCode.Powers.InsectPowers;
using broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace broodmother.broodmotherCode.Cards.InsectCards;

public class ReleaseDawnBeetle : BroodmotherInsectCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IHoverTip InsectPowerTip => HoverTipFactory.FromPower<DawnBeetlePower>();

    protected override async Task ApplySummonPowers(PlayerChoiceContext choiceContext, Creature creature)
    {
        await PowerCmd.Apply<DawnBeetlePower>(choiceContext, creature, 1m, null, null);
    }
    
    public static Task<CardModel?> CreateInHand(Player owner, ICombatState combatState)
    {
        return CreateInHand<ReleaseDawnBeetle>(owner, combatState);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var summon = await SummonInsect<DawnBeetle>(choiceContext);
        
    }

    protected override void OnUpgrade()
    {
    }
}