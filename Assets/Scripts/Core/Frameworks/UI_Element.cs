using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UI_Manager
{

    public class UI_Element : MonoBehaviour
    {

        [SerializeField] private string _name;
        private UIElementType _type = UIElementType.None;
        [SerializeField][ReadOnly] private UIElementStatus _status;


        private UI_Element _parent;
        private List<UI_Element> _childs;



        [SerializeField]
        private UnityEvent _beforeOpen;
        [SerializeField]
        private UnityEvent _afterOpen;
        [SerializeField]
        private UnityEvent _beforeClose;
        [SerializeField]
        private UnityEvent _afterClose;

        [SerializeField]
        private UI_Animation _openAnimation;
        [SerializeField]
        private UI_Animation _closeAnimation;


        private bool _configurationsAwake;
        private bool _configurationsStart;


        protected virtual void Awake()
        {
            if (!_configurationsAwake)
            {
                if (UI_Manager.Instance != null)
                {
                    UI_Manager.Instance.AddElement(this);
                    ConfigurationsAwake();
                }

            }
        }

        protected virtual void Start()
        {
            if (!_configurationsStart)
            {
                if (UI_Manager.Instance != null)
                {
                    ConfigurationsStart();
                }

            }
        }


        protected virtual void OnDestroy()
        {
            UI_Manager.Instance.RemoveElement(this);
        }


        public virtual void ConfigurationsAwake()
        {
            _configurationsAwake = true;
            Transform parentTransform = transform.parent;
            if (parentTransform != null) Parent = parentTransform.GetComponent<UI_Element>();

            Childs = new List<UI_Element>();
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<UI_Element>(out var c))
                {
                    Childs.Add(c);
                }
            }

            if (gameObject.activeSelf)
            {
                Status = UIElementStatus.Opened;
            }
            else
            {
                Status = UIElementStatus.Closed;
            }


        }

        public virtual void ConfigurationsStart()
        {
            _configurationsStart = true;
        }



        // don't use it on unity's update method
        public virtual void UpdateUI(params object[] parameters)
        {

        }


        public void Open(float delay = 0)
        {
            if (delay <= 0)
                UI_Manager.Instance.StartCoroutine(OpenRoutine());
            else
                UI_Manager.Instance.OpenUIElement(this, delay);
        }


        private IEnumerator OpenRoutine()
        {
            transform.SetAsLastSibling();
            // must be closed
            if (Status != UIElementStatus.Closed) yield break;

            Status = UIElementStatus.Opening;
            BeforeOpen?.Invoke();

            if (OpenAnimation != null) yield return OpenAnimation.Enumerator(this);

            if (Status != UIElementStatus.Opening) yield break;
            OpenNow();

            AfterOpen?.Invoke();
        }


        private void OpenNow()
        {
            //Debug.Log("OPEN");
            Status = UIElementStatus.Opened;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }


        public void Close(float delay = 0)
        {
            if (delay <= 0)
                UI_Manager.Instance.StartCoroutine(CloseRoutine());
            else
                UI_Manager.Instance.OpenUIElement(this, delay);
        }



        private IEnumerator CloseRoutine()
        {
            // must be opened
            if (Status != UIElementStatus.Opened) yield break;

            Status = UIElementStatus.Closing;
            BeforeClose?.Invoke();

            if (CloseAnimation != null) yield return CloseAnimation.Enumerator(this);

            if (Status != UIElementStatus.Closing) yield break;
            CloseNow();

            AfterClose?.Invoke();
        }

        private void CloseNow()
        {
            //Debug.Log("CLOSE");
            Status = UIElementStatus.Closed;
            gameObject.SetActive(false);
        }



        public void ForceOpen()
        {
            UI_Manager.Instance.ForceOpenUIElement(this);
        }

        public void ForceClose()
        {
            UI_Manager.Instance.ForceCloseUIElement(this);
        }




        #region GetterSetter
        public virtual UIElementType Type => _type;

        public UIElementStatus Status { get => _status; set => _status = value; }
        public UnityEvent BeforeOpen { get { return _beforeOpen; } set { _beforeOpen = value; } }
        public UnityEvent AfterOpen { get { return _afterOpen; } set { _afterOpen = value; } }
        public UnityEvent BeforeClose { get { return _beforeClose; } set { _beforeClose = value; } }
        public UnityEvent AfterClose { get { return _afterClose; } set { _afterClose = value; } }

        public string Name { get => _name; }

        public UI_Element Parent { get => _parent; set => _parent = value; }

        public List<UI_Element> Childs { get => _childs; set => _childs = value; }

        public UI_Animation OpenAnimation { get => _openAnimation; set => _openAnimation = value; }
        public UI_Animation CloseAnimation { get => _closeAnimation; set => _closeAnimation = value; }

        #endregion



    }


    public enum UIElementStatus { Closed, Closing, Opened, Opening }

    public enum UIElementType { None, Screen, PopUp, Text, Button, TabMenu, TabPanel, TabButton, ProgressBar }
}

