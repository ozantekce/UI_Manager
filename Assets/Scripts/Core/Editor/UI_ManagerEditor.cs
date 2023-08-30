using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(UI_Manager))]
public class UI_ManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UI_Manager ui_manager = (UI_Manager)target;
        if (GUILayout.Button("Find All UI Elements"))
        {
            ui_manager.FindAllUIElements();
        }
    }
}
