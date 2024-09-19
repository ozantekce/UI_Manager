using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIManager
{
    public class UIExtendedText : UIElement
    {
        public override UIElementType Type => UIElementType.Text;


        public ExtendedTextType textType = ExtendedTextType.TextMeshPro;

        [SerializeField, ShowInEnum(nameof(textType), nameof(ExtendedTextType.TextMeshPro))] private TextMeshProUGUI _textMeshPro;
        [SerializeField, ShowInEnum(nameof(textType), nameof(ExtendedTextType.UnityText))]   private Text _unityText;

        private Func<string> _textMethod;

        [SerializeField] private float _updateDelay = 0.2f;

        private CooldownDynamic _updateCD;

        public override void ConfigurationsAwake()
        {
            base.ConfigurationsAwake();
            _updateCD = new CooldownDynamic();

            if (textType == ExtendedTextType.TextMeshPro)
            {
                if(_textMeshPro == null)
                    _textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
            }
            else if (textType == ExtendedTextType.UnityText)
            {
                if(_unityText == null)
                    _unityText = gameObject.GetComponent<Text>();
            }

        }

        public override void ConfigurationsStart()
        {
            base.ConfigurationsStart();
            if (TextMethod != null)
            {
                UpdateTextWithMethod();
            }

        }

        protected override void Update()
        {
            base.Update();

            if (_updateDelay > 0f && TextMethod != null && _updateCD.Ready(_updateDelay))
            {
                UpdateTextWithMethod();
            }

        }


        public void UpdateTextWithMethod()
        {
            Text = TextMethod.Invoke();
        }

        public void UpdateText(string text, bool removeTextMethod = true)
        {
            if(removeTextMethod && _updateDelay > 0f && TextMethod != null)
            {
                RemoveTextMethod();
            }
            Text = text;
        }


        public void SetTextMethod(Func<string> textMethod)
        {
            _textMethod = textMethod;
        }

        public void RemoveTextMethod()
        {
            _textMethod = null;
        }


        #region GetterSetter

        public string Text
        {
            get
            {
                if (textType == ExtendedTextType.TextMeshPro) return _textMeshPro.text;
                else if (textType == ExtendedTextType.UnityText) return _unityText.text;
                else return null;
            }
            set
            {
                if (textType == ExtendedTextType.TextMeshPro) _textMeshPro.text = value;
                else if (textType == ExtendedTextType.UnityText) _unityText.text = value;

            }

        }

        public Func<string> TextMethod { get => _textMethod; private set => _textMethod = value; }

        

        #endregion



    }

    

}

