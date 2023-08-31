using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public interface I_UI_Element : IAliasEntity
{


    public I_UI_Element Parent { get; set; }
    public List<I_UI_Element> Childs { get; set; }

    public UIElementType ElementType { get; }
    public UIElementStatus Status { get; set; }
    public MonoBehaviour MonoBehaviour { get; }

    #region Events
    public UnityEvent BeforeOpen { get; set; }
    public UnityEvent AfterOpen { get; set; }

    public UnityEvent BeforeClose { get; set; }
    public UnityEvent AfterClose { get; set; }
    #endregion



    public void ConfigurationsAwake()
    {
        Transform parentTransform = MonoBehaviour.transform.parent;
        if (parentTransform == null)
        {
            Parent = parentTransform.GetComponent<I_UI_Element>();
        }
        Childs = new List<I_UI_Element>();
        foreach (Transform child in MonoBehaviour.transform)
        {
            I_UI_Element c = child.GetComponent<I_UI_Element>();
            if (c != null)
            {
                Childs.Add(c);
            }
        }

        if (MonoBehaviour.gameObject.activeSelf)
        {
            Status = UIElementStatus.Opened;
        }
        else
        {
            Status = UIElementStatus.Closed;
        }

        ConfigurationsAwake_();
    }

    public void ConfigurationsStart()
    {
        ConfigurationsStart_();
    }

    public void ConfigurationsAwake_();
    public void ConfigurationsStart_();


    public void Open()
    {
        UI_Manager.Instance.StartCoroutine(OpenRoutine());
    }
    private IEnumerator OpenRoutine()
    {
        // must be closed
        if (Status != UIElementStatus.Closed) yield break;

        Status = UIElementStatus.Opening;
        if (BeforeOpen != null) BeforeOpen.Invoke();

        if (Status != UIElementStatus.Opening) yield break;
        OpenNow();

        if (AfterOpen != null) AfterOpen.Invoke();
    }


    private void OpenNow()
    {
        //Debug.Log("OPEN");
        Status = UIElementStatus.Opened;
        MonoBehaviour.gameObject.SetActive(true);
        MonoBehaviour.transform.SetAsLastSibling();
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
        if (BeforeClose != null) BeforeClose.Invoke();

        if (Status != UIElementStatus.Closing) yield break;
        CloseNow();

        if (AfterClose != null) AfterClose.Invoke();
    }

    private void CloseNow()
    {
        //Debug.Log("CLOSE");
        Status = UIElementStatus.Closed;
        MonoBehaviour.gameObject.SetActive(false);
    }


}


public enum UIElementStatus { Closed, Closing, Opened, Opening }

public enum UIElementType { None, Screen, PopUp, Text, Button }