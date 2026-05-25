using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Powers;
using  System.Threading.Tasks; 
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

public class WaspNestPower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    static public decimal _damage = 3m;
    public DamageVar PassiveDamage = new DamageVar("PassiveDamage",_damage, ValueProp.Move);
    public DamageVar DeathDamage =  new DamageVar("DeathDamage",2 * _damage, ValueProp.Move);
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        PassiveDamage,
        DeathDamage
    };
}