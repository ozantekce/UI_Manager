using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(AliasSystem))]
public class AliasSystemEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AliasSystem aliasSystem = (AliasSystem)target;
        if (GUILayout.Button("Find All Alias Entities"))
        {
            aliasSystem.FindAllEntities();
        }
    }
}
