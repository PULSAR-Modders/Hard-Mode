using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection.Emit;
using PulsarPluginLoader.Patches;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
namespace Hard_Mode
{
    class Enemies //All creatures will be here to help with ballancing and changing stats
    {
        [HarmonyPatch(typeof(PLAirElemental), "Start")]
        class Tornados
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLAnt), "Start")]
        class Ant
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLAntArmored), "Start")]
        class ArmoredAnt
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLAntHeavy), "Start")]
        class HeavyAnt
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLAntRavager), "Start")]
        class RavangerAnt
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLBanditLandDrone), "Start")]
        class BanditLandDrone
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLBrainCreature), "Start")]
        class Brain
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLCUInvestigatorDrone), "Update")]
        class InestigationDrone
        {
            static void Postfix(PLCUInvestigatorDrone __instance)
            {
                if (PhotonNetwork.isMasterClient && PLInGameUI.Instance.BossUI_Target != __instance)
                {
                    PLServer.Instance.photonView.RPC("SetupNewHunter", PhotonTargets.All, new object[]
                        {
                                __instance.CombatTargetID,
                        });
                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedSpider), "Start")]
        class InfectedCrawlers
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedSpider_Medium), "Start")]
        class MediumInfectedSpider
        {
            static void Postfix(PLInfectedSpider_Medium __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    __instance.MaxHealth += 50 * PLServer.Instance.ChaosLevel;
                    __instance.Health = __instance.MaxHealth;
                    __instance.Armor += PLServer.Instance.ChaosLevel * 5;
                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedSpider_WG), "Start")]
        class GuardianInfectedSpider
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }


        [HarmonyPatch(typeof(PLInfectedSwarm), "Start")]
        class Dontknowwhatthisis
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLInfectedBoss), "Start")]
        class AlsoDontKnow
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLLCLabEnemy), "Start")]
        class ColonyGhost
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLRaptor), "Start")]
        class Raptor
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLRobotWalker), "Start")]
        class Paladin
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLRobotWalkerLarge), "Start")]
        class ElitPaladin
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLSpider), "Start")]
        class Spider
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLRat), "Start")]
        class Rat
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLSlime), "Start")]
        class Slime
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLWasteWasp), "Start")]
        class Wasp
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLSlimeBoss), "Start")]
        class WastedWingSlime
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
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
        [HarmonyPatch(typeof(PLStalkerPawn), "Start")]
        class Stalker
        {
            static void Postfix(PLStalkerPawn __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    __instance.MaxHealth += 50 * PLServer.Instance.ChaosLevel;
                    __instance.Health = __instance.MaxHealth;
                    __instance.Armor += PLServer.Instance.ChaosLevel * 5;
                }
            }
        }
        [HarmonyPatch(typeof(PLStalkerPawn), "Update")]
        class StalkerUpdate
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //Makes the boss attack more fequently
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
                if (PhotonNetwork.isMasterClient)
                {
                    __instance.MeleeDamage *= PLServer.Instance.ChaosLevel / 2;
                    __instance.MaxHealth += 50 * PLServer.Instance.ChaosLevel;
                    __instance.Health = __instance.MaxHealth;
                    __instance.Armor += PLServer.Instance.ChaosLevel * 5;
                }
            }
        }
        [HarmonyPatch(typeof(PLCrystalBoss), "Start")]
        class TheSource
        {
            static void Postfix(PLCrystalBoss __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    __instance.Armor += PLServer.Instance.ChaosLevel * 5;
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
        class TheSourceTimer
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //Wasted Wing final boss starts with 8 minutes
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[32].operand = 480f;
                return instructionsList.AsEnumerable();
            }
        }
        [HarmonyPatch(typeof(PLInfectedBoss_WDFlagship), "Start")]
        class MindSlaver
        {
            static void Postfix(PLInfectedBoss_WDFlagship __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    __instance.Armor += PLServer.Instance.ChaosLevel * 10;
                }
            }
        }

        [HarmonyPatch(typeof(PLInfectedHeart_WDFlagship), "Start")]
        class ForsakenFlagshipHeart
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLInfectedCrewmember), "Start")]
        class InfectedScientits
        {
            static void Postfix(PLInfectedCrewmember __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    __instance.MeleeDamage *= PLServer.Instance.ChaosLevel / 2;
                    __instance.MaxHealth += 50 * PLServer.Instance.ChaosLevel;
                    __instance.Health = __instance.MaxHealth;
                    __instance.Armor += PLServer.Instance.ChaosLevel * 2;
                }
            }
        }

        [HarmonyPatch(typeof(PLAssassinBot), "Start")]
        class AssassinBot
        {
            static void Postfix(PLAssassinBot __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    __instance.Armor += (int)(PLServer.Instance.ChaosLevel * 1.2);
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

        [HarmonyPatch(typeof(PLBoardingBot), "Start")]
        class BoardingBot
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLGiantRobotHead), "Start")]
        class DownedProtector
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLGroundTurret), "Start")]
        class GroundTurret
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLRoamingSecurityGuardRobot), "Start")]
        class MadmansMansionDrone
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }

            }
        }

        [HarmonyPatch(typeof(PLSmokeCreature), "Start")]
        class CyphersSmoke
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLPlayer), "Start")]
        class BanditsECrew
        {
            static void Postfix(PLPlayer __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    if (__instance.GetPlayerName() == "Bandit" && PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.AOG_MISSIONCHAIN_MADMANS_MANSION) //Guards from the Madman's Mansion
                    {
                        __instance.Talents[0] = 3 + (int)(PLServer.Instance.ChaosLevel * 1.5);
                        __instance.Talents[2] = 2 + (int)(PLServer.Instance.ChaosLevel * 1.2);
                        __instance.Talents[25] = 3;
                        __instance.Talents[49] = 12;
                        __instance.Talents[56] = 5 + (int)(PLServer.Instance.ChaosLevel * 1.5);
                        __instance.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 500 - Mathf.RoundToInt(PLServer.Instance.ChaosLevel * 60f * UnityEngine.Random.value));
                        if (random < 10)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            __instance.MyInventory.UpdateItem(ItemID, 9, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 20)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            __instance.MyInventory.UpdateItem(ItemID, 25, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 30)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            __instance.MyInventory.UpdateItem(ItemID, 12, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 40)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            __instance.MyInventory.UpdateItem(ItemID, 8, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 50)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            __instance.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 60)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            __instance.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 70)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            __instance.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            __instance.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLVaultDoorMadmansMansion), "Update")]
        class MadmansMansionFinalTimer //This makes the last minute from the laser enemies spawn 2x faster
        {
            static public float SpawnTimer = 5f;
            static void Postfix(ref float ___SecondsLeft_Countdown)
            {
                if (___SecondsLeft_Countdown < 60f)
                {
                    SpawnTimer = 2f;
                }
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
        [HarmonyPatch(typeof(PLSpawner), "DoSpawnStatic")]
        class TestOverpower
        {
            /*
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> targetSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldloc_S, 29),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLPlayer),"Talents")),
                new CodeInstruction(OpCodes.Ldc_I4_S, 56),
                new CodeInstruction(OpCodes.Ldc_I4_5),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ObscuredInt),"op_Implicit",new Type[]{typeof(int)})),
                new CodeInstruction(OpCodes.Stelem)
            };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldloc_S, 29),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLPlayer),"Talents")),
                new CodeInstruction(OpCodes.Ldc_I4_S, 49),
                new CodeInstruction(OpCodes.Ldc_I4_S, 12),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ObscuredInt),"op_Implicit",new Type[]{typeof(int)})),
                new CodeInstruction(OpCodes.Stelem)
            };
                return HarmonyHelpers.PatchBySequence(instructions, targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, true);
            }
            */
        }
    }
}
