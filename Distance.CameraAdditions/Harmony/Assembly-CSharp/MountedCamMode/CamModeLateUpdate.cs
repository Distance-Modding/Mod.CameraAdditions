using HarmonyLib;

namespace Distance.CameraAdditions.Harmony
{
    [HarmonyPatch(typeof(MountedCamMode), "CamModeLateUpdate")]
    internal static class MountedCamMode__CamModeLateUpdate
    {
        [HarmonyPostfix]
        internal static void PositionPostfix(MountedCamMode __instance)
        {
            __instance.transform.position += __instance.transform.right * (Mod.Instance.Config.XOffset/10);
            __instance.transform.position += __instance.transform.up * (Mod.Instance.Config.YOffset/10);
            __instance.transform.position += __instance.transform.forward * (Mod.Instance.Config.ZoomOffset/10);

            __instance.transform.rotation *= UnityEngine.Quaternion.AngleAxis(Mod.Instance.Config.ZRotationOffset, UnityEngine.Vector3.forward);
            __instance.transform.rotation *= UnityEngine.Quaternion.AngleAxis(Mod.Instance.Config.XRotationOffset, UnityEngine.Vector3.right);
            __instance.transform.rotation *= UnityEngine.Quaternion.AngleAxis(Mod.Instance.Config.YRotationOffset, UnityEngine.Vector3.up);
        }
    }
}
