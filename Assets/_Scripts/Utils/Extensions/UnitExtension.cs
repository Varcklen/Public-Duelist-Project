using Project.UnitNS;
using System;
using System.Collections.Generic;

namespace Project.Utils.Extension.UnitNS
{
    internal static class UnitExtension
    {
        /// <summary>
        /// Returns a list with units from the list of unit areas.
        /// </summary>
        /// <returns></returns>
        public static Unit GetRandomUnit(this List<Unit> units)
        {
            var rand = new Random();
            int number = rand.Next(units.Count);
            return units[number];
        }
    }
}