using HarmonyLib;

namespace CameraAdditions.Patches
{
    //Ensure that the car doesn't take input during free cam unless specified otherwise

    [HarmonyPatch(typeof(SpectatorFreeCamera), nameof(SpectatorFreeCamera.CamModeOnGainFocus))]
    internal static class SpectatorFreeCamera__CamModeOnGainFocus
    {
        [HarmonyPostfix]
        internal static void ControlLock(SpectatorFreeCamera __instance)
        {
            Mod.Instance.isFreeCamActive = true;

            if (!Mod.LockFreeCam.Value)
                __instance.parent_.PlayerDataOwner_.DisableCarInput();
        }
    }
}
