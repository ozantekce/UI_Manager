using UnityEngine;
using UnityEditor;
using System;

namespace UIManager
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AssignIDButtonAttribute))]
    public class AssignIDButtonDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Define button widths and spacing
            const float buttonWidth = 80;
            const float spacing = 10;
            const float totalButtonWidth = 2 * buttonWidth + spacing;

            // Calculate positions
            Rect labelPosition = position;
            labelPosition.width -= totalButtonWidth;  // Subtract total width of buttons and spacing

            Rect buttonPositionID = position;
            buttonPositionID.x += position.width - totalButtonWidth;
            buttonPositionID.width = buttonWidth;

            Rect buttonPositionName = position;
            buttonPositionName.x += position.width - buttonWidth;
            buttonPositionName.width = buttonWidth;

            // Draw the string as a label (making it read-only)
            EditorGUI.LabelField(labelPosition, label.text, property.stringValue);

            // Draw the Assign ID button
            if (GUI.Button(buttonPositionID, "Assign ID"))
            {
                if (property.propertyType == SerializedPropertyType.String)
                {
                    if (EditorUtility.DisplayDialog("Confirm ID Assignment",
                                                    "Are you sure you want to assign a new unique ID to each selected object? This action cannot be undone.",
                                                    "Yes", "No"))
                    {
                        AssignUniqueIDToEachSelectedObject(property);
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "AssignIDButton can only be used with string fields.", "OK");
                }
            }

            // Draw the Assign Name as ID button
            if (GUI.Button(buttonPositionName, "NameAsID"))
            {
                if (property.propertyType == SerializedPropertyType.String)
                {
                    if (EditorUtility.DisplayDialog("Confirm Name Assignment",
                                                    "Are you sure you want to assign the GameObject's name as ID? This action cannot be undone.",
                                                    "Yes", "No"))
                    {
                        AssignGameObjectNameToEachSelectedObject(property);
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "AssignIDButton can only be used with string fields.", "OK");
                }
            }
        }

        private void AssignUniqueIDToEachSelectedObject(SerializedProperty property)
        {
            foreach (UnityEngine.Object targetObject in property.serializedObject.targetObjects)
            {
                SerializedObject serializedObject = new SerializedObject(targetObject);
                SerializedProperty targetProperty = serializedObject.FindProperty(property.propertyPath);
                targetProperty.stringValue = Guid.NewGuid().ToString();  // Generate a unique GUID for each object
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void AssignGameObjectNameToEachSelectedObject(SerializedProperty property)
        {
            foreach (UnityEngine.Object targetObject in property.serializedObject.targetObjects)
            {
                SerializedObject serializedObject = new SerializedObject(targetObject);
                SerializedProperty targetProperty = serializedObject.FindProperty(property.propertyPath);
                targetProperty.stringValue = targetObject.name;  // Assign the GameObject's name to each object
                serializedObject.ApplyModifiedProperties();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
#else
    // Runtime code here
#endif

    public class AssignIDButtonAttribute : PropertyAttribute { }

}

