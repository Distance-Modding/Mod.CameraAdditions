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

        public float XRotationOffset
        {
            get { return Get<float>("XRotationOffset"); }
            set { Set("XRotationOffset", value); }
        }

        public float YRotationOffset
        {
            get { return Get<float>("YRotationOffset"); }
            set { Set("YRotationOffset", value); }
        }

        public float ZRotationOffset
        {
            get { return Get<float>("ZRotationOffset"); }
            set { Set("ZRotationOffset", value); }
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

        public bool EnableRotation
        {
            get { return Get<bool>("EnableRotation"); }
            set { Set("EnableRotation", value, false); }
        }

        public string IncreaseFOVHotkey
        {
            get { return Get<string>("IncreaseFOVHotkey"); }
            set { Set("IncreaseFOVHotkey", value, false); }
        }

        public string DecreaseFOVHotkey
        {
            get { return Get<string>("DecreaseFOVHotkey"); }
            set { Set("DecreaseFOVHotkey", value, false); }
        }

        public string ZoomOutHotkey
        {
            get { return Get<string>("ZoomOutHotkey"); }
            set { Set("ZoomOutHotkey", value, false); }
        }

        public string ZoomInHotkey
        {
            get { return Get<string>("ZoomInHotkey"); }
            set { Set("ZoomInHotkey", value, false); }
        }

        public string DefaultsHotkey
        {
            get { return Get<string>("DefaultsHotkey"); }
            set { Set("DefaultsHotkey", value, false); }
        }

        public string IncreaseXOffsetHotkey
        {
            get { return Get<string>("IncreaseXOffsetHotkey"); }
            set { Set("IncreaseXOffsetHotkey", value, false); }
        }

        public string DecreaseXOffsetHotkey
        {
            get { return Get<string>("DecreaseXOffsetHotkey"); }
            set { Set("DecreaseXOffsetHotkey", value, false); }
        }

        public string IncreaseYOffsetHotkey
        {
            get { return Get<string>("IncreaseYOffsetHotkey"); }
            set { Set("IncreaseYOffsetHotkey", value, false); }
        }

        public string DecreaseYOffsetHotkey
        {
            get { return Get<string>("DecreaseYOffsetHotkey"); }
            set { Set("DecreaseYOffsetHotkey", value, false); }
        }

        public string EnableRotationHotkey
        {
            get { return Get<string>("EnableRotationHotkey"); }
            set { Set("EnableRotationHotkey", value, false); }
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
            Get("IncreaseFOVHotkey", "LeftControl+P");
            Get("DecreaseFOVHotkey", "LeftControl+O");
            Get("ZoomOutHotkey", "LeftControl+N");
            Get("ZoomInHotkey", "LeftControl+M");
            Get("DefaultsHotkey", "LeftControl+L");
            Get("IncreaseXOffsetHotkey", "LeftAlt+L");
            Get("DecreaseXOffsetHotkey", "LeftAlt+J");
            Get("IncreaseYOffsetHotkey", "LeftAlt+I");
            Get("DecreaseYOffsetHotkey", "LeftAlt+K");
            Get("EnableRotationHotkey", "LeftControl+U");
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

        public void Set<T>(string key, T value, bool invoke)
        {
            Config[key] = value;
            if (invoke)
            {
                OnChanged?.Invoke(this);
            }
            Config?.Save();
        }

        public void Save()
        {
            Config?.Save();
            OnChanged?.Invoke(this);
        }
    }
}
