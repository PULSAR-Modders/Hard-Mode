using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using System.Reflection.Emit;
using PulsarPluginLoader.Patches;
using static PulsarPluginLoader.Patches.HarmonyHelpers;
using System.Linq;

namespace Hard_Mode
{
    class Custom_Bounty_Hunters
    {
        [HarmonyPatch(typeof(PLEncounterManager), "Start")]
        class HunterAdder //Adds the extra hunters from the ShipExport.txt for the random list when the game starts 
        {
            static void Postfix(ref List<PLEncounterManager.ShipLayout> ___PossibleHunters_LayoutData) 
            {
                using (StreamReader streamReader = new StreamReader(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ShipExport.txt")))
                {
                    while (!streamReader.EndOfStream) 
                    {
                        if(streamReader.ReadLine() == "- - - Finish Components - - -") 
                        {
                            string bountyHunter = streamReader.ReadLine();
                            ___PossibleHunters_LayoutData.Add(new PLEncounterManager.ShipLayout(bountyHunter));
                        }
                    }
                }
            }
        }
        [HarmonyPatch(typeof(PLPersistantEncounterInstance), "PlayerEnter")]
        class RelicHunterBalance //Allow to change the Min and Max values for the difference between you and the relic hunter
        {
            static public float MinCombatLevel = 1.2f;
            static public float MaxCombatLevel = 1.5f;
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> targetSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLEncounterManager),"Instance")),
                new CodeInstruction(OpCodes.Ldc_R4,1.2f),
                new CodeInstruction(OpCodes.Ldc_R4,1.5f)
            };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLEncounterManager),"Instance")),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(RelicHunterBalance), "MinCombatLevel")),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(RelicHunterBalance), "MaxCombatLevel"))
            };
                return HarmonyHelpers.PatchBySequence(instructions, targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }
        [HarmonyPatch(typeof(PLServer), "SpawnHunter")]
        class BountyHunterBalance //Allow to change the Min and Max values for the difference between you and the bounty hunter
        {
            static public float MinCombatLevel = 1.2f;
            static public float MaxCombatLevel = 1.5f;
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> targetSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLEncounterManager),"Instance")),
                new CodeInstruction(OpCodes.Ldloc_3),
                new CodeInstruction(OpCodes.Ldloc_S),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLEncounterManager),"GetRandomPossibleHunterLayout")),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLEncounterManager),"Instance")),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(BountyHunterBalance), "MinCombatLevel")),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(BountyHunterBalance), "MaxCombatLevel")),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLEncounterManager),"GetRandomPossibleHunterLayout")),
                };
                patchSequence[0].labels = instructions.ToList()[FindSequence(instructions, targetSequence, CheckMode.NONNULL)-4].labels;
                return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.REPLACE, CheckMode.NONNULL, false);
            }
        }
    }
}
