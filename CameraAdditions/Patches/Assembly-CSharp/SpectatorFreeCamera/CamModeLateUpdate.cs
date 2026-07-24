using HarmonyLib;

namespace CameraAdditions.Patches
{
    //Ensure that if the free camera is meant to be locked, then you can't move it

    [HarmonyPatch(typeof(SpectatorFreeCamera), nameof(SpectatorFreeCamera.CamModeLateUpdate))]
    internal static class SpectatorFreeCamera__CamModeLateUpdate
    {
        [HarmonyPrefix]
        internal static bool ControlInput()
        {
            if (Mod.LockFreeCam.Value)
                return false;

            return true;
        }
    }
}
