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
    [HarmonyPatch(typeof(PLShipInfoBase), "set_AlertLevel")]
    class EnemyCrewSpawn //This will make all ship crew stronger
    { 
        static void Postfix() 
        {
            foreach(PLPlayer component in PLServer.Instance.AllPlayers) 
            {
                if (component != null && component.gameObject.name == "Simple Combat Bot Player" && component.StartingShip != null) 
                {
                    if (component.StartingShip.IsRelicHunter || component.StartingShip.IsBountyHunter) continue;
                    int chaos = (int)PLServer.Instance.ChaosLevel;
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
                        component.MyInventory.UpdateItem(ItemID, 26, 0, chaos, 1);
                    }
                    else if(random < 20)
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
                    if(component.GetClassID() == 2) 
                    {
                        ItemID2 = PLServer.Instance.PawnInvItemIDCounter;
                        PLServer.Instance.PawnInvItemIDCounter = ItemID2 + 1;
                        component.MyInventory.UpdateItem(ItemID2, 26, 0, chaos, 4);
                    }
                    component.gameObject.name = "Simple Combat Bot Player Modded";
                    for(int i = 0; i < component.Talents.Length; i++) 
                    {
                        if(component.Talents[i] > 0) 
                        {
                            component.photonView.RPC("ClientGetUpdatedTalent", PhotonTargets.Others, new object[]
                            {
                                i,
                                component.Talents[i],
                                0
                            });
                        }
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
                    __instance.MaxHealth += 30 * (PLServer.Instance.ChaosLevel + 1);
                    __instance.Health = __instance.MaxHealth;
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel*4;
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
                    __instance.MaxHealth += 30 * (PLServer.Instance.ChaosLevel + 1);
                    __instance.Health = __instance.MaxHealth;
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
                    __instance.MaxHealth += 35 * (PLServer.Instance.ChaosLevel + 1);
                    __instance.Health = __instance.MaxHealth;
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 6;
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
                    __instance.MaxHealth += 40 * (PLServer.Instance.ChaosLevel + 1);
                    __instance.Health = __instance.MaxHealth;
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 10;
                    __instance.m_RangedDamage += PLServer.Instance.ChaosLevel * 5;
                }
            }
        }
        [HarmonyPatch(typeof(PLBanditLandDrone), "Start")]
        class BanditLandDrone
        {
            static void Postfix()
            {
                if (Options.MasterHasMod)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLBrainCreature), "Start")]
        class Brain
        {
            static void Postfix()
            {
                if (Options.MasterHasMod)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLCUInvestigatorDrone), "Update")]
        class InestigationDrone
        {
            static void Postfix(PLCUInvestigatorDrone __instance)
            {
                /*
                if (PhotonNetwork.isMasterClient && PLInGameUI.Instance.BossUI_Target != __instance)
                {
                    PLServer.Instance.photonView.RPC("SetupNewHunter", PhotonTargets.All, new object[]
                        {
                                __instance.CombatTargetID,
                        });
                }
                */
            }
        }
        [HarmonyPatch(typeof(PLInfectedSpider), "Start")]
        class InfectedCrawlers
        {
            static void Postfix()
            {
                if (Options.MasterHasMod)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedSpider_Medium), "Start")]
        class MediumInfectedSpider
        {
            static void Postfix(PLInfectedSpider_Medium __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth += 50 * PLServer.Instance.ChaosLevel;
                    __instance.Health = __instance.MaxHealth;
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 15;
                    __instance.Armor += PLServer.Instance.ChaosLevel * 5;
                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedSwarm), "Start")]
        class Dontknowwhatthisis
        {
            static void Postfix()
            {
                if (Options.MasterHasMod)
                {

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
            static void Postfix()
            {
                if (Options.MasterHasMod)
                {

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
                    __instance.MaxHealth += 50 * (PLServer.Instance.ChaosLevel + 1);
                    __instance.Health = __instance.MaxHealth;
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLRobotWalker), "Start")]
        class Paladin
        {
            static void Postfix()
            {
                if (Options.MasterHasMod)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLRobotWalkerLarge), "Start")]
        class ElitPaladin
        {
            static void Postfix()
            {
                if (Options.MasterHasMod)
                {

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
                    __instance.MaxHealth += 30 * (PLServer.Instance.ChaosLevel + 1);
                    __instance.Health = __instance.MaxHealth;
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
                    __instance.MaxHealth += 30 * (PLServer.Instance.ChaosLevel + 1);
                    __instance.Armor += PLServer.Instance.ChaosLevel * 3;
                    __instance.Health = __instance.MaxHealth;
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
                    __instance.MaxHealth += 2 * (PLServer.Instance.ChaosLevel + 1);
                    __instance.moveSpeed *= PLServer.Instance.ChaosLevel / 2;
                    __instance.Health = __instance.MaxHealth;
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 2;
                    __instance.ShouldSpawnOnDamage = true;
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
                    __instance.MaxHealth += 30 * (PLServer.Instance.ChaosLevel + 1);
                    __instance.Health = __instance.MaxHealth;
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 7;
                }
            }
        }
        [HarmonyPatch(typeof(PLSlimeBoss), "Start")]
        class WastedWingSlime
        {
            static void Postfix()
            {
                if (Options.MasterHasMod)
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
                if (Options.MasterHasMod)
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
                    __instance.MeleeDamage *= PLServer.Instance.ChaosLevel / 3;
                    __instance.MaxHealth += 75 * PLServer.Instance.ChaosLevel;
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
        class TheSourceTimer
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //Wasted Wing final boss starts with 6 minutes
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[32].operand = 360f;
                return instructionsList.AsEnumerable();
            }
        }
        [HarmonyPatch(typeof(PLInfectedBoss_WDFlagship), "Start")]
        class MindSlaver
        {
            static void Postfix(PLInfectedBoss_WDFlagship __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.Armor += PLServer.Instance.ChaosLevel * 10;
                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedBoss_WDFlagship), "Update")]
        class MindSlaverUpdate //Allows the MindSlaver to heal if it is attacking no one
        {
            static void Postfix(PLInfectedBoss_WDFlagship __instance)
            {
                if (Options.MasterHasMod && __instance.GetTargetPawn() == null && __instance.Health < __instance.MaxHealth && !__instance.IsDead && __instance.LastDamageTakenTime - Time.time > 5)
                {
                    __instance.Health += (__instance.MaxHealth/30)*Time.deltaTime;
                    if (__instance.Health > __instance.MaxHealth) __instance.Health = __instance.MaxHealth;
                }
            }
        }

        [HarmonyPatch(typeof(PLInfectedHeart_WDFlagship), "Start")]
        class ForsakenFlagshipHeart
        {
            static void Postfix()
            {
                if (Options.MasterHasMod)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLInfectedCrewmember), "Start")]
        class InfectedScientits
        {
            static void Postfix(PLInfectedCrewmember __instance)
            {
                if (Options.MasterHasMod)
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
                if (Options.MasterHasMod)
                {
                    Vector3 spawnpoint = __instance.transform.position;
                    foreach(PLTeleportationTargetInstance teleporter in UnityEngine.Object.FindObjectsOfType<PLTeleportationTargetInstance>()) 
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
                    __instance.Armor += (int)(PLServer.Instance.ChaosLevel * 5);
                    __instance.MaxHealth += (__instance.MaxHealth/15)*PLServer.Instance.ChaosLevel;
                    __instance.Health = __instance.MaxHealth;
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
                if (Options.MasterHasMod)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLGiantRobotHead), "Start")]
        class DownedProtector
        {
            static void Postfix()
            {
                if (Options.MasterHasMod)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLGroundTurret), "Start")]
        class GroundTurret
        {
            static void Postfix()
            {
                if (Options.MasterHasMod)
                {

                }
            }
        }

        [HarmonyPatch(typeof(PLRoamingSecurityGuardRobot), "Start")]
        class MadmansMansionDrone
        {
            static void Postfix(PLRoamingSecurityGuardRobot __instance)
            {
                if (Options.MasterHasMod)
                {
                    __instance.MaxHealth *= PLServer.Instance.ChaosLevel;
                    __instance.Health = __instance.MaxHealth;
                    __instance.Armor = PLServer.Instance.ChaosLevel * 5;
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
                SpawnerModder.Health = 3 + (int)(PLServer.Instance.ChaosLevel * 1.5);
                SpawnerModder.Pistoleer = 2 + (int)(PLServer.Instance.ChaosLevel * 1.2);
                SpawnerModder.Armor = 5 + (int)(PLServer.Instance.ChaosLevel * 1.5);
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
        {
            public static int Health = 0;
            public static int Pistoleer = 0;
            public static int Armored_Skin = 3;
            public static int Reloader = 12;
            public static int Armor = 0;
            static void Postfix() 
            {
                foreach(PLPlayer player in PLServer.Instance.AllPlayers) 
                {
                    if(player != null && player.gameObject.name == "Simple Combat Bot Player" && PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.AOG_MISSIONCHAIN_MADMANS_MANSION) 
                    {
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 500 - Mathf.RoundToInt(PLServer.Instance.ChaosLevel * 60f * UnityEngine.Random.value));
                        if (random < 10)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 9, 0, (int)PLServer.Instance.ChaosLevel+1, 1);
                        }
                        else if (random < 20)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 25, 0, (int)PLServer.Instance.ChaosLevel + 1, 1);
                        }
                        else if (random < 30)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 12, 0, (int)PLServer.Instance.ChaosLevel + 1, 1);
                        }
                        else if (random < 40)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 8, 0, (int)PLServer.Instance.ChaosLevel + 1, 1);
                        }
                        else if (random < 50)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel + 1, 1);
                        }
                        else if (random < 60)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel + 1, 1);
                        }
                        else if (random < 70)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel + 1, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel + 2, 1);
                        }
                        player.gameObject.name = "Simple Combat Bot Player Modded";
                    }
                    else if(player != null && player.gameObject.name == "Simple Combat Bot Player" && player.GetPlayerName() == "Heavy Metal Bandit") 
                    {
                        player.Talents[0] += (int)(PLServer.Instance.ChaosLevel * 5);
                        player.Talents[2] += (int)(PLServer.Instance.ChaosLevel * 1.2);
                        player.Talents[56] += (int)(PLServer.Instance.ChaosLevel * 1.8);
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 140);
                        if (random < 90)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 9, 0, (int)PLServer.Instance.ChaosLevel + 2, 1);
                        }
                        else if (random < 120)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel + 1, 1);
                        }
                        else if (random < 140)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel + 1, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel + 4, 1);
                        }
                        player.gameObject.name = "Simple Combat Bot Player Modded";
                    }
                    else if (player != null && player.gameObject.name == "Simple Combat Bot Player" && player.GetPlayerName() == "Metal Bandit")
                    {
                        player.Talents[0] += (int)(PLServer.Instance.ChaosLevel*3.5);
                        player.Talents[2] += (int)(PLServer.Instance.ChaosLevel*1.2);
                        player.Talents[56] += (int)(PLServer.Instance.ChaosLevel*1.5);
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 500);
                        if (random < 30)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 60)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 100)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel + 1, 1);
                        }
                        player.gameObject.name = "Simple Combat Bot Player Modded";
                    }
                    else if (player != null && player.gameObject.name == "Simple Combat Bot Player" && player.GetPlayerName() == "Elite Metal Bandit")
                    {
                        player.Talents[0] += (int)(PLServer.Instance.ChaosLevel * 5);
                        player.Talents[2] += (int)(PLServer.Instance.ChaosLevel * 1.2);
                        player.Talents[56] += (int)(PLServer.Instance.ChaosLevel * 3.5);
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 140);
                        if (random < 90)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 120)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 140)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel + 1, 1);
                        }
                        player.gameObject.name = "Simple Combat Bot Player Modded";
                    }
                    else if (player != null && player.gameObject.name == "Simple Combat Bot Player" && player.GetPlayerName() == "Robot Guard")
                    {
                        player.Talents[0] += (int)(PLServer.Instance.ChaosLevel * 4.2);
                        player.Talents[2] += (int)(PLServer.Instance.ChaosLevel);
                        player.Talents[56] += (int)(PLServer.Instance.ChaosLevel * 3.2);
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 140);
                        if (random < 90)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 120)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 140)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel + 1, 1);
                        }
                        player.gameObject.name = "Simple Combat Bot Player Modded";
                    }
                    else if (player != null && player.gameObject.name == "Simple Combat Bot Player" && (player.GetPlayerName() == "Bandit"||player.GetPlayerName() == "Guard"))
                    {
                        player.Talents[0] += (int)(PLServer.Instance.ChaosLevel*4);
                        player.Talents[2] += (int)(PLServer.Instance.ChaosLevel);
                        player.Talents[56] += (int)(PLServer.Instance.ChaosLevel * 2.6);
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 140);
                        if (random < 90)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 120)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else if (random < 140)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel + 1, 1);
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
        [HarmonyPatch(typeof(PLStopAsteroidEncounter),"Update")]

        class MeteorMission 
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions)
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[282].operand = 300f;
                instructionsList[576].opcode = OpCodes.Ldc_I4_S;
                instructionsList[576].operand = 0;
                return instructionsList.AsEnumerable();
            }
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
