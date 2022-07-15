using UnityEngine;
using UnityEngine.UI;
using Project.Abilities;
using System;

namespace Project.UI.BattlefieldNS
{
    public abstract class AbilityUI : MonoBehaviour
    {
        protected abstract Image Image { get; set; }
        protected abstract Button Button { get; set; }

        private AbilityChargesUI _charges;
        private CooldownUI _cooldownUI;
        private AbilityFrontgroundUI _frontground;

        private Ability _ability;
        protected Ability Ability => _ability;
        private AbilityBan _abilityBan;

        protected void Awake()
        {
            _charges = GetComponentInChildren<AbilityChargesUI>();
            _cooldownUI = GetComponentInChildren<CooldownUI>();
            _frontground = GetComponentInChildren<AbilityFrontgroundUI>();
        }

        private void Start()
        {
            _cooldownUI.Hide();
            _frontground.Hide();
            _charges.Hide();
        }

        #region Subscribe
        private bool isSubscribed;
        private void Subscribe()
        {
            if (_ability is ClickableAbility clickable)
            {
                Button?.onClick.AddListener(clickable.Click);
            }
            if (isSubscribed) return;
            isSubscribed = true;
            _ability?.Caster.UnitTurn.OnAfterUnitNextAction.AddListener(UpdateButton);
        }

        private void Unsubscribe()
        {
            if (_ability is ClickableAbility clickable)
            {
                Button?.onClick.RemoveListener(clickable.Click);
            }
            if (isSubscribed == false) return;
            isSubscribed = false;
            _ability?.Caster.UnitTurn.OnAfterUnitNextAction.RemoveListener(UpdateButton);
        }
        #endregion

        public void SetAbility(Ability ability)
        {
            if (ability == null)
            {
                Debug.LogWarning("You can't set AbilityUI. ability is null.");
                return;
            }
            ClearOldAbility();
            _ability = ability;
            SetNewAbility();
        }

        private void ClearOldAbility()
        {
            if (_ability == null) return;
            Unsubscribe();
        }

        private void SetNewAbility()
        {
            if (_ability == null) throw new ArgumentNullException("Ability cannot be null.");
            Image.sprite = _ability.AbilityInfo.Icon.Sprite;
            Subscribe();
            UpdateButton();
        }

        public void UpdateButton()
        {
            _ability.BanModule.UpdateAbilityBan();
            _abilityBan = _ability.BanModule.AbilityBanStatus;
            SetCooldown();
            SetFrontGround();
            SetCharges();
        }

        private void SetCooldown()
        {
            var cooldownModule = _ability.CooldownModule;
            if (cooldownModule.OnCooldown)
            {
                _cooldownUI.Show();
                _cooldownUI.SetCooldown(cooldownModule.CurrentCooldown);
            }
            else
            {
                _cooldownUI.Hide();
            }
        }

        private void SetCharges()
        {
            Charge charge = _ability.AbilityInfo.Charge;
            if (charge.VisibleInGame)
            {
                _charges.Show();
                _charges.SetCharges(charge.CurrentCharge);
            }
            else
            {
                _charges.Hide();
            }
        }

        private void SetFrontGround()
        {
            if (_abilityBan == AbilityBan.None)
            {
                _frontground.Hide();
            }
            else
            {
                _frontground.Show();
                _frontground.SetFrontgroundColor(_abilityBan);
            }
        }
    }
}
