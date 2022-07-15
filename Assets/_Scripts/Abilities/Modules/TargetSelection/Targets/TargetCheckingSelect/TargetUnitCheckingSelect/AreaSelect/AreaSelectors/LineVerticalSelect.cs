using Project.BattlefieldNS;
using System.Linq;

namespace Project.Abilities
{
    public sealed class LineVerticalSelect : AreaSelect
    {
        protected override void OnCursorEnable(Area unitArea)
        {
            var list = unitArea.Side.UnitAreas.Where(x => x.VectorPosition.x == unitArea.VectorPosition.x && x.PlacedUnit != null).ToList();
            TryAddContains(list);
        }
    }
}
