using SweetCandy.Basic;
using SweetCandy.Managers;
using UnityEngine;
namespace SweetCandy.PrefabsLogic
{
    public class CholeteObject:Collectible
    {
        
        public GameObject hitEffect; //触碰特效
        public GameObject missEffect; //丢失特效
        void FixedUpdate()
        {
            transform.Translate(0, 0, -speed * Time.deltaTime,Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!Global.PointDictionary["getCholo"])
                {
                    EventManager.Instance.TriggerEvent("ShowCholo");
                    Global.PointDictionary["getCholo"] = true;
                }
                GameObjectPool.Instance.CollectObject(gameObject);
                ScoreManager.Instance.Cho();
                AudioManager.Instance.PlaySound("点击");
            }
            if (other.CompareTag("Wall"))
            {
               
                GameObjectPool.Instance.CollectObject(gameObject);
               
                
            }
        }
    }
    
}