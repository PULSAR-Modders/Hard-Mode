using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLShipInfoBase), "Ship_WarpOutNow")]
    class Lose_Rep_With_Faction
    {
        static void Prefix(PLShipInfoBase __instance)
        {
            if (__instance.FactionID >= 0 && __instance.FactionID <= 3 && __instance.HostileShips.Contains(PLEncounterManager.Instance.PlayerShip.ShipID)) // This will decrease Your rep with the faction of the ship that escaped and your faction
            {
                if (__instance.GetModifiers() == (int)EShipModifierType.REPUTABLE)
                {
                    PLServer.Instance.RepLevels[__instance.FactionID] -= 2;
                    PulsarPluginLoader.Utilities.Messaging.Echo(PhotonTargets.All, "-2 Rep for " + PLGlobal.GetFactionTextForFactionID(__instance.FactionID) + " (due to reports of escaped reputable ship)");
                }
                else
                {
                    PLServer.Instance.RepLevels[__instance.FactionID] -= 1;
                    PulsarPluginLoader.Utilities.Messaging.Echo(PhotonTargets.All, "-1 Rep for " + PLGlobal.GetFactionTextForFactionID(__instance.FactionID) + " (due to reports of escaped ship)");
                }
                if (PLServer.Instance.CrewFactionID == 3)
                {
                    PLServer.Instance.RepLevels[3] -= 2;
                    PulsarPluginLoader.Utilities.Messaging.Echo(PhotonTargets.All, "-2 Rep for " + PLGlobal.GetFactionTextForFactionID(3) + " (due to enemies escaping from your attack)");
                }
                else if (PLServer.Instance.CrewFactionID != -1)
                {
                    PLServer.Instance.RepLevels[PLServer.Instance.CrewFactionID] -= 1;
                    PulsarPluginLoader.Utilities.Messaging.Echo(PhotonTargets.All, "-1 Rep for " + PLGlobal.GetFactionTextForFactionID(PLServer.Instance.CrewFactionID) + " (due to enemies escaping)");
                }
            }

        }
    }
    [HarmonyPatch(typeof(PLServer), "Update")]
    class Feckthisshitiamout
    {
        static void Postfix() // Enemy will try to Escape if you are 1.5 times stronger than him (combat level)
        {
            foreach (PLShipInfoBase ship in UnityEngine.Object.FindObjectsOfType(typeof(PLShipInfoBase)))
            {
                if (PLEncounterManager.Instance.PlayerShip.GetCombatLevel() > ship.GetCombatLevel() * 1.5 && ship.WarpChargeStage == EWarpChargeStage.E_WCS_COLD_START && ship.FactionID != 6 && !ship.IsInfected && !ship.IsSectorCommander && ship.HostileShips.Contains(PLEncounterManager.Instance.PlayerShip.ShipID) && ship.ShipTypeID != EShipType.E_BEACON)
                {
                    ship.WarpChargeStage = EWarpChargeStage.E_WCS_PREPPING;
                }
                else if (ship.WarpChargeStage == EWarpChargeStage.E_WCS_READY && ship.FactionID != 6 && !ship.GetIsPlayerShip() && ship.GetRelevantCrewMember(0) != null)
                {
                    ship.Ship_WarpOutNow();
                }
            }
        }
    }
    [HarmonyPatch(typeof(PLShipInfoBase), "Update")]
    class Warpmoretimes
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //This should make the enemy warp faster, not just waiting the basically dead to jump
        {
            List<CodeInstruction> instructionsList = Instructions.ToList();
            instructionsList[579].operand = 0.2f;
            instructionsList[560].operand = 3f;
            return instructionsList.AsEnumerable();
        }
    }
}
