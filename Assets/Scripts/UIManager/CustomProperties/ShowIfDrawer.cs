using UnityEngine;
using UnityEditor;
using System;

namespace UIManager
{


#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIfAttribute = attribute as ShowIfAttribute;
            SerializedProperty boolProperty = FindSerializedProperty(property, showIfAttribute.BooleanFieldName);

            if (boolProperty != null && boolProperty.propertyType == SerializedPropertyType.Boolean)
            {
                if (boolProperty.boolValue)
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIfAttribute = attribute as ShowIfAttribute;
            SerializedProperty boolProperty = FindSerializedProperty(property, showIfAttribute.BooleanFieldName);

            if (boolProperty != null && boolProperty.propertyType == SerializedPropertyType.Boolean)
            {
                return boolProperty.boolValue ? EditorGUI.GetPropertyHeight(property, label, true) : -EditorGUIUtility.standardVerticalSpacing;
            }

            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        private SerializedProperty FindSerializedProperty(SerializedProperty property, string propertyName)
        {
            string propertyPath = property.propertyPath;
            int lastIndex = propertyPath.LastIndexOf('.');

            if (lastIndex >= 0)
            {
                string newPath = propertyPath.Substring(0, lastIndex) + "." + propertyName;
                SerializedProperty boolProperty = property.serializedObject.FindProperty(newPath);

                if (boolProperty != null)
                {
                    return boolProperty;
                }
            }

            return property.serializedObject.FindProperty(propertyName);
        }
    }
#endif



    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public string BooleanFieldName;

        public ShowIfAttribute(string booleanFieldName)
        {
            BooleanFieldName = booleanFieldName;
        }
    }

}

