using broodmother.broodmotherCode.Powers;
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

public class ReleaseScarabBeetle : BroodmotherInsectCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IHoverTip InsectPowerTip => HoverTipFactory.FromPower<ScarabBeetlePower>();
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<SwanSongPower>()];

    protected override async Task ApplySummonPowers(PlayerChoiceContext choiceContext, Creature creature)
    {
        await PowerCmd.Apply<ScarabBeetlePower>(choiceContext, creature, 1m, null, null);
    }

    public static Task<CardModel?> CreateInHand(Player owner, ICombatState combatState)
    {
        return CreateInHand<ReleaseScarabBeetle>(owner, combatState);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var summon = await SummonInsect<ScarabBeetle>(choiceContext);
        
    }

    protected override void OnUpgrade()
    {
    }
}