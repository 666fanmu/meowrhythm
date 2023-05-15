using SweetCandy.Basic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SweetCandy.Managers
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [Tooltip("音效播放器")] public AudioSource soundAudioSource;
        [Tooltip("BGM播放器")] public AudioSource bgmAudioSource;
       
        protected override void Init()
        {
            soundAudioSource = GameObject.Find("soundAudioSource").GetComponent<AudioSource>();
            bgmAudioSource = GameObject.Find("bgmAudioSource").GetComponent<AudioSource>();
            bgmAudioSource.volume = Global.MUSIC_VOLUME;
            soundAudioSource.volume = Global.SOUND_VOLUME;
            soundAudioSource.mute = !Global.SOUND_SWITCH;
            bgmAudioSource.mute = !Global.MUSIC_SWITCH;
        }

        public void PlayMusic(string s)
        {
            bgmAudioSource.clip= Resources.Load<AudioClip>("音乐/" + s);
            bgmAudioSource.Play();
        }

        public void PlaySound(string s, int random = 0)
        {
            if (random == 0)
                this.soundAudioSource.PlayOneShot(Resources.Load<AudioClip>("音效/" + s));
            else
                this.soundAudioSource.PlayOneShot(Resources.Load<AudioClip>("音效/" + s + (object)Random.Range(0, random)));
        }

    }
}