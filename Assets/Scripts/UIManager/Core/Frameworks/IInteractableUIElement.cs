using UnityEngine.EventSystems;


namespace UIManager
{
    public interface IInteractableUIElement : IUIElement
    {

        public EventTrigger EventTrigger { get; }

        public InteractableData[] InteractableData { get; }


    }



}

