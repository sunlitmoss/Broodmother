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
    private readonly Dictionary<Type, Type> _shiftPairs = new Dictionary<Type, Type>()
    {
        {typeof(MetamorphicStrike), typeof(MetamorphicDefend)},
        {typeof(MetamorphicDefend),  typeof(MetamorphicStrike)},
    };
    
    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)    
    {
        if ((oldPileType == PileType.Hand || oldPileType == PileType.Play) &&
            (card.Pile!.Type == PileType.Discard || card.Pile.Type == PileType.Exhaust) &&
            card.Keywords.Contains(Utils.BroodmotherKeywords.Shift))
        {
            if (_shiftPairs.TryGetValue(card.GetType(), out Type? altType))
            {
                var modelDbCard = typeof(ModelDb).GetMethod("Card", Type.EmptyTypes)!
                    .MakeGenericMethod(altType)
                    .Invoke(null, null) as CardModel;

                CardModel alt = card.CardScope!.CreateCard(modelDbCard!, card.Owner);
                await CardCmd.Transform(new CardTransformation(card, alt).Yield(), null, CardPreviewStyle.None);
                if (card.IsUpgraded && alt.IsUpgradable)
                    CardCmd.Upgrade(alt);
            }
        }
    }
}