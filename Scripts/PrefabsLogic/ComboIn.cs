using System;
using System.Security.Cryptography.X509Certificates;
using SweetCandy.Event;
using UnityEngine;

namespace SweetCandy.PrefabsLogic
{
    public class ComboIn :MonoBehaviour
    {
        private ComboTimeEvent _comboTimeEvent;
        public Animation comboAnimation;
        private void Awake()
        {
            _comboTimeEvent = FindObjectOfType<ComboTimeEvent>();
        }

        private void OnEnable()
        {
            comboAnimation.Play("ComboIn");
            //TODO 播放对话
        }

        public void Exit()
        {
            comboAnimation.Play("ComboOut",AnimationPlayMode.Stop);
        }

        private void ComboTimeIn()
        {
            _comboTimeEvent.ComboStart();
        }
        
    }
}