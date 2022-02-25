using HarmonyLib;

namespace Distance.CameraAdditions.Harmony
{
    [HarmonyPatch(typeof(ChaseCamMode), "CamModeLateUpdate")]
    internal class ChaseCamMode__CamModeLateUpdate
    {
        [HarmonyPrefix]
        internal static void PositionPrefix(ChaseCamMode __instance)
        {
            __instance.maxDistanceLowSpeed_ = Mod.Instance.maxDistanceLowSpeed + Mod.Instance.Config.ZoomOffset;
            __instance.maxDistanceHighSpeed_ = Mod.Instance.maxDistanceHighSpeed + Mod.Instance.Config.ZoomOffset;
            __instance.minDistance_ = Mod.Instance.minDistance + Mod.Instance.Config.ZoomOffset;
            __instance.height_ = Mod.Instance.height + (Mod.Instance.Config.ZoomOffset / 4.5f); 

            if (Mod.Instance.Config.LockCameraPosition)
            {
                __instance.maxDistanceLowSpeed_ = __instance.maxDistanceHighSpeed_;
            }
        }
    }
}
