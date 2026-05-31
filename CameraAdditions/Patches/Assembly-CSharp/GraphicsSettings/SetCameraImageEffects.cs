using HarmonyLib;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace CameraAdditions.Patches
{
    [HarmonyPatch(typeof(GraphicsSettings), nameof(GraphicsSettings.SetCameraImageEffects))]
    internal static class GraphicsSettings__SetCameraImageEffects
    {
        [HarmonyPostfix]
        internal static void DrawDistancePostfix(GraphicsSettings __instance, GameObject camObj)
        {
            //Mod.Instance.changedDrawDistance = true;
            //Mod.Log.LogInfo("Setted Camera Image Effects");
            Camera cam = camObj.GetComponent<Camera>();
            GlobalFog gFog = camObj.GetComponent<GlobalFog>();
            CameraCullDistance drawDistanceSetting = (CameraCullDistance)__instance.drawDistance_;
            if (Mod.Instance.isGraphicsMenuOpen)
            {
                //Mod.Log.LogInfo("       Graphics Menu is open, attempting to change modded Draw Distance setting...");
                switch (drawDistanceSetting)
                {
                    case CameraCullDistance.Near:
                        //Mod.Instance.changedDrawDistance = false;
                        Mod.DrawDistance.Value = .67f;
                        break;

                    case CameraCullDistance.Medium:
                        //Mod.Instance.changedDrawDistance = false;
                        Mod.DrawDistance.Value = 1f;
                        break;

                    case CameraCullDistance.Far:
                        //Mod.Instance.changedDrawDistance = false;
                        Mod.DrawDistance.Value = 1.5f;
                        break;
                }
            }

            if (gFog == null)
            {
                float farClip = __instance.carCamera_.GetComponent<CarCamera>().camera_.farClipPlane;

                if (!Mod.Instance.isGraphicsMenuOpen)
                {
                    //Mod.Log.LogInfo("   Graphics Menu is closed, attempting to change vanilla Draw Distance setting...");
                    cam.farClipPlane = farClip * Mod.DrawDistance.Value;
                }

            }
            cam.SetCullDistances();
        }
    }
}
