﻿using HarmonyLib;

namespace Distance.CameraAdditions.Harmony
{
    [HarmonyPatch(typeof(CarCamera), "SetCameraFOV")]
    internal static class CarCamera__SetCameraFOV
    {
        [HarmonyPostfix]
        internal static void FOVPostfix(CarCamera __instance)
        {
            if (__instance.activeCameraMode_ is ChaseCamMode && Mod.Instance.Config.LockFOV)
            {
                __instance.fov_ = Mod.Instance.Config.FOV;
                __instance.camera_.fieldOfView = Mod.Instance.Config.FOV;
            }

            __instance.fov_ += Mod.Instance.Config.FOVOffset;
            __instance.camera_.fieldOfView += Mod.Instance.Config.FOVOffset;
        }
    }
}
