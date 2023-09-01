using UnityEngine;
using UnityEditor;
using System;
using System.Linq;



#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ShowInEnumAttribute))]
public class ShowInEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowInEnumAttribute showInAttribute = attribute as ShowInEnumAttribute;
        SerializedProperty enumProperty = FindSerializedProperty(property, showInAttribute.EnumFieldName);

        if (enumProperty != null && enumProperty.propertyType == SerializedPropertyType.Enum)
        {
            string selectedEnumName = enumProperty.enumNames[enumProperty.enumValueIndex];
            if (showInAttribute.EnumNames.Contains(selectedEnumName))
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowInEnumAttribute showInAttribute = attribute as ShowInEnumAttribute;
        SerializedProperty enumProperty = FindSerializedProperty(property, showInAttribute.EnumFieldName);

        if (enumProperty != null && enumProperty.propertyType == SerializedPropertyType.Enum)
        {
            string selectedEnumName = enumProperty.enumNames[enumProperty.enumValueIndex];
            if (showInAttribute.EnumNames.Contains(selectedEnumName))
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                return 0;
            }
        }

        return EditorGUI.GetPropertyHeight(property, label);
    }

    private SerializedProperty FindSerializedProperty(SerializedProperty property, string propertyName)
    {
        string propertyPath = property.propertyPath;
        int startIndex = propertyPath.LastIndexOf('.');

        if (startIndex >= 0)
        {
            string newPath = propertyPath.Substring(0, startIndex) + "." + propertyName;
            SerializedProperty enumProperty = property.serializedObject.FindProperty(newPath);

            if (enumProperty != null)
            {
                return enumProperty;
            }
        }

        SerializedProperty siblingProperty = property.serializedObject.FindProperty(propertyName);
        if (siblingProperty != null)
        {
            return siblingProperty;
        }

        return null;
    }

}
#else
    // Runtime code here
#endif





[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ShowInEnumAttribute : PropertyAttribute
{
    public string[] EnumNames;
    public string EnumFieldName;

    public ShowInEnumAttribute(string enumFieldName, params string[] enumNames)
    {
        EnumFieldName = enumFieldName;
        EnumNames = enumNames;
    }
}