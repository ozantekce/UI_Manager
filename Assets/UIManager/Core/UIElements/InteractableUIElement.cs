using UnityEngine;
using UnityEngine.EventSystems;

namespace UIManager
{

    [RequireComponent(typeof(EventTrigger))]
    public class InteractableUIElement : UIElement
    {

        public UnityEngine.UI.ScrollRect ScrollView { get; private set; }
        public EventTrigger EventTrigger { get; private set; }
        [field: SerializeField] public InteractableData[] InteractableData { get; private set; }

        public override void Initialize()
        {
            base.Initialize();

            if (!TryGetComponent(out EventTrigger EventTrigger))
            {
                EventTrigger = gameObject.AddComponent<EventTrigger>();
            }


            ScrollView = gameObject.GetComponentInParent<UnityEngine.UI.ScrollRect>();

            // Setup interaction handlers specific to ScrollView if it's available.
            if (ScrollView != null)
            {
                EventTrigger.Entry entryBegin = new EventTrigger.Entry(),
                    entryDrag = new EventTrigger.Entry(),
                    entryEnd = new EventTrigger.Entry(),
                    entrypotential = new EventTrigger.Entry(),
                    entryScroll = new EventTrigger.Entry();


                entryBegin.eventID = EventTriggerType.BeginDrag;
                entryBegin.callback.AddListener((data) => { ScrollView.OnBeginDrag((PointerEventData)data); });
                EventTrigger.triggers.Add(entryBegin);

                entryDrag.eventID = EventTriggerType.Drag;
                entryDrag.callback.AddListener((data) => { ScrollView.OnDrag((PointerEventData)data); });
                EventTrigger.triggers.Add(entryDrag);

                entryEnd.eventID = EventTriggerType.EndDrag;
                entryEnd.callback.AddListener((data) => { ScrollView.OnEndDrag((PointerEventData)data); });
                EventTrigger.triggers.Add(entryEnd);

                entrypotential.eventID = EventTriggerType.InitializePotentialDrag;
                entrypotential.callback.AddListener((data) => { ScrollView.OnInitializePotentialDrag((PointerEventData)data); });
                EventTrigger.triggers.Add(entrypotential);

                entryScroll.eventID = EventTriggerType.Scroll;
                entryScroll.callback.AddListener((data) => { ScrollView.OnScroll((PointerEventData)data); });
                EventTrigger.triggers.Add(entryScroll);
            }


            // Setup additional custom event handlers defined in the InteractableData array.
            if(InteractableData != null)
            {
                foreach (InteractableData interactableData in InteractableData)
                {
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = interactableData.EventTriggerType;
                    entry.callback.AddListener(delegate
                    {
                        if (interactableData.AnimationWrapper != null)
                        {
                            if (AnimationComponent == null)
                            {
                                _animationComponent = gameObject.AddComponent<AnimationComponent>();
                            }
                            BaseUIAnimation animation = interactableData.AnimationWrapper.Animation;
                            Manager.StartCoroutine(AnimationComponent.PlayCustomAnimation(this, animation));
                        }
                        interactableData.UnityEvent?.Invoke();
                    });
                    EventTrigger.triggers.Add(entry);

                }
            }

            
        }

    }


}


