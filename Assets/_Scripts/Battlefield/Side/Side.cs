using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Project.UnitNS;
using Project.BattlefieldNS.Interfaces;
using Project.Utils.Events;
using Project.Utils.Events.Interfaces;

namespace Project.BattlefieldNS
{
    /// <summary>
    /// Here is information about the opposing side of the battle.
    /// </summary>
    public class Side : MonoBehaviour, IAddUnitArea
    {
        [SerializeField] private SideType _sideType = SideType.Ally;
        /// <summary>
        /// Returns the type of the opposing side.
        /// </summary>
        public SideType SideType => _sideType;

        /// <summary>
        /// Returns all areas controlled by this side.
        /// </summary>
        public List<Area> UnitAreas { get; private set; } = new List<Area>();

        private const int UNIT_AREA_LIMIT = 6;

        private Battlefield _battlefield;
        private Graveyard _graveyard;

        private void Awake()
        {
            _battlefield = transform.GetComponentInParent<Battlefield>();
            _graveyard = Graveyard.Instance;
        }

        private void OnEnable()
        {
            Events.OnAfterUnitKill.AddListener(OnUnitKill);
        }

        private void OnDisable()
        {
            Events.OnAfterUnitKill.RemoveListener(OnUnitKill);
        }

        /// <summary>
        /// After adding a unit to the graveyard, a check is created. If there are no units on the side, then we call the "Side is empty" event.
        /// </summary>
        private void OnUnitKill(Unit unit)
        {
            if (_graveyard.GetSideType(unit) != _sideType) return;
            if (IsEmpty())
            {
                var winnerSide = GetOppositeSide().SideType;
                ((IEventInvoke<SideType>)Events.OnSideEmpty).Invoke(winnerSide);
            }
        }

        void IAddUnitArea.AddUnitArea(Area unitArea)
        {
            UnitAreas.Add(unitArea);
            if (UnitAreas.Count == UNIT_AREA_LIMIT)
            {
                UnitAreas = UnitAreas.OrderBy(x => x.name).ToList();
                for (int i = 0; i < UnitAreas.Count; i++)
                {
                    ((IUnitAreaSetPosition)UnitAreas[i]).SetPosition(i + 1);
                }
            }
        }

        #region GetMethods
        public int GetUnitCount()
        {
            return UnitAreas.Where(x => x.PlacedUnit != null).Count();
        }

        public bool IsFull()
        {
            int count = GetUnitCount();
            if (count > UNIT_AREA_LIMIT)
            {
                Debug.LogWarning($"For some reason, unitCount in {SideType} more than area limit.");
            }
            return count >= UNIT_AREA_LIMIT;
        }

        public bool HasEmptySlot()
        {
            return GetUnitCount() < UNIT_AREA_LIMIT;
        }

        public bool IsEmpty()
        {
            int count = GetUnitCount();
            if (count < 0)
            {
                Debug.LogWarning($"For some reason, unitCount in {SideType} less than zero.");
            }
            return count <= 0;
        }

        public Area GetFirstEmptyArea()
        {
            return UnitAreas.FirstOrDefault(x => x.PlacedUnit == null);
        }

        public Unit GetRandomUnit()
        {
            if (IsEmpty())
            {
                Debug.LogWarning($"{SideType} is empty. You can't get random unit.");
                return null;
            }
            var list = UnitAreas.Where(x => x.PlacedUnit != null).ToList();
            var random = new System.Random();
            return list[random.Next(list.Count)].PlacedUnit;
        }

        public Unit GetFirstUnit()
        {
            if (IsEmpty())
            {
                Debug.LogWarning($"{SideType} is empty. You can't get first unit.");
                return null;
            }
            int min = UnitAreas.Where(x => x.PlacedUnit != null).Min(x => x.Position);
            return UnitAreas.FirstOrDefault(a => a.Position == min).PlacedUnit;
        }

        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public Area GetUnitArea(int position)
        {
            if (position < 1 || position > UNIT_AREA_LIMIT)
            {
                throw new System.ArgumentOutOfRangeException($"position variable out of range [1,{UNIT_AREA_LIMIT}]. Current: {position}.");
            }
            return UnitAreas.FirstOrDefault(x => x.Position == position);
        }

        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public Area GetUnitArea(Vector2 position, bool isExceptionEnabled = true)
        {
            if (position.x < 0 || position.x > 1 || position.y < 0 || position.y > 2)
            {
                if (isExceptionEnabled)
                {
                    throw new System.ArgumentOutOfRangeException($"position variable out of range [x: [0,1], y: [0,2]]. Current: {position.x}|{position.y}.");
                }
                return null;
            }
            return UnitAreas.FirstOrDefault(x => x.VectorPosition == position);
        }

        public List<Unit> GetAllUnits()
        {
            return UnitAreas.Where(x => x.PlacedUnit != null).Select(x => x.PlacedUnit).ToList();
        }

        public Side GetOppositeSide()
        {
            return _battlefield.GetOppositeSide(SideType);
        }
        #endregion
    }

    public enum SideType : byte
    {
        Ally,
        Enemy
    }
}
