using System;
using System.Collections.Generic;
using SweetCandy.Managers;
using SweetCandy.UI.Views;
using DG.Tweening;
using SweetCandy.Event;

namespace SweetCandy.UI.Ctrls
{
    public class GameDialogueCtrl : UIBase
    {
        public Queue<DialogueMsg> DialogueQueue=new Queue<DialogueMsg>();
        private GameDialogueView _view;
        public override void InitState()
        {
            _view= UIView as GameDialogueView;
            EventManager.Instance.StartListening("CloseDialogue",CloseRobotDialogue);
            _view.cat.SetActive(false);
            _view.robot.SetActive(false);
        }
        public override void RemoveListeners()
        {
            EventManager.Instance.StopListening("CloseDialogue",CloseRobotDialogue);
        }
        
        /// <summary>
        /// ChangeText
        /// </summary>
        /// <param name="whose">0为猫说 1为robot说</param>
        /// <param name="info">主文本内容</param>
        /// <param name="subinfo">副文本内容</param>
        public void ChangeDialogueInfo(int whose,string info,string subinfo="",float lastTime=5f)
        {
            if (whose == 1)
            {
                _view.robotText.text = info;
                _view.robotSubText.text = subinfo;
                if (!_view.robot.activeInHierarchy)
                {
                    _view.robot.SetActive(true);
                    _view.robot.transform.DOLocalMoveX(-250, 1f).SetEase(Ease.OutCubic);
                    return;
                }
            }
            else
            {
                DialogueMsg dm = new DialogueMsg();
                dm.info = info;
                dm.subinfo = subinfo;
                dm.lastTime = lastTime;
                DialogueQueue.Enqueue(dm);
            }

        }

        private void Update()
        {
            if (!_view.cat.activeInHierarchy && DialogueQueue.Count > 0)
            {
                CtrlQueue();
                _view.cat.SetActive(true);
            }
        }

        private void CtrlQueue()
        {
            if (DialogueQueue.Count > 0)
            {
                DialogueMsg sdm = DialogueQueue.Dequeue(); 
                _view.catText.text = sdm.info;
                _view.catSubText.text = sdm.subinfo;
                _view.cat.transform.DOLocalMoveX(250, 1f).SetEase(Ease.OutCubic);
                Invoke("CloseCatDialogue",sdm.lastTime);
            }
        }

        private void CloseCatDialogue()
        {
            if(_view.cat.activeInHierarchy)
                _view.cat.transform.DOLocalMoveX(600, 1f).SetEase(Ease.OutCubic).OnComplete(()=>_view.cat.SetActive(false));
        }
        private void CloseRobotDialogue()
        {
            if(_view.robot.activeInHierarchy) 
                _view.robot.transform.DOLocalMoveX(-600, 1f).SetEase(Ease.OutCubic).OnComplete(() => _view.robot.SetActive(false));
        }
    }
}
