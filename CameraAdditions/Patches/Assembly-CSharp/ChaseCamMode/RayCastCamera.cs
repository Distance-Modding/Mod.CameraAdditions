using HarmonyLib;

namespace CameraAdditions.Patches
{
    [HarmonyPatch(typeof(ChaseCamMode), nameof(ChaseCamMode.RayCastCamera))]
    internal static class ChaseCamMod__RayCastCamera
    {
        [HarmonyPostfix]
        internal static void RayCastDisabler(ChaseCamMode __instance, ref UnityEngine.Vector3 __result, UnityEngine.Vector3 rayPos, UnityEngine.Vector3 rayDir, float dist)
        {
            if (!Mod.CameraCollision.Value)
            {
                __result = rayPos + rayDir * dist;
            }
        }
    }
}
