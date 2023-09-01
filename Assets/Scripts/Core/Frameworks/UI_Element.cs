using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UI_Element : MonoBehaviour, IAliasEntity
{

    [SerializeField] private string _alias;
    [SerializeField] private UIElementStatus _status;

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




    public virtual void ConfigurationsAwake()
    {
        Transform parentTransform = transform.parent;
        if (parentTransform == null)
        {
            Parent = parentTransform.GetComponent<UI_Element>();
        }
        Childs = new List<UI_Element>();
        foreach (Transform child in transform)
        {
            UI_Element c = child.GetComponent<UI_Element>();
            if (c != null)
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

    }



    public void Open()
    {
        UI_Manager.Instance.StartCoroutine(OpenRoutine());
    }
    private IEnumerator OpenRoutine()
    {
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


    public void Close()
    {
        UI_Manager.Instance.StartCoroutine(CloseRoutine());
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




    #region GetterSetter
    public virtual UIElementType ElementType => UIElementType.None;

    public UIElementStatus Status { get => _status; set => _status = value; }
    public UnityEvent BeforeOpen { get { return _beforeOpen; } set { _beforeOpen = value; } }
    public UnityEvent AfterOpen { get { return _afterOpen; } set { _afterOpen = value; } }
    public UnityEvent BeforeClose { get { return _beforeClose; } set { _beforeClose = value; } }
    public UnityEvent AfterClose { get { return _afterClose; } set { _afterClose = value; } }

    public string Alias { get => _alias; set => _alias = value; }

    public UI_Element Parent { get => _parent; set => _parent = value; }

    public List<UI_Element> Childs { get => _childs; set => _childs = value; }

    public UI_Animation OpenAnimation { get => _openAnimation; set => _openAnimation = value; }
    public UI_Animation CloseAnimation { get => _closeAnimation; set => _closeAnimation = value; }

    #endregion



}


public enum UIElementStatus { Closed, Closing, Opened, Opening }

public enum UIElementType { None, Screen, PopUp, Text, Button }