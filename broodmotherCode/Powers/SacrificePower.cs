using broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Powers;

public sealed class SacrificePower : broodmotherPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override bool ShouldPlayVfx => false;
    
    public override Creature ModifyUnblockedDamageTarget(Creature target, decimal _, ValueProp props, Creature? __)
    {
        if (base.Owner.Monster is not BroodmotherSummonModel summon)
            return target;
        if (target != summon.Summoner?.Creature)
            return target;
        if (base.Owner.IsDead)
            return target;
        if (!props.IsPoweredAttack())
            return target;
        return base.Owner;
    }
}