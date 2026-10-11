using UnityEditor;
using UnityEngine;
using Utility.Attribute;

namespace Editor
{
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    public class HideIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (IsVisible(property))
                return;

            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (IsVisible(property))
                return 0f;

            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        private bool IsVisible(SerializedProperty property)
        {
            HideIfAttribute hideif = (HideIfAttribute)attribute;

            SerializedProperty condition = property.serializedObject.FindProperty(hideif.conditionField);

            condition ??= property.serializedObject.FindProperty($"<{hideif.conditionField}>k__BackingField");

            return condition != null && condition.boolValue;
        }

    }
}
