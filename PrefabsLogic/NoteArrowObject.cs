using System;
using SweetCandy.Basic;
using SweetCandy.Managers;
using UnityEngine;

namespace SweetCandy.PrefabsLogic
{
    public class NoteArrowObject : Collectible
    {

        public GameObject hitEffect; //触碰特效
        public GameObject missEffect; //丢失特效

        void FixedUpdate()
        {
            transform.Translate(0, 0, -speed * Time.deltaTime);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Wall"))
            {
                //this.isCollectedByPlayer = -1;
                ScoreManager.Instance.NoteMissed();
                Collect();
            }
        }

        public override void Collect()
        {
            base.Collect();
        }
    }
}