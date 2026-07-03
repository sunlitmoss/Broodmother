using broodmother.broodmotherCode.Cards.InsectCards;
using broodmother.broodmotherCode.Powers.InsectPowers;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace broodmother.broodmotherCode.Summons;

public class Razorwasp : BroodmotherSummonModel
{
    public override int MinInitialHp => 3;
    public override int MaxInitialHp => 3;

    protected override AbstractIntent GetIntent()
    {
        return new SingleAttackIntent(3);
    }
    public override Task CreateReleaseCard(ICombatState combatState, Player owner) =>
        ReleaseRazorwasp.CreateInHand(owner, combatState);
    public override async Task OnPassive(ICombatState combatState, PlayerChoiceContext? choiceContext = null)
    {
        var power = Creature.GetPower<RazorwaspPower>();
        if (power == null) return;
        var target = combatState.RunState.Rng.CombatTargets.NextItem(
            combatState.HittableEnemies.Where(c => !(c.Monster is IBroodmotherSummon)));
        if (target != null)
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, power.PassiveDamage, Creature);
    }

    public override async Task OnDeath(PlayerChoiceContext choiceContext)
    {
        var power = Creature.GetPower<RazorwaspPower>();
        if (power == null) return;
        var targets = CombatState.HittableEnemies
            .Where(c => !(c.Monster is IBroodmotherSummon))
            .ToList();
        await CreatureCmd.Damage(choiceContext, targets, power.DeathDamage.BaseValue, power.DeathDamage.Props, null!);
    }
}