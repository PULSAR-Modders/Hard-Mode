using HarmonyLib;
using UnityEngine;
using static UnityEngine.Object;

namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLServer), "Update")]
    class Update
    {
        static void Postfix()
        {
            if (PhotonNetwork.isMasterClient)
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
                            return;
                        }
                    }
                    Warpdisableinspection.inInspection = false;
                }
            }
        }
    }
}
