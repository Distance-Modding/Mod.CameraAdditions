using HarmonyLib;

namespace CameraAdditions.Patches
{
    [HarmonyPatch(typeof(CockpitCamMode), "CamModeLateUpdate")]
    internal static class CockpitCamMode__CamModeLateUpdate
    {
        [HarmonyPostfix]
        internal static void PositionPostfix(CockpitCamMode __instance)
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
