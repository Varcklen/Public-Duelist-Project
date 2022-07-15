using System;
using UnityEngine;
using Project.Utils.Events;
using Project.Utils.Events.Interfaces;
using Project.BattlefieldNS;
using Project.UnitNS.Interfaces;

namespace Project.UnitNS
{
    /// <summary>
    /// Stores information about a unit's turn.
    /// </summary>
    public sealed class UnitTurn : MonoBehaviour, IActionPoints, IUnitSelection
    {
        public event Action OnTurnStart;
        public event Action OnTurnEnd;

        public ActionEvent OnUnitNextAction = new ActionEvent();
        private IEventInvoke OnUnitNextActionInterface => OnUnitNextAction;
        public ActionEvent OnAfterUnitNextAction = new ActionEvent();
        private IEventInvoke OnAfterUnitNextActionInterface => OnAfterUnitNextAction;

        /// <summary>
        /// True if selected for currently active.
        /// </summary>
        public bool IsSelected { get; private set; } = false;

        private Unit _unit;

        /// <summary>
        /// Returns the maximum number of actions a unit can take before it ends its turn.
        /// </summary>
        public int MaxActionPoints { get; private set; } = 1;
        private int _actionPoints = 1;

        /// <summary>
        /// Returns the number of actions a unit can take before it ends its turn.
        /// </summary>
        public bool HasActionPoints => _actionPoints > 0;

        private BattlefieldStages _battlefieldStages;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
            _battlefieldStages = BattlefieldStages.Instance;
        }

        private void OnEnable()
        {
            _battlefieldStages.OnRoundChanged.AddListener(RefreshCurrentActionPoint);
        }

        private void OnDisable()
        {
            _battlefieldStages.OnRoundChanged.RemoveListener(RefreshCurrentActionPoint);
        }

        private void RefreshCurrentActionPoint(int round)
        {
            _actionPoints = MaxActionPoints;
        }

        public void RemoveCurrentActionPoint()
        {
            _actionPoints--;
            if (_actionPoints <= 0)
            {
                if (_actionPoints < 0)
                {
                    throw new Exception($"_currentActionPoints must be 0 or higher. Currect: {_actionPoints}.");
                }
                Battlefield.Instance.BattleStages.NextTurn();
            }
            else
            {
                OnUnitNextActionInterface.Invoke();
                ((IEventInvoke)BattlefieldStages.Instance.OnActionEnded).Invoke();
                OnAfterUnitNextActionInterface.Invoke();
            }
        }

        /// <summary>
        /// The unit ends its turn. Its action points are reset to zero and it passes the turn to the next unit in the queue.
        /// </summary>
        public void EndTurn()
        {
            if (IsSelected == false)
            {
                Debug.LogWarning("You can't end turn for unselected unit.");
                return;
            }
            _actionPoints = 0;
            Battlefield.Instance.BattleStages.NextTurn();
        }

        void IActionPoints.SetActionPoints(int newPoints)
        {
            MaxActionPoints = Math.Clamp(newPoints, 1, int.MaxValue);
            _actionPoints = Math.Clamp(Math.Max(_actionPoints, newPoints), 0, MaxActionPoints);
        }

        void IActionPoints.AddActionPoints(int points)
        {
            ((IActionPoints)this).SetActionPoints(MaxActionPoints + points);
        }

        void IUnitSelection.SelectUnit()
        {
            if (IsSelected)
            {
                Debug.LogWarning("You can't select already selected unit.");
                return;
            }
            IsSelected = true;
            OnTurnStart?.Invoke();
            ((IEventInvoke<Unit>)Events.OnUnitTurnStart)?.Invoke(_unit);
        }

        void IUnitSelection.DeselectUnit()
        {
            if (IsSelected == false)
            {
                Debug.LogWarning("You can't deselect not selected unit.");
                return;
            }
            IsSelected = false;
            OnTurnEnd?.Invoke();
            ((IEventInvoke<Unit>)Events.OnUnitTurnEnd)?.Invoke(_unit);
        }

    }
}
