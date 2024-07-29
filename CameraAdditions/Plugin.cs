using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;

namespace CameraAdditions
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public sealed class Mod : BaseUnityPlugin
    {
        //Mod Details
        private const string modGUID = "Distance.CameraAdditions";
        private const string modName = "Camera Additions";
        private const string modVersion = "1.2.6";

        //Config Entry Strings
        public static string DecreaseFOVShortcutKey = "The Decrease FOV Shortcut";
        public static string DecreaseXShortcutKey = "The Decrease X Offset Shortcut";
        public static string DecreaseYShortcutKey = "The Decrease Y Offset Shortcut";
        public static string DecreaseZShortcutKey = "The Decrease Z or Zoom Offset Shortcut";
        public static string DefaultShortcutKey = "The Shortcut that sets all values to their Defaults";
        public static string EnableFreeCamKey = "Enable Free Cam";
        public static string EnableRotationShortcutKey = "The Shortcut that toggles whether the offset shortcuts affect rotation";
        public static string FOVKey = "The Locked FOV Value";
        public static string FOVOffsetKey = "The current FOV Offset";
        public static string IncreaseFOVShortcutKey = "The Increase FOV Shortcut";
        public static string IncreaseXShortcutKey = "The Increase X Offset Shortcut";
        public static string IncreaseYShortcutKey = "The Increase Y Offset Shortcut";
        public static string IncreaseZShortcutKey = "The Increase Z or Zoom Offset Shortcut";
        public static string LockPositionKey = "Lock the Camera's Position";
        public static string LockFOVKey = "Lock the Camera's FOV";
        public static string XOffsetKey = "The Offset of the Position on the X axis";
        public static string XRotationOffsetKey = "The Offset of the Rotation on the X axis";
        public static string YOffsetKey = "The Offset of the Position on the Y axis";
        public static string YRotationOffsetKey = "The Offset of the Rotation on the Y axis";
        public static string ZoomOffsetKey = "The Offset of the Camera's Zoom";
        public static string ZRotationOffsetKey = "The Offset of the Rotation on the Z axis";

        //Config Entries
        public static ConfigEntry<bool> EnableFreeCam { get; set; }
        public static ConfigEntry<bool> LockCameraPosition { get; set; }
        public static ConfigEntry<bool> LockFOV { get; set; }
        public static ConfigEntry<float> XOffset { get; set; }
        public static ConfigEntry<float> XRotationOffset { get; set; }
        public static ConfigEntry<float> YOffset { get; set; }
        public static ConfigEntry<float> YRotationOffset { get; set; }
        public static ConfigEntry<float> ZoomOffset { get; set; }
        public static ConfigEntry<float> ZRotationOffset { get; set; }
        public static ConfigEntry<int> FOV { get; set; }
        public static ConfigEntry<int> FOVOffset { get; set; }
        public static ConfigEntry<KeyboardShortcut> DecreaseFOVShortcut { get; set; }
        public static ConfigEntry<KeyboardShortcut> DecreaseXOffsetShortcut { get; set; }
        public static ConfigEntry<KeyboardShortcut> DecreaseYOffsetShortcut { get; set; }
        public static ConfigEntry<KeyboardShortcut> DecreaseZOffsetShortcut { get; set; }
        public static ConfigEntry<KeyboardShortcut> DefaultsShortcut { get; set; }
        public static ConfigEntry<KeyboardShortcut> EnableRotationShortcut { get; set; }
        public static ConfigEntry<KeyboardShortcut> IncreaseFOVShortcut { get; set; }
        public static ConfigEntry<KeyboardShortcut> IncreaseXOffsetShortcut { get; set; }
        public static ConfigEntry<KeyboardShortcut> IncreaseYOffsetShortcut { get; set; }
        public static ConfigEntry<KeyboardShortcut> IncreaseZOffsetShortcut { get; set; }

        //Public Variables
        public float maxDistanceLowSpeed { get; private set; }
        public float maxDistanceHighSpeed { get; private set; }
        public float minDistance { get; private set; }
        public float height { get; private set; }
        public float yOffset { get; }
        public float xOffset { get; }

        //Private Variables
        private bool defaultWasFired;
        private bool rotationWasFired;
        private bool shortcutsAffectRotation;

        //Other
        private static readonly Harmony harmony = new Harmony(modGUID);
        public static ManualLogSource Log = new ManualLogSource(modName);
        public static Mod Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Log = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            Logger.LogInfo("Thanks for using Camera Additions!");

            //Assign Values
            maxDistanceLowSpeed = 4.5f;
            maxDistanceHighSpeed = 3.25f;
            minDistance = 4f;
            height = 1.3f;

            //Config Setup
            EnableFreeCam = Config.Bind<bool>("General",
                EnableFreeCamKey,
                false,
                new ConfigDescription("Make Free Cam an available camera while playing (Will not apply until outside gameplay)"));

            LockCameraPosition = Config.Bind<bool>("General",
                LockPositionKey,
                false,
                new ConfigDescription("Lock the camera's position when in Chase Cam mode"));

            FOVOffset = Config.Bind<int>("General",
                FOVOffsetKey,
                0,
                new ConfigDescription("Adjust the offset of the FOV (Affects all cameras)",
                    new AcceptableValueRange<int>(-50, 50)));

            LockFOV = Config.Bind<bool>("General",
                LockFOVKey,
                false,
                new ConfigDescription("Lock the FOV of Chase Cam mode"));

            FOV = Config.Bind<int>("General",
                FOVKey,
                125,
                new ConfigDescription("Adjust the FOV of Chase Cam mode (Affective only when LOCK FOV is toggled on)",
                    new AcceptableValueRange<int>(0, 180)));

            XOffset = Config.Bind<float>("Position Offsets",
                XOffsetKey,
                0f,
                new ConfigDescription("Adjust the X axis offset of all Car Camera Modes",
                    new AcceptableValueRange<float>(-20f, 20f)));

            YOffset = Config.Bind<float>("Position Offsets",
                YOffsetKey,
                0f,
                new ConfigDescription("Adjust the Y axis offset of all Car Camera Modes",
                    new AcceptableValueRange<float>(-20f, 20f)));

            ZoomOffset = Config.Bind<float>("Position Offsets",
                ZoomOffsetKey,
                0f,
                new ConfigDescription("Adjust the Z axis offset of all Car Camera Modes (For the Chase Camera it affects the Zoom, not the Z axis)",
                    new AcceptableValueRange<float>(-20f, 20f)));

            XRotationOffset = Config.Bind<float>("Rotation Offsets",
                XRotationOffsetKey,
                0f,
                new ConfigDescription("Adjust the X rotational offset of all Car Camera Modes",
                    new AcceptableValueRange<float>(-90f, 90f)));

            YRotationOffset = Config.Bind<float>("Rotation Offsets",
                YRotationOffsetKey,
                0f,
                new ConfigDescription("Adjust the Y rotational offset of all Car Camera Modes",
                    new AcceptableValueRange<float>(-90f, 90f)));

            ZRotationOffset = Config.Bind<float>("Rotation Offsets",
                ZRotationOffsetKey,
                0f,
                new ConfigDescription("Adjust the Z rotational offset of all Car Camera Modes",
                    new AcceptableValueRange<float>(-90f, 90f)));

            DefaultsShortcut = Config.Bind<KeyboardShortcut>("Shortcuts",
                DefaultShortcutKey,
                new KeyboardShortcut(UnityEngine.KeyCode.L, new UnityEngine.KeyCode[] { UnityEngine.KeyCode.LeftControl }),
                new ConfigDescription("Set the shortcut for setting all values to default"));

            EnableRotationShortcut = Config.Bind<KeyboardShortcut>("Shortcuts",
                EnableRotationShortcutKey,
                new KeyboardShortcut(UnityEngine.KeyCode.U, new UnityEngine.KeyCode[] { UnityEngine.KeyCode.LeftControl }),
                new ConfigDescription("Set the shortcut toggling if the offset shortcuts affect rotation"));

            IncreaseFOVShortcut = Config.Bind<KeyboardShortcut>("Shortcuts",
                IncreaseFOVShortcutKey,
                new KeyboardShortcut(UnityEngine.KeyCode.P, new UnityEngine.KeyCode[] { UnityEngine.KeyCode.LeftControl }),
                new ConfigDescription("Set the shortcut for increasing the FOV value"));

            DecreaseFOVShortcut = Config.Bind<KeyboardShortcut>("Shortcuts",
                DecreaseFOVShortcutKey,
                new KeyboardShortcut(UnityEngine.KeyCode.O, new UnityEngine.KeyCode[] { UnityEngine.KeyCode.LeftControl }),
                new ConfigDescription("Set the shortcut for decreasing the FOV value"));

            IncreaseXOffsetShortcut = Config.Bind<KeyboardShortcut>("Shortcuts",
                IncreaseXShortcutKey,
                new KeyboardShortcut(UnityEngine.KeyCode.L, new UnityEngine.KeyCode[] { UnityEngine.KeyCode.LeftAlt }),
                new ConfigDescription("Set the shortcut increasing the X Offset"));

            DecreaseXOffsetShortcut = Config.Bind<KeyboardShortcut>("Shortcuts",
                DecreaseXShortcutKey,
                new KeyboardShortcut(UnityEngine.KeyCode.J, new UnityEngine.KeyCode[] { UnityEngine.KeyCode.LeftAlt }),
                new ConfigDescription("Set the shortcut decreasing the X Offset"));

            IncreaseYOffsetShortcut = Config.Bind<KeyboardShortcut>("Shortcuts",
                IncreaseYShortcutKey,
                new KeyboardShortcut(UnityEngine.KeyCode.I, new UnityEngine.KeyCode[] { UnityEngine.KeyCode.LeftAlt }),
                new ConfigDescription("Set the shortcut increasing the Y Offset"));

            DecreaseYOffsetShortcut = Config.Bind<KeyboardShortcut>("Shortcuts",
                DecreaseYShortcutKey,
                new KeyboardShortcut(UnityEngine.KeyCode.K, new UnityEngine.KeyCode[] { UnityEngine.KeyCode.LeftAlt }),
                new ConfigDescription("Set the shortcut decreasing the Y Offset"));

            IncreaseZOffsetShortcut = Config.Bind<KeyboardShortcut>("Shortcuts",
                IncreaseZShortcutKey,
                new KeyboardShortcut(UnityEngine.KeyCode.N, new UnityEngine.KeyCode[] { UnityEngine.KeyCode.LeftControl }),
                new ConfigDescription("Set the shortcut increasing the Z Offset"));

            DecreaseZOffsetShortcut = Config.Bind<KeyboardShortcut>("Shortcuts",
                DecreaseZShortcutKey,
                new KeyboardShortcut(UnityEngine.KeyCode.M, new UnityEngine.KeyCode[] { UnityEngine.KeyCode.LeftControl }),
                new ConfigDescription("Set the shortcut decreasing the Z Offset"));


            DecreaseFOVShortcut.SettingChanged += OnConfigChanged;
            DecreaseXOffsetShortcut.SettingChanged += OnConfigChanged;
            DecreaseYOffsetShortcut.SettingChanged += OnConfigChanged;
            DecreaseZOffsetShortcut.SettingChanged += OnConfigChanged;
            DefaultsShortcut.SettingChanged += OnConfigChanged;
            EnableFreeCam.SettingChanged += OnConfigChanged;
            EnableRotationShortcut.SettingChanged += OnConfigChanged;
            FOV.SettingChanged += OnConfigChanged;
            FOVOffset.SettingChanged += OnConfigChanged;
            IncreaseFOVShortcut.SettingChanged += OnConfigChanged;
            IncreaseXOffsetShortcut.SettingChanged += OnConfigChanged;
            IncreaseYOffsetShortcut.SettingChanged += OnConfigChanged;
            IncreaseZOffsetShortcut.SettingChanged += OnConfigChanged;
            LockCameraPosition.SettingChanged += OnConfigChanged;
            LockFOV.SettingChanged += OnConfigChanged;
            XOffset.SettingChanged += OnConfigChanged;
            YOffset.SettingChanged += OnConfigChanged;
            ZoomOffset.SettingChanged += OnConfigChanged;

            //Apply Patches
            Logger.LogInfo("Loading...");
            harmony.PatchAll();
            Logger.LogInfo("Loaded!");
        }

        void Update()
        {
            if (UnityEngine.Input.anyKey)
            {
                if (DefaultsShortcut.Value.IsDown())
                    SetDefaults();

                if (EnableRotationShortcut.Value.IsDown())
                    shortcutsAffectRotation = !shortcutsAffectRotation;

                if (IncreaseFOVShortcut.Value.IsDown())
                    FOVOffset.Value++;

                if (DecreaseFOVShortcut.Value.IsDown())
                    FOVOffset.Value--;

                if (IncreaseXOffsetShortcut.Value.IsDown())
                {
                    if (!shortcutsAffectRotation)
                        XOffset.Value += 0.25f;
                    else
                        XRotationOffset.Value += .5f;
                }

                if (DecreaseXOffsetShortcut.Value.IsDown())
                {
                    if (!shortcutsAffectRotation)
                        XOffset.Value -= 0.25f;
                    else
                        XRotationOffset.Value -= .5f;
                }

                if (IncreaseYOffsetShortcut.Value.IsDown())
                {
                    if (!shortcutsAffectRotation)
                        YOffset.Value += 0.25f;
                    else
                        YRotationOffset.Value += .5f;
                }

                if (DecreaseYOffsetShortcut.Value.IsDown())
                {
                    if (!shortcutsAffectRotation)
                        YOffset.Value -= 0.25f;
                    else
                        YRotationOffset.Value -= .5f;
                }

                if (IncreaseZOffsetShortcut.Value.IsDown())
                {
                    if (!shortcutsAffectRotation)
                        ZoomOffset.Value += 0.25f;
                    else
                        ZRotationOffset.Value += .5f;
                }

                if (DecreaseZOffsetShortcut.Value.IsDown())
                {
                    if (!shortcutsAffectRotation)
                        ZoomOffset.Value -= 0.25f;
                    else
                        ZRotationOffset.Value -= .5f;
                }



            }
        }

        private void OnConfigChanged(object sender, EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            if (settingChangedEventArgs == null) return;
        }

        //Function should be used before any Chase Camera becomes active
        public void SetChaseCamValues(bool isNear)
        {
            //Hardcoded values to make sure Zoom behaves correctly
            //May have been a better way to do this
            if (isNear)
            {
                maxDistanceLowSpeed = 4.5f;
                maxDistanceHighSpeed = 3.25f;
                minDistance = 4f;
                height = 1.3f;
            }
            else
            {
                maxDistanceLowSpeed = 5.625f;
                maxDistanceHighSpeed = 4.0625f;
                minDistance = 4f;
                height = 1.55f;
            }
        }

        //Set camera values back to default
        private void SetDefaults()
        {
            FOV.Value = 125;
            FOVOffset.Value = 0;
            LockCameraPosition.Value = false;
            LockFOV.Value = false;
            XOffset.Value = 0f;
            XRotationOffset.Value = 0f;
            YOffset.Value = 0f;
            YRotationOffset.Value = 0f;
            ZoomOffset.Value = 0f;
            ZRotationOffset.Value = 0f;
        }
    }
}
