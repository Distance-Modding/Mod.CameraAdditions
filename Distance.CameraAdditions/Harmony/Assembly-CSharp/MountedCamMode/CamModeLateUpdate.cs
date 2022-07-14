using HarmonyLib;

namespace Distance.CameraAdditions.Harmony
{
    [HarmonyPatch(typeof(MountedCamMode), "CamModeLateUpdate")]
    internal class MountedCamMode__CamModeLateUpdate
    {
        [HarmonyPostfix]
        internal static void PositionPostfix(MountedCamMode __instance)
        {
            __instance.transform.position += __instance.transform.right * (Mod.Instance.Config.XOffset/10);
            __instance.transform.position += __instance.transform.up * (Mod.Instance.Config.YOffset/10);
            __instance.transform.position += __instance.transform.forward * (Mod.Instance.Config.ZoomOffset/10);
        }
    }
}
