using System;
using UnityEngine;

namespace Project.Utils.Extension.ObjectNS
{
    public static class ObjectExtension
    {
        /// <summary>
        /// Checks if an object is null and throws an exception if it is.
        /// </summary>
        /// <param name="exceptionText">Throws the given text in an Exception.</param>
        /// <returns></returns>
        public static T IsNullException<T>(this T objectToCheck, string exceptionText = null) where T : class
        {
            if (objectToCheck == null)
            {
                throw new ArgumentNullException($"{typeof(T).FullName} is null, but shouldn't. {exceptionText}");
            }
            return objectToCheck;
        }

        /// <summary>
        /// Checks if an struct is null and throws an exception if it is.
        /// </summary>
        public static T IsNullExceptionSctruct<T>(this T objectToCheck, string exceptionText = null) where T : struct
        {
            if (objectToCheck.Equals(default(T)))
            {
                throw new ArgumentNullException($"{typeof(T).FullName} is null, but shouldn't. {exceptionText}");
            }
            return objectToCheck;
        }

    }
}
