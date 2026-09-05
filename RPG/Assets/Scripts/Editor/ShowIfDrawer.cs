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
            if (!IsVisible(property))
                return;

            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!IsVisible(property))
                return 0f;

            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        private bool IsVisible(SerializedProperty property)
        {
            ShowIfAttribute showIf = (ShowIfAttribute)attribute;

            SerializedProperty condition = property.serializedObject.FindProperty(showIf.conditionField);

            condition ??= property.serializedObject.FindProperty($"<{showIf.conditionField}>k__BackingField");

            return condition != null && condition.boolValue;
        }
    }
}
