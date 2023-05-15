
using SweetCandy.Basic;
using SweetCandy.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SweetCandy.PrefabsLogic
{
    public class FastHandObject : Collectible
    {
        // Start is called before the first frame update

        public GameObject hitEffect; //触碰特效
        private Animation animation;
        public Animation playerAnimation;

        private void Awake()
        {
            animation = this.gameObject.GetComponent<Animation>();
            playerAnimation = GameObject.Find("Player").GetComponent<Animation>();
        }

        private void OnEnable()
        {
            animation.Play("EventBossHand"+Random.Range(0,3));
            GameObjectPool.Instance.CollectObject(gameObject, 6f);
        }

        private void FixedUpdate()
        {
            CheckoutMode(GameMode.Game,true);
        }

        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.CompareTag("Player"))
            {
                Debug.Log("hand get player");
                AudioManager.Instance.PlaySound("钩爪进入音效");
                ScoreManager.Instance.getChange(-10, 10, 5, -10);
                GameManager.Instance.isRain = true;
                Invoke("ReturnRain", 10f);
                GameManager.Instance.DisPlayerManager();
                playerAnimation.Play("Reverse");
                GameManager.Instance.EnablePlayerManager(2f);
                Vector3 hitEffectPosition =
                    new Vector3(0,-1855,-3500);
                 //GameManager.Instance.HitEffect("HitEffect", hitEffect,hitEffectPosition);
                 GameObjectPool.Instance.CollectObject(gameObject, 1f);
                 
                 if (!Global.PointDictionary["getHand"])
                 {
                     EventManager.Instance.TriggerEvent("ShowHand");
                     Global.PointDictionary["getHand"] = true;
                 }
            }

            /*if (other.CompareTag("Wall"))
            {
                ScoreManager.Instance.NoteMissed();
            }*/
        }
        private void ReturnRain()
        {
            GameManager.Instance.isRain = false;
            EventManager.Instance.TriggerEvent("ShowBackRain");
        }
    }
}