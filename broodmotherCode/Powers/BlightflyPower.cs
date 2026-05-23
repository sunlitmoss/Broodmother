using Broodmother.broodmotherCode.Summons;

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

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)    {
        if (side == CombatSide.Player)
        {
            Creature creature = base.CombatState.RunState.Rng.CombatTargets.NextItem(base.CombatState.HittableEnemies.Where(c => c != base.Owner));
            if (creature != null && !(creature.Monster is IBroodmotherSummon))
            {
                await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), creature, base.Amount, base.Owner, null);
            }
        }
    }
}