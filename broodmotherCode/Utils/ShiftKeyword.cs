using BaseLib.Abstracts;
using broodmother.broodmotherCode.Cards.ShiftCards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;
namespace broodmother.broodmotherCode.Utils;

public class ShiftKeyword() : CustomSingletonModel(true, false)
{

    
    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)    
    {
        if ((oldPileType == PileType.Hand || oldPileType == PileType.Play) &&
            (card.Pile?.Type == PileType.Discard || card.Pile?.Type == PileType.Exhaust) &&
            card.Keywords.Contains(Utils.BroodmotherKeywords.Shift))
        {
            if (ShiftRegistries.ShiftPairs.TryGetValue(card.GetType(), out Type? altType))
            {
                CardModel? modelDbCard = typeof(ModelDb).GetMethod("Card", Type.EmptyTypes)!
                    .MakeGenericMethod(altType)
                    .Invoke(null, null) as CardModel;

                CardModel alt = card.CardScope!.CreateCard(modelDbCard!, card.Owner);
                await CardCmd.Transform(new CardTransformation(card, alt).Yield(), null, CardPreviewStyle.None);
                if (card.IsUpgraded && alt.IsUpgradable)
                    CardCmd.Upgrade(alt);
                return;
            }
            
            if (ShiftRegistries.CombatPairs.TryGetValue(card.GetHashCode(),out (Type altTypeC, bool wasUpgraded) tuple))
            {
                CardModel? modelDbCard = typeof(ModelDb).GetMethod("Card", Type.EmptyTypes)!
                    .MakeGenericMethod(tuple.altTypeC)
                    .Invoke(null, null) as CardModel;

                CardModel alt = card.CardScope!.CreateCard(modelDbCard!, card.Owner);
                alt.AddKeyword(BroodmotherKeywords.Shift);
                if (tuple.wasUpgraded) CardCmd.Upgrade(alt);
                await CardCmd.Transform(new CardTransformation(card, alt).Yield(), null, CardPreviewStyle.None);
                ShiftRegistries.CombatPairs.Remove(card.GetHashCode());
                ShiftRegistries.CombatPairs[alt.GetHashCode()] = (card.GetType(), card.IsUpgraded);                return;
            }
        }
    }
}