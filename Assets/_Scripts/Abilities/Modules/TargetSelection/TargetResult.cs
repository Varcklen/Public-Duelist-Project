using Project.UnitNS;
using System.Collections.Generic;

namespace Project.Abilities
{
    /// <summary>
    /// Anyone possessing this interface can be obtained as a result of target selection.
    /// </summary>
    public interface ITargetResult { }

    /// <summary>
    /// A list of units that can be returned as the result of a multi-target search for an ability.
    /// </summary>
    public class TargetList : List<Unit>, ITargetResult { } 
}
