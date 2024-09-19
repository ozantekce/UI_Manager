using EditorTools;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIManager
{
    public class ExtendedText : UIElement
    {
        private CooldownDynamic _updateCD;

        [field: SerializeField] public ExtendedTextType TextType { get; private set; }
        [field: SerializeField] public float UpdateDelay { get; private set; }

        [SerializeField, ShowInEnum(nameof(TextType), nameof(ExtendedTextType.TextMeshPro))] private TextMeshProUGUI _textMeshPro;
        [SerializeField, ShowInEnum(nameof(TextType), nameof(ExtendedTextType.UnityText))] private Text _unityText;

        public Func<string> TextMethod { get; private set; }

        public string Text
        {
            get
            {
                if (TextType == ExtendedTextType.TextMeshPro) return _textMeshPro.text;
                else if (TextType == ExtendedTextType.UnityText) return _unityText.text;
                else return null;
            }
            set
            {
                if (TextType == ExtendedTextType.TextMeshPro) _textMeshPro.text = value;
                else if (TextType == ExtendedTextType.UnityText) _unityText.text = value;

            }

        }

        public override void Initialize()
        {
            base.Initialize();
            _updateCD = new CooldownDynamic();

            if (TextType == ExtendedTextType.TextMeshPro)
            {
                if (_textMeshPro == null)
                    _textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
            }
            else if (TextType == ExtendedTextType.UnityText)
            {
                if (_unityText == null)
                    _unityText = gameObject.GetComponent<Text>();
            }

        }

        protected override void Update()
        {
            base.Update();

            if (UpdateDelay > 0f && TextMethod != null && _updateCD.Ready(UpdateDelay))
            {
                ExecuteTextMethod();
            }

        }


        public void ExecuteTextMethod()
        {
            Text = TextMethod.Invoke();
        }

        public void SetText(string text, bool removeTextMethod = true)
        {
            if (removeTextMethod && UpdateDelay > 0f && TextMethod != null)
            {
                RemoveTextMethod();
            }
            Text = text;
        }


        public void SetTextMethod(Func<string> textMethod)
        {
            TextMethod = textMethod;
        }

        public void RemoveTextMethod()
        {
            TextMethod = null;
        }


    }



}

