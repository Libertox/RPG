using UnityEditor;
using UnityEngine;
using Utility.Attribute;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIf = (ShowIfAttribute)attribute;
            SerializedProperty condition = property.serializedObject.FindProperty(showIf.conditionField);

            if (condition != null && condition.boolValue)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIf = (ShowIfAttribute)attribute;
            SerializedProperty condition = property.serializedObject.FindProperty(showIf.conditionField);

            if (condition != null && condition.boolValue)
                return EditorGUI.GetPropertyHeight(property, label, true);
            else
                return 0;
        }
    }
}
