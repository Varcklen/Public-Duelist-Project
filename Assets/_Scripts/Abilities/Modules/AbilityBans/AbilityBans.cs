using Project.UnitNS;
using Project.Utils.Events;
using Project.Utils.Events.Interfaces;
using System;
using UnityEngine;

namespace Project.Abilities
{
    /// <summary>
    /// The class is responsible for ability bans, which prohibit the use of the ability under certain conditions.
    /// </summary>
    public class AbilityBans
    {
        private readonly Ability _ability;
        private readonly Func<bool, AbilityBan> _extraBanCondition;
        private readonly Func<bool> _isUsableCondition;

        public bool IsUsable => _abilityBanStatus == AbilityBan.None;

        private AbilityBan _abilityBanStatus = AbilityBan.None;
        public AbilityBan AbilityBanStatus => _abilityBanStatus;

        public readonly ActionEvent OnAbilityBecomeUsable = new ActionEvent();
        private IEventInvoke OnAbilityBecomeUsableInterface => OnAbilityBecomeUsable;
        public readonly ActionEvent OnAbilityBecomeUnusable = new ActionEvent();
        private IEventInvoke OnAbilityBecomeUnusableInterface => OnAbilityBecomeUnusable;

        public AbilityBans(Ability ability, Unit caster, Func<bool, AbilityBan> extraBanCondition, Func<bool> isUsableCondition)
        {
            _ability = ability;
            _extraBanCondition = extraBanCondition;
            _isUsableCondition = isUsableCondition;
        }

        private void SetAbilityBanStatus(AbilityBan abilityBan)
        {
            if (_abilityBanStatus == abilityBan) return;
            _abilityBanStatus = abilityBan;
            if (abilityBan == AbilityBan.None)
            {
                OnAbilityBecomeUsableInterface.Invoke();
            }
            else
            {
                OnAbilityBecomeUnusableInterface.Invoke();
            }
        }

        private AbilityBan CheckConditions(bool useWarnings)
        {
            var ban = _extraBanCondition(useWarnings);
            if (ban != AbilityBan.None)
            {
                return ban;
            }
            if (_ability.ResourceModule.IsEnoughResource == false)
            {
                if (useWarnings) Debug.LogWarning("Not enough resource!");
                return AbilityBan.NotEnoughResource;
            }
            if (_ability.CooldownModule.OnCooldown)
            {
                if (useWarnings) Debug.LogWarning("Ability on cooldown!");
                return AbilityBan.OnCooldown;
            }
            if (!_isUsableCondition())
            {
                if (useWarnings) Debug.LogWarning("Ability is not playable!");
                return AbilityBan.NotPlayable;
            }
            return AbilityBan.None;
        }

        /// <summary>
        /// Updates the ban status of the ability.
        /// </summary>
        /// <param name="useWarnings">True when the ability is pressed. Allows you to turn off notifications for cases when they need to be called only when an ability is pressed.</param>
        public void UpdateAbilityBan(bool useWarnings = false)
        {
            var ban = CheckConditions(useWarnings);
            if (ban != AbilityBan.None)
            {
                SetAbilityBanStatus(ban);
            }
            else
            {
                SetAbilityBanStatus(AbilityBan.None);
            }
        }
    }

    public enum AbilityBan : byte
    {
        None,
        NotEnoughResource,
        OnCooldown,
        Silenced,
        NotPlayable
    }
}
