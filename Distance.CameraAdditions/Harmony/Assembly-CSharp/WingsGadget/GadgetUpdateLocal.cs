using System.Collections.Generic;
using HarmonyLib;
using System.Reflection.Emit;

namespace Distance.CameraAdditions.Harmony
{
    /*[HarmonyPatch(typeof(WingsGadget), "GadgetUpdateLocal")]
    internal class WingsGadget__GadgetUpdateLocal
    {
        //This does not stop the car from stabilizing, it just stops it from doing landing assist
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> DisableWingStabilization(IEnumerable<CodeInstruction> instr)
        {
            foreach (var codeInstruction in instr)
            {
                if (codeInstruction.opcode == OpCodes.Ldc_R4 && (float)codeInstruction.operand == 0.25)
                {
                    yield return new CodeInstruction(OpCodes.Ldc_R4, 0.0);
                }
                else
                {
                    yield return codeInstruction;
                }
            }
        }
    }*/
}
