using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIManager
{

    [System.Serializable]
    public class InteractableData
    {

        public EventTriggerType eventTriggerType;
        public UIAnimationWrapper animationWrapper;
        public UnityEvent unityEvent;


    }

}