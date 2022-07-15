using Project.Singleton.MonoBehaviourSingleton;
using Project.Systems.DeathSystemNS;
using Project.UI.BattlefieldNS;
using Project.UnitNS;
using Project.UnitNS.Interfaces;
using Project.Utils.Events;
using Project.Utils.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.BattlefieldNS
{
    /// <summary>
    /// The class is responsible for the state of the current battle: who is currently acting, creates a queue of active units and oversees this.
    /// </summary>
    [RequireComponent(typeof(Battlefield))]
    public class BattlefieldStages : MonoBehaviourSingleton<BattlefieldStages>, IBattleStagesSkipUnit
    {
        /// <summary>
        /// Returns the current battle stage.
        /// </summary>
        public BattleStage BattleStage { get; private set; } = BattleStage.Preparation;

        public readonly ActionEvent<BattleStage> OnStageChange = new ActionEvent<BattleStage>();
        private IEventInvoke<BattleStage> OnStageChangeInterface => OnStageChange;
        public readonly ActionEvent<int> OnRoundChanged = new ActionEvent<int>();
        private IEventInvoke<int> OnRoundChangedInterface => OnRoundChanged;
        public readonly ActionEvent<List<Unit>> OnTurnChangedRoundUnits = new ActionEvent<List<Unit>>();
        private IEventInvoke<List<Unit>> OnTurnChangedRoundUnitsInterface => OnTurnChangedRoundUnits;

        public readonly ActionEvent OnActionEnded = new ActionEvent();
        private IEventInvoke OnActionEndedInterface => OnActionEnded;

        private Battlefield _battlefield;

        private List<Unit> _currentRoundUnits = new List<Unit>();
        private List<Unit> _allUnitsInQueue;
        /// <summary>
        /// Returns all units in their queue sequence.
        /// </summary>
        public List<Unit> AllUnitsInQueue => _allUnitsInQueue;

        private Unit _currentTurnUnit;
        private IUnitSelection _unitSelection;
        /// <summary>
        /// Returns the unit that should be doing the action at the moment.
        /// </summary>
        public Unit CurrentTurnUnit { 
            get { return _currentTurnUnit; } 
            private set
            {
                _currentTurnUnit = value;
                _unitSelection = _currentTurnUnit.UnitTurn;
            } 
        }

        /// <summary>
        /// Returns the value of the current round.
        /// </summary>
        public int Round { get; private set; } = 0;

        private new void Awake()
        {
            base.Awake();
            _battlefield = GetComponent<Battlefield>();
        }

        private void OnDestroy()
        {
            Unsbscribe();
        }

        #region Subscribe
        private bool isSubcribed;
        private void Subscribe()
        {
            if (isSubcribed) return;
            isSubcribed = true;
            Events.OnUnitCreate.AddListener(AddUnitInQueue);
            Events.OnUnitRessurect.AddListener(AddUnitInQueue);
            Events.OnUnitKill.AddListener(RemoveUnitFromQueue);
            Events.OnSideEmpty.AddListener(EndBattle);
            Events.OnSpeedStatChanged.AddListener(OnSpeedChanged);
        }

        private void Unsbscribe()
        {
            if (isSubcribed == false) return;
            isSubcribed = false;
            Events.OnUnitCreate.RemoveListener(AddUnitInQueue);
            Events.OnUnitRessurect.RemoveListener(AddUnitInQueue);
            Events.OnUnitKill.RemoveListener(RemoveUnitFromQueue);
            Events.OnSideEmpty.RemoveListener(EndBattle);
            Events.OnSpeedStatChanged.RemoveListener(OnSpeedChanged);
        }
        #endregion

        private void ChangeStage(BattleStage newStage)
        {
            if (BattleStage == newStage)
            {
                if (newStage != BattleStage.Waiting) Debug.LogWarning($"You trying to set stage that already setted. Current stage: {BattleStage}.");
                return;
            }
            BattleStage = newStage;
            OnStageChangeInterface.Invoke(newStage);
        }

        /// <summary>
        /// Starts the selection of targets for the battle.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void StartBattle()
        {
            if (BattleStage != BattleStage.Preparation)
            {
                throw new Exception("You can't start already started battle.");
            }
            ((IEventInvoke)Events.OnBattleStart).Invoke();
            NextTurn();
            Subscribe();
        }

        /// <summary>
        /// Trigger the end of battle.
        /// </summary>
        private void EndBattle(SideType winSide)
        {
            ChangeStage(BattleStage.End);
            ((IEventInvoke<SideType>)Events.OnBattleEnd).Invoke(winSide);
            Debug.Log($"Winner: {winSide}");
            ClearOldUnit();
        }

        /// <summary>
        /// Ends the turn of the current unit and starts the turn of the next unit in the list.
        /// </summary>
        public void NextTurn(bool isMoveToEnd = true)
        {
            ChangeStage(BattleStage.Waiting);
            if (isMoveToEnd) MoveToEndOfQueue(_currentTurnUnit);
            ClearOldUnit();
            _ = SetNewUnit();
            UpdateVisual();
        }

        /// <summary>
        /// Places the unit at the end of the queue.
        /// </summary>
        private void MoveToEndOfQueue(Unit unit)
        {
            if (unit == null) return;
            if (!_currentRoundUnits.Contains(unit)) return;

            _currentRoundUnits.Remove(unit);
            _allUnitsInQueue.Remove(unit);
            _allUnitsInQueue.Add(unit);
        }

        /// <summary>
        /// Allows you to skip a certain number of units (including the current one) and move forward a few units.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        void IBattleStagesSkipUnit.SkipUnit(int unitsToSkip)
        {
            if (unitsToSkip < 1)
            {
                throw new ArgumentOutOfRangeException($"You must skip positive amount of units. Currect: {unitsToSkip}.");
            }
            for (int i = 0; i < unitsToSkip; i++)
            {
                NextTurn();
            }
        }

        /// <summary>
        /// Clears information about the currently selected unit.
        /// </summary>
        private void ClearOldUnit()
        {
            if (_currentTurnUnit == null) return;
            _unitSelection.DeselectUnit();
            _currentTurnUnit = null;
        }

        /// <summary>
        /// Selects the next unit in the list and updates the list if it is empty.
        /// </summary>
        private async Task SetNewUnit()
        {
            if (_currentRoundUnits.Count == 0)
            {
                await RefreshRound();
                return;
            }
            if (_currentRoundUnits[0].UnitTurn.HasActionPoints == false)
            {
                RemoveUnitFromQueue(_currentRoundUnits[0]);
                await SetNewUnit();
                return;
            }
            CurrentTurnUnit = _currentRoundUnits[0];
            _unitSelection.SelectUnit();
            SideType sideType = _currentTurnUnit.Area.Side.SideType;
            switch (sideType)
            {
                case SideType.Ally:
                    ChangeStage(BattleStage.AllyAction);
                    break;
                case SideType.Enemy:
                    ChangeStage(BattleStage.EnemyAction);
                    break;
                default: throw new ArgumentOutOfRangeException($"There is no current SideType: {sideType}.");
            }
        }

        /// <summary>
        /// Refreshes the list of units.
        /// </summary>
        public async Task RefreshRound()
        {
            ClearOldUnit();

            Round++;
            if (Round > 1)
            {
                OnRoundChangedInterface.Invoke(Round);
                await BattlefieldUI.Instance.RoundPanel.StartAsync(Round);
            }

            _currentRoundUnits.Clear();
            _currentRoundUnits.AddRange(_battlefield.GetAllUnits());
            _currentRoundUnits = _currentRoundUnits.OrderByDescending(x => x.UnitStats.UnitInfo.Speed.MaxValue).ToList();
            _allUnitsInQueue = _currentRoundUnits.ToList();

            NextTurn(isMoveToEnd: false);
        }

        //Возможно его можно как-то оптимизировать?
        /// <summary>
        /// When any unit changes speed stat, update its position in the queue. 
        /// Does not change the position of the unit that is currently active and those that have completed their turn this round.
        /// </summary>
        private void OnSpeedChanged()
        {
            Debug.Log("OnSpeedChanged");
            var notActedunits = _currentRoundUnits.Where(x => x.UnitTurn.HasActionPoints && x != _currentTurnUnit).ToList();
            foreach (var item in notActedunits)
            {
                Debug.Log($"{item} - {item.UnitStats.UnitInfo.Speed.MaxValue}");
            }
            var orderBy = notActedunits.OrderByDescending(x => x.UnitStats.UnitInfo.Speed.MaxValue).ToList();

            _currentRoundUnits.RemoveRange(1, orderBy.Count);
            _currentRoundUnits.InsertRange(1, orderBy);

            _allUnitsInQueue.RemoveRange(1, orderBy.Count);
            _allUnitsInQueue.InsertRange(1, orderBy);

            UpdateVisual();
        }

        /// <summary>
        /// Updates a visual element.
        /// </summary>
        private void UpdateVisual()
        {
            //OnTurnChangedInterface.Invoke(_allUnitsInQueue);
            OnActionEndedInterface.Invoke();
            List<Unit> roundUnits;
            if (_currentRoundUnits.Count == 0)
            {
                roundUnits = _allUnitsInQueue.ToList();
            }
            else
            {
                roundUnits = _currentRoundUnits.ToList();
            }
            OnTurnChangedRoundUnitsInterface.Invoke(roundUnits);
        }

        /// <summary>
        /// Adds a unit to the end of the queue.
        /// </summary>
        private void AddUnitInQueue(Unit unit)
        {
            if (_currentRoundUnits.Contains(unit))
            {
                Debug.LogWarning($"{unit} already in turn queue.");
                return;
            }
            _currentRoundUnits.Add(unit);
            _allUnitsInQueue.Insert(_currentRoundUnits.Count - 1, unit);
            UpdateVisual();
        }

        /// <summary>
        /// Removes a unit from the queue.
        /// </summary>
        private void RemoveUnitFromQueue(Unit unit)
        {
            if (unit == null)
            {
                return;
            }
            if (_currentRoundUnits.Contains(unit))
            {
                _currentRoundUnits.Remove(unit);
            }
            _allUnitsInQueue.Remove(unit);
            if (unit.GetComponent<UnitDeath>().IsDead && unit == _currentTurnUnit)
            {
                NextTurn();
            }
            UpdateVisual();
        }
    }

    public enum BattleStage : byte
    {
        Preparation,
        AllyAction,
        EnemyAction,
        Waiting,
        End
    }

    public interface IBattleStagesSkipUnit
    {
        internal void SkipUnit(int unitsToSkip = 1);
    }
}