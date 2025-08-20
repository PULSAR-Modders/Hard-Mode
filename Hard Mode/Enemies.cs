using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using HarmonyLib;
using System.Reflection.Emit;
using PulsarModLoader.Patches;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using System.Reflection;
using PulsarModLoader.Utilities;

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
    [HarmonyPatch(typeof(PLShipInfo), "CreateDefaultItemsForEnemyBotPlayer")]
    class EnemyCrewSpawn //This will make all ship crew stronger
    {
        static void Postfix(PLPlayer inPlayer)
        {
            if (inPlayer != null && inPlayer.StartingShip != null)
            {
                if (inPlayer.StartingShip.IsRelicHunter || inPlayer.StartingShip.IsBountyHunter) return;
                int chaos = Mathf.RoundToInt(Mathf.FloorToInt(PLServer.Instance.ChaosLevel) + UnityEngine.Random.Range(0, 2) + Mathf.CeilToInt(inPlayer.StartingShip.GetCombatLevel() / 20) * UnityEngine.Random.Range(0.70f, 1f));
                inPlayer.Talents[56] = chaos;//Armor
                inPlayer.Talents[58] = chaos;//Armor2
                inPlayer.Talents[0] = chaos;//Health
                inPlayer.Talents[57] = chaos;//Heatlh2
                inPlayer.Talents[3] = Mathf.Clamp(chaos, 0, 5);//Respawn timer
                inPlayer.Talents[2] = 3; //Pistol damage
                switch (inPlayer.GetClassID())
                {
                    case 0:
                        inPlayer.Talents[47] = Mathf.Clamp(chaos,0,8); //Friendly screen capture
                        inPlayer.Talents[27] = chaos; //Captain Armor
                        inPlayer.Talents[5] = chaos; //Captain speed boost
                        inPlayer.Talents[50] = chaos; //Screen safety
                        break;
                    case 1:
                        inPlayer.Talents[36] = Mathf.Clamp(chaos,0,5); //Reduce hull damage
                        inPlayer.Talents[35] = Mathf.Clamp(chaos,0,7); //Reduce system damage
                        inPlayer.Talents[8] = Mathf.Clamp(chaos, 0, 5); //Ship speed
                        inPlayer.Talents[9] = Mathf.Clamp(chaos, 0, 5); //Ship maneuver
                        break;
                    case 2:
                        inPlayer.Talents[13] = chaos; //Bridge medic
                        inPlayer.Talents[11] = chaos; //Sensor boost
                        inPlayer.Talents[12] = Mathf.Clamp(chaos/2, 0, 5); //Sensor Hide
                        break;
                    case 3:
                        inPlayer.Talents[38] = chaos; //Turret charge boost crew
                        inPlayer.Talents[39] = chaos; //Turret damage boost crew
                        inPlayer.Talents[23] = chaos; //Weapon cooling
                        inPlayer.Talents[17] = chaos; //Missile expert
                        inPlayer.Talents[25] = Mathf.Clamp(chaos/2,0,5); //Reduce pawn damage
                        inPlayer.Talents[62] = chaos; //Turret cooling boost crew
                        inPlayer.Talents[48] = Mathf.Clamp(chaos,0,5); //Enemy screen capture
                        break;
                    case 4:
                        inPlayer.Talents[20] = chaos; //Coolant
                        inPlayer.Talents[19] = chaos; //Fire reduction
                        inPlayer.Talents[21] = chaos; //auto-repair
                        inPlayer.Talents[45] = chaos/2; //Reactor power boost
                        inPlayer.Talents[61] = chaos; //Turret cooling boost crew
                        break;
                }
                inPlayer.MyInventory.Clear();
                int random = UnityEngine.Random.Range(0, 500 - Mathf.RoundToInt(PLServer.Instance.ChaosLevel * 60f * UnityEngine.Random.Range(0.70f, 1f)));
                if (random < 10)
                {
                    int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                    inPlayer.MyInventory.UpdateItem(ItemID, 26, 0, chaos, 1);
                }
                else if (random < 20)
                {
                    int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                    inPlayer.MyInventory.UpdateItem(ItemID, 9, 0, chaos, 1);
                }
                else if (random < 30)
                {
                    int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                    inPlayer.MyInventory.UpdateItem(ItemID, 25, 0, chaos, 1);
                }
                else if (random < 40)
                {
                    int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                    inPlayer.MyInventory.UpdateItem(ItemID, 12, 0, chaos, 1);
                }
                else if (random < 50)
                {
                    int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                    inPlayer.MyInventory.UpdateItem(ItemID, 8, 0, chaos, 1);
                }
                else if (random < 100)
                {
                    int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                    inPlayer.MyInventory.UpdateItem(ItemID, 7, 0, chaos, 1);
                }
                else if (random < 150)
                {
                    int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                    inPlayer.MyInventory.UpdateItem(ItemID, 10, 0, chaos, 1);
                }
                else if (random < 200)
                {
                    int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                    inPlayer.MyInventory.UpdateItem(ItemID, 11, 0, chaos, 1);
                }
                else
                {
                    int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                    inPlayer.MyInventory.UpdateItem(ItemID, 2, 0, chaos + 1, 1);
                }
                int ItemID2 = PLServer.Instance.PawnInvItemIDCounter;
                PLServer.Instance.PawnInvItemIDCounter = ItemID2 + 1;
                inPlayer.MyInventory.UpdateItem(ItemID2, 3, 0, chaos, 2);
                ItemID2 = PLServer.Instance.PawnInvItemIDCounter;
                PLServer.Instance.PawnInvItemIDCounter = ItemID2 + 1;
                inPlayer.MyInventory.UpdateItem(ItemID2, 4, 0, chaos, 3);
                if (inPlayer.GetClassID() == 2)
                {
                    ItemID2 = PLServer.Instance.PawnInvItemIDCounter;
                    PLServer.Instance.PawnInvItemIDCounter = ItemID2 + 1;
                    inPlayer.MyInventory.UpdateItem(ItemID2, 26, 0, chaos, 4);
                }
                inPlayer.gameObject.name += " HardMode";
            }
        }
    }

    [HarmonyPatch(typeof(PLIntrepidCommanderInfo), "CreateDefaultItemsForEnemyBotPlayer")]
    internal class BossCrewSpawn
    {
        internal static void Postfix(PLPlayer inPlayer)
        {
            int chaos = Mathf.FloorToInt(PLServer.Instance.ChaosLevel) + UnityEngine.Random.Range(0, 2) + Mathf.CeilToInt(inPlayer.StartingShip.GetCombatLevel() / 20);
            inPlayer.Talents[56] = 5 + chaos; //Armor
            inPlayer.Talents[58] = 5 + chaos; //Armor2
            inPlayer.Talents[0] = 5 + chaos; //Health
            inPlayer.Talents[57] = 5 + chaos; //Heatlh2
            inPlayer.Talents[2] = 5; //Pistol damage
            inPlayer.Talents[3] = 5; //Respawn timer
            switch (inPlayer.GetClassID())
            {
                case 0:
                    inPlayer.Talents[27] = 8 + chaos; //Captain Armor
                    inPlayer.Talents[5] = 5 + chaos; //Captain speed boost
                    inPlayer.Talents[47] = 8; //Friendly screen capture
                    inPlayer.Talents[50] = 8 + chaos; //Screen safety
                    break;
                case 1:
                    inPlayer.Talents[36] = Mathf.Clamp(5 + chaos, 0, 10);//Reduce hull damage
                    inPlayer.Talents[35] = Mathf.Clamp(7 + chaos, 0, 15);//Reduce system damage
                    inPlayer.Talents[8] = 5; //Ship speed
                    inPlayer.Talents[9] = 5; //Ship maneuver
                    break;
                case 2:
                    inPlayer.Talents[13] = 5 + chaos; //Bridge medic
                    inPlayer.Talents[11] = 5 + chaos; //Sensor boost
                    inPlayer.Talents[12] = 5; //Sensor Hide
                    break;
                case 3:
                    inPlayer.Talents[38] = 6 + chaos; //Turret charge boost crew
                    inPlayer.Talents[39] = 6 + chaos; //Turret damage boost crew
                    inPlayer.Talents[23] = 5 + chaos; //Weapon cooling
                    inPlayer.Talents[17] = 5 + chaos; //Missile expert
                    inPlayer.Talents[25] = 5; //Reduce pawn damage
                    inPlayer.Talents[62] = 5 + chaos; //Turret cooling boost crew
                    inPlayer.Talents[48] = 5; //Enemy screen capture
                    break;
                case 4:
                    inPlayer.Talents[20] = 5 + chaos; //Coolant
                    inPlayer.Talents[19] = 25 + chaos; //Fire reduction
                    inPlayer.Talents[21] = 25 + chaos; //auto-repair
                    inPlayer.Talents[45] = 8 + chaos/2; //Reactor power boost
                    inPlayer.Talents[61] = 5 + chaos; //Turret cooling boost crew
                    break;
            }
            chaos += 2;
            inPlayer.MyInventory.Clear();
            int random = UnityEngine.Random.Range(0, 500 - Mathf.RoundToInt(PLServer.Instance.ChaosLevel * 60f * UnityEngine.Random.Range(0.70f, 1f)));
            if (random < 10)
            {
                int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                inPlayer.MyInventory.UpdateItem(ItemID, 26, 0, chaos, 1);
            }
            else if (random < 20)
            {
                int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                inPlayer.MyInventory.UpdateItem(ItemID, 9, 0, chaos, 1);
            }
            else if (random < 30)
            {
                int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                inPlayer.MyInventory.UpdateItem(ItemID, 25, 0, chaos, 1);
            }
            else if (random < 40)
            {
                int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                inPlayer.MyInventory.UpdateItem(ItemID, 12, 0, chaos, 1);
            }
            else if (random < 50)
            {
                int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                inPlayer.MyInventory.UpdateItem(ItemID, 8, 0, chaos, 1);
            }
            else if (random < 100)
            {
                int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                inPlayer.MyInventory.UpdateItem(ItemID, 7, 0, chaos, 1);
            }
            else if (random < 150)
            {
                int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                inPlayer.MyInventory.UpdateItem(ItemID, 10, 0, chaos, 1);
            }
            else if (random < 200)
            {
                int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                inPlayer.MyInventory.UpdateItem(ItemID, 11, 0, chaos, 1);
            }
            else
            {
                int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                inPlayer.MyInventory.UpdateItem(ItemID, 2, 0, chaos + 1, 1);
            }
            int ItemID2 = PLServer.Instance.PawnInvItemIDCounter;
            PLServer.Instance.PawnInvItemIDCounter = ItemID2 + 1;
            inPlayer.MyInventory.UpdateItem(ItemID2, 3, 0, chaos, 2);
            ItemID2 = PLServer.Instance.PawnInvItemIDCounter;
            PLServer.Instance.PawnInvItemIDCounter = ItemID2 + 1;
            inPlayer.MyInventory.UpdateItem(ItemID2, 4, 0, chaos, 3);
            if (inPlayer.GetClassID() == 2)
            {
                ItemID2 = PLServer.Instance.PawnInvItemIDCounter;
                PLServer.Instance.PawnInvItemIDCounter = ItemID2 + 1;
                inPlayer.MyInventory.UpdateItem(ItemID2, 26, 0, chaos, 4);
            }
            inPlayer.gameObject.name += " HardMode";
        }
    } //This will make the boss ship crew stronger

    [HarmonyPatch(typeof(PLAlchemistShipInfo), "CreateDefaultItemsForEnemyBotPlayer")]
    class AlchemistCrewSpawn
    {
        static void Postfix(PLPlayer inPlayer)
        {
            BossCrewSpawn.Postfix(inPlayer);
        }
    } //This will make the caustic corsair crew stronger

    [HarmonyPatch(typeof(PLShipInfoBase), "GetCombatLevel")]
    class CombatLevel //Modify the combat level calculation
    {
        private static MethodInfo GetDPS = AccessTools.Method(typeof(PLTurret), "GetDPS");
        private static FieldInfo HeatGeneratedOnFire = AccessTools.Field(typeof(PLTurret), "HeatGeneratedOnFire");
        private static FieldInfo FireDelay = AccessTools.Field(typeof(PLTurret), "FireDelay");
        private static FieldInfo TurretRange = AccessTools.Field(typeof(PLTurret), "TurretRange");
        private static FieldInfo damage_Normal = AccessTools.Field(typeof(PLAbyssTurret), "damage_Normal");
        public static float GetTurretCombatLevel(PLTurret turret, ref float CombatLevel)
        {
            if (turret is PLAbyssTurret)
            {
                PLAbyssTurret abyssturret = turret as PLAbyssTurret;
                CombatLevel += (float)damage_Normal.GetValue(abyssturret) * abyssturret.LevelMultiplier(0.15f, 1f) * (abyssturret.ShipStats != null ? abyssturret.ShipStats.TurretDamageFactor : 1) / ((float)FireDelay.GetValue(abyssturret) / ((abyssturret.ShipStats != null) ? abyssturret.ShipStats.TurretChargeFactor : 1f)) * 0.1f * (1f - Mathf.Clamp01((float)HeatGeneratedOnFire.GetValue(abyssturret) * 2.2f / (float)FireDelay.GetValue(abyssturret)));
                CombatLevel += (float)TurretRange.GetValue(turret) * 0.0005f;
                return CombatLevel;
            }
            CombatLevel += (float)GetDPS.Invoke(turret, null) * 0.1f * (1f - Mathf.Clamp01((float)HeatGeneratedOnFire.GetValue(turret) * 2.2f / (float)FireDelay.GetValue(turret)));
            CombatLevel += (float)TurretRange.GetValue(turret) * 0.0005f;
            return CombatLevel;
        }
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
                        PLTurret turret = plshipComponent as PLTurret;
                        GetTurretCombatLevel(turret, ref __result);
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
        private static FieldInfo m_RangedDamage = AccessTools.Field(typeof(PLAntRavager), "m_RangedDamage");
        private static FieldInfo moveSpeed = AccessTools.Field(typeof(PLSlime), "moveSpeed");
        [HarmonyPatch(typeof(PLCombatTarget), "Start")]
        class CombatTarget
        {
            static void Postfix(PLCombatTarget __instance)
            {
                // PLCreature       // PLAirElemental (Tornado), PLAnt, PLAntArmored, PLAntHeavy, PLAntRavager, PLInfectedSpider (Crawler), PLInfectedSpider_Medium, PLLCLabEnemy (Lost colony ghosts), PLRaptor
                // PLCreature       // PLRat, PLSlime, PLSlimeBoss, PLSpider
                // PLPawnBase       // PLAssassinBot, PLCrystalBoss (Source), PLInfectedBoss_WDFlagship (MindSlaver), PLInfectedCrewmember (Infected), PLInfectedHeart_WDFlagship, PLInfectedScientist (Source), PLStalkerPawn (Teleporting figure)
                // PLCombatTarget   // PLBoardingBot, PLGroundTurret, PLGiantRobotHead, PLRoamingSecurityGuardBot

                if (Options.MasterHasMod)
                {
                    if (__instance is PLBoardingBot || __instance is PLGroundTurret || __instance is PLGiantRobotHead || __instance is PLRoamingSecurityGuardRobot)
                    {
                        __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                        __instance.Health = __instance.MaxHealth;
                        if (__instance.Armor == 0) __instance.Armor = 5f;
                        __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    }
                    else if (__instance is PLCreature) // Must be checked before PLPawnBase as PLCreature derives from it
                    {
                        PLCreature instance = __instance as PLCreature;
                        if (instance is PLSlimeBoss || instance is PLInfectedSpider_WG) return;
                        instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                        instance.Health = instance.MaxHealth;
                        if (instance.Armor == 0) instance.Armor = 5f;
                        instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                        instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                        if (instance is PLAntRavager) m_RangedDamage.SetValue(instance as PLAntRavager, (float)m_RangedDamage.GetValue(instance as PLAntRavager) + PLServer.Instance.ChaosLevel * 5);
                        if (instance is PLSlime) moveSpeed.SetValue(instance as PLSlime, (2 * (float)moveSpeed.GetValue(instance as PLSlime)) * PLServer.Instance.ChaosLevel / 2);
                    }
                    else if (__instance is PLPawnBase)
                    {
                        PLPawnBase instance = __instance as PLPawnBase;
                        if (instance is PLCrystalBoss || instance is PLInfectedBoss_WDFlagship || instance is PLPawn) return;
                        __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                        __instance.Health = __instance.MaxHealth;
                        if (__instance.Armor == 0) __instance.Armor = 5f;
                        __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLNullpoint), "Start")] //This are the green things that spread green fire on the unseen mothership 
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
            private static MethodInfo PlayerShip_IsPilotedByAI = AccessTools.Method(typeof(PLUnseenEye), "PlayerShip_IsPilotedByAI");
            private static FieldInfo energyProjCounter = AccessTools.Field(typeof(PLUnseenEye), "energyProjCounter");
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
                        bool _PlayerShip_IsPilotedByAI = (bool)PlayerShip_IsPilotedByAI.Invoke(__instance, null);
                        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(__instance.EnergyProjPrefab, __instance.BeamFireLoc.position, __instance.BeamFireLoc.rotation);
                        gameObject.SetActive(true);
                        PLCurvedProjectile component = gameObject.GetComponent<PLCurvedProjectile>();
                        float d = 520f;
                        Vector3 b = UnityEngine.Random.insideUnitSphere * (_PlayerShip_IsPilotedByAI ? 20f : 10f) * 7f;
                        PLDriftTowardPlayerShip component2 = gameObject.GetComponent<PLDriftTowardPlayerShip>();
                        if (_PlayerShip_IsPilotedByAI)
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
                        int _energyProjCounter = (int)energyProjCounter.GetValue(__instance);
                        component.ProjID = -40000 - _energyProjCounter;
                        _energyProjCounter++;
                        energyProjCounter.SetValue(__instance, _energyProjCounter);
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
        [HarmonyPatch(typeof(PLUnseenEye), "Update")]
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
                    if (player != null && !player.gameObject.name.Contains("HardMode") && PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.AOG_MISSIONCHAIN_MADMANS_MANSION)
                    {
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 500 - Mathf.RoundToInt(PLServer.Instance.ChaosLevel * 60f * UnityEngine.Random.value));
                        if (random < 10)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 9, 0, (int)PLServer.Instance.ChaosLevel / 2 + 2, 1);
                        }
                        else if (random < 20)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 25, 0, (int)PLServer.Instance.ChaosLevel / 2 + 2, 1);
                        }
                        else if (random < 30)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 12, 0, (int)PLServer.Instance.ChaosLevel / 2 + 2, 1);
                        }
                        else if (random < 40)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 8, 0, (int)PLServer.Instance.ChaosLevel / 2 + 2, 1);
                        }
                        else if (random < 50)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel / 2 + 2, 1);
                        }
                        else if (random < 60)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel / 2 + 2, 1);
                        }
                        else if (random < 70)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel / 2 + 2, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel / 2 + 2, 1);
                        }
                        player.gameObject.name += " HardMode";
                    }
                    else if (player != null && !player.gameObject.name.Contains("HardMode") && player.GetPlayerName() == "Heavy Metal Bandit")
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
                            player.MyInventory.UpdateItem(ItemID, 9, 0, (int)PLServer.Instance.ChaosLevel / 2 + 1, 1);
                        }
                        else if (random < 120)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel / 2 + 1, 1);
                        }
                        else if (random < 140)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel / 2 + 1, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel / 2 + 1, 1);
                        }
                        player.gameObject.name += " HardMode"; 
                    }
                    else if (player != null && !player.gameObject.name.Contains("HardMode") && player.GetPlayerName() == "Metal Bandit")
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
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel / 2, 1);
                        }
                        else if (random < 60)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel / 2, 1);
                        }
                        else if (random < 100)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel / 2, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel / 2, 1);
                        }
                        player.gameObject.name += " HardMode";
                    }
                    else if (player != null && !player.gameObject.name.Contains("HardMode") && player.GetPlayerName() == "Elite Metal Bandit")
                    {
                        player.Talents[0] += (int)(PLServer.Instance.ChaosLevel + 2);
                        player.Talents[2] += (int)(PLServer.Instance.ChaosLevel);
                        player.Talents[56] += (int)(PLServer.Instance.ChaosLevel * 0.9);
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 140);
                        if (random < 90)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel / 2, 1);
                        }
                        else if (random < 120)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel / 2, 1);
                        }
                        else if (random < 140)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel / 2, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel / 2, 1);
                        }
                        player.gameObject.name += " HardMode";
                    }
                    else if (player != null && !player.gameObject.name.Contains("HardMode") && player.GetPlayerName() == "Robot Guard")
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
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel / 2 + 1, 1);
                        }
                        else if (random < 120)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel / 2 + 1, 1);
                        }
                        else if (random < 140)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel / 2 + 1, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel / 2 + 2, 1);
                        }
                        player.gameObject.name += " HardMode";
                    }
                    else if (player != null && !player.gameObject.name.Contains("HardMode") && (player.GetPlayerName() == "Bandit" || player.GetPlayerName() == "Guard"))
                    {
                        player.Talents[0] += (int)(PLServer.Instance.ChaosLevel + 1);
                        player.Talents[2] += (int)(PLServer.Instance.ChaosLevel * 0.5);
                        player.Talents[56] += (int)(PLServer.Instance.ChaosLevel * 0.5);
                        player.MyInventory.Clear();
                        int random = UnityEngine.Random.Range(0, 140);
                        if (random < 90)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 7, 0, (int)PLServer.Instance.ChaosLevel / 2, 1);
                        }
                        else if (random < 120)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 10, 0, (int)PLServer.Instance.ChaosLevel / 2, 1);
                        }
                        else if (random < 140)
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 11, 0, (int)PLServer.Instance.ChaosLevel / 2, 1);
                        }
                        else
                        {
                            int ItemID = PLServer.Instance.PawnInvItemIDCounter;
                            PLServer.Instance.PawnInvItemIDCounter = ItemID + 1;
                            player.MyInventory.UpdateItem(ItemID, 2, 0, (int)PLServer.Instance.ChaosLevel / 2, 1);
                        }
                        player.gameObject.name += " HardMode";
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
        [HarmonyPatch(typeof(PLAlienTentacleCreatureInfo), "FireBeam")]
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
                        jetpackVolume.transform.position = jetpackVolume.transform.position + new Vector3(5, 0, 15);
                        jetpackVolume.Dimensions = new Vector3(126, 60, 74);
                        jetpackVolume.enabled = __instance.ArenaIsActive && __instance.WaveID != 5 && __instance.WaveID != 6;
                    }
                }
            }
        }
    }
}
