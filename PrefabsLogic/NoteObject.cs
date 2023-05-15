// ******************************************************************
//       /\ /|       @author     Tubbti 
//       \ V/        @file 挂载在音符上的自身逻辑
//       | "")       其中Boat为飞船Tag
//       /  |                    
//      /  \\        
//    *(__\_\        @Copyright  Copyright (c) , SweetCandy
// ******************************************************************

using System;
using SweetCandy.Basic;
using SweetCandy.Managers;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace SweetCandy.PrefabsLogic
{
    public class NoteObject : Collectible
    {

        public GameObject hitEffect; //触碰特效
        public GameObject missEffect; //丢失特效
        

        void FixedUpdate()
        {
            transform.Translate(0, 0, -speed * Time.deltaTime);
            
        }
        
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.CompareTag("Player"))
            {
                AudioManager.Instance.PlaySound("点击");
                ScoreManager.Instance.Hit();
                this.isCollectedByPlayer = 1;
                //hitEffect往近处偏一点
                Vector3 hitEffectPosition =
                    new Vector3(transform.position.x, transform.position.y, transform.position.z - 500);
                GameManager.Instance.HitEffect("NoteEffect",hitEffect, hitEffectPosition);
            }

            if (!ScoreManager.Instance.isGetDisco)
            {
                if (other.CompareTag("Wall"))
                {
                    this.isCollectedByPlayer = -1; 
                    ScoreManager.Instance.NoteMissed();
                }
            }
            
            Collect();
        }
        
    }
}