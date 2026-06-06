using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Random;

namespace broodmother.broodmotherCode.Utils;
public static class PatchHelper
{
    public static Creature? RandomNonInsectTarget(ICombatState combatState, Rng rng)
    {
        var targets = combatState.HittableEnemies
            .Where(c => c.Monster is not IBroodmotherSummon)
            .ToList();
        return targets.Count > 0 ? rng.NextItem(targets) : null;
    }
}