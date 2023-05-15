using System;
using DG.Tweening;
using SweetCandy.Extension;
using SweetCandy.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace SweetCandy.UI.Ctrls
{
    public class BuilderCanvasCtrl : MonoBehaviour
    {
        private Canvas _canvas;
        public Button closeBtn;
        private void Awake()
        {
            _canvas = this.gameObject.GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            EventManager.Instance.StartListening("OpenBuilder",Open);
            closeBtn.onClick.AddListener(Close);
        }

        void Open()
        {
            _canvas.FadeIn(this.gameObject,null);
        }

        void Close()
        {
            _canvas.FadeOut(this.gameObject);
        }
    }
}