using Project.Stats;

namespace Project.BattlefieldNS.Interfaces
{
    /// <summary>
    /// In Side.cs it sets the unitArea field.
    /// </summary>
    internal interface IAddUnitArea
    {
        internal void AddUnitArea(Area unitArea);
    }

    /// <summary>
    /// Sets the stat type in UnitArea component childs.
    /// </summary>
    public interface ISetResourceType
    {
        public abstract void SetStatType(StatType statType);
    }
}
