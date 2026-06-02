using broodmother.broodmotherCode.Summons;
using Broodmother.broodmotherCode.Summons;
using Godot;
using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace broodmother.broodmotherCode.Utils;

[UsedImplicitly]
public class BroodmotherPatches
{
    [HarmonyPatch(typeof(NCombatRoom), "AddCreature")]
    [UsedImplicitly]
    public class AddCreaturePatch
    {
        public static void Postfix(Creature creature)
        {
            if (creature.Monster is IBroodmotherSummon)
            {
                var node = NCombatRoom.Instance?.GetCreatureNode(creature);
                var slot = (creature.Monster as BroodmotherSummonModel)?.SlotIndex ?? -1;
                if (slot >= 0) node.Position = BroodmotherInsectSlots.ActiveSlots[slot];

                node.Scale = new Vector2(0.5f, 0.5f);
            }
        }
    }

    [HarmonyPatch(typeof(CombatManager), "SetUpCombat")]
    public class ResetInsectSlotsPatch
    {
        public static void Postfix()
        {
            BroodmotherInsectSlots.Reset();
        }
    }

    [HarmonyPatch(typeof(RavenousPower), "AfterDeath")]
    [UsedImplicitly]
    public class RavenousPowerPatch
    {
        [UsedImplicitly]
        public static bool Prefix(Creature target)
        {
            if (target.Monster is IBroodmotherSummon)
                return false;
            return true;
        }
    }

    [HarmonyPatch(typeof(CardModel), "get_HoverTips")]
    [UsedImplicitly]
    public class ShiftCombatHoverTipsPatch
    {
        public static void Postfix(CardModel __instance, ref IEnumerable<IHoverTip> __result)
        {
            if (__instance.Keywords.Contains(BroodmotherKeywords.Shift) &&
                ShiftRegistries.CombatPairs.TryGetValue(__instance.GetHashCode(),
                    out (Type altTypeC, bool wasUpgraded) tuple))
            {
                var modelDbCard = typeof(ModelDb).GetMethod("Card", Type.EmptyTypes)!
                    .MakeGenericMethod(tuple.altTypeC)
                    .Invoke(null, null) as CardModel;
                var list = __result.ToList();
                var alt = __instance.CardScope?.CreateCard(modelDbCard!, __instance.Owner);
                alt?.AddKeyword(BroodmotherKeywords.Shift);
                if (tuple.wasUpgraded && alt != null) alt.UpgradeInternal();
                list.Add(HoverTipFactory.FromCard(alt!));
                __result = list;
            }
        }
    }

[HarmonyPatch(typeof(RunState), "CreateShared")]
[UsedImplicitly]
public class SetInsectSlotsPatch
{
    public static void Postfix(RunState __result)
    {
        int count = __result.Players.
            Count(p => p.Character is Character.Broodmother);
        if (count > 1) BroodmotherInsectSlots.SetSlots();
    }
}
}