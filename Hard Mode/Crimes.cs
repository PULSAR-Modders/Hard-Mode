using HarmonyLib;
using UnityEngine;
using System.Reflection.Emit;
using System.Collections.Generic;
using static PulsarModLoader.Patches.HarmonyHelpers;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Linq;
using System.Reflection;

namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLWarpDriveScreen), "Update")]
    class Warpdisableinspection
    {
        static public bool inInspection = false;
        static void Postfix(UISprite ___WarpDrivePanel, PLWarpDriveScreen __instance, UISprite ___JumpComputerPanel, UISprite ___m_BlockingTargetOnboardPanel) // this is what disables the warp during inspection
        {
            if (Options.MasterHasMod && inInspection)
            {
                PLGlobal.SafeGameObjectSetActive(___WarpDrivePanel.gameObject, false);
                PLGlobal.SafeGameObjectSetActive(___JumpComputerPanel.gameObject, false);
                PLGlobal.SafeGameObjectSetActive(___m_BlockingTargetOnboardPanel.gameObject, true);
            }
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
            if (PhotonNetwork.isMasterClient && damage != 200 && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.IsFlagged)
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
            if (PhotonNetwork.isMasterClient && FlagWarp.warpflag && FlagWarp.target == HubID && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.IsFlagged)
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
            if (PhotonNetwork.isMasterClient && cost == 0 && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.IsFlagged)
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

    [HarmonyPatch(typeof(PLCUInvestigatorDrone), "Start")]
    class InvestigationDroneStart //This can add more things for the drone to check
    {
        private static FieldInfo ComponentsToCasuallyInvestigate = AccessTools.Field(typeof(PLCUInvestigatorDrone), "ComponentsToCasuallyInvestigate");
        static void Postfix(PLCUInvestigatorDrone __instance)
        {
            if (Options.MasterHasMod)
            {
                InvestigationDroneUpdate.foundSecret = false;
                InvestigationDroneUpdate.secretCargo = new List<CargoObjectDisplay>();
                /*
                if (__instance.CurrentShip != null && __instance.CurrentShip.InteriorDynamic != null)
                {
                    __instance.ComponentsToCasuallyInvestigate.AddRange(__instance.CurrentShip.InteriorDynamic.GetComponentsInChildren<PLSystemInstance>());
                }
                if (__instance.CurrentShip != null && __instance.CurrentShip.InteriorStatic != null)
                {
                    __instance.ComponentsToCasuallyInvestigate.AddRange(__instance.CurrentShip.InteriorStatic.GetComponentsInChildren<PLSystemInstance>());
                }
                */
                List<Component> components = (List<Component>)ComponentsToCasuallyInvestigate.GetValue(__instance);
                components.Add(__instance.CurrentShip.ResearchLockerCollider.transform);
                ComponentsToCasuallyInvestigate.SetValue(__instance, components);
            }
        }
    }
    [HarmonyPatch(typeof(PLCUInvestigatorDrone), "Update")]
    class InvestigationDroneUpdate //This is to check for illegal components
    {
        public static bool foundSecret = false;

        public static List<CargoObjectDisplay> secretCargo = new List<CargoObjectDisplay>();
        private static FieldInfo ContrabandFound = AccessTools.Field(typeof(PLCUInvestigatorDrone), "ContrabandFound");
        private static MethodInfo GetVisualForDroppedItem = AccessTools.Method(typeof(PLGameStatic), "GetVisualForDroppedItem");
        private static FieldInfo currentInvestigationTarget = AccessTools.Field(typeof(PLCUInvestigatorDrone), "currentInvestigationTarget");
        private static FieldInfo lastInvestigateActionCompletedTime = AccessTools.Field(typeof(PLCUInvestigatorDrone), "lastInvestigateActionCompletedTime");
        private static FieldInfo ComponentsToCasuallyInvestigate = AccessTools.Field(typeof(PLCUInvestigatorDrone), "ComponentsToCasuallyInvestigate");
        static void Postfix(PLCUInvestigatorDrone __instance)
        {
            if (Options.MasterHasMod)
            {
                Transform _currentInvestigationTarget = (Transform)currentInvestigationTarget.GetValue(__instance);
                if (!(bool)ContrabandFound.GetValue(__instance))
                {
                    foreach (PLPlayerDroppedItem itemDrop in __instance.CurrentShip.AllPlayerDroppedItems)
                    {

                        PLPawnItem item = PLPawnItem.CreatePawnItemFromHash((int)itemDrop.ItemHash.GetDecrypted());
                        float num = 7f;
                        RaycastHit hit;
                        bool hitSomething = Physics.Linecast(__instance.transform.position, itemDrop.Position, out hit);
                        DroppedItemVisual droppedItemVisual = (DroppedItemVisual)GetVisualForDroppedItem.Invoke(PLGameStatic.Instance, new object[] { itemDrop });
                        if (item != null && !hitSomething && droppedItemVisual != null)
                        {
                            float distance = Vector3.SqrMagnitude(__instance.transform.position - itemDrop.Position);
                            if (distance < num)
                            {
                                num = distance;
                                currentInvestigationTarget.SetValue(__instance, droppedItemVisual.Visual.transform);
                            }
                        }
                        else if (item != null && droppedItemVisual != null && hit.collider.GetComponentsInParent<GameObject>() == null)
                        {
                            float distance = Vector3.SqrMagnitude(__instance.transform.position - itemDrop.Position);
                            if (distance < num)
                            {
                                num = distance;
                                currentInvestigationTarget.SetValue(__instance, droppedItemVisual.Visual.transform);
                            }

                        }
                        if (_currentInvestigationTarget != null)
                        {
                            __instance.Scan.startLifetime = Vector3.Distance(_currentInvestigationTarget.position, __instance.transform.position) * 0.2f;
                            __instance.ScanOutline.startLifetime = __instance.Scan.startLifetime;
                            Renderer componentInChildren = _currentInvestigationTarget.GetComponentInChildren<Renderer>();
                            if (componentInChildren != null)
                            {
                                __instance.ScanGridRoot.transform.position = componentInChildren.bounds.center;
                                __instance.ScanGridRoot.transform.rotation = Quaternion.identity;
                                __instance.GridScanX.startLifetime = componentInChildren.bounds.size.x;
                                __instance.GridScanY.startLifetime = componentInChildren.bounds.size.y;
                                __instance.GridScanZ.startLifetime = componentInChildren.bounds.size.z;
                            }
                            else
                            {
                                __instance.ScanGridRoot.transform.position = _currentInvestigationTarget.position;
                                __instance.ScanGridRoot.transform.rotation = Quaternion.identity;
                                __instance.GridScanX.startLifetime = 1f;
                                __instance.GridScanY.startLifetime = 1f;
                                __instance.GridScanZ.startLifetime = 1f;
                            }
                            __instance.GridScanX.transform.localPosition = new Vector3(__instance.GridScanX.startLifetime * -1f, 0f, 0f);
                            __instance.GridScanY.transform.localPosition = new Vector3(0f, __instance.GridScanY.startLifetime + 0.5f, 0f);
                            __instance.GridScanZ.transform.localPosition = new Vector3(0f, 0f, __instance.GridScanZ.startLifetime * -1f);
                        }
                    }
                }
                if (_currentInvestigationTarget != null && !(bool)ContrabandFound.GetValue(__instance))
                {
                    PLSystemInstance targetSystem = _currentInvestigationTarget.gameObject.GetComponent<PLSystemInstance>();
                    PLPawn player = _currentInvestigationTarget.gameObject.GetComponent<PLPawn>();
                    DroppedItemVisual itemVisual = null;
                    PLReactorInstance reactor = _currentInvestigationTarget.gameObject.GetComponent<PLReactorInstance>();
                    foreach(DroppedItemVisual items in PLGameStatic.Instance.Displayed_DroppedItemVisuals) 
                    {
                        if(items.Visual == _currentInvestigationTarget.gameObject) 
                        {
                            itemVisual = items;
                            break;
                        }
                    }
                    List<PLShipComponent> possibleContraband = new List<PLShipComponent>();
                    float _lastInvestigateActionCompletedTime = (float)lastInvestigateActionCompletedTime.GetValue(__instance);
                    if (targetSystem != null && Time.time - _lastInvestigateActionCompletedTime > 0.33f)
                    {
                        if (targetSystem.MySystem is PLEngineeringSystem)
                        {
                            foreach (PLShipComponent component in __instance.CurrentShip.MyStats.AllComponents)
                            {
                                if (component.Contraband && component.IsEquipped && ((component is PLWarpDrive) || (component is PLThruster) || (component is PLInertiaThruster) || (component is PLManeuverThruster) || (component is PLShieldGenerator)))
                                {
                                    __instance.photonView.RPC("InvestigationFailed", PhotonTargets.All, new object[0]);
                                    break;
                                }
                            }
                        }
                        else if (targetSystem.MySystem is PLComputerSystem)
                        {
                            foreach (PLShipComponent component in __instance.CurrentShip.MyStats.AllComponents)
                            {
                                if (component.Contraband && component.IsEquipped && ((component is PLCPU) || (component is PLWarpDriveProgram) || (component is PLSensor)))
                                {
                                    __instance.photonView.RPC("InvestigationFailed", PhotonTargets.All, new object[0]);
                                    break;
                                }
                            }
                        }
                        else if (targetSystem.MySystem is PLWeaponsSystem)
                        {
                            foreach (PLShipComponent component in __instance.CurrentShip.MyStats.AllComponents)
                            {
                                if (component.Contraband && component.IsEquipped && (component.ActualSlotType == ESlotType.E_COMP_MAINTURRET || component.ActualSlotType == ESlotType.E_COMP_MAINTURRET || component.ActualSlotType == ESlotType.E_COMP_AUTO_TURRET || component.ActualSlotType == ESlotType.E_COMP_TRACKERMISSILE || component.ActualSlotType == ESlotType.E_COMP_NUCLEARDEVICE))
                                {
                                    __instance.photonView.RPC("InvestigationFailed", PhotonTargets.All, new object[0]);
                                    break;
                                }
                            }
                        }
                        _lastInvestigateActionCompletedTime = Time.time;
                        lastInvestigateActionCompletedTime.SetValue(__instance, _lastInvestigateActionCompletedTime);
                    }
                    else if (reactor != null && Time.time - _lastInvestigateActionCompletedTime > 0.33f && __instance.CurrentShip.MyReactor != null && __instance.CurrentShip.MyReactor.Contraband)
                    {
                        __instance.photonView.RPC("InvestigationFailed", PhotonTargets.All, new object[0]);
                    }
                    else if (player != null && Time.time - _lastInvestigateActionCompletedTime > 0.33f && !player.IsDead && player.GetPlayer() != null && player.GetPlayer().MyInventory != null)
                    {
                        using (List<List<PLPawnItem>>.Enumerator enumerator5 = player.GetPlayer().MyInventory.GetAllItems(true).GetEnumerator())
                        {
                            while (enumerator5.MoveNext())
                            {
                                List<PLPawnItem> list = enumerator5.Current;
                                foreach (PLPawnItem plpawnItem in list)
                                {
                                    if (plpawnItem != null && plpawnItem.Contraband)
                                    {
                                        __instance.photonView.RPC("InvestigationFailed", PhotonTargets.All, new object[0]);
                                        break;
                                    }
                                }
                            }
                        }
                        _lastInvestigateActionCompletedTime = Time.time;
                        lastInvestigateActionCompletedTime.SetValue(__instance, _lastInvestigateActionCompletedTime);
                    }
                    else if (_currentInvestigationTarget == __instance.CurrentShip.ResearchLockerCollider.transform && Time.time - _lastInvestigateActionCompletedTime > 0.33f)
                    {
                        using (List<List<PLPawnItem>>.Enumerator enumerator5 = PLServer.Instance.ResearchLockerInventory.GetAllItems(true).GetEnumerator())
                        {
                            while (enumerator5.MoveNext())
                            {
                                List<PLPawnItem> list = enumerator5.Current;
                                foreach (PLPawnItem plpawnItem in list)
                                {
                                    if (plpawnItem != null && plpawnItem.Contraband)
                                    {
                                        __instance.photonView.RPC("InvestigationFailed", PhotonTargets.All, new object[0]);
                                        break;
                                    }
                                }
                            }
                        }
                        _lastInvestigateActionCompletedTime = Time.time;
                        lastInvestigateActionCompletedTime.SetValue(__instance, _lastInvestigateActionCompletedTime);
                    }
                    else if ((__instance.CurrentShip.GetSecretPoster() != null && _currentInvestigationTarget == __instance.CurrentShip.GetSecretPoster().transform) || (__instance.CurrentShip.GetFakeWalls() != null && __instance.CurrentShip.GetFakeWalls().Count > 0 && _currentInvestigationTarget == __instance.CurrentShip.GetFakeWalls()[0].transform))
                    {
                        foundSecret = true;
                    }
                    else if(itemVisual != null) 
                    {
                        PLPawnItem item = PLPawnItem.CreatePawnItemFromHash((int)itemVisual.DroppedItem.ItemHash.GetDecrypted());
                        if(item != null && item.Contraband) 
                        {
                            __instance.photonView.RPC("InvestigationFailed", PhotonTargets.All, new object[0]);
                        }
                    }
                    if (foundSecret)
                    {
                        List<Component> _ComponentsToCasuallyInvestigate = (List<Component>)ComponentsToCasuallyInvestigate.GetValue(__instance);
                        foreach (CargoObjectDisplay cargoObjectDisplay2 in __instance.CurrentShip.GetHiddenCODs())
                        {
                            if (cargoObjectDisplay2 != null && cargoObjectDisplay2.DisplayedItem != null && cargoObjectDisplay2.DisplayObj != null && !_ComponentsToCasuallyInvestigate.Contains(cargoObjectDisplay2.DisplayObj.transform))
                            {
                                _ComponentsToCasuallyInvestigate.Add(cargoObjectDisplay2.DisplayObj.transform);
                            }
                            if (!secretCargo.Contains(cargoObjectDisplay2))
                            {
                                secretCargo.Add(cargoObjectDisplay2);
                            }
                        }
                        ComponentsToCasuallyInvestigate.SetValue(__instance, _ComponentsToCasuallyInvestigate);
                        if (secretCargo.Count > 0)
                        {
                            foreach (CargoObjectDisplay cargoObjectDisplay2 in secretCargo)
                            {
                                if (cargoObjectDisplay2 != null && cargoObjectDisplay2.DisplayedItem != null && cargoObjectDisplay2.DisplayObj != null && cargoObjectDisplay2.DisplayedItem.Contraband && _currentInvestigationTarget.gameObject == cargoObjectDisplay2.DisplayObj.gameObject)
                                {
                                    __instance.photonView.RPC("InvestigationFailed", PhotonTargets.All, new object[0]);
                                    break;
                                }
                            }
                        }
                    }
                    /*
                    if (foundSecret)
                    {
                        
                        PLPathfinderGraphEntity pgeforTLIAndTransform = PLPathfinder.GetInstance().GetPGEforTLIAndTransform(__instance.MyCurrentTLI, __instance.transform);
                        CargoObjectDisplay cargoObjectDisplay = null;
                        float num2 = __instance.currentInvestigationTarget == null ? 9f : Vector3.SqrMagnitude(__instance.currentInvestigationTarget.position - __instance.transform.position);
                        foreach (CargoObjectDisplay cargoObjectDisplay2 in __instance.CurrentShip.GetHiddenCODs())
                        {
                            PulsarModLoader.Utilities.Messaging.Notification("pre 1");
                            PulsarModLoader.Utilities.Messaging.Notification(pgeforTLIAndTransform.Graph.Linecast(cargoObjectDisplay2.DisplayObj.transform.position, __instance.transform.position).ToString());
                            if (cargoObjectDisplay2 != null && cargoObjectDisplay2.DisplayedItem != null && cargoObjectDisplay2.DisplayObj != null && !__instance.investigatedGOs.Contains(cargoObjectDisplay2.DisplayObj.gameObject) && !pgeforTLIAndTransform.Graph.Linecast(cargoObjectDisplay2.DisplayObj.transform.position, __instance.transform.position))
                            {
                                float num6 = Vector3.SqrMagnitude(cargoObjectDisplay2.DisplayObj.transform.position - __instance.transform.position);
                                PulsarModLoader.Utilities.Messaging.Notification("pre 2");
                                PulsarModLoader.Utilities.Messaging.Notification("distance: " + num6);
                                if (num6 < num2)
                                {
                                    num2 = num6;
                                    __instance.currentInvestigationTarget = cargoObjectDisplay2.DisplayObj.transform;
                                    cargoObjectDisplay = cargoObjectDisplay2;
                                }
                            }
                        }
                        if (cargoObjectDisplay != null)
                        {
                            if (Time.time - __instance.lastInvestigateActionCompletedTime > 0.33f && Random.Range(0, 120) == 0 && !__instance.investigatedGOs.Contains(__instance.currentInvestigationTarget.gameObject))
                            {
                                __instance.investigatedGOs.Add(__instance.currentInvestigationTarget.gameObject);
                                __instance.lastInvestigateActionCompletedTime = Time.time;
                                if (cargoObjectDisplay.DisplayedItem != null && cargoObjectDisplay.DisplayedItem.Contraband)
                                {
                                    __instance.photonView.RPC("InvestigationFailed", PhotonTargets.All, new object[0]);
                                }
                            }
                        }
                        
                    }
                    */
                }
            }
        }
    }
}
