using HarmonyLib;
using UnityStandardAssets.ImageEffects;

namespace CameraAdditions.Patches
{
    [HarmonyPatch(typeof(GlobalFog), nameof(GlobalFog.OnPreCull))]
    internal static class GlobalFog__OnPreCull
    {
        [HarmonyPostfix]
        internal static void DrawDistancePostfix(GlobalFog __instance)
        {
            if (G.Sys.EnvironmentManager_ == null || !__instance.CheckResources())
                return;
            EnvironmentManager.Settings bioSettings = G.Sys.EnvironmentManager_.GetBioSettings(__instance.transform.position, true);
            float w = bioSettings.skybox_.common_.skyFadeDistance_;
            if (__instance.affectsDrawDistance_)
            {
                float distance = Mod.DrawDistance.Value;
                if (LevelEditor.IsActive_ && !__instance.camera_.orthographic)
                    distance = 1;

                w = bioSettings.skybox_.common_.skyFadeDistance_ * distance;
                __instance.SetCamFarClip(__instance.camera_, !LevelEditor.IsActive_ || G.Sys.LevelEditor_.ShowFogInEditor_ ? bioSettings.skybox_.common_.skyFar_ * distance * OptimizationManager.cameraFarDistanceMult_ : 50000f);
            }

            UnityEngine.Vector4 fogVector = new UnityEngine.Vector4(bioSettings.fog_.near_, bioSettings.fog_.far_, bioSettings.fog_.maxDistance_, w);
            UnityEngine.Vector4 fogPenumbraVector = new UnityEngine.Vector4(1f / bioSettings.fog_.bottomPenumbra_, 1f / bioSettings.fog_.topPenumbra_, (float)(1.0 / ((double)bioSettings.fog_.far_ - (double)bioSettings.fog_.near_)), 1f / w);

            UnityEngine.Material fogMaterial = __instance.fogMaterial_;
            fogMaterial.SetVector(__instance.shaderIDs_[0], fogVector);
            fogMaterial.SetVector(__instance.shaderIDs_[3], fogPenumbraVector);
            UnityEngine.Material depthMaskMaterial = __instance.depthMaskMaterial_;
            depthMaskMaterial.SetVector(__instance.shaderIDs_[0], fogVector);
            depthMaskMaterial.SetVector(__instance.shaderIDs_[3], fogPenumbraVector);
            UnityEngine.Vector4 farClipVector = new UnityEngine.Vector4(__instance.camera_.nearClipPlane, __instance.camera_.farClipPlane, 0.0f, 0.0f);
            depthMaskMaterial.SetMatrix(__instance.shaderIDs_[9], __instance.camera_.cameraToWorldMatrix);
            depthMaskMaterial.SetVector(__instance.shaderIDs_[11], farClipVector);
            __instance.SetViveOffset(depthMaskMaterial);
        }
    }
}
