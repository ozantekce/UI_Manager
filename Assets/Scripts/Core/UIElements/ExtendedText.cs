using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExtendedText : MonoBehaviour, I_UI_Element
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
    public I_UI_Element Parent { get => _parent; set => _parent = value; }

    public List<I_UI_Element> Childs { get => _childs; set => _childs = value; }

    #endregion



}


public delegate string TextMethod();

public enum ExtendedTextType { UnityText, TextMeshPro }