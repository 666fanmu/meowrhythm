using System.Collections;
using System.Collections.Generic;
using SweetCandy.Basic;
using SweetCandy.Managers;
using SweetCandy.PrefabsLogic;
using SweetCandy.UI.Ctrls;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SweetCandy.Event
{
    
    public class GuideEvent : MonoBehaviour
    {
        public Dictionary<string, string> GuideTextDic = new Dictionary<string, string>()
        {
            {"Space","按Space收到"},
            {"欢迎1","欢迎来到节奏猫猫！"},
            {"欢迎2","你和你的朋友不小心踏上了机器世界，而我会告诉你们这个世界的规则，帮助你们逃出这个机械世界！"},
            {"引言","音符是这个世界代码运行的能量来源，后面的路上中尽可能多的收集音符吧。" },
            {"第一个音符", "快看前面有一个音符！\n一只猫猫按住<color=green><b>D/→</b></color>就可移动到较近的轨道上！\n快试试吧！" },
            {"吃到第一个音符","太棒了！\n松开按键就会自动回到中间哦！"},
            {"没吃到第一个音符","不可以一起按住或是太着急松开哦，会吃不到的！"},
            {"第二个音符","两只猫猫一起按住<color=green><b>D和→</b></color>试试看！"},
            {"吃到第二个音符","如果向左，也是同理的按<color=green><b>A和←</b></color>哦!\n松开<color=blue><b>D和→</b></color>快试试向左吧！"},
            {"没吃到第二个音符","下次可以注意哦，不要着急喵！"},
            {"第一个跳跃音符","有时有些音符会在空中，\n需要两只猫猫同时按下<color=green><b>W和↑</b></color>就可以跳跃吃到音符！"},
            {"吃到第一个跳跃音符","太棒了！"},
            {"没吃到第一个跳跃音符","注意观察音符的高度哦！"},
            {"第一个休止符","小心！那是一个休止符，不要碰到哦！"},
            {"吃到第一个休止符","若吃到休止符将增加玩家危险度！"},
            {"没吃到第一个休止符","干的漂亮！\n若吃到休止符将增加你们的危险度！"},
            {"能量条","画面左上角上方是你们的危险度若过低将会发生不好的事情！\n吃到音符可补充下方的能量条，当能量条满时就可以召唤**！"},
            {"拾取物","这个世界里还会出现许多神奇的小道具，\n尝试发现它们的效果" },
            {"开始游戏","好了！\n你已经了解\n这个世界的基本程序组成。\n现在\n开始你的节奏猫猫之旅吧！"},
        };
        private GuideMenuCtrl guideMenuCtrl;
        public List<Collectible> Collectibles=new List<Collectible>();
        private PlayerManager PlayerManager;
        public GameObject[] vcams;
        public Animator VcamAnimator;
        public GameObject blackCanvas;
        private void Awake()
        {
            guideMenuCtrl = FindObjectOfType<GuideMenuCtrl>();
            PlayerManager = FindObjectOfType<PlayerManager>();
            foreach (var variablCollectible in Collectibles)
            {
                variablCollectible.speed = 0f;
            }
        }

        private void Start()
        {
            AudioManager.Instance.PlaySound("QTE1");
            AudioManager.Instance.PlayMusic("教学关demo");
            GameManager.Instance.gameMode = GameMode.Guide;
            StartCoroutine(StartGuideAsync());
        }

        private IEnumerator StartGuideAsync()
        {
            yield return new WaitForSeconds(2f);
            vcams[1].SetActive(true);
            blackCanvas.SetActive(false);
            guideMenuCtrl.ChangeGuideInfo(GuideTextDic["欢迎1"],"");
            EventManager.Instance.TriggerEvent("OpenGuideMenu");
            yield return new WaitForSecondsRealtime(1.5f);
            guideMenuCtrl.ChangeGuideInfo(GuideTextDic["欢迎2"],"");
            yield return new WaitForSecondsRealtime(3f);
            EventManager.Instance.TriggerEvent("CloseGuideMenu");
            foreach (var variablCollectible in Collectibles)
            {
                variablCollectible.speed = 1000f;
            }
            guideMenuCtrl.ChangeGuideInfo(GuideTextDic["引言"],GuideTextDic["Space"]);
            EventManager.Instance.TriggerEvent("OpenGuideMenu");
            yield return StartCoroutine(CheckPageIsClosed());
            yield return new WaitForSeconds(2f);
            guideMenuCtrl.ChangeGuideInfo(GuideTextDic["第一个音符"],"按住<color=green><b>D或→</b></color>");
            EventManager.Instance.TriggerEvent("OpenGuideMenu");
            yield return StartCoroutine(CheckPos(0.3f,LocationState.Right));
            yield return StartCoroutine(CheckNote(0));
            yield return new WaitForSeconds(1f);
            guideMenuCtrl.ChangeGuideInfo(GuideTextDic["第二个音符"],"同时按住<color=green><b>D和→</b></color>");
            EventManager.Instance.TriggerEvent("OpenGuideMenu");
            yield return StartCoroutine(CheckPos(0.3f,LocationState.RightMax));
            yield return StartCoroutine(CheckNote(1));
            yield return new WaitForSeconds(3.5f);
            guideMenuCtrl.ChangeGuideInfo(GuideTextDic["第一个跳跃音符"],"同时按下<color=green><b>W和↑</b></color>");
            EventManager.Instance.TriggerEvent("OpenGuideMenu");
            yield return StartCoroutine(CheckJump());
            yield return StartCoroutine(CheckNote(2));
            yield return new WaitForSeconds(2f);
            guideMenuCtrl.ChangeGuideInfo(GuideTextDic["第一个休止符"],"快躲开！");
            EventManager.Instance.TriggerEvent("OpenGuideMenu");
            yield return new WaitForSecondsRealtime(1.5f);
            EventManager.Instance.TriggerEvent("CloseGuideMenu");
            yield return StartCoroutine(CheckNote(3));
            EventManager.Instance.TriggerEvent("OpenGameShow");
            guideMenuCtrl.ChangeGuideInfo(GuideTextDic["能量条"],GuideTextDic["Space"]);
            EventManager.Instance.TriggerEvent("OpenGuideMenu");
            yield return StartCoroutine(CheckPageIsClosed());
            yield return new WaitForSecondsRealtime(3f);
            guideMenuCtrl.ChangeGuideInfo(GuideTextDic["拾取物"],"");
            EventManager.Instance.TriggerEvent("OpenGuideMenu");
            yield return new WaitForSecondsRealtime(3f);
            guideMenuCtrl.ChangeGuideInfo(GuideTextDic["开始游戏"],"按Space开始游戏");
            yield return StartCoroutine(CheckPageIsClosed());
            EventManager.Instance.TriggerEvent("CloseGameShow");
            PlayerManager.ChangeMoveMode(MoveMode.Wait);
            vcams[2].SetActive(true);
            VcamAnimator.Play("CMvcam1");
            Global.isPassedGuide = true;
            SaveManager.Instance.Save();
            yield return new WaitForSeconds(2.5f);
            AppInformation.nextSceneName = "Game";
            SceneManager.LoadScene("LoadingScene");



        }
              
        #region IEnumerators
        
        
        IEnumerator CheckNote(int notenum)
        {
            yield return new WaitForSeconds(0.1f);
            while (Collectibles[notenum].isCollectedByPlayer == 0)
                yield return new WaitForEndOfFrame();
            
            if (Collectibles[notenum].isCollectedByPlayer == 1)
            {
                switch (notenum)
                {
                    case 0:
                        guideMenuCtrl.ChangeGuideInfo(GuideTextDic["吃到第一个音符"],"松开回到中间");
                        EventManager.Instance.TriggerEvent("OpenGuideMenu");
                        yield return StartCoroutine(CheckPos(0.3f,LocationState.Middle));
                        break;
                    case 1:
                        guideMenuCtrl.ChangeGuideInfo(GuideTextDic["吃到第二个音符"],"同时按下<color=green><b>A和←</b></color>");
                        EventManager.Instance.TriggerEvent("OpenGuideMenu");
                        yield return StartCoroutine(CheckPos(0.3f,LocationState.LeftMax));
                        break;
                    case 2:
                        guideMenuCtrl.ChangeGuideInfo(GuideTextDic["吃到第一个跳跃音符"],GuideTextDic["Space"]);
                        yield return new WaitForSecondsRealtime(0.2f);
                        EventManager.Instance.TriggerEvent("OpenGuideMenu");
                        yield return StartCoroutine(CheckPageIsClosed());
                        break;
                    case 3:
                        guideMenuCtrl.ChangeGuideInfo(GuideTextDic["吃到第一个休止符"],"下次注意喵");
                        EventManager.Instance.TriggerEvent("OpenGuideMenu");
                        yield return new WaitForSecondsRealtime(1.5f);
                        EventManager.Instance.TriggerEvent("CloseGuideMenu");
                        break;
                }
                
            }
            else if (Collectibles[notenum].isCollectedByPlayer == -1)
            {
                switch (notenum)
                {
                    case 0:
                        guideMenuCtrl.ChangeGuideInfo(GuideTextDic["没吃到第一个音符"],GuideTextDic["Space"]);
                        EventManager.Instance.TriggerEvent("OpenGuideMenu");
                        yield return new WaitForSecondsRealtime(0.3f);
                        yield return StartCoroutine(CheckPageIsClosed());
                        break;
                    case 1:
                        guideMenuCtrl.ChangeGuideInfo(GuideTextDic["没吃到第二个音符"],GuideTextDic["Space"]);
                        EventManager.Instance.TriggerEvent("OpenGuideMenu");
                        yield return new WaitForSecondsRealtime(0.3f);
                        yield return StartCoroutine(CheckPageIsClosed());
                        break;
                    case 2:
                        guideMenuCtrl.ChangeGuideInfo(GuideTextDic["没吃到第一个跳跃音符"],GuideTextDic["Space"]);
                        EventManager.Instance.TriggerEvent("OpenGuideMenu");
                        yield return new WaitForSecondsRealtime(0.3f);
                        yield return StartCoroutine(CheckPageIsClosed());
                        break;
                    case 3:
                        guideMenuCtrl.ChangeGuideInfo(GuideTextDic["没吃到第一个休止符"],"喵~~~！冲！");
                        EventManager.Instance.TriggerEvent("OpenGuideMenu");
                        yield return new WaitForSecondsRealtime(1.5f);
                        EventManager.Instance.TriggerEvent("CloseGuideMenu");
                        break;
                }
               
            }
        }
        IEnumerator CheckPos(float waitTime, LocationState targetState)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            yield return new WaitUntil(() => PlayerManager.GetPlayerLocationState() == targetState);
            EventManager.Instance.TriggerEvent("CloseGuideMenu");
        }
        IEnumerator CheckJump()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            yield return new WaitUntil(() => Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.UpArrow));
            EventManager.Instance.TriggerEvent("CloseGuideMenu");
        }
        IEnumerator CheckPageIsClosed()
        {
            yield return new WaitForSecondsRealtime(0.3f);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            EventManager.Instance.TriggerEvent("CloseGuideMenu");
        }

        #endregion
    }
    
}