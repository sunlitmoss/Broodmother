using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace broodmother.broodmotherCode.Powers;

public class BlightflyPower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new List<IHoverTip> { HoverTipFactory.FromPower<WeakPower>() };
}