using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLProximityMine), "Explode")]
    class Better_Mines
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> Instructions) //Incrises the range from the mines
        {
            List<CodeInstruction> instructionsList = Instructions.ToList();
            instructionsList[49].operand = 480f;
            instructionsList[66].operand = 1000f;
            return instructionsList.AsEnumerable();
        }
    }
}
