using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIManager
{

    [System.Serializable]
    public class InteractableData
    {

        [field: SerializeField] public EventTriggerType EventTriggerType { get; private set; }
        [field: SerializeField] public UIAnimationWrapper AnimationWrapper { get; private set; }
        [field: SerializeField] public UnityEvent UnityEvent { get; private set; }

    }

}