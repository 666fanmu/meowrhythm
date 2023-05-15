using SweetCandy.Managers;
using UnityEngine;

namespace SweetCandy.Event
{

    public class NextTimeEvent : MonoBehaviour
    {
        public GameObject upArrow;
        public GameObject downArrow;
        public GameObject leftArrow;
        public GameObject rightArrow;
        public static int TotalArrow;
        public int score;
        public GameObject firstRound;
        public GameObject LastRound;
        private bool isRhytimeStart;
        [SerializeField] private float Interval_time;
        private float time;
        [SerializeField] private GameObject ObsPrefeb;
        [SerializeField] private Vector3 objPos;
        private void OnEnable()
        {
            EventManager.Instance.StartListening("RhythmTime", StartRhythm);
        }

        private void OnDisable()
        {
            EventManager.Instance.StopListening("RhythmTime", StartRhythm);
        }

        private void StartRhythm()
        {
            //TODO 节奏时刻部分

            firstRound.SetActive(false);
            LastRound.SetActive(false);
        }

        
           
            // 普通击中的距离
            public float normalHitDistance = 4.0f;

            // 完美击中的距离
            public float perfectHitDistance = 2.0f;

            // 目标物体，即玩家
            public Transform target;

            void Update()
            {
                // 检测是否触碰到了预制体
                if (Input.GetMouseButtonDown(0))
                {
                    //需定义
                    /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        // 获取被触碰的预制体的名字
                        string objectName = hit.collider.gameObject.name;

                        // 根据预制体名字判断方向
                        switch (objectName)
                        {
                            case "UpArrowPrefab":
                                MoveObject(Vector3.forward);
                                
                                break;
                            case "DownArrowPrefab":
                                MoveObject(Vector3.back);
                                break;
                            case "LeftArrowPrefab":
                                MoveObject(Vector3.left);
                                break;
                            case "RightArrowPrefab":
                                MoveObject(Vector3.right);
                                break;
                        }
                    }*/
                }
            }

            void MoveObject(Vector3 direction)
            {
                // 计算预制体与目标之间的距离
                float distance = Vector3.Distance(transform.position, target.position);

                // 判断击中效果
                if (distance < 0)
                {
                    Debug.Log("未击中");
                    MissHit();
                }
                else if (distance < perfectHitDistance)
                {
                    Debug.Log("完美击中");
                    PerfectHit();
                }
                else if (distance < normalHitDistance)
                {
                    Debug.Log("普通击中");
                    NormalHit();
                }

               
            }
        

        

        public void NormalHit()
        {
            score += 1;

        }

        public void GreatHit()
        {
            score += 2;
        }

        public void PerfectHit()
        { 
            score += 3;
        }

        public void MissHit()
        {

        }
        
      
    

        
        /*private void CreatArrow()
        {
            GameObject TopCreatPoint = GameObject.Find("Top");
            GameObject nextArrow;
            float randomPositionX= Random.Range(0, 0); //此处为音符固定出现X轴坐标
            int arrowDir = Random.Range(1, 5);
            switch (arrowDir)
            {
                case 1:
                    nextArrow = GameObjectPool.CreateObject("upArrow", upArrow, Vector3(randomPosition.x, 0,TopCreatPoint.transform.position.z), upArrow.transform.rotation);
                    nextArrow.transform.parent = upArrow.transform.parent;
                    nextArrow.tag = "CloneArrow";
                    //print("Up");
                    //print(GetComponents<NoteObject>().Length);
                    break;
                case 2:
                    nextArrow = GameObjectPool.CreateObject("downArrow", downArrow, Vector3(randomPosition.x,0, TopCreatPoint.transform.position.z), downArrow.transform.rotation);
                    nextArrow.transform.parent = downArrow.transform.parent;
                    nextArrow.tag = "CloneArrow";
                    //print("Down");
                    //print(GetComponents<NoteObject>().Length);
                    break;
                case 3:
                    nextArrow = GameObjectPool.CreateObject("leftArrow", leftArrow, Vector3(randomPosition.x, 0,TopCreatPoint.transform.position.z), leftArrow.transform.rotation);
                    nextArrow.transform.parent = leftArrow.transform.parent;
                    nextArrow.tag = "CloneArrow";
                    //print("Left");
                    //print(GetComponents<NoteObject>().Length);
                    break;
                case 4:
                    nextArrow = GameObjectPool.CreateObject("rightArrow", rightArrow, Vector3(randomPosition.x, 0 ,TopCreatPoint.transform.position.z), rightArrow.transform.rotation);
                    nextArrow.transform.parent = rightArrow.transform.parent;
                    nextArrow.tag = "CloneArrow";
                    //print("Right");
                    //print(GetComponents<NoteObject>().Length);
                    break;
                default:
                    break;
            }
            totalArrow++;
        }*/
        
    }
}