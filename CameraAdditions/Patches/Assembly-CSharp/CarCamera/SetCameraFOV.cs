using HarmonyLib;

namespace CameraAdditions.Patches
{
    [HarmonyPatch(typeof(CarCamera), "SetCameraFOV")]
    internal static class CarCamera__SetCameraFOV
    {
        [HarmonyPostfix]
        internal static void FOVPostfix(CarCamera __instance)
        {
            if (__instance.activeCameraMode_ is ChaseCamMode && Mod.LockFOV.Value)
            {
                __instance.fov_ = Mod.FOV.Value;
                __instance.camera_.fieldOfView = Mod.FOV.Value;
            }

            __instance.fov_ += Mod.FOVOffset.Value;
            __instance.camera_.fieldOfView += Mod.FOVOffset.Value;
        }
    }
}
