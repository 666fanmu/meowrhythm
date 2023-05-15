using System;
using System.Collections.Generic;

namespace SweetCandy.Settings
{
    [Serializable]
        public class UserSettings
        {
            public int[] resolution = new int[2]{ 1920, 1080 };
            public bool isWindowMode = false;
            public string windowMode = new string("全屏");
            public bool MUSIC_SWITCH = true;
            public bool SOUND_SWITCH = true; 
            public float MUSIC_VOLUME = 0.75f;
            public float SOUND_VOLUME = 0.75f;
            public bool isPassedGuide = false;
        }
}