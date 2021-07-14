using HarmonyLib;
using UnityEngine;
using static UnityEngine.Object;
using System.Collections.Generic;

namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLServer), "Update")]
    class Update
    {
        public static float timer = 1;
        static void Postfix()
        {
            if (PhotonNetwork.isMasterClient && PLServer.Instance != null)
            {
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
                    foreach (PLShipInfoBase ship in FindObjectsOfType(typeof(PLShipInfoBase))) //This makes all ships with the shields offline lose 10% of integrity per second
                    {
                        if (!ship.IsDrone && !ship.IsInfected)
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
                            List<PLPoweredShipComponent> allPoweredComponents = PLReactor.GetAllPoweredComponents(ship.MyStats);
                            if(shield.Current >= shield.CurrentMax * (0.99f - shield.ChargeRateMax/500)) //This is just a place holder, this for now should ensure shield is always using power
                            {
                                shield.Current = shield.CurrentMax * (0.99f - shield.ChargeRateMax/500);
                            }
                            if(shield.GetPowerPercentInput() < 0.5f && shield.GetPowerPercentInput() > 0) //This makes if shield is not reciving at least 50% it will lose 
                            {
                                float chargelost = (shield.CurrentMax * shield.ChargeRateMax) / 1000 / (shield.GetPowerPercentInput());
                                shield.Current -= chargelost > shield.CurrentMax/ 10? 10 : chargelost;
                            }
                        }
                    }
                    timer = 1;
                }
            }
        }
    }
}
