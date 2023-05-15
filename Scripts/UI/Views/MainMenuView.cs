// ******************************************************************
//       /\ /|       @author     Tubbti 
//       \ V/        @file 游戏主菜单View
//       | "")       负责获取主菜单面板组件
//       /  |                    
//      /  \\        
//    *(__\_\        @Copyright  Copyright (c) , SweetCandy
// ******************************************************************

using System;
using UnityEngine;
using UnityEngine.UI;

namespace SweetCandy.UI.Views
{
    public class MainMenuView : UIView
    {
        public Button PlayBtn;
        public Button ExitBtn;
        public Button OpenSettingsBtn;
        public Button titleBtn;
        private void Awake()
        {
            PlayBtn = GameObject.Find("PlayBtn").GetComponent<Button>();
            ExitBtn = GameObject.Find("ExitBtn").GetComponent<Button>();
            OpenSettingsBtn = GameObject.Find("OpenSettingsBtn").GetComponent<Button>();
            titleBtn = GameObject.Find("Title").GetComponent<Button>();
        }

        

        public override void Refresh()
        {
            base.Refresh();
        }
    }
}