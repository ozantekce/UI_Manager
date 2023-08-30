using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ExtendedText : MonoBehaviour, I_UI_Element
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


    public ExtendedTextType textType = ExtendedTextType.TextMeshPro;
    [ShowInEnum("textType", "TextMeshPro")]
    public TextMeshProUGUI textMeshPro;
    [ShowInEnum("textType", "UnityText")]
    public Text unityText;

    private TextMethod textMethod;

    private void Update()
    {
        
        if(TextMethod != null)
        {
            Text = TextMethod.Invoke();
        }

    }




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

    public UIElementType ElementType => UIElementType.Text;

    public string Text { 
        get
        {
            if (textType == ExtendedTextType.TextMeshPro) return textMeshPro.text;
            else if (textType == ExtendedTextType.UnityText) return unityText.text;
            else return null;
        }
        set
        {
            if (textType == ExtendedTextType.TextMeshPro) textMeshPro.text = value;
            else if (textType == ExtendedTextType.UnityText) unityText.text = value;

        }
    
    }

    public TextMethod TextMethod { get => textMethod; set => textMethod = value; }

    #endregion



}


public delegate string TextMethod();

public enum ExtendedTextType { UnityText, TextMeshPro }