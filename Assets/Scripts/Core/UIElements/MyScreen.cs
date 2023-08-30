using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyScreen : MonoBehaviour, I_UI_Element
{

    [SerializeField] private string _alias;

    public UIElementStatus Status { get; set; }

    public MonoBehaviour MonoBehaviour { get => this; set { } }

    [SerializeField]
    private UnityEvent _beforeOpen;
    [SerializeField]
    private UnityEvent _afterOpen;
    [SerializeField]
    private UnityEvent _beforeClose;
    [SerializeField]
    private UnityEvent _afterClose;

    public virtual void ConfigurationsAwake()
    {

    }
    public virtual void ConfigurationsStart()
    {

    }

    #region GetterSetter
    public UnityEvent BeforeOpen { get { return _beforeOpen; } set { _beforeOpen = value; } }
    public UnityEvent AfterOpen { get { return _afterOpen; } set { _afterOpen = value; } }
    public UnityEvent BeforeClose { get { return _beforeClose; } set { _beforeClose = value; } }
    public UnityEvent AfterClose { get { return _afterClose; } set { _afterClose = value; } }

    public string Alias { get => _alias; set => _alias = value; }

    public UIElementType ElementType => UIElementType.Screen;

    #endregion
}
