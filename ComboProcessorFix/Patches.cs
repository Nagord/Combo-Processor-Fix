using HarmonyLib;

namespace ComboProcessorFix
{
    [HarmonyPatch(typeof(PLCPU))]
    class Patches
    {
        [HarmonyPatch("Tick"), HarmonyPostfix]
        static void PowerPatch(PLCPU __instance, ref float ___m_RequestPowerUsage_Percent)
        {
            //Return early if Mod is disabled, Processor is not combo, stats are null.
            if (!Mod.IsRunning || __instance.CPUClass != ECPUClass.COMBO || __instance.ShipStats == null || __instance.ShipStats.Ship == null) return;

            //Reduces power consumption if not charging warp
            if (__instance.ShipStats.Ship.WarpChargeStage != EWarpChargeStage.E_WCS_PREPPING)
            {
                ___m_RequestPowerUsage_Percent *= 0.5f;
            }
        }

        [HarmonyPatch("AddStats"), HarmonyPrefix]
        static bool ComboFunctionPatch(PLCPU __instance, PLShipStats inStats)
        {
            //Stop early if mod disabled, processor not combo, stats are null.
            if (!Mod.IsRunning || __instance.CPUClass != ECPUClass.COMBO || inStats.Ship == null) return true;

            //Sets CD rate to appropriate power percent when not charging warp.
            if (inStats.Ship.WarpChargeStage != EWarpChargeStage.E_WCS_PREPPING)
            {
                inStats.CyberDefenseRating += 0.375f * __instance.LevelMultiplier(0.75f, 1f) * (__instance.GetPowerPercentInput() / 0.3125f);
                return false;
            }
            return true;
        }

        [HarmonyPatch("ShowValueWithPowerLevelApplied"), HarmonyPrefix]
        static bool ComboJPStatLinePatch(PLCPU __instance, float value, ref string __result)
        {
            if (Mod.IsRunning && __instance.CPUClass == ECPUClass.COMBO && __instance.ShipStats != null && __instance.ShipStats.Ship != null && __instance.ShipStats.Ship.WarpChargeStage != EWarpChargeStage.E_WCS_PREPPING && !__instance.ShipStats.isPreview && !__instance.InCargoSlot())
            {
                //JP portion
                __result = "0/" + value.ToString("0");
                return false;
            }
            return true;
        }

        [HarmonyPatch("ShowValueWithPowerLevelApplied_Decimal"), HarmonyPrefix]
        static bool ComboCDStatLinePatch(PLCPU __instance, float value, ref string __result)
        {
            if (Mod.IsRunning && __instance.CPUClass == ECPUClass.COMBO && __instance.ShipStats != null && __instance.ShipStats.Ship != null && __instance.ShipStats.Ship.WarpChargeStage != EWarpChargeStage.E_WCS_PREPPING && !__instance.ShipStats.isPreview && !__instance.InCargoSlot())
            {
                //CD portion
                __result = (value * (__instance.GetPowerPercentInput() / 0.3125f)).ToString("0.0") + "/" + value.ToString("0.0");
                return false;
            }
            return true;
        }
    }
}
