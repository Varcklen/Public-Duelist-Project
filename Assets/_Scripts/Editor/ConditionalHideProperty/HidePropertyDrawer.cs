using UnityEngine;
using UnityEditor;
using Project.Attributes;

namespace Project.Editor.Drawer.HideProperty
{
    public abstract class HidePropertyDrawer : PropertyDrawer
    {
        protected abstract bool GetConditionalHideAttributeResult(ParentHideAttribute condHAtt, SerializedProperty property);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ParentHideAttribute condHAtt = (ParentHideAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property);
            if (condHAtt.Inverse)
            {
                enabled = !enabled;
            }

            bool wasEnabled = GUI.enabled;
            GUI.enabled = enabled;
            if (!condHAtt.HideInInspector || enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            GUI.enabled = wasEnabled;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ParentHideAttribute condHAtt = (ParentHideAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property);
            if (condHAtt.Inverse)
            {
                enabled = !enabled;
            }
            if (!condHAtt.HideInInspector || enabled)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
}