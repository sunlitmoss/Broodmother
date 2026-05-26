using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards;


public class BugShield() : broodmother.broodmotherCode.Cards.broodmotherCard
    (1, CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new CalculationBaseVar(5m),
        new CalculationExtraVar(2m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) =>
            card.CombatState?.Enemies
                .Where(c => c.IsAlive && c.Monster is IBroodmotherSummon)
                .Count() ?? 0)
    };


protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
{
    await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), base.DynamicVars.CalculatedBlock.Props, cardPlay);
}

protected override void OnUpgrade()
{
    base.EnergyCost.UpgradeBy(-1);
}
}
