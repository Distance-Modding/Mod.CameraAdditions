using HarmonyLib;

namespace CameraAdditions.Patches
{
    [HarmonyPatch(typeof(SpectatorFreeCamera), "CamModeInitialize")]
    internal static class SpectatorFreeCamera__CamModeInitialize
    {
        [HarmonyPrefix]
        internal static void AddFreeCameraToPlayModeCycle(SpectatorFreeCamera __instance)
        {
            //This is the easiest thing ever wow
            if (Mod.EnableFreeCam.Value)
            {
                __instance.includeInPlayModeCycle_ = true;
            }
        }
    }
}
