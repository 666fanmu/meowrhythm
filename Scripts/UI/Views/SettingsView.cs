using SweetCandy.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SweetCandy.UI.Views
{
    public class SettingsView : UIView
    {
        public Slider MusicVolumeSlider;
        public Slider SoundVolumeSlider;

        public Button MusicBtn;
        public Button SoundBtn;
        public GameObject MusicBtnLine;
        public GameObject SoundBtnLine;
        public Button BackBtn;
        public Button SaveBtn;
        public Text resolutionText;
        public Button resolutionBtn0;//向左
        public Button resolutionBtn1;//向右
        public Text windowModeText;
        public Button windowBtn0;
        public Button windowBtn1;

        private void Awake()
        {
            resolutionText = GameObject.Find("resolutionText").GetComponent<Text>();
            resolutionBtn0 = GameObject.Find("resolutionBtn0").GetComponent<Button>();
            resolutionBtn1 = GameObject.Find("resolutionBtn1").GetComponent<Button>();
            windowModeText = GameObject.Find("windowModeText").GetComponent<Text>();
            windowBtn0= GameObject.Find("windowBtn0").GetComponent<Button>();
            windowBtn1=GameObject.Find("windowBtn1").GetComponent<Button>();
            SoundBtnLine = GameObject.Find("SoundBtnLine");
            SoundBtn = GameObject.Find("SoundBtn").GetComponent<Button>();
            SoundVolumeSlider = GameObject.Find("SoundVolumeSlider").GetComponent<Slider>();
            MusicBtnLine = GameObject.Find("MusicBtnLine");
            MusicBtn = GameObject.Find("MusicBtn").GetComponent<Button>();
            MusicVolumeSlider = GameObject.Find("MusicVolumeSlider").GetComponent<Slider>();
            SaveBtn = GameObject.Find("SaveBtn").GetComponent<Button>();
            BackBtn = GameObject.Find("BackBtn").GetComponent<Button>();

        }

        public override void Refresh()
        {
            SoundVolumeSlider.value = (float)Global.SOUND_VOLUME;
            MusicVolumeSlider.value = (float)Global.MUSIC_VOLUME;
            resolutionText.text = Global.resolution[0] + " x " + Global.resolution[1];
            windowModeText.text = Global.windowMode;
            SoundBtnLine.SetActive(!Global.SOUND_SWITCH);
            MusicBtnLine.SetActive(!Global.MUSIC_SWITCH);
        }
    }
}