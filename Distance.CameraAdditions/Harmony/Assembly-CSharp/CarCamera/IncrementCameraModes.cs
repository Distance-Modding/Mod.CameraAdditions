using HarmonyLib;

namespace Distance.CameraAdditions.Harmony
{
    [HarmonyPatch(typeof(CarCamera), "IncrementCameraModes")]
    internal static class CarCamera__IncrementCameraModes
    {
        [HarmonyPostfix]
        internal static void SetChaseCamValues(CarCamera __instance)
        {
            if (__instance.userSelectedCameraModeIndex_ == 5)
            {
                Mod.Instance.SetChaseCamValues(true);
            }
            else if (__instance.userSelectedCameraModeIndex_ == 6)
            {
                Mod.Instance.SetChaseCamValues(false);
            }
        }
    }
}
