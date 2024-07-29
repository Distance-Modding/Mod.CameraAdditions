using HarmonyLib;

namespace CameraAdditions.Patches
{
    /*[HarmonyPatch(typeof(CarCamera), "BalanceFlyingCamera")]
    internal class CarCamera__BalanceFlyingCamera
    {
        //Ok so the goal was to try to find a way to get the camera to not rotate with the car AND get the car to not automatically orient
        //itself to the camera when flying. Unfortunately, I've got no idea how to do that right now and I'm just gonna leave it at that
        //kthx bai
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> DestabilizeCamera(IEnumerable<CodeInstruction> instructions)
        {
            bool foundTarget = false;
            int startIndex = -1, endIndex = -1;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

            //Loop through the code instructions and find where the brtrue starts and ends
            //It is looking for brtrue because the if statement that is being changed has an "||" in it
            //Code technically unsafe, if the game EVER changes that if statement it might break lol
            for(int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Brtrue)
                {
                    //This will only be set if the code found what it was looking for
                    if(foundTarget)
                    {
                        endIndex = i;
                    }
                    else
                    {
                        startIndex = i + 1; //Don't remove the first brtrue
                        foundTarget = true;
                    }
                }
            }

            //Removes the instructions for stabilizing camera 
            if (startIndex > -1 && endIndex > -1)
            {
                codes.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
            }

            //Return the new instruction stack
            //Mod.Instance.Logger.Info("Yes This is Happening");
            return codes.AsEnumerable();
        }
    }*/
}
