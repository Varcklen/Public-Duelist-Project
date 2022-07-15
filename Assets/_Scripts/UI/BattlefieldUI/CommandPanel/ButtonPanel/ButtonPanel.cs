using Project.Abilities;
using Project.BattlefieldNS;
using Project.Interfaces;
using Project.Singleton.MonoBehaviourSingleton;
using Project.UnitNS;
using Project.Utils.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI.BattlefieldNS
{
    public class ButtonPanel : MonoBehaviourSingleton<ButtonPanel>, IShowHide
    {
        private List<ActiveAbilityUI> _acviveAbilities = new List<ActiveAbilityUI>();
        private List<PassiveAbilityUI> _passiveAbilities = new List<PassiveAbilityUI>();
        private List<SkillUI> _skills = new List<SkillUI>();

        private UnitAbilities _unitAbilities;

        private new void Awake()
        {
            base.Awake();
            var active = transform.Find("ActiveAbilities").GetComponentsInChildren<ActiveAbilityUI>();
            var passive = transform.Find("PassiveAbilities").GetComponentsInChildren<PassiveAbilityUI>();
            var skills = transform.Find("Skills").GetComponentsInChildren<SkillUI>();

            _acviveAbilities.AddRange(active);
            _passiveAbilities.AddRange(passive);
            _skills.AddRange(skills);

            Hide();
        }

        #region Subscribe
        private bool isSubscribed;
        private void Subscribe()
        {
            if (isSubscribed || _unitAbilities == null) return;
            isSubscribed = true;
            _unitAbilities.OnActiveAbilityChanged += SetActiveAbility;
            _unitAbilities.OnPassiveAbilityChanged += SetPassiveAbility;
            _unitAbilities.OnSkillChanged += SetSkill;
            Events.OnAbilitySearchTargetStart.AddListener(DisableClickableButtons);
            Events.OnAbilityCastEnd.AddListener(EnableClickableButtons);
            Events.OnAbilityCanceled.AddListener(EnableClickableButtons);
        }

        private void Unsubscribe()
        {
            if (isSubscribed == false || _unitAbilities == null) return;
            isSubscribed = false;
            _unitAbilities.OnActiveAbilityChanged -= SetActiveAbility;
            _unitAbilities.OnPassiveAbilityChanged -= SetPassiveAbility;
            _unitAbilities.OnSkillChanged -= SetSkill;
            Events.OnAbilitySearchTargetStart.RemoveListener(DisableClickableButtons);
            Events.OnAbilityCastEnd.RemoveListener(EnableClickableButtons);
            Events.OnAbilityCanceled.RemoveListener(EnableClickableButtons);
        }
        #endregion

        private void SetActiveAbility(int position, Ability ability)
        {
            _acviveAbilities[position].SetAbility(ability);
        }

        private void SetPassiveAbility(int position, Ability ability)
        {
            _passiveAbilities[position].SetAbility(ability);
        }

        private void SetSkill(int position, Ability ability)
        {
            _skills[position].SetAbility(ability);
        }

        public void SetUnitAbilities(Unit unit)
        {
            if (_unitAbilities == unit.UnitAbilities)
            {
                UpdateButtons();
            }
            else
            {
                SetAbilities(unit);
            }
        }

        private void SetAbilities(Unit unit)
        {
            Unsubscribe();
            _unitAbilities = unit.UnitAbilities;
            SetAbilitiesCycle(_acviveAbilities, _unitAbilities.ActiveAbilities, unit);
            SetAbilitiesCycle(_passiveAbilities, _unitAbilities.PassiveAbilities, unit);
            SetAbilitiesCycle(_skills, _unitAbilities.Skills, unit);
            Subscribe();
        }

        private void UpdateButtons()
        {
            UpdateAbilitiesInList(_acviveAbilities);
            UpdateAbilitiesInList(_passiveAbilities);
            UpdateAbilitiesInList(_skills);

            void UpdateAbilitiesInList<T>(List<T> abilitiesUI) where T : AbilityUI
            {
                foreach (var item in abilitiesUI)
                {
                    item.UpdateButton();
                }
            }
        }

        private void EnableClickableButtons(ClickableAbility ability)
        {
            SetActiveButtonsActivness(true);
        }

        private void DisableClickableButtons(ClickableAbility ability)
        {
            SetActiveButtonsActivness(false);
        }

        private void SetActiveButtonsActivness(bool isActive)
        {
            foreach (var activeAbility in _acviveAbilities)
            {
                activeAbility.SetButtonInteractability(isActive);
            }
            foreach (var skill in _skills)
            {
                skill.SetButtonInteractability(isActive);
            }
        }

        private void SetAbilitiesCycle<T, K>(List<T> abilitiesUI, List<K> abilities, Unit unit) where T : AbilityUI where K : Ability
        {
            int count = abilitiesUI.Count;
            for (int i = 0; i < count; i++)
            {
                if (abilities.Count > i)
                {
                    abilitiesUI[i].SetAbility(abilities[i]);
                }
                else
                {
                    Debug.LogWarning($"Unit {unit.name} dont have enough abilities in {typeof(K).GetType()} list. Buttons with them will be rendered incorrectly.");
                    break;
                }
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

