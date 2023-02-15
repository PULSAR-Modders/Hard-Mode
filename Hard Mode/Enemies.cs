using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using HarmonyLib;
using System.Reflection.Emit;
using PulsarModLoader.Patches;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using static PulsarModLoader.Patches.HarmonyHelpers;
namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLShipInfoBase), "GetChaosBoost", new Type[] { typeof(PLPersistantShipInfo), typeof(int) })]
    class ChaosBoost //This will make enemy ships spawn with higher level components
    {
        static int Postfix(int __result, PLPersistantShipInfo inPersistantShipInfo, int offset)
        {
            if (PLServer.Instance != null && inPersistantShipInfo != null)
            {
                PLRand shipDeterministicRand = PLShipInfoBase.GetShipDeterministicRand(inPersistantShipInfo, 30 + offset);
                __result = Mathf.RoundToInt(PLServer.Instance.ChaosLevel * shipDeterministicRand.Next(0.5f, 0.9f) * shipDeterministicRand.Next(0.5f, 0.9f) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar + PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 20);
                return __result;
            }
            __result = 0;
            return __result;
        }
    }
    [HarmonyPatch(typeof(PLServer), "ServerAddEnemyCrewBotPlayer")]
    class EnemyCrewSpawn //This will make all ship crew stronger
    {
        static void Postfix()
        {
            foreach (PLPlayer component in PLServer.Instance.AllPlayers)
            {
                if (component != null && component.gameObject.name == "Simple Combat Bot Player" && component.StartingShip != null)
                {
                    if (component.StartingShip.IsRelicHunter || component.StartingShip.IsBountyHunter) continue;
                    int chaos = (int)PLServer.Instance.ChaosLevel + UnityEngine.Random.Range(0, 4);
                    if (component.StartingShip.ShipTypeID == EShipType.E_INTREPID_SC)
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
                        chaos += 2;
                    }
                    else if (component.StartingShip.ShipTypeID == EShipType.E_ALCHEMIST)
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
                        chaos += 2;
                    }
                    else
                    {
                        component.Talents[56] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                        component.Talents[58] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                        component.Talents[0] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                        component.Talents[57] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                        component.Talents[48] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                        component.Talents[3] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                        switch (component.GetClassID())
                        {
                            case 0:
                                component.Talents[47] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[27] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[5] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[50] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                break;
                            case 1:
                                component.Talents[36] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[35] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[8] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[9] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                break;
                            case 2:
                                component.Talents[13] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[11] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[12] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                break;
                            case 3:
                                component.Talents[38] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[39] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[23] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[17] = Mathf.RoundToInt(chaos * 0.5f * UnityEngine.Random.value);
                                component.Talents[25] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
                                component.Talents[62] = Mathf.RoundToInt(chaos * UnityEngine.Random.value);
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
                        component.MyInventory.UpdateItem(ItemID, 26, 0, chaos, 1);
                    }
                    else if (random < 20)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 9, 0, chaos, 1);
                    }
                    else if (random < 30)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 25, 0, chaos, 1);
                    }
                    else if (random < 40)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 12, 0, chaos, 1);
                    }
                    else if (random < 50)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 8, 0, chaos, 1);
                    }
                    else if (random < 100)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 7, 0, chaos, 1);
                    }
                    else if (random < 150)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 10, 0, chaos, 1);
                    }
                    else if (random < 200)
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 11, 0, chaos, 1);
                    }
                    else
                    {
                        int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                        component.MyInventory.UpdateItem(ItemID, 2, 0, chaos + 1, 1);
                    }
                    int ItemID2 = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID2 + 1;
                    component.MyInventory.UpdateItem(ItemID2, 3, 0, chaos, 2);
                    ItemID2 = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID2 + 1;
                    component.MyInventory.UpdateItem(ItemID2, 4, 0, chaos, 3);
                    if (component.GetClassID() == 2)
                    {
                        ItemID2 = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID2 + 1;
                        component.MyInventory.UpdateItem(ItemID2, 26, 0, chaos, 4);
                    }
                    component.gameObject.name = "Simple Combat Bot Player Modded";
                }
            }
        }
    }

    [HarmonyPatch(typeof(PLShipInfoBase), "GetCombatLevel")]
    class CombatLevel //Modify the combat level calculation
    {
        static float Postfix(float __result, PLShipInfoBase __instance)
        {
            if (!Options.MasterHasMod)
            {
                return __result;
            }
            __result = 0f;
            foreach (PLShipComponent plshipComponent in __instance.MyStats.AllComponents)
            {
                if (plshipComponent != null && plshipComponent.IsEquipped)
                {
                    __result += Mathf.Pow((float)plshipComponent.GetScaledMarketPrice(true), 0.8f) * 0.001f;
                    if (plshipComponent.ActualSlotType == ESlotType.E_COMP_MAINTURRET || plshipComponent.ActualSlotType == ESlotType.E_COMP_TURRET || plshipComponent.ActualSlotType == ESlotType.E_COMP_AUTO_TURRET)
                    {
                        if (plshipComponent is PLAbyssTurret)
                        {
                            PLAbyssTurret abyssturret = plshipComponent as PLAbyssTurret;
                            __result += (abyssturret.damage_Normal * abyssturret.LevelMultiplier(0.15f,1f) * (abyssturret.ShipStats != null ? abyssturret.ShipStats.TurretDamageFactor : 1))/(abyssturret.FireDelay/ ((abyssturret.ShipStats != null) ? abyssturret.ShipStats.TurretChargeFactor : 1f)) * 0.1f * (1f - Mathf.Clamp01(abyssturret.HeatGeneratedOnFire * 2.2f / abyssturret.FireDelay));
                            __result += abyssturret.TurretRange * 0.0005f;
                            continue;
                        }
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
                if (Options.MasterHasMod)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLAnt), "Start")]
        class Ant
        {
            static void Postfix(PLAnt __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel/6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLAntArmored), "Start")]
        class ArmoredAnt
        {
            static void Postfix(PLAntArmored __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLAntHeavy), "Start")]
        class HeavyAnt
        {
            static void Postfix(PLAntHeavy __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLAntRavager), "Start")]
        class RavangerAnt
        {
            static void Postfix(PLAntRavager __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                    __instance.m_RangedDamage += PLServer.Instance.ChaosLevel * 5;
                }
            }
        }
        [HarmonyPatch(typeof(PLBanditLandDrone), "Start")]
        class BanditLandDrone
        {
            static void Postfix(PLBanditLandDrone __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLBrainCreature), "Start")]
        class Brain
        {
            static void Postfix(PLBrainCreature __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedSpider), "Start")]
        class InfectedCrawlers
        {
            static void Postfix(PLInfectedSpider __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;

                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedSwarm), "Start")]
        class Impossibility
        {
            static void Postfix(PLInfectedSwarm __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedBoss), "Start")]
        class AlsoDontKnow
        {
            static void Postfix()
            {
                if (Options.MasterHasMod)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLLCLabEnemy), "Start")]
        class ColonyGhost
        {
            static void Postfix(PLLCLabEnemy __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLRaptor), "Start")]
        class Raptor
        {
            static void Postfix(PLRaptor __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLRobotWalker), "Start")]
        class Paladin
        {
            static void Postfix(PLRobotWalker __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLRobotWalkerLarge), "Start")]
        class ElitPaladin
        {
            static void Postfix(PLRobotWalkerLarge __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLSpider), "Start")]
        class Spider
        {
            static void Postfix(PLSpider __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLRat), "Start")]
        class Rat
        {
            static void Postfix(PLRat __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLSlime), "Start")]
        class Slime
        {
            static void Postfix(PLSlime __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.moveSpeed += __instance.moveSpeed * PLServer.Instance.ChaosLevel / 2;
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLWasteWasp), "Start")]
        class Wasp
        {
            static void Postfix(PLWasteWasp __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLBoardingBot), "Start")]
        class BoardingBot
        {
            static void Postfix(PLBoardingBot __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                }
            }
        }
        [HarmonyPatch(typeof(PLGroundTurret), "Start")]
        class GroundTurret
        {
            static void Postfix(PLGroundTurret __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                }
            }
        }
        [HarmonyPatch(typeof(PLSmokeCreature), "Start")]
        class CyphersSmoke
        {
            static void Postfix()
            {
                if (Options.MasterHasMod)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLPlayer), "Start")]
        class BanditsECrew
        {
            static void Postfix(PLPlayer __instance)
            {
                if (Options.MasterHasMod)
                {
                }
            }
        }
        [HarmonyPatch(typeof(PLNullpoint),"Start")] //This are the green things that spread green fire on the unseen mothership 
        class NullPoint 
        {
            static void Postfix(PLNullpoint __instance) 
            {
                if (Options.MasterHasMod) 
                {
                    __instance.SpeedMultiplier = 125f;
                }
            }
        }
        [HarmonyPatch(typeof(PLUnseenEye), "FireEnergyProj")]
        public class UnseenEyePhysicalAttack 
        {
            /*
            public static float damage = 400f;
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions)
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                return instructionsList.AsEnumerable();
            }
            static void Postfix() 
            {
                PulsarModLoader.Utilities.Messaging.Notification("test");
                foreach(PLCurvedProjectile projectile in UnityEngine.Object.FindObjectsOfType(typeof(PLCurvedProjectile))) 
                {
                    projectile.Damage = damage;
                    projectile.ForwardForce *= 5f;
                }
            }
            */
        }
        [HarmonyPatch(typeof(PLUnseenEye), "ClientPerformBeamAttackOne")]
        class ReplaceUnseenEyeProjectile 
        {
            static bool Prefix(PLUnseenEye __instance) 
            {
                if (Options.MasterHasMod)
                {
                    __instance.StartCoroutine(UnseenEyeAttack.PerformPhysicalAttack(__instance));
                    return false;
                }
                else 
                {
                    return true;
                }
            }
        }
        public class UnseenEyeAttack 
        {
            public static IEnumerator PerformPhysicalAttack(PLUnseenEye __instance) 
            {
                if (__instance.Animator.GetBool("IsDead"))
                {
                    yield break;
                }
                PLMusic.PostEvent("play_sx_ship_enemy_unseeneye_attackone_windup", __instance.gameObject);
                __instance.Animator.SetBool("IsCharging", true);
                __instance.BeamOne_WindUp.Play(true);
                yield return new WaitForSeconds(3f);
                __instance.Animator.SetBool("IsCharging", false);
                __instance.Animator.SetBool("IsUsingBeam", true);
                __instance.BeamOne_MainPhase.Play(true);
                PLMusic.PostEvent("play_sx_ship_enemy_unseeneye_attackone_beam", __instance.gameObject);

                
                Vector3 angVel = UnityEngine.Random.insideUnitSphere * 110f;
                int num;
                for (int i = 0; i < 500; i = num + 1)
                {
                    if (__instance.TargetShip != null && __instance.TargetShip.Exterior != null)
                    {
                        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(__instance.EnergyProjPrefab, __instance.BeamFireLoc.position, __instance.BeamFireLoc.rotation);
                        gameObject.SetActive(true);
                        PLCurvedProjectile component = gameObject.GetComponent<PLCurvedProjectile>();
                        float d = 520f;
                        Vector3 b = UnityEngine.Random.insideUnitSphere * (__instance.PlayerShip_IsPilotedByAI() ? 20f : 10f) * 7f;
                        PLDriftTowardPlayerShip component2 = gameObject.GetComponent<PLDriftTowardPlayerShip>();
                        if (__instance.PlayerShip_IsPilotedByAI())
                        {
                            if (component2 != null)
                            {
                                component2.ForceScale *= 0.66f;
                            }
                        }
                        component2.ForceScale *= 5;
                        Vector3 normalized = (__instance.TargetShip.Exterior.transform.position - __instance.BeamFireLoc.position).normalized;
                        Vector3 a = Vector3.Lerp(__instance.BeamFireLoc.forward, normalized, 1f - Mathf.Clamp(Vector3.Angle(__instance.BeamFireLoc.forward, normalized) * 0.125f - 5f, 0f, 0.50f));
                        a += PLEncounterManager.Instance.PlayerShip.ExteriorRigidbody.velocity * (1f + UnityEngine.Random.value) * 8f;
                        if (__instance.TargetShip != null)
                        {
                            component.GetComponent<Rigidbody>().velocity = a * d + b;
                        }
                        component.ProjID = -40000 - __instance.energyProjCounter;
                        __instance.energyProjCounter++;
                        component.Damage = 400f * PLWarpGuardian.GetPlayerBasedDifficultyMultiplier();
                        component.MaxLifetime = 11f;
                        component.OwnerShipID = __instance.ShipID;
                        component.TurretID = -1;
                        component.MyDamageType = EDamageType.E_ENERGY;
                        component.ForwardForce *= 5f;
                        component.AngVel = angVel;
                        component.Target = __instance.TargetShip;
                        PLServer.Instance.m_ActiveProjectiles.Add(component);
                    }
                    yield return 0;
                    num = i;
                }
                yield return new WaitForSeconds(20f);



                __instance.Animator.SetBool("IsUsingBeam", false);
                yield break;
            }
        }
        [HarmonyPatch(typeof(PLUnseenEye),"Update")]
        class EyePhysicalAim 
        {
            static void Postfix(PLUnseenEye __instance) 
            {
                if (__instance.BeamOne_MainPhase.isPlaying) 
                {
                    /*
                    PLShipInfoBase target = PLEncounterManager.Instance.PlayerShip;
                    Vector3 a = target.GetSpaceLoc();
                    PLShipInfoBase plshipInfoBase = target as PLShipInfoBase;
                    if (plshipInfoBase != null && plshipInfoBase.ExteriorRigidbody != null)
                    {
                        a += plshipInfoBase.ExteriorRigidbody.velocity * 2f;
                    }
                    __instance.BeamFireLoc.position = (a - __instance.BeamFireLoc.position).normalized;
                    */
                }
            }
        }
        [HarmonyPatch(typeof(PLSpawner), "DoSpawnStatic")]
        public class SpawnerModder // Could be used to directly change values in the spawner
        {
            public static int Health = 0;
            public static int Pistoleer = 0;
            public static int Armored_Skin = 3;
            public static int Reloader = 12;
            public static int Armor = 0;
            static void Postfix()
            {
                foreach (PLPlayer player in PLServer.Instance.AllPlayers)
                {
                    if (player != null && player.gameObject.name == "Simple Combat Bot Player" && PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.AOG_MISSIONCHAIN_MADMANS_MANSION)
                    {
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 500 - Mathf.RoundToInt(PLServer.Instance.ChaosLevel * 60f * UnityEngine.Random.value));
                        if (random < 10)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 9, 0, (int)PLServer.Instance.ChaosLevel/2 + 2, 1);
                        }
                        else if (random < 20)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 25, 0, (int)PLServer.Instance.ChaosLevel/2 + 2, 1);
                        }
                        else if (random < 30)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 12, 0, (int)PLServer.Instance.ChaosLevel/2 + 2, 1);
                        }
                        else if (random < 40)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 8, 0, (int)PLServer.Instance.ChaosLevel/2 + 2, 1);
                        }
                        else if (random < 50)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel/2 + 2, 1);
                        }
                        else if (random < 60)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel/2 + 2, 1);
                        }
                        else if (random < 70)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel/2 + 2, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel/2 + 2, 1);
                        }
                        player.gameObject.name = "Simple Combat Bot Player Modded";
                    }
                    else if (player != null && player.gameObject.name == "Simple Combat Bot Player" && player.GetPlayerName() == "Heavy Metal Bandit")
                    {
                        player.Talents[0] += (int)(PLServer.Instance.ChaosLevel * 1.2);
                        player.Talents[2] += (int)(PLServer.Instance.ChaosLevel * 0.8);
                        player.Talents[56] += (int)(PLServer.Instance.ChaosLevel * 0.8);
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 140);
                        if (random < 90)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 9, 0, (int)PLServer.Instance.ChaosLevel/2 + 1, 1);
                        }
                        else if (random < 120)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel/2 + 1, 1);
                        }
                        else if (random < 140)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel/2 + 1, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel/2 + 1, 1);
                        }
                        player.gameObject.name = "Simple Combat Bot Player Modded";
                    }
                    else if (player != null && player.gameObject.name == "Simple Combat Bot Player" && player.GetPlayerName() == "Metal Bandit")
                    {
                        player.Talents[0] += (int)(PLServer.Instance.ChaosLevel * 0.8);
                        player.Talents[2] += (int)(PLServer.Instance.ChaosLevel * 0.9);
                        player.Talents[56] += (int)(PLServer.Instance.ChaosLevel * 0.8);
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 500);
                        if (random < 30)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel/2, 1);
                        }
                        else if (random < 60)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel/2, 1);
                        }
                        else if (random < 100)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel/2, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel/2, 1);
                        }
                        player.gameObject.name = "Simple Combat Bot Player Modded";
                    }
                    else if (player != null && player.gameObject.name == "Simple Combat Bot Player" && player.GetPlayerName() == "Elite Metal Bandit")
                    {
                        player.Talents[0] += (int)(PLServer.Instance.ChaosLevel +2);
                        player.Talents[2] += (int)(PLServer.Instance.ChaosLevel);
                        player.Talents[56] += (int)(PLServer.Instance.ChaosLevel * 0.9);
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 140);
                        if (random < 90)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel/2, 1);
                        }
                        else if (random < 120)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel/2, 1);
                        }
                        else if (random < 140)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel/2, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel/2, 1);
                        }
                        player.gameObject.name = "Simple Combat Bot Player Modded";
                    }
                    else if (player != null && player.gameObject.name == "Simple Combat Bot Player" && player.GetPlayerName() == "Robot Guard")
                    {
                        player.Talents[0] += (int)(PLServer.Instance.ChaosLevel * 1.5);
                        player.Talents[2] += (int)(PLServer.Instance.ChaosLevel);
                        player.Talents[56] += (int)(PLServer.Instance.ChaosLevel * 1);
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 140);
                        if (random < 90)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel/2 + 1, 1);
                        }
                        else if (random < 120)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel/2 + 1, 1);
                        }
                        else if (random < 140)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel/2 + 1, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel/2 + 2, 1);
                        }
                        player.gameObject.name = "Simple Combat Bot Player Modded";
                    }
                    else if (player != null && player.gameObject.name == "Simple Combat Bot Player" && (player.GetPlayerName() == "Bandit" || player.GetPlayerName() == "Guard"))
                    {
                        player.Talents[0] += (int)(PLServer.Instance.ChaosLevel + 1);
                        player.Talents[2] += (int)(PLServer.Instance.ChaosLevel*0.5);
                        player.Talents[56] += (int)(PLServer.Instance.ChaosLevel * 0.5);
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 140);
                        if (random < 90)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel/2, 1);
                        }
                        else if (random < 120)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel/2, 1);
                        }
                        else if (random < 140)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel/2, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel/2, 1);
                        }
                        player.gameObject.name = "Simple Combat Bot Player Modded";
                    }
                }
            }
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
                new CodeInstruction(OpCodes.Ldc_I4_S, 0),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(SpawnerModder),"Health")),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ObscuredInt),"op_Implicit",new Type[]{typeof(int)})),
                new CodeInstruction(OpCodes.Stelem, typeof(ObscuredInt)),
                new CodeInstruction(OpCodes.Ldloc_S, 29),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLPlayer),"Talents")),
                new CodeInstruction(OpCodes.Ldc_I4_S, 2),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(SpawnerModder),"Pistoleer")),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ObscuredInt),"op_Implicit",new Type[]{typeof(int)})),
                new CodeInstruction(OpCodes.Stelem, typeof(ObscuredInt)),
                new CodeInstruction(OpCodes.Ldloc_S, 29),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLPlayer),"Talents")),
                new CodeInstruction(OpCodes.Ldc_I4_S, 25),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(SpawnerModder),"Armored_Skin")),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ObscuredInt),"op_Implicit",new Type[]{typeof(int)})),
                new CodeInstruction(OpCodes.Stelem, typeof(ObscuredInt)),
                new CodeInstruction(OpCodes.Ldloc_S, 29),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLPlayer),"Talents")),
                new CodeInstruction(OpCodes.Ldc_I4_S, 49),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(SpawnerModder),"Reloader")),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ObscuredInt),"op_Implicit",new Type[]{typeof(int)})),
                new CodeInstruction(OpCodes.Stelem, typeof(ObscuredInt)),
                new CodeInstruction(OpCodes.Ldloc_S, 29),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLPlayer),"Talents")),
                new CodeInstruction(OpCodes.Ldc_I4_S, 56),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(SpawnerModder),"Armor")),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ObscuredInt),"op_Implicit",new Type[]{typeof(int)})),
                new CodeInstruction(OpCodes.Stelem, typeof(ObscuredInt)),
            };
                return HarmonyHelpers.PatchBySequence(instructions, targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
            }

        }
        [HarmonyPatch(typeof(PLPersistantEncounterInstance), "PlayerEnter")]
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
        [HarmonyPatch(typeof(PLAlienTentacleCreatureInfo),"FireBeam")]
        class VuroogAttack 
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions)
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[37].operand = 1500f;
                return instructionsList.AsEnumerable();
            }
        }
        [HarmonyPatch(typeof(PLBurrowArena), "Update")]
        class ArenaAntiJetPack //This is so you can't use jetpack on the burrow arena
        {
            static void Postfix(PLBurrowArena __instance) 
            {
                if (Options.MasterHasMod && PLServer.GetCurrentSector() != null && PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.DESERT_HUB) 
                {
                    if (__instance.gameObject.GetComponent<PLKillJetpackVolume>() == null)
                    {
                        __instance.gameObject.AddComponent<PLKillJetpackVolume>();
                    }
                    PLKillJetpackVolume jetpackVolume = __instance.gameObject.GetComponent<PLKillJetpackVolume>();
                    if (__instance.PlayerSpawnLoc != null && jetpackVolume != null) 
                    {
                        jetpackVolume.transform.position = __instance.PlayerSpawnLoc.position;
                        jetpackVolume.transform.position = jetpackVolume.transform.position + new Vector3(5,0,15);
                        jetpackVolume.Dimensions = new Vector3(126, 60, 74);
                        jetpackVolume.enabled = __instance.ArenaIsActive && __instance.WaveID != 5 && __instance.WaveID != 6;
                    }
                }
            }
        }
    }
}
