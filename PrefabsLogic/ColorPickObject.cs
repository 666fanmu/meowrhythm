using System;
using System.Collections;
using System.Collections.Generic;
using SweetCandy.Basic;
using SweetCandy.Event;
using UnityEngine;
using SweetCandy.Managers;
using Random = UnityEngine.Random;

namespace SweetCandy.PrefabsLogic
{
    public class ColorPickObject : Collectible
    {
        public int ColorNum;
        protected PlayerManager _PlayerManager;
        protected ColorTimeEvent _ColorTimeEvent;
        protected LocationState PlaceNum;
        GameObject hitEffect;
        private void Awake()
        {
            _PlayerManager = GameObject.FindObjectOfType<PlayerManager>();
            _ColorTimeEvent = GameObject.FindObjectOfType<ColorTimeEvent>();
        }

        public override void onReset()
        {
            speed = Random.Range(800, 1200);
        }

        void FixedUpdate()
        {
            transform.Translate(0, 0, -speed * Time.deltaTime);
            CheckoutMode(GameMode.ColorTime,true);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //Debug.Log(_ColorTimeEvent.ColorTimeScore);
                _ColorTimeEvent.colorTimeScore += _ColorTimeEvent.CheckColor(ColorNum);
                
                if (_ColorTimeEvent.CheckColor(ColorNum) > 0)
                {
                    ScoreManager.Instance.getChange(3,0,0,5);
                    AudioManager.Instance.PlaySound("点击");
                    Collect();
                    Vector3 hitEffectPosition =
                        new Vector3(transform.position.x, transform.position.y, transform.position.z - 500);
                    GameManager.Instance.HitEffect("NoteEffect", hitEffect, hitEffectPosition);
                }
            }


            if (!ScoreManager.Instance.isGetDisco)
            {
                if (other.CompareTag("Wall"))
                {
                    Collect();
                }
            }
        }
    }
}