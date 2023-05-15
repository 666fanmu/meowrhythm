using System;
using System.Collections;
using SweetCandy.Basic;
using SweetCandy.Event;
using SweetCandy.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace SweetCandy.PrefabsLogic
{
    
    public class Generatepickups : MonoBehaviour,IResetable
    {
        public float moveSpeed=500;
        public bool whichPosition=false;
        private ComboTimeEvent _comboTimeEvent;
        private void Awake()
        {
            _comboTimeEvent = FindObjectOfType<ComboTimeEvent>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("DestoryWall"))
            {
                Debug.Log("destory self");
                _comboTimeEvent.DoComboEffect(ComboEffect.Miss,this.gameObject.transform.localPosition);
                GameObjectPool.Instance.CollectObject(this.gameObject);
            }
        }

        #region OldFunc

        /*private void OnEnable()
        {
            StartCoroutine(Move());
        }

        private void OnDisable()
        {
            StopCoroutine(Move());
        }

        IEnumerator Move()
        {
            while (this.gameObject.activeSelf)
            {
                if (whichPosition)
                {
                    this.transform.Translate(-moveSpeed * Time.unscaledDeltaTime, 0, 0);
                }
                else
                {
                    this.transform.Translate(moveSpeed * Time.unscaledDeltaTime, 0, 0);
                }

                yield return new WaitForEndOfFrame();
            }
           
        }*/

        #endregion
        
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
                moveSpeed = 1000f;
            else
                moveSpeed = 700f;
        }
    }
}
