using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace broodmother.broodmotherCode.Powers.InsectPowers;

public class PlagueflyPower() : broodmotherPower
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("InfestAmount", 1m)
    ];

    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;
    
}