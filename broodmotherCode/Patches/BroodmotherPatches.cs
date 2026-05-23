using broodmother.broodmotherCode.Cards;
using broodmother.broodmotherCode.Character;
using broodmother.broodmotherCode.Summons;
using Broodmother.broodmotherCode.Summons;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace broodmother.broodmotherCode.Patches;

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
}