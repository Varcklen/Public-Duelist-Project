using Project.BattlefieldNS;
using System.Collections.Generic;
using System.Linq;

namespace Project.Abilities
{
    public abstract class TargetUnitCheckingSelect : TargetCheckingSelect
    {
        protected override sealed void ExtraConditions(ref List<Area> list)
        {
            list = list.Where(x => x.PlacedUnit != null).ToList();
            if (!_targetType.CanChooseSelf && list.Contains(Caster.Area))
            {
                list.Remove(Caster.Area);
            }
        }
    }
}
