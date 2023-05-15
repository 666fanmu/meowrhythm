using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SweetCandy.UI.Views
{
    public class GuideMenuView : UIView
    {
        public Button GuideGroundBtn;
        public TextMeshProUGUI GuideInfoText;
        public TextMeshProUGUI GuidePressText;
        private void Awake()
        {
            GuideGroundBtn = GameObject.Find("GuideBtn").GetComponent<Button>();
            GuideInfoText = GameObject.Find("GuideText").GetComponent<TextMeshProUGUI>();
            GuidePressText = GameObject.Find("GuidePressText").GetComponent<TextMeshProUGUI>();
        }
    }
}