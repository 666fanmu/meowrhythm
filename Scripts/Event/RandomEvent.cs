using System.Collections;
using SweetCandy.Basic;
using SweetCandy.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SweetCandy.Event
{
    public class RandomEvent : MonoBehaviour
    {
     
        
        #region Random OBJ

        public GameObject Bomb;
        public GameObject eventBossHand;
        public GameObject ObsGroup;
        public GameObject WaringHand;

        #endregion


        public IEnumerator RandomEventCoroutine()
        {
            while (GameManager.Instance.gameMode == GameMode.Game)
            {
                yield return new WaitForSeconds(Random.Range(6, 8));
                switch (Random.Range(0, 3))
                {
                    case 0:
                        CreatHand();
                        break;
                    case 1:
                        CreatObsGroup();
                        break;
                    case 2:
                        GameObjectPool.Instance.CreateObject("waringHand", WaringHand,
                            new Vector3(0, 0, 0), this.transform.rotation);
                        CreateBombGroup();
                        break;
                }
            }
        }


        #region random event

        public void CreatHand()
        {
            GameObjectPool.Instance.CreateObject("eventBossHand", eventBossHand,
                new Vector3(0, 0, 0), this.transform.rotation);
        }

        public void CreateBombGroup()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            do
            {
                i = Random.Range(0, 5);
                j = Random.Range(0, 5);
                k = Random.Range(0, 5);
            } while (i == j || j == k || i == k);

            CreateBomb(i);
            CreateBomb(j);
            CreateBomb(k);
        }

        private void CreateBomb(int i)
        {
            GameObjectPool.Instance.CreateObject("Bomb", Bomb,
                new Vector3(GameManager.Instance.notePosList[i], 2000, -3500), new Quaternion(0, 0, 0, 0));
        }

        public void CreatObsGroup()
        {
            GameObjectPool.Instance.CreateObject("ObsGroup", ObsGroup,
                new Vector3(0, 0, 0), this.transform.rotation);
        }

        #endregion
    }
}