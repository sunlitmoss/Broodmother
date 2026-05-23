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

    readonly DamageVar _damage = new DamageVar(3m, ValueProp.Move);

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side == CombatSide.Player)
        {
            Creature creature = base.CombatState.RunState.Rng.CombatTargets.NextItem(base.CombatState.HittableEnemies.Where(c => c != base.Owner));
            if (creature != null && !(creature.Monster is IBroodmotherSummon))
            {
                await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), creature, _damage, base.Owner);
            }
        }
    }
}