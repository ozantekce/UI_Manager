using UnityEngine;
using UnityEngine.EventSystems;



namespace UI_Manager
{
    [RequireComponent(typeof(EventTrigger))]
    public class Interactable_UI_Element : UI_Element, I_Interactable
    {
        [SerializeField] private bool _findScrollViewInParent;
        [SerializeField] private UnityEngine.UI.ScrollRect _scrollView;
        private EventTrigger _eventTrigger;
        [SerializeField] private InteractableData[] _interactableDatas;
        private Coroutine _currentAnimation;

        public override void ConfigurationsAwake()
        {
            base.ConfigurationsAwake();


            _eventTrigger = GetComponent<EventTrigger>();
            if (_eventTrigger == null)
            {
                _eventTrigger = gameObject.AddComponent<EventTrigger>();
            }

            if (_findScrollViewInParent)
            {
                _scrollView = gameObject.GetComponentInParent<UnityEngine.UI.ScrollRect>();
            }

            if (_scrollView != null)
            {
                EventTrigger.Entry entryBegin = new EventTrigger.Entry(), entryDrag = new EventTrigger.Entry(), entryEnd = new EventTrigger.Entry(), entrypotential = new EventTrigger.Entry()
                    , entryScroll = new EventTrigger.Entry();

                entryBegin.eventID = EventTriggerType.BeginDrag;
                entryBegin.callback.AddListener((data) => { _scrollView.OnBeginDrag((PointerEventData)data); });
                _eventTrigger.triggers.Add(entryBegin);

                entryDrag.eventID = EventTriggerType.Drag;
                entryDrag.callback.AddListener((data) => { _scrollView.OnDrag((PointerEventData)data); });
                _eventTrigger.triggers.Add(entryDrag);

                entryEnd.eventID = EventTriggerType.EndDrag;
                entryEnd.callback.AddListener((data) => { _scrollView.OnEndDrag((PointerEventData)data); });
                _eventTrigger.triggers.Add(entryEnd);

                entrypotential.eventID = EventTriggerType.InitializePotentialDrag;
                entrypotential.callback.AddListener((data) => { _scrollView.OnInitializePotentialDrag((PointerEventData)data); });
                _eventTrigger.triggers.Add(entrypotential);

                entryScroll.eventID = EventTriggerType.Scroll;
                entryScroll.callback.AddListener((data) => { _scrollView.OnScroll((PointerEventData)data); });
                _eventTrigger.triggers.Add(entryScroll);
            }



            foreach (InteractableData id in _interactableDatas)
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = id.eventTriggerType;
                entry.callback.AddListener(delegate
                {
                    if (id.animation != null)
                    {
                        if (_currentAnimation != null) StopCoroutine(_currentAnimation);
                        _currentAnimation = StartCoroutine(id.animation.Enumerator(this));
                    }
                    id.unityEvent?.Invoke();
                });
                _eventTrigger.triggers.Add(entry);

            }

        }

        public override void ConfigurationsStart()
        {
            base.ConfigurationsStart();
        }

        protected override void Update()
        {
            base.Update();
        }
        public EventTrigger EventTrigger { get => _eventTrigger; set => _eventTrigger = value; }
        public InteractableData[] InteractableDatas { get => _interactableDatas; set => _interactableDatas = value; }
    }


}


