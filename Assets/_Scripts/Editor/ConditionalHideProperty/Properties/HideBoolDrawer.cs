using UnityEditor;
using System;
using Project.Attributes;

namespace Project.Editor.Drawer.HideProperty
{
    [CustomPropertyDrawer(typeof(BoolHideAttribute))]
    public class HideBoolDrawer : HidePropertyDrawer
    {
        protected override bool GetConditionalHideAttributeResult(ParentHideAttribute condHAtt, SerializedProperty property)
        {
            string propertyPath = property.propertyPath;
            string conditionPath = propertyPath.Replace(property.name, ((BoolHideAttribute)condHAtt).ConditionalSourceField);
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);
            if (sourcePropertyValue == null)
            {
                throw new ArgumentNullException("Current sourcePropertyValue is null.");
            }
            return sourcePropertyValue.boolValue;
        }
    }
}