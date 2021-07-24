using HarmonyLib;
using UnityEngine;
using Pathfinding;

namespace Hard_Mode
{
    class Fire
    {
        [HarmonyPatch(typeof(PLServer), "CreateFireAtSystem")]
        class GreenFireatSystem //Allow green fire to appear randomly with system damage 
        {
            static void Prefix(ref bool green)
            {
                if (UnityEngine.Random.Range(1, 100) <= 25) // 25% chance 
                {
                    green = true;
                }
            }
        }

    }
}
