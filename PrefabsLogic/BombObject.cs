using System;
using SweetCandy.Basic;
using SweetCandy.Managers;
using UnityEngine;
namespace SweetCandy.PrefabsLogic
{
    public class BombObject :Collectible
    {
        public GameObject BombEffect;
        void FixedUpdate()
        {
            transform.Translate(0, -speed * Time.deltaTime,0,Space.World);
            if (this.transform.position.y<=-2300)
            {
                AudioManager.Instance.PlaySound("爆炸音效2");
                GameObjectPool.Instance.CollectObject(this.gameObject);
               
            }
            
            CheckoutMode(GameMode.Game,true);
        }

        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AudioManager.Instance.PlaySound("爆炸音效1");
                EventManager.Instance.TriggerEvent("Bomb");
                ScoreManager .Instance.getChange(-5,5,0,-5);
                //遇到玩家时不一样的特效
                GameObjectPool.Instance.CollectObject(this.gameObject);
                if (!Global.PointDictionary["getBomb"])
                {
                    EventManager.Instance.TriggerEvent("ShowBomb");
                    Global.PointDictionary["getBomb"] = true;
                }
              
            }

            if (other.CompareTag("Wall"))
            {
                Vector3 hitEffectPosition =
                    new Vector3(transform.position.x, transform.position.y, transform.position.z);
                GameManager.Instance.HitEffect("BombEffect",BombEffect,hitEffectPosition);
                GameObjectPool.Instance.CollectObject(this.gameObject);
                
            }
        }
    }
}
