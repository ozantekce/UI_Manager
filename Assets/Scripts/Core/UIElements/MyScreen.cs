using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyScreen : MonoBehaviour, I_UI_Element
{

    [SerializeField] private string _alias;
    [SerializeField] private UIElementStatus _status;

    private I_UI_Element _parent;
    private List<I_UI_Element> _childs;



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

    public void ConfigurationsAwake_()
    {

    }
    public void ConfigurationsStart_()
    {

    }

    #region GetterSetter

    public UIElementStatus Status { get => _status; set => _status = value; }

    public MonoBehaviour MonoBehaviour { get => this; }

    public UnityEvent BeforeOpen { get { return _beforeOpen; } set { _beforeOpen = value; } }
    public UnityEvent AfterOpen { get { return _afterOpen; } set { _afterOpen = value; } }
    public UnityEvent BeforeClose { get { return _beforeClose; } set { _beforeClose = value; } }
    public UnityEvent AfterClose { get { return _afterClose; } set { _afterClose = value; } }

    public string Alias { get => _alias; set => _alias = value; }
    public UIElementType ElementType => UIElementType.Screen;

    public I_UI_Element Parent { get =>_parent; set => _parent = value; }

    public List<I_UI_Element> Childs { get =>  _childs; set => _childs = value;}

    public UI_Animation OpenAnimation { get => _openAnimation; set => _openAnimation = value; }
    public UI_Animation CloseAnimation { get => _closeAnimation; set => _closeAnimation = value; }

    #endregion
}
