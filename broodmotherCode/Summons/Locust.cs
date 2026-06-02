using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace broodmother.broodmotherCode.Summons;

public class Locust : BroodmotherSummonModel
{
    protected override AbstractIntent GetIntent()
    {
        return new SingleAttackIntent(1);
    }
    //  deal 1 damage to all enemies, increases each turn. VERY FRAGILE
    public override Task OnPassive(ICombatState combatState, PlayerChoiceContext? choiceContext = null)
    {
        return Task.CompletedTask;
    }

    public override Task OnDeath(PlayerChoiceContext choiceContext)
    {
        return Task.CompletedTask;
    }


    public override int MinInitialHp => 2;
    public override int MaxInitialHp => 2;
}