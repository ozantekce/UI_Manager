using System.Collections;
using UnityEngine;


namespace UI_Manager
{
    public abstract class UI_Animation : ScriptableObject
    {
        public abstract IEnumerator Enumerator(UI_Element element);

    }

}
