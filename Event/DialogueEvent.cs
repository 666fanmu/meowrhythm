using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SweetCandy.Managers;
using SweetCandy.UI.Ctrls;
using UnityEngine;

namespace SweetCandy.Event
{
    [Serializable]
    public class DialogueMsg
    {
        public string info;
        public string subinfo;
        public float lastTime;
    }
    public class DialogueEvent : MonoBehaviour
    {
        public Dictionary<string, string> Dialogues = new Dictionary<string, string>()
        {
            {"喵问","喵？"},
            {"R开场","什么！你们是怎么进来的？\n难道是上次的漏洞忘记修复了吗？\n我们的系统已经快负荷运行了！"},
            {"R开场2","算了，既然进来了就别想出去了！\n我要把你们和那些没用的节奏一起删除！"},
            {"喵惊","喵！"},
            {"R被击败","可恶，\n我居然被这些愚蠢的节奏和小漏洞打败了\n...我一定会回来的！"},
            {"胜利","喵！我们成功啦！"},
            {"喵","喵"},
            {"混乱","哇！方向混乱了！\n现在按<color=blue>←</color>会向<color=red>右</color>、按<color=blue>→</color>会向<color=red>左</color>移动哦"},
            {"Color指引1","现在只能拾取与<color=green>轨道颜色</color><color=red>相同</color>的音符"},
            {"Color指引2","车外的猫移动到不同轨道将会控制轨道变成不同的颜色，车内的猫移动车车拾取音符收集能量！"},
            {"BeCatch指引","爪子抓过来了！快按下<color=blue>对应的方向键</color>!"},
            {"IsCatching指引","被抓的猫猫必须在短时间内快速正确按下对应方向！\n未被抓的猫猫需要一人控制车车躲避障碍！"},
            {"ComboTime指引","能量满了！糟糕是机器魔王！快做好准备！\n当方向箭头抵达爪子处时按下正确的方向键来攻击Boss吧！"},
            {"getCholo","巧克力可以增加<color=blue>能量</color>诶！"},
            {"getDisco","获得了双倍<color=green>分数BUFF</color>！太棒了"},
            {"getBomb","<color=red>危险度变高了</color>，好可怕"},
            {"getHand","移动方向<color=red>反转</color>了，头昏眼花！"},
            {"getHuatong","安全度增加！得分增加！好耶喵！"},
            {"backRain","移动又正常了！"},
        };

        private GameDialogueCtrl _gameDialogueCtrl;
        
        private void Awake()
        {
            _gameDialogueCtrl = FindObjectOfType<GameDialogueCtrl>();
        }

        private void Start()
        {
            EventManager.Instance.StartListening("ShowBeCatched",ShowBeCatched);
            EventManager.Instance.StartListening("ShowIsCatching",ShowIsCatching);
            EventManager.Instance.StartListening("ShowColorTime",ShowColorTime);
            EventManager.Instance.StartListening("ShowComboTime",ShowComboTime);
            EventManager.Instance.StartListening("ShowCholo",ShowCholo);
            EventManager.Instance.StartListening("ShowBomb",ShowBomb);
            EventManager.Instance.StartListening("ShowHand",ShowHand);
            EventManager.Instance.StartListening("ShowMik",ShowMik);
            EventManager.Instance.StartListening("ShowDisco",ShowDisco);
            EventManager.Instance.StartListening("ShowBackRain",ShowBackRain);
            StartCoroutine(IEDialogue());
        }

        private void OnDisable()
        {
            EventManager.Instance.StopListening("ShowBeCatched",ShowBeCatched);
            EventManager.Instance.StopListening("ShowIsCatching",ShowIsCatching);
            EventManager.Instance.StopListening("ShowColorTime",ShowColorTime);
            EventManager.Instance.StopListening("ShowComboTime",ShowComboTime);
            EventManager.Instance.StopListening("ShowCholo",ShowCholo);
            EventManager.Instance.StopListening("ShowBomb",ShowBomb);
            EventManager.Instance.StopListening("ShowHand",ShowHand);
            EventManager.Instance.StopListening("ShowMik",ShowMik);
            EventManager.Instance.StopListening("ShowDisco",ShowDisco);
            EventManager.Instance.StopListening("ShowBackRain",ShowBackRain);
            StopAllCoroutines();
        }

        private void ShowComboTime() => ShowMes("ComboTime指引");
        private void ShowIsCatching()=> ShowMes("IsCatching指引",3f);
        private void ShowBeCatched() => ShowMes("BeCatch指引",3f);

        private void ShowColorTime() => StartCoroutine(ColorTime());
     

        private IEnumerator ColorTime()
        {
            Time.timeScale = 0.9f;
            _gameDialogueCtrl.ChangeDialogueInfo(0 ,Dialogues["Color指引1"],"",3f);
            _gameDialogueCtrl.ChangeDialogueInfo(0 ,Dialogues["Color指引2"],"",6f);
            yield return new WaitForSeconds(7f);
            Time.timeScale = 1f;
        }
        private void ShowDisco() => ShowMes("getDisco",3f);
        private void ShowMik() => ShowMes("getHuatong",3f);
        private void ShowCholo() => ShowMes("getCholo",3f);
        private void ShowBomb() => ShowMes("getBomb",3f);
        private void ShowHand() => ShowMes("getHand",3f);

        private void ShowBackRain() =>ShowMes("backRain",1.5f);
    
        private void ShowMes(string key,float lastTime=5f)
        {
            _gameDialogueCtrl.ChangeDialogueInfo(0 ,Dialogues[key],"",lastTime);
        }
        private IEnumerator IEDialogue()
        {
            yield return new WaitForSeconds(2f);
           // GameManager.Instance.GameModeChange(GameMode.Talk);
            _gameDialogueCtrl.ChangeDialogueInfo(0 ,Dialogues["喵问"],"",1.5f);
            yield return new WaitForSeconds(2f);
            _gameDialogueCtrl.ChangeDialogueInfo(1,Dialogues["R开场"]);
            yield return new WaitForSeconds(3f);
            _gameDialogueCtrl.ChangeDialogueInfo(1,Dialogues["R开场2"]);
            yield return new WaitForSeconds(4f);
            EventManager.Instance.TriggerEvent("CloseDialogue");
            
            yield return new WaitUntil(() => FindObjectOfType<ComboTimeEvent>().isBossGGInComboTime == true);
            _gameDialogueCtrl.ChangeDialogueInfo(1,Dialogues["R被击败"]);
            yield return new WaitForSeconds(0.5f);
            _gameDialogueCtrl.ChangeDialogueInfo(0,Dialogues["胜利"],"",3f);
            yield return new WaitForSeconds(3f);
            EventManager.Instance.TriggerEvent("CloseDialogue");
            GameManager.Instance.GameModeChange(GameMode.Waiting);
            yield return new WaitForSeconds(0.2f);
            GameManager.Instance.GameModeChange(GameMode.Win);


        }
    }
}