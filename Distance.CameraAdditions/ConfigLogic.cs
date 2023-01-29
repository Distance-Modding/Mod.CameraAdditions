using System;
using Reactor.API.Configuration;
using UnityEngine;

namespace Distance.CameraAdditions
{
    public class ConfigLogic : MonoBehaviour
    {
        #region Properties
        public int FOV
        {
            get { return Get<int>("FOV"); }
            set { Set("FOV", value, false); }
        }

        public int FOVOffset
        {
            get { return Get<int>("FOVOffset"); }
            set { Set("FOVOffset", value, false); }
        }

        public float ZoomOffset
        {
            get { return Get<float>("ZoomOffset"); }
            set { Set("ZoomOffset", value, false); }
        }

        public float XOffset
        {
            get { return Get<float>("XOffset"); }
            set { Set("XOffset", value, false); }
        }

        public float YOffset
        {
            get { return Get<float>("YOffset"); }
            set { Set("YOffset", value, false); }
        }

        public float XRotationOffset
        {
            get { return Get<float>("XRotationOffset"); }
            set { Set("XRotationOffset", value, false); }
        }

        public float YRotationOffset
        {
            get { return Get<float>("YRotationOffset"); }
            set { Set("YRotationOffset", value, false); }
        }

        public float ZRotationOffset
        {
            get { return Get<float>("ZRotationOffset"); }
            set { Set("ZRotationOffset", value, false); }
        }

        public bool LockFOV
        {
            get { return Get<bool>("LockFOV"); }
            set { Set("LockFOV", value, false); }
        }

        public bool LockCameraPosition
        {
            get { return Get<bool>("LockCameraPosition"); }
            set { Set("LockCameraPosition", value, false); }
        }

        public bool EnableFreeCam
        {
            get { return Get<bool>("EnableFreeCam"); }
            set { Set("EnableFreeCam", value, false); }
        }

        public bool EnableRotation
        {
            get { return Get<bool>("EnableRotation"); }
            set { Set("EnableRotation", value, false); }
        }

        public string IncreaseFOVHotkey
        {
            get { return Get<string>("IncreaseFOVHotkey"); }
            set { Set("IncreaseFOVHotkey", value, true); }
        }

        public string DecreaseFOVHotkey
        {
            get { return Get<string>("DecreaseFOVHotkey"); }
            set { Set("DecreaseFOVHotkey", value, true); }
        }

        public string ZoomOutHotkey
        {
            get { return Get<string>("ZoomOutHotkey"); }
            set { Set("ZoomOutHotkey", value, true); }
        }

        public string ZoomInHotkey
        {
            get { return Get<string>("ZoomInHotkey"); }
            set { Set("ZoomInHotkey", value, true); }
        }

        public string DefaultsHotkey
        {
            get { return Get<string>("DefaultsHotkey"); }
            set { Set("DefaultsHotkey", value, true); }
        }

        public string IncreaseXOffsetHotkey
        {
            get { return Get<string>("IncreaseXOffsetHotkey"); }
            set { Set("IncreaseXOffsetHotkey", value, true); }
        }

        public string DecreaseXOffsetHotkey
        {
            get { return Get<string>("DecreaseXOffsetHotkey"); }
            set { Set("DecreaseXOffsetHotkey", value, true); }
        }

        public string IncreaseYOffsetHotkey
        {
            get { return Get<string>("IncreaseYOffsetHotkey"); }
            set { Set("IncreaseYOffsetHotkey", value, true); }
        }

        public string DecreaseYOffsetHotkey
        {
            get { return Get<string>("DecreaseYOffsetHotkey"); }
            set { Set("DecreaseYOffsetHotkey", value, true); }
        }

        public string EnableRotationHotkey
        {
            get { return Get<string>("EnableRotationHotkey"); }
            set { Set("EnableRotationHotkey", value, true); }
        }
        #endregion

        internal Settings Config;

        public event Action<ConfigLogic> OnChanged;

        //Initialize Config
        private void Load()
        {
            Config = new Settings("Config");
        }

        public void Awake()
        {
            Load();
            //Setting Defaults
            Get("FOV", 125);
            Set("ZoomOffset", 0f); //Zoom Offset will not remember previous settings
            Set("XOffset", 0f); //XOffset will not remember previous settings
            Set("YOffset", 0f); //YOffset will not remember previous settings
            Set("ZRotationOffset", 0f); //same
            Set("XRotationOffset", 0f); //same
            Set("YRotationOffset", 0f); //same
            Get("IncreaseFOVHotkey", "left ctrl+p");
            Get("DecreaseFOVHotkey", "left ctrl+o");
            Get("ZoomOutHotkey", "left ctrl+n");
            Get("ZoomInHotkey", "left ctrl+m");
            Get("DefaultsHotkey", "left ctrl+l");
            Get("IncreaseXOffsetHotkey", "left alt+l");
            Get("DecreaseXOffsetHotkey", "left alt+j");
            Get("IncreaseYOffsetHotkey", "left alt+i");
            Get("DecreaseYOffsetHotkey", "left alt+k");
            Get("EnableRotationHotkey", "left ctrl+u");
            //Save settings to Config.json
            Save();
            Mod.Instance.OnConfigChanged(this);
        }

        public T Get<T>(string key, T @default = default(T))
        {
            return Config.GetOrCreate(key, @default);
        }

        public void Set<T>(string key, T value)
        {
            Config[key] = value;
            Save();
        }

        public void Set<T>(string key, T value, bool invoke)
        {
            Config[key] = value;
            Config?.Save();
            if (invoke)
            {
                OnChanged?.Invoke(this);
            }
        }

        public void Save()
        {
            Config?.Save();
            OnChanged?.Invoke(this);
        }
    }
}
