using HarmonyLib;

namespace Distance.CameraAdditions.Harmony
{
    [HarmonyPatch(typeof(SpectatorFreeCamera), "CamModeInitialize")]
    internal class SpectatorFreeCamera__CamModeInitialize
    {
        [HarmonyPrefix]
        static void AddFreeCameraToPlayModeCycle(SpectatorFreeCamera __instance)
        {
            //This is the easiest thing ever wow
            if (Mod.Instance.Config.EnableFreeCam)
            {
                __instance.includeInPlayModeCycle_ = true;
            }
        }
    }
}
