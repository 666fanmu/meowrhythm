using System.Collections;
using DG.Tweening;
using SweetCandy.Managers;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SweetCandy.Event
{
    public class IsCatchingEvent : MonoBehaviour
    {
        public GameObject handCanvas;
        private CanvasGroup _handCanvasGroup;
        public Image fill;
        public Image circle;
        public Image directionImage;
        public Image baseImage;
        public Sprite[] directionSprites=new Sprite[4];
        public Animator animator;//动画
        public GameObject[] catsInHand;
        public GameObject[] catsInRoad;
        public GameObject car;
        public int whichBeCatched;
        private int _directionLength = 200;
        private int[] _directions=new int[200];
        [SerializeField] private float _trueAmount=0;
        [SerializeField] private bool isCatchingOn=false;
        private bool _isNext = false;
        private void Awake()
        {
            _handCanvasGroup = handCanvas.GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            _handCanvasGroup.alpha = 0;
        }

        private void CatchingOn()
        {
            if (!Global.PointDictionary["getIsCatching"])
            {
                EventManager.Instance.TriggerEvent("ShowIsCatching");
                Global.PointDictionary["getIsCatching"] = true;
            }
            GameManager.Instance.GameModeChange(GameMode.BeCatched);
            for (int i = 0; i < _directionLength; i++)
            {
                _directions[i] = Random.Range(0, 4);
            }
            isCatchingOn = true;
            StartCatching();
            StartCheckStruggle();
            
        }
        private void StartCatching()=>StartCoroutine(CatchingCoroutine());
        private void StartCheckStruggle() => StartCoroutine(CheckIsStruggle());
        IEnumerator CatchingCoroutine()
        {
            int j = 0;
            while(j<_directionLength)
            {
                if (_trueAmount < 1)
                {
                    directionImage.enabled = true;
                    directionImage.sprite = directionSprites[_directions[j]];
                    switch (_directions[j])
                    {
                        case 0:
                            yield return StartCoroutine(CheckKeyPressForSeconds(0, 0.5f));
                            break;
                        case 1:
                            yield return StartCoroutine(CheckKeyPressForSeconds(1, 0.5f));
                            break;
                        case 2:
                            yield return StartCoroutine(CheckKeyPressForSeconds(2, 0.5f));
                            break;
                        case 3:
                            yield return StartCoroutine(CheckKeyPressForSeconds(3, 0.5f));
                            break;
                    }

                    if (_isNext)
                    {
                        j++;
                        AudioManager.Instance.PlaySound("QTE4");
                        baseImage.DOColor(Color.green, 0.1f).OnComplete(()=>baseImage.DOColor(Color.white,0.1f));
                        circle.transform.DOScale(new Vector3(1.3f, 1.3f, 1f), 0.1f);
                        //fill.DOFillAmount(_trueAmount+0.1f, 0.1f);
                        _trueAmount += 0.08f;
                    }
                    else if (!_isNext)
                    {
                        AudioManager.Instance.PlaySound("QTE3");
                        baseImage.DOColor(Color.red, 0.1f).OnComplete(()=>baseImage.DOColor(Color.white,0.1f));
                        circle.transform.DOScale(new Vector3(1.3f, 1.3f, 1f), 0.1f);
                        //fill.DOFillAmount(_trueAmount-0.2f, 0.1f);
                        if(_trueAmount>0.1) _trueAmount -= 0.09f;
                    }
                }
                else if (_trueAmount > 1)
                {
                    Escape();
                    break;
                }
            }
        }

        IEnumerator CheckKeyPressForSeconds(int keycode,float seconds)
        {
            yield return new WaitForSeconds(0.1f);
            //circle.transform.DOShakeScale(0.1f, new Vector3(1f, 1f, 1f), 4, 180,true);
            circle.transform.DOPunchScale(new Vector3(0.5f,0.5f,1f),0.5f,2,1);
            KeyCode[] wrongKeyCode;
            wrongKeyCode = GetWrongKeyCode(keycode);
            float startTime = Time.time;
            while (Time.time < startTime + seconds)
            {
                if(Input.GetKeyDown(GetKeyCode(keycode)))
                {
                    _isNext = true;
                    directionImage.enabled = false;
                    yield break;
                }
                else if(Input.GetKeyDown(wrongKeyCode[0])||Input.GetKeyDown(wrongKeyCode[1])||Input.GetKeyDown(wrongKeyCode[2]))
                {
                    break;
                }
                yield return null;
            }
            _isNext = false;
            directionImage.enabled = false;
            yield return new WaitForSeconds(0.1f);
        }

        private void FixedUpdate()
        {
            if (isCatchingOn&&_trueAmount<1)
            {
                fill.DOFillAmount(_trueAmount, 0.2f);
            }

            
            if (_trueAmount > 0 && _trueAmount < 1)
            {
                _trueAmount -= 0.001f;
            }
        }

        IEnumerator CheckIsStruggle()
        {
            yield return new WaitUntil(() => _trueAmount > 0.2f);
            animator.SetBool("isStruggle",true);
            yield return new WaitUntil((() => _trueAmount > 0.8f));
            animator.SetBool("isStruggle",false);
        }
        private KeyCode[] GetWrongKeyCode(int keycode)
        {
            if (whichBeCatched == 0)
            {
                switch (keycode)
                {
                    case 0:
                        return new KeyCode[]{KeyCode.S,KeyCode.A,KeyCode.D};
                        break;
                    case 1:
                        return new KeyCode[]{KeyCode.W,KeyCode.A,KeyCode.D};
                        break;
                    case 2:
                        return new KeyCode[]{KeyCode.W,KeyCode.S,KeyCode.D};
                        break;
                    case 3:
                        return new KeyCode[]{KeyCode.W,KeyCode.S,KeyCode.A};
                        break;
                }
            }
            else if(whichBeCatched==1)
            {
                switch (keycode)
                {
                    case 0:
                        return new KeyCode[]
                            { KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
                        break;
                    case 1:
                        return new KeyCode[]
                            { KeyCode.UpArrow,KeyCode.LeftArrow, KeyCode.RightArrow };
                        break;
                    case 2:
                        return new KeyCode[]
                            { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow };
                        break;
                    case 3:
                        return new KeyCode[]
                            { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow,};
                        break;
                }
            }

            return null;
        }
        private KeyCode GetKeyCode(int keycode)
        {
            if (whichBeCatched == 0)
            {
                switch (keycode)
                {
                    case 0:
                        return KeyCode.W;
                        break;
                    case 1:
                        return KeyCode.S;
                        break;
                    case 2:
                        return KeyCode.A;
                        break;
                    case 3:
                        return KeyCode.D;
                        break;
                }
            }
            else
            {
                switch (keycode)
                {
                    case 0:
                        return KeyCode.UpArrow;
                        break;
                    case 1:
                        return KeyCode.DownArrow;
                        break;
                    case 2:
                        return KeyCode.LeftArrow;
                        break;
                    case 3:
                        return KeyCode.RightArrow;
                        break;
                }
            }

            return KeyCode.Alpha0;
        }
        public void ToBeCatched()
        {
            _handCanvasGroup.DOFade(1f, 1f).OnComplete(CatchingOn);
        }
        private void Escape()
        {
            _trueAmount = 0f;
            fill.DOFillAmount(0, 0.5f);
            _handCanvasGroup.DOFade(0f, 0.2f);
            animator.SetTrigger("Escape");
        }

        private void EscapeOver()
        {
            catsInHand[whichBeCatched].SetActive(false);
            catsInRoad[whichBeCatched].SetActive(true);
            car.transform.DOMove(new Vector3(0, -1722, -3224), 0.5f);
            GameManager.Instance.GameModeChange(GameMode.ColorTime);
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                GameManager.Instance.ChangeWhichBeCatched(1);
                catsInHand[1].SetActive(false);
                catsInRoad[1].SetActive(true);
                car.transform.DOMove(new Vector3(0, -1722, -3224), 0.5f);
                GameManager.Instance.GameModeChange(GameMode.ColorTime);
            }
            
        }
#endif
        
    }
}