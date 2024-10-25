using HarmonyLib;
using UnityEngine;

namespace Hard_Mode
{
	[HarmonyPatch(typeof(PLPersistantShipInfo_FBRival), "OnWarp")]
	class Better_Biscuit_Race
    {
		static void Postfix(PLPersistantShipInfo_FBRival __instance) //This will make the opponents in the race sell more
		{
			if (Random.Range(0, 20) == 5) // 1 In 20 (5%) Chances to sell 500 Biscuits per jump
			{
				__instance.BiscuitsSold += 500;
			}
			if (__instance.IsShipDestroyed || PLServer.Instance == null || PLServer.Instance.BiscuitContestIsOver)
			{
				return;
			}
			__instance.BiscuitsSold += Random.Range(0, 100); // All ships will sell from 0 to 100 extra bicuits per jump
		}
	}

	[HarmonyPatch(typeof(PLServer), "ClientSoldBiscuits")]
	class Decrease_Sell_Odds // Makes so the biscuits you sell decrease more the odds of selling more biscuits
	{
		static void Postfix(int biscuitsSold) 
		{
            if (Options.MasterHasMod && PLServer.GetCurrentSector() != null)
            {
                PLServer.GetCurrentSector().BiscuitsSoldCounter += Mathf.RoundToInt(0.55f * (float)biscuitsSold);
            }
        }
	}
}
