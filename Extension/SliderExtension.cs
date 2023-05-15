using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SweetCandy.Extension
{
    public static class SliderExtension
    {
        public static void OnBeginDragListener(this Slider slider, UnityAction<PointerEventData> action)
        {
            EventTrigger trigger = slider.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = slider.gameObject.AddComponent<EventTrigger>();
            }
        
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.BeginDrag;
            entry.callback.AddListener((data) => action((PointerEventData)data));
            trigger.triggers.Add(entry);
        }
    
        public static void OnEndDragListener(this Slider slider, UnityAction<PointerEventData> action)
        {
            EventTrigger trigger = slider.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = slider.gameObject.AddComponent<EventTrigger>();
            }
        
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener((data) => action((PointerEventData)data));
            trigger.triggers.Add(entry);
        }
    }
}