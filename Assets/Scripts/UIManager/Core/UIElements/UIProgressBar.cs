using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIManager
{
    public class UIProgressBar : UIElement
    {

        public override UIElementType Type => UIElementType.ProgressBar;

        [SerializeField] private Image _image;
        [SerializeField] private Scrollbar _scrollbar;

        [SerializeField][Range(0, 1)] private float _fillSpeed = 0.5f;
        private float _targetValue;

        private Func<float> _targetValueMethod;

        public override void ConfigurationsAwake()
        {
            base.ConfigurationsAwake();
        }

        protected override void Update()
        {
            base.Update();
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            float targetValue = _targetValueMethod != null ? _targetValueMethod() : _targetValue;
            if (_image != null)
            {
                _image.fillAmount = Mathf.MoveTowards(_image.fillAmount, targetValue, FillDelta * Time.deltaTime);
            }
            else if (_scrollbar != null)
            {
                _scrollbar.value = Mathf.MoveTowards(_scrollbar.value, targetValue, FillDelta * Time.deltaTime);
            }
        }

        private float FillDelta => _fillSpeed;

        public Func<float> TargetValueMethod { get => _targetValueMethod; set => _targetValueMethod = value; }

        public float TargetValue { get => _targetValue; set => _targetValue = value; }

    }
}
