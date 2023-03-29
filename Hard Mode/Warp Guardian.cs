using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace Hard_Mode
{
    class Warp_Guardian //All changes related to the warp guardian battle
    {
        [HarmonyPatch(typeof(PLWarpGuardian), "GetPlayerBasedDifficultyMultiplier")]
        class GuardianDifficultyPatch //This removes the multiplier on the guardian to allow more than 2X difficulty
        {
            static void Postfix(ref float __result)
            {
                if (PLEncounterManager.Instance.PlayerShip != null)
                {
                    __result = UnityEngine.Mathf.Min(1f + (PLEncounterManager.Instance.PlayerShip.GetCombatLevel() - 100) * 0.01f, 1f);
                    return;
                }
                __result = 1f;
            }

        }

        [HarmonyPatch(typeof(PLWarpGuardian), "Start")]
        class StartGuardian
        {
            static void Postfix(PLWarpGuardian __instance)
            {
                //This makes so the guardian starts with all components
                if (Options.MasterHasMod)
                {
                    __instance.SideCannonModule.Health = __instance.SideCannonModule.MaxHealth * 0.15f;
                    __instance.BoardingSystem.Health = __instance.BoardingSystem.MaxHealth * 0.20f;
                    __instance.ModuleRepairModule.Health = __instance.ModuleRepairModule.MaxHealth * 0.25f;
                    __instance.BoostModule.Health = __instance.BoostModule.MaxHealth * 0.85f;

                }
            }
        }

        [HarmonyPatch(typeof(PLWarpGuardian), "Update")]
        class UpdateGuardian
        {
            private static FieldInfo AllModules = AccessTools.Field(typeof(PLWarpGuardian), "AllModules");
            static void Postfix(PLWarpGuardian __instance)
            {
                //This makes so the guardian starts with all components
                if (Options.MasterHasMod)
                {
                    foreach (PLDamageableSpaceObject pldamageableSpaceObject2 in (List<PLDamageableSpaceObject>)AllModules.GetValue(__instance))
                    {
                        pldamageableSpaceObject2.HideVisuals = false;
                    }
                }
            }
        }
    }
}
