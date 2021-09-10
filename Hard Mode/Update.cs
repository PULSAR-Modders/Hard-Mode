using HarmonyLib;
using UnityEngine;
using static UnityEngine.Object;
using System.Collections.Generic;
using System.Linq;
using PulsarPluginLoader;

namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLShipInfoBase), "Update")]
    class Update
    {
        public static float timer = 1;
        static void Postfix(PLShipInfoBase __instance)
        {
            if (Options.MasterHasMod) //This is to help with desyncs due to client having the mod, but the host doesn't have it
            {
                Enemies.TheSourceTimer.timer = 360f;
                Enemies.MeteorMission.timer = 300f;
            }
            else
            {
                Enemies.TheSourceTimer.timer = 600f;
                Enemies.MeteorMission.timer = 600f;
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
                });
                if (PLEncounterManager.Instance.PlayerShip.IsFlagged && PLServer.Instance.CrewFactionID != -1 && PLServer.Instance.CrewFactionID != 1) // Checks if is flagged and has a faction, in that case it will lose alligment
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
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {

                    foreach (PLShipInfoBase ship in FindObjectsOfType(typeof(PLShipInfoBase)))
                    {
                        if (!(ship is PLHighRollersShipInfo) && ship.ShipTypeID != EShipType.E_ACADEMY)
                        {
                            if (!ship.IsDrone && !ship.IsInfected && ship.ShipTypeID != EShipType.E_CIVILIAN_FUEL) //This makes all ships with the shields offline lose 10% of integrity per second
                            {
                                PLShipInfo realship = ship as PLShipInfo;
                                PLShieldGenerator shield = ship.MyStats.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD, false);
                                if (ship != null && !realship.StartupSwitchBoard.GetStatus(2))
                                {
                                    if (shield.Current > 0)
                                    {
                                        shield.Current -= shield.CurrentMax / 10;
                                    }
                                }
                            }
                            /*
                            List<PLPoweredShipComponent> allPoweredComponents = PLReactor.GetAllPoweredComponents(ship.MyStats);
                            if(shield.Current >= shield.CurrentMax * (0.99f - shield.ChargeRateMax/500)) //This is just a place holder, this for now should ensure shield is always using power
                            {
                                shield.Current = shield.CurrentMax * (0.99f - shield.ChargeRateMax/500);
                            }
                            if(shield.GetPowerPercentInput() < 0.25f && shield.GetPowerPercentInput() > 0) //This makes if shield is not reciving at least 50% it will lose 
                            {
                                float chargelost = (shield.CurrentMax * shield.ChargeRateMax) / 500 / (shield.GetPowerPercentInput());
                                shield.Current -= chargelost > shield.CurrentMax/ 10? 10 : chargelost;
                            }
                            */

                            if (!ship.GetIsPlayerShip() && ship.ShipTypeID != EShipType.E_CIVILIAN_FUEL) //This should make attacking one ship all it's friends will attack you
                            {
                                foreach (PLShipInfoBase Allied in FindObjectsOfType(typeof(PLShipInfoBase)))
                                {
                                    if (Allied.FactionID == ship.FactionID && !Allied.HostileShips.Contains(ship.ShipID))
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
                        }
                        if (!ship.GetIsPlayerShip()) //This part is to prevent too high level thrusters (that normally make enemy miss all shots) and prevent components above level 32 
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
                        foreach (PLShipComponent component in ship.MyStats.AllComponents)
                        {
                            if (component.Level > 31) component.Level = 31;
                        }
                    }
                    timer = 1;
                }
                //This will make drone call for help to protect sector if too weak also civilians will call for help if in danger
                if ((((__instance.ShipTypeID == EShipType.E_WDDRONE1 || __instance.ShipTypeID == EShipType.E_WDDRONE2 || __instance.ShipTypeID == EShipType.E_WDDRONE3 || __instance.ShipTypeID == EShipType.E_REPAIR_DRONE) && __instance.TargetShip != null && __instance.TargetShip.GetCombatLevel() > __instance.GetCombatLevel() * 1.5) || (__instance.ShipTypeID == EShipType.E_CIVILIAN_FUEL && __instance.AlertLevel > 1)) && !PLServer.Instance.LongRangeCommsDisabled && __instance.FactionID != 6)
                {
                    __instance.DistressSignalActive = true;
                }

            }
        }
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //This should make the enemy warp faster, not just waiting the basically dead to jump
        {
            List<CodeInstruction> instructionsList = Instructions.ToList();
            instructionsList[579].operand = 0.2f;
            instructionsList[560].operand = 3f;
            return instructionsList.AsEnumerable();
        }
    }
}
