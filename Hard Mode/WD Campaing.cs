using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace Hard_Mode
{
    class WD_Campaing
    {
        [HarmonyPatch(typeof(PLInfectedBoss_WDFlagship), "Start")]
        class MindSlaver
        {
            static void Postfix(PLInfectedBoss_WDFlagship __instance)
            {
                if (Options.MasterHasMod)
                {
                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedBoss_WDFlagship), "Update")]
        class MindSlaverUpdate //Allows the MindSlaver to heal if it is attacking no one
        {
            static void Postfix(PLInfectedBoss_WDFlagship __instance)
            {
                if (Options.MasterHasMod && __instance.Health < __instance.MaxHealth && !__instance.IsDead && Time.time - __instance.LastDamageTakenTime > 5 && PhotonNetwork.isMasterClient)
                {
                    __instance.Health += (__instance.MaxHealth / 30) * Time.deltaTime;
                    if (__instance.Health > __instance.MaxHealth) __instance.Health = __instance.MaxHealth;
                }
            }
        }

        [HarmonyPatch(typeof(PLInfectedHeart_WDFlagship), "Start")]
        class ForsakenFlagshipHeart
        {
            static void Postfix(PLInfectedHeart_WDFlagship __instance)
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
        [HarmonyPatch(typeof(PLInfectedHeart_WDFlagship), "Update")]
        class ForsakenFlagshipHeartUpdate
        {
            private static FieldInfo FightActivated = AccessTools.Field(typeof(PLInfectedHeart_WDFlagship), "FightActivated");
            static void Postfix(PLInfectedHeart_WDFlagship __instance)
            {
                if (Options.MasterHasMod)
                {
                    if ((bool)FightActivated.GetValue(__instance) && !__instance.IsDead)
                    {
                        __instance.Health += 150 * Time.deltaTime;
                        __instance.Health = Mathf.Clamp(__instance.Health, 0f, __instance.MaxHealth);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(PLInfectedHeart_WDFlagship), "TakeDamage")]
        class ForsakenFlagshipHeartDamage
        {
            static float lastSpawn = Time.time;
            static void Postfix(PLInfectedHeart_WDFlagship __instance)
            {
                if (Options.MasterHasMod && Time.time - lastSpawn > 2f && PhotonNetwork.isMasterClient)
                {
                    lastSpawn = Time.time;
                    PLSpawner.DoSpawnStatic(PLEncounterManager.Instance.GetCPEI(), "InfectedLargeCrawlerSpawnElite", __instance.transform, null, __instance.MyCurrentTLI, __instance.MyInterior, __instance);
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
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
        [HarmonyPatch(typeof(PLForsakenFlagshipCountdown), "StartCountdown")]
        class FlagshipCountdown //This is so player have 45 seconds to warp away
        {
            static void Postfix(ref float ___SelfDestructTimeLeft)
            {
                if (Options.MasterHasMod)
                {
                    if (___SelfDestructTimeLeft > 60) ___SelfDestructTimeLeft = 60;
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
                    __instance.MaxHealth *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.Health = __instance.MaxHealth;
                    if (__instance.Armor == 0) __instance.Armor = 5f;
                    __instance.Armor *= 1f + (PLServer.Instance.ChaosLevel / 6);
                    __instance.MeleeDamage += PLServer.Instance.ChaosLevel * 4;
                }
            }
        }
    }
}
