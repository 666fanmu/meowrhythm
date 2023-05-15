using System;
using System.Collections;
using SweetCandy.Basic;
using SweetCandy.Event;
using SweetCandy.UI.Ctrls;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SweetCandy.Managers
{
    public enum RiskLevel
    {
        low,
        normal,
        high,
    }

    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        private GameShowCtrl GameShowCtrl;
        private float _timer;

        #region energy

        private int setEnergy = 30; //初始能量
        public  int currentEnergy; //当前能量
        public int setCosEn = 1; //初始每秒消耗能量
        private float setEnTime = 1;
        private float EnTime;
        public bool isCostEn = true;

        #endregion

        #region risk level

        public RiskLevel riskLevel;
        private float setRiskLevel = 0;

        ///<summary>当前危险难度</summary>
        public  float currentRiskLevel;

        public int setBoundary1 = 25;
        public int setBoundary2 = 75;
        private float setRiskTime = 2.5f;
        private float RiskTime;
        public bool isRiskLevel = false;

        #endregion

        #region Robot Blood Volume

        public Image MaxImage;
        public Image fillImage;
        public Text text;

        private int setRobotBlood = 100;
        public float currentRobotBlood;
        private float setBloodTime = 5f;
        private float bloodTime;
        public bool isRobotBlood = false;

        #endregion


        #region score数据

        public int hits; //击中次数
        public int missHits; //丢失次数
        public int multiplierTracker;
        public int[] multiplierThresholds;
        public  int currentScore; //当前分数
        public int scorePerNote=1; //每个音符分数
        public int currentMultiplier; //当前倍率
       
        public int finalRank;
        float baifenbi = 0;

        #endregion

        #region time数据

        public float setBombTime = 5f;
        public float bombTime;
        public bool isGetDisco = false; //拾取音乐球
        public bool isbomb = false; //被炸了
        float setDiscTime = 5;
        private float DiscoTime;
        float _t;
        
        

        #endregion


     
        
        

        protected override void Init()
        {
            GameShowCtrl = FindObjectOfType<GameShowCtrl>();
            
        }

        private void Start()
        {
            RiskTime = setRiskTime;
            currentEnergy = setEnergy;
            currentRiskLevel = setRiskLevel;
            EnTime = setEnTime;
            bloodTime = setBloodTime;
            currentRobotBlood = setRobotBlood;
         
         
            StartCheckCombo();
            StartCheckEnd();
        }

        private void StartCheckCombo() => StartCoroutine(CheckCombo());
        private void StartCheckEnd() => StartCoroutine(CheckEnd());
         void FixedUpdate()
        {
            
            if (currentRiskLevel > setBoundary1 && currentRiskLevel < setBoundary2 && riskLevel != RiskLevel.normal)
            {
                riskLevel = RiskLevel.normal;
            }
            else if (currentRiskLevel <= setBoundary1 && riskLevel != RiskLevel.low)
            {
                riskLevel = RiskLevel.low;
            }
            else if (currentRiskLevel >= setBoundary2 && riskLevel != RiskLevel.high)
            {
                riskLevel = RiskLevel.high;
            }
            
        }
         
         IEnumerator CheckCombo()
         {
             yield return new WaitForSeconds(0.1f);
             yield return new WaitUntil((() => currentEnergy >= 100));
             yield return new WaitUntil((() => GameManager.instance.gameMode == GameMode.Game));
             GameManager.instance.GameModeChange(GameMode.ComboTime);
             Invoke("StartCheckCombo",3f);
         }
         
         IEnumerator CheckEnd()
         {
             yield return new WaitForSeconds(0.1f);
             yield return new WaitUntil((() => currentRiskLevel >= 100));
             GameManager.Instance.GameModeChange(GameMode.End);
         }
        private void Update()
        {

            GameShowCtrl.RefreshEnergyShow(currentEnergy);
            GameShowCtrl.RefreshDangerShow(currentRiskLevel);
            _timer += Time.deltaTime;
            if (currentScore < 0)
            {
                currentScore = 0;
            }
            
            if (currentRiskLevel < 0)
            {
                currentRiskLevel = 0;
            }

            if (currentRobotBlood>=100)
            {
                currentRobotBlood = 100;
            }
            //每秒增加危险值
            if (isRiskLevel)
            {
                if (RiskTime > 0)
                {
                    RiskTime -= Time.deltaTime;
                }
                else
                {
                    currentRiskLevel= Mathf.Lerp(currentRiskLevel, currentRiskLevel + 1f, _timer / 1f);
                    RiskTime = setRiskTime;
                }
            }

            //每秒boss扣血
            /*if (isRobotBlood)
            {
                currentRobotBlood= Mathf.Lerp(currentRobotBlood, currentRobotBlood - 1f, _timer / 1000f);
            }*/
            
            if (currentEnergy >= 100)
            {
                currentEnergy = 100;
            }
            else if (currentEnergy < 30)
            {
                currentEnergy = 30;
            }
            
            if (isGetDisco)
            {
                if (DiscoTime >= 0)
                {
                    DiscoTime -= Time.deltaTime;
                }
                else
                {
                    isGetDisco = false;

                    DiscoTime = setDiscTime;
                }
            }

            if (isbomb)
            {
                if (bombTime >= 0)
                {
                    bombTime -= Time.deltaTime;
                    currentMultiplier = 0;
                }
                else
                {
                    currentMultiplier = 1;
                    currentEnergy = (int)Mathf.Round((float)(currentEnergy * 0.7));
                    bombTime = setBombTime;
                    isbomb = false;
                }
            }
            
        }
        public void changeCostMode(bool i)
        {
            isCostEn = i;
            isRiskLevel = i;
        }
        
        public void NoteHit()
        {
            if (!isGetDisco)
            {
                currentEnergy += 2;
            }
            else
            {
                currentEnergy += 4;
            }
        }
        
        public void Hit()
        {
            currentScore += scorePerNote * currentMultiplier;
            NoteHit();
            hits++;
        }

        public void DiscoHit() //get disco ball
        {
            isGetDisco = true;
         //   GameShowCtrl.RefreshMulti(currentMultiplier);
        }

        public void Microphone() //加分数
        {
            getChange(0,-10,-10,10);
            //GameShowCtrl.RefreshScore(currentScore);
        }
        
        public void Cho() //get 巧克力加能量
        {
            getChange(10,0,0,5);
            GameShowCtrl.RefreshEnergyShow(currentEnergy);
        }


        //debuff hit
        public void NoteMissed()
        {
            currentMultiplier = 1;
            multiplierTracker = 0;
         //   GameShowCtrl.RefreshMulti(currentMultiplier);
            missHits++;
        }
        
        public void LaserHit() //激光
        {
            currentScore -= 10;
            //GameShowCtrl.RefreshScore(currentScore);
            currentMultiplier = 1;
            multiplierTracker = 0;
          //  GameShowCtrl.RefreshMulti(currentMultiplier);
        }
        
        public void ComBoHit(ComboEffect comboEffect)
        {
            switch (comboEffect)
            {
                case ComboEffect.Miss :
                   break;
                case ComboEffect.Good:
                    getChange(0,0,-1,3);
                    
                    break;
                case ComboEffect.Perfect:
                    getChange(0,0,-3,10);
                  
                    break;
            }
            
        }
        
        public void GetFinalRank()
        {
            finalRank = JudgeRank();
        }

        private int JudgeRank()
        {
            baifenbi = 100 * ((float)hits / ((float)hits + (float)missHits + 1));

            if (baifenbi >= 90)
            {
                return 0;
            }
            else if (80 <= baifenbi && baifenbi < 90)
            {
                return 1;
            }
            else if (baifenbi >= 70 && baifenbi < 80)
            {
                return 2;
            }
            else if (baifenbi >= 60 && baifenbi < 70)
            {
                return 3;
            }
            else if (baifenbi >= 50 && baifenbi < 60)
            {
                return 4;
            }
            else if (baifenbi >= 40 && baifenbi < 50)
            {
                return 5;
            }
            else if (baifenbi >= 30 && baifenbi < 40)
            {
                return 6;
            }
            else
            {
                return 7;
            }
        }

        public void getChange(int Energy, int Risk, int Blood, int Score)
        {
            currentEnergy += Energy;
            currentRiskLevel += Risk;
            StartCoroutine(changeBossBlood(Blood));
            currentScore += Score;
            GameShowCtrl.RefreshEnergyShow(currentEnergy);
        }

        public IEnumerator changeBossBlood(float i)
        {
            float y;
            y = 0;
            while (Mathf.Abs(y)<Mathf.Abs(i))
            {
                yield return new WaitForSeconds(0.01f);
                currentRobotBlood += i/50;
                y += i / 50;

            }
            StopCoroutine(changeBossBlood(0));
        }
        
    }
}