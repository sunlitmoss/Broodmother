using broodmother.broodmotherCode.Cards.InsectCards;
using broodmother.broodmotherCode.Powers.InsectPowers;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace broodmother.broodmotherCode.Summons;

public class Dronewasp : BroodmotherSummonModel
{
    public override Task CreateReleaseCard(ICombatState combatState, Player owner) =>
        ReleaseDronewasp.CreateInHand(owner, combatState);
    public override int MinInitialHp => 2;
    public override int MaxInitialHp => 2;
    
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
    }

    public override Task OnDeath(PlayerChoiceContext choiceContext)
    {
        return Task.CompletedTask;
    }
}