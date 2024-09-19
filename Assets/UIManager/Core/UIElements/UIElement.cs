using EditorTools;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UIManager
{

    public class UIElement : MonoBehaviour, IUIElement
    {

        protected ElementInfo? _elementInfo = null;

        internal bool Initialized { get; private set; }

        [SerializeField, HideInInspector] protected UIManager _manager;
        [SerializeField, HideInInspector] protected RectTransform _rectTransform;
        [SerializeField, HideInInspector] protected AnimationComponent _animationComponent;

        [field: SerializeField, ReadOnlyInPlayMode] public string Name { get; internal set; }
        [field: SerializeField, ReadOnly] public ElementStatus Status { get; internal set; }


        [Header("Settings")]
        [SerializeField] private bool _setAsLastSiblingOnOpen = true;
        [SerializeField] private bool _startWithOpenAnimation = false;


        [field: Header("Events")]
        [field: SerializeField] public UnityEvent BeforeOpen { get; set; }
        [field: SerializeField] public UnityEvent AfterOpen { get; set; }
        [field: SerializeField] public UnityEvent BeforeClose { get; set; }
        [field: SerializeField] public UnityEvent AfterClose { get; set; }



        public ElementInfo? ElementInfo => _elementInfo;
        public UIManager Manager => _manager == null ? _manager = UIManager.Instance : _manager;
        public RectTransform RectTransform => _rectTransform != null ? _rectTransform : _rectTransform = GetComponent<RectTransform>();
        public AnimationComponent AnimationComponent => _animationComponent != null ? _animationComponent : _animationComponent = GetComponent<AnimationComponent>();

        public GameObject GameObject => gameObject;

        protected virtual void Awake()
        {
            if (!Initialized)
            {
                if (Manager != null)
                {
                    Manager.RegisterElement(this);
                }
            }
        }

        protected virtual void Start()
        {
            ElementInfo info = new ElementInfo(this);
            _elementInfo = info;

            if (_startWithOpenAnimation)
            {
                Manager.StartCoroutine(AnimationComponent.PlayAnimation(this, AnimationExecuteTime.Open, 0));
            }
        }


        protected virtual void Update()
        {
            if (_elementInfo == null)
            {
                ElementInfo info = new ElementInfo(this);
                _elementInfo = info;
            }
        }


        protected virtual void OnDestroy()
        {
            if (Manager != null)
            {
                Manager.UnregisterElement(this);
            }
        }


        public virtual void Initialize()
        {
            Initialized = true;

            if (gameObject.activeSelf)
            {
                Status = ElementStatus.Opened;
                if (AnimationComponent != null)
                {
                    StartCoroutine(AnimationComponent.PlayAnimation(this, AnimationExecuteTime.Idle, 0));
                }
            }
            else
            {
                Status = ElementStatus.Closed;
            }
        }

        public void Open(float delay = 0f, int animIndex = 0)
        {
            if (Manager == null)
            {
                ForceOpen();
            }
            else
            {
                if (delay <= 0)
                    Manager.StartCoroutine(OpenRoutine(animIndex)); // execute request directly
                else
                    Manager.OpenUIElement(this, delay, animIndex); // send request to manager
            }

        }

        public void Open() => Open(0, 0);

        private IEnumerator OpenRoutine(int animIndex = 0)
        {
            // must be closed
            if (Status != ElementStatus.Closed)
            {
                yield break;
            }

            if (_setAsLastSiblingOnOpen)
            {
                transform.SetAsLastSibling();
            }

            Status = ElementStatus.Opening;
            BeforeOpen?.Invoke();

            if (AnimationComponent != null)
            {
                yield return AnimationComponent.PlayAnimation(this, AnimationExecuteTime.Open, animIndex);
            }

            if (Status != ElementStatus.Opening) yield break;
            OpenImmediately();

            AfterOpen?.Invoke();

            if (AnimationComponent != null)
            {
                Manager.StartCoroutine(AnimationComponent.PlayAnimation(this, AnimationExecuteTime.Idle, 0));
            }

        }


        private void OpenImmediately()
        {
            if (AnimationComponent != null)
            {
                AnimationComponent.KillActiveAnimation();
            }
            Status = ElementStatus.Opened;
            gameObject.SetActive(true);
            if (_setAsLastSiblingOnOpen)
            {
                transform.SetAsLastSibling();
            }
        }


        public void Close(float delay = 0, int animIndex = 0)
        {
            if (Manager == null)
            {
                ForceClose();
            }
            else
            {
                if (delay <= 0)
                    Manager.StartCoroutine(CloseRoutine(animIndex));
                else
                    Manager.CloseUIElement(this, delay, animIndex);
            }
        }

        public void Close() => Close(0, 0);



        private IEnumerator CloseRoutine(int animIndex = 0)
        {
            // must be opened
            if (Status != ElementStatus.Opened)
            {
                yield break;
            }

            Status = ElementStatus.Closing;
            BeforeClose?.Invoke();

            if (AnimationComponent != null)
            {
                yield return AnimationComponent.PlayAnimation(this, AnimationExecuteTime.Close, animIndex);
            }

            if (Status != ElementStatus.Closing)
            {
                yield break;
            }

            CloseImmediately();

            AfterClose?.Invoke();
        }

        private void CloseImmediately()
        {
            if (AnimationComponent != null)
            {
                AnimationComponent.KillActiveAnimation();
            }
            Status = ElementStatus.Closed;
            gameObject.SetActive(false);
        }



        public void ForceOpen()
        {
            if (AnimationComponent != null)
            {
                AnimationComponent.KillActiveAnimation();
            }
            Status = ElementStatus.Opened;
            gameObject.SetActive(true);
        }

        public void ForceClose()
        {
            if (AnimationComponent != null)
            {
                AnimationComponent.KillActiveAnimation();
            }
            Status = ElementStatus.Closed;
            gameObject.SetActive(false);
        }





    }





}

