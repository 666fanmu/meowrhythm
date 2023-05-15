using UnityEngine;  
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SweetCandy.Managers;
using UnityEngine.UI;  
using UnityEngine.SceneManagement;

namespace SweetCandy.Basic
{
    public class AsyncLoadScene : MonoBehaviour
    {
        #region Panel
        
        private List<string> tips = new List<string>()
        {
            "前进和左右移动都消耗能量 拾取音符可以回复能量 能量耗尽猫猫就要逃不出去了！",
            "注意避开休止符！巧克力是回复能量的好东西 麦克风可以增加大量分数 闪光球是好东西，千万别错过!",
            "建议双人游玩！",
        };
        public Slider loadingSlider;
        public Text tipsText;
        public Image Title;
        #endregion
       
        
        private float ColorAlpha = 0.9f;
        
        private float temp = 0;

        private float loadingSpeed = 1f;

        private float targetValue; 

        private AsyncOperation asyncLoad; 
        
        void Start()
        {
            loadingSlider.value = 0.0f;
            if (SceneManager.GetActiveScene().name == "LoadingScene")
            {
                DOTween.Sequence()
                    .Append(Title.DOFade(1, 0.2f))
                    .Append(tipsText.DOFade(0, 0))
                    .AppendCallback(() =>
                    {
                        tipsText.text = tips[0];
                    })
                    .Append(tipsText.DOFade(1, 0.2f))
                    .AppendInterval(1.5f)
                    .Append(tipsText.DOFade(0, 0))
                    .AppendCallback(() =>
                    {
                        tipsText.text = tips[1];
                    })
                    .Append(tipsText.DOFade(1, 0.2f))
                    .AppendInterval(1.5f)
                    .Append(tipsText.DOFade(0, 0))
                    .AppendCallback(() =>
                    {
                        tipsText.text = tips[2];
                    })
                    .Append(tipsText.DOFade(1, 0.2f));
                
                StartCoroutine(AsyncLoading());
            }
        }

        IEnumerator AsyncLoading()
        {
            asyncLoad = SceneManager.LoadSceneAsync(AppInformation.nextSceneName);
            asyncLoad.allowSceneActivation = false;
            yield return asyncLoad;
        }

        void Update()
        {
            targetValue = asyncLoad.progress;
            if (asyncLoad.progress >= 0.9f)
            {
                targetValue = 1.0f;
            }

            if (targetValue != loadingSlider.value)
            {
                loadingSlider.value = Mathf.Lerp(loadingSlider.value, targetValue, Time.deltaTime * loadingSpeed);
                if (Mathf.Abs(loadingSlider.value - targetValue) < 0.01f)
                {
                    loadingSlider.value = targetValue;
                }
            }
            
            if ((int)(loadingSlider.value * 100) == 100)
            {
                asyncLoad.allowSceneActivation = true;
            }
        }
    }
}