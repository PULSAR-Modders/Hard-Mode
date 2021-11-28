using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection.Emit;
using UnityEngine;
using static PulsarModLoader.Patches.HarmonyHelpers;

namespace Hard_Mode
{
    internal class Sector_Commanders //With recent Updates and the chaos deterministic patched, caustic and grim are good enough
    {
        class AncientSentry
        {
            static float LastEMP = Time.time;
            [HarmonyPatch(typeof(PLCorruptedDroneShipInfo), "SetupShipStats")]
            class AncientPatch
            {
                static void Postfix(PLCorruptedDroneShipInfo __instance)
                {
                    if (PhotonNetwork.isMasterClient)
                    {
                        PLShipStats MyStats = __instance.MyStats;
                        List<PLShipComponent> components = MyStats.AllComponents;
                        for (int i = components.Count - 1; i > -1; i--)
                        {
                            if (components[i].SlotType == ESlotType.E_COMP_TURRET)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                            else if (components[i].SlotType == ESlotType.E_COMP_HULL)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                            else if (components[i].SlotType == ESlotType.E_COMP_REACTOR)
                            {
                                __instance.MyStats.RemoveShipComponent(components[i]);
                            }
                        }
                        for (int i = 0; i < 19; i++)
                        {
                            MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(10, 7, (int)PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 100, 0, 12), null), -1, ESlotType.E_COMP_TURRET);
                        }
                        int hulllive = (int)PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 30;
                        if (hulllive < 3)
                        {
                            hulllive = 3;
                        }
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(6, 5, hulllive, 0, 12), null), -1, ESlotType.E_COMP_HULL);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 14, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 14, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 25, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 4, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(3, 9, (int)PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 75, 0, 12), null), -1, ESlotType.E_COMP_REACTOR);
                        __instance.EngineeringSystem.MaxHealth = 200f;
                        __instance.EngineeringSystem.Health = 200f;
                        __instance.WeaponsSystem.MaxHealth = 200f;
                        __instance.WeaponsSystem.Health = 200f;
                        __instance.ComputerSystem.MaxHealth = 200f;
                        __instance.ComputerSystem.Health = 200f;
                        __instance.Regen_DSO.MaxHealth = 5000f * PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 25;
                        __instance.Regen_DSO.Health = 5000f * PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 25;
                        __instance.Missiles_DSO.MaxHealth = 3000f * PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 25;
                        __instance.Missiles_DSO.Health = 3000f * PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 25;
                    }
                    PulsarModLoader.Utilities.Logger.Info("local");
                }
            }
            [HarmonyPatch(typeof(PLCorruptedLaserTurret), "Tick")]
            class Ancientturret
            {
                static void Postfix(PLCorruptedLaserTurret __instance)
                {
                    if (Options.MasterHasMod)
                    {
                        __instance.m_Damage = 230f * (PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 100) * (Update.sickomode ? 3f : 1.5f);
                    }
                }

            }
            [HarmonyPatch(typeof(PLAncientMissle), "Start")]
            class AncientMissile
            {
                static void Postfix(PLAncientMissle __instance, ref bool ___CanHitOwner)
                {
                    if (Options.MasterHasMod)
                    {
                        __instance.Damage = (3000f * PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 75) * (Update.sickomode ? 3f : 1.5f);
                        __instance.TurnFactor = (2f * PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 25) * (Update.sickomode ? 3f : 1.5f);
                        __instance.AccelerationFactor = (0.3f * PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 100) * (Update.sickomode ? 3f : 1.5f);
                        __instance.Speed = (200f * PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 100) * (Update.sickomode ? 3f : 1.5f);
                        __instance.MaxLifetime = (60f * PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 75) * (Update.sickomode ? 3f : 1.5f);
                        __instance.TrackingPower *= PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 75 * (Update.sickomode ? 3f : 1.5f);
                        ___CanHitOwner = false;
                    }
                }
            }
            [HarmonyPatch(typeof(PLCorruptedDroneShipInfo), "Update")]
            class Update
            {
                public static bool sickomode = false;
                static void Postfix(PLCorruptedDroneShipInfo __instance, ref float ___Server_LastMissleFireTime)
                {
                    if (PhotonNetwork.isMasterClient)
                    {
                        if (PLEncounterManager.Instance.PlayerShip != null && Vector3.Distance(__instance.Exterior.transform.position, PLEncounterManager.Instance.PlayerShip.Exterior.transform.position) < 500 && Time.time - LastEMP > 60f)
                        {
                            __instance.photonView.RPC("EMPBlast", PhotonTargets.All, new object[0]);
                            LastEMP = Time.time;
                        }
                        if (__instance.Regen_DSO != null && __instance.Regen_DSO.Health > 0f && __instance.MyHull != null)
                        {
                            __instance.MyHull.Current += Time.deltaTime * (90f * PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 50);
                            __instance.MyHull.Current = Mathf.Clamp(__instance.MyHull.Current, 0f, __instance.MyStats.HullMax);
                        }
                        if (Time.time - ___Server_LastMissleFireTime > 20f / (PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 100))
                        {
                            ___Server_LastMissleFireTime = 0f;
                        }
                        if (__instance.MyHull.Current < (__instance.MyStats.HullMax * 0.5f) && !sickomode)
                        {
                            List<PLShipComponent> components = __instance.MyStats.AllComponents;
                            for (int i = components.Count - 1; i > -1; i--)
                            {
                                if (components[i].SlotType == ESlotType.E_COMP_TURRET)
                                {
                                    __instance.MyStats.RemoveShipComponent(components[i]);
                                }
                                else if (components[i].SlotType == ESlotType.E_COMP_CPU)
                                {
                                    __instance.MyStats.RemoveShipComponent(components[i]);
                                }
                            }
                            for (int i = 0; i < 19; i++)
                            {
                                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(10, 7, ((int)PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 100) * 3, 0, 12), null), -1, ESlotType.E_COMP_TURRET);
                            }
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 15, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 15, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 14, 24, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 14, 24, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 25, 15, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 4, 15, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 15, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 15, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                            sickomode = true;
                        }
                    }
                }
            }
        }
        public class Keeper
        {
            [HarmonyPatch(typeof(PLSwarmKeeperShipInfo), "SetupShipStats")]
            class KeeperPatch
            {
                static void Postfix(PLSwarmKeeperShipInfo __instance, bool previewStats)
                {
                    if (PhotonNetwork.isMasterClient || previewStats)
                    {
                        PLShipStats MyStats = __instance.MyStats;
                        List<PLShipComponent> components = MyStats.AllComponents;
                        for (int i = components.Count - 1; i > -1; i--)
                        {
                            if (components[i].SlotType == ESlotType.E_COMP_TURRET)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                            else if (components[i].SlotType == ESlotType.E_COMP_HULL)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                            else if (components[i].SlotType == ESlotType.E_COMP_MANEUVER_THRUSTER)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                            else if (components[i].SlotType == ESlotType.E_COMP_REACTOR)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                        }
                        int hulllevel = (int)PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 30;
                        if (hulllevel < 3)
                        {
                            hulllevel = 3;
                        }
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(6, 2, hulllevel + 6, 0, 12), null), -1, ESlotType.E_COMP_HULL);
                        int turretlevel = (int)PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 25;
                        if (turretlevel < 4)
                        {
                            turretlevel = 4;
                        }
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(10, 0, turretlevel, 0, 12), null), -1, ESlotType.E_COMP_TURRET);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(10, 0, turretlevel, 0, 12), null), -1, ESlotType.E_COMP_TURRET);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(10, 0, turretlevel, 0, 12), null), -1, ESlotType.E_COMP_TURRET);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(26, 0, 5, 0, 12), null), -1, ESlotType.E_COMP_MANEUVER_THRUSTER);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(26, 0, 5, 0, 12), null), -1, ESlotType.E_COMP_MANEUVER_THRUSTER);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 14, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 14, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(3, 8, ((int)PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 100) + 2, 0, 12), null), -1, ESlotType.E_COMP_REACTOR);
                        __instance.EngineeringSystem.MaxHealth = 1000f;
                        __instance.EngineeringSystem.Health = 1000f;
                        __instance.WeaponsSystem.MaxHealth = 200f;
                        __instance.WeaponsSystem.Health = 200f;
                        __instance.ComputerSystem.MaxHealth = 200f;
                        __instance.ComputerSystem.Health = 200f;
                    }
                }
            }

            [HarmonyPatch(typeof(PLSwarmKeeperShipInfo), "Update")]
            public class KeeperUpdate //This is so the keeper beam moves faster
            {
                public static float speed = 0.5f;
                private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
                {
                    List<CodeInstruction> targetSequence = new List<CodeInstruction>
                    {
                        new CodeInstruction(OpCodes.Ldc_R4, 0.5f),
                        new CodeInstruction(OpCodes.Mul),
                    };
                    List<CodeInstruction> patchSequence = new List<CodeInstruction>
                    {
                        new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(KeeperUpdate),"speed")),
                        new CodeInstruction(OpCodes.Mul),
                    };
                    return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.REPLACE, CheckMode.NONNULL, false);
                }
            }
        }
        class Swarm
        {
            [HarmonyPatch(typeof(PLSwarmCommanderInfo), "SetupShipStats")]
            class SwarmPatch
            {
                static void Postfix(PLSwarmCommanderInfo __instance, bool previewStats)
                {
                    if (PhotonNetwork.isMasterClient || previewStats)
                    {
                        PLShipStats MyStats = __instance.MyStats;
                        List<PLShipComponent> components = MyStats.AllComponents;
                        for (int i = components.Count - 1; i > -1; i--)
                        {
                            if (components[i].SlotType == ESlotType.E_COMP_MANEUVER_THRUSTER)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                            else if (components[i].SlotType == ESlotType.E_COMP_THRUSTER)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                            else if (components[i].SlotType == ESlotType.E_COMP_REACTOR)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                        }
                        int thrusterlevel = (int)PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 25;
                        if (thrusterlevel < 6)
                        {
                            thrusterlevel = 6;
                        }
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(26, 2, thrusterlevel, 0, 12), null), -1, ESlotType.E_COMP_MANEUVER_THRUSTER);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(26, 2, thrusterlevel, 0, 12), null), -1, ESlotType.E_COMP_MANEUVER_THRUSTER);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(9, 3, thrusterlevel, 0, 12), null), -1, ESlotType.E_COMP_THRUSTER);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 14, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 14, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 14, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 14, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 14, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(3, 8, thrusterlevel + 15, 0, 12), null), -1, ESlotType.E_COMP_REACTOR);
                        __instance.EngineeringSystem.MaxHealth = 1000f;
                        __instance.EngineeringSystem.Health = 1000f;
                        __instance.WeaponsSystem.MaxHealth = 200f;
                        __instance.WeaponsSystem.Health = 200f;
                        __instance.ComputerSystem.MaxHealth = 200f;
                        __instance.ComputerSystem.Health = 200f;
                    }
                }
            }
            [HarmonyPatch(typeof(PLSwarmCommanderInfo), "Update")]
            class SwarmUpdate
            {
                static void Postfix(PLSwarmCommanderInfo __instance)
                {
                    if (Options.MasterHasMod)
                    {
                        if (__instance.MyHull.Current < __instance.MyStats.HullMax * 0.5)
                        {
                            __instance.MyStats.ManeuverThrustOutputCurrent *= 50f * ((int)PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 100);
                            __instance.MyStats.ManeuverThrustOutputMax *= 50f * ((int)PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 100);
                        }
                    }
                }
            }
        }
        class DeathWarden
        {
            [HarmonyPatch(typeof(PLDeathseekerCommanderDrone), "SetupShipStats")]
            class WardenPatch
            {
                static void Postfix(PLDeathseekerCommanderDrone __instance)
                {
                    if (PhotonNetwork.isMasterClient)
                    {
                        PLShipStats MyStats = __instance.MyStats;
                        List<PLShipComponent> components = MyStats.AllComponents;
                        for (int i = components.Count - 1; i > -1; i--)
                        {
                            if (components[i].SlotType == ESlotType.E_COMP_TURRET)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                            else if (components[i].SlotType == ESlotType.E_COMP_HULL)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                            else if (components[i].SlotType == ESlotType.E_COMP_INERTIA_THRUSTER)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                            else if (components[i].SlotType == ESlotType.E_COMP_THRUSTER)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                            else if (components[i].SlotType == ESlotType.E_COMP_REACTOR)
                            {
                                MyStats.RemoveShipComponent(components[i]);
                            }
                        }
                        int hulllevel = (int)PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 100;
                        if (hulllevel < 3)
                        {
                            hulllevel = 3;
                        }
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(6, 12, hulllevel + 3, 0, 12), null), -1, ESlotType.E_COMP_HULL);
                        int turretlevel = (int)PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 75;
                        if (turretlevel < 4)
                        {
                            turretlevel = 4;
                        }
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(10, 9, turretlevel, 0, 12), null), -1, ESlotType.E_COMP_TURRET);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(10, 9, turretlevel, 0, 12), null), -1, ESlotType.E_COMP_TURRET);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(25, 3, 20, 0, 12), null), -1, ESlotType.E_COMP_INERTIA_THRUSTER);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(25, 3, 20, 0, 12), null), -1, ESlotType.E_COMP_INERTIA_THRUSTER);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(25, 3, 20, 0, 12), null), -1, ESlotType.E_COMP_INERTIA_THRUSTER);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(25, 3, 20, 0, 12), null), -1, ESlotType.E_COMP_INERTIA_THRUSTER);
                        int thrusterlevel = (int)PLEncounterManager.Instance.PlayerShip.GetCombatLevel() / 75;
                        if (thrusterlevel < 4)
                        {
                            thrusterlevel = 4;
                        }
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(9, 3, thrusterlevel, 0, 12), null), -1, ESlotType.E_COMP_THRUSTER);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(9, 3, thrusterlevel, 0, 12), null), -1, ESlotType.E_COMP_THRUSTER);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 14, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 14, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(7, 7, 5, 0, 12), null), -1, ESlotType.E_COMP_CPU);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(3, 8, thrusterlevel + 15, 0, 12), null), -1, ESlotType.E_COMP_REACTOR);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(17, 6, 0, 0, 12), null), -1, ESlotType.E_COMP_PROGRAM);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(17, 6, 0, 0, 12), null), -1, ESlotType.E_COMP_PROGRAM);
                        MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(17, 0, 0, 0, 12), null), -1, ESlotType.E_COMP_PROGRAM);
                        __instance.EngineeringSystem.MaxHealth = 200f;
                        __instance.EngineeringSystem.Health = 200f;
                        __instance.WeaponsSystem.MaxHealth = 200f;
                        __instance.WeaponsSystem.Health = 200f;
                        __instance.ComputerSystem.MaxHealth = 200f;
                        __instance.ComputerSystem.Health = 200f;
                        PLShockSphere shock = __instance.Exterior.GetComponentInChildren<PLShockSphere>();
                        if (shock != null)
                        {
                            shock.Intensity = 6f;
                        }
                    }
                }
            }
        }
    }
}
