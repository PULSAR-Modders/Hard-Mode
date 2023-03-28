using HarmonyLib;
using System.Reflection;
using UnityEngine;
namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLSylvassiCypher),"Update")]
    class CypherSpinning
    {
        public static float speed = 100;
        private static FieldInfo CenterCore = AccessTools.Field(typeof(PLSylvassiCypher), "CenterCore");
        static void Postfix(PLSylvassiCypher __instance) 
        {
            if (Options.MasterHasMod && Options.SpinningCycpher && __instance.GetCurrentState() == PLSylvassiCypher.PuzzleGameState.E_ACTIVE)
            {
                Renderer _CenterCore = (Renderer)CenterCore.GetValue(__instance);
                _CenterCore.transform.transform.localEulerAngles += new Vector3(0, 0, speed) * Time.deltaTime;
                CenterCore.SetValue(__instance, _CenterCore);
            }
        }
    }
}
