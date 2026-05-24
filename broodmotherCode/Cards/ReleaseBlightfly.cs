using broodmother.broodmotherCode.Character;
using broodmother.broodmotherCode.Powers;
using broodmother.broodmotherCode.Summons;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace broodmother.broodmotherCode.Cards;
public class ReleaseBlightfly() : BroodmotherInsectCard(1)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public static Task<CardModel?> CreateInHand(Player owner, ICombatState combatState)
    {
        return CreateInHand<ReleaseBlightfly>(owner, combatState);
    }

    protected override IHoverTip InsectPowerTip => HoverTipFactory.FromPower<BlightflyPower>();

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await SummonInsect<Blightfly, BlightflyPower>(choiceContext);
        await ApplyToRandomEnemy<WeakPower>(choiceContext, 1m);

    }
    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}