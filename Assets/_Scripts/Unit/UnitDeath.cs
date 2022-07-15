using UnityEngine;
using Project.UnitNS;
using Project.Stats;
using Project.BattlefieldNS;
using Project.Utils.Events;
using Project.Utils.Events.Interfaces;
using Project.UnitNS.Interfaces;

namespace Project.Systems.DeathSystemNS
{
    /// <summary>
    /// The class is responsible for the unit's interaction with the graveyard and death.
    /// </summary>
    [RequireComponent(typeof(UnitStats))]
    public sealed class UnitDeath : MonoBehaviour, IOnAwake
    {
        /// <summary>
        /// Returns true if the unit is dead and in the graveyard.
        /// </summary>
        public bool IsDead { get; private set; } = false;

        private HealthStat _healthStat;
        private Unit _unit;

        public ActionEvent OnKill { get; } = new ActionEvent();
        private IEventInvoke OnKillInterface => OnKill;

        void IOnAwake.OnAwake()
        {
            
            _healthStat = GetComponent<UnitStats>().UnitInfo.Health;
            _unit = GetComponent<Unit>();

            _healthStat.OnResourceEmpty += Kill;
        }

        private void OnDestroy()
        {
            _healthStat.OnResourceEmpty -= Kill;
        }

        /// <summary>
        /// Kills a unit. The unit disappears from the battlefield and goes to the graveyard. A dead unit cannot interact with the battlefield.
        /// </summary>
        public void Kill()
        {
            if (IsDead)
            {
                Debug.LogWarning("You trying to kill dead unit.");
                return;
            }
            IsDead = true;
            _unit.gameObject.SetActive(false);
            //This sequence must be preserved.
            ((IEventInvoke<Unit>)Events.OnUnitKill).Invoke(_unit);
            OnKillInterface.Invoke();
            ((IEventInvoke<Unit>)Events.OnAfterUnitKill).Invoke(_unit);
            //
            Debug.Log("Unit is dead.");
        }

        /// <summary>
        /// Ressurect unit with current health and mana.
        /// </summary>
        public void Ressurect(SideType sideType, int health, int mana)
        {
            if (IsDead == false)
            {
                Debug.LogWarning("You trying to ressurect alive unit.");
                return;
            }
            Side side = Battlefield.Instance.GetSide(sideType);
            if (side.IsFull())
            {
                Debug.LogWarning($"You cant ressurect unit on {sideType}, because it's full.");
                return;
            }
            IsDead = false;
            side.GetFirstEmptyArea().SetPlacedUnit(_unit);
            ((IEventInvoke<Unit>)Events.OnUnitRessurect).Invoke(_unit);
            _unit.UnitStats.UnitInfo.Health.SetResource(health);
            _unit.UnitStats.UnitInfo.Mana.SetResource(mana);
        }

        /// <summary>
        /// Ressurect unit with full health and mana.
        /// </summary>
        public void Ressurect(SideType sideType)
        {
            Ressurect(sideType, _unit.UnitStats.UnitInfo.Health.MaxValue, _unit.UnitStats.UnitInfo.Mana.MaxValue);
        }

        /// <summary>
        /// Ressurect unit with percent health and mana.
        /// </summary>
        public void RessurectPercent(SideType sideType, int percentHealth, int percentMana)
        {
            Ressurect(sideType, _unit.UnitStats.UnitInfo.Health.MaxValue * percentHealth / 100, _unit.UnitStats.UnitInfo.Mana.MaxValue * percentMana / 100);
        }
    }
}
