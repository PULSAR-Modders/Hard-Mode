using HarmonyLib;
using UnityEngine;

namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLWarpDriveScreen), "Update")]
    class Warpdisableinspection
    {
        static public bool inInspection = false;
        static void Postfix(UISprite ___WarpDrivePanel, PLWarpDriveScreen __instance, UISprite ___JumpComputerPanel, UISprite ___m_BlockingTargetOnboardPanel) // this is what disables the warp during inspection
        {
            PLGlobal.SafeGameObjectSetActive(___WarpDrivePanel.gameObject, !inInspection && !__instance.MyScreenHubBase.OptionalShipInfo.BlockingCombatTargetOnboard && __instance.MyScreenHubBase.OptionalShipInfo.WarpChargeStage != EWarpChargeStage.E_WCS_ACTIVE && __instance.MyScreenHubBase.OptionalShipInfo.NumberOfFuelCapsules > 0 && !__instance.MyScreenHubBase.OptionalShipInfo.InWarp && !__instance.MyScreenHubBase.OptionalShipInfo.Abandoned);
            PLGlobal.SafeGameObjectSetActive(___JumpComputerPanel.gameObject, !inInspection && !__instance.MyScreenHubBase.OptionalShipInfo.BlockingCombatTargetOnboard);
            PLGlobal.SafeGameObjectSetActive(___m_BlockingTargetOnboardPanel.gameObject, inInspection || __instance.MyScreenHubBase.OptionalShipInfo.BlockingCombatTargetOnboard);
        }
    }
    [HarmonyPatch(typeof(PLWarpStation), "SetTargetedSectorID")]
    class FlagWarp
    {
        static public bool warpflag = false;
        static public int target = -1;
        static void Postfix(int inSectorID, bool chargeRequired) // This checks if the warp gate was a free one
        {
            warpflag = !chargeRequired;
            target = inSectorID;
        }
    }
    [HarmonyPatch(typeof(PLServer), "NetworkBeginWarp")]
    class IllegalWarp
    {
        static void Postfix(int HubID) // This will make using a free warp gate punish you
        {
            if (PhotonNetwork.isMasterClient && FlagWarp.warpflag && FlagWarp.target == HubID)
            {
                PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[]
                        {
                            "Illegal Action Reported!",
                            Color.red,
                            0,
                            "[SRCH]"
                        });
                PLEncounterManager.Instance.PlayerShip.IsFlagged = true;
                PLServer.Instance.ServerChaosIncrease(1);
                FlagWarp.warpflag = false;
                FlagWarp.target = -1;
            }
        }
    }
    [HarmonyPatch(typeof(PLServer), "ServerRepairHull")]
    class IllegalRepair
    {
        static void Postfix(int cost) // This will make the free repair flag you
        {
            if (PhotonNetwork.isMasterClient && cost == 0)
            {
                PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[]
                            {
                            "Illegal Action Reported!",
                            Color.red,
                            0,
                            "[SRCH]"
                            });
                PLEncounterManager.Instance.PlayerShip.IsFlagged = true;
            }
        }
    }
}
