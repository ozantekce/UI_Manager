using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UI_Manager
{

    public class ProgressBar : UI_Element
    {


        [SerializeField] private Image _fillArea;

        [SerializeField][Range(0, 100)] private float _fillSpeed = 50f;

        [SerializeField] private float _targetValue;

        private Func<float> _targetValueMethod;

        public override void ConfigurationsAwake()
        {
            base.ConfigurationsAwake();
        }

        public override void ConfigurationsStart()
        {
            base.ConfigurationsStart();
        }



        protected override void Update()
        {
            base.Update();
            if (TargetValue != CurrentValue)
            {
                float val = CurrentValue;
                if (TargetValue > CurrentValue)
                {
                    val += FillDelta * Time.deltaTime;
                    if (val > TargetValue)
                    {
                        val = TargetValue;
                    }
                }
                else
                {
                    val -= FillDelta * Time.deltaTime;
                    if (val < TargetValue)
                    {
                        val = TargetValue;
                    }
                }
                CurrentValue = val;

            }
        }



        private float FillDelta { get { return _fillSpeed / 50f; } }
        public float CurrentValue { get => _fillArea.fillAmount; set => _fillArea.fillAmount = value; }

        public float TargetValue { get { return _targetValueMethod == null ? _targetValue : _targetValueMethod.Invoke(); } set => _targetValue = value; }


        public override UIElementType Type => UIElementType.ProgressBar;

        public Func<float> TargetValueMethod { get => _targetValueMethod; set => _targetValueMethod = value; }
    }

}

