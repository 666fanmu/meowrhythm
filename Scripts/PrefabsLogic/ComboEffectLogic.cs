using System;
using DG.Tweening;
using SweetCandy.Basic;
using UnityEngine;
using UnityEngine.UI;

namespace SweetCandy.PrefabsLogic
{
    public class ComboEffectLogic : MonoBehaviour
    {
        private Image _image;

        private void Start()
        {
            _image = this.gameObject.GetComponent<Image>();
        }

        private void OnEnable()
        {
            GameObjectPool.Instance.CollectObject(this.gameObject,3f);
            this.gameObject.transform.DOLocalMoveY(200f,2f);
            _image.DOFade(1, 2f).OnComplete(()=>_image.DOFade(0,1f));
            
        }
    }
}