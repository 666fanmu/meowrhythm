using SweetCandy.Managers;
using SweetCandy.UI.Views;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SweetCandy.UI.Ctrls
{
    public class MainMenuCtrl : UIBase
    {
        private MainMenuView _view;
        public Animation Animation;
        public override void InitState()
        {
#if UNITY_EDITOR
            SaveManager.Instance.Delete();
#endif
            _view = UIView as MainMenuView;
            //Enter(null);
            SaveManager.Instance.Load();
            UpdateView<MainMenuView>(_view);
            _view.PlayBtn.onClick.AddListener(StartGame);
            _view.OpenSettingsBtn.onClick.AddListener(OpenSettings);
            _view.ExitBtn.onClick.AddListener(QuitGame);
            _view.titleBtn.onClick.AddListener(OpenBuilder);
        }
    
        public override void AddListeners()
        {
            Animation.PlayQueued("Lobby2D");
            EventManager.Instance.StartListening("StartGame",StartGame);
        }

        public override void RemoveListeners()
        {
            EventManager.Instance.StopListening("StartGame",StartGame);

        }
        
        private void StartGame()
        {
            AudioManager.Instance.PlaySound("点击");
            //保存需要加载的目标场景  
            if (Global.isPassedGuide)
            {
                AppInformation.nextSceneName = "Game";
            }
            else
            {
                AppInformation.nextSceneName = "Guide";
            }
           
            SceneManager.LoadScene("LoadingScene");        
                        
        }
        private void OpenSettings()
        {
            AudioManager.Instance.PlaySound("点击");
            EventManager.Instance.TriggerEvent("OpenSettings");
        }

        private void OpenBuilder()
        {
            EventManager.Instance.TriggerEvent("OpenBuilder");
        }
        private void QuitGame()
        {
            AudioManager.Instance.PlaySound("点击");
            Debug.Log("quit");
            EventManager.Instance.TriggerEvent("QuitGame");
        }
    }
}