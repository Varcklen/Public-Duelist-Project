using System;

namespace Project.Utils.BaseInfoNS
{
    /// <summary>
    /// Is the base class for collecting information to pass from a ScriptableObject to a component.
    /// </summary>
    public abstract class BaseInfo : ICloneable
    {
        /// <summary>
        /// If the child class contains class fields, then they must be copied. All copies are placed in this method and called.
        /// </summary>
        protected virtual void ReplaceClasses() {}

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }

    }

    public static class BaseInfoExtension
    {
        /// <summary>
        /// Creates a copy of the information so as not to use its ScriptableObject version.
        /// </summary>
        public static T GetClone<T>(this BaseInfo oldObject) where T : BaseInfo
        {
            return (T)((ICloneable)oldObject).Clone();
        }
    }

    /// <summary>
    /// Allows you to initialize information in child BaseInfo classes.
    /// </summary>
    internal interface IBaseInfoInitialize
    {
        internal void Initialize();
    }
}
