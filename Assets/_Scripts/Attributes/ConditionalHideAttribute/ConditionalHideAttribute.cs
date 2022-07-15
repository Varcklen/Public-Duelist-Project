using UnityEngine;
using System;

namespace Project.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public abstract class ParentHideAttribute : PropertyAttribute
    {
        //The name of the bool field that will be in control
        public string ConditionalSourceField = "";
        //TRUE = Hide in inspector / FALSE = Disable in inspector 
        public bool HideInInspector = false;
        public bool Inverse = false;
    }

    public class BoolHideAttribute : ParentHideAttribute
    {
        /// <summary>
        /// When conditions are met, hides the variable in the inspector.
        /// </summary>
        /// <param name="conditionalSourceField">The name of the hiding variable.</param>
        /// <param name="hideInInspector">If true, then instead of being turned off, the variable disappears from the inspector.</param>
        /// <param name="inverse">Invert the condition of the hiding variable.</param>
        public BoolHideAttribute(string conditionalSourceField, bool hideInInspector = true, bool inverse = false)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = hideInInspector;
            Inverse = inverse;
        }
    }

    public class EnumHideAttribute : ParentHideAttribute
    {
        public int EnumIndex;

        /// <summary>
        /// When conditions are met, hides the variable in the inspector.
        /// </summary>
        /// <param name="conditionalSourceField">The name of the hiding variable.</param>
        /// <param name="enumIndex">Required enum value.</param>
        /// <param name="hideInInspector">If true, then instead of being turned off, the variable disappears from the inspector.</param>
        /// <param name="inverse">Invert the condition of the hiding variable.</param>
        public EnumHideAttribute(string conditionalSourceField, int enumIndex, bool hideInInspector = true, bool inverse = false)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = hideInInspector;
            EnumIndex = enumIndex;
            Inverse = inverse;
        }
    }

    public class IntHideAttribute : ParentHideAttribute
    {
        public int MinIndex;
        public int MaxIndex;

        /// <summary>
        /// When conditions are met, hides the variable in the inspector.
        /// </summary>
        /// <param name="conditionalSourceField">The name of the hiding variable.</param>
        /// <param name="hideInInspector">If true, then instead of being turned off, the variable disappears from the inspector.</param>
        /// <param name="inverse"Invert the condition of the hiding variable.></param>
        public IntHideAttribute(string conditionalSourceField, int minIndex, int maxIndex = int.MaxValue, bool hideInInspector = true, bool inverse = false)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = hideInInspector;
            MinIndex = minIndex;
            MaxIndex = maxIndex;
            Inverse = inverse;
        }
    }
}