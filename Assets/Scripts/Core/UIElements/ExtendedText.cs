using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI_Manager
{
    public class ExtendedText : UI_Element
    {


        public ExtendedTextType textType = ExtendedTextType.TextMeshPro;
        [ShowInEnum("textType", "TextMeshPro")]
        public TextMeshProUGUI textMeshPro;
        [ShowInEnum("textType", "UnityText")]
        public Text unityText;

        private TextMethod textMethod;

        public bool dontUpdateText = true;
        [HideInInspectorIf("dontUpdateText")]
        public float updateDelay = 0.2f;

        private CooldownDynamic _updateCD;

        public override void ConfigurationsAwake()
        {
            base.ConfigurationsAwake();
            _updateCD = new CooldownDynamic();

            if (textType == ExtendedTextType.TextMeshPro && textMeshPro == null) textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
            if (textType == ExtendedTextType.UnityText && unityText == null) unityText = gameObject.GetComponent<Text>();

        }

        public override void ConfigurationsStart()
        {
            base.ConfigurationsStart();
            if (TextMethod != null)
            {
                UpdateText();
            }

        }


        private void Update()
        {

            if (!dontUpdateText && TextMethod != null && _updateCD.Ready(updateDelay))
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

        public override UIElementType Type => UIElementType.Text;

        #endregion



    }


    public delegate string TextMethod();

    public enum ExtendedTextType { UnityText, TextMeshPro }

}

