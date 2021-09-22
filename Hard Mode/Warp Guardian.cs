using HarmonyLib;

namespace Hard_Mode
{
    class Warp_Guardian //All changes related to the warp guardian battle
    {
        [HarmonyPatch(typeof(PLInfectedSpider_WG), "Start")]
        class GuardianInfectedSpider
        {
            static void Postfix()
            {
                if (PhotonNetwork.isMasterClient)
                {

                }
            }
        }
        [HarmonyPatch(typeof(PLWarpGuardian), "GetPlayerBasedDifficultyMultiplier")]
        class GuardianDifficultyPatch //This removes the multiplier on the guardian to allow more than 2X difficulty
        {
            static float Postfix(float __result) 
            {
                if (PLEncounterManager.Instance.PlayerShip != null)
                {
                    __result = 1f + (PLEncounterManager.Instance.PlayerShip.GetCombatLevel() - 100) * 0.01f;
                    return __result;
                }
                __result = 1f;
                return __result;
            }

        }
    }
}
