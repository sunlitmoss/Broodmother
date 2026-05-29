using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace broodmother.broodmotherCode.Powers;

public class EncouragementPower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<VigorPower>()
    ];

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == CombatSide.Player)
            await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), Owner,
                BroodmotherInsectSlots.CountOccupiedSlots() * Amount, Owner, null);
    }
}