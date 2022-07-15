using Project.Abilities;
using Project.UnitNS.DataNS;
using System.Collections.Generic;
using UnityEngine;
using Project.Abilities.Data;
using Project.Utils.Extension.ObjectNS;
using System;
using Project.Singleton.ConfigurationAbilitiesNS;
using Project.UnitNS.Interfaces;

namespace Project.UnitNS
{
    /// <summary>
    /// Stores information about the abilities of the current unit.
    /// </summary>
    [RequireComponent(typeof(UnitTurn))]
    public class UnitAbilities : MonoBehaviour
    {
        /// <summary>
        /// Returns a list of the unit's active abilities.
        /// </summary>
        public LimitList<ActiveAbility> ActiveAbilities { get; private set; }
        /// <summary>
        /// Returns a list of the unit's passive abilities.
        /// </summary>
        public LimitList<PassiveAbility> PassiveAbilities { get; private set; }
        /// <summary>
        /// Returns a list of the unit's skills.
        /// </summary>
        public LimitList<Skill> Skills { get; private set; }
        /// <summary>
        /// Returns a list of the unit's active abilities and skills.
        /// </summary>
        public readonly List<ClickableAbility> ClickableAbilities = new List<ClickableAbility>();
        private readonly List<Ability> _allAbilities = new List<Ability>();

        private Unit _unit;

        public event Action<int, ActiveAbility> OnActiveAbilityChanged;
        public event Action<int, PassiveAbility> OnPassiveAbilityChanged;
        public event Action<int, Skill> OnSkillChanged;

        private UnitTurn _unitSelection;

        private void Awake()
        {
            _unitSelection = GetComponent<UnitTurn>().IsNullException();
        }

        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            _unitSelection.OnUnitNextAction.AddListener(UpdateAllAbilitiesBans);
            _unitSelection.OnTurnStart += UpdateAllAbilitiesBans;
        }

        private void OnDisable()
        {
            _unitSelection.OnUnitNextAction.RemoveListener(UpdateAllAbilitiesBans);
            _unitSelection.OnTurnStart -= UpdateAllAbilitiesBans;
        }

        private void Init()
        {
            _unit = GetComponent<Unit>();
            ActiveAbilities = new LimitList<ActiveAbility>(ConfigurationAbilities.Instance.ActiveAbilitiesLimit);
            PassiveAbilities = new LimitList<PassiveAbility>(ConfigurationAbilities.Instance.PassiveAbilitiesLimit);
            Skills = new LimitList<Skill>(ConfigurationAbilities.Instance.BaseSkills.Count);

            UnitData unitData = ((IUnitData)_unit).Data;
            CreateAbilities(unitData.ActiveAbilities, ActiveAbilities, OnActiveAbilityChanged);
            CreateAbilities(unitData.PassiveAbilities, PassiveAbilities, OnPassiveAbilityChanged);
            CreateAbilities(ConfigurationAbilities.Instance.BaseSkills, Skills, OnSkillChanged);

            void CreateAbilities<K, T>(List<K> abilities, LimitList<T> listToAdd, Action<int, T> action) where K : AbilityData where T : Ability
            {
                T ability;
                for (int i = 0; i < abilities.Count; i++)
                {
                    if (listToAdd.isFull) break;
                    ability = Ability.Create<T>(_unit, abilities[i], i);
                    AddAbility(ability, listToAdd);
                    action?.Invoke(i, ability);
                }
            }
        }

        private void UpdateAllAbilitiesBans()
        {
            foreach (var item in _allAbilities)
            {
                item.BanModule.UpdateAbilityBan();
            }
        }

        private void AddAbility<T>(T ability, LimitList<T> listToAdd, int toPosition = -1) where T : Ability
        {
            if (ability == null)
            {
                Debug.LogWarning("You can't add nullable ability.");
                return;
            }
            if (toPosition == -1)
            {
                listToAdd.Add(ability);
            } else if (toPosition >= 0 || toPosition <= listToAdd.Count - 1)
            {
                listToAdd[toPosition] = ability;
            }
            else
            {
                throw new ArgumentOutOfRangeException($"toPosition must be on range: [0, {listToAdd.Count - 1}]. Current: {toPosition}");
            }
            _allAbilities.Add(ability);
            if (ability is ClickableAbility clickable)
            {
                clickable.OnAbilityAfterCast.AddListener(_unitSelection.RemoveCurrentActionPoint);
                ClickableAbilities.Add(clickable);
            }
        }

        private void RemoveAbility<T>(T ability, LimitList<T> listToRemove) where T : Ability
        {
            if (listToRemove.Contains(ability))
            {
                listToRemove.Remove(ability);
            }
            if (ability is ClickableAbility clickable)
            {
                clickable.OnAbilityAfterCast.RemoveListener(_unitSelection.RemoveCurrentActionPoint);
                ClickableAbilities.Remove(clickable);
            }
            _allAbilities.Remove(ability);
            ability.Destroy();
        }

        #region Replace
        public ActiveAbility ReplaceActiveAbility(ActiveAbilityData abilityData, int position)
        {
            if (position < 0 || position > ActiveAbilities.Limit)
            {
                throw new ArgumentOutOfRangeException($"You cannot replace ability on position #{position}. Use positions between [0, {ActiveAbilities.Limit}].");
            }
            var ability = Ability.Create<ActiveAbility>(_unit, abilityData, position);
            ReplaceAbility(position, ability, ActiveAbilities);
            OnActiveAbilityChanged?.Invoke(position, ActiveAbilities[position]);
            return ability;
        }
        public PassiveAbility ReplacePassiveAbility(PassiveAbilityData abilityData, int position)
        {
            if (position < 0 || position > PassiveAbilities.Limit)
            {
                throw new ArgumentOutOfRangeException($"You cannot replace ability on position #{position}. Use positions between [0, {PassiveAbilities.Limit}].");
            }
            var ability = Ability.Create<PassiveAbility>(_unit, abilityData, position);
            ReplaceAbility(position, ability, PassiveAbilities);
            OnPassiveAbilityChanged?.Invoke(position, PassiveAbilities[position]);
            return ability;
        }
        public Skill ReplaceSkill(SkillData abilityData, int position)
        {
            if (position < 0 || position > Skills.Limit)
            {
                throw new ArgumentOutOfRangeException($"You cannot replace ability on position #{position}. Use positions between [0, {Skills.Limit}].");
            }
            var ability = Ability.Create<Skill>(_unit, abilityData, position);
            ReplaceAbility(position, ability, Skills);
            OnSkillChanged?.Invoke(position, Skills[position]);
            return ability;
        }

        private void ReplaceAbility<T>(int position, T newAbility, LimitList<T> listToReplace) where T : Ability
        {
            if (position < 0 || position > listToReplace.Count - 1)
            {
                throw new ArgumentOutOfRangeException($"position must be on range: [0, {listToReplace.Count - 1}]. Current: {position}");
            }
            var oldAbility = listToReplace[position];

            AddAbility(newAbility, listToReplace, position);
            RemoveAbility(oldAbility, listToReplace);
        }
        #endregion
    }
}
