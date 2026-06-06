using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace broodmother.broodmotherCode.Utils;

[UsedImplicitly]
public class ShiftKeyword() : CustomSingletonModel(true, false)
{
    public override async Task AfterCardChangedPiles(CardModel card,
        PileType oldPileType,
        AbstractModel? clonedBy)
    {
        if ((oldPileType == PileType.Hand || oldPileType == PileType.Play || oldPileType == PileType.Draw) &&
            (card.Pile?.Type == PileType.Discard || card.Pile?.Type == PileType.Exhaust) &&
            card.Keywords.Contains(BroodmotherKeywords.Shift))
        {
            if (ShiftRegistries.ShiftPairs.TryGetValue(card.GetType(), out var altType))
            {
                var modelDbCard = typeof(ModelDb).GetMethod("Card", Type.EmptyTypes)!
                    .MakeGenericMethod(altType)
                    .Invoke(null, null) as CardModel;
                var alt = card.CardScope!.CreateCard(modelDbCard!, card.Owner);
                await CardCmd.Transform(new CardTransformation(card, alt).Yield(),
                    null, CardPreviewStyle.None);
                if (card.IsUpgraded && alt.IsUpgradable)
                    CardCmd.Upgrade(alt);
                return;
            }

            if (ShiftRegistries.CombatPairs.TryGetValue(card.GetHashCode(),
                    out (Type altTypeC, bool wasUpgraded) tuple))
            {
                var modelDbCard = typeof(ModelDb).GetMethod("Card", Type.EmptyTypes)!
                    .MakeGenericMethod(tuple.altTypeC)
                    .Invoke(null, null) as CardModel;
                var alt = card.CardScope!.CreateCard(modelDbCard!, card.Owner);
                alt.AddKeyword(BroodmotherKeywords.Shift);
                if (tuple.wasUpgraded)
                    CardCmd.Upgrade(alt);
                await CardCmd.Transform(new CardTransformation(card, alt).Yield(),
                    null, CardPreviewStyle.None);
                ShiftRegistries.CombatPairs.Remove(card.GetHashCode());
                ShiftRegistries.CombatPairs[alt.GetHashCode()] = (card.GetType(), card.IsUpgraded);
                return;
            }
        }
    }
}