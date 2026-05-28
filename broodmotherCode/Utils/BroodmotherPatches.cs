using broodmother.broodmotherCode.Powers;
using broodmother.broodmotherCode.Summons;
using Broodmother.broodmotherCode.Summons;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace broodmother.broodmotherCode.Utils;

public class BroodmotherPatches
{
    [HarmonyPatch(typeof(NCombatRoom), "AddCreature")]
    class AddCreaturePatch
    {
        static void Postfix(Creature creature)
        {
            if (creature.Monster is IBroodmotherSummon)
            {
                var node = NCombatRoom.Instance?.GetCreatureNode(creature);
                var slot = (creature.Monster as Blightfly)?.SlotIndex ?? -1;
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
        static bool Prefix(Creature target)
        {
            if (target.Monster is IBroodmotherSummon)
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
}
