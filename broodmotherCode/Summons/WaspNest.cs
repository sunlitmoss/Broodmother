using broodmother.broodmotherCode.Powers;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace broodmother.broodmotherCode.Summons;

public class WaspNest : BroodmotherSummonModel
{
    public override int MinInitialHp => 6;
    public override int MaxInitialHp => 6;

    protected override AbstractIntent GetIntent()
    {
        return new SingleAttackIntent(3);
    }

    public override async Task OnPassive(ICombatState combatState)
    {
        var power = Creature.GetPower<WaspNestPower>();
        if (power == null) return;
        var target = combatState.RunState.Rng.CombatTargets.NextItem(
            combatState.HittableEnemies.Where(c => !(c.Monster is IBroodmotherSummon)));
        if (target != null)
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, power.PassiveDamage, Creature);
    }

    public override async Task OnDeath(PlayerChoiceContext choiceContext)
    {
        var power = Creature.GetPower<WaspNestPower>();
        if (power == null) return;
        var targets = CombatState.HittableEnemies
            .Where(c => !(c.Monster is IBroodmotherSummon))
            .ToList();
        await CreatureCmd.Damage(choiceContext, targets, power.DeathDamage.BaseValue, power.DeathDamage.Props, null);
    }
}