using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExtendedButton : MonoBehaviour, I_UI_Element
{
    [SerializeField] private string _alias;
    [SerializeField] private UIElementStatus _status;



    private I_UI_Element _parent;
    private List<I_UI_Element> _childs;



    [SerializeField]
    private List<OnClickData> _onClicks;

    private Button _button;



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




    public void ConfigurationsAwake_()
    {

    }
    public void ConfigurationsStart_()
    {

    }



    [System.Serializable]
    public class OnClickData
    {

        public OperationType type;
        [ShowInEnum("type", "OpenElement", "CloseElement")]
        public string elementAlias;
        [InterfaceType(typeof(I_UI_Element))]
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

        public I_UI_Element Element
        {
            get
            {
                if (type == OperationType.LoadScene) return null;
                if(element != null) return (I_UI_Element) element;
                return elementAlias.GetEntity<I_UI_Element>();
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



    #region GetterSetter

    public UIElementStatus Status { get => _status; set => _status = value; }

    public MonoBehaviour MonoBehaviour { get => this; }
    public UnityEvent BeforeOpen { get { return _beforeOpen; } set { _beforeOpen = value; } }
    public UnityEvent AfterOpen { get { return _afterOpen; } set { _afterOpen = value; } }
    public UnityEvent BeforeClose { get { return _beforeClose; } set { _beforeClose = value; } }
    public UnityEvent AfterClose { get { return _afterClose; } set { _afterClose = value; } }

    public string Alias { get => _alias; set => _alias = value; }
    public UIElementType ElementType => UIElementType.Button;

    public I_UI_Element Parent { get => _parent; set => _parent = value; }

    public List<I_UI_Element> Childs { get => _childs; set => _childs = value; }

    public UI_Animation OpenAnimation { get => _openAnimation; set => _openAnimation = value; }
    public UI_Animation CloseAnimation { get => _closeAnimation; set => _closeAnimation = value; }

    #endregion

}
