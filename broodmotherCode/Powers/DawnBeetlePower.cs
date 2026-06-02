using MegaCrit.Sts2.Core.Entities.Powers;

namespace broodmother.broodmotherCode.Powers;

public class DawnBeetlePower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;
    
}