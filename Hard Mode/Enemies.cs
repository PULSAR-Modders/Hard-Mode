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
    [HarmonyPatch(typeof(PLShipInfoBase), "GetChaosBoost",new Type[] {typeof(PLPersistantShipInfo),typeof(int)})]
    class ChaosBoost //This will make enemy ships spawn with higher level components
    {
        static int Postfix(int __result, PLPersistantShipInfo inPersistantShipInfo, int offset) 
        {
            if (PLServer.Instance != null && inPersistantShipInfo != null)
            {
                PLRand shipDeterministicRand = PLShipInfoBase.GetShipDeterministicRand(inPersistantShipInfo, 30 + offset);
                __result = Mathf.RoundToInt(PLServer.Instance.ChaosLevel * shipDeterministicRand.Next(0.5f, 0.9f) * shipDeterministicRand.Next(0.5f, 0.9f) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar + PLEncounterManager.Instance.PlayerShip.GetCombatLevel()/20);
                return __result;
            }
            __result = 0;
            return __result;
        }
    }
    [HarmonyPatch(typeof(PLServer), "ServerAddEnemyCrewBotPlayer")]
    class EnemyCrewSpawn //This will make all ship crew stronger
    { 
        static void Postfix(PLShipInfo inShip) 
        {
            foreach(PLPlayer component in PLServer.Instance.AllPlayers) 
            {
                if(component != null && component.StartingShip == inShip) 
                {
                    int chaos = (int)PLServer.Instance.ChaosLevel;
                    if (inShip.ShipTypeID == EShipType.E_INTREPID_SC)
                    {
                        component.Talents[56] = 5 + chaos;
                        component.Talents[58] = 5 + chaos;
                        component.Talents[0] = 5 + chaos;
                        component.Talents[57] = 5 + chaos;
                        component.Talents[2] = 5 + chaos;
                        component.Talents[3] = 8;
                        switch (component.GetClassID())
                        {
                            case 0:
                                component.Talents[27] = 8 + chaos;
                                component.Talents[5] = 5 + chaos;
                                component.Talents[47] = 8 + chaos;
                                component.Talents[50] = 8 + chaos;
                                break;
                            case 1:
                                component.Talents[36] = 12 + chaos;
                                component.Talents[35] = 12 + chaos;
                                component.Talents[8] = 5 + chaos;
                                component.Talents[9] = 8 + chaos;
                                break;
                            case 2:
                                component.Talents[13] = 5 + chaos;
                                component.Talents[11] = 5 + chaos;
                                component.Talents[12] = 5 + chaos;
                                break;
                            case 3:
                                component.Talents[38] = 8 + chaos;
                                component.Talents[39] = 8 + chaos;
                                component.Talents[23] = 5 + chaos;
                                component.Talents[17] = 5 + chaos;
                                component.Talents[25] = 5 + chaos;
                                component.Talents[62] = 5 + chaos;
                                break;
                            case 4:
                                component.Talents[20] = 5 + chaos;
                                component.Talents[19] = 25 + chaos;
                                component.Talents[21] = 25 + chaos;
                                component.Talents[45] = 8 + chaos;
                                component.Talents[61] = 5 + chaos;
                                break;
                        }
                    }
                    else if (inShip.ShipTypeID == EShipType.E_ALCHEMIST)
                    {
                        component.Talents[56] = 5 + chaos;
                        component.Talents[58] = 5 + chaos;
                        component.Talents[0] = 5 + chaos;
                        component.Talents[57] = 5 + chaos;
                        component.Talents[2] = 5 + chaos;
                        component.Talents[3] = 8;
                        switch (component.GetClassID())
                        {
                            case 0:
                                component.Talents[27] = 8 + chaos;
                                component.Talents[5] = 5 + chaos;
                                component.Talents[47] = 8 + chaos;
                                component.Talents[50] = 8 + chaos;
                                break;
                            case 1:
                                component.Talents[36] = 12 + chaos;
                                component.Talents[35] = 12 + chaos;
                                component.Talents[8] = 5 + chaos;
                                component.Talents[9] = 8 + chaos;
                                break;
                            case 2:
                                component.Talents[13] = 5 + chaos;
                                component.Talents[11] = 5 + chaos;
                                component.Talents[12] = 5 + chaos;
                                break;
                            case 3:
                                component.Talents[38] = 8 + chaos;
                                component.Talents[39] = 8 + chaos;
                                component.Talents[23] = 5 + chaos;
                                component.Talents[17] = 5 + chaos;
                                component.Talents[25] = 5 + chaos;
                                component.Talents[62] = 5 + chaos;
                                break;
                            case 4:
                                component.Talents[20] = 5 + chaos;
                                component.Talents[19] = 25 + chaos;
                                component.Talents[21] = 25 + chaos;
                                component.Talents[45] = 8 + chaos;
                                component.Talents[61] = 5 + chaos;
                                break;
                        }
                    }
                    else
                    {
                        component.Talents[56] = Mathf.RoundToInt(chaos * 2f * UnityEngine.Random.value);
                        component.Talents[58] = Mathf.RoundToInt(chaos * 2f * UnityEngine.Random.value);
                        component.Talents[0] = Mathf.RoundToInt(chaos * 2f * UnityEngine.Random.value);
                        component.Talents[57] = Mathf.RoundToInt(chaos * 2f * UnityEngine.Random.value);
                        component.Talents[48] = Mathf.RoundToInt(chaos * 2f * UnityEngine.Random.value);
                        component.Talents[3] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                        switch (component.GetClassID())
                        {
                            case 0:
                                component.Talents[47] = Mathf.RoundToInt(chaos * 2f * UnityEngine.Random.value);
                                component.Talents[27] = Mathf.RoundToInt(chaos * 2f * UnityEngine.Random.value);
                                component.Talents[5] = Mathf.RoundToInt(chaos * 2f * UnityEngine.Random.value);
                                component.Talents[50] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                break;
                            case 1:
                                component.Talents[36] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[35] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[8] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[9] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                break;
                            case 2:
                                component.Talents[13] = Mathf.RoundToInt(chaos * 2f * UnityEngine.Random.value);
                                component.Talents[11] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[12] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                break;
                            case 3:
                                component.Talents[38] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[39] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[23] = Mathf.RoundToInt(chaos * 2f * UnityEngine.Random.value);
                                component.Talents[17] = Mathf.RoundToInt(chaos * 0.5f * UnityEngine.Random.value);
                                component.Talents[25] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[62] = Mathf.RoundToInt(chaos * 2f * UnityEngine.Random.value);
                                break;
                            case 4:
                                component.Talents[20] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[19] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[21] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[45] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[61] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                break;
                        }
                    }
                    component.MyInventory.Clear();
                    int random = UnityEngine.Random.Range(0, 500 - Mathf.RoundToInt(PLServer.Instance.ChaosLevel * 60f * UnityEngine.Random.value));
                    if (random < 10)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 26, 0, (int)PLServer.Instance.ChaosLevel, 1);
                    }
                    else if(random < 20)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 9, 0, (int)PLServer.Instance.ChaosLevel, 1);
                    }
                    else if (random < 30)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 25, 0, (int)PLServer.Instance.ChaosLevel, 1);
                    }
                    else if (random < 40)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 12, 0, (int)PLServer.Instance.ChaosLevel, 1);
                    }
                    else if (random < 50)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 8, 0, (int)PLServer.Instance.ChaosLevel, 1);
                    }
                    else if (random < 100)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel, 1);
                    }
                    else if (random < 150)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel, 1);
                    }
                    else if (random < 200)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel, 1);
                    }
                    else
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel+1, 1);
                    }
                    int ItemID2 = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID2 + 1;
                    component.MyInventory.UpdateItem(ItemID2, 3, 0, (int)PLServer.Instance.ChaosLevel, 2);
                    ItemID2 = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID2 + 1;
                    component.MyInventory.UpdateItem(ItemID2, 4, 0, (int)PLServer.Instance.ChaosLevel, 3);
                    if(component.GetClassID() == 2) 
                    {
                        ItemID2 = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID2 + 1;
                        component.MyInventory.UpdateItem(ItemID2, 26, 0, (int)PLServer.Instance.ChaosLevel, 4);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(PLShipInfoBase),"GetCombatLevel")]
    class CombatLevel //Modify the combat level calculation
    {
        static float Postfix(float __result,PLShipInfoBase __instance) 
        {
            __result = 0f;
            foreach (PLShipComponent plshipComponent in __instance.MyStats.AllComponents)
            {
                if (plshipComponent != null && plshipComponent.IsEquipped)
                {
                    __result += Mathf.Pow((float)plshipComponent.GetScaledMarketPrice(true), 0.8f) * 0.001f;
                    if(plshipComponent.ActualSlotType == ESlotType.E_COMP_MAINTURRET || plshipComponent.ActualSlotType == ESlotType.E_COMP_TURRET || plshipComponent.ActualSlotType == ESlotType.E_COMP_AUTO_TURRET) 
                    {
                        PLTurret turret = plshipComponent as PLTurret;
                        __result += turret.GetDPS() * 0.1f * (1f - Mathf.Clamp01(turret.HeatGeneratedOnFire * 2.2f / turret.FireDelay));
                        __result += turret.TurretRange * 0.0005f;
                    }
                }
            }
            __result += __instance.MyStats.HullMax * 0.005f;
            __result += __instance.MyStats.HullArmor * 10f;
            __result += __instance.MyStats.ShieldsMax * 0.01f;
            if (!__instance.InWarp)
            {
                __result += __instance.MyStats.ShieldsChargeRateMax * 0.1f;
            }
            else
            {
                __result += __instance.MyStats.ShieldsChargeRateMax * 0.1f * 0.1f;
            }
            __result += __instance.MyStats.ThrustOutputMax * 0.01f;
            __result += __instance.MyStats.ReactorOutputMax * 0.0005f;
            return __result;
        }
    }
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
            static void Postfix(PLAnt __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    __instance.Armor += PLServer.Instance.ChaosLevel*3;
                }
            }
        }
        [HarmonyPatch(typeof(PLAntArmored), "Start")]
        class ArmoredAnt
        {
            static void Postfix(PLAntArmored __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    __instance.Armor += PLServer.Instance.ChaosLevel * 5;
                }
            }
        }
        [HarmonyPatch(typeof(PLAntHeavy), "Start")]
        class HeavyAnt
        {
            static void Postfix(PLAntHeavy __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    __instance.Armor += PLServer.Instance.ChaosLevel * 3;
                }
            }
        }
        [HarmonyPatch(typeof(PLAntRavager), "Start")]
        class RavangerAnt
        {
            static void Postfix(PLAntRavager __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    __instance.Armor += PLServer.Instance.ChaosLevel * 4;
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
            static void Postfix(PLRoamingSecurityGuardRobot __instance)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    __instance.MaxHealth *= PLServer.Instance.ChaosLevel;
                    __instance.Health = __instance.MaxHealth;
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
        [HarmonyPatch(typeof(PLTeleporterUnlocker_RangeBased), "Update")]
        class ForsakenFlagshipReactorTeleport //This should disable the teleporter near the reactor room
        {
            static void Postfix(ref PLTeleportationTargetInstance ___MyTTI)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    if (___MyTTI.TeleporterTargetName.Contains("Reactor")) 
                    {
                        ___MyTTI.Unlocked = false;
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
        class SpawnerModder // Could be used to directly change values in the spawner
        {/*
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) 
            {
                
                List<CodeInstruction> targetSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldloc_S),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLPlayer),"Talents")),
                new CodeInstruction(OpCodes.Ldc_I4_S),
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
                new CodeInstruction(OpCodes.Stelem, typeof(ObscuredInt))
            };
                return HarmonyHelpers.PatchBySequence(instructions, targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
            }
           */
        }

        [HarmonyPatch(typeof(PLPersistantEncounterInstance),"PlayerEnter")]
        class InfectedCarriersOnWars //Enables Infected Carriers to spawn in combat zones with the infected team
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> targetSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLServer),"Instance")),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLServer),"get_AllPSIs")),
                new CodeInstruction(OpCodes.Ldc_I4_S),
                new CodeInstruction(OpCodes.Ldloc_S),
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldloc_S),
                new CodeInstruction(OpCodes.Ldc_I4_M1),
                new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(PLPersistantShipInfo), new Type[]{typeof(EShipType),typeof(int),typeof(PLSectorInfo),typeof(int),typeof(bool),typeof(bool),typeof(bool),typeof(int),typeof(int) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<PLPersistantShipInfo>),"Add")),
            };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLServer),"Instance")),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLServer),"get_AllPSIs")),
                new CodeInstruction(OpCodes.Ldc_I4_S, 0xC),
                new CodeInstruction(OpCodes.Ldloc_S, 7),
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldloc_S, 6),
                new CodeInstruction(OpCodes.Ldc_I4_M1),
                new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(PLPersistantShipInfo), new Type[]{typeof(EShipType),typeof(int),typeof(PLSectorInfo),typeof(int),typeof(bool),typeof(bool),typeof(bool),typeof(int),typeof(int) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<PLPersistantShipInfo>),"Add")),
            };
                return HarmonyHelpers.PatchBySequence(instructions, targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }
        [HarmonyPatch(typeof(PLPersistantEncounterInstance), "PlayerEnter")]
        class InfectedCarriersOnWars2 //Enables Infected Carriers to spawn in combat zones with the infected team, there are 2 separated parts that can spawn it, so just to be sure
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> targetSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ble_Un),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLServer),"Instance")),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLServer),"get_AllPSIs")),
                new CodeInstruction(OpCodes.Ldc_I4_S),
                new CodeInstruction(OpCodes.Ldloc_S),
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldloc_S),
                new CodeInstruction(OpCodes.Ldc_I4_M1),
                new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(PLPersistantShipInfo), new Type[]{typeof(EShipType),typeof(int),typeof(PLSectorInfo),typeof(int),typeof(bool),typeof(bool),typeof(bool),typeof(int),typeof(int) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<PLPersistantShipInfo>),"Add")),
            };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLServer),"Instance")),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLServer),"get_AllPSIs")),
                new CodeInstruction(OpCodes.Ldc_I4_S, 0xC),
                new CodeInstruction(OpCodes.Ldloc_S, 7),
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldloc_S, 6),
                new CodeInstruction(OpCodes.Ldc_I4_M1),
                new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(PLPersistantShipInfo), new Type[]{typeof(EShipType),typeof(int),typeof(PLSectorInfo),typeof(int),typeof(bool),typeof(bool),typeof(bool),typeof(int),typeof(int) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<PLPersistantShipInfo>),"Add")),
            };
                return HarmonyHelpers.PatchBySequence(instructions, targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }
    }
}
