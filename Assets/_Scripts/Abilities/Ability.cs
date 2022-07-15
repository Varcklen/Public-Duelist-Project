using Project.Abilities.Data;
using Project.UnitNS;
using UnityEngine;
using System;
using Project.Abilities.Interfaces;
using Project.Utils.BaseInfoNS;
using Project.Utils.Events;
using Project.BattlefieldNS;

namespace Project.Abilities
{
    /// <summary>
    /// The class is the base class for any ability.
    /// </summary>
    public abstract class Ability : ActiveObject
    {
        [SerializeField] private AbilityInfo _abilityInfo;
        /// <summary>
        /// This field stores all the basic stats of the ability.
        /// </summary>
        public AbilityInfo AbilityInfo => _abilityInfo;

        private int _position;
        /// <summary>
        /// Returns the position of the ability on the ability bar of its type.
        /// </summary>
        public int Position => _position;

        public abstract AbilityType AbilityType { get; }
        /// <summary>
        /// Ability owner and the unit using this ability.
        /// </summary>
        public Unit Caster { get; private set; }

        /// <summary>
        /// Triggers when an ability is created. The base class call is not needed.
        /// </summary>
        protected virtual void Init() { }

        /// <summary>
        /// Is a class for calling a select action and getting the targets of that ability.
        /// </summary>
        protected ITargetSelection TargetModule { get; private set; }

        /// <summary>
        /// The class stores the interactions between the ability and the unit's resources.
        /// </summary>
        public AbilityResource ResourceModule { get; private set; }
        /// <summary>
        /// The class is responsible for interactions with this ability's cooldown.
        /// </summary>
        public AbilityCooldown CooldownModule { get; private set; }
        /// <summary>
        /// The class is responsible for ability bans, which prohibit the use of the ability under certain conditions.
        /// </summary>
        public AbilityBans BanModule { get; private set; }

        /// <summary>
        /// Use this instead of Awake. This method is called in the Create() method.
        /// </summary>
        private void CreateInit(int position)
        {
            Caster = GetComponent<Unit>();
            _position = position;

            TargetModule = TargetSelection.Create(this, AbilityInfo.TargetType);
            ResourceModule = new AbilityResource(this, Caster);
            CooldownModule = new AbilityCooldown(this, Caster);
            BanModule = new AbilityBans(this, Caster, ExtraBanConditions, ExtraUsableConditions);

            Init();
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
            if (this is IAbilityUsable usable)
            {
                usable.OnUnusable();
            }
        }

        #region Subscribe
        private void Subscribe()
        {
            if (this is IAbilityTurnStart turnStart) Caster.UnitTurn.OnTurnStart += turnStart.TurnStart;
            if (this is IAbilityTurnEnd abilityEnd) Caster.UnitTurn.OnTurnEnd += abilityEnd.TurnEnd;
            if (this is IAbilityBattleStart battleStart) Events.OnBattleStart.AddListener(battleStart.BattleStart);
            if (this is IAbilityBattleEnd battleEnd) Events.OnBattleEnd.AddListener(battleEnd.BattleEnd);
            if (this is IAbilityRoundEnd roundEnd) BattlefieldStages.Instance.OnRoundChanged.AddListener(roundEnd.RoundEnd);
            if (this is IAbilityUsable playable)
            {
                BanModule.OnAbilityBecomeUsable.AddListener(playable.OnUsable);
                BanModule.OnAbilityBecomeUnusable.AddListener(playable.OnUnusable);
            }
        }

        private void Unsubscribe()
        {
            if (this is IAbilityTurnStart turnStart) Caster.UnitTurn.OnTurnStart -= turnStart.TurnStart;
            if (this is IAbilityTurnEnd turnEnd) Caster.UnitTurn.OnTurnEnd -= turnEnd.TurnEnd;
            if (this is IAbilityBattleStart battleStart) Events.OnBattleStart.RemoveListener(battleStart.BattleStart);
            if (this is IAbilityBattleEnd battleEnd) Events.OnBattleEnd.RemoveListener(battleEnd.BattleEnd);
            if (this is IAbilityRoundEnd roundEnd) BattlefieldStages.Instance.OnRoundChanged.RemoveListener(roundEnd.RoundEnd);
            if (this is IAbilityUsable playable)
            {
                BanModule.OnAbilityBecomeUsable.RemoveListener(playable.OnUsable);
                BanModule.OnAbilityBecomeUnusable.RemoveListener(playable.OnUnusable);
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Allows you to create an ability for a unit.
        /// </summary>
        /// <typeparam name="T">Ability type.</typeparam>
        /// <param name="unit">Ability owner.</param>
        /// <param name="abilityData">Ability base data.</param>
        /// <param name="position">The position of the ability on the ability bar of the current type.</param>
        /// <returns></returns>
        public static T Create<T>(Unit unit, AbilityData abilityData, int position) where T : Ability
        {
            Component component = unit.gameObject.AddComponent(abilityData.PropertyType);
            dynamic ability = Convert.ChangeType(component, abilityData.PropertyType);

            //Copy Base Info
            ability._abilityInfo = abilityData.AbilityInfo.GetClone<AbilityInfo>();
            ((IBaseInfoInitialize)ability._abilityInfo).Initialize();
            //Copy Current Ability Info
            ((IAbilityComponent)ability).SetInfo(abilityData.Property.GetClone<PropertyInfo>());

            ability.CreateInit(position);
            return (T)ability;
        }
        #endregion

        /// <summary>
        /// Additional conditions for checking the ban for different types of abilities.
        /// </summary>
        /// <param name="useWarnings">True when the ability is pressed. Allows you to turn off notifications for cases when they need to be called only when an ability is pressed.</param>
        /// <returns></returns>
        protected abstract AbilityBan ExtraBanConditions(bool useWarnings);

        /// <summary>
        /// Allows you to add additional conditions for whether the ability can be used or not.
        /// </summary>
        protected virtual bool ExtraUsableConditions() { return true; }

        public virtual void Destroy()
        {
            Destroy(this);
        }
    }

    public enum AbilityType
    {
        Active,
        Passive,
        Skill
    }
}

