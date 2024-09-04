using HarmonyLib;
using UnityEngine;
using static UnityEngine.Object;
using System.Collections.Generic;
using System.Linq;
using PulsarModLoader;
using PulsarModLoader.Patches;
using System.Reflection.Emit;

namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLShipInfoBase), "Update")]
    class Update
    {
        public static float timer = 1;
        public static float LastGalaxySync = Time.time;
        static void Postfix(PLShipInfoBase __instance)
        {
            if (Options.MasterHasMod) //This is to help with desyncs due to client having the mod, but the host doesn't have it
            {
                CU_Campaing.TheSourceTimer.timer = 360f;
                CU_Campaing.MeteorMission.timer = 300f;
                Sector_Commanders.Keeper.KeeperUpdate.speed = 2.5f;
                //Enemies.UnseenEyePhysicalAttack.damage = 400f * PLWarpGuardian.GetPlayerBasedDifficultyMultiplier();
            }
            else
            {
                CU_Campaing.TheSourceTimer.timer = 600f;
                CU_Campaing.MeteorMission.timer = 600f;
                Sector_Commanders.Keeper.KeeperUpdate.speed = 0.5f;
                //Enemies.UnseenEyePhysicalAttack.damage = 400f;
            }
            if (PhotonNetwork.isMasterClient && __instance.GetIsPlayerShip())//This uses the player ship to make updates, better than the PLServer that likes to exception while leaving and entering a game
            {
                //This will make the relic and bounty hunters harder depending on the chaos
                Custom_Bounty_Hunters.BountyHunterBalance.MaxCombatLevel = 1.5f + PLServer.Instance.ChaosLevel / 10;
                Custom_Bounty_Hunters.BountyHunterBalance.MinCombatLevel = 1.2f + PLServer.Instance.ChaosLevel / 15;
                Custom_Bounty_Hunters.RelicHunterBalance.MaxCombatLevel = 1.5f + PLServer.Instance.ChaosLevel / 5;
                Custom_Bounty_Hunters.RelicHunterBalance.MinCombatLevel = 1.2f + PLServer.Instance.ChaosLevel / 10;
                ModMessage.SendRPC("modders.hardmode", "Hard_Mode.ReciveOptions", PhotonTargets.Others, new object[] //This is responsible to send the options to all clients
                {
                    Options.FogOfWar,
                    Options.DangerousReactor,
                    Options.MasterHasMod,
                    Options.WeakReactor,
                    Options.SpinningCycpher,
                });
                if (Time.time - LastGalaxySync > 30 && PLServer.GetCurrentSector().VisualIndication != ESectorVisualIndication.ABYSS)//Syncs the discovered sectors with all clients every 30 seconds
                {
                    foreach (PLSectorInfo sector in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                    {
                        if (sector.Discovered)
                        {
                            PLServer.Instance.photonView.RPC("ClientSectorInfo", PhotonTargets.Others, new object[]
                            {
                                sector.ID,
                                true,
                                sector.Visited
                            });
                        }
                    }
                    LastGalaxySync = Time.time;
                }
                if (PLEncounterManager.Instance.PlayerShip.IsFlagged && PLServer.Instance.CrewFactionID != -1 && PLServer.Instance.CrewFactionID != 5 && PLServer.Instance.CrewFactionID != 1) // Checks if is flagged and has a faction, in that case it will lose alligment (except AOG and PF, since neither care for that)
                {
                    PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[]
                    {
                        "Crew Alignment Lost!",
                        Color.red,
                        0,
                        "[SRCH]"
                    });
                    PLServer.Instance.CrewFactionID = -1;
                }
                if (PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.CONTRABAND_STATION) // Ensures Warp is disabled during inspection
                {
                    foreach (PLCUInvestigatorDrone drone in FindObjectsOfType(typeof(PLCUInvestigatorDrone)))
                    {
                        if (drone != null)
                        {
                            Warpdisableinspection.inInspection = true;
                            PLEncounterManager.Instance.PlayerShip.WarpChargeStage = EWarpChargeStage.E_WCS_PAUSED;
                            return;
                        }
                    }
                    Warpdisableinspection.inInspection = false;
                }
                if(PLServer.GetCurrentSector().VisualIndication != ESectorVisualIndication.ABYSS)
                {
                    foreach (PLShipInfoBase ship in FindObjectsOfType(typeof(PLShipInfoBase)))
                    {
                        if (!(ship is PLHighRollersShipInfo) && ship.MyStats != null && ship.ShipTypeID != EShipType.E_ACADEMY)
                        {
                            if (!ship.IsDrone && !ship.IsInfected && ship.ShipTypeID != EShipType.E_CIVILIAN_FUEL && !__instance.InWarp) //This makes all boardable ships with the shields offline lose 10% of integrity per second
                            {
                                if (timer > 0)
                                {
                                    timer -= Time.deltaTime;
                                }
                                else
                                {
                                    PLShipInfo realship = ship as PLShipInfo;
                                    PLShieldGenerator shield = ship.MyStats.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD, false);
                                    if (ship != null && !realship.StartupSwitchBoard.GetStatus(2) && shield != null)
                                    {
                                        if (shield.Current > 0)
                                        {
                                            shield.Current -= shield.CurrentMax / 10;
                                            timer = 1;
                                        }
                                    }
                                }
                            }
                            if (!ship.GetIsPlayerShip() && ship.ShipTypeID != EShipType.E_CIVILIAN_FUEL && ship.ShipTypeID != EShipType.E_BEACON && !ship.HasModifier(EShipModifierType.CORRUPTED)) //This should make attacking one ship all it's friends will attack you (execpt beacon)
                            {
                                foreach (PLShipInfoBase Allied in FindObjectsOfType(typeof(PLShipInfoBase)))
                                {
                                    if (!(Allied is PLHighRollersShipInfo) && Allied.ShipTypeID != EShipType.E_ACADEMY && Allied.MyStats != null && Allied.FactionID == ship.FactionID && !Allied.HostileShips.Contains(ship.ShipID) && Allied.ShipTypeID != EShipType.E_BEACON && !Allied.HasModifier(EShipModifierType.CORRUPTED))
                                    {
                                        if (Allied.GetIsPlayerShip())
                                        {
                                            foreach (int enemy in Allied.HostileShips)
                                            {
                                                if (PLEncounterManager.Instance.GetShipFromID(enemy).FactionID != ship.FactionID || (PLEncounterManager.Instance.GetShipFromID(enemy).IsFlagged && ship.FactionID != 1))
                                                {
                                                    ship.HostileShips.Add(enemy);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (int enemy in Allied.HostileShips)
                                            {
                                                if (!ship.HostileShips.Contains(enemy))
                                                {
                                                    ship.HostileShips.Add(enemy);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //Enemy will try to Escape if you are 1.5 times stronger than him (combat level)
                            if (ship.WarpChargeStage != EWarpChargeStage.E_WCS_PREPPING && ship.WarpChargeStage != EWarpChargeStage.E_WCS_READY && ((ship.HostileShips.Contains(PLEncounterManager.Instance.PlayerShip.ShipID) && PLEncounterManager.Instance.PlayerShip.GetCombatLevel() > ship.GetCombatLevel() * 1.5 && ship.FactionID != 6 && !ship.IsDrone && !ship.IsInfected && !ship.IsSectorCommander && ship.ShipTypeID != EShipType.E_BEACON) || (ship.ShipTypeID == EShipType.E_CIVILIAN_FUEL && ship.AlertLevel > 1)))
                            {
                                ship.WarpChargeStage = EWarpChargeStage.E_WCS_PREPPING;
                            }
                            else if (ship.WarpChargeStage == EWarpChargeStage.E_WCS_READY && PLBeaconInfo.GetBeaconStatAdditive(EBeaconType.E_WARP_DISABLE, false) < 0.5f && ((ship.HostileShips.Contains(PLEncounterManager.Instance.PlayerShip.ShipID) && ship.FactionID != 6 && !ship.GetIsPlayerShip() && (ship.GetRelevantCrewMember(0) != null || ship.IsDrone) && PLEncounterManager.Instance.PlayerShip.GetCombatLevel() > ship.GetCombatLevel() * 1.5) || (ship.ShipTypeID == EShipType.E_CIVILIAN_FUEL && ship.AlertLevel > 1)))
                            {
                                ship.Ship_WarpOutNow();
                            }
                            if (!ship.GetIsPlayerShip() && ship.MyStats != null) //This part is to prevent too high level thrusters (that normally make enemy miss all shots) and prevent components above level 32 
                            {
                                foreach (PLShipComponent component in ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_THRUSTER))
                                {
                                    if (component.Level > 9) component.Level = 9;
                                }
                                foreach (PLShipComponent component in ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_INERTIA_THRUSTER))
                                {
                                    if (component.Level > 9) component.Level = 9;
                                }
                            }
                            if (ship.MyStats != null)
                            {
                                foreach (PLShipComponent component in ship.MyStats.AllComponents)
                                {
                                    if (component.Level > 31) component.Level = 31;
                                }
                            }
                            //This will make drone call for help to protect sector if too weak also civilians will call for help if in danger
                            if ((((ship.ShipTypeID == EShipType.E_WDDRONE1 || ship.ShipTypeID == EShipType.E_WDDRONE2 || ship.ShipTypeID == EShipType.E_WDDRONE3 || ship.ShipTypeID == EShipType.E_REPAIR_DRONE) && ship.TargetShip != null && ship.TargetShip.GetCombatLevel() > ship.GetCombatLevel() * 1.5) || (ship.ShipTypeID == EShipType.E_CIVILIAN_FUEL && ship.AlertLevel > 1)) && !PLServer.Instance.LongRangeCommsDisabled && ship.FactionID != 6)
                            {
                                ship.DistressSignalActive = true;
                            }
                        }
                    }
                    
                }

            }
        }
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) //This should make the enemy warp faster, not just waiting the basically dead to jump
        {
            List<CodeInstruction> targetSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLShipInfoBase),"MyStats")),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLShipStats),"get_HullMax")),
                new CodeInstruction(OpCodes.Ldc_R4, 0.1f)
            };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLShipInfoBase),"MyStats")),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLShipStats),"get_HullMax")),
                new CodeInstruction(OpCodes.Ldc_R4, 0.2f)
            };

            instructions = HarmonyHelpers.PatchBySequence(instructions, targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);

            targetSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLShipInfoBase),"LastHullDamageRecievedTime")),
                new CodeInstruction(OpCodes.Sub),
                new CodeInstruction(OpCodes.Ldc_R4,5f)
            };
            patchSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLShipInfoBase),"LastHullDamageRecievedTime")),
                new CodeInstruction(OpCodes.Sub),
                new CodeInstruction(OpCodes.Ldc_R4,3f)
            };


            return HarmonyHelpers.PatchBySequence(instructions, targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);

        }
    }
}
