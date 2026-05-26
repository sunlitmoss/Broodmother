using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;

namespace broodmother.broodmotherCode.Utils;

public class ShiftSingleton() : CustomSingletonModel(true, false)
{
    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)    
    {
        if ((oldPileType == PileType.Hand || oldPileType == PileType.Play) &&
            (card.Pile.Type == PileType.Discard || card.Pile.Type == PileType.Exhaust) &&
            card.Keywords.Contains(Utils.BroodmotherKeywords.Shift))
        {
            await CardCmd.TransformToRandom(card, new Rng());
        }
    }
    
}