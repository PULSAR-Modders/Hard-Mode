using HarmonyLib;

namespace Hard_Mode
{
    class Starting_Components //Keep all the changes to any ship initial setup here
    {
        [HarmonyPatch(typeof(PLFluffyShipInfo),"SetupShipStats")]
        class FluffyOne 
        {
            static void Postfix(PLFluffyShipInfo __instance, bool previewStats) 
            {
                /*
                if (__instance.ShouldCreateDefaultComponents && (PhotonNetwork.isMasterClient || previewStats) && PLServer.Instance != null)
                {
                    __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetShipComponent<PLReactor>(ESlotType.E_COMP_REACTOR));
                    __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(3, 4, 1, 0, 12), null), -1, ESlotType.E_COMP_REACTOR);
                }
                */
            }
        }
        [HarmonyPatch(typeof(PLAlienTentacleCreatureInfo), "SetupShipStats")]
        class Vuroog
        {
            static void Postfix(PLAlienTentacleCreatureInfo __instance, bool previewStats)
            {
                if (__instance.ShouldCreateDefaultComponents && (PhotonNetwork.isMasterClient || previewStats) && PLServer.Instance != null)
                {
                    __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetShipComponent<PLReactor>(ESlotType.E_COMP_HULL));
                    __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo(6, 1, 500, 11, 12), null), -1, ESlotType.E_COMP_HULL);
                }
            }
        }
    }
}
