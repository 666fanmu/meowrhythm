// ******************************************************************
//       /\ /|       @author     Tubbti 
//       \ V/        @file 游戏内菜单控制脚本
//       | "")       挂载在UICtrl上
//       /  |                    
//      /  \\        
//    *(__\_\        @Copyright  Copyright (c) , SweetCandy
// ******************************************************************

using DG.Tweening;
using SweetCandy.Managers;
using SweetCandy.UI.Views;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SweetCandy.UI.Ctrls
{
        public class GameMenuCtrl : UIBase
        {
                private GameMenuView _view;
                [Tooltip("游戏菜单面板")] public GameObject Panel;

                public override void InitState()
                {
                        Cursor.visible = false;
                        _view = UIView as GameMenuView;
                        UpdateView<GameMenuView>(_view);
                        _view.SettingBtn.onClick.AddListener(OpenSettingsPanel);
                        _view.BackGameBtn.onClick.AddListener(CloseGameMenu);
                        _view.ReturnLobbyBtn.onClick.AddListener(QuitToLobby);
                }

                public override void AddListeners()
                {
                        
                        EventManager.Instance.StartListening("OpenMenu",OpenGameMenu);
                        EventManager.Instance.StartListening("CloseMenu",CloseGameMenu);
                }

                public override void RemoveListeners()
                {
                        EventManager.Instance.StopListening("OpenMenu",OpenGameMenu);
                        EventManager.Instance.StopListening("CloseMenu",CloseGameMenu);
                }

                public void OpenSettingsPanel()
                {
                        //Panel.SetActive(false);
                        Debug.Log("open setting!");
                        AudioManager.Instance.PlaySound("点击");
                        EventManager.Instance.TriggerEvent("OpenSettings");
                }

                public void QuitToLobby()
                {
                        Time.timeScale = 1;
                        AudioManager.Instance.PlaySound("点击");
                        AppInformation.nextSceneName = "Lobby";
                        SceneManager.LoadScene("LoadingScene");  
                }

                public void OpenGameMenu()
                {
                        AudioManager.Instance.bgmAudioSource.Pause();
                        Enter(OpenGameMenu_del);
                }

                private void OpenGameMenu_del()
                {
                        Cursor.visible = true;
                        Time.timeScale = 0;
                }
                public void CloseGameMenu()
                {
                        AudioManager.Instance.PlaySound("点击");
                        Time.timeScale = 1;
                        Cursor.visible = false;
                        Exit();
                        AudioManager.Instance.bgmAudioSource.Play();
                        
                }
        }
}

