using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection.Emit;
using UnityEngine;
using PulsarModLoader.Patches;

namespace Hard_Mode
{
    class AOG_Campaing
    {
        [HarmonyPatch(typeof(PLVaultDoorMadmansMansion), "Update")]
        class MadmansMansionFinalTimer //At the last minute from the laser the enemies spawn 2x faster
        {
            static public float SpawnTimer = 4f;
            static void Postfix(ref float ___SecondsLeft_Countdown)
            {
                if (___SecondsLeft_Countdown < 60f)
                {
                    SpawnTimer = 2f;
                }
                Enemies.SpawnerModder.Health = 3 + (int)(PLServer.Instance.ChaosLevel * 1.5);
                Enemies.SpawnerModder.Pistoleer = 2 + (int)(PLServer.Instance.ChaosLevel * 1.2);
                Enemies.SpawnerModder.Armor = 5 + (int)(PLServer.Instance.ChaosLevel * 1.5);
            }
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> targetSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLVaultDoorMadmansMansion),"LastGuardSpawnedTime")),
                new CodeInstruction(OpCodes.Sub),
                new CodeInstruction(OpCodes.Ldc_R4, 5f)
            };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLVaultDoorMadmansMansion),"LastGuardSpawnedTime")),
                new CodeInstruction(OpCodes.Sub),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(MadmansMansionFinalTimer), "SpawnTimer"))
            };
                
                return HarmonyHelpers.PatchBySequence(instructions, targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }
        [HarmonyPatch(typeof(PLAssassinBot), "Start")]
        class AssassinBot
        {
            static void Postfix(PLAssassinBot __instance)
            {
                if (Options.MasterHasMod)
                {
                    Vector3 spawnpoint = __instance.transform.position;
                    foreach (PLTeleportationTargetInstance teleporter in UnityEngine.Object.FindObjectsOfType<PLTeleportationTargetInstance>())
                    {
                        if (teleporter.TeleporterTargetName == "Master Suite" && teleporter.Unlocked)
                        {
                            spawnpoint = new Vector3(269.0862f, -58.01f, -113.205f);
                            break;
                        }
                        if (teleporter.TeleporterTargetName == "Museum Entrance" && teleporter.Unlocked)
                        {
                            spawnpoint = new Vector3(239.5412f, -41.6382f, -211.9386f);
                        }
                    }
                    __instance.transform.position = spawnpoint;
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLAssassinBot), "Update")]
        class AssassinBotUpdate
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //Makes the bot respawn faster
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[259].operand = 5f;
                return instructionsList.AsEnumerable();
            }

        }
    }
}
