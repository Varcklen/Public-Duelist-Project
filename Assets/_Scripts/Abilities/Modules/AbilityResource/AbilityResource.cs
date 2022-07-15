using Project.UnitNS;
using Project.Stats;
using UnityEngine;
using System;

namespace Project.Abilities
{
    /// <summary>
    /// The class stores the interactions between the ability and the unit's resources.
    /// </summary>
    public class AbilityResource
    {
        private readonly AbilityInfo _abilityInfo;
        private readonly ResourceStat _resourceStat;

        /// <summary>
        /// Returns true if the unit has enough resource to use the ability.
        /// </summary>
        public bool IsEnoughResource => _resourceStat.Value >= _abilityInfo.Cost.Amount;

        public AbilityResource(Ability ability, Unit caster)
        {
            _abilityInfo = ability.AbilityInfo;
            ResourceType spendStatType = ability.AbilityInfo.Cost.CostType;
            _resourceStat = caster.UnitStats.UnitInfo.GetResourceStat(spendStatType);
        }

        /// <summary>
        /// Consumes a unit's resource equal to the value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Spend(int value)
        {
            if (value == 0) return;
            else if (value < 0)
            {
                throw new ArgumentOutOfRangeException($"You're trying to spend negative resource.");
            }
            if (IsEnoughResource == false)
            {
                Debug.LogWarning($"You're trying to spend resource, that you dont have. Current Resource: {_resourceStat.Value}, Trying to spend: {value}");
                return;
            }
            _resourceStat.SpendResource(value);
        }

        /// <summary>
        /// Consumes a unit's resource equal to the cost of the ability.
        /// </summary>
        public void Spend()
        {
            Spend(_abilityInfo.Cost.Amount);
        }
    }
}
