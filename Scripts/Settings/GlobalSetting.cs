using System;
using DG.Tweening;
using SweetCandy.Basic;
using UnityEngine;

namespace SweetCandy.Settings
{
    public class GlobalSetting : MonoBehaviour
    {
        private FitScreen _fitScreen;
        [SerializeField]
        private AudioSource soundAudioSource;
        [SerializeField]
        private AudioSource bgmAudioSource;

        private void Start()
        {
            _fitScreen = FindObjectOfType<FitScreen>();
            soundAudioSource = GameObject.Find("soundAudioSource").GetComponent<AudioSource>();
            bgmAudioSource = GameObject.Find("bgmAudioSource").GetComponent<AudioSource>();
            //lights = FindObjectsOfType<Light>();
            SetVolume();
            SetResolution();
        }

        public void SetVolume()
        {
            bgmAudioSource.volume = Global.MUSIC_VOLUME;
            soundAudioSource.volume = Global.SOUND_VOLUME;
            soundAudioSource.mute = !Global.SOUND_SWITCH;
            bgmAudioSource.mute = !Global.MUSIC_SWITCH;
            
        }
        
        public void SetResolution() => this._fitScreen.FitResolutionScreen(Global.resolution[0], Global.resolution[1]);
    }
}