using HarmonyLib;
using UnityEngine;
using static UnityEngine.Object;

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
                            if (ship != null && !realship.StartupSwitchBoard.GetStatus(2))
                            {
                                PLShieldGenerator shield = ship.MyStats.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD, false);
                                if (shield.Current > 0)
                                {
                                    shield.Current -= shield.Max / 10;
                                }
                            }
                        }
                    }
                    timer = 1;
                }
            }
        }
    }
}
