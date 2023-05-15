using System;
using SweetCandy.Basic;
using SweetCandy.Managers;
using Unity.VisualScripting;
using UnityEngine;
namespace SweetCandy.PrefabsLogic
{
    public class DiscoObject:Collectible
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
                ScoreManager.Instance.DiscoHit();
                if (!Global.PointDictionary["getDisco"])
                {
                    EventManager.Instance.TriggerEvent("ShowDisco");
                    Global.PointDictionary["getDisco"] = true;
                }
                Collect();
            }
            if (other.CompareTag("Wall"))
            {
                Collect();
            }
        }
    }
    
}