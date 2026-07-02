using broodmother.broodmotherCode.Cards.InsectCards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace broodmother.broodmotherCode.Powers;

public class BroodPower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<ReleaseDronewasp>()];

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        for (var i = 0; i < Amount; i++)
            await ReleaseDronewasp.CreateInHand(Owner.Player!, combatState);
        
        if (participants.Contains(base.Owner) && base.AmountOnTurnStart != 0)
        {
            await PowerCmd.Remove(this);
        }
    }
    
}