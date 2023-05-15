using SweetCandy.Basic;
using UnityEngine;

namespace SweetCandy.PrefabsLogic
{
    public class WaringHand : Collectible
    {
        private Animation animation;
        private void Start()
        {
            animation = this.gameObject.GetComponent<Animation>();
            animation.Play("WaringHand");
            GameObjectPool.Instance.CollectObject(gameObject, 5f);
        }
    }
}
