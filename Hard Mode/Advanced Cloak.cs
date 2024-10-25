using HarmonyLib;
using static PulsarModLoader.Patches.HarmonyHelpers;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;

namespace Hard_Mode
{
    public class Advanced_Cloak
    {
        [HarmonyPatch(typeof(PLCloakingSystem), "Tick")]
        public class CloakPowerIncrease //Increases the base power of cloak when advanced cloak is enabled and decreases the percentage of power required to stay on
        {
            public static float percent = 0.75f;
            static void Postfix(PLCloakingSystem __instance)
            {
                if (Options.MasterHasMod && Options.AdvancedCloak)
                {
                    if (__instance.SubType == 0)
                    {
                        __instance.CalculatedMaxPowerUsage_Watts = 9000f;
                    }
                    else if (__instance.SubType == 1)
                    {
                        __instance.CalculatedMaxPowerUsage_Watts = 6750f;
                    }
                    percent = 0.5f;
                }
                else 
                {
                    if (__instance.SubType == 0)
                    {
                        __instance.CalculatedMaxPowerUsage_Watts = 6000f;
                    }
                    else if (__instance.SubType == 1)
                    {
                        __instance.CalculatedMaxPowerUsage_Watts = 4500f;
                    }
                    percent = 0.75f;
                }
            }
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> targetSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Ldc_R4, 0.75f),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(CloakPowerIncrease),"percent")),
                };
                patchSequence[0].labels = instructions.ToList()[FindSequence(instructions, targetSequence, CheckMode.NONNULL) - 1].labels;
                return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.REPLACE, CheckMode.NONNULL, false);
            }
        }

        [HarmonyPatch(typeof(PLShipStats), "CalculateStats")]
        class EMSignatureDecreaser //Decreases your EM signature based on cloak power percentage
        {
            static void Postfix(PLShipStats __instance) 
            {
                if(Options.MasterHasMod && Options.AdvancedCloak) 
                {
                    PLCloakingSystem cloak = __instance.Ship.MyCloakingSystem;
                    if (cloak != null && __instance.Ship.GetIsCloakingSystemActive()) 
                    {
                        __instance.EMSignature *=  1 - cloak.GetPowerPercentInput();
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLScientistComputerScreen),"Update")]
        class ScientistScreenFix // Fixes scientis screen showing 0 EM signature while cloaked
        {
            static void Postfix(PLScientistComputerScreen __instance, UILabel ___MainScreen_EMLabel, PLCachedFormatString<float> ___cMainScreen_EMLabel) 
            {
                if (Options.MasterHasMod && Options.AdvancedCloak)
                {
                    PLGlobal.SafeLabelSetText(___MainScreen_EMLabel, ___cMainScreen_EMLabel.ToString(__instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMSignature));
                }
            }
        }

        [HarmonyPatch(typeof(PLSensorObjectShip), "GetIsCloaked")]
        class Remove0EMSignature // Makes so your EM signature isn't just set to 0 when cloaked
        {
            static void Postfix(ref bool __result) 
            {
                if(Options.MasterHasMod && Options.AdvancedCloak) 
                {
                    __result = false;
                }
            }
        }

        [HarmonyPatch(typeof(PLShipInfo), "UpdateCloakingShipScreen")]
        class CloakNeedsPowerFix // Fixes cloak saying not enough power
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> targetSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Ldc_R4, 0.75f),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(CloakPowerIncrease),"percent")),
                };
                patchSequence[0].labels = instructions.ToList()[FindSequence(instructions, targetSequence, CheckMode.NONNULL) - 1].labels;
                return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.REPLACE, CheckMode.NONNULL, false);
            }
        }
    }
}
