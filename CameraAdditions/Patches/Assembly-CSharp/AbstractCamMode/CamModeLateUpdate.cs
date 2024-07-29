using HarmonyLib;

namespace CameraAdditions.Patches
{
    [HarmonyPatch(typeof(AbstractCamMode), nameof(AbstractCamMode.CamModeLateUpdate))]
    internal static class AbstractCamMode__CamModeLateUpdate
    {
        [HarmonyPrefix]
        internal static void PositionPrefix(AbstractCamMode __instance)
        {
            if (__instance is ChaseCamMode chaseCam)
            {
                float zoomOffset;
                //This is to prevent a super glitchy camera when zooming in too far
                if (Mod.ZoomOffset.Value < -3f)
                    zoomOffset = -3f;
                else
                    zoomOffset = Mod.ZoomOffset.Value;

                chaseCam.maxDistanceLowSpeed_ = Mod.Instance.maxDistanceLowSpeed + zoomOffset;
                chaseCam.maxDistanceHighSpeed_ = Mod.Instance.maxDistanceHighSpeed + zoomOffset;
                chaseCam.minDistance_ = Mod.Instance.minDistance + zoomOffset;
                chaseCam.height_ = Mod.Instance.height + (zoomOffset / 4.5f);

                if (Mod.LockCameraPosition.Value)
                {
                    chaseCam.maxDistanceLowSpeed_ = chaseCam.maxDistanceHighSpeed_;
                }
            }
        }

        [HarmonyPostfix]
        internal static void PositionPostfix(AbstractCamMode __instance)
        {
            if (__instance is ChaseCamMode)
            {
                __instance.transform.position += __instance.transform.right * Mod.XOffset.Value;
                __instance.transform.position += __instance.transform.up * Mod.YOffset.Value;

                __instance.transform.rotation *= UnityEngine.Quaternion.AngleAxis(Mod.ZRotationOffset.Value, UnityEngine.Vector3.forward);
                __instance.transform.rotation *= UnityEngine.Quaternion.AngleAxis(Mod.XRotationOffset.Value, UnityEngine.Vector3.right);
                __instance.transform.rotation *= UnityEngine.Quaternion.AngleAxis(Mod.YRotationOffset.Value, UnityEngine.Vector3.up);
            }

            if (__instance is MountedCamMode || __instance is CockpitCamMode)
            {
                __instance.transform.position += __instance.transform.right * (Mod.XOffset.Value / 10);
                __instance.transform.position += __instance.transform.up * (Mod.YOffset.Value / 10);
                __instance.transform.position += __instance.transform.forward * (Mod.ZoomOffset.Value / 10);

                __instance.transform.rotation *= UnityEngine.Quaternion.AngleAxis(Mod.ZRotationOffset.Value, UnityEngine.Vector3.forward);
                __instance.transform.rotation *= UnityEngine.Quaternion.AngleAxis(Mod.XRotationOffset.Value, UnityEngine.Vector3.right);
                __instance.transform.rotation *= UnityEngine.Quaternion.AngleAxis(Mod.YRotationOffset.Value, UnityEngine.Vector3.up);
            }
        }
    }
}