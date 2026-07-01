using broodmother.broodmotherCode.Cards.InsectCards;
using Broodmother.broodmotherCode.Powers;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace broodmother.broodmotherCode.Summons;

public class Snarefly : BroodmotherSummonModel
{
    public override Task OnDeath(PlayerChoiceContext choiceContext)
    {
        return Task.CompletedTask;
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var activeState =
            new MoveState("ACTIVE", (IReadOnlyList<Creature> _) => Task.CompletedTask, new DebuffIntent());
        var dormantState =
            new MoveState("DORMANT", (IReadOnlyList<Creature> _) => Task.CompletedTask, new SleepIntent());

        activeState.FollowUpState = dormantState;
        dormantState.FollowUpState = activeState;

        return new MonsterMoveStateMachine(new List<MonsterState> { activeState, dormantState }, activeState);
    }

    private bool _isActive = true;
    public override Task CreateReleaseCard(ICombatState combatState, Player owner) =>
        ReleaseSnarefly.CreateInHand(owner, combatState);
    public override async Task OnPassive(ICombatState combatState, PlayerChoiceContext? choiceContext = null)
    {
        if (_isActive)
        {
            if (Target != null && Target.Monster is not IBroodmotherSummon)
                await PowerCmd.Apply<SnareflyConstrictPower>(choiceContext!, Target, 1m, Creature, null);
        }

        _isActive = !_isActive;
        
    }

    public Creature? Target { get; set; }

    public override int MinInitialHp => 5;
    public override int MaxInitialHp => 5;
}