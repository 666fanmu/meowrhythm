using System;
using System.Collections;
using DG.Tweening;
using SweetCandy.Basic;
using SweetCandy.Managers;
using SweetCandy.PrefabsLogic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SweetCandy.Event
{
    public enum BossState
    {
        Normal,
        Speed,
    }
    public enum ComboEffect
    {
        Miss=0,
        Good=1,
        Perfect=2,
    }
    public class ComboTimeEvent : MonoBehaviour
    {
        #region Include
        public GameObject comboMissPrefab;
        public GameObject comboGoodPrefab;
        public GameObject comboPerfectPrefab;
        public Material darkSky;
        public Material blueSky;
        public GameObject[] RobotEyes;
        [FormerlySerializedAs("RhythmTimeCanvas")] public GameObject comboTimeCanvas;
        public CanvasGroup stateCanvas;
        public RectMask2D bossHp;
        public Slider energyTimeSlider;
        public GameObject[] pickup;//拾取物
        public Transform[] bornPlace;//生成地点
        public Transform comboCanvas;
        public GameObject playerGroup;
        public GameObject ComboTimeObjects;
        private ComboIn _comboIn;
        public ParticleSystem catAttack;
        public bool isBossGGInComboTime = false;
        #endregion

        #region timer

        //拾取物生成时间
        public float setBornTime=1.5f;
        private float _BornTime;
        //旋转时间
        public float setAttackTime=5f;
        private float _AttackTime;
        private float _timer;
        
        #endregion
        public bool isReady=false;
        //public bool isBossGGInComboTime = false;
        
        #region Combo界面Effect

        public void DoComboEffect(ComboEffect ce,Vector2 pos)
        {
            switch (ce)
            {
                case ComboEffect.Miss:
                    GameObjectPool.Instance.CreateObject("ComboMiss", comboMissPrefab, comboCanvas, pos,
                        new Quaternion(0, 0, 0, 0));
                    break;
                case ComboEffect.Good:
                    GameObjectPool.Instance.CreateObject("ComboGood", comboGoodPrefab, comboCanvas, pos,
                        new Quaternion(0, 0, 0, 0));
                    break;
                case ComboEffect.Perfect:
                    GameObjectPool.Instance.CreateObject("ComboPerfect", comboPerfectPrefab, comboCanvas, pos,
                        new Quaternion(0, 0, 0, 0));
                    break;
            }
        }

        #endregion

        private void Awake()
        {
            _comboIn = FindObjectOfType<ComboIn>();
        }

        private void Start()
        {
            comboTimeCanvas.GetComponent<CanvasGroup>().alpha = 0f;
            ComboTimeObjects.SetActive(false);
            energyTimeSlider.value = 1f;
        }

        public void ShowBoss()
        {
            ComboTimeObjects.SetActive(true);
            energyTimeSlider.value = 1f;
            GameManager.Instance.bossState = BossState.Normal;
            RenderSettings.skybox = darkSky;
            _BornTime = setBornTime;
            _AttackTime = setAttackTime;
            //RobotEyes[1].SetActive(true);
            EventManager.Instance.TriggerEvent("CloseGameShow");
            if (!Global.PointDictionary["getCombo"])
            {
                EventManager.Instance.TriggerEvent("ShowComboTime");
                Global.PointDictionary["getCombo"] = true;
            }
            
        }
        public void ComboStart()
        {
            isReady = true;
            
            //ScoreManager.Instance.isRobotBlood = true;
            stateCanvas.DOFade(1, 2f);
            comboTimeCanvas.GetComponent<CanvasGroup>().DOFade(1,2f);
            StartCheck();
            StartCheckEnd();
            StartCheckReturn();
        }

        public void ComboEnd()
        {
            isReady = false;
            _comboIn.Exit();
            GameManager.Instance.bossState = BossState.Normal;
            RenderSettings.skybox = blueSky;
            ScoreManager.Instance.currentEnergy = 30;
            stateCanvas.DOFade(0, 2f);
            comboTimeCanvas.GetComponent<CanvasGroup>().DOFade(0, 2f).OnComplete(() => ComboTimeObjects.SetActive(false));
            StopCoroutine(Check());
            StopCoroutine(CheckBossEnd());
            StopCoroutine(CheckReturn());
            EventManager.Instance.TriggerEvent("OpenGameShow");
            GameManager.Instance.GameModeChange(GameMode.Game);
        }
        private void StartCheck()=>StartCoroutine(Check());
        private void StartCheckEnd() => StartCoroutine(CheckBossEnd());
        private void StartCheckReturn() => StartCoroutine(CheckReturn());
        private void Update()
        {
            if (GameManager.Instance.gameMode == GameMode.ComboTime)
            {
                    if(isReady)
                    {
                            bossHp.padding = new Vector4(0,0,700-ScoreManager.Instance.currentRobotBlood*7,0);
                            _timer += Time.deltaTime;
                            energyTimeSlider.value = Mathf.Lerp(1f,0,_timer/40f);
                            if (_BornTime > 0)
                            {
                                _BornTime -= Time.deltaTime;
                            }
                            else
                            {
                                _BornTime = setBornTime;
                                CreatPickUpsp1();
                                CreatPickUpsp2();
                            }
                            if (_AttackTime > 0)
                            {
                                _AttackTime -= Time.deltaTime;
                            }
                            else
                            {
                                _AttackTime = setAttackTime;
                                catAttack.Play();
                            }
                    }
            }
            else
            {
                isReady = false;
                _timer = 0;
            }
        }

        private IEnumerator Check()
        {
            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(()=>Math.Abs(ScoreManager.Instance.currentRobotBlood %40 - 5) < 1f);
            _comboIn.comboAnimation.Play("BossAngry");
            setBornTime = 1f;
            yield return new WaitForSeconds(2f);
            GameManager.Instance.bossState = BossState.Speed;
            Invoke("CheckSpeedBack",5f);
        }

        private void CheckSpeedBack()
        {
            GameManager.Instance.bossState = BossState.Normal;
            StartCheck();
        }
        private IEnumerator CheckReturn()
        {
            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(()=>energyTimeSlider.value==0);
            ComboEnd();
        }
        private IEnumerator CheckBossEnd()
        {
            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => ScoreManager.Instance.currentRobotBlood <1f);
            comboTimeCanvas.GetComponent<CanvasGroup>().DOFade(0, 1f)
                .OnComplete((() => comboTimeCanvas.SetActive(false)));
            //RobotAnimator.SetBool("BossGG",true);
            //RobotAnimator.Play("BossGG");\
            RobotEyes[1].SetActive(true);
            _comboIn.comboAnimation.Play("BossGG",AnimationPlayMode.Stop);
            
            yield return new WaitForSeconds(2f);
            isBossGGInComboTime = true;

        }
        void Change()
        {
           
        /*    Vector3 endv = PlayerGroup.transform.localEulerAngles + new Vector3(0, 0, 180);
            PlayerGroup.transform.DOLocalRotate(endv, 0.2f).SetUpdate(true);
            /*var movex = PlayerGroup.transform.localPosition.x+Random.Range(0,150);
            if (PlayerGroup.transform.localPosition.x > -250f && PlayerGroup.transform.localPosition.x < 250f)
            {
                PlayerGroup.transform.localPosition.x = Random.Range(-50, 50);
            }*/
       //     PlayerGroup.transform.DOLocalMoveX(Random.Range(-300,300), 0.5f).SetUpdate(true);
        }

        public void CreatPickUpsp1()
        {
            var num = Random.Range(0, 4);
            GameObjectPool.Instance.CreateObject("combopick"+num, 
                pickup[num],comboCanvas,
                bornPlace[0].localPosition,
                new Quaternion(0,0,0,0));
        }
                
        public void CreatPickUpsp2()
        {
            var num = Random.Range(4, 8);
            GameObjectPool.Instance.CreateObject("combopick"+num, 
                pickup[num],comboCanvas,
                bornPlace[1].localPosition,
                new Quaternion(0,0,0,0));
        }
        
    }
}