using System;
using System.Collections;
using DG.Tweening;
using SweetCandy.Managers;
using UnityEngine;

namespace SweetCandy.Event
{
    [Serializable]
    public class ColorModel
    {
        public Color PuColor;
        public Color PuDiColor;
        public Color RoadColor;

        public ColorModel(Color pu,Color road,Color pudi)
        {
            this.PuColor = pu;
            this.RoadColor = road;
            this.PuDiColor = pudi;
        }
    }
    public class RoadColorEvent : MonoBehaviour
    {
        public Material pu;
        public Material pudi;
        public Material road;
        [SerializeField] private ColorModel Model0=new ColorModel
            (new Color(254/255f,255/255f,128/255f),new Color(255/255f,175/255f,95/255f),new Color(255/255f,238/255f,210/255f));
        [SerializeField] private ColorModel Model1=new ColorModel
            (new Color(255/255f,231/255f,153/255f),new Color(102/255f,255/255f,123/255f),new Color(255/255f,120/255f,90/255f));
        [SerializeField] private ColorModel Model2=new ColorModel
            (new Color(107/255f,158/255f,255/255f),new Color(255/255f,231/255f,78/255f),new Color(141/255f,213/255f,255/255f));
        [SerializeField] private ColorModel Model3=new ColorModel
            (new Color(255/255f,255/255f,255/255f),new Color(255/255f,139/255f,129/255f),new Color(250/255f,136/255f,255/255f));
        [SerializeField] private ColorModel Model4=new ColorModel
            (new Color(255/255f,244/255f,102/255f),new Color(129/255f,146/255f,255/255f),new Color(194/255f,112/255f,255/255f));
        [SerializeField] private ColorModel Model5=new ColorModel
            (new Color(95/255f,255/255f,122/255f),new Color(255/255f,174/255f,105/255f),new Color(255/255f,255/255f,255/255f));
        private PlayerManager _playerManager;
        private LocationState _currentCatLocation;
        private int _currentColorModel;
        private bool _isRoadColor=false;
        private void Awake()
        {
            _playerManager = FindObjectOfType<PlayerManager>();
            _currentColorModel = 2;
        }

        private void Start()
        {
            ChangeModelColor(Model0);
        }

        private void Update()
        {
            if(_isRoadColor)
                _currentCatLocation = _playerManager.GetCatLocationState();
        }
        public void RoadColorIn()
        {
            _isRoadColor = true;
            StartCoroutine(ColorDialogue());
            StartCoroutine(WaitForValue(LocationState.Left,0.4f,ChangeColor));
            StartCoroutine(WaitForValue(LocationState.LeftMax,0.4f,ChangeColor));
            StartCoroutine(WaitForValue(LocationState.Middle,0.4f,ChangeColor));
            StartCoroutine(WaitForValue(LocationState.Right,0.4f,ChangeColor));
            StartCoroutine(WaitForValue(LocationState.RightMax,0.4f,ChangeColor));
        }

        private IEnumerator ColorDialogue()
        {
            yield return new WaitForSeconds(2f);
            if (!Global.PointDictionary["getColor"])
            {
                EventManager.Instance.TriggerEvent("ShowColorTime");
                Global.PointDictionary["getColor"] = true;
            }
        }
        public void RoadColorOut()
        {
            ChangeModelColor(Model0);
            _isRoadColor=false;
            StopAllCoroutines();
            EventManager.Instance.TriggerEvent("OpenGameShow");
        }
        private IEnumerator WaitForValue(LocationState targetLocationState, float waitTime,Action<LocationState> callback)
        {
            float timer = 0.0f;
            while (true)
            {
                if (_currentCatLocation == targetLocationState&& _currentColorModel!=(int)targetLocationState)
                {
                    timer += Time.deltaTime;
                    if (timer >= waitTime)
                    {
                        callback(targetLocationState);
                        //Debug.Log("Change!");
                    }
                }
                else
                {
                    timer = 0.0f;
                }
                yield return null;
            }
        }
        private void ChangeColor(LocationState where)
        {
            if (where == LocationState.LeftMax)
            {
                road.SetVector("_cat_position",new Vector2(0.8f,0.2f));
                ChangeModelColor(Model1);
                _currentColorModel = 0;
            }
            else if (where == LocationState.Left)
            {
                road.SetVector("_cat_position",new Vector2(0.8f,0.7f));
                ChangeModelColor(Model2);
                _currentColorModel = 1;
            }
            else if (where == LocationState.Middle)
            {
                road.SetVector("_cat_position",new Vector2(0.8f,0.75f));
                ChangeModelColor(Model3);
                _currentColorModel = 2;
            }
            else if (where == LocationState.Right)
            {
                road.SetVector("_cat_position",new Vector2(0.8f,0.82f));
                ChangeModelColor(Model4);
                _currentColorModel = 3;
            }
            else if(where ==LocationState.RightMax)
            {
                road.SetVector("_cat_position",new Vector2(0.8f,1f));
                ChangeModelColor(Model5);
                _currentColorModel = 4;
            }
            else
            {
                road.SetVector("_cat_position",new Vector2(0.8f,0.75f));
                ChangeModelColor(Model3);
                _currentColorModel = 2;
            }

        }

        private void ChangeModelColor(ColorModel cm)
        {
            pu.DOColor(cm.PuColor,"_Color",1f);
            pudi.DOColor(cm.PuDiColor, "_Color", 1f);
            road.SetColor("_base_color",cm.RoadColor);
            road.DOFloat(1f, "_transition", 1.2f).OnComplete(() => ChangeRoadColor(cm));
        }

        private void ChangeRoadColor(ColorModel cm)
        {
            road.DOColor(cm.RoadColor,"_Color",0.1f).OnComplete(() => road.DOFloat(0f, "_transition", 0.1f));
        }

        public int GetCurrentColorModel()
        {
            return _currentColorModel;
        }
    }
}