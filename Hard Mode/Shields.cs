using HarmonyLib;

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
    /*
    [HarmonyPatch(typeof(PLShieldGenerator), "ShipUpdate")]
    class ShieldUsingPower 
    { 
        static void Postfix(PLShieldGenerator __instance) 
        {
            if (__instance != null)
            {
                __instance.IsPowerActive = true;
                __instance.RequestPowerUsage_Limit = __instance.CalculatedMaxPowerUsage_Watts * 0.25f;
                __instance.InputPower_Watts = __instance.CalculatedMaxPowerUsage_Watts * 0.25f;
                __instance.RequestPowerUsage_Percent = 1f;
            }
        }
    }
    */
}
