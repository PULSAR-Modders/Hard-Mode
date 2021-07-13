using HarmonyLib;

namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLReactorInstance), "Update")]
    class Reactor_Radiation
    {
        static void Postfix(PLRadiationPoint ___RadPoint, PLReactorInstance __instance) //Increases radiation from reactor deppending on max temp and current stability
        {
            PLReactor reactor = __instance.MyShipInfo.MyStats.GetShipComponent<PLReactor>(ESlotType.E_COMP_REACTOR, false);
            ___RadPoint.RaditationRange = reactor.TempMax / 75f;
            ___RadPoint.RaditationRange *= 1f + (__instance.MyShipInfo.CoreInstability * 5f);
        }
    }

}
