using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace broodmother.broodmotherCode.Utils;

[UsedImplicitly]
#pragma warning disable CS0618 // Type or member is obsolete
public class ShiftKeyword() : CustomSingletonModel(true, false)
#pragma warning restore CS0618 // Type or member is obsolete
{
    public override async Task AfterCardChangedPiles(CardModel card,
        PileType oldPileType,
        AbstractModel? clonedBy)
    {
        if (card is IShiftCard shiftCard &&
            card.Pile?.Type != OtherSidePile.OtherSide &&
            !ShiftRegistries.CombatPairs.ContainsKey(card))
        {
            var modelDbCard = typeof(ModelDb).GetMethod("Card", Type.EmptyTypes)!
                .MakeGenericMethod(shiftCard.AlternateCardType)
                .Invoke(null, null) as CardModel;
            var alt = card.CardScope!.CreateCard(modelDbCard!, card.Owner);
            alt.AddKeyword(BroodmotherKeywords.Shift);
            ShiftRegistries.CombatPairs[card] = alt;
            ShiftRegistries.CombatPairs[alt] = card;
            await CardPileCmd.Add(alt, OtherSidePile.OtherSide);
        }

        if ((oldPileType == PileType.Hand || oldPileType == PileType.Play || oldPileType == PileType.Draw) &&
            (card.Pile?.Type == PileType.Discard) &&
            card.Keywords.Contains(BroodmotherKeywords.Shift))
        {
            await HelperMethods.ShiftCard(card);
        }
    }
}