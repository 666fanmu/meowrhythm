
using SweetCandy.Extension;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SweetCandy.UI
{

    public class UIBase : MonoBehaviour
    {
        public GameObject Root; //根Canvas对象
        protected Canvas Canvas; //根Canvas组件
        protected UIView UIView; //对应的View脚本

        private void Awake()
        {
            Canvas = Root.GetComponent<Canvas>();
            UIView = Root.GetComponentInChildren<UIView>();
        }

        private void Start() => InitState(); //子类中调用即Start()
        private void OnEnable() => AddListeners(); //当激活时 开启监听
        private void OnDisable() => RemoveListeners(); //当不激活时 关闭监听

        public virtual void InitState()
        {
        }

        public virtual void AddListeners()
        {
        }

        public virtual void RemoveListeners()
        {
        }

        public void Enter(UnityAction complete) => Canvas.FadeIn(Root, () => complete());
        public void Exit() => Canvas.FadeOut(Root);
        public void UpdateView<T>(T t) where T : UIView => t.Refresh();
    }
}