using HarmonyLib;

namespace Distance.CameraAdditions.Harmony
{
    [HarmonyPatch(typeof(CockpitCamMode), "CamModeLateUpdate")]
    internal static class CockpitCamMode__CamModeLateUpdate
    {
        [HarmonyPostfix]
        internal static void PositionPostfix(CockpitCamMode __instance)
        {
            __instance.transform.position += __instance.transform.right * (Mod.Instance.Config.XOffset/10);
            __instance.transform.position += __instance.transform.up * (Mod.Instance.Config.YOffset/10);
            __instance.transform.position += __instance.transform.forward * (Mod.Instance.Config.ZoomOffset/10);
        }
    }
}