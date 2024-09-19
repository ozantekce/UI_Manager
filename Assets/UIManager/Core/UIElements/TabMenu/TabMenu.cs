using UnityEngine;


namespace UIManager
{
    public class TabMenu : UIElement
    {

        [field: SerializeField] public TabButton CurrentTabButton { get; protected set; }
        [field: SerializeField] public TabPanel CurrentTabPanel { get; protected set; }

        public override void Initialize()
        {
            base.Initialize();

            if (CurrentTabButton != null)
            {
                CurrentTabPanel = CurrentTabButton.TabPanel;
                CurrentTabPanel.transform.SetAsLastSibling();
                if (CurrentTabButton.SelectedAnimation != null)
                    UIManager.Instance.StartCoroutine(CurrentTabButton.SelectedAnimation.Animation.Enumerator(CurrentTabButton));
                CurrentTabPanel.Open(0, 0);
            }
        }


        public bool ChangeTabPanel(TabButton tabButton)
        {
            if (CurrentTabPanel == tabButton.TabPanel)
                return false;

            if (CurrentTabButton != null)
            {
                if (CurrentTabButton.NonSelectedAnimation != null) 
                    StartCoroutine(CurrentTabButton.NonSelectedAnimation.Animation.Enumerator(CurrentTabButton));
                CurrentTabPanel.Close(0, 0);
            }
            CurrentTabButton = tabButton;
            CurrentTabPanel = CurrentTabButton.TabPanel;

            if (CurrentTabButton.SelectedAnimation != null) 
                StartCoroutine(CurrentTabButton.SelectedAnimation.Animation.Enumerator(CurrentTabButton));
            CurrentTabPanel.Open(0, 0);
            return true;

        }


    }

}
