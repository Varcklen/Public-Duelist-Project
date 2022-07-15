using Project.BattlefieldNS;
using System;
using UnityEngine;

namespace Project.UnitNS
{
    /// <summary>
    /// The class stores methods for moving a unit around the battlefield. These methods can only be used while the unit is on the battlefield. Cannot be used in the graveyard or during creating.
    /// </summary>
    public sealed class UnitMovement : MonoBehaviour
    {
        public event Action<Area> OnUnitMove;
        public event Action<Unit> OnUnitSwap;

        private Unit _unit;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        private void IsAreaNull()
        {
            if (_unit.Area == null)
            {
                throw new ArgumentNullException($"Area is null. You cant move your unit. Use {nameof(Area.SetPlacedUnit)} for this instead.");
            }
        }

        /// <summary>
        /// Moves the unit to the specified area. If there is already a unit in the area, raises an error.
        /// </summary>
        public void MoveTo(Area unitArea)
        {
            IsAreaNull();
            OnUnitMove?.Invoke(unitArea);
        }

        /// <summary>
        /// Swaps the target unit with the current one. If the target unit does not exist, raises an error.
        /// </summary>
        public void SwapWith(Unit unit)
        {
            IsAreaNull();
            OnUnitSwap?.Invoke(unit);
        }

        /// <summary>
        /// Attempts to move the unit to the specified area. If there is already a unit in the target area, they are swapped.
        /// </summary>
        public void MoveOrSwap(Area unitArea)
        {
            IsAreaNull();
            if (unitArea.PlacedUnit == null)
            {
                MoveTo(unitArea);
            }
            else
            {
                SwapWith(unitArea.PlacedUnit);
            }
        }
    }
}
