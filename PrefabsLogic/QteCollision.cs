using System;
using UnityEngine;

namespace SweetCandy.PrefabsLogic
{
    public class QteCollision : MonoBehaviour
    {
        public bool CanPressDown=false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Circle"))
            {
                this.CanPressDown = true;
                Debug.Log("InTrigger"+this.CanPressDown);
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Circle"))
            {
                this.CanPressDown = false;
                Debug.Log("OutTrigger"+this.CanPressDown);
            }
        }
    }
}