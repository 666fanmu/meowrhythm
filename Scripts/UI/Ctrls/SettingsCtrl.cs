// ******************************************************************
//       /\ /|       @author     Tubbti 
//       \ V/        @file 控制设置菜单
//       | "")       
//       /  |                    
//      /  \\        
//    *(__\_\        @Copyright  Copyright (c) , SweetCandy
// ******************************************************************

using SweetCandy.Extension;
using SweetCandy.Managers;
using SweetCandy.Settings;
using SweetCandy.UI.Views;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SweetCandy.UI.Ctrls
{
    public class SettingsCtrl : UIBase
    { 
        private SettingsView _view;
        private int _resolutionIndex;
        private int _windowModeIndex;
        private GlobalSetting _globalSetting;
        public GameObject Panel;
        public override void InitState()
        {
           
            _globalSetting = FindObjectOfType<GlobalSetting>();
            _view = UIView as SettingsView;
            if(Panel.activeInHierarchy) Panel.SetActive(false);
            Enter(null);
            UpdateView<SettingsView>(_view);
            _view.BackBtn.onClick.AddListener(Back);
            _view.SaveBtn.onClick.AddListener(SaveGame);
            _view.SoundBtn.onClick.AddListener(ChangeSoundOn);
            _view.MusicBtn.onClick.AddListener(ChangeMusicOn);
            _view.SoundVolumeSlider.onValueChanged.AddListener(ChangeSoundVolume);
            _view.MusicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
            _view.SoundVolumeSlider.OnEndDragListener(OnSoundSliderEndDrag);
            _view.resolutionBtn0.onClick.AddListener(() => OnResolutionChange(0));
            _view.resolutionBtn1.onClick.AddListener(() => OnResolutionChange(1));
            _view.windowBtn0.onClick.AddListener(() => OnWindowChange(0));
            _view.windowBtn1.onClick.AddListener(() => OnWindowChange(1));
        }

        public override void AddListeners()
        {
            EventManager.Instance.StartListening("OpenSettings",OpenSettings);
            
        }
        public override void RemoveListeners()
        {
            EventManager.Instance.StopListening("OpenSettings",OpenSettings);
        }
        void OnResolutionChange(int turn)
        {
            AudioManager.Instance.PlaySound("点击");
            switch (turn)
            {
                case 0:
                    --this._resolutionIndex;
                    break;
                case 1:
                    ++this._resolutionIndex;
                    break;
            }
            if (this._resolutionIndex > 9)
                this._resolutionIndex = 0;
            else if (this._resolutionIndex < 0)
                this._resolutionIndex = 9;
            if (this._resolutionIndex == 0)
            {
                Screen.SetResolution(1280, 720, !Global.isWindowMode);
                Global.resolution = new int[2]{ 1280, 720 };
            }
            else if (this._resolutionIndex == 1)
            {
                Screen.SetResolution(1280, 800, !Global.isWindowMode);
                Global.resolution = new int[2]{ 1280, 800 };
            }
            else if (this._resolutionIndex == 2)
            {
                Screen.SetResolution(1360, 768, !Global.isWindowMode);
                Global.resolution = new int[2]{ 1360, 768 };
            }
            else if (this._resolutionIndex == 3)
            {
                Screen.SetResolution(1366, 768, !Global.isWindowMode);
                Global.resolution = new int[2]{ 1366, 768 };
            }
            else if (this._resolutionIndex == 4)
            {
                Screen.SetResolution(1440, 900, !Global.isWindowMode);
                Global.resolution = new int[2]{ 1440, 900 };
            }
            else if (this._resolutionIndex == 5)
            {
                Screen.SetResolution(1600, 900, !Global.isWindowMode);
                Global.resolution = new int[2]{ 1600, 900 };
            }
            else if (this._resolutionIndex == 6)
            {
                Screen.SetResolution(1680, 1050, !Global.isWindowMode);
                Global.resolution = new int[2]{ 1680, 1050 };
            }
            else if (this._resolutionIndex == 7)
            {
                Screen.SetResolution(1920, 1080, !Global.isWindowMode);
                Global.resolution = new int[2]{ 1920, 1080 };
            }
            else if(this._resolutionIndex == 8)
            {
                Screen.SetResolution(2560,1440,!Global.isWindowMode);
                Global.resolution = new int[2] { 2560, 1440 };
            }
            else if(this._resolutionIndex == 9)
            {
                Screen.SetResolution(2560,1600,!Global.isWindowMode);
                Global.resolution = new int[2] { 2560, 1600 };
            }
            _globalSetting.SetResolution(); 
            _view.resolutionText.text = Global.resolution[0].ToString() + " x " + (object) Global.resolution[1];
        }
        void OnWindowChange(int turn)
        {
            AudioManager.Instance.PlaySound("点击");
            switch (turn)
            {
                case 0:
                    --this._windowModeIndex;
                    break;
                case 1:
                    ++this._windowModeIndex;
                    break;
            }
            if (this._windowModeIndex > 1)
                this._windowModeIndex = 0;
            else if (this._windowModeIndex < 0)
                this._windowModeIndex = 1;
            if (_windowModeIndex==0)
            {
                Global.windowMode = "窗口化";
                Screen.fullScreen = false;
                Global.isWindowMode = true;
            }
            else if(_windowModeIndex==1)
            {
                Global.windowMode = "全屏";
                Screen.fullScreen = true;
                Global.isWindowMode = false;
            }
            _view.windowModeText.text = Global.windowMode;
        }
        
        void OnSoundSliderEndDrag(PointerEventData data)
        {
            AudioManager.Instance.PlaySound("点击");
        }
        void Back()
        {
            AudioManager.Instance.PlaySound("点击");
            //EventManager.Instance.TriggerEvent("OpenMenu");
            Panel.SetActive(false);
        }
        
        void ChangeSoundOn()
        {
            AudioManager.Instance.PlaySound("点击");
            Global.SOUND_SWITCH = !Global.SOUND_SWITCH;
            _view.SoundBtnLine.SetActive(!Global.SOUND_SWITCH);
            _globalSetting.SetVolume();
        }

        void ChangeMusicOn()
        {
            AudioManager.Instance.PlaySound("点击");
            Global.MUSIC_SWITCH = !Global.MUSIC_SWITCH;
            _view.MusicBtnLine.SetActive(!Global.MUSIC_SWITCH);
            _globalSetting.SetVolume();
        }

        void ChangeSoundVolume(float newValue)
        {
            Global.SOUND_VOLUME = newValue;
            _globalSetting.SetVolume();
        }

        void ChangeMusicVolume(float newValue)
        {
            Global.MUSIC_VOLUME =newValue;
            _globalSetting.SetVolume();
        }
        void OpenSettings()
        {
            Panel.SetActive(true);
        }
        void SaveGame()
        {
            SaveManager.Instance.Save();
            AudioManager.Instance.PlaySound("点击");
        }
        
    }
}