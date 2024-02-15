using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace UI_Manager
{
    public class ExtendedButton : Interactable_UI_Element
    {

        [SerializeField]
        private List<OnClickData> _onClicks;

        private Button _button;



        private Action _baseOnClick;
        private float _baseOnClickWaitReuseTime = 0.2f;
        private float _baseOnClickLastUseTime;


        public override void ConfigurationsAwake()
        {
            base.ConfigurationsAwake();
            if (!TryGetComponent(out _button))
            {
                _button = gameObject.AddComponent<Button>();
            }
            _button.onClick.AddListener(BaseOnClick);
            foreach (OnClickData onClick in _onClicks)
            {
                _button.onClick.AddListener(() => ButtonHelper.Execute(onClick));
            }
        }
        public override void ConfigurationsStart()
        {
            base.ConfigurationsStart();
        }



        private void BaseOnClick()
        {
            float elapsedTime = (Time.time - _baseOnClickLastUseTime);
            if (elapsedTime > _baseOnClickWaitReuseTime)
            {
                _baseOnClick?.Invoke();
                _baseOnClickLastUseTime = Time.time;
            }

        }

        protected override void Update()
        {
            base.Update();
        }


        public override UIElementType Type => UIElementType.Button;

        public Button Button { 
            get { 
                if(_button == null)
                {
                    _button = gameObject.GetOrAddComponent<Button>();
                }
                return _button;
            } 
        }

        public bool Interactable
        {
            get
            {
                if (_button == null)
                {
                    _button = gameObject.GetOrAddComponent<Button>();
                }
                if (_button == null)
                    return false;
                return _button.interactable ;
            }
            set
            {
                if (_button == null)
                {
                    _button = gameObject.GetOrAddComponent<Button>();
                }
                if (_button != null)
                    _button.interactable = value;
            }

        }


        public Action OnClick { get => _baseOnClick; set => _baseOnClick = value; }
        public float BaseWaitToReuseTime { get => _baseOnClickWaitReuseTime; set => _baseOnClickWaitReuseTime = value; }


        [System.Serializable]
        public class OnClickData
        {

            public OperationType type;
            [ShowInEnum("type", "OpenElement", "CloseElement")]
            public string elementName;
            [ShowInEnum("type", "OpenElement", "CloseElement")]
            public UI_Element element;
            [ShowInEnum("type", "LoadScene")]
            public string sceneName;
            [ShowInEnum(nameof(type), nameof(OperationType.OpenElement), nameof(OperationType.CloseElement))]
            public int animIndex = 0;

            public float waitToExecute;
            public float waitToReuse;

            public UnityEvent onClickEvent;

            private float _nextClickTime = -1f;

            public string ElementName
            {
                get
                {
                    if (type == OperationType.LoadScene) return sceneName;
                    return elementName;
                }
            }

            public UI_Element Element
            {
                get
                {
                    if (type == OperationType.LoadScene) return null;
                    if (element != null) return element;
                    return elementName.NameToUiElement<UI_Element>();
                }

            }


            public float NextClickTime { get { return _nextClickTime; } set { _nextClickTime = value; } }

        }


        public static class ButtonHelper
        {

            private static Dictionary<OperationType, OperationMethod>
                _typeMethodPairs = new Dictionary<OperationType, OperationMethod>() {
                {OperationType.None ,               None },
                {OperationType.OpenElement  ,       OpenElement },
                {OperationType.CloseElement ,       CloseElement },
                {OperationType.QuitApplication ,    QuitApplication },
                {OperationType.LoadScene ,          LoadScene }
                };


            public static void Execute(OnClickData data)
            {
                if (data.NextClickTime > Time.time) return;
                data.onClickEvent?.Invoke();
                data.NextClickTime = Time.time + data.waitToReuse;
                _typeMethodPairs[data.type](data);

            }


            private static void OpenElement(OnClickData data)
            {
                data.Element.OpenUIElement(data.waitToExecute, data.animIndex);
            }

            private static void CloseElement(OnClickData data)
            {
                data.Element.CloseUIElement(data.waitToExecute, data.animIndex);
            }

            private static void QuitApplication(OnClickData data)
            {
                UI_Manager.Instance.QuitApplication();
            }

            private static void LoadScene(OnClickData data)
            {
                data.ElementName.LoadScene(data.waitToExecute);
            }
            private static void None(OnClickData data)
            {

            }

            private delegate void OperationMethod(OnClickData data);


        }


        public enum OperationType
        {
            None, OpenElement, CloseElement, LoadScene
            , QuitApplication

        }



    }

}
