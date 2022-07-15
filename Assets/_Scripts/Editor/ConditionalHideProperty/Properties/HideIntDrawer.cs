using UnityEditor;
using System;
using Project.Attributes;

namespace Project.Editor.Drawer.HideProperty
{
    [CustomPropertyDrawer(typeof(IntHideAttribute))]
    public class HideIntDrawer : HidePropertyDrawer
    {
        protected override bool GetConditionalHideAttributeResult(ParentHideAttribute condHAtt, SerializedProperty property)
        {
            string propertyPath = property.propertyPath;
            string conditionPath = propertyPath.Replace(property.name, ((IntHideAttribute)condHAtt).ConditionalSourceField);
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);
            if (sourcePropertyValue == null)
            {
                throw new ArgumentNullException("Current sourcePropertyValue is null.");
            }
            var attribute = (IntHideAttribute)condHAtt;
            bool resultMin = sourcePropertyValue.intValue > attribute.MinIndex;
            bool resultMax = true;
            if (attribute.MaxIndex != int.MaxValue)
            {
                resultMax = sourcePropertyValue.intValue < attribute.MaxIndex;
            }
            return resultMin && resultMax;
        }
    }
}