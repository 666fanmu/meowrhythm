using System;
using System.Collections.Generic;
using DG.Tweening;
using SweetCandy.Managers;
using SweetCandy.UI.Views;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SweetCandy.UI.Ctrls
{
    public class ResultMenuCtrl:UIBase
    {
        private ResultMenuView _view;
        public List<Sprite> rankSprites;
        public override void InitState()
        {
            _view = UIView as ResultMenuView;
            if(Root.GetComponent<CanvasGroup>().alpha==1) Exit();
            _view.backBtn.onClick.AddListener(BackLobby);
            _view.restartBtn.onClick.AddListener(BackGame);
        }

        public override void AddListeners()
        {
            EventManager.Instance.StartListening("OpenResultMenu",OpenResultMenu);
        }

        public void GetResult()
        {
            ScoreManager.Instance.GetFinalRank();
            _view.levelImage.sprite = rankSprites[ScoreManager.Instance.finalRank];
            _view.levelImage.DOFade(1f, 5f);
            DOTween.Sequence().Append(
                DOTween.To(
                    delegate (float value) {
                        var temp = Math.Floor(value);
                        _view.scoreText.text = temp + "";
                        AudioManager.Instance.PlaySound("拾取音效5");
                    }, 0, ScoreManager.Instance. currentScore+ScoreManager.Instance.currentEnergy, 2f));
            //_view.scoreText.text = ScoreManager.Instance.currentScore.ToString();
            //Time.timeScale = 0f;
        }

        private void BackLobby()
        {
            AudioManager.Instance.PlaySound("点击");
            AppInformation.nextSceneName = "Lobby";
            SceneManager.LoadScene("LoadingScene");        
        }

        private void BackGame()
        {
            AudioManager.Instance.PlaySound("点击");
            AppInformation.nextSceneName = "Game";
            SceneManager.LoadScene("LoadingScene");        
        }
        public void OpenResultMenu()
        {
            EventManager.Instance.TriggerEvent("CloseGameShow");
            Cursor.visible = true;
            Enter(GetResult);
        }
    }
}