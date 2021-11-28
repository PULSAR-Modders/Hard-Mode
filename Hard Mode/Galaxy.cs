using HarmonyLib;
using UnityEngine;
namespace Hard_Mode
{
    internal class Galaxy
    {
        [HarmonyPatch(typeof(PLWare), "GetScaledMarketPrice")]
        class MarketPrice 
        {
            static void Postfix(PLWare __instance, ref int __result, bool inIsSellPrice) 
            {
                if (Options.MasterHasMod)
                {
                    if (!inIsSellPrice)
                    {
                        __result = Mathf.RoundToInt(__result * 1.35f);
                    }
                }
            }
        }
    }
}
