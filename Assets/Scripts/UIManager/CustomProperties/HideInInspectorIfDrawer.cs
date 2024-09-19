using UnityEngine;
using UnityEditor;


namespace UIManager
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(HideInInspectorIf))]
    public class HideInInspectorIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            HideInInspectorIf hideInInspectorIf = attribute as HideInInspectorIf;

            string fullPath = string.IsNullOrEmpty(hideInInspectorIf.parentPath) ? hideInInspectorIf.condition : hideInInspectorIf.parentPath + "." + hideInInspectorIf.condition;

            SerializedProperty conditionProperty = property.serializedObject.FindProperty(fullPath);

            if (conditionProperty == null || conditionProperty.boolValue == false)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            HideInInspectorIf hideInInspectorIf = attribute as HideInInspectorIf;

            string fullPath = string.IsNullOrEmpty(hideInInspectorIf.parentPath) ? hideInInspectorIf.condition : hideInInspectorIf.parentPath + "." + hideInInspectorIf.condition;

            SerializedProperty conditionProperty = property.serializedObject.FindProperty(fullPath);

            if (conditionProperty == null || conditionProperty.boolValue == false)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
            else
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
#else
    // Runtime code here
#endif

    public class HideInInspectorIf : PropertyAttribute
    {
        public string condition;
        public string parentPath;

        public HideInInspectorIf(string condition, string parentPath = "")
        {
            this.condition = condition;
            this.parentPath = parentPath;
        }
    }



}

