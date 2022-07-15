using Project.BattlefieldNS;
using System.Linq;

namespace Project.Abilities
{
    public sealed class LineHorizontalSelect : AreaSelect
    {
        protected override void OnCursorEnable(Area unitArea)
        {
            var list = Battlefield.Instance.GetAllUnitAreas().Where(x => x.VectorPosition.y == unitArea.VectorPosition.y && x.PlacedUnit != null).ToList();
            TryAddContains(list);
        }
    }
}
