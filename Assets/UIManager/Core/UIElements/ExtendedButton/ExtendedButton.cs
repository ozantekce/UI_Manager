using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UIManager
{
    public class ExtendedButton : InteractableUIElement
    {

        [field: SerializeField] private List<OnClickData> OnClickData { get; set; }

        private Button _button;

        private Action _baseOnClick;
        private float _baseOnClickWaitReuseTime = 0.2f;
        private float _baseOnClickLastUseTime;


        public override void Initialize()
        {
            base.Initialize();
            _button = gameObject.GetOrAddComponent<Button>();
            _button.onClick.AddListener(BaseOnClick);
            _button.onClick.AddListener(ExecuteAllOnClicks);
        }


        private void ExecuteAllOnClicks()
        {
            foreach (OnClickData onClick in OnClickData)
            {
                ButtonHelper.Execute(onClick);
            }
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
