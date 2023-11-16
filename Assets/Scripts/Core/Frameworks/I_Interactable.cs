using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace UI_Manager
{
    public interface I_Interactable
    {

        public EventTrigger EventTrigger { get; set; }

        public InteractableData[] InteractableDatas { get; set; }


    }

    [System.Serializable]
    public class InteractableData
    {

        public EventTriggerType eventTriggerType;
        public UI_Animation animation;
        public UnityEvent unityEvent;


    }

}

