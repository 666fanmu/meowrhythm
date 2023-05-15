using System;
using SweetCandy.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SweetCandy.PrefabsLogic
{
    public class NoteGroupObject : MonoBehaviour
    {
        public GameObject[] GroupObjects; //y=375
        private int[] _height=new []{0,375};
        private void Awake()
        {
            
            for (int i = 0; i < GroupObjects.Length; ++i)
            {
                float rand = Random.Range(0f, 100f);
                GroupObjects[i].transform.localPosition = new Vector3(GameManager.Instance.notePosList[Random.Range(0, 5)],
                     _height[rand>90?1:0], GroupObjects[i].transform.position.z);
            }
            /*foreach(Transform child in this.transform)
            {
                child.transform.position = new Vector3(GameManager.Instance.notePosList[Random.Range(0, 5)],child.transform.position.y,child.transform.position.z);
            }*/

        }
    }
}