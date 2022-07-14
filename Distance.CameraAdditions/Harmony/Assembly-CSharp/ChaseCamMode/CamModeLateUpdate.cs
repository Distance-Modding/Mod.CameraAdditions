using HarmonyLib;

namespace Distance.CameraAdditions.Harmony
{
    [HarmonyPatch(typeof(ChaseCamMode), "CamModeLateUpdate")]
    internal class ChaseCamMode__CamModeLateUpdate
    {
        [HarmonyPrefix]
        internal static void PositionPrefix(ChaseCamMode __instance)
        {
            float zoomOffset;
            //This is to prevent a super glitchy camera when zooming in too far
            if(Mod.Instance.Config.ZoomOffset < -3f)
                zoomOffset = -3f;
            else
                zoomOffset = Mod.Instance.Config.ZoomOffset;

            __instance.maxDistanceLowSpeed_ = Mod.Instance.maxDistanceLowSpeed + zoomOffset;
            __instance.maxDistanceHighSpeed_ = Mod.Instance.maxDistanceHighSpeed + zoomOffset;
            __instance.minDistance_ = Mod.Instance.minDistance + zoomOffset;
            __instance.height_ = Mod.Instance.height + (zoomOffset / 4.5f);

            if (Mod.Instance.Config.LockCameraPosition)
            {
                __instance.maxDistanceLowSpeed_ = __instance.maxDistanceHighSpeed_;
            }
        }

        [HarmonyPostfix]
        internal static void PositionPostfix(ChaseCamMode __instance)
        {
            __instance.transform.position += __instance.transform.right * Mod.Instance.Config.XOffset;
            __instance.transform.position += __instance.transform.up * Mod.Instance.Config.YOffset;
        }
    }
}
