using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace UIManager
{
    public class UIExtendedButton : InteractableUIElement
    {

        public override UIElementType Type => UIElementType.Button;

        [SerializeField] private List<OnClickData> _onClicks;

        private Button _button;

        private Action _baseOnClick;
        private float _baseOnClickWaitReuseTime = 0.2f;
        private float _baseOnClickLastUseTime;


        public override void ConfigurationsAwake()
        {
            base.ConfigurationsAwake();
            _button = gameObject.GetOrAddComponent<Button>();
            _button.onClick.AddListener(BaseOnClick);
            foreach (OnClickData onClick in _onClicks)
            {
                _button.onClick.AddListener(() => ButtonHelper.Execute(onClick));
            }
        }
        public override void ConfigurationsStart()
        {
            base.ConfigurationsStart();
        }



        private void BaseOnClick()
        {
            float elapsedTime = (Time.time - _baseOnClickLastUseTime);
            if (elapsedTime > _baseOnClickWaitReuseTime)
            {
                _baseOnClick?.Invoke();
                _baseOnClickLastUseTime = Time.time;
            }

        }

        protected override void Update()
        {
            base.Update();
        }


        

        public Button Button => _button != null ? _button : (_button = gameObject.GetOrAddComponent<Button>());


        public bool Interactable { get => Button.interactable; set => Button.interactable = value; }

        public Action OnClick { get => _baseOnClick; set => _baseOnClick = value; }
        public float BaseWaitToReuseTime { get => _baseOnClickWaitReuseTime; set => _baseOnClickWaitReuseTime = value; }


    }

}
