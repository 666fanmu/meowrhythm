using TMPro;
using UnityEngine;
namespace SweetCandy.UI.Views
{
    public class GameDialogueView :UIView
    {
        public GameObject robot;
        public GameObject cat;
        public TextMeshProUGUI robotText;
        public TextMeshProUGUI robotSubText;
        public TextMeshProUGUI catText;
        public TextMeshProUGUI catSubText;
        
        void Awake()
        {
            robot = GameObject.Find("Robots");
            cat = GameObject.Find("Cats");
            robotText = GameObject.Find("RobotText").GetComponent<TextMeshProUGUI>();
            catText = GameObject.Find("CatText").GetComponent<TextMeshProUGUI>();
            robotSubText = GameObject.Find("RobotSubText").GetComponent<TextMeshProUGUI>();
            catSubText = GameObject.Find("CatSubText").GetComponent<TextMeshProUGUI>();
        }
    }
}
