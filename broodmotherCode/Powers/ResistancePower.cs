using MegaCrit.Sts2.Core.Entities.Powers;

namespace broodmother.broodmotherCode.Powers;

public class ResistancePower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.None;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
}