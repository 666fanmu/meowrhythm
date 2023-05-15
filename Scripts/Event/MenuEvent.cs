using UnityEngine;
using SweetCandy.Managers;

namespace SweetCandy.Event
{

    public class MenuEvent : MonoBehaviour
    {
        private void Start()
        {
            EventManager.Instance.StartListening("QuitGame", QuitGame);
        }
        
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }
}

