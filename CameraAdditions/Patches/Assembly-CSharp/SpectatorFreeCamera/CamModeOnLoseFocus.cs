using HarmonyLib;

namespace CameraAdditions.Patches
{
    // Ensure that the car can move again once free cam turns off

    [HarmonyPatch(typeof(SpectatorFreeCamera), nameof(SpectatorFreeCamera.CamModeOnLoseFocus))]
    internal static class SpectatorFreeCamera__CamModeOnLoseFocus
    {
        [HarmonyPostfix]
        internal static void ControlLock(SpectatorFreeCamera __instance)
        {
            if (!__instance.parent_.PlayerDataOwner_.carInputEnabled_ && !G.Sys.GameManager_.PauseMenuOpen_)
                __instance.parent_.PlayerDataOwner_.EnableCarInput();

            Mod.Instance.isFreeCamActive = false;
        }
    }
}
