using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Powers.InsectPowers;

public class WaspNestPower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    public static decimal _damage = 3m;
    public DamageVar PassiveDamage = new("PassiveDamage", _damage, ValueProp.Move);
    public DamageVar DeathDamage = new("DeathDamage", 2 * _damage, ValueProp.Move);

    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        PassiveDamage,
        DeathDamage
    };
}