using Broodmother.broodmotherCode.Summons;
using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;

namespace broodmother.broodmotherCode.Utils;

public class TargetPatches
{
    // [UsedImplicitly]
    // [HarmonyPatch(typeof(CountdownPower), nameof(CountdownPower.AfterSideTurnStart))]
    // public class CountdownPowerExcludeInsectsPatch
    // {
    //     public static bool Prefix(CountdownPower __instance, CombatSide side,
    //         IReadOnlyList<Creature> participants, ICombatState combatState, ref Task __result)
    //     {
    //         if (!participants.Contains(__instance.Owner))
    //         {
    //             __result = Task.CompletedTask;
    //             return false;
    //         }
    //
    //         __result = RunFiltered(__instance, combatState);
    //         return false;
    //     }
    //
    //     private static async Task RunFiltered(CountdownPower __instance, ICombatState combatState)
    //     {
    //         Traverse.Create(__instance).Method("Flash").GetValue();
    //         var creature = HelperMethods.RandomNonInsectTarget(
    //             combatState,
    //             __instance.Owner.Player!.RunState.Rng.CombatTargets);
    //         if (creature != null)
    //             await PowerCmd.Apply<DoomPower>(new ThrowingPlayerChoiceContext(),
    //                 creature,
    //                 __instance.Amount,
    //                 __instance.Owner,
    //                 null);
    //     }
    // }
    //
    // [UsedImplicitly]
    // [HarmonyPatch(typeof(HauntPower), nameof(HauntPower.AfterCardPlayed))]
    // public class HauntPowerExcludeInsectsPatch
    // {
    //     public static bool Prefix(HauntPower __instance, PlayerChoiceContext choiceContext, CardPlay cardPlay, ref Task __result)
    //     {
    //         if (cardPlay.Card is Soul && cardPlay.Card.Owner.Creature == __instance.Owner)
    //         {
    //             __result = RunFiltered(__instance, choiceContext);
    //             return false;
    //         }
    //         __result = Task.CompletedTask;
    //         return false;
    //     }
    //     
    //     private static async Task RunFiltered(HauntPower __instance, PlayerChoiceContext choiceContext)
    //     {
    //         var creature = HelperMethods.RandomNonInsectTarget(
    //             __instance.CombatState,
    //             __instance.Owner.Player!.RunState.Rng.CombatTargets);
    //         if (creature != null)
    //             await CreatureCmd.Damage(choiceContext, new[] { creature },
    //                 __instance.Amount, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
    //     }
    // }
    
    [UsedImplicitly]
    [HarmonyPatch(typeof(CardCmd), nameof(CardCmd.AutoPlay))]
    public class AutoPlayExcludeInsectsPatch
    {
        public static void Prefix(CardModel card, ref Creature? target)
        {
            if (card.TargetType != TargetType.AnyEnemy || target != null) return;
            
            ICombatState combatState = card.CombatState ?? card.Owner.Creature.CombatState!;
    
            target = HelperMethods.RandomNonInsectTarget(
                combatState,
                card.Owner.RunState.Rng.CombatTargets);
        }
    }
    //
    // [UsedImplicitly]
    // [HarmonyPatch(typeof(JuggernautPower), nameof(JuggernautPower.AfterBlockGained))]
    // public class JuggernautPowerExcludeInsectsPatch
    // {
    //     public static bool Prefix(JuggernautPower __instance, Creature creature, decimal amount, ref Task __result)
    //     {
    //         if (amount <= 0m || creature != __instance.Owner)
    //         {
    //             __result = Task.CompletedTask;
    //             return false;
    //         }
    //
    //         __result = RunFiltered(__instance);
    //         return false;
    //     }
    //
    //     private static async Task RunFiltered(JuggernautPower __instance)
    //     {
    //         var target = HelperMethods.RandomNonInsectTarget(
    //             __instance.CombatState,
    //             __instance.Owner.Player.RunState.Rng.CombatTargets);
    //         if (target != null)
    //         {
    //             Traverse.Create(__instance).Method("Flash").GetValue();
    //             await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target,
    //                 __instance.Amount, ValueProp.Unpowered, __instance.Owner, null);
    //         }
    //     }
    // }
    //
    // [UsedImplicitly]
    // [HarmonyPatch(typeof(SerpentFormPower), nameof(SerpentFormPower.AfterCardPlayed))]
    // public class SerpentFormPowerExcludeInsectsPatch
    // {
    //     private static readonly Type DataType =
    //         typeof(SerpentFormPower).GetNestedType("Data", BindingFlags.NonPublic)!;
    //     private static readonly FieldInfo AmountsField =
    //         DataType.GetField("amountsForPlayedCards")!;
    //     private static readonly MethodInfo GetInternalDataMethod =
    //         typeof(PowerModel).GetMethod("GetInternalData", BindingFlags.NonPublic | BindingFlags.Instance)!
    //             .MakeGenericMethod(DataType);
    //
    //     public static bool Prefix(SerpentFormPower __instance, PlayerChoiceContext choiceContext,
    //         CardPlay cardPlay, ref Task __result)
    //     {
    //         if (cardPlay.Card.Owner != __instance.Owner.Player)
    //         {
    //             __result = Task.CompletedTask;
    //             return false;
    //         }
    //
    //         var data = GetInternalDataMethod.Invoke(__instance, null)!;
    //         var dict = (Dictionary<CardModel, int>)AmountsField.GetValue(data)!;
    //
    //         if (!dict.Remove(cardPlay.Card, out var damage) || damage <= 0)
    //         {
    //             __result = Task.CompletedTask;
    //             return false;
    //         }
    //
    //         __result = RunFiltered(__instance, choiceContext, damage);
    //         return false;
    //     }
    //
    //     private static async Task RunFiltered(SerpentFormPower __instance,
    //         PlayerChoiceContext choiceContext, decimal damage)
    //     {
    //         await Cmd.CustomScaledWait(0.1f, 0.2f);
    //         var creature = HelperMethods.RandomNonInsectTarget(
    //             __instance.Owner.CombatState,
    //             __instance.Owner.Player.RunState.Rng.CombatTargets);
    //         if (creature != null)
    //         {
    //             VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_attack_blunt");
    //             await CreatureCmd.Damage(choiceContext, creature, damage,
    //                 ValueProp.Unpowered, __instance.Owner);
    //         }
    //     }
    // }
    //
    // [UsedImplicitly]
    // [HarmonyPatch(typeof(BouncingFlask), "OnPlay")]
    // public class BouncingFlaskExcludeInsectsPatch
    // {
    //     private static readonly Color VfxTint = new Color("83eb85");
    //
    //     public static bool Prefix(BouncingFlask __instance, PlayerChoiceContext choiceContext,
    //         ref Task __result)
    //     {
    //         __result = RunFiltered(__instance, choiceContext);
    //         return false;
    //     }
    //
    //     private static async Task RunFiltered(BouncingFlask __instance, PlayerChoiceContext choiceContext)
    //     {
    //         await CreatureCmd.TriggerAnim(__instance.Owner.Creature, "Cast",
    //             __instance.Owner.Character.CastAnimDelay);
    //         Vector2 lastPos = Vector2.Zero;
    //
    //         for (int i = 0; i < __instance.DynamicVars.Repeat.IntValue; i++)
    //         {
    //             Creature enemy = HelperMethods.RandomNonInsectTarget(
    //                 __instance.CombatState,
    //                 __instance.Owner.RunState.Rng.CombatTargets);
    //             if (enemy == null) continue;
    //
    //             if (TestMode.IsOff)
    //             {
    //                 if (i == 0)
    //                     lastPos = NCombatRoom.Instance.GetCreatureNode(__instance.Owner.Creature).VfxSpawnPosition;
    //
    //                 NCreature targetNode = NCombatRoom.Instance.GetCreatureNode(enemy);
    //                 if (targetNode != null)
    //                 {
    //                     NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(
    //                         NItemThrowVfx.Create(lastPos, targetNode.GetBottomOfHitbox(),
    //                             ModelDb.Potion<PoisonPotion>().Image));
    //                     lastPos = targetNode.VfxSpawnPosition;
    //                     await Cmd.Wait(0.5f);
    //                     NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(
    //                         NSplashVfx.Create(targetNode.VfxSpawnPosition, VfxTint));
    //                     NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(
    //                         NLiquidOverlayVfx.Create(enemy, VfxTint));
    //                     NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(
    //                         NGaseousImpactVfx.Create(targetNode.VfxSpawnPosition, VfxTint));
    //                 }
    //             }
    //
    //             await PowerCmd.Apply<PoisonPower>(choiceContext, enemy,
    //                 __instance.DynamicVars.Poison.BaseValue, __instance.Owner.Creature, __instance);
    //         }
    //     }
    // }
    
    [HarmonyPatch(typeof(CombatState), nameof(CombatState.HittableEnemies), MethodType.Getter)]
    public static class ExcludeInsectsFromHittableEnemiesPatch
    {
        public static void Postfix(ref IReadOnlyList<Creature> __result)
        {
            __result = __result.Where(c => c.Monster is not IBroodmotherSummon).ToList();
        }
    }
}