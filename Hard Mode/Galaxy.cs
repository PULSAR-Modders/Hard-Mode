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
                    if (inIsSellPrice)
                    {
                        __result = Mathf.RoundToInt((float)__result * 0.5f);
                    }
                    else
                    {
                        __result *= 2;
                    }
                }
            }
        }
    }
}
