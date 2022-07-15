using System;

namespace Project.Utils.Exceptions
{
    /// <summary>
    /// Called when the checked element is not in the list.
    /// </summary>
    public class ElementNotContainedException : Exception
    {
        public ElementNotContainedException(string message) : base(message) { }
    }
}
