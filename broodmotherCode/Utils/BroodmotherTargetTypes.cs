using BaseLib.Patches.Content;
using BaseLib.Patches.Features;
using Broodmother.broodmotherCode.Summons;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

public static class BroodmotherTargetTypes
{
    [CustomEnum] public static TargetType AnyInsect; 
}

[HarmonyPatch(typeof (ModelDb), "Init")]
public static class ModelDbTargetTypeInitPatch
{
    [HarmonyPostfix]
    public static void RegisterTargetTypes()
    {
        CustomTargetType.RegisterSingleTargetType(BroodmotherTargetTypes.AnyInsect, target => target is { IsAlive: true, IsEnemy: true, Monster: IBroodmotherSummon});
    }
}