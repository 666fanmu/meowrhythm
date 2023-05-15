using UnityEngine.Events;
using System.Collections.Generic;
using SweetCandy.Basic;

namespace SweetCandy.Managers
{
    public class AppInformation
    {
        public static string nextSceneName;
    }
    public class EventManager : MonoSingleton<EventManager>
    {
        //这个dic用来管理需要监听的各类事件
        private Dictionary<string, UnityEvent> _eventDictionary = new Dictionary<string, UnityEvent>();

        protected override void Init()
        {
            if (_eventDictionary == null)
            {
                _eventDictionary = new Dictionary<string, UnityEvent>();
            }
        }

        /// <summary>
        /// 开始监听某一事件
        /// </summary>
        /// <param name="eventName">事件String类型ID</param>
        /// <param name="listener">事件触发时执行方法</param>
        public void StartListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Instance._eventDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        /// 停止监听 节约资源
        /// </summary>
        /// <param name="eventName">事件String类型ID</param>
        /// <param name="listener">事件触发时执行方法</param>
        public void StopListening(string eventName, UnityAction listener)
        {
            if (instance == null) return;
            UnityEvent thisEvent = null;
            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        /// <summary>
        /// 触发某事件
        /// </summary>
        /// <param name="eventName">触发事件string类型ID</param>
        public void TriggerEvent(string eventName)
        {
            UnityEvent thisEvent = null;
            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}