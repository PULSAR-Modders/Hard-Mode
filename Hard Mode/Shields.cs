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
}
