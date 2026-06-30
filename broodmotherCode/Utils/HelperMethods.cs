using BaseLib.Abstracts;
using broodmother.broodmotherCode.Summons;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;

namespace broodmother.broodmotherCode.Utils;
public static class HelperMethods
{
    public static Creature? RandomNonInsectTarget(ICombatState combatState, Rng rng)
    {
        var targets = combatState.HittableEnemies
            .Where(c => c.Monster is not IBroodmotherSummon)
            .ToList();
        return targets.Count > 0 ? rng.NextItem(targets) : null;
    }

    public static async Task ShiftCard(CardModel card)
    {
        if (!ShiftRegistries.CombatPairs.TryGetValue(card, out var otherSide)) return;

        ShiftRegistries.CombatPairs.Remove(card);
        ShiftRegistries.CombatPairs.Remove(otherSide);

        otherSide.RemoveFromCurrentPile();

        var upgraded = card.IsUpgraded;
        await CardCmd.Transform(
            new CardTransformation(card, otherSide).Yield(),
            null,
            CardPreviewStyle.None);

        if (upgraded && otherSide.IsUpgradable && !otherSide.IsUpgraded)
            CardCmd.Upgrade(otherSide);

        if (!ShiftRegistries.CombatPairs.ContainsKey(otherSide))
        {
            var newAlt = otherSide.CardScope!.CreateCard(
                (typeof(ModelDb).GetMethod("Card", Type.EmptyTypes)!
                    .MakeGenericMethod(card.GetType())
                    .Invoke(null, null) as CardModel)!,
                otherSide.Owner);
            newAlt.AddKeyword(BroodmotherKeywords.Shift);
            ShiftRegistries.CombatPairs[otherSide] = newAlt;
            ShiftRegistries.CombatPairs[newAlt] = otherSide;
            await CardPileCmd.Add(newAlt, OtherSidePile.OtherSide);
        }
    }

    public static Task StickyCard(CardModel card, int amount)
    {
        var existingModifier = CardModifier.Modifiers(card)
            .OfType<StickyModifier>().FirstOrDefault();
    
        if (existingModifier != null)
        {
            amount += existingModifier.RemainingTurns;
            CardModifier.RemoveModifier(card, existingModifier);
            card.RemoveKeyword(BroodmotherKeywords.Sticky); 
        }

        card.AddKeyword(BroodmotherKeywords.Sticky);
        var modifier = (StickyModifier)ModelDb
            .GetById<StickyModifier>(ModelDb.GetId<StickyModifier>())
            .MutableClone();
        modifier.RemainingTurns = amount;
        CardModifier.AddModifier(card, modifier);
        return Task.CompletedTask;
    }
    
    public static async Task ConsumeInsect(Creature insect, PlayerChoiceContext choiceContext)
    {
        if (insect.Monster is BroodmotherSummonModel insectModel)
            insectModel.WasConsumed = true;
        await CreatureCmd.Kill(insect);
    }
}