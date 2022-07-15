using UnityEngine;
using Project.Systems.DeathSystemNS;
using Project.Stats;

namespace Project.UnitNS
{
    /// <summary>
    /// Performs interactions with the unit's resources.
    /// </summary>
    [RequireComponent(typeof(UnitStats))]
    public sealed class UnitResourceManagment : MonoBehaviour
    {
        private UnitStats _unitStats;
        private UnitDeath _deathSystem;
        private Unit _unit;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
            _unitStats = GetComponent<UnitStats>();
            _deathSystem = GetComponent<UnitDeath>();
        }

        /// <summary>
        /// Causes the unit to take health damage.
        /// </summary>
        /// <param name="dealer">A unit that deals damage. If null, then the taking damage unit becomes a dealer.</param>
        public void TakeDamage(Unit dealer, int damage)
        {
            if (_deathSystem.IsDead)
            {
                Debug.LogWarning("You cannot deal damage to dead unit.");
                return;
            }
            if (damage <= 0)
            {
                Debug.LogWarning($"You can't deal negative damage. Use {nameof(Restore)} for it.");
                return;
            }
            if (dealer == null)
            {
                dealer = _unit;
            }

            _unitStats.UnitInfo.Health.SpendResource(damage);
            Debug.Log($"{dealer?.UnitStats.UnitInfo.Name} deal {damage} to {_unitStats.UnitInfo.Name}. Current health: {_unitStats.UnitInfo.Health.Value}");
        }

        /// <summary>
        /// Forces the unit to restore the selected resource.
        /// </summary>
        /// <param name="dealer">A unit that restore resource. If null, then the restoring resource unit becomes a healer.</param>
        public void Restore(Unit healer, ResourceType resourceType, int value)
        {
            if (_deathSystem.IsDead)
            {
                Debug.LogWarning("You cannot restore to dead unit.");
                return;
            }
            if (value <= 0)
            {
                Debug.LogWarning($"You can't restore negative value. Use {nameof(TakeDamage)} for it.");
                return;
            }

            if (healer == null)
            {
                healer = _unit;
            }

            var resource = _unitStats.UnitInfo.GetResourceStat(resourceType);
            resource.AddResource(value);
            Debug.Log($"{healer.UnitStats.UnitInfo.Name} restore {value} {resourceType} to {_unitStats.UnitInfo.Name}. Current resource: {resource.Value}");
        }

        /// <summary>
        /// Tries to restore health if value is greater than zero, or take damage if value is less than zero. At zero, nothing happens. 
        /// It is recommended for use when you have situations where the resulting number can be positive or negative and this is not known in advance.
        /// </summary>
        /// <param name="initiator">Healer or dealer.</param>
        public void TakeDamageOrRestore(Unit initiator, ResourceType resourceType, int value)
        {
            if (value > 0)
            {
                Restore(initiator, resourceType, value);
            }
            else if (value < 0)
            {
                TakeDamage(initiator, -value);
            }
        }
    }
}
