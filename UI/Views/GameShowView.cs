using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SweetCandy.UI.Views
{
    public class GameShowView : UIView
    {
        //public Text scoreText;
        //public Text multiText;
        //public Text energyText;
        public Scrollbar energyScroll;
        public RectMask2D playerDangerMask;

        private void Awake()
        {
            //scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            //multiText = GameObject.Find("multiText").GetComponent<Text>();
            //energyText = GameObject.Find("energyText").GetComponent<Text>();
            energyScroll = GameObject.Find("energyScroll").GetComponent<Scrollbar>();
            playerDangerMask = GameObject.Find("playerDangerMask").GetComponent<RectMask2D>();
        }
    }
}