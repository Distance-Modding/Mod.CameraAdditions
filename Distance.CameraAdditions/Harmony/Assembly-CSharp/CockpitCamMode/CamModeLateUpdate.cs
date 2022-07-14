using HarmonyLib;

namespace Distance.CameraAdditions.Harmony
{
    [HarmonyPatch(typeof(CockpitCamMode), "CamModeLateUpdate")]
    internal class CockpitCamMode__CamModeLateUpdate
    {
        [HarmonyPostfix]
        internal static void PositionPostfix(CockpitCamMode __instance)
        {
            __instance.transform.position += new UnityEngine.Vector3(Mod.Instance.Config.XOffset, Mod.Instance.Config.YOffset, 0f);
        }
    }
}