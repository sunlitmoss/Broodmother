using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace broodmother.broodmotherCode.Powers;

public class RecallPower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(CardModel card, bool isAutoPlay, ResourceInfo resources, PileType pileType, CardPilePosition position)
    {
        if (card.Owner.Creature != Owner) return (pileType, position);
        if (card.Type != CardType.Attack && card.Type != CardType.Skill) return (pileType, position);        
        if (pileType != PileType.Discard) return (pileType, position);
        if (Amount <= 0) return (pileType, position);
        return (PileType.Draw, CardPilePosition.Random);
    }

    public override async Task AfterModifyingCardPlayResultPileOrPosition(CardModel card, PileType pileType, CardPilePosition position)
    {
        if (card.Owner.Creature != base.Owner) return;
        Flash();
        await PowerCmd.Decrement(this);
    }
    
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext,
        CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player) return;
        if (!participants.Contains(Owner)) return;
        await PowerCmd.Remove(this);
    }
}