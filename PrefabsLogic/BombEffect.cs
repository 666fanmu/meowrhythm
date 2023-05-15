using System;
using SweetCandy.Managers;
using UnityEngine;

namespace SweetCandy.PrefabsLogic
{
    public class BombEffect : MonoBehaviour
    {
        private ParticleSystem ps;

        private void Awake()
        {
            ps = this.gameObject.GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            EventManager.Instance.StartListening("Bomb",Bomb);
        }
        
        private void Bomb()
        {
            ps.Play();
        }
    }
}