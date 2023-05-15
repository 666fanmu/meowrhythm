using System;
using SweetCandy.Basic;
using SweetCandy.Managers;
using UnityEngine;

namespace SweetCandy.PrefabsLogic
{
    public class ObsObject : Collectible
    {
        void FixedUpdate()
        {
            transform.Translate(0, 0, -speed * Time.deltaTime);
            
            //CheckoutMode(GameMode.BeCatched,false);
            CheckoutMode(GameMode.Waiting,false);
            CheckoutMode(GameMode.ColorTime,false);
            CheckoutMode(GameMode.ComboTime,false);
        }
        
       

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ScoreManager.Instance.NoteMissed();
                ScoreManager.Instance.multiplierTracker = 0;
                ScoreManager.Instance.currentMultiplier = 1;
                isCollectedByPlayer = 1;
                ScoreManager.Instance.getChange(0, 1, 1, -3);
                AudioManager.Instance.PlaySound("跳跃音效3");
                Collect();
            }

            if (other.CompareTag("Note"))
            {
                GameManager.Instance.CreateObject(ObjName.Obs);
                isCollectedByPlayer = 0;
                Collect();
            }

            if (other.CompareTag("Wall"))
            {
                isCollectedByPlayer = -1;
                Collect();
            }

            
        }

        public override void onReset()
        {
            speed = 1300f;
        }
    }
}