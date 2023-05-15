using UnityEngine;
using UnityEngine.EventSystems;

namespace SweetCandy.Event
{
    public class LobbyMoveEvent : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        public RectTransform TitleRectTransform;

        public void OnPointerEnter(PointerEventData eventData)
        {
            TitleRectTransform.localScale = new Vector3(1.05f, 1.05f, 1);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TitleRectTransform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}