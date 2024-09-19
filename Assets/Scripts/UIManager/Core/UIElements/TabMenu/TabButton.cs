using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIManager
{
    public class TabButton : UIExtendedButton
    {


        [SerializeField] private TabMenu _menu;
        [SerializeField] private TabPanel _panel;


        [SerializeField] private UIAnimationWrapper _selectedAnimation;
        [SerializeField] private UIAnimationWrapper _nonSelectedAnimation;


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
        public BaseUIAnimation SelectedAnimation { get => _selectedAnimation != null ? _selectedAnimation.Animation : null; }
        public BaseUIAnimation NonSelectedAnimation { get => _nonSelectedAnimation != null ? _nonSelectedAnimation.Animation : null; }

    }

}

