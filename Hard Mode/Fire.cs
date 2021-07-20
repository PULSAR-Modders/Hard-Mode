using HarmonyLib;

namespace Hard_Mode
{
    class Fire
    {
        [HarmonyPatch(typeof(PLServer), "CreateFireAtSystem")]
        class GreenFireatSystem //Allow green fire to appear randomly with system damage 
        { 
            static void Prefix(ref bool green) 
            { 
                if(UnityEngine.Random.Range(1,100) <= 5) // 5% chance 
                {
                    green = true;
                }
            }
        }
    }
}
