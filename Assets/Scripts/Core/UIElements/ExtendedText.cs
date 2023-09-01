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

    public bool dontUpdateText;
    [HideInInspectorIf("dontUpdateText")]
    public float updateRate;

    private CooldownDynamic _updateCD;

    public override void ConfigurationsAwake()
    {
        base.ConfigurationsAwake();
        _updateCD = new CooldownDynamic();
    }

    public override void ConfigurationsStart()
    {
        base.ConfigurationsStart();

    }


    private void Update()
    {

        if (!dontUpdateText && TextMethod != null && _updateCD.Ready(updateRate))
        {
            UpdateText();
        }

    }


    public void UpdateText()
    {
        Text = TextMethod.Invoke();
    }






    #region GetterSetter

    public string Text
    {
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