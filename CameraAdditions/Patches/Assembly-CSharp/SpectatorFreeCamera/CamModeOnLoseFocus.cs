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
            PlayerDataLocal player = __instance.parent_.PlayerDataOwner_; 

            if (player != null)
                if (!player.carInputEnabled_ && !G.Sys.GameManager_.PauseMenuOpen_)
                    player.EnableCarInput();

            Mod.Instance.isFreeCamActive = false;
        }
    }
}
