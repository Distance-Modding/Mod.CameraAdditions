using System;
using Reactor.API.Attributes;
using Reactor.API.Input;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using UnityEngine;

namespace Distance.CameraAdditions
{
    [ModEntryPoint("TribowCameraAdditions")]
    public class Mod : MonoBehaviour
    {
        public static Mod Instance;

        public IManager Manager { get; set; }

        public Log Logger { get; set; }

        public ConfigLogic Config { get; private set; }

        //Chase Cam related variables
        public float maxDistanceLowSpeed { get; private set; }
        public float maxDistanceHighSpeed { get; private set; }
        public float minDistance { get; private set; }
        public float height { get; private set; }

        public void Initialize(IManager manager)
        {
            //Dont remove the object when entering new scenes
            DontDestroyOnLoad(this);

            //Assign public variables
            Instance = this;
            Manager = manager;
            Logger = LogManager.GetForCurrentAssembly();
            Config = gameObject.AddComponent<ConfigLogic>();
            maxDistanceLowSpeed = 4.5f;
            maxDistanceHighSpeed = 3.25f;
            minDistance = 4f;
            height = 1.3f;


            //Subscribe to config event
            Config.OnChanged += OnConfigChanged;

            try
            {
                CreateSettingsMenu();
            }
            catch(Exception e)
            {
                Logger.Info(e);
                Logger.Info("This likely happened because you have the wrong version of Centrifuge.Distance. \nTo fix this, be sure to use the Centrifuge.Distance.dll file that came included with the mod's zip file. \nDespite this error, the mod will still function, however, you will not have access to the mod's menu.");
            }

            //Run the patches
            RuntimePatcher.AutoPatch();

            Logger.Info("Thanks for using CameraAdditions!");
        }

        private void CreateSettingsMenu()
        {
            MenuTree settingsMenu = new MenuTree("menu.mod.cameradd", "Camera Additions Settings")
            {
                new IntegerSlider(MenuDisplayMode.Both, "setting:fov_offset", "FOV OFFSET")
                .LimitedByRange(-50, 50)
                .WithDefaultValue(0)
                .WithGetter(() => Config.FOVOffset)
                .WithSetter(((x) => Config.FOVOffset = x))
                .WithDescription("Adjust the offset of the FOV (Affects all cameras)"),

                new IntegerSlider(MenuDisplayMode.Both, "setting:fov_value", "LOCKED FOV VALUE")
                .LimitedByRange(0, 180)
                .WithDefaultValue(125)
                .WithGetter(() => Config.FOV)
                .WithSetter(((x) => Config.FOV = x))
                .WithDescription("Adjust the FOV of Chase Cam mode (Affective only when LOCK FOV is toggled on)"),

                new CheckBox(MenuDisplayMode.Both, "setting:lock_fov", "LOCK FOV")
                .WithGetter(() => Config.LockFOV)
                .WithSetter(((x) => Config.LockFOV = x))
                .WithDescription("Lock the FOV of Chase Cam mode"),

                new FloatSlider(MenuDisplayMode.Both, "setting:zoom_offset", "ZOOM OFFSET")
                .LimitedByRange(-3f, 20f)
                .WithDefaultValue(0f)
                .WithGetter(() => Config.ZoomOffset)
                .WithSetter(((x) => Config.ZoomOffset = x))
                .WithDescription("Adjust zoom offset of the Chase Cam mode"),

                new CheckBox(MenuDisplayMode.Both, "setting:lock_camera", "LOCK CAMERA POSITION")
                .WithGetter(() => Config.LockCameraPosition)
                .WithSetter(((x) => Config.LockCameraPosition = x))
                .WithDescription("Lock the camera's position when in Chase Cam mode"),

                new CheckBox(MenuDisplayMode.Both, "setting:free_cam", "ENABLE FREE CAM IN PLAY MODE")
                .WithGetter(() => Config.EnableFreeCam)
                .WithSetter(((x) => Config.EnableFreeCam = x))
                .WithDescription("Make Free Cam an available camera while playing (Will not apply until outside gameplay)"),

                new InputPrompt(MenuDisplayMode.Both, "setting:increase_fov_hotkey", "SET INCREASE FOV HOTKEY")
                .WithDefaultValue(() => Config.IncreaseFOVHotkey)
                .WithSubmitAction((x) => Config.IncreaseFOVHotkey = x)
                .WithTitle("INCREASE FOV HOTKEY")
                .WithDescription("Set the hotkey for increasing the FOV offset"),

                new InputPrompt(MenuDisplayMode.Both, "setting:decrease_fov_hotkey", "SET DECREASE FOV HOTKEY")
                .WithDefaultValue(() => Config.DecreaseFOVHotkey)
                .WithSubmitAction((x) => Config.DecreaseFOVHotkey = x)
                .WithTitle("DECREASE FOV HOTKEY")
                .WithDescription("Set the hotkey for decreasing the FOV offset"),

                new InputPrompt(MenuDisplayMode.Both, "setting:zoom_in_hotkey", "SET ZOOM IN HOTKEY")
                .WithDefaultValue(() => Config.ZoomInHotkey)
                .WithSubmitAction((x) => Config.ZoomInHotkey = x)
                .WithTitle("ZOOM IN FOV HOTKEY")
                .WithDescription("Set the hotkey for zooming in"),

                new InputPrompt(MenuDisplayMode.Both, "setting:zoom_out_hotkey", "SET ZOOM OUT HOTKEY")
                .WithDefaultValue(() => Config.ZoomOutHotkey)
                .WithSubmitAction((x) => Config.ZoomOutHotkey = x)
                .WithTitle("ZOOM OUT FOV HOTKEY")
                .WithDescription("Set the hotkey for zooming out")
            };

            Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "CAMERA ADDITIONS", "Settings for the camera additions mod.");
        }

        #region Key Bindings
        private Hotkey _keybindIncreaseFOVOffset = null;
        private Hotkey _keybindDecreaseFOVOffset = null;
        private Hotkey _keybindZoomIn = null;
        private Hotkey _keybindZoomOut = null;

        public void OnConfigChanged(ConfigLogic config)
        {
            BindAction(ref _keybindIncreaseFOVOffset, config.IncreaseFOVHotkey, () =>
            {
                Config.FOVOffset += 1;
            });

            BindAction(ref _keybindDecreaseFOVOffset, config.DecreaseFOVHotkey, () =>
            {
                Config.FOVOffset -= 1;
            });

            BindAction(ref _keybindZoomIn, config.ZoomInHotkey, () =>
            {
                Config.ZoomOffset += 0.5f;
            });

            BindAction(ref _keybindZoomOut, config.ZoomOutHotkey, () =>
            {
                //Prevent the super glitchy camera when zooming in too much
                if (Config.ZoomOffset > -3f)
                {
                    Config.ZoomOffset -= 0.5f;
                }
            });
        }

        public void BindAction(ref Hotkey unbind, string rebind, Action callback)
        {
            if (unbind != null)
            {
                Manager.Hotkeys.UnbindHotkey(unbind);
            }

            unbind = Manager.Hotkeys.BindHotkey(rebind, callback, true);
        }
        #endregion

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
    }
}
