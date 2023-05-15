using System;
using System.Collections;
using DG.Tweening;
using SweetCandy.Basic;
using UnityEngine;
using SweetCandy.Managers;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace SweetCandy.Event
{
    public class ColorTimeEvent : MonoBehaviour
    {
        private RoadColorEvent _roadColorEvent;
        public GameObject[] notePrefabs;
        public int colorTimeScore;
        public GameObject obsPrefab;
        private IsCatchingEvent _isCatchingEvent;
        private CatchCatEvent _catchCatEvent;
        #region Time

        //colorPickBornTime
        private float setBornTime = 2f;
        public float SetBornTime
        {
            get => setBornTime;
            set => setBornTime = value;
        }
        private float borntime;
        
        //obs born time
        private float setObsTime = 0.5f;
        private float obsTIme;

        #endregion
        
        private void Awake()
        {
            _roadColorEvent = FindObjectOfType<RoadColorEvent>();
            _isCatchingEvent = FindObjectOfType<IsCatchingEvent>();
            _catchCatEvent = FindObjectOfType<CatchCatEvent>();
        }

        private void Start()
        {
            colorTimeScore = 0;
            borntime = SetBornTime;
            obsTIme = setObsTime;
            StartCoroutine(CheckChangeMode());
        }


        private void FixedUpdate()
        {
           
            if (GameManager.Instance.gameMode == GameMode.ColorTime)
            {
                if (ScoreManager.Instance.currentEnergy > 95)
                {
                    ScoreManager.Instance.currentEnergy = 95;
                }
                if (borntime >= 0)
                {
                    borntime -= Time.deltaTime;
                }
                else
                {
                    CreatColorPick();
                    borntime = SetBornTime;
                }
            }
        }

        //检测分数切换模式
        private IEnumerator CheckChangeMode()
        {
            yield return new WaitUntil(() => colorTimeScore >= 100);
            colorTimeScore = 0;
            _isCatchingEvent.catsInRoad[_isCatchingEvent.whichBeCatched].SetActive(false);
            _catchCatEvent.catsInCar[_isCatchingEvent.whichBeCatched].SetActive(true);
            _isCatchingEvent.car.transform.DOMove(new Vector3(0, -1855, -3500), 0.5f);
            _roadColorEvent.RoadColorOut();
            GameManager.Instance.GameModeChange(GameMode.Game);
        }

        public int CheckColor(int ColorNum)
        {
            if (ColorNum == _roadColorEvent.GetCurrentColorModel())
            {
               // Debug.Log( _roadColorEvent.GetCurrentColorModel());
                return 7;
            }
            else
            {
                return 0;
            }
        }

        private void CreatColorPick()
        {
         //   Debug.Log("color born");
            int num = Random.Range(0, 5);
            var notePrefab = notePrefabs[num];
            GameObjectPool.Instance.CreateObject("NoteColor" + num, notePrefab,
                new Vector3(GameManager.Instance.notePosList[Random.Range(1, 4)], GameManager.Instance.objPos.y,
                    GameManager.Instance.objPos.z), notePrefab.transform.rotation);
        }
    }
}