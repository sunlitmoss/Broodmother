using broodmother.broodmotherCode.Summons;
using Broodmother.broodmotherCode.Summons;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace broodmother.broodmotherCode.Utils;

public class BroodmotherPatches
{
    [HarmonyPatch(typeof(NCombatRoom), "AddCreature")]
    class AddCreaturePatch
    {
        static void Postfix(Creature __creature)
        {
            if (__creature.Monster is IBroodmotherSummon)
            {
                var node = NCombatRoom.Instance?.GetCreatureNode(__creature);
                var slot = (__creature.Monster as Blightfly)?.SlotIndex ?? -1;
                if (slot >= 0)
                {
                    node.Position = BroodmotherInsectSlots.InsectSlots[slot];
                }

                node.Scale = new Vector2(0.5f, 0.5f);

            }
        }
    }

    [HarmonyPatch(typeof(CombatManager), "SetUpCombat")]
    class ResetInsectSlotsPatch
    {
        public static void Postfix()
        {
            BroodmotherInsectSlots.Reset();
        }
    }

    [HarmonyPatch(typeof(RavenousPower), "AfterDeath")]
    class RavenousPowerPatch
    {
        static bool Prefix(Creature __target)
        {
            if (__target.Monster is IBroodmotherSummon)
                return false;
            return true;
        }
    }

    [HarmonyPatch(typeof(CardModel), "get_HoverTips")]
    class ShiftCombatHoverTipsPatch
    {
        public static void Postfix(CardModel __instance, ref IEnumerable<IHoverTip> __result)
        {
            if (__instance.Keywords.Contains(BroodmotherKeywords.Shift) &&
                ShiftRegistries.CombatPairs.TryGetValue(__instance.GetHashCode(),
                    out (Type altTypeC, bool wasUpgraded) tuple))
            {
                CardModel? modelDbCard = typeof(ModelDb).GetMethod("Card", Type.EmptyTypes)!
                    .MakeGenericMethod(tuple.altTypeC)
                    .Invoke(null, null) as CardModel;
                List<IHoverTip> list = __result.ToList();
                CardModel? alt = __instance.CardScope?.CreateCard(modelDbCard!, __instance.Owner);
                alt?.AddKeyword(BroodmotherKeywords.Shift);
                if (tuple.wasUpgraded && alt != null) alt.UpgradeInternal();
                list.Add(HoverTipFactory.FromCard(alt!));
                __result = list;
            }
        }
    }

    [HarmonyPatch(typeof(RunState), "CreateShared")]
    class SetInsectSlotsPatch
    {
        public static void Prefix(RunState __instance)
        {
            int count = 0;
            foreach (Player p in __instance.Players)
                if (p.Character is Character.Broodmother)
                    count++;
            if (count == 2) BroodmotherInsectSlots.SetSlots();
        }
    }
}
