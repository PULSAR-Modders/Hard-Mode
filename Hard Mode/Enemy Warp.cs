using PulsarModLoader.Utilities;
using HarmonyLib;

namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLShipInfoBase), "Ship_WarpOutNow")]
    class Lose_Rep_With_Faction
    {
        static void Prefix(PLShipInfoBase __instance)
        {
            if (__instance.FactionID >= 0 && __instance.FactionID <= 3 && __instance.HostileShips.Contains(PLEncounterManager.Instance.PlayerShip.ShipID) && PhotonNetwork.isMasterClient && PLEncounterManager.Instance.PlayerShip != null && PLEncounterManager.Instance.PlayerShip.HostileShips.Contains(__instance.ShipID) && !__instance.HasModifier(EShipModifierType.CORRUPTED)) // This will decrease Your rep with the faction of the ship that escaped and your faction
            {
                if (__instance.GetModifiers() == (int)EShipModifierType.REPUTABLE)
                {
                    PLServer.Instance.RepLevels[__instance.FactionID] -= 2;
                    Messaging.Echo(PhotonTargets.All, "-2 Rep for " + PLGlobal.GetFactionTextForFactionID(__instance.FactionID) + " (due to reports of escaped reputable ship)");
                }
                else if(__instance.ShipTypeID == EShipType.E_CIVILIAN_FUEL) 
                {
                    PLServer.Instance.RepLevels[0] -= 3;
                    Messaging.Echo(PhotonTargets.All, "-3 Rep for " + PLGlobal.GetFactionTextForFactionID(0) + " (due to reports of attacking unarmed civilian)");
                    Messaging.Echo(PhotonTargets.All, "Ship Flagged!" + " (due to reports of attacking unarmed civilian)");
                    PLEncounterManager.Instance.PlayerShip.IsFlagged = true;
                }
                else
                {
                    PLServer.Instance.RepLevels[__instance.FactionID] -= 1;
                    Messaging.Echo(PhotonTargets.All, "-1 Rep for " + PLGlobal.GetFactionTextForFactionID(__instance.FactionID) + " (due to reports of escaped ship)");
                }
                if (PLServer.Instance.CrewFactionID == 2 && __instance.ShipTypeID != EShipType.E_CIVILIAN_FUEL)
                {
                    PLServer.Instance.RepLevels[2] -= 2;
                    Messaging.Echo(PhotonTargets.All, "-2 Rep for " + PLGlobal.GetFactionTextForFactionID(2) + " (due to enemies escaping from your attack)");
                }
                else if (PLServer.Instance.CrewFactionID != 5 && PLServer.Instance.CrewFactionID != -1 && __instance.ShipTypeID != EShipType.E_CIVILIAN_FUEL)
                {
                    PLServer.Instance.RepLevels[PLServer.Instance.CrewFactionID] -= 1;
                    Messaging.Echo(PhotonTargets.All, "-1 Rep for " + PLGlobal.GetFactionTextForFactionID(PLServer.Instance.CrewFactionID) + " (due to enemies escaping)");
                }
            }

        }
    }
}
