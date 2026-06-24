using broodmother.broodmotherCode.Cards.InsectCards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace broodmother.broodmotherCode.Powers.InsectPowers;


public class WaspNestPower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new List<IHoverTip> { HoverTipFactory.FromCard<ReleaseDronewasp>() };
}