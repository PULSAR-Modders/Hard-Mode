using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System;
using System.Collections;
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
            static void Postfix(PLAbyssShipInfo __instance) 
            {
                if(Options.MasterHasMod)__instance.Flood_EndingPos = __instance.Flood_StartingPos + new Vector3(0, 1.06f);
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
            static void Postfix(PLAbyssFighterInfo __instance)
            {
                if (PLEncounterManager.Instance.PlayerShip != null)
                {
                    if (__instance.MyHull != null) __instance.MyHull.Level += Mathf.CeilToInt(PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 15);
                    __instance.FirePower += PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 50;
                }
            }
        }

        [HarmonyPatch(typeof(PLAbyssHeavyFighterInfo), "SetupShipStats")]
        class HeavyAbyssFighter
        {
            static void Postfix(PLAbyssHeavyFighterInfo __instance)
            {
                if (PLEncounterManager.Instance.PlayerShip != null)
                {
                    if (__instance.MyHull != null) __instance.MyHull.Level += Mathf.CeilToInt(PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 15);
                    __instance.FirePower += PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 50;
                }
            }
        }
        [HarmonyPatch(typeof(PLMatrixDroneInfo), "SetupShipStats")]
        class Cultivator
        {
            static void Postfix(PLMatrixDroneInfo __instance)
            {
                if (PLEncounterManager.Instance.PlayerShip != null)
                {
                    if(__instance.MyHull != null)__instance.MyHull.Level = Mathf.CeilToInt(PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 15) + 1;
                    __instance.FireRateScalar += PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 50;
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
            static void Postfix(PLMatrixDroneCommanderInfo __instance)
            {
                if (__instance.ShouldShowBossUI() && Options.MasterHasMod)
                {
                    if (PLEncounterManager.Instance.PlayerShip != null && Options.MasterHasMod)
                    {
                        __instance.FireRateScalar = 2 + PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 50;
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
                if (!__instance.ShouldShowBossUI() && __instance.MyHull != null && __instance.MyStats != null && Options.MasterHasMod)
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
            static float cachedWeakHealth = 5000;
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
                    if (!__instance.ShouldShowBossUI())
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
                            yield return __instance.ExecuteBeamAttack();
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
                            yield return __instance.StartCoroutine(__instance.ProjAttackTimed(PLServer.Instance.ServerProjIDCounter));
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
            static void Postfix(PLAbyssBoss __instance)
            {
                if (PLEncounterManager.Instance.PlayerShip != null && Options.MasterHasMod)
                {
                    __instance.EngineeringSystem.Health = __instance.EngineeringSystem.MaxHealth;
                    if (!__instance.ShouldShowBossUI()) 
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
            static void Postfix(PLAbyssLavaBoss __instance)
            {
                if (PLEncounterManager.Instance.PlayerShip != null && Options.MasterHasMod)
                {
                    __instance.EngineeringSystem.Health = __instance.EngineeringSystem.MaxHealth;
                    __instance.ComputerSystem.Health = __instance.ComputerSystem.MaxHealth;
                    if (!__instance.ShouldShowBossUI())
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
                                __instance.SourceRingSpeedX = 10f;
                                __instance.SourceRingSpeedZ = 10f;
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
                                __instance.SourceRingSpeedX = 50f;
                                __instance.SourceRingSpeedY = 50f;
                                __instance.SourceRingSpeedZ = 50f;
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
                                __instance.SourceRingSpeedX = 50f;
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
                                __instance.SourceRingSpeedY = 50f;
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
                                __instance.SourceRingSpeedX = 20f;
                                __instance.SourceRingSpeedZ = 30f;
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
                        __instance.SourceRingSpeedX = 0f;
                        __instance.SourceRingSpeedY = 0f;
                        __instance.SourceRingSpeedZ = 1f;
                        yield return (UnityEngine.Random.value < 0.5f) ? SmallTiming : VerySmallTiming;
                    }
                }
                yield break;
            }
        }
    }
}
