using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class Interactable_UI_Element : UI_Element, I_Interactable
{


    private EventTrigger _eventTrigger;
    [SerializeField] private InteractableData[] _interactableDatas;
    private Coroutine _currentAnimation;

    public override void ConfigurationsAwake()
    {
        base.ConfigurationsAwake();

        _eventTrigger = GetComponent<EventTrigger>();
        if( _eventTrigger == null)
        {
            _eventTrigger = gameObject.AddComponent<EventTrigger>();
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


    public EventTrigger EventTrigger { get => _eventTrigger; set => _eventTrigger = value; }
    public InteractableData[] InteractableDatas { get => _interactableDatas; set => _interactableDatas = value; }
}
