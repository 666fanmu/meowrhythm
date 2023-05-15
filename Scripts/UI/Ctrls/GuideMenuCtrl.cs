using System;
using System.Collections;
using SweetCandy.Managers;
using SweetCandy.UI.Views;
using UnityEngine;

namespace SweetCandy.UI.Ctrls
{
    public class GuideMenuCtrl : UIBase
    {
        private GuideMenuView _view;
        //public GameObject Panel;
        public override void InitState()
        {
            //Panel.SetActive(true);
            _view= UIView as GuideMenuView;
            Exit();
            EventManager.Instance.StartListening("OpenGuideMenu",OpenGuideMenu);
            EventManager.Instance.StartListening("CloseGuideMenu",CloseGuideMenu);
        }
        public override void RemoveListeners()
        {
            EventManager.Instance.StopListening("OpenGuideMenu",OpenGuideMenu);
            EventManager.Instance.StopListening("CloseGuideMenu",CloseGuideMenu);
        }
        

        public void ChangeGuideInfo(string info,string pressinfo)
        {
            _view.GuideInfoText.text = info;
            _view.GuidePressText.text = pressinfo;
        }
        private void OpenGuideMenu()
        {
            Enter(OpenGuideMenu_del);
        }

        private void OpenGuideMenu_del()
        {
            Time.timeScale = 0.05f;
        }

        private void CloseGuideMenu()
        {
            Time.timeScale = 1f;
            Exit();
        }
    }
}