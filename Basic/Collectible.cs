using SweetCandy.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SweetCandy.Basic
{
    public class Collectible : MonoBehaviour, IResetable
    {
        public int isCollectedByPlayer = 0;
        public float speed = 1300f;

        public virtual void Collect()
        {
            GameObjectPool.Instance.CollectObject(gameObject, 0f);
        }

        ///<summary>true判断不是mode回收，false判断是mode回收</summary>
        public void CheckoutMode(GameMode mode, bool i)
        {
            if (i)
            {
                if (GameManager.Instance.gameMode != mode)
                {
                    Collect();
                }
            }
            else
            {
                if (GameManager.Instance.gameMode == mode)
                {
                    Collect();
                }
            }
        }


        public virtual void onReset()
        {
            speed = Random.Range(1200, 1400);
        }
    }
}