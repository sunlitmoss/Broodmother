using broodmother.broodmotherCode.Cards.InsectCards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace broodmother.broodmotherCode.Summons;

public class DawnBeetle : BroodmotherSummonModel
{
    public override Task CreateReleaseCard(ICombatState combatState, Player owner) =>
        ReleaseDawnBeetle.CreateInHand(owner, combatState);
    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var activeState =
            new MoveState("ACTIVE", (IReadOnlyList<Creature> _) => Task.CompletedTask, new BuffIntent());
        var dormantState =
            new MoveState("DORMANT", (IReadOnlyList<Creature> _) => Task.CompletedTask, new SleepIntent());

        activeState.FollowUpState = dormantState;
        dormantState.FollowUpState = activeState;

        return new MonsterMoveStateMachine(new List<MonsterState> { activeState, dormantState }, activeState);
    }
    private bool _activeThisTurn = false;

    public override async Task OnPassive(ICombatState combatState, PlayerChoiceContext? choiceContext)
    {
        if (_activeThisTurn) await CardPileCmd.Draw(choiceContext!, Summoner!);
        _activeThisTurn = !_activeThisTurn;    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == CombatSide.Player) await OnPassive(combatState, choiceContext);
    }

    public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        return Task.CompletedTask;
    }
    public override async Task OnDeath(PlayerChoiceContext choiceContext)
    {
        await PlayerCmd.GainEnergy(3, Summoner!);
    }
    public override int MinInitialHp => 4;
    public override int MaxInitialHp => 4;
    
}