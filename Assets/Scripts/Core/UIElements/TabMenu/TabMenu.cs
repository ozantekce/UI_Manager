using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI_Manager
{
    public class TabMenu : UI_Element
    {


        [SerializeField] private TabButton _currentTabButton;
        private TabPanel _currentTabPanel;

        public override void ConfigurationsAwake()
        {
            base.ConfigurationsAwake();

        }

        public override void ConfigurationsStart()
        {
            base.ConfigurationsStart();
            if (_currentTabButton != null)
            {
                _currentTabPanel = _currentTabButton.Panel;
                _currentTabPanel.transform.SetAsLastSibling();
                if (_currentTabButton.SelectedAnimation != null) UI_Manager.Instance.StartCoroutine(_currentTabButton.SelectedAnimation.Enumerator(_currentTabButton));
                _currentTabPanel.Open(0, 0);
            }
        }


        public bool ChangeTabPanel(TabButton tabButton)
        {
            if (_currentTabPanel == tabButton.Panel) return false;
            if (_currentTabButton != null)
            {
                if (_currentTabButton.NonSelectedAnimation != null) StartCoroutine(_currentTabButton.NonSelectedAnimation.Enumerator(_currentTabButton));
                _currentTabPanel.Close(0, 0);
            }
            _currentTabButton = tabButton;
            _currentTabPanel = _currentTabButton.Panel;

            if (_currentTabButton.SelectedAnimation != null) StartCoroutine(_currentTabButton.SelectedAnimation.Enumerator(_currentTabButton));
            _currentTabPanel.Open(0, 0);
            return true;

        }




        public override UIElementType Type => UIElementType.TabMenu;

        public TabButton CurrentTabButton { get => _currentTabButton; }
    }

}
