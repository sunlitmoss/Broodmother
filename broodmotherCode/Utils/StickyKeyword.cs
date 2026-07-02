using BaseLib.Abstracts;
using broodmother.broodmotherCode.Cards;
using broodmother.broodmotherCode.Utils;
using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

public class StickyModifier : CardModifier
{
    public int RemainingTurns;

    public override void ModifyDescriptionPost(Creature? target, ref string description)
    {
        description = $"[gold]Sticky[/gold] {RemainingTurns}. \n" + description;
    }
    public override void StoreSaveData(ModifierSave save)
        => save.IntProperties["RemainingTurns"] = RemainingTurns;

    public override void LoadSaveData(ModifierSave save)
        => RemainingTurns = save.IntProperties.GetValueOrDefault("RemainingTurns", 0);
    
    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != CombatSide.Player) return;
        if (Owner?.Pile?.Type != PileType.Hand) return;

        RemainingTurns--;
        if (RemainingTurns <= 0)
        {
            Owner.RemoveKeyword(BroodmotherKeywords.Sticky);
            RemoveModifier(Owner, this);
        }
    }
}

#pragma warning disable CS0618 // Type or member is obsolete
[UsedImplicitly]
public class StickyLogic() : CustomSingletonModel(true, false)
#pragma warning restore CS0618 // Type or member is obsolete
{
    public override async Task AfterCardEnteredCombat(CardModel card)
    {
        if (card is PollinatedBite)
        {
            await HelperMethods.StickyCard(card, card.DynamicVars["Sticky"].IntValue);
        }
    }

    public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(CardModel card, bool isAutoPlay, ResourceInfo resources, PileType pileType, CardPilePosition position)
    {        
        if (!card.Keywords.Contains(BroodmotherKeywords.Sticky)) return (pileType, position);
        if (card.Keywords.Contains(CardKeyword.Exhaust)) return (pileType, position);
        
        var modifier = CardModifier.Modifiers(card).OfType<StickyModifier>().FirstOrDefault();
        if (modifier == null) return (pileType, position);

        modifier.RemainingTurns--;

        if (modifier.RemainingTurns <= 0)
        {
            card.RemoveKeyword(BroodmotherKeywords.Sticky);
            CardModifier.RemoveModifier(card, modifier);
            return (pileType, position);
        }
        
        return (PileType.Hand, CardPilePosition.Top);
    }
    
}

public class StickyPatches
{
    [UsedImplicitly]
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.ShouldRetainThisTurn), MethodType.Getter)]
    public static class StickyRetainPatch
    {
        public static void Postfix(CardModel __instance, ref bool __result)
        {
            if (__result) return;
            if (__instance.Keywords.Contains(BroodmotherKeywords.Sticky)) __result = true;
        }
    }

    [UsedImplicitly]
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.GetDescriptionForPile), 
        new Type[] { typeof(PileType), typeof(Creature) })]
    public static class HideStickyPatch
    {
        public static void Postfix(CardModel __instance, ref string __result)
        {
            if (!__instance.Keywords.Contains(BroodmotherKeywords.Sticky)) return;

            var lines = __result.Split('\n').Where(line =>
                line != "[gold]Sticky[/gold].");
            
            __result = string.Join('\n', lines);
        }
    }
}