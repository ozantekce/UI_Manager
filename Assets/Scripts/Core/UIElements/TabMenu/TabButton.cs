using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace UI_Manager
{
    public class TabButton : ExtendedButton
    {


        [SerializeField] private TabMenu _menu;
        [SerializeField] private TabPanel _panel;


        [SerializeField] private UiAnimationWrapper _selectedAnimation;
        [SerializeField] private UiAnimationWrapper _nonSelectedAnimation;


        private void OnClickButton()
        {
            Menu.ChangeTabPanel(this);
        }


        public override void ConfigurationsAwake()
        {
            base.ConfigurationsAwake();
            Button.onClick.AddListener(OnClickButton);
        }

        public override void ConfigurationsStart()
        {
            base.ConfigurationsStart();

        }




        public override UIElementType Type => UIElementType.TabButton;

        public TabPanel Panel { get => _panel; set => _panel = value; }
        public TabMenu Menu { get => _menu; set => _menu = value; }
        public UI_Animation SelectedAnimation { get => _selectedAnimation != null ? _selectedAnimation.Animation : null; }
        public UI_Animation NonSelectedAnimation { get => _nonSelectedAnimation != null ? _nonSelectedAnimation.Animation : null; }

    }

}

