using UnityEngine;
using UnityEditor;

namespace UIManager
{

    [CustomEditor(typeof(UIManager))]
    public class UIManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UIManager myScript = (UIManager)target;
            if (GUILayout.Button("Find UI Elements"))
            {
                myScript.FindAndStoreUIElements();
            }
        }
    }


}

