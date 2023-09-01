using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_Animation : ScriptableObject
{
    public abstract IEnumerator Enumerator(UI_Element element);

}