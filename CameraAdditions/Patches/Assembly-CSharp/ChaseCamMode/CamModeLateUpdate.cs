using HarmonyLib;

namespace CameraAdditions.Patches
{
    [HarmonyPatch(typeof(ChaseCamMode), "CamModeLateUpdate")]
    internal static class ChaseCamMode__CamModeLateUpdate
    {
        [HarmonyPrefix]
        internal static void PositionPrefix(ChaseCamMode __instance)
        {
            float zoomOffset;
            //This is to prevent a super glitchy camera when zooming in too far
            if (Mod.ZoomOffset.Value < -3f)
                zoomOffset = -3f;
            else
                zoomOffset = Mod.ZoomOffset.Value;

            __instance.maxDistanceLowSpeed_ = Mod.Instance.maxDistanceLowSpeed + zoomOffset;
            __instance.maxDistanceHighSpeed_ = Mod.Instance.maxDistanceHighSpeed + zoomOffset;
            __instance.minDistance_ = Mod.Instance.minDistance + zoomOffset;
            __instance.height_ = Mod.Instance.height + (zoomOffset / 4.5f);

            if (Mod.LockCameraPosition.Value)
            {
                __instance.maxDistanceLowSpeed_ = __instance.maxDistanceHighSpeed_;
            }
        }

        [HarmonyPostfix]
        internal static void PositionPostfix(ChaseCamMode __instance)
        {
            __instance.transform.position += __instance.transform.right * Mod.XOffset.Value;
            __instance.transform.position += __instance.transform.up * Mod.YOffset.Value;

            __instance.transform.rotation *= UnityEngine.Quaternion.AngleAxis(Mod.ZRotationOffset.Value, UnityEngine.Vector3.forward);
            __instance.transform.rotation *= UnityEngine.Quaternion.AngleAxis(Mod.XRotationOffset.Value, UnityEngine.Vector3.right);
            __instance.transform.rotation *= UnityEngine.Quaternion.AngleAxis(Mod.YRotationOffset.Value, UnityEngine.Vector3.up);
        }
    }
}
