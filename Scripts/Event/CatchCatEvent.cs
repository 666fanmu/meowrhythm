using System.Collections;
using DG.Tweening;
using SweetCandy.Managers;
using SweetCandy.PrefabsLogic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SweetCandy.Event
{
    public class CatchCatEvent : MonoBehaviour
    {
        private GameObject _robotHand;
        [SerializeField] private GameObject qtePanel;
        public GameObject[] catsInCar; 
        [SerializeField] private GameObject[] catsInHand;
        public Animator animator;//动画
        [SerializeField] private GameObject[] directionImagesObj=new GameObject[2];
        private Image[] _directionImages = new Image[2];
        [SerializeField] private Sprite[] directionSprites = new Sprite[4];
        public QteCollision qteCollision0;
        public QteCollision qteCollision1;
        public int[,] PlayerDirection = new int[2,10];//0黑 1黄
        public bool[] isCatched= new bool[2];
        public Image blackCircle;
        public Image yellowCircle;
        public Image blackPanel;
        public Image yellowPanel;
        private int _qteLength=8;
        private int[] _falseCount=new int[2];
        private void Awake()
        {
            _robotHand = GameObject.Find("RobotHand");
            _directionImages[0] = directionImagesObj[0].GetComponent<Image>();
            _directionImages[1] = directionImagesObj[1].GetComponent<Image>();
            _falseCount[0] = 0;
            _falseCount[1] = 0;
        }

        private void Start()
        {
            EventManager.Instance.StartListening("CatchCat",CatchCat);
        }
        
        private void StartPlayer0Qte() => StartCoroutine( JudgingCourotine0());
        private void StartPlayer1Qte() => StartCoroutine(JudgingCourotine1());
        public void StartJudging()//通过动画状态机调用 无需脚本调用
        {
            GameManager.Instance.DisPlayerManager();
            qtePanel.SetActive(true);
            for (int i = 0; i < _qteLength; i++)
            {
                PlayerDirection[0,i] = Random.Range(0, 4);
                PlayerDirection[1,i] = Random.Range(0, 4);
            }
            StartPlayer0Qte();
            StartPlayer1Qte();
        }

        IEnumerator JudgingCourotine0()
        {
            for (int j = 0; j < _qteLength; j++)
            {
                _directionImages[0].enabled = true;
                _directionImages[0].sprite = directionSprites[PlayerDirection[0,j]];
                switch (PlayerDirection[0,j])
                {
                    case 0:
                        yield return StartCoroutine(CheckKeyPressForSeconds(KeyCode.W, 1f,0));
                        break;
                    case 1:
                        yield return StartCoroutine(CheckKeyPressForSeconds(KeyCode.S, 1f,0));
                        break;
                    case 2:
                        yield return StartCoroutine(CheckKeyPressForSeconds(KeyCode.A, 1f,0));
                        break;
                    case 3:
                        yield return StartCoroutine(CheckKeyPressForSeconds(KeyCode.D, 1f,0));
                        break;
                }
            }
        }
        IEnumerator JudgingCourotine1()
        {
            for (int j = 0; j < _qteLength; j++)
            {
                _directionImages[1].enabled = true;
                _directionImages[1].sprite = directionSprites[PlayerDirection[1,j]];
                
                switch (PlayerDirection[1,j])
                {
                    case 0:
                        yield return StartCoroutine(CheckKeyPressForSeconds(KeyCode.UpArrow, 1.5f, 1));
                        break;
                    case 1:
                        yield return StartCoroutine(CheckKeyPressForSeconds(KeyCode.DownArrow, 1.5f, 1));
                        break;
                    case 2:
                        yield return StartCoroutine(CheckKeyPressForSeconds(KeyCode.LeftArrow, 1.5f, 1));
                        break;
                    case 3:
                        yield return StartCoroutine(CheckKeyPressForSeconds(KeyCode.RightArrow, 1.5f, 1));
                        break;
                }
            }
        }
        IEnumerator CheckKeyPressForSeconds(KeyCode key, float seconds,int which)
        {
            if (which == 0)
            {
                blackCircle.rectTransform.DOScale(new Vector3(1.1f, 1f, 1), seconds-0.05f).SetEase(GetRandomEase());
            }
            else
            {
                yellowCircle.rectTransform.DOScale(new Vector3(1.1f, 1f, 1), seconds-0.05f).SetEase(GetRandomEase());
            }
            float startTime = Time.time;
            while (Time.time < startTime + seconds)
            {
                if (which == 0)
                {
                    if (qteCollision0.CanPressDown&&Input.GetKeyDown(key))
                    {
                        _directionImages[0].enabled = false;
                        AudioManager.Instance.PlaySound("QTE4");
                        blackPanel.DOColor(Color.green, 0.1f).OnComplete(()=>blackPanel.DOColor(Color.white,0.1f));
                        blackCircle.rectTransform.DOScale(new Vector3(2, 2, 1), 0f);
                        yield break;
                    }
                } 
                else
                {
                    if (qteCollision1.CanPressDown&&Input.GetKeyDown(key))
                    {
                        _directionImages[1].enabled = false;
                        AudioManager.Instance.PlaySound("QTE4");
                        yellowPanel.DOColor(Color.green, 0.1f).OnComplete(()=>yellowPanel.DOColor(Color.white,0.1f));
                        yellowCircle.rectTransform.DOScale(new Vector3(2, 2, 1), 0f);
                        yield break;
                    }
                }

                yield return null;
            }
            _directionImages[which].enabled = false;
            AudioManager.Instance.PlaySound("QTE3");
            if (which == 0)
            {
                blackPanel.DOColor(Color.red, 0.1f).OnComplete(()=>blackPanel.DOColor(Color.white,0.1f));
                blackCircle.rectTransform.DOScale(new Vector3(2, 2, 1), 0f);
            }
            else
            {
                yellowPanel.DOColor(Color.red, 0.1f).OnComplete(()=>yellowPanel.DOColor(Color.white,0.1f));
                yellowCircle.rectTransform.DOScale(new Vector3(2, 2, 1), 0f);
            }
            _falseCount[which]++;
            yield return new WaitForSeconds(0.1f);
        }

        private Ease GetRandomEase()
        {
            Ease[] easeArray = new Ease[] { Ease.InCirc ,Ease.InSine,Ease.InCubic,Ease.InQuad};
            return easeArray[Random.Range(0, 4)];
        }
        public void ExitJudging()
        {
            yellowCircle.rectTransform.localScale = new Vector3(2f,2f,1);
            blackCircle.rectTransform.localScale = new Vector3(2f,2f,1);
            qtePanel.SetActive(false);
            CheckIsCatch(0);
            CheckIsCatch(1);
            if (isCatched[0] && isCatched[1]) 
                GameManager.Instance.GameModeChange(GameMode.End);
            else if ( isCatched[0] || isCatched[1])
            {
                if(!animator.GetBool("BeCatched"))
                    animator.SetBool("BeCatched",true);
                GameManager.Instance.EnablePlayerManager();
            }
            else GameManager.Instance.EnablePlayerManager();
           
         
        }
        void CatchCat()
        {
            GameManager.Instance.GameModeChange(GameMode.Waiting);
            if (!Global.PointDictionary["getBeCatch"])
            {
                EventManager.Instance.TriggerEvent("ShowBeCatched");
                Global.PointDictionary["getBeCatch"] = true;
            }
            ScoreManager.Instance.getChange(0,1,0,0);
            Debug.Log("catch cat");
            animator.SetTrigger("Catch");
        }

        #region AnimatorFunc

        public void ToGame()
        {
            GameManager.Instance.GameModeChange(GameMode.Game);
            ScoreManager.Instance.getChange(10,-10,0,5);
        }
        public void ToEscape()
        {
            animator.SetBool("BeCatched",false);
        }
        public void ToIsCatching()
        {
            animator.SetBool("IsCatching",true);
        }

        public void ToCatchingOver()
        {
            animator.SetBool("IsCatching",false);
        }
        #endregion
        
        private void CheckIsCatch(int which)
        {
            if (_falseCount[which] > 1)
            {
                isCatched[which] = true;
                GameManager.Instance.ChangeWhichBeCatched(which);
                catsInCar[which].SetActive(false);
                catsInHand[which].SetActive(true);
                _falseCount[which] = 0;
            }
        }

    }
}