﻿using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Input;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Distance.CameraAdditions
{
	[ModEntryPoint("com.github.tribow/CameraAdditions")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance;

		public IManager Manager { get; set; }

		public Log Logger { get; set; }

		public ConfigLogic Config { get; private set; }

		//Chase Cam related variables
		public float MaxDistanceLowSpeed { get; private set; }
		public float MaxDistanceHighSpeed { get; private set; }
		public float MinDistance { get; private set; }
		public float Height { get; private set; }
		public float YOffset { get; }
		public float XOffset { get; }

        private bool DefaultWasFired;
        private bool RotationWasFired;

        public void Initialize(IManager manager)
		{
			//Dont remove the object when entering new scenes
			DontDestroyOnLoad(this);

			//Assign public variables
			Instance = this;
			Manager = manager;
			Logger = LogManager.GetForCurrentAssembly();
			Config = gameObject.AddComponent<ConfigLogic>();
            MaxDistanceLowSpeed = 4.5f;
			MaxDistanceHighSpeed = 3.25f;
			MinDistance = 4f;
			Height = 1.3f;

        //Subscribe to config event
        Config.OnChanged += OnConfigChanged;

			try
			{
				CreateSettingsMenu();
			}
			catch (Exception e)
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
                new ActionButton(MenuDisplayMode.Both, "setting:defaults", "RESTORE DEFAULTS")
                .WhenClicked(() => SetDefaults())
                .WithDescription("Restores camera values back to default"),

                new IntegerSlider(MenuDisplayMode.Both, "setting:fov_offset", "FOV OFFSET")
                .LimitedByRange(-50, 50)
                .WithDefaultValue(0)
                .WithGetter(() => Config.FOVOffset)
                .WithSetter((x) => Config.FOVOffset = x)
                .WithDescription("Adjust the offset of the FOV (Affects all cameras)"),

                new IntegerSlider(MenuDisplayMode.Both, "setting:fov_value", "LOCKED FOV VALUE")
                .LimitedByRange(0, 180)
                .WithDefaultValue(125)
                .WithGetter(() => Config.FOV)
                .WithSetter((x) => Config.FOV = x)
                .WithDescription("Adjust the FOV of Chase Cam mode (Affective only when LOCK FOV is toggled on)"),

                new CheckBox(MenuDisplayMode.Both, "setting:lock_fov", "LOCK FOV")
                .WithGetter(() => Config.LockFOV)
                .WithSetter((x) => Config.LockFOV = x)
                .WithDescription("Lock the FOV of Chase Cam mode"),

                new FloatSlider(MenuDisplayMode.Both, "setting:zoom_offset", "ZOOM OFFSET")
                .LimitedByRange(-10f, 10f)
                .WithDefaultValue(0f)
                .WithGetter(() => Config.ZoomOffset)
                .WithSetter((x) => Config.ZoomOffset = x)
                .WithDescription("Adjust zoom offset of the Chase Cam mode (Acts as the Z offset for Cockpit and Mounted Camera)"),

                new FloatSlider(MenuDisplayMode.Both, "setting:x_offset", "X OFFSET")
                .LimitedByRange(-10f, 10f)
                .WithDefaultValue(0f)
                .WithGetter(() => Config.XOffset)
                .WithSetter((x) => Config.XOffset = x)
                .WithDescription("Adjust the X axis offset of all Car Camera Modes"),

                new FloatSlider(MenuDisplayMode.Both, "setting:y_offset", "Y OFFSET")
                .LimitedByRange(-10f, 10f)
                .WithDefaultValue(0f)
                .WithGetter(() => Config.YOffset)
                .WithSetter((x) => Config.YOffset = x)
                .WithDescription("Adjust the Y axis offset of all Car Camera Modes"),

                new CheckBox(MenuDisplayMode.Both, "setting:lock_camera", "LOCK CAMERA POSITION")
                .WithGetter(() => Config.LockCameraPosition)
                .WithSetter((x) => Config.LockCameraPosition = x)
                .WithDescription("Lock the camera's position when in Chase Cam mode"),

                new CheckBox(MenuDisplayMode.Both, "setting:free_cam", "ENABLE FREE CAM IN PLAY MODE")
                .WithGetter(() => Config.EnableFreeCam)
                .WithSetter((x) => Config.EnableFreeCam = x)
                .WithDescription("Make Free Cam an available camera while playing (Will not apply until outside gameplay)"),

                new InputPrompt(MenuDisplayMode.Both, "setting:set_defaults_hotkey", "SET DEFAULTS HOTKEY")
                .WithDefaultValue(() => Config.DefaultsHotkey)
                .WithSubmitAction((x) => Config.DefaultsHotkey = x)
                .WithTitle("DEFAULTS HOTKEY")
                .WithDescription("Set the hotkey for restoring default camera values"),

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
                .WithDescription("Set the hotkey for zooming out"),

                new InputPrompt(MenuDisplayMode.Both, "setting:increase_x_hotkey", "SET INCREASE X HOTKEY")
                .WithDefaultValue(() => Config.IncreaseXOffsetHotkey)
                .WithSubmitAction((x) => Config.IncreaseXOffsetHotkey = x)
                .WithTitle("INCREASE X OFFSET HOTKEY")
                .WithDescription("Set the hotkey for increasing the X offset"),

                new InputPrompt(MenuDisplayMode.Both, "setting:decrease_x_hotkey", "SET DECREASE X HOTKEY")
                .WithDefaultValue(() => Config.DecreaseXOffsetHotkey)
                .WithSubmitAction((x) => Config.DecreaseXOffsetHotkey = x)
                .WithTitle("DECREASE X OFFSET HOTKEY")
                .WithDescription("Set the hotkey for decreasing the X offset"),

                new InputPrompt(MenuDisplayMode.Both, "setting:increase_y_hotkey", "SET INCREASE Y HOTKEY")
                .WithDefaultValue(() => Config.IncreaseYOffsetHotkey)
                .WithSubmitAction((x) => Config.IncreaseYOffsetHotkey = x)
                .WithTitle("INCREASE Y OFFSET HOTKEY")
                .WithDescription("Set the hotkey for increasing the Y offset"),

                new InputPrompt(MenuDisplayMode.Both, "setting:decrease_y_hotkey", "SET DECREASE Y HOTKEY")
                .WithDefaultValue(() => Config.DecreaseYOffsetHotkey)
                .WithSubmitAction((x) => Config.DecreaseYOffsetHotkey = x)
                .WithTitle("DECREASE Y OFFSET HOTKEY")
                .WithDescription("Set the hotkey for decreasing the Y offset"),

                new CheckBox(MenuDisplayMode.Both, "setting:enable_rotation_toggle", "ROTATION TOGGLE")
                .WithGetter(() => Config.EnableRotation)
                .WithSetter((x) => Config.EnableRotation = x)
                .WithDescription("Toggle whether or not every X,Y,Z hotkey/slider affects the rotation or the position."),

                new InputPrompt(MenuDisplayMode.Both, "setting:enable_rotation_toggle_hotkey", "SET ROTATION TOGGLE HOTKEY")
                .WithDefaultValue(() => Config.EnableRotationHotkey)
                .WithSubmitAction((x) => Config.EnableRotationHotkey = x)
                .WithTitle("ROTATION TOGGLE HOTKEY")
                .WithDescription("Set the hotkey for the rotation toggle setting"),

                new FloatSlider(MenuDisplayMode.Both, "setting:x_rotation_offset", "X ROTATION OFFSET")
                .LimitedByRange(-90f, 90f)
                .WithDefaultValue(0f)
                .WithGetter(() => Config.XRotationOffset)
                .WithSetter((x) => Config.XRotationOffset = x)
                .WithDescription("Adjust X rotational offset"),

                new FloatSlider(MenuDisplayMode.Both, "setting:y_rotation_offset", "Y ROTATION OFFSET")
                .LimitedByRange(-90f, 90f)
                .WithDefaultValue(0f)
                .WithGetter(() => Config.YRotationOffset)
                .WithSetter((x) => Config.YRotationOffset = x)
                .WithDescription("Adjust Y rotational offset"),

                new FloatSlider(MenuDisplayMode.Both, "setting:z_rotation_offset", "Z ROTATION OFFSET")
                .LimitedByRange(-90f, 90f)
                .WithDefaultValue(0f)
                .WithGetter(() => Config.ZRotationOffset)
                .WithSetter((x) => Config.ZRotationOffset = x)
                .WithDescription("Adjust Z rotational offset")
            };
            
			Menus.AddNew(MenuDisplayMode.Both, settingsMenu, "CAMERA ADDITIONS", "Settings for the camera additions mod.");
		}

        #region Key Bindings

        public void OnConfigChanged(ConfigLogic config)
		{

        }

        public void Update()
        {
            if (Input.anyKey)
            {
                try
                {
                    if (Config.IncreaseFOVHotkey.Contains("+"))
                    {
                        string[] array = Parse(Config.IncreaseFOVHotkey);

                        if (KeyComboIsPressed(array))
                            ++Config.FOVOffset;
                    }
                    else
                    {
                        if (Input.GetKey(Config.IncreaseFOVHotkey))
                            ++Config.FOVOffset;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.ToString());
                    Logger.Error("Increase FOV key either does not exist or is formatted incorrectly!");
                }

                try
                {
                    if (Config.DecreaseFOVHotkey.Contains("+"))
                    {
                        string[] array = Parse(Config.DecreaseFOVHotkey);

                        if (KeyComboIsPressed(array))
                            --Config.FOVOffset;
                    }
                    else
                    {
                        if (Input.GetKey(Config.DecreaseFOVHotkey))
                            --Config.FOVOffset;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.ToString());
                    Logger.Error("Decrease FOV key either does not exist or is formatted incorrectly!");
                }

                try
                {
                    if (Config.ZoomInHotkey.Contains("+"))
                    {
                        string[] array = Parse(Config.ZoomInHotkey);

                        if (KeyComboIsPressed(array))
                        {
                            if (!Config.EnableRotation)
                                Config.ZoomOffset += 0.25f;
                            else
                                Config.ZRotationOffset += .5f;
                        }
                    }
                    else
                    {
                        if (Input.GetKey(Config.ZoomInHotkey))
                        {
                            if (!Config.EnableRotation)
                                Config.ZoomOffset += 0.25f;
                            else
                                Config.ZRotationOffset += .5f;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.ToString());
                    Logger.Error("Zoom in / Positive Z Rotation key either does not exist or is formatted incorrectly!");
                }

                try
                {
                    if (Config.ZoomOutHotkey.Contains("+"))
                    {
                        string[] array = Parse(Config.ZoomOutHotkey);

                        if (KeyComboIsPressed(array))
                        {
                            if (!Config.EnableRotation)
                                Config.ZoomOffset -= 0.25f;
                            else
                                Config.ZRotationOffset -= .5f;
                        }
                    }
                    else
                    {
                        if (Input.GetKey(Config.ZoomOutHotkey))
                        {
                            if (!Config.EnableRotation)
                                Config.ZoomOffset -= 0.25f;
                            else
                                Config.ZRotationOffset -= .5f;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.ToString());
                    Logger.Error("Zoom out / Negative Z Rotation key either does not exist or is formatted incorrectly!");
                }

                try
                {
                    if (Config.DefaultsHotkey.Contains("+"))
                    {
                        string[] array = Parse(Config.DefaultsHotkey);

                        if (KeyComboIsPressed(array) && !DefaultWasFired)
                        {
                            SetDefaults();
                            DefaultWasFired = true;
                        }
                        else if (!KeyComboIsPressed(array))
                        {
                            DefaultWasFired = false;
                        }
                    }
                    else
                    {
                        if (Input.GetKey(Config.DefaultsHotkey) && !DefaultWasFired)
                        {
                            SetDefaults();
                            DefaultWasFired = true;
                        }
                        else if (!Input.GetKey(Config.DefaultsHotkey))
                        {
                            DefaultWasFired = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.ToString());
                    Logger.Error("Camera defaults key either does not exist or is formatted incorrectly!");
                }

                try
                {
                    if (Config.IncreaseXOffsetHotkey.Contains("+"))
                    {
                        string[] array = Parse(Config.IncreaseXOffsetHotkey);

                        if (KeyComboIsPressed(array))
                        {
                            if (!Config.EnableRotation)
                                Config.XOffset += 0.25f;
                            else
                                Config.XRotationOffset += .5f;
                        }
                    }
                    else
                    {
                        if (Input.GetKey(Config.IncreaseXOffsetHotkey))
                        {
                            if (!Config.EnableRotation)
                                Config.XOffset += 0.25f;
                            else
                                Config.XRotationOffset += .5f;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.ToString());
                    Logger.Error("Increase X offset / Positive X rotation key either does not exist or is formatted incorrectly!");
                }

                try
                {
                    if (Config.DecreaseXOffsetHotkey.Contains("+"))
                    {
                        string[] array = Parse(Config.DecreaseXOffsetHotkey);

                        if (KeyComboIsPressed(array))
                        {
                            if (!Config.EnableRotation)
                                Config.XOffset -= 0.25f;
                            else
                                Config.XRotationOffset -= .5f;
                        }
                    }
                    else
                    {
                        if (Input.GetKey(Config.DecreaseXOffsetHotkey))
                        {
                            if (!Config.EnableRotation)
                                Config.XOffset -= 0.25f;
                            else
                                Config.XRotationOffset -= .5f;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.ToString());
                    Logger.Error("Decrease X offset / Negative X rotation key either does not exist or is formatted incorrectly!");
                }

                try
                {
                    if (Config.IncreaseYOffsetHotkey.Contains("+"))
                    {
                        string[] array = Parse(Config.IncreaseYOffsetHotkey);

                        if (KeyComboIsPressed(array))
                        {
                            if (!Config.EnableRotation)
                                Config.YOffset += 0.25f;
                            else
                                Config.YRotationOffset += .5f;
                        }
                    }
                    else
                    {
                        if (Input.GetKey(Config.IncreaseYOffsetHotkey))
                        {
                            if (!Config.EnableRotation)
                                Config.YOffset += 0.25f;
                            else
                                Config.YRotationOffset += .5f;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.ToString());
                    Logger.Error("Increase Y offset / Positive Y rotation key either does not exist or is formatted incorrectly!");
                }

                try
                {
                    if (Config.DecreaseYOffsetHotkey.Contains("+"))
                    {
                        string[] array = Parse(Config.DecreaseYOffsetHotkey);

                        if (KeyComboIsPressed(array))
                        {
                            if (!Config.EnableRotation)
                                Config.YOffset -= 0.25f;
                            else
                                Config.YRotationOffset -= .5f;
                        }
                    }
                    else
                    {
                        if (Input.GetKey(Config.DecreaseYOffsetHotkey))
                        {
                            if (!Config.EnableRotation)
                                Config.YOffset -= 0.25f;
                            else
                                Config.YRotationOffset -= .5f;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.ToString());
                    Logger.Error("Decrease Y offset / Negative Y rotation key either does not exist or is formatted incorrectly!");
                }

                try
                {
                    if (Config.EnableRotationHotkey.Contains("+"))
                    {
                        string[] array = Parse(Config.EnableRotationHotkey);

                        if (KeyComboIsPressed(array) && !RotationWasFired)
                        {
                            Config.EnableRotation = !Config.EnableRotation;
                            RotationWasFired = true;
                        }
                        else if (!KeyComboIsPressed(array))
                        {
                            RotationWasFired = false;
                        }
                    }
                    else
                    {
                        if (Input.GetKey(Config.EnableRotationHotkey) && !RotationWasFired)
                        {
                            Config.EnableRotation = !Config.EnableRotation;
                            RotationWasFired = true;
                        }
                        else if (!Input.GetKey(Config.EnableRotationHotkey))
                        {
                            RotationWasFired = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.ToString());
                    Logger.Error("Enable rotation key either does not exist or is formatted incorrectly!");
                }
            }
        }

        private string[] Parse(string hotkey)
        {
            string[] array = { };

            string modifiedHotkeyString = hotkey;
            if (Regex.IsMatch(modifiedHotkeyString, @"^[A-z +]+"))
            {
                if (modifiedHotkeyString.EndsWith("+"))
                {
                    modifiedHotkeyString.Substring(0, modifiedHotkeyString.Length - 1);
                }
                
                array = hotkey.Split('+');
            }

            string[] noDuplicates = array.Distinct().ToArray();
            return noDuplicates;
        }

        private bool KeyComboIsPressed(string[] keys)
        {
            bool pressed = false;
            foreach (string s in keys)
            {
                if (Input.GetKey(s))
                {
                    pressed = true;
                    continue;
                }
                pressed = false;
                break;
            }

            return pressed;
        }
		#endregion

		//Function should be used before any Chase Camera becomes active
		public void SetChaseCamValues(bool isNear)
		{
			//Hardcoded values to make sure Zoom behaves correctly
			//May have been a better way to do this
			if (isNear)
			{
				MaxDistanceLowSpeed = 4.5f;
				MaxDistanceHighSpeed = 3.25f;
				MinDistance = 4f;
				Height = 1.3f;
			}
			else
			{
				MaxDistanceLowSpeed = 5.625f;
				MaxDistanceHighSpeed = 4.0625f;
				MinDistance = 4f;
				Height = 1.55f;
			}
		}

		//Set camera values back to default
		private void SetDefaults()
		{
			Config.FOVOffset = 0;
			Config.ZoomOffset = 0f;
			Config.XOffset = 0f;
			Config.YOffset = 0f;
            Config.ZRotationOffset = 0f;
            Config.XRotationOffset = 0f;
            Config.YRotationOffset = 0f;
            Config.LockCameraPosition = false;
            Config.LockFOV = false;
            Config.FOV = 125;
		}
	}
}
