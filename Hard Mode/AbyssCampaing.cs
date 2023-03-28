using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System;
using System.Collections;
using System.Reflection;

namespace Hard_Mode
{
    internal class AbyssCampaing
    {
        [HarmonyPatch(typeof(PLAbyssShipInfo),"Update")]
        class AbyssPlayerShip 
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //Increases the water damage range
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[437].operand = 0.7f;
                return instructionsList.AsEnumerable();
            }
            private static FieldInfo Flood_EndingPos = AccessTools.Field(typeof(PLAbyssShipInfo), "Flood_EndingPos");
            private static FieldInfo Flood_StartingPos = AccessTools.Field(typeof(PLAbyssShipInfo), "Flood_StartingPos");
            static void Postfix(PLAbyssShipInfo __instance) 
            {
                if (Options.MasterHasMod) Flood_EndingPos.SetValue(__instance, (Vector3)Flood_StartingPos.GetValue(__instance) + new Vector3(0, 1.06f));
            }
        }
        [HarmonyPatch(typeof(PLAbyssShipInfo), "AddHullBreach")]
        class AbyssAddHullBreach
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //Increases the water damage range
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[16].opcode = OpCodes.Ldc_I4_S;
                instructionsList[16].operand = 11;
                return instructionsList.AsEnumerable();
            }
        }
        [HarmonyPatch(typeof(PLAbyssFighterInfo), "SetupShipStats")]
        class AbyssFighter
        {
            private static FieldInfo FirePower = AccessTools.Field(typeof(PLAbyssFighterInfo), "FirePower");
            static void Postfix(PLAbyssFighterInfo __instance)
            {
                if (PLEncounterManager.Instance.PlayerShip != null)
                {
                    if (__instance.MyHull != null) __instance.MyHull.Level += Mathf.CeilToInt(PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 15);
                    FirePower.SetValue(__instance, (float)FirePower.GetValue(__instance) + (PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 50));
                }
            }
        }

        [HarmonyPatch(typeof(PLAbyssHeavyFighterInfo), "SetupShipStats")]
        class HeavyAbyssFighter
        {
            private static FieldInfo FirePower = AccessTools.Field(typeof(PLAbyssHeavyFighterInfo), "FirePower");
            static void Postfix(PLAbyssHeavyFighterInfo __instance)
            {
                if (PLEncounterManager.Instance.PlayerShip != null)
                {
                    if (__instance.MyHull != null) __instance.MyHull.Level += Mathf.CeilToInt(PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 15);
                    FirePower.SetValue(__instance, (float)FirePower.GetValue(__instance) + (PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 50));
                }
            }
        }
        [HarmonyPatch(typeof(PLMatrixDroneInfo), "SetupShipStats")]
        class Cultivator
        {
            private static FieldInfo FireRateScalar = AccessTools.Field(typeof(PLMatrixDroneInfo), "FireRateScalar");
            static void Postfix(PLMatrixDroneInfo __instance)
            {
                if (PLEncounterManager.Instance.PlayerShip != null)
                {
                    if(__instance.MyHull != null)__instance.MyHull.Level = Mathf.CeilToInt(PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 15) + 1;
                    FireRateScalar.SetValue(__instance, (float)FireRateScalar.GetValue(__instance) + (PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 50));
                }
            }
        }
        [HarmonyPatch(typeof(PLMatrixDroneInfo), "Update")]
        class CultivatorFireCount
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //Doubles the fire power of the cultivators
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[132].opcode = OpCodes.Ldc_I4_S;
                instructionsList[132].operand = 10;
                return instructionsList.AsEnumerable();
            }
        }
        [HarmonyPatch(typeof(PLMatrixDroneCommanderInfo), "Update")]
        class HarvesterHeal
        {
            static float lastHeal = Time.time;
            private static MethodInfo ShouldShowBossUI = AccessTools.Method(typeof(PLMatrixDroneCommanderInfo), "ShouldShowBossUI");
            private static FieldInfo FireRateScalar = AccessTools.Field(typeof(PLMatrixDroneCommanderInfo), "FireRateScalar");
            static void Postfix(PLMatrixDroneCommanderInfo __instance)
            {
                if ((bool)ShouldShowBossUI.Invoke(__instance, null) && Options.MasterHasMod)
                {
                    if (PLEncounterManager.Instance.PlayerShip != null && Options.MasterHasMod)
                    {
                        FireRateScalar.SetValue(__instance, 2 + PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 50);
                        __instance.EngineeringSystem.Health = __instance.EngineeringSystem.MaxHealth;
                        __instance.WeaponsSystem.Health = __instance.WeaponsSystem.MaxHealth;
                        __instance.ComputerSystem.Health = __instance.ComputerSystem.MaxHealth;
                    }
                    if (PhotonNetwork.isMasterClient &&  Time.time - lastHeal < 45)
                    {
                        __instance.photonView.RPC("Recharge", PhotonTargets.All, new object[0]);
                        lastHeal = Time.time;
                    }
                }
                if (!(bool)ShouldShowBossUI.Invoke(__instance, null) && __instance.MyHull != null && __instance.MyStats != null && Options.MasterHasMod)
                {
                    if (__instance.MyHull != null && __instance.MyStats != null) __instance.MyHull.Level = Mathf.CeilToInt(PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 15) + 1;
                    __instance.MyHull.Current = __instance.MyStats.HullMax;
                }
            }
        }
        [HarmonyPatch(typeof(PLAbyssArmoredWarden), "TakeDamage")]
        class BulwarkDamage 
        {
            static void Postfix(PLAbyssArmoredWarden __instance, float dmg, EDamageType dmgType) 
            {
                if (__instance.MyActivatorVolume != null && __instance.MyActivatorVolume.GetHasBeenTriggered())
                {
                    __instance.WeakPoint.Health += dmg * 0.015f;
                    float num = dmg;
                    float num3 = __instance.MyHull.Current;
                    float num4 = __instance.MyStats.HullArmor;
                    if (dmgType == EDamageType.E_BEAM || dmgType == EDamageType.E_ARMOR_PIERCE_PHYS)
                    {
                        if (__instance.MyHull.SubType == 14)
                        {
                            num4 *= 0.8f;
                        }
                        else
                        {
                            num4 *= 0.5f;
                            num4 -= 0.05f;
                        }
                        num4 = Mathf.Clamp(num4, 0f, float.MaxValue);
                    }
                    else if (dmgType == EDamageType.E_INFECTED)
                    {
                        num4 *= 0.3f;
                        num4 -= 0.2f;
                        num4 = Mathf.Clamp(num4, 0f, float.MaxValue);
                    }
                    if (__instance.MyStats.Ship.GetIsPlayerShip() && __instance.MyStats.HullMax != 0f)
                    {
                        float num5 = 1f - Mathf.Clamp01(__instance.MyStats.HullCurrent / __instance.MyStats.HullMax);
                        num4 += 0.35f + num5 * 0.2f;
                    }
                    float num6 = Mathf.Clamp(num - num4 * 350f, num * (0.7f - Mathf.Clamp(num4 * 0.5f, 0f, 0.6f)), float.MaxValue);
                    __instance.WeakPoint.Health -= num6;
                }
            }
        }
        [HarmonyPatch(typeof(PLAbyssArmoredWarden), "Update")]
        class BulwarkUpdate
        {
            private static MethodInfo ShouldShowBossUI = AccessTools.Method(typeof(PLAbyssArmoredWarden), "ShouldShowBossUI");
            static void Postfix(PLAbyssArmoredWarden __instance)
            {
                if (PLEncounterManager.Instance.PlayerShip != null && Options.MasterHasMod)
                {
                    
                    foreach (PLAbyssWardenTurret turret in __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET).Cast<PLAbyssWardenTurret>())
                    {
                        turret.Level = Mathf.CeilToInt(PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 15) + 1;
                    }
                    __instance.EngineeringSystem.Health = __instance.EngineeringSystem.MaxHealth;
                    __instance.WeaponsSystem.Health = __instance.WeaponsSystem.MaxHealth;
                    __instance.ComputerSystem.Health = __instance.ComputerSystem.MaxHealth;
                    /*
                    bool isup = false;
                    Vector3 normalized = (PLEncounterManager.Instance.PlayerShip.GetSpaceLoc() - __instance.Exterior.transform.position).normalized;
                    Vector3 up = __instance.Exterior.transform.up;
                    isup = Vector3.Dot(normalized, up) > 0;
                    if (isup) 
                    {
                        __instance.WeakPoint.Health = cachedWeakHealth;
                    }
                    else 
                    {
                        cachedWeakHealth = __instance.WeakPoint.Health;
                    }
                    */
                    if (!(bool)ShouldShowBossUI.Invoke(__instance, null))
                    {
                        if (__instance.MyHull != null && __instance.MyStats != null) __instance.MyHull.Level = Mathf.CeilToInt(PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 15) + 1;
                        __instance.MyHull.Current = __instance.MyStats.HullMax;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(PLAbyssArmoredWarden), "OnCalculateStatsFinal")]
        class BulwarkStats
        {
            static void Postfix(ref PLShipStats inStats)
            {
                inStats.HullArmor *= 500;
                inStats.TurretCoolingSpeedFactor *= 2;
                inStats.TurretChargeFactor *= 5;
                inStats.InertiaThrustOutputCurrent *= 3;
                inStats.InertiaThrustOutputMax *= 3;
            }
        }
        [HarmonyPatch(typeof(PLAbyssBoss), "Start")]
        class StartWarden
        {
            static void Postfix(PLAbyssBoss __instance)
            {
                __instance.StopAllCoroutines();
                __instance.StartCoroutine(TimedRoutine(__instance));
            }
            private static MethodInfo ExecuteBeamAttack = AccessTools.Method(typeof(PLAbyssBoss), "ExecuteBeamAttack");
            private static MethodInfo ProjAttackTimed = AccessTools.Method(typeof(PLAbyssBoss), "ProjAttackTimed");
            static IEnumerator TimedRoutine(PLAbyssBoss __instance)
            {
                WaitForSeconds normalWait = new WaitForSeconds(1f);
                WaitForSeconds longWait = new WaitForSeconds(3f);
                while (Application.isPlaying)
                {
                    if (__instance.MyActivatorVolume == null || !__instance.MyActivatorVolume.GetHasBeenTriggered())
                    {
                        yield return normalWait;
                    }
                    else if (PhotonNetwork.isMasterClient && __instance.TargetShip != null)
                    {
                        float num = __instance.MyStats.HullCurrent / __instance.MyStats.HullMax;
                        float value = UnityEngine.Random.value;
                        if (num < 0.5f)
                        {
                            normalWait = new WaitForSeconds(0.5f);
                            longWait = new WaitForSeconds(1.5f);
                        }
                        if (value < 0.3f && num < 0.8f)
                        {
                            __instance.FlagFlightAIToFaceTarget = true;
                            __instance.AttackMode = EAbyssBossAttackMode.FIRE;
                            yield return new WaitForSeconds(25f);
                            __instance.AttackMode = EAbyssBossAttackMode.DEFAULT;
                        }
                        else if (value < 0.5f)
                        {
                            __instance.AttackMode = EAbyssBossAttackMode.SPAWNS;
                            yield return new WaitForSeconds(15f);
                            __instance.AttackMode = EAbyssBossAttackMode.DEFAULT;
                        }
                        else if (value < 0.8f)
                        {
                            __instance.FlagFlightAIToFaceTarget = true;
                            __instance.photonView.RPC("ClientExecuteBeamAttack", PhotonTargets.Others, Array.Empty<object>());
                            yield return ExecuteBeamAttack.Invoke(__instance, null);
                        }
                        else
                        {
                            __instance.FlagFlightAIToFaceTarget = true;
                            yield return longWait;
                            yield return normalWait;
                            __instance.photonView.RPC("ClientProjAttackTimed", PhotonTargets.Others, new object[]
                            {
                            PLServer.Instance.ServerProjIDCounter
                            });
                            yield return __instance.StartCoroutine((IEnumerator)ProjAttackTimed.Invoke(__instance, new object[] { PLServer.Instance.ServerProjIDCounter }));
                            PLServer.Instance.ServerProjIDCounter += 12;
                            yield return normalWait;
                        }
                        __instance.FlagFlightAIToFaceTarget = false;
                        yield return longWait;
                    }
                    else
                    {
                        yield return normalWait;
                    }
                }
                yield break;
            }
        }

        [HarmonyPatch(typeof(PLAbyssBoss), "Update")]
        class WardenUpdate
        {
            private static MethodInfo ShouldShowBossUI = AccessTools.Method(typeof(PLAbyssBoss), "ShouldShowBossUI");
            static void Postfix(PLAbyssBoss __instance)
            {
                if (PLEncounterManager.Instance.PlayerShip != null && Options.MasterHasMod)
                {
                    __instance.EngineeringSystem.Health = __instance.EngineeringSystem.MaxHealth;
                    if (!(bool)ShouldShowBossUI.Invoke(__instance, null)) 
                    {
                        if (__instance.MyHull != null && __instance.MyStats != null) __instance.MyHull.Level = Mathf.CeilToInt(PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 15) + 1;
                        __instance.MyHull.Current = __instance.MyStats.HullMax;
                    }
                }
            }
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //Increases enemy spawn number
            {
                List<CodeInstruction> instructionsList = Instructions.ToList();
                instructionsList[245].opcode = OpCodes.Ldc_I4_5;
                instructionsList[291].operand = 1.5f;
                return instructionsList.AsEnumerable();
            }
        }

        [HarmonyPatch(typeof(PLAbyssLavaBoss), "Update")]
        class Excavator
        {
            private static MethodInfo ShouldShowBossUI = AccessTools.Method(typeof(PLAbyssLavaBoss), "ShouldShowBossUI");
            static void Postfix(PLAbyssLavaBoss __instance)
            {
                if (PLEncounterManager.Instance.PlayerShip != null && Options.MasterHasMod)
                {
                    __instance.EngineeringSystem.Health = __instance.EngineeringSystem.MaxHealth;
                    __instance.ComputerSystem.Health = __instance.ComputerSystem.MaxHealth;
                    if (!(bool)ShouldShowBossUI.Invoke(__instance, null))
                    {
                        if (__instance.MyHull != null && __instance.MyStats != null) __instance.MyHull.Level = Mathf.CeilToInt(PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 15) + 1;
                        __instance.MyHull.Current = __instance.MyStats.HullMax;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(PLAbyssLavaBoss), "Start")]
        class ExcavatorWarden
        {
            static void Postfix(PLAbyssLavaBoss __instance)
            {
                __instance.StopAllCoroutines();
                __instance.StartCoroutine(TimedRoutine(__instance));
            }
            private static FieldInfo SourceRingSpeedX = AccessTools.Field(typeof(PLAbyssLavaBoss), "SourceRingSpeedX");
            private static FieldInfo SourceRingSpeedY = AccessTools.Field(typeof(PLAbyssLavaBoss), "SourceRingSpeedY");
            private static FieldInfo SourceRingSpeedZ = AccessTools.Field(typeof(PLAbyssLavaBoss), "SourceRingSpeedZ");
            static IEnumerator TimedRoutine(PLAbyssLavaBoss __instance)
            {
                WaitForSeconds VerySmallTiming = new WaitForSeconds(0.6f);
                WaitForSeconds SmallTiming = new WaitForSeconds(1.2f);
                WaitForSeconds VerySmallTimingEnhanced = new WaitForSeconds(0.4f);
                WaitForSeconds SmallTimingEnhanced = new WaitForSeconds(1f);
                bool enhancedAttacks = false;
                while (Application.isPlaying)
                {
                    if (__instance.MyActivatorVolume == null || !__instance.MyActivatorVolume.GetHasBeenTriggered() || __instance.HasBeenDestroyed)
                    {
                        yield return SmallTimingEnhanced;
                    }
                    else
                    {
                        if (__instance.MyStats.HullMax > 0f)
                        {
                            enhancedAttacks = (__instance.MyStats.HullCurrent / __instance.MyStats.HullMax < 0.66f);
                        }
                        if (enhancedAttacks && SmallTiming != SmallTimingEnhanced)
                        {
                            SmallTiming = SmallTimingEnhanced;
                            VerySmallTiming = VerySmallTimingEnhanced;
                        }
                        if (PhotonNetwork.isMasterClient && __instance.TargetShip != null)
                        {
                            float value = UnityEngine.Random.value;
                            if (value < 0.33f)
                            {
                                SourceRingSpeedX.SetValue(__instance, 10f);
                                SourceRingSpeedZ.SetValue(__instance, 10f);
                                yield return SmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                                    enhancedAttacks
                                });
                                yield return SmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                                    enhancedAttacks
                                });
                                yield return SmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                                    enhancedAttacks
                                });
                            }
                            else if (value < 0.45f && enhancedAttacks)
                            {
                                SourceRingSpeedX.SetValue(__instance, 50f);
                                SourceRingSpeedY.SetValue(__instance, 50f);
                                SourceRingSpeedZ.SetValue(__instance, 50f);
                                yield return VerySmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                                enhancedAttacks
                                });
                                yield return VerySmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                                enhancedAttacks
                                });
                                yield return VerySmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                                enhancedAttacks
                                });
                                yield return VerySmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                                enhancedAttacks
                                });
                                yield return VerySmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                                enhancedAttacks
                                });
                                yield return VerySmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                                enhancedAttacks
                                });
                                yield return VerySmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                                enhancedAttacks
                                });
                            }
                            else if (value < 0.6f)
                            {
                                SourceRingSpeedX.SetValue(__instance, 50f);
                                yield return SmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                                enhancedAttacks
                                });
                                yield return SmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                                enhancedAttacks
                                });
                            }
                            else if (value < 0.75f)
                            {
                                SourceRingSpeedY.SetValue(__instance, 50f);
                                yield return SmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                            enhancedAttacks
                                });
                                yield return SmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                            enhancedAttacks
                                });
                            }
                            else if (value < 0.9f)
                            {
                                SourceRingSpeedX.SetValue(__instance, 20f);
                                SourceRingSpeedZ.SetValue(__instance, 30f);
                                yield return SmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                            enhancedAttacks
                                });
                                yield return SmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                            enhancedAttacks
                                });
                            }
                            else
                            {
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                            enhancedAttacks
                                });
                                yield return VerySmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                            enhancedAttacks
                                });
                                yield return VerySmallTiming;
                                __instance.photonView.RPC("TriggerShockwave", PhotonTargets.All, new object[]
                                {
                            enhancedAttacks
                                });
                            }
                        }
                        SourceRingSpeedX.SetValue(__instance, 0f);
                        SourceRingSpeedY.SetValue(__instance, 0f);
                        SourceRingSpeedZ.SetValue(__instance, 1f);
                        yield return (UnityEngine.Random.value < 0.5f) ? SmallTiming : VerySmallTiming;
                    }
                }
                yield break;
            }
        }
    }
}
