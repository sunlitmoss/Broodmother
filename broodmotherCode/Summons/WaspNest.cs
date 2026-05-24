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

    static readonly decimal _damage = 3m;
    readonly DamageVar _passiveDamage = new DamageVar(_damage, ValueProp.Move);
    readonly DamageVar _deathDamage =  new DamageVar(2 * _damage, ValueProp.Move);

    protected override AbstractIntent GetIntent() => new SingleAttackIntent(3);
    
    public override async Task OnPassive(ICombatState combatState)
    {
        Creature? target = combatState.RunState.Rng.CombatTargets.NextItem(
            combatState.HittableEnemies.Where(c => !(c.Monster is IBroodmotherSummon)));
        if (target != null)
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, _passiveDamage, base.Creature);

    }
    public override async Task OnDeath(PlayerChoiceContext choiceContext)
    {
        List<Creature> targets = base.CombatState.HittableEnemies
            .Where(c => !(c.Monster is IBroodmotherSummon))
            .ToList();
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), targets, _deathDamage.BaseValue, _deathDamage.Props, null);
    }
}