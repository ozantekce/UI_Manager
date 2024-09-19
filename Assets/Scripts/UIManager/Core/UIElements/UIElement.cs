using System.Collections;
using UnityEngine;
using UnityEngine.Events;


namespace UIManager
{

    public class UIElement : MonoBehaviour
    {

        [SerializeField] private UIManager _manager;

        [SerializeField] private string _name;
        [SerializeField, ReadOnly] private UIElementStatus _status;
        [SerializeField] bool _setAsLastSiblingOnOpen = true;
        [SerializeField] bool _startWithOpenAnimation = false;

        #region Events
        [SerializeField] private UnityEvent _beforeOpen;
        [SerializeField] private UnityEvent _afterOpen;
        [SerializeField] private UnityEvent _beforeClose;
        [SerializeField] private UnityEvent _afterClose;
        #endregion


        private RectTransform _rectTransform;
        private bool _triedFindToAnimationComponent = false;
        protected UIAnimationComponent _animationComponent;

        protected ElementInfo? _elementInfo = null;

        private bool _configurationsAwakeExecuted;
        private bool _configurationsStartExecuted;


        protected virtual void Awake()
        {
            if (!_configurationsAwakeExecuted)
            {
                if (Manager != null)
                {
                    Manager.AddElement(this);
                    ConfigurationsAwake();
                }
            }
        }

        protected virtual void Start()
        {
            ElementInfo info = new ElementInfo(this);
            _elementInfo = info;
            if (!_configurationsStartExecuted)
            {
                if (Manager != null)
                {
                    ConfigurationsStart();
                }
            }

            if (_startWithOpenAnimation)
            {
                Manager.StartCoroutine(AnimationComponent.PlayAnimation(this, AnimationExecuteTime.Open, 0));
            }
        }


        protected virtual void Update()
        {
            if(_elementInfo == null)
            {
                ElementInfo info = new ElementInfo(this);
                _elementInfo = info;
            }
        }


        protected virtual void OnDestroy()
        {
            if(Manager != null)
            {
                Manager.RemoveElement(this);
            }
        }


        public virtual void ConfigurationsAwake()
        {
            _configurationsAwakeExecuted = true;

            if (gameObject.activeSelf)
            {
                Status = UIElementStatus.Opened;
                if (AnimationComponent != null)
                {
                    StartCoroutine(AnimationComponent.PlayAnimation(this, AnimationExecuteTime.Idle, 0));
                }
            }
            else
            {
                Status = UIElementStatus.Closed;
            }

        }

        public virtual void ConfigurationsStart()
        {
            _configurationsStartExecuted = true;
        }


        public virtual void UpdateUI(params object[] parameters)
        {

        }


        public void Open(float delay = 0f, int animIndex = 0)
        {
            if(Manager == null)
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

        public void Open()
        {
            Open(0, 0);
        }


        private IEnumerator OpenRoutine(int animIndex = 0)
        {
           
            // must be closed
            if (Status != UIElementStatus.Closed)
            {
                yield break;
            }

            if (_setAsLastSiblingOnOpen)
            {
                transform.SetAsLastSibling();
            }

            Status = UIElementStatus.Opening;
            BeforeOpen?.Invoke();

            if (AnimationComponent != null)
            {
                yield return AnimationComponent.PlayAnimation(this, AnimationExecuteTime.Open, animIndex);
            }

            if (Status != UIElementStatus.Opening) yield break;
            OpenImmediately();

            AfterOpen?.Invoke();

            if (AnimationComponent != null)
            {
                Manager.StartCoroutine(AnimationComponent.PlayAnimation(this, AnimationExecuteTime.Idle, 0));
            }

        }


        private void OpenImmediately()
        {
            //Debug.Log("OPEN");
            Status = UIElementStatus.Opened;
            gameObject.SetActive(true);
            if (_setAsLastSiblingOnOpen)
            {
                transform.SetAsLastSibling();
            }
        }


        public void Close(float delay = 0, int animIndex = 0)
        {
            if(Manager == null)
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

        public void Close()
        {
            Close(0, 0);
        }



        private IEnumerator CloseRoutine(int animIndex = 0)
        {
            // must be opened
            if (Status != UIElementStatus.Opened)
            {
                yield break;
            }

            Status = UIElementStatus.Closing;
            BeforeClose?.Invoke();

            if (AnimationComponent != null)
            {
                yield return AnimationComponent.PlayAnimation(this, AnimationExecuteTime.Close, animIndex);
            }

            if (Status != UIElementStatus.Closing)
            {
                yield break;
            }

            CloseImmediately();

            AfterClose?.Invoke();
        }

        private void CloseImmediately()
        {
            //Debug.Log("CLOSE");
            Status = UIElementStatus.Closed;
            gameObject.SetActive(false);
        }



        public void ForceOpen()
        {
            if(AnimationComponent != null)
            {
                AnimationComponent.KillActiveAnimation();
            }
            Status = UIElementStatus.Opened;
            gameObject.SetActive(true);
        }

        public void ForceClose()
        {
            if(AnimationComponent != null)
            {
                AnimationComponent.KillActiveAnimation();
            }
            Status = UIElementStatus.Closed;
            gameObject.SetActive(false);
        }




        #region GetterSetter
        public virtual UIElementType Type => UIElementType.None;
        public string Name => _name;

        public UIElementStatus Status { get => _status; set => _status = value; }
        public UnityEvent BeforeOpen { get => _beforeOpen; set => _beforeOpen = value; }
        public UnityEvent AfterOpen { get => _afterOpen; set => _afterOpen = value; }
        public UnityEvent BeforeClose { get => _beforeClose; set => _beforeClose = value; }
        public UnityEvent AfterClose { get => _afterClose; set => _afterClose = value; }

        public UIAnimationComponent AnimationComponent => _triedFindToAnimationComponent ? _animationComponent :
            (_triedFindToAnimationComponent = true, _animationComponent = GetComponent<UIAnimationComponent>()).Item2;

        public ElementInfo? ElementInfo { get => _elementInfo; }
        public RectTransform RectTransform { get => _rectTransform != null ? _rectTransform: _rectTransform = GetComponent<RectTransform>(); }

        public UIManager Manager => _manager == null ? _manager = UIManager.Instance : _manager;

        public GameObject GameObject => gameObject;


        #endregion



    }





}

