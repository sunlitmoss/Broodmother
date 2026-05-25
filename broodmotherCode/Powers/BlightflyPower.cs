using MegaCrit.Sts2.Core.Localization.DynamicVars;

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

public class BlightflyPower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public PowerVar<WeakPower> PassiveWeak => new PowerVar<WeakPower>(1m);
    public PowerVar<WeakPower> DeathWeak => new PowerVar<WeakPower>(1m);
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>{HoverTipFactory.FromPower<WeakPower>()};

    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        PassiveWeak,
        DeathWeak
    };
    
}