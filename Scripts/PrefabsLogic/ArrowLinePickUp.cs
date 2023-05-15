using System;
using SweetCandy.Basic;
using SweetCandy.Event;
using SweetCandy.Managers;
using UnityEngine;

namespace SweetCandy.PrefabsLogic
{
    public class ArrowLinePickUp : MonoBehaviour,IResetable
    {
        public float moveSpeed=500;
            public bool whichPosition=false;
            private void OnTriggerEnter2D(Collider2D col)
            {
                if (col.CompareTag("DestoryWall"))
                {
                    Debug.Log("destory self");
                    GameObjectPool.Instance.CollectObject(this.gameObject);
                }
            }

            private void OnTriggerStay2D(Collider2D other)
            {
               
            }

            void Update()
            {
           
                if (whichPosition)
                {
                    this.transform.Translate(-moveSpeed * Time.unscaledDeltaTime, 0, 0);
                }
                else
                {
                    this.transform.Translate(moveSpeed * Time.unscaledDeltaTime, 0, 0);
                }

          
            }

            public void onReset()
            {
                if (GameManager.Instance.bossState == BossState.Speed)
                    moveSpeed = 800f;
                else
                    moveSpeed = 500f;
            }
    }
}