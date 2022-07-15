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
            set { Set("FOV", value); }
        }

        public int FOVOffset
        {
            get { return Get<int>("FOVOffset"); }
            set { Set("FOVOffset", value); }
        }

        public float ZoomOffset
        {
            get { return Get<float>("ZoomOffset"); }
            set { Set("ZoomOffset", value); }
        }

        public float XOffset
        {
            get { return Get<float>("XOffset"); }
            set { Set("XOffset", value); }
        }

        public float YOffset
        {
            get { return Get<float>("YOffset"); }
            set { Set("YOffset", value); }
        }

        public bool LockFOV
        {
            get { return Get<bool>("LockFOV"); }
            set { Set("LockFOV", value); }
        }

        public bool LockCameraPosition
        {
            get { return Get<bool>("LockCameraPosition"); }
            set { Set("LockCameraPosition", value); }
        }

        public bool EnableFreeCam
        {
            get { return Get<bool>("EnableFreeCam"); }
            set { Set("EnableFreeCam", value); }
        }

        public string IncreaseFOVHotkey
        {
            get { return Get<string>("IncreaseFOVHotkey"); }
            set { Set("IncreaseFOVHotkey", value); }
        }

        public string DecreaseFOVHotkey
        {
            get { return Get<string>("DecreaseFOVHotkey"); }
            set { Set("DecreaseFOVHotkey", value); }
        }

        public string ZoomOutHotkey
        {
            get { return Get<string>("ZoomOutHotkey"); }
            set { Set("ZoomOutHotkey", value); }
        }

        public string ZoomInHotkey
        {
            get { return Get<string>("ZoomInHotkey"); }
            set { Set("ZoomInHotkey", value); }
        }

        public string DefaultsHotkey
        {
            get { return Get<string>("DefaultsHotkey"); }
            set { Set("DefaultsHotkey", value); }
        }

        public string IncreaseXOffsetHotkey
        {
            get { return Get<string>("IncreaseXOffsetHotkey"); }
            set { Set("IncreaseXOffsetHotkey", value); }
        }

        public string DecreaseXOffsetHotkey
        {
            get { return Get<string>("DecreaseXOffsetHotkey"); }
            set { Set("DecreaseXOffsetHotkey", value); }
        }

        public string IncreaseYOffsetHotkey
        {
            get { return Get<string>("IncreaseYOffsetHotkey"); }
            set { Set("IncreaseYOffsetHotkey", value); }
        }

        public string DecreaseYOffsetHotkey
        {
            get { return Get<string>("DecreaseYOffsetHotkey"); }
            set { Set("DecreaseYOffsetHotkey", value); }
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
            Get("FOVOffset", 0);
            Get("ZoomOffset", 0f);
            Set("ZoomOffset", 0f); //Zoom Offset will not remember previous settings
            Get("XOffset", 0f);
            Set("XOffset", 0f); //XOffset will not remember previous settings
            Get("YOffset", 0f);
            Set("YOffset", 0f); //YOffset will not remember previous settings
            Get("IncreaseFOVHotkey", "LeftControl+P");
            Get("DecreaseFOVHotkey", "LeftControl+O");
            Get("ZoomOutHotkey", "LeftControl+N");
            Get("ZoomInHotkey", "LeftControl+M");
            Get("DefaultsHotkey", "LeftControl+L");
            Get("IncreaseXOffsetHotkey", "LeftAlt+L");
            Get("DecreaseXOffsetHotkey", "LeftAlt+J");
            Get("IncreaseYOffsetHotkey", "LeftAlt+I");
            Get("DecreaseYOffsetHotkey", "LeftAlt+K");
            //Save settings to Config.json
            Save();
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

        public void Save()
        {
            Config?.Save();
            OnChanged?.Invoke(this);
        }
    }
}
