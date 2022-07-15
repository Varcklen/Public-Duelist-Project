using UnityEngine;
using Project.UnitNS;
using System;
using Project.BattlefieldNS.Interfaces;
using Project.Utils.Extension.ObjectNS;
using Project.Stats;
using System.Collections.Generic;
using Project.Abilities;
using Project.Utils.Events;
using Project.Utils.Events.Interfaces;

namespace Project.BattlefieldNS
{
    /// <summary>
    /// The component is responsible for the area in which the unit is located.
    /// </summary>
    public class Area : MonoBehaviour, IUnitAreaSetPosition, ITargetResult
    {
        /// <summary>
        /// The unit that is planted in the current area.
        /// </summary>
        public Unit PlacedUnit { get; private set; }

        /// <summary>
        /// Returns the owner of the area.
        /// </summary>
        public Side Side { get; private set; }
        /// <summary>
        /// Returns the numeric position of the area in the side.
        /// </summary>
        public int Position { get; private set; }
        /// <summary>
        /// Returns the vector position of the area in the side.
        /// </summary>
        public Vector2 VectorPosition { get; private set; }

        public Action OnPlacedUnitChanged;

        /// <summary>
        /// This class is responsible for interacting with the Area Arrow Hint.
        /// </summary>
        public AreaArrowHint AreaArrowHint { get; private set; }
        /// <summary>
        /// This class is responsible for interacting with the Target Circle.
        /// </summary>
        public TargetCircle TargetCircle { get; private set; }

        private ResourceBar _healthBar;
        private ResourceBar _manaBar;

        private StatDisplay _attackDisplay;
        private StatDisplay _speedDisplay;

        private List<AreaUI> _childsUI { get; } = new List<AreaUI>();

        private void Awake()
        {
            Side = GetComponentInParent<Side>().IsNullException();
            ((IAddUnitArea)Side).AddUnitArea(this);

            AreaArrowHint = GetComponent<AreaArrowHint>();
            TargetCircle = transform.GetComponentInChildren<TargetCircle>(includeInactive: true).IsNullException();

            AddChildToList("HealthBar", StatType.Health);
            AddChildToList("ManaBar", StatType.Mana);
            AddChildToList("AttackDisplay", StatType.Attack);
            AddChildToList("SpeedDisplay", StatType.Speed);

            void AddChildToList(string name, StatType statType)
            {
                AreaUI child = transform.Find(name).GetComponent<AreaUI>().IsNullException();
                ((ISetResourceType)child).SetStatType(statType);
                _childsUI.Add(child);
            }
        }

        #region Subscribe
        private void Subscribe()
        {
            UnitMovement unitMovement = PlacedUnit?.UnitMovement;
            if (unitMovement != null)
            {
                unitMovement.OnUnitMove += ChangeArea;
                unitMovement.OnUnitSwap += Swap;
            }
            foreach (var child in _childsUI)
            {
                child.Show();
            }
            PlacedUnit?.UnitDeath.OnKill.AddListener(ClearUnit);
        }

        private void Unsubscribe()
        {
            UnitMovement unitMovement = PlacedUnit?.UnitMovement;
            if (unitMovement != null)
            {
                unitMovement.OnUnitMove -= ChangeArea;
                unitMovement.OnUnitSwap -= Swap;
            }
            foreach (var child in _childsUI)
            {
                child.Hide();
            }
            PlacedUnit?.UnitDeath.OnKill.RemoveListener(ClearUnit);
        }
        #endregion

        void IUnitAreaSetPosition.SetPosition(int position)
        {
            if (position < 0 || position > 6)
            {
                throw new ArgumentOutOfRangeException($"position must be on range: [0,6]. Current: {position}.");
            }
            Position = position;

            int x = (position + 1) % 2;
            int y = (int)Mathf.Floor((float)(position - 1) / 2);
            VectorPosition = new Vector2(x, y);
        }

        #region TargetCircleSelection
        /// <summary>
        /// True if it can be a potential target for the targeted ability.
        /// </summary>
        public bool IsTargeted { get; private set; } = false;

        public readonly ActionEvent OnUnitTargeted = new ActionEvent();
        private IEventInvoke OnUnitTargetedInterface => OnUnitTargeted;
        public readonly ActionEvent OnUnitDetargeted = new ActionEvent();
        private IEventInvoke OnUnitDetargetedInterface => OnUnitDetargeted;
        public void TargetArea()
        {
            if (IsTargeted)
            {
                Debug.LogWarning("You can't target already targeted unit.");
                return;
            }
            IsTargeted = true;
            OnUnitTargetedInterface.Invoke();
        }

        public void DetargetArea()
        {
            if (IsTargeted == false)
            {
                Debug.LogWarning("You can't detarget not targeted unit.");
                return;
            }
            IsTargeted = false;
            OnUnitDetargetedInterface.Invoke();
        }
        #endregion

        /// <summary>
        /// Here we disable all links between the old area and the unit. Here the old area and the unit interact.
        /// </summary>
        private void ClearUnit()
        {
            if (PlacedUnit == null)
            {
                return;
            }
            Unsubscribe();
            //Clear Area in Unit
            ((IOnAreaUpdate)PlacedUnit).OnAreaUpdate(null);
            //Clear Unit in Area
            PlacedUnit = null;
        }

        private void ChangeArea(Area unitArea)
        {
            if (unitArea == null)
            {
                throw new ArgumentNullException("Current unitArea is null.");
            }
            unitArea.SetPlacedUnit(PlacedUnit);
        }

        /// <summary>
        /// Sets the unit area to a new placed unit.
        /// </summary>
        public void SetPlacedUnit(Unit newUnit)
        {
            if (newUnit == null)
            {
                Debug.LogWarning($"You cant use {nameof(SetPlacedUnit)} method for new units as nullable. Use {nameof(ClearUnit)} instead.");
                return;
            }
            if (PlacedUnit == newUnit)
            {
                Debug.LogWarning("This minion already on this area.");
                return;
            }
            else if (PlacedUnit != null)
            {
                Debug.LogWarning("You cannot move minion in busy area.");
                return;
            }
            //Unbind old area
            newUnit?.Area?.ClearUnit();//isWarningNeed:false
            PlacedUnit = newUnit;
            //Bind new area
            SetNewArea();
        }

        /// <summary>
        /// Here we connect all the links between the new area and the unit. This is where the new area and unit interact.
        /// </summary>
        private void SetNewArea()
        {
            if (PlacedUnit == null)
            {
                return;
            }

            Transform unitTransform = PlacedUnit.transform;
            if (unitTransform.IsChildOf(transform) == false)
            {
                unitTransform.SetParent(transform, false);
            }
            unitTransform.rotation = new Quaternion(0, 180 * (int)Side.SideType, 0, 0);
            ((IOnAreaUpdate)PlacedUnit).OnAreaUpdate(this);
            PlacedUnit.transform.localPosition = Vector2.down;
            PlacedUnit.gameObject.SetActive(true);
            Subscribe();
            OnPlacedUnitChanged?.Invoke();
        }

        private void Swap(Unit unit)
        {
            if (unit == null || PlacedUnit == null)
            {
                Debug.LogWarning($"Units cannot be swapped if one of them is null. Is null: unit: {unit == null}, _placedUnit: {PlacedUnit == null}.");
                return;
            }
            if (unit == PlacedUnit)
            {
                Debug.LogWarning("You are trying to swap the same unit.");
                return;
            }
            Area unitArea = unit.Area;
            unitArea.ClearUnit();
            unitArea.SetPlacedUnit(PlacedUnit);
            SetPlacedUnit(unit);
        }

        #region GetMethods
        public Area GetMirrorUnitArea()
        {
            return Battlefield.Instance.GetOppositeSide(Side.SideType).GetUnitArea(VectorPosition);
        }
        #endregion
    }
}
