using HarmonyLib;

namespace CameraAdditions.Patches
{
    //Not making two scripts for tiny patches like this
    [HarmonyPatch(typeof(GraphicsMenu), nameof(GraphicsMenu.Initialize))]
    internal static class GraphicsMenu__Initialize
    {
        [HarmonyPostfix]
        internal static void InitializeCheck(GraphicsMenu __instance)
        {
            Mod.Instance.isGraphicsMenuOpen = __instance.isInitialized_;
            //Mod.Log.LogInfo("Graphics Menu Open: " + Mod.Instance.isGraphicsMenuOpen);
        }
    }

    [HarmonyPatch(typeof(GraphicsMenu), nameof(GraphicsMenu.CleanUp))]
    internal static class GraphicsMenu__CleanUp
    {
        [HarmonyPostfix]
        internal static void InitializeCheck(GraphicsMenu __instance)
        {
            Mod.Instance.isGraphicsMenuOpen = __instance.isInitialized_;
            //Mod.Log.LogInfo("Graphics Menu Open: " + Mod.Instance.isGraphicsMenuOpen);
        }
    }
}
