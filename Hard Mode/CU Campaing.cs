﻿using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection.Emit;
using static PulsarModLoader.Patches.HarmonyHelpers;

namespace Hard_Mode
{
    public class CU_Campaing
    {
        [HarmonyPatch(typeof(PLStopAsteroidEncounter), "Update")] //Decreases timer to 5 minutes
        public class MeteorMission
        {
            public static float timer = 300f;
            /*
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions)
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[282] = new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(MeteorMission), "timer"));
                instructionsList[576].opcode = OpCodes.Ldc_I4_S;
                instructionsList[576].operand = 0;
                return instructionsList.AsEnumerable();
            }
            */
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> targetSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(UnityEngine.Random),"get_value")),
                new CodeInstruction(OpCodes.Ldc_I4_M1),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(UnityEngine.Random),"get_value")),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                };

                instructions = PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.REPLACE, CheckMode.NONNULL, false);

                targetSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Ldc_R4, 600f),
                };
                patchSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(MeteorMission),"timer")),
                };
                return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.REPLACE, CheckMode.NONNULL, false);
            }
        }
        [HarmonyPatch(typeof(PLSlimeBoss), "Update")]
        class WastedWingSlimeUpdate
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //Makes the boss attack more fequently
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[406].operand = 2f;
                instructionsList[423].operand = 4f;
                return instructionsList.AsEnumerable();
            }
        }
        [HarmonyPatch(typeof(PLStalkerPawn), "Update")]
        class StalkerUpdate
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //Makes the stalker attack more fequently
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[208].operand = 1.5f;
                return instructionsList.AsEnumerable();
            }
        }
        [HarmonyPatch(typeof(PLInfectedScientist), "Start")]
        class WastedWingScientists
        {
            static void Postfix(PLInfectedScientist __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLCrystalBoss), "Start")]
        class TheSource
        {
            static void Postfix(PLCrystalBoss __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.Armor += PLServer.Instance.ChaosLevel * 10;
                }
            }
        }
        [HarmonyPatch(typeof(PLCrystalBoss), "UpdateAttacks")]
        class TheSourceUpdate
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //Makes the boss attack more fequently
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[332].operand = 3f;
                instructionsList[340].operand = 2f;
                instructionsList[346].operand = 1f;
                return instructionsList.AsEnumerable();
            }
        }
        [HarmonyPatch(typeof(PLWastedWingInfoBox), "Update")]
        public class TheSourceTimer
        {
            public static float timer = 360f;
            /*
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //Wasted Wing final boss starts with 6 minutes
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[32] = new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(TheSourceTimer), "timer"));
                return instructionsList.AsEnumerable();
            }
            */
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> targetSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldc_R4),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>
                {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(TheSourceTimer),"timer")),
                };
                return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.REPLACE, CheckMode.NONNULL, false);
            }
        }
    }
}
