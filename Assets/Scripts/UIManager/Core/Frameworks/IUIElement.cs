using UnityEngine;
using UnityEngine.Events;

namespace UIManager
{
    public interface IUIElement
    {
        public UIElementStatus Status { get; set; }
        public UnityEvent BeforeOpen { get; set; }
        public UnityEvent AfterOpen { get; set; }
        public UnityEvent BeforeClose { get; set; }
        public UnityEvent AfterClose { get; set; }

        public UIAnimationComponent AnimationComponent { get; }
        public ElementInfo? ElementInfo { get; }
        public RectTransform RectTransform { get; }
        public UIManager Manager { get; }

        public GameObject GameObject { get; }

        public void ConfigurationsAwake();
        public void ConfigurationsStart();
        public void UpdateUI(params object[] parameters);
        public void Open(float delay = 0f, int animIndex = 0);
        public void Close(float delay = 0, int animIndex = 0);
        public void ForceOpen();
        public void ForceClose();


    }
}
