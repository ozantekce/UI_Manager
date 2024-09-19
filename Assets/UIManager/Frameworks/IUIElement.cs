using UnityEngine;
using UnityEngine.Events;

namespace UIManager
{
    public interface IUIElement
    {
        public ElementStatus Status { get; }
        public UnityEvent BeforeOpen { get; }
        public UnityEvent AfterOpen { get; }
        public UnityEvent BeforeClose { get; }
        public UnityEvent AfterClose { get; }

        public AnimationComponent AnimationComponent { get; }
        public ElementInfo? ElementInfo { get; }
        public RectTransform RectTransform { get; }
        public UIManager Manager { get; }
        public GameObject GameObject { get; }

        public void Initialize();
        public void Open(float delay = 0f, int animIndex = 0);
        public void Close(float delay = 0, int animIndex = 0);
        public void ForceOpen();
        public void ForceClose();


    }
}