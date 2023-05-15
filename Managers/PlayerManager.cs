using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SweetCandy.Managers
{
    public enum LocationState
    {
        LeftMax=0,
        Left=1,
        Middle=2,
        Right=3,
        RightMax=4,
    }
    public enum KeyCombo 
    {
        A_RightArrow,
        D_LeftArrow,
        A_LeftArrow,
        D_RightArrow,
        A_or_LeftArrow,
        D_or_RightArrow,
        None
    }

    public enum MoveMode
    {
        Normal, //常规模式
        ComboTime, //ComboTime
        Color,//变色模式
        Wait,
        CatCatched,//一只被抓时
    }
    public class PlayerManager : MonoBehaviour
    {
        public List<float> movePoint = new List<float>(5);
        private GameObject _player;
        public float moveDuration=0.2f;
        private Transform _carTransform;
        [SerializeField]
        private Transform[] catTransform;
        private Animation _playerAnimation;
        [SerializeField]
        private Animation blackCatAnimation;
        [SerializeField]
        private Animation yellowCatAnimation;
        private LocationState _playerLocationState = LocationState.Middle;
        private LocationState _catLocationState = LocationState.Middle;
        private MoveMode _moveMode; //游戏阶段
        private bool _isPlayerMoving=false;
        private bool _isCatMoving = false;
        private bool _isJump=false;
        private List<KeyCode> _keysPressed = new List<KeyCode>();
        private bool _jumpRequested = false;
        private float _jumpCd;
        public float jumpCd;
        public int whichBeCatched;
        public int JumpTime=0;
        
        void Awake()
        {
            _player = GameObject.Find("Player");
            _moveMode = MoveMode.Normal;
            _carTransform = _player.GetComponent<Transform>();
            _playerAnimation = _player.GetComponent<Animation>();
        }

        public LocationState GetPlayerLocationState()
        {
            return this._playerLocationState;
        }

        public LocationState GetCatLocationState()
        {
            return this._catLocationState;
        }
        public void ChangeMoveMode(MoveMode newmoveMode)
        {
            this._moveMode = newmoveMode;
            if (_playerLocationState > LocationState.Middle)
                _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
            else if(_playerLocationState<LocationState.Middle)
                _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
            ReturnMiddle();
            _isJump = false;
            _jumpCd = 0f;
            _jumpRequested = false;
            _isPlayerMoving = false;
            _isCatMoving = false;
        }

        private void ReturnMiddle()
        {
            if (_playerLocationState > LocationState.Middle)
            {
                _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
            }
            else if (_playerLocationState < LocationState.Middle)
            {
                _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
            }
            _carTransform.DOMoveX(movePoint[2], moveDuration);
            _playerLocationState = LocationState.Middle;

        }
        private void Update()
        {
            if (_moveMode == MoveMode.Normal)
            {
                if (!_isJump) //不在跳跃状态才能下一步操作
                {
                    if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (Random.Range(0, 4) >= 2)
                        {
                            AudioManager.Instance.PlaySound("车子移动音效1");
                        }
                        else
                        {
                            AudioManager.Instance.PlaySound("车子移动音效2");
                        }
                    }
                    if (!_jumpRequested&&_jumpCd>=jumpCd)
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            _keysPressed.Add(KeyCode.W);
                            Invoke("ClearKeys", 0.4f);
                        }
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            _keysPressed.Add(KeyCode.UpArrow);
                            Invoke("ClearKeys", 0.4f);
                        }
                    }
                    
                    #region NormalXMove_New

                    switch (GetKeyCombo())
                    {
                        case KeyCombo.A_RightArrow:
                            if (_playerLocationState != LocationState.Middle)
                            {
                                if (_playerLocationState == LocationState.Left)
                                    _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
                                else if (_playerLocationState == LocationState.Right)
                                    _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
                            }
                            _carTransform.DOMoveX(movePoint[2], moveDuration);
                            _playerLocationState = LocationState.Middle;
                            break;
                        case KeyCombo.D_LeftArrow:
                            if (_playerLocationState != LocationState.Middle)
                            {
                                if (_playerLocationState == LocationState.Left)
                                    _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
                                else if (_playerLocationState == LocationState.Right)
                                    _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
                            }
                            _carTransform.DOMoveX(movePoint[2], moveDuration);
                            _playerLocationState = LocationState.Middle;
                            break;
                        case KeyCombo.A_LeftArrow:
                            if (_playerLocationState > LocationState.LeftMax)
                                _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
                            _carTransform.DOMoveX(movePoint[0], moveDuration);
                            _playerLocationState = LocationState.LeftMax;
                            break;
                        case KeyCombo.D_RightArrow:
                            if (_playerLocationState < LocationState.RightMax)
                                _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
                            _carTransform.DOMoveX(movePoint[4], moveDuration);
                            _playerLocationState = LocationState.RightMax;
                            break;
                        case KeyCombo.A_or_LeftArrow:
                            if (_playerLocationState < LocationState.Left)
                                _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
                            else if (_playerLocationState > LocationState.Left)
                                _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
                            _carTransform.DOMoveX(movePoint[1], moveDuration);
                            _playerLocationState = LocationState.Left;
                            break;
                        case KeyCombo.D_or_RightArrow:
                            if (_playerLocationState < LocationState.Right)
                                _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
                            else if (_playerLocationState > LocationState.Right)
                                _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
                            _carTransform.DOMoveX(movePoint[3], moveDuration);
                            _playerLocationState = LocationState.Right;
                            break;
                        case KeyCombo.None:
                            if (_playerLocationState > LocationState.Middle)
                            {
                                _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
                            }
                            else if (_playerLocationState < LocationState.Middle)
                            {
                                _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
                            }
                            _carTransform.DOMoveX(movePoint[2], moveDuration);
                            _playerLocationState = LocationState.Middle;
                            break;
                    }

                    #endregion
                    

                }
            }
            else if(_moveMode == MoveMode.Color)
            {
                ColorMove(whichBeCatched);
            }
            else if (_moveMode == MoveMode.ComboTime)
            {
                return;
            }
            else if (_moveMode == MoveMode.CatCatched)
            {
                if (!GameManager.Instance.GetIsCatched()[0])
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        if (_playerLocationState > LocationState.Left)
                        {
                            _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
                            _carTransform.DOMoveX(movePoint[1], moveDuration);
                            _playerLocationState = LocationState.Left;
                        }
                       
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        if (_playerLocationState < LocationState.Right)
                        {
                            _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
                            _carTransform.DOMoveX(movePoint[3], moveDuration);
                            _playerLocationState = LocationState.Right;
                        }
                    }
                    else
                    {
                            if (_playerLocationState > LocationState.Middle)
                            {
                                _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
                            }
                            else if (_playerLocationState < LocationState.Middle)
                            {
                                _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
                            }
                            _carTransform.DOMoveX(movePoint[2], moveDuration);
                            _playerLocationState = LocationState.Middle;
                    }
                }
                else if (!GameManager.Instance.GetIsCatched()[1])
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        if (_playerLocationState > LocationState.Left)
                        {
                            _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
                            _carTransform.DOMoveX(movePoint[1], moveDuration);
                            _playerLocationState = LocationState.Left;
                        }
                       
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                    {
                        if (_playerLocationState < LocationState.Right)
                        {
                            _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
                            _carTransform.DOMoveX(movePoint[3], moveDuration);
                            _playerLocationState = LocationState.Right;
                        }
                    }
                    else
                    {
                        if (_playerLocationState > LocationState.Middle)
                        {
                            _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
                        }
                        else if (_playerLocationState < LocationState.Middle)
                        {
                            _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
                        }
                        _carTransform.DOMoveX(movePoint[2], moveDuration);
                        _playerLocationState = LocationState.Middle;
                    }
                }

            }
            else if (_moveMode == MoveMode.Wait)
            {
                return;
            }
        }

        private void ColorMove(int which)
        {
            if (which == 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) && (!_isPlayerMoving))
                {
                    _isPlayerMoving = true;
                    _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
                    DoMoveLeft(_carTransform,ref _playerLocationState);
                    
                }
                if (Input.GetKeyDown(KeyCode.RightArrow) && (!_isPlayerMoving))
                {
                    
                    _isPlayerMoving= true;
                    _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
                    DoMoveRight(_carTransform,ref _playerLocationState);
                    
                }

                if (Input.GetKeyDown(KeyCode.A) && !_isCatMoving)
                {
                    _isCatMoving = true;
                    blackCatAnimation.Play("BlackCatMoveLeft", AnimationPlayMode.Mix);
                    DoCatMoveLeft(catTransform[which],ref _catLocationState);
                }

                if (Input.GetKeyDown(KeyCode.D) && !_isCatMoving)
                {
                    _isCatMoving = true;
                    blackCatAnimation.Play("BlackCatMoveRight", AnimationPlayMode.Mix);
                    DoCatMoveRight(catTransform[which],ref _catLocationState);
                }
            }
            else if(which == 1)
            {
                if (Input.GetKeyDown(KeyCode.A) && (!_isPlayerMoving))
                {
                    _isPlayerMoving = true;
                    _playerAnimation.Play("MoveLeft", AnimationPlayMode.Mix);
                    DoMoveLeft(_carTransform,ref _playerLocationState);
                    
                }
                if (Input.GetKeyDown(KeyCode.D) && (!_isPlayerMoving))
                {
                    
                    _isPlayerMoving= true;
                    _playerAnimation.Play("MoveRight", AnimationPlayMode.Mix);
                    DoMoveRight(_carTransform,ref _playerLocationState);
                    
                }
              

                if (Input.GetKeyDown(KeyCode.LeftArrow) && !_isCatMoving)
                {
                    _isCatMoving = true;
                    yellowCatAnimation.Play("YellowCatMoveLeft", AnimationPlayMode.Mix);
                    DoCatMoveLeft(catTransform[which],ref _catLocationState);
                }

                if (Input.GetKeyDown(KeyCode.RightArrow) && !_isCatMoving)
                {
                    _isCatMoving = true;
                    yellowCatAnimation.Play("YellowCatMoveRight", AnimationPlayMode.Mix);
                    DoCatMoveRight(catTransform[which],ref _catLocationState);
                }
            }
            
        }
        private void CatMoveDoTween(float xValue,Transform trans)
        {
            DOTween.Sequence().AppendInterval(0.1f).Append(trans.DOMoveX(xValue, moveDuration));
            DOTween.Sequence().AppendInterval(0.1f).Append(trans.DOMoveY(-1600, 0.1f).SetEase(Ease.OutCubic)
                .OnComplete(() => trans.DOMoveY(-1886, 0.08f).SetEase(Ease.InQuad)));
            Invoke("ReturnIsCatMoving",0.15f);
        }
        private void DoCatMoveLeft(Transform trans, ref LocationState pls)
        {
            if (pls == LocationState.Middle)
            {
                CatMoveDoTween(movePoint[1],trans);
                pls = LocationState.Left;
            }
            else if(pls== LocationState.Left)
            {
                CatMoveDoTween(movePoint[0],trans);
                pls = LocationState.LeftMax;
            }
            else if(pls==LocationState.Right)
            {
                CatMoveDoTween(movePoint[2],trans);
                pls = LocationState.Middle;
            }
            else if (pls == LocationState.RightMax)
            {
                CatMoveDoTween(movePoint[3],trans);
                pls = LocationState.Right;
            }
            else
            {
                DOTween.Sequence().AppendInterval(0.1f).Append(trans.DOMoveY(-1600, 0.1f).SetEase(Ease.OutCubic)
                    .OnComplete(() => trans.DOMoveY(-1886, 0.08f).SetEase(Ease.InQuad)));
                Invoke("ReturnIsCatMoving",0.15f);
            }
        }
        private void DoCatMoveRight(Transform trans, ref LocationState pls)
        {
            if (pls == LocationState.Middle)
            {
                CatMoveDoTween(movePoint[3],trans);
                pls = LocationState.Right;
            }
            else if(pls==LocationState.Left)
            {
                CatMoveDoTween(movePoint[2],trans);
                pls = LocationState.Middle;
            }
            else if (pls == LocationState.Right)
            {
                CatMoveDoTween(movePoint[4],trans);
                pls = LocationState.RightMax;
            }
            else if (pls == LocationState.LeftMax)
            {
                CatMoveDoTween(movePoint[1],trans);
                pls = LocationState.Left;
            }
            else
            {
                DOTween.Sequence().AppendInterval(0.1f).Append(trans.DOMoveY(-1600, 0.1f).SetEase(Ease.OutCubic)
                    .OnComplete(() => trans.DOMoveY(-1886, 0.08f).SetEase(Ease.InQuad)));
                Invoke("ReturnIsCatMoving",0.15f);
            }
        }
        /// <summary>
        /// 向左移动轨道通用代码
        /// </summary>
        /// <param name="trans">移动目标</param>
        /// <param name="pls">该目标的位置状态</param>
        private void DoMoveLeft(Transform trans,ref LocationState pls)
        {
            if (pls == LocationState.Middle)
            {
                trans.DOMoveX(movePoint[1], moveDuration);
                Invoke("ReturnIsPlayerMoving",0.1f);
                pls = LocationState.Left;
            }
            else if(pls== LocationState.Left)
            {
                trans.DOMoveX(movePoint[0], moveDuration);
                Invoke("ReturnIsPlayerMoving",0.1f);
                pls = LocationState.LeftMax;
            }
            else if(pls==LocationState.Right)
            {
                trans.DOMoveX(movePoint[2], moveDuration);
                Invoke("ReturnIsPlayerMoving",0.1f);
                pls = LocationState.Middle;
            }
            else if (pls == LocationState.RightMax)
            {
                trans.DOMoveX(movePoint[3], moveDuration);
                Invoke("ReturnIsPlayerMoving",0.1f);
                pls = LocationState.Right;
            }
            else
            {
                Invoke("ReturnIsPlayerMoving",0.1f);
            }
            
        }

        private void DoMoveRight(Transform trans, ref LocationState pls)
        {
            if (pls == LocationState.Middle)
            {
                trans.DOMoveX(movePoint[3], moveDuration);
                Invoke("ReturnIsPlayerMoving",0.1f);
                pls = LocationState.Right;
            }
            else if(pls==LocationState.Left)
            {
                trans.DOMoveX(movePoint[2], moveDuration);
                Invoke("ReturnIsPlayerMoving",0.1f);
                pls = LocationState.Middle;
            }
            else if (pls == LocationState.Right)
            {
                trans.DOMoveX(movePoint[4], moveDuration);
                Invoke("ReturnIsPlayerMoving",0.1f);
                pls = LocationState.RightMax;
            }
            else if (pls == LocationState.LeftMax)
            {
                trans.DOMoveX(movePoint[1], moveDuration);
                Invoke("ReturnIsPlayerMoving",0.1f);
                pls = LocationState.Left;
            }
            else
            {
                Invoke("ReturnIsPlayerMoving",0.1f);
            }
            
        }
        private void ReturnIsPlayerMoving()
        {
            _isPlayerMoving = false;
        }

        private void ReturnIsCatMoving()
        {
            _isCatMoving = false;
        }

        private void FixedUpdate()
        {
            if (_moveMode == MoveMode.Normal)
            {
                if (_keysPressed.Contains(KeyCode.W) && _keysPressed.Contains(KeyCode.UpArrow) && _jumpCd >= jumpCd)
                {
                    _jumpRequested = true;
                    _keysPressed.Clear();
                }

                if (_jumpRequested && !_isJump && _jumpCd >= jumpCd)
                {
                    _jumpCd = 0;
                    _jumpRequested = false;
                    _keysPressed.Clear();
                    DoJump();
                    _playerAnimation.Play("Jump", AnimationPlayMode.Mix);
                }
                else
                {
                    _jumpCd += Time.deltaTime;
                }
            }
            else if (_moveMode == MoveMode.Color)
            {
                if (_jumpRequested && !_isJump && _jumpCd >= jumpCd)
                {
                    _jumpCd = 0;
                    _jumpRequested = false;
                    DoJump();
                    _playerAnimation.Play("Jump", AnimationPlayMode.Mix);
                }
                else
                {
                    _jumpCd += Time.deltaTime;
                }
            }
        }

        private void ClearKeys()
        {
            _keysPressed.Clear();
        }
        private void DoJump()
        {
            switch (JumpTime)
            {
                case 0:
                    AudioManager.Instance.PlaySound("跳跃音效1");
                    break;
                case 1:
                    AudioManager.Instance.PlaySound("跳跃音效2");
                    break;
                case 2:
                    AudioManager.Instance.PlaySound("跳跃音效3");
                    JumpTime = 0;
                    break;
            }
            _isJump = true;
            _carTransform.DOMoveY(-1500, 0.2f).SetEase(Ease.OutCirc).OnComplete(JumpDown);
        }

       

        private void JumpDown()
        {
            _carTransform.DOMoveY(-1855, 0.2f).SetEase(Ease.InCirc);
            _isJump = false;
        }
        private KeyCombo GetKeyCombo() {
            if (Input.GetKey(KeyCode.A)&&Input.GetKey(KeyCode.RightArrow)) {
                if (GameManager.Instance.isRain)
                {
                    return KeyCombo.D_LeftArrow;
                }
                else
                {
                    return KeyCombo.A_RightArrow;
                }
            }else if (Input.GetKey(KeyCode.D)&&Input.GetKey(KeyCode.LeftArrow)){
                if (GameManager.Instance.isRain)
                {
                    return KeyCombo.A_RightArrow;
                }
                else
                {
                    return KeyCombo.D_LeftArrow;
                }
            }else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftArrow)) {
                if (GameManager.Instance.isRain)
                {
                    return KeyCombo.D_RightArrow;
                }
                else
                {
                    return KeyCombo.A_LeftArrow;
                }
            } else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.RightArrow)) {
                if (GameManager.Instance.isRain)
                {
                    return KeyCombo.A_LeftArrow;
                }
                else
                {
                    return KeyCombo.D_RightArrow;
                }
                
            } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                if (GameManager.Instance.isRain)
                {
                    return KeyCombo.D_or_RightArrow;
                }
                else
                {
                    return KeyCombo.A_or_LeftArrow;
                }
                
            } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                if (GameManager.Instance.isRain)
                {
                    return KeyCombo.A_or_LeftArrow;
                }
                else
                {
                    return KeyCombo.D_or_RightArrow;
                }
                
            } else {
                return KeyCombo.None;
            }
        }
        
    }
    
}