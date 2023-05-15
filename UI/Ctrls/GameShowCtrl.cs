using System;
using DG.Tweening;
using SweetCandy.Managers;
using SweetCandy.UI.Views;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SweetCandy.UI.Ctrls
{
    public class GameShowCtrl : UIBase
    {
        public GameObject Panel;
        private GameShowView _view;
        //private Sequence scoreSequence;
        public override void InitState()
        {
            //Exit();
            _view = UIView as GameShowView;
            if(SceneManager.GetActiveScene()==SceneManager.GetSceneByName("Game"))
                Invoke("Open",4f);
            //RefreshEnergyShow(ScoreManager.Instance.currentEnergy);
            //_view.scoreText.text = "0";
            //scoreSequence = DOTween.Sequence();
            //scoreSequence.SetAutoKill(false);
        }
        public override void AddListeners()
        {
            EventManager.Instance.StartListening("OpenGameShow",Open);
            EventManager.Instance.StartListening("CloseGameShow",Close);
        }

        public override void RemoveListeners()
        {
            EventManager.Instance.StopListening("OpenGameShow",Open);
            EventManager.Instance.StopListening("CloseGameShow",Close);
        }

        private void Close()
        {
            Exit();
        }
        private void Open()
        {
            Enter(null);
        }
        /*public void RefreshScore(float score)
        {
            scoreSequence.Append(
                DOTween.To(
                    delegate (float value) {
                var temp = Math.Floor(value);
                _view.scoreText.text = temp + "";
            }, oldScore, score, 0.4f));
            oldScore = score;
           // _view.scoreText.text = score.ToString();
        }*/

        /*public void RefreshMulti(int rate)//倍率显示
        {
            _view.multiText.text="X " + rate;
        }*/
        public void RefreshEnergyShow(float energy)//能量显示
        {
            _view.energyScroll.size = (float)energy/100;
            //_view.energyText.text = energy.ToString();
        }

        public void RefreshDangerShow(float danger)
        {
            _view.playerDangerMask.padding = new Vector4(0,0,danger*3.5f,0);
        }
    }
}