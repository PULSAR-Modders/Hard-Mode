using HarmonyLib;
using UnityEngine;
using System.Reflection.Emit;
using PulsarModLoader.Patches;
using System.Collections.Generic;
using static PulsarModLoader.Patches.HarmonyHelpers;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Linq;

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
    [HarmonyPatch(typeof(PLBGShip), "TakeDamage")]
    class KillingCivilShip // This is for attacking the inoffencive ships in the outpost 448
    {
        static void Postfix(PLBGShip __instance, float damage) //They only die if the player attack, since they seem to not be affected by ramming (and the 200 damage is for the auto turrets)
        {
            if (PhotonNetwork.isMasterClient && damage != 200)
            {
                PulsarModLoader.Utilities.Messaging.Echo(PhotonTargets.All, "Ship Flagged!" + " (due to killing unarmed civilian ship in a public station)");
                PLEncounterManager.Instance.PlayerShip.IsFlagged = true;
            }
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
    [HarmonyPatch(typeof(PLSpaceTurretTargeting_FactionGuardian), "UpdateTurretTargeting")]
    class StationTurrets //This is responsible to make the space turrets attack any ship with rep too low to that faction
    {
        static public bool truth = true;
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> targetSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(PLServer),"Instance")),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLServer),"IsCrewRepRevealed")),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ObscuredBool),"op_Implicit",new System.Type[]{typeof(ObscuredBool)})),
            };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(StationTurrets),"truth")),
            };
            patchSequence[0].labels = instructions.ToList()[FindSequence(instructions, targetSequence, CheckMode.NONNULL) - 3].labels;
            return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.REPLACE, CheckMode.NONNULL, false);
        }
    }
}
