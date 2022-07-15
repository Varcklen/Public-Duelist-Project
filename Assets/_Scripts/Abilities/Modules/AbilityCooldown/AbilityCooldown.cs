using Project.UnitNS;
using System;
using UnityEngine;

namespace Project.Abilities
{
    /// <summary>
    /// The class is responsible for interactions with this ability's cooldown.
    /// </summary>
    public class AbilityCooldown
    {
        private readonly Unit _caster;
        private readonly AbilityInfo _abilityInfo;

        public bool OnCooldown => _currentCooldown > 0;

        private int _currentCooldown;
        public int CurrentCooldown => _currentCooldown;
        
        public AbilityCooldown(Ability ability, Unit caster)
        {
            _caster = caster;
            _abilityInfo = ability.AbilityInfo;

            if (_abilityInfo.Cooldown.StartCooldown && _abilityInfo.Cooldown.Value > 0)
            {
                SetCooldown(_abilityInfo.Cooldown.Value + 1);
            }
            _caster.UnitTurn.OnTurnStart += DecreaseCooldown;
        }

        ~AbilityCooldown()
        {
            _caster.UnitTurn.OnTurnStart -= DecreaseCooldown;
        }

        /// <summary>
        /// Reduces the ability's cooldown by 1.
        /// </summary>
        public void DecreaseCooldown()
        {
            DecreaseCooldown(1);
        }

        /// <summary>
        /// Reduces the ability's cooldown by a certain amount.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void DecreaseCooldown(int valueToDecrease)
        {
            if (_currentCooldown == 0) return;
            if (valueToDecrease < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(valueToDecrease)} must be higher than 0. Currect: {valueToDecrease}.");
            }
            _currentCooldown = Math.Clamp(_currentCooldown - valueToDecrease, 0, int.MaxValue);
        }

        /// <summary>
        /// Sets the cooldown to the base value of the ability.
        /// </summary>
        public void SetCooldown()
        {
            SetCooldown(_abilityInfo.Cooldown.Value);
        }

        /// <summary>
        /// Sets the cooldown to a specific value.
        /// </summary>
        /// <param name="turnCooldown">The number of moves required to finish cooldown.</param>
        public void SetCooldown(int turnCooldown)
        {
            if (turnCooldown < 0)
            {
                Debug.Log($"{nameof(turnCooldown)} cannot be less that 0. It's setted to 0.");
            }
            _currentCooldown = Math.Clamp(turnCooldown, 0, int.MaxValue);
        }
    }
}
