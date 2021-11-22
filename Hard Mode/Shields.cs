using HarmonyLib;
using UnityEngine;
namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLWarpDriveProgram), "ExecuteBasedOnType")]
    class ShockSystem
    {
        static void Postfix(PLWarpDriveProgram __instance) // This will make the shock the shields wait 10 seconds before losing integrity when using shock the system
        {
            if ((EWarpDriveProgramType)__instance.SubType == EWarpDriveProgramType.SHOCK_THE_SYSTEM)
            {
                Update.timer = 10f;
            }
        }
    }
    [HarmonyPatch(typeof(PLShieldGenerator), "Tick")]
    class ShieldUsingPower 
    { 
        static void Postfix(PLShieldGenerator __instance) 
        {
            if (__instance != null && __instance.IsEquipped && Options.MasterHasMod && !__instance.ShipStats.Ship.IsDrone)
            {
                __instance.IsPowerActive = true;
                if (__instance.Current >= __instance.CurrentMax)
                {
                    if(__instance.RequestPowerUsage_Percent == 0 || __instance.RequestPowerUsage_Percent == 0.15f) __instance.RequestPowerUsage_Percent = 0.15f;
                    if (__instance.Current > __instance.CurrentMax) __instance.Current = __instance.CurrentMax;
                }
                if(__instance.GetPowerPercentInput() < 0.15f) 
                {
                    __instance.ChargeRateCurrent = __instance.ChargeRateMax * __instance.LevelMultiplier(0.5f, 1f) * (1 - __instance.GetPowerPercentInput()) * -3 ;
                    if (__instance.Current - __instance.ShipStats.ShieldsChargeRate * Time.deltaTime <= 0 && __instance.ChargeRateCurrent < 0) __instance.ShipStats.ShieldsChargeRate = 0f;
                    if (__instance.Current < __instance.ChargeRateMax/5 && __instance.ChargeRateCurrent < 0)
                    {
                        __instance.Current = 0f;
                        __instance.ChargeRateCurrent = 0;
                    }
                }
            }
        }
    }
}
