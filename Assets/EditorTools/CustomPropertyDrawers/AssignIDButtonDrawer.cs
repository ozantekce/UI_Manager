using UnityEngine;
using UnityEditor;
using System;

namespace EditorTools
{

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AssignIDButtonAttribute))]
    public class AssignIDButtonDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Calculate positions
            Rect labelPosition = position;
            labelPosition.width -= 80; // Adjust the 80 based on your desired button width
            Rect buttonPosition = position;
            buttonPosition.x += position.width - 80;
            buttonPosition.width = 80;

            // Draw the string as a label (making it read-only)
            EditorGUI.LabelField(labelPosition, label.text, property.stringValue);

            // Draw the button
            if (GUI.Button(buttonPosition, "Assign ID"))
            {
                if (property.propertyType == SerializedPropertyType.String)
                {
                    // Generate a unique ID (in this case a GUID)
                    property.stringValue = Guid.NewGuid().ToString();
                    property.serializedObject.ApplyModifiedProperties();
                }
                else
                {
                    Debug.LogError("AssignIDButton can only be used with string fields.");
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
#endif


    public class AssignIDButtonAttribute : PropertyAttribute { }



}

