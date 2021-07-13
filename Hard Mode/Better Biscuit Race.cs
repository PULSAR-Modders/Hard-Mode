using HarmonyLib;

namespace Hard_Mode
{
	[HarmonyPatch(typeof(PLPersistantShipInfo_FBRival), "OnWarp")]
	class Better_Biscuit_Race
    {
		static bool Prefix(PLPersistantShipInfo_FBRival __instance) //This will make the opponents in the race sell more
		{
			if (UnityEngine.Random.Range(0, 100) == 50) // 1 In 100 Chances to sell 500 Biscuits
			{
				__instance.BiscuitsSold += 500;
			}
			if (__instance.IsShipDestroyed || PLServer.Instance == null || PLServer.Instance.BiscuitContestIsOver)
			{
				return false;
			}
			switch (__instance.Type)
			{
				case EShipType.E_FLUFFY_RIVAL1:
					__instance.BiscuitsSold += UnityEngine.Random.Range(100, 400);
					return false;
				case EShipType.E_FLUFFY_RIVAL2:
					__instance.BiscuitsSold += UnityEngine.Random.Range(80, 300);
					return false;
				case EShipType.E_FLUFFY_RIVAL3:
					__instance.BiscuitsSold += UnityEngine.Random.Range(70, 280);
					return false;
				case EShipType.E_FLUFFY_RIVAL4:
					__instance.BiscuitsSold += UnityEngine.Random.Range(60, 260);
					return false;
				case EShipType.E_FLUFFY_RIVAL5:
					__instance.BiscuitsSold += UnityEngine.Random.Range(50, 240);
					return false;
				case EShipType.E_FLUFFY_RIVAL6:
					__instance.BiscuitsSold += UnityEngine.Random.Range(40, 220);
					return false;
				case EShipType.E_FLUFFY_RIVAL_GENERIC:
					__instance.BiscuitsSold += UnityEngine.Random.Range(30, 200);
					return false;
				default:
					return false;
			}
		}
	}
}
