using broodmother.broodmotherCode.Character;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace broodmother.broodmotherCode.Summons;

using broodmother.broodmotherCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

public class WaspNest : BroodmotherSummonModel
{
    public override int MinInitialHp => 6;
    public override int MaxInitialHp => 6;
    
    protected override AbstractIntent GetIntent() => new SingleAttackIntent(3);
    
    public override async Task OnPassive(ICombatState combatState)
    {
        WaspNestPower? power = base.Creature.GetPower<WaspNestPower>();
        if (power == null) return;
        Creature? target = combatState.RunState.Rng.CombatTargets.NextItem(
            combatState.HittableEnemies.Where(c => !(c.Monster is IBroodmotherSummon)));
        if (target != null)
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, power.PassiveDamage, base.Creature);

    }
    public override async Task OnDeath(PlayerChoiceContext choiceContext)
    {
        WaspNestPower? power = base.Creature.GetPower<WaspNestPower>();
        if (power == null) return;
        List<Creature> targets = base.CombatState.HittableEnemies
            .Where(c => !(c.Monster is IBroodmotherSummon))
            .ToList();
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), targets, power.DeathDamage.BaseValue, power.DeathDamage.Props, null);
    }
}