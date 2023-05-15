using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace SweetCandy.Extension
{
    public static class UIExtension
    {
        public static void FadeIn(this Canvas canvas, GameObject target, TweenCallback action)
        {
            var canvasgroup = target.GetOrAddComponent<CanvasGroup>();
            canvasgroup.DOFade(1f,.3f).OnComplete(action);
            canvasgroup.blocksRaycasts = true;
        }

        public static void FadeOut(this Canvas canvas, GameObject target)
        {
            var canvasgroup = target.GetOrAddComponent<CanvasGroup>();
            canvasgroup.DOFade(0f, .3f);
            canvasgroup.blocksRaycasts = false;
           
            
        }
    }
}