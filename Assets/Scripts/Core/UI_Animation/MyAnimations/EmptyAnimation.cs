using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace UI_Manager
{
    [CreateAssetMenu(fileName = "Empty", menuName = "UIManagerAnimation/Empty")]
    public class EmptyAnimation : UI_Animation
    {

        public override IEnumerator Enumerator(UI_Element element)
        {
            yield break;
        }
    }

}

