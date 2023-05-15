using System.Collections.Generic;
using SweetCandy.Basic;
using SweetCandy.Event;
using SweetCandy.Settings;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

namespace SweetCandy.Managers
{
    
    public enum ObjName //在此枚举中添加新道具名
    {
        Note,
        Obs,
        Mikphone,
        Chocolate,
        Discoball,
    }

    public enum GameMode
    {
        Game, //游戏界面
        Guide, //Guide模式
        BeCatched, //被抓了
        ComboTime, //Rhythm已改名ComboTime
        ColorTime, //变色时刻
        Waiting,
        Talk,
        End,
        Win,
    }

    public class GameManager : MonoSingleton<GameManager>
    {
        #region Prefabs

        public Animation playerAnimation;
        public GameObject noteGroupPrefab;
        public GameObject obsPrefab;
        public GameObject Mikphone;
        public GameObject Chocolete;
        public GameObject DiscoBall;

        public GameObject[] levelPickGroup; //除音符外的拾取物

        #endregion

        #region Include

        private IsCatchingEvent _isCatchingEvent;
        private CatchCatEvent _catchCatEvent;
        private GlobalSetting _globalSetting;
        private PlayerManager _playerManager;
        private ComboTimeEvent _comboTimeEvent;
        private DialogueEvent _dialogueEvent;
        private CameraCtrl _cameraCtrl;
        private RoadColorEvent _roadColorEvent;
        private RandomEvent _randomEvent;
        #endregion

        #region Enum

        public GameMode gameMode;
        public BossState bossState;

        #endregion

        #region Panel

        public GameObject gameMenu;
        public GameObject settingMenu;

        #endregion

        #region Bool

        public bool ifCatchCat = false;
        private bool _gameOver = false;
        public bool isRain =false;

        #endregion

        #region Pos

        public Vector3 noteGroupPos = new Vector3(-2114, -6260, -3837);
        public List<int> notePosList = new List<int> { -896, -443, 0, 443, 896 };
        public Vector3 objPos;

        #endregion

        #region timer

        private float setGameTime = 300f; //五分钟标准游戏时间

        public float SetGameTime
        {
            get => setGameTime;
            set => setGameTime = value;
        }

        private float setNextTime = 30f;
        public float SetNextTime //能量条满进入   时间
        {
            get => setNextTime;
            set => setNextTime = value;
        }

        private float setWaitingTime = 10f;
        public float SetWaitingTime
        {
            get => setWaitingTime;
            set => setWaitingTime = value;
        }

        private float CatchTime;
        private float setCatchTime = 5f;

        private float setRhythemTime = 50f;
        private float RhythemTime;


        private float _timer;
        //效果拾取物
        public float setPickGroupTime = 25f;
        public float PickGroupTime;

        public float _collectTimer;

        public float setCollectTime=7f;
        //音符
        public float setNoteGroupTime = 4f;
        public float NoteGroupTime;
        //beCatChing阶段生成障碍物
        private float setObsTime = 0.1f;
        private float ObsTime=0;
        private int whichCollect;
        #endregion
        
        protected override void Init()
        {
#if UNITY_EDITOR
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Test"))
            {
                return;
            }
#endif
            //find 
            playerAnimation = GameObject.Find("Player").GetComponent<Animation>();
            _isCatchingEvent = FindObjectOfType<IsCatchingEvent>();
            _cameraCtrl = FindObjectOfType<CameraCtrl>();
            _globalSetting = FindObjectOfType<GlobalSetting>();
            _playerManager = FindObjectOfType<PlayerManager>();
            _comboTimeEvent = FindObjectOfType<ComboTimeEvent>();
            _dialogueEvent = FindObjectOfType<DialogueEvent>();
            _catchCatEvent = FindObjectOfType<CatchCatEvent>();
            _roadColorEvent = FindObjectOfType<RoadColorEvent>();
            _randomEvent = FindObjectOfType<RandomEvent>();
            SaveManager.Instance.Load();
            _globalSetting.SetVolume();
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game"))
            {
                CreateNoteGroupWait();
                GameModeChange(GameMode.Game);
                AudioManager.Instance.PlayMusic("游戏音乐");
            }
        }

        

        public void GameModeChange(GameMode newGameMode)
        {
            switch (newGameMode)
            {
                case GameMode.Game:
                    Debug.Log("Game");
                    _cameraCtrl.ToNormalCam();
                    ScoreManager.instance.changeCostMode(true);
                    Time.timeScale = 1f;
                    _playerManager.ChangeMoveMode(MoveMode.Normal);
                    gameMode = GameMode.Game;
                    ObsTime = setObsTime;
                    PickGroupTime = 4f;
                    CatchTime = setCatchTime;
                    StartCoroutine(_randomEvent.RandomEventCoroutine());
                    break;
                
                case GameMode.BeCatched:
                    Time.timeScale = 1f;
                    _cameraCtrl.ToBeCatchedCam();
                    ScoreManager.Instance.changeCostMode(false);
                    _playerManager.ChangeMoveMode(MoveMode.CatCatched);
                    gameMode = GameMode.BeCatched;
                    break;
                
                case GameMode.ColorTime:
                    EventManager.Instance.TriggerEvent("CloseGameShow");
                    _cameraCtrl.ToHighCam();
                    _playerManager.ChangeMoveMode(MoveMode.Color);
                    _roadColorEvent.RoadColorIn();
                    gameMode = GameMode.ColorTime;
                    //TODO ColorTime能量和音效管理
                    break;
                
                case GameMode.ComboTime:
                    ScoreManager.Instance.currentEnergy = 90;
                    Time.timeScale = 1;
                    ScoreManager.Instance.changeCostMode(false);
                    _cameraCtrl.ToBossFightCam();
                    _playerManager.ChangeMoveMode(MoveMode.ComboTime);
                    gameMode = GameMode.ComboTime;
                    _comboTimeEvent.ShowBoss();
                    break;
                
                case GameMode.End:
                    gameMode = GameMode.End;
                    _playerManager.enabled = false;
                    playerAnimation.Play("PlayerGG", AnimationPlayMode.Stop);
                    ScoreManager.Instance.changeCostMode(false);
                    Invoke("GameOver",2.5f);
                    ScoreManager.instance.currentEnergy = 0;
                    ScoreManager.instance.currentRiskLevel = 0;
                    break;
                
                case GameMode.Win:
                    gameMode = GameMode.Win;
                    _playerManager.enabled = false;
                    //加入动画
                    GameOver();
                    ScoreManager.Instance.changeCostMode(false);
                    ScoreManager.instance.currentEnergy = 0;
                    ScoreManager.instance.currentRiskLevel = 0;
                    break;
                
                case GameMode.Waiting:
                    ScoreManager.Instance.changeCostMode(false);
                    //Time.timeScale = 1;
                    _playerManager.ChangeMoveMode(MoveMode.Wait);
                    gameMode = GameMode.Waiting;
                    break;
                
                case GameMode.Talk:
                    Time.timeScale = 0.1f;
                    break;
            }
        }

        private void GameOver()
        {
            EventManager.Instance.TriggerEvent("OpenResultMenu");
            //GameObjectPool.Instance.ClearAll();
            AudioManager.Instance.PlayMusic("结算");
        }
        private void Update()
        {
            //调试快捷键
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameModeChange(GameMode.ComboTime);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                EventManager.Instance.TriggerEvent("CatchCat");
                GameModeChange(GameMode.Waiting);
                DisPlayerManager();
            }
#endif

            #region 菜单栏呼出

            if (Input.GetKeyDown(KeyCode.Escape) && gameMode != GameMode.Guide && gameMode != GameMode.End)
            {
                if (gameMenu.GetComponent<CanvasGroup>().alpha == 0)
                    EventManager.Instance.TriggerEvent("OpenMenu");
                else if (!settingMenu.activeInHierarchy)
                    EventManager.Instance.TriggerEvent("CloseMenu");
            }

            #endregion
           

            if (gameMode == GameMode.Game)
            {
                if (!GetIsCatching())
                {
                    if (ScoreManager.Instance.currentRiskLevel == 25)
                    {
                        EventManager.Instance.TriggerEvent("CatchCat");
                        DisPlayerManager();
                    }
                    else if (ScoreManager.Instance.currentRiskLevel == 50)
                    {
                        EventManager.Instance.TriggerEvent("CatchCat");
                        DisPlayerManager();
                    }
                    else if (ScoreManager.Instance.currentRiskLevel == 75)
                    {
                        EventManager.Instance.TriggerEvent("CatchCat");
                        DisPlayerManager();
                    }
                }
            }
        }


        #region 外部传参

        public void DisPlayerManager() => _playerManager.enabled = false;

        public void EnablePlayerManager(float delay=0)
        {
            Invoke("Enpl",delay);
        }

        private void Enpl()=>_playerManager.enabled = true;
        public bool GetIsCatching()
        {
            return _catchCatEvent.animator.GetBool("IsCatching");
        }

        public bool[] GetIsCatched()
        {
            return _catchCatEvent.isCatched;
        }

        public void ChangeWhichBeCatched(int which)
        {
            _playerManager.whichBeCatched = which;
            _isCatchingEvent.whichBeCatched = which;
        }

        #endregion

        private void FixedUpdate()
        {
            //game场景生成代码
            if (gameMode == GameMode.Game)
            {
                if (_collectTimer >= 0)
                {
                    _collectTimer -= Time.deltaTime;
                }
                else
                {
                    whichCollect= Random.Range(0, 3);
                    switch (whichCollect)
                    {
                        case 0:
                            CreateObject(ObjName.Discoball);
                            break;
                        case 1:
                            CreateObject(ObjName.Chocolate);
                            break;
                        case 2:
                            CreateObject(ObjName.Mikphone);
                            break;
                    }
                    CreateObs();
                    CreateObs();
                    _collectTimer = setCollectTime;
                }
                //拾取物的生成
                if (PickGroupTime >= 0)
                {
                    PickGroupTime -= Time.deltaTime;
                }
                else
                {
                    switch (ScoreManager.instance.riskLevel)
                    {
                        case RiskLevel.low:
                            CreatPickGroup(0);
                            break;
                        case RiskLevel.normal:
                            CreatPickGroup(1);
                            break;
                        case RiskLevel.high:
                            CreatPickGroup(2);
                            break;
                    }

                    PickGroupTime = setPickGroupTime;
                }

                //音符的生成
                if (NoteGroupTime >= 0)
                {
                    NoteGroupTime -= Time.deltaTime;
                }
                else
                {
                    CreateNoteGroup();
                    CreateObs();
                    NoteGroupTime = setNoteGroupTime;
                }
            }
            else if (gameMode==GameMode.BeCatched)
            {
                if (ObsTime>=0)
                {
                    ObsTime -= Time.deltaTime;
                }
                else
                {
                    CreateObs();
                    ObsTime = setObsTime;
                }
            }
            
            
        }

        /// <summary>
        /// 已封装内部创建道具方法，外部请使用此方法创建道具
        /// </summary>
        /// <param name="objName">创建的道具名</param>
        public void CreateObject(ObjName objName)
        {
            switch (objName)
            {
                case ObjName.Obs:
                    CreateObs();
                    break;
                case ObjName.Mikphone:
                    CreatMikphone();
                    break;
                case ObjName.Chocolate:
                    CreatChocolate();
                    break;
                case ObjName.Discoball:
                    CreatDiscoBall();
                    break;
            }
        }

        #region Game场景

        public void CreateNoteGroup()
        {
            GameObjectPool.Instance.CreateObject("NoteGroupObj", noteGroupPrefab,
                new Vector3(0, -1516, 0), new Quaternion(0, 0, 0, 0));
        }

        public void CreateNoteGroupWait()
        {
            GameObjectPool.Instance.CreateObject("NoteGroupObj", noteGroupPrefab,
                noteGroupPos, new Quaternion(0, 0, 0, 0));
        }

        private void CreatPickGroup(int i)
        {
            GameObjectPool.Instance.CreateObject("PickGroup", levelPickGroup[i],
                new Vector3(0, -1516, 0), new Quaternion(0, 0, 0, 0));
        }

        private void CreateObs()
        {
            GameObjectPool.Instance.CreateObject("ObsObj", obsPrefab,
                new Vector3(notePosList[Random.Range(0, 5)], objPos.y, objPos.z), new Quaternion(0, 0, 0, 0));
        }

        private void CreatMikphone()
        {
            GameObjectPool.Instance.CreateObject("Mikphone", Mikphone,
                new Vector3(notePosList[Random.Range(0, 5)], objPos.y, objPos.z), Quaternion.Euler(-90, 0, 0));
        }

        private void CreatChocolate()
        {
            GameObjectPool.Instance.CreateObject("Chocolate", Chocolete,
                new Vector3(notePosList[Random.Range(0, 5)], objPos.y, objPos.z), Quaternion.Euler(-90, 0, 0));
        }

        private void CreatDiscoBall()
        {
            GameObjectPool.Instance.CreateObject("DiscoBall", DiscoBall,
                new Vector3(notePosList[Random.Range(0, 5)], objPos.y, objPos.z), Quaternion.Euler(-90, 0, 0));
        }

        public void HitEffect(string keyEffect, GameObject hitEffect, Vector3 position)
        {
            var generatedHitEffect =
                GameObjectPool.instance.CreateObject(keyEffect, hitEffect, position, new Quaternion(0, 0, 0, 0));
            GameObjectPool.Instance.CollectObject(generatedHitEffect, 2f);
        }

        #endregion

    }
}