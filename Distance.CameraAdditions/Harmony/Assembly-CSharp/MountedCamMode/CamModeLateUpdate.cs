using HarmonyLib;

namespace Distance.CameraAdditions.Harmony
{
    [HarmonyPatch(typeof(MountedCamMode), "CamModeLateUpdate")]
    internal class MountedCamMode__CamModeLateUpdate
    {
        [HarmonyPostfix]
        internal static void PositionPostfix(MountedCamMode __instance)
        {
            __instance.transform.position += new UnityEngine.Vector3(Mod.Instance.Config.XOffset, Mod.Instance.Config.YOffset, 0f);
        }
    }
}
