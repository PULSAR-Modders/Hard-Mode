using HarmonyLib;

namespace Hard_Mode
{
	[HarmonyPatch(typeof(PLPersistantShipInfo_FBRival), "OnWarp")]
	class Better_Biscuit_Race
    {
		static bool Prefix(PLPersistantShipInfo_FBRival __instance) //This will make the opponents in the race sell more
		{
			if (UnityEngine.Random.Range(0, 10) == 5) // 1 In 10 Chances to sell 500 Biscuits
			{
				__instance.BiscuitsSold += 500;
			}
			if (__instance.IsShipDestroyed || PLServer.Instance == null || PLServer.Instance.BiscuitContestIsOver)
			{
				return false;
			}
			__instance.BiscuitsSold += UnityEngine.Random.Range(0, 500); // All ships will sell from 0 to 500 per jump
			return false;
		}
	}
}
