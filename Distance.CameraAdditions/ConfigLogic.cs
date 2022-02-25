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
            Get("IncreaseFOVHotkey", "LeftControl+P");
            Get("DecreaseFOVHotkey", "LeftControl+O");
            Get("ZoomOutHotkey", "LeftControl+N");
            Get("ZoomInHotkey", "LeftControl+M");
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
