using UnityEngine;
using UnityEngine.EventSystems;

namespace SweetCandy.UI
{

    public class BechosenCtrl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject image;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("鼠标进入");
            image.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("鼠标退出");
            image.SetActive(false);
        }
    }
}