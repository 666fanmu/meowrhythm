
using UnityEngine;
using UnityEngine.UI;

namespace SweetCandy.UI.Views
{

    public class GameMenuView : UIView
    {
        public Button SettingBtn;
        public Button ReturnLobbyBtn;
        public Button BackGameBtn;

        private void Awake()
        {
            SettingBtn = GameObject.Find("OpenSettingBtn").GetComponent<Button>();
            ReturnLobbyBtn = GameObject.Find("ReturnLobbyBtn").GetComponent<Button>();
            BackGameBtn = GameObject.Find("BackGameBtn").GetComponent<Button>();
        }

        public override void Refresh()
        {
            base.Refresh();
        }
    }
}