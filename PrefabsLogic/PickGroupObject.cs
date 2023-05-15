using System;
using SweetCandy.Managers;
using UnityEngine;
using Random = UnityEngine.Random;


namespace SweetCandy
{
    public class PickGroupObject : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Awake()
        {
            foreach(Transform child in this.transform)
            {
                child.transform.position = new Vector3(GameManager.Instance.notePosList[Random.Range(0, 5)],child.transform.position.y,child.transform.position.z);
            }
        }
    }
}
