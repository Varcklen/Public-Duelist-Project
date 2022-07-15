using Project.UnitNS.DataNS;

namespace Project.UnitNS.Interfaces
{
    /// <summary>
    /// Called on StartBattle to initialize the created unit as if it were instantiated.
    /// </summary>
    internal interface IOnUnitCreateForLimbo
    {
        internal void OnUnitCreateForLimbo();
    }

    /// <summary>
    /// Calls the Awake method after initializing data for components at the Unit.cs level.
    /// </summary>
    public interface IOnAwake
    {
        internal void OnAwake();
    }

    /// <summary>
    /// Gives access to Data. 
    /// </summary>
    public interface IUnitData
    {
        /// <summary>
        /// The field is a link to the basic information about the unit from the Scriptable Object. 
        /// The data in Data is not allowed to be changed, as this will lead to changes in the Scriptable Object.
        /// </summary>
        internal UnitData Data { get; }
    }

    /// <summary>
    /// Causes the unit's stats to be initialized.
    /// </summary>
    public interface IUnitStatsInit
    {
        void Init(UnitData unitData);
    }

    /// <summary>
    /// Allows you to start a unit's turn, as well as end it. Used in BattleStages to start and end a turn.
    /// </summary>
    public interface IUnitSelection
    {
        internal void SelectUnit();
        internal void DeselectUnit();
    }

    /// <summary>
    /// Allows you to change the number of current action points a unit has.
    /// </summary>
    public interface IActionPoints
    {
        internal void SetActionPoints(int newPoints);
        internal void AddActionPoints(int points);
    }
}
