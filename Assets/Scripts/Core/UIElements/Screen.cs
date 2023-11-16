using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace UI_Manager
{
    public class Screen : UI_Element
    {


        public override void ConfigurationsAwake()
        {
            base.ConfigurationsAwake();
        }

        public override void ConfigurationsStart()
        {
            base.ConfigurationsStart();

        }



        public override UIElementType Type => UIElementType.Screen;


    }


}
