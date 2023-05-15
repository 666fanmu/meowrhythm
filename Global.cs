using System;
using System.Collections.Generic;
using SweetCandy.Event;
using SweetCandy.Managers;

namespace SweetCandy
{
    public class Global
    {
        public static int[] resolution = new int[2]{ 1920, 1080 };
        public static bool isWindowMode = false;
        public static string windowMode = new string("全屏");
        public static bool MUSIC_SWITCH = true; // 音乐开关
        public static bool SOUND_SWITCH = true; // 音效开关
        public static float MUSIC_VOLUME = 0.75f; // 音乐音量
        public static float SOUND_VOLUME = 0.5f; // 音效音量
        public static bool isPassedGuide = false;
        public static Dictionary<string, bool> PointDictionary = new Dictionary<string, bool>()
        {
            {"getColor",false},
            {"getBeCatch",false},
            {"getIsCatching",false},
            {"getCombo",false},
            {"getCholo",false},
            {"getDisco",false},
            {"getBomb",false},
            {"getHand",false},
            {"getHuatong",false},
        };
    }
}