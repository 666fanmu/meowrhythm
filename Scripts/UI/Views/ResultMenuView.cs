using UnityEngine;
using UnityEngine.UI;

namespace SweetCandy.UI.Views
{
    public class ResultMenuView : UIView
    {
        public Text scoreText;
        public Image levelImage;
        public Image newScore;
        public Button backBtn;
        public Button restartBtn;
        private void Awake()
        {
            scoreText = GameObject.Find("scoreText").GetComponent<Text>();
            levelImage = GameObject.Find("levelImage").GetComponent<Image>();
            newScore = GameObject.Find("newScore").GetComponent<Image>();
            backBtn = GameObject.Find("backBtn").GetComponent<Button>();
            restartBtn = GameObject.Find("restartBtn").GetComponent<Button>();
        }
    }
}