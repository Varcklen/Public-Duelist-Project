namespace Project.BattlefieldNS.Interfaces
{
    /// <summary>
    /// In Unit.cs, sets the unitArea field.
    /// </summary>
    internal interface IOnAreaUpdate
    {
        internal void OnAreaUpdate(Area unitArea);
    }

    /// <summary>
    /// With the help of Side.cs sets the positions for the UnitArea.
    /// </summary>
    public interface IUnitAreaSetPosition
    {
        void SetPosition(int position);
    }
}