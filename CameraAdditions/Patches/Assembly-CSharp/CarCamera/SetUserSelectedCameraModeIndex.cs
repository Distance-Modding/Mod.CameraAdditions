﻿using HarmonyLib;

namespace CameraAdditions.Patches
{
    [HarmonyPatch(typeof(CarCamera), "SetUserSelectedCameraModeIndex")]
    internal static class CarCamera__SetUserSelectedCameraModeIndex
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
