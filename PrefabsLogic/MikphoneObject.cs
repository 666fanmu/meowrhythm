
using System;
using SweetCandy.Basic;
using SweetCandy.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace SweetCandy.PrefabsLogic
{
    public class MikphoneObject:Collectible
    {
       
        void FixedUpdate()
        {
            transform.Translate(0, 0, -speed * Time.deltaTime,Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AudioManager.Instance.PlaySound("点击");
                GameObjectPool.Instance.CollectObject(gameObject);
                ScoreManager.Instance.Microphone();
                if (!Global.PointDictionary["getHuatong"])
                {
                    EventManager.Instance.TriggerEvent("ShowMik");
                    Global.PointDictionary["getHuatong"] = true;
                }
            }
            if (other.CompareTag("Wall"))
            {
               
                GameObjectPool.Instance.CollectObject(gameObject);
               
                
            }
        }
    }
}