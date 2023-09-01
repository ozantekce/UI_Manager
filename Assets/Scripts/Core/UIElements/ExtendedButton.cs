using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExtendedButton : Interactable_UI_Element
{

    [SerializeField]
    private List<OnClickData> _onClicks;

    private Button _button;


    private void Awake()
    {
        _button = GetComponent<Button>();
        if (_button == null)
        {
            _button = gameObject.AddComponent<Button>();
        }
        foreach (OnClickData onClick in _onClicks)
        {
            _button.onClick.AddListener(() => ButtonHelper.Execute(onClick));
        }
    }



    private void Update()
    {


    }




    public override void ConfigurationsAwake()
    {
        base.ConfigurationsAwake();
    }
    public override void ConfigurationsStart()
    {
        base.ConfigurationsStart();
    }


    public override UIElementType ElementType => UIElementType.Button;


    [System.Serializable]
    public class OnClickData
    {

        public OperationType type;
        [ShowInEnum("type", "OpenElement", "CloseElement")]
        public string elementAlias;
        [InterfaceType(typeof(UI_Element))]
        public MonoBehaviour element;
        [ShowInEnum("type", "LoadScene")]
        public string sceneName;

        public float waitToExecute;
        public float waitToReuse;

        public UnityEvent onClickEvent;

        private float _nextClickTime = -1f;

        public string ElementAlias
        {
            get
            {
                if (type == OperationType.LoadScene) return sceneName;
                return elementAlias;
            }
        }

        public UI_Element Element
        {
            get
            {
                if (type == OperationType.LoadScene) return null;
                if(element != null) return (UI_Element) element;
                return elementAlias.GetEntity<UI_Element>();
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
            data.Element.OpenUIElement(data.waitToExecute);
        }

        private static void CloseElement(OnClickData data)
        {
            data.Element.CloseUIElement(data.waitToExecute);
        }

        private static void QuitApplication(OnClickData data)
        {
            UI_Manager.Instance.QuitApplication();
        }

        private static void LoadScene(OnClickData data)
        {
            data.ElementAlias.LoadScene(data.waitToExecute);
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
