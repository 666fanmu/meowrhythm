using UnityEngine;

namespace SweetCandy.Basic
{

    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> //注意此约束为T必须为其本身或子类
    {

        private protected static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;

                instance = FindObjectOfType<T>();

                //为了防止脚本还未挂到物体上，找不到的异常情况，自动创建空物体挂上去
                if (instance == null)
                {
                    new GameObject("Singleton of " + typeof(T)).AddComponent<T>();
                }
                else instance.Init();

                return instance;

            }
        }

        private void Awake()
        {
            //若无其它脚本在Awake中调用此实例，则可在Awake中自行初始化instance
            instance = this as T;
            //初始化
            Init();
        }

        //子类对成员进行初始化如果放在Awake里仍会出现Null问题所以自行制作一个init函数解决（可用可不用）
        protected virtual void Init()
        {

        }
    }
}