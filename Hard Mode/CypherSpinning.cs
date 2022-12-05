using HarmonyLib;
using UnityEngine;
namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLSylvassiCypher),"Update")]
    class CypherSpinning
    {
        public static float speed = 100;
        static void Postfix(PLSylvassiCypher __instance) 
        {
            if(Options.MasterHasMod && Options.SpinningCycpher && __instance.CurrentState == PLSylvassiCypher.PuzzleGameState.E_ACTIVE)__instance.CenterCore.transform.localEulerAngles += new Vector3(0, 0, speed) * Time.deltaTime;
        }
    }
}
