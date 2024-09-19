using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIManager
{
    public class TabButton : ExtendedButton
    {

        [field: SerializeField] public AnimationWrapper SelectedAnimation { get; private set; }
        [field: SerializeField] public AnimationWrapper NonSelectedAnimation { get; private set; }


        [field: SerializeField] public TabPanel TabPanel { get; set; }
        [field: SerializeField] public TabMenu TabMenu { get; set; }





        private void OnClickButton()
        {
            TabMenu.ChangeTabPanel(this);
        }


        public override void Initialize()
        {
            base.Initialize();
            Button.onClick.AddListener(OnClickButton);
        }

    }

}

