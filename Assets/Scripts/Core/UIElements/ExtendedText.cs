using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExtendedText : UI_Element
{


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



    public override void ConfigurationsAwake()
    {
        base.ConfigurationsAwake();
    }

    public override void ConfigurationsStart()
    {
        base.ConfigurationsStart();

    }



    #region GetterSetter

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

    //public override UIElementType ElementType => UIElementType.Text;

    #endregion



}


public delegate string TextMethod();

public enum ExtendedTextType { UnityText, TextMeshPro }