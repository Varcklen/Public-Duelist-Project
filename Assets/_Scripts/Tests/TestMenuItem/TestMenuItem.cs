using UnityEngine;
using System.ComponentModel;

namespace Project.Tests.MenuItemNS
{
    /// <summary>
    /// Parent class for all MenuItem tests.
    /// </summary>
    internal abstract class TestMenuItem
    {
        /// <summary>
        /// Throws a warning exception if the test is applied outside of gameplay.
        /// </summary>
        protected static void PlayingCheck()
        {
            if (!Application.isPlaying)
            {
                throw new WarningException("You must use it at runtime.");
            }
        }
    }

}
