using Project.BattlefieldNS;
using Project.UnitNS;
using System.Collections.Generic;
using System.Linq;

namespace Project.Utils.Extension.UnitAreaNS
{
    public static class UnitAreaExtension
    {
        public static List<Unit> GetPlacedUnits(this List<Area> unitAreas)
        {
            return unitAreas.Where(x => x.PlacedUnit != null).Select(x => x.PlacedUnit).ToList();
        }

        public static Area GetRandomUnitArea(this List<Area> unitAreas)
        {
            var rand = new System.Random();
            int number = rand.Next(unitAreas.Count);
            return unitAreas[number];
        }
    }
}
