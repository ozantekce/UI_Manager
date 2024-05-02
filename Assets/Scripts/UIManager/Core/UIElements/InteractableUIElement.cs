using UnityEngine;
using UnityEngine.EventSystems;

namespace UIManager
{

    /// <summary>
    /// An interactable UI element that extends UIElement to include event-driven behavior.
    /// This component requires an EventTrigger to work.
    /// </summary>
    [RequireComponent(typeof(EventTrigger))]
    public class InteractableUIElement : UIElement, IInteractableUIElement
    {
        /// <summary>
        /// Indicates whether to search for a ScrollRect component in the parent GameObjects.
        /// </summary>
        [field: SerializeField] public bool FindScrollViewInParent { get; private set; }
        /// <summary>
        /// References the ScrollRect component, allowing the element to interact with scroll behaviors.
        /// </summary>
        [field: SerializeField] public UnityEngine.UI.ScrollRect ScrollView { get; private set; }
        /// <summary>
        /// Array of data objects defining how interactions should be handled.
        /// </summary>
        [field: SerializeField] public InteractableData[] InteractableData { get; private set; }

        /// <summary>
        /// The EventTrigger component attached to this UI element. 
        /// </summary>
        [field: SerializeField] public EventTrigger EventTrigger { get; private set; }

        /// <summary>
        /// Setup configurations specific to this UI element that should occur during the Awake phase.
        /// This includes setting up event triggers and scroll view interactions if applicable.
        /// </summary>
        public override void ConfigurationsAwake()
        {
            base.ConfigurationsAwake(); // Call to base class to execute its configuration logic.

            // Ensure the component has an EventTrigger, and add one if it doesn't.
            if (!TryGetComponent(out EventTrigger EventTrigger))
            {
                EventTrigger = gameObject.AddComponent<EventTrigger>();
            }

            // If the flag is set, try to find a ScrollView component in the parent hierarchy.
            if (FindScrollViewInParent)
            {
                ScrollView = gameObject.GetComponentInParent<UnityEngine.UI.ScrollRect>();
            }

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
                                _animationComponent = gameObject.AddComponent<UIAnimationComponent>();
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

        /// <summary>
        /// Setup configurations specific to this UI element that should occur during the Start phase.
        /// </summary>
        public override void ConfigurationsStart()
        {
            base.ConfigurationsStart();
        }

        /// <summary>
        /// Update method to handle any runtime updates specific to this UI element.
        /// </summary>
        protected override void Update()
        {
            base.Update();
        }


    }


}


