using broodmother.broodmotherCode.Powers.InsectPowers;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace broodmother.broodmotherCode.Summons;

public class Dronewasp : BroodmotherSummonModel
{
    public override int MinInitialHp => 1;
    public override int MaxInitialHp => 1;

    private int _lifespan = 2;

    protected override AbstractIntent GetIntent()
    {
        return new SingleAttackIntent(3);
    }
    
    public override async Task OnPassive(ICombatState combatState, PlayerChoiceContext? choiceContext = null)
    {
        var power = Creature.GetPower<DronewaspPower>();
        if (power == null) return;
        var target = combatState.RunState.Rng.CombatTargets.NextItem(
            combatState.HittableEnemies.Where(c => !(c.Monster is IBroodmotherSummon)));
        if (target != null)
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, power.PassiveDamage, Creature);
        _lifespan--;
        if (_lifespan <= 0) await CreatureCmd.Kill(this.Creature);

    }

    public override Task OnDeath(PlayerChoiceContext choiceContext)
    {
        return Task.CompletedTask;
    }
}