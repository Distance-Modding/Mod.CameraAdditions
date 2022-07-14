using HarmonyLib;

namespace Distance.CameraAdditions.Harmony
{
    [HarmonyPatch(typeof(SpectatorFreeCamera), "CamModeInitialize")]
    internal static class SpectatorFreeCamera__CamModeInitialize
    {
        [HarmonyPrefix]
        internal static void AddFreeCameraToPlayModeCycle(SpectatorFreeCamera __instance)
        {
            //This is the easiest thing ever wow
            if (Mod.Instance.Config.EnableFreeCam)
            {
                __instance.includeInPlayModeCycle_ = true;
            }
        }
    }
}
