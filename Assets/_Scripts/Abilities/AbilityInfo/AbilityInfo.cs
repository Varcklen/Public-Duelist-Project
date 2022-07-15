using Project.UnitNS.Interfaces;
using Project.Utils.BaseInfoNS;
using Project.Utils.IconNS;
using System;
using UnityEngine;

namespace Project.Abilities
{
    /// <summary>
    /// Stores basic information about an ability. It is an intermediary between Scriptible Object and Game Object.
    /// </summary>
    [Serializable]
    public class AbilityInfo : BaseInfo, IBaseInfoInitialize
    {
        [Header("String Information")]
        public string Name;
        [TextArea(5, 10)]
        public string Description;
        public Icon Icon;

        [Header("Parameters")]
        [Min(1)]
        public int Level = 1;
        public Cooldown Cooldown;
        public TargetType TargetType;
        public Cost Cost;
        [SerializeField] private Charge _charge;
        public Charge Charge => _charge;
        [SerializeField] private AbilityAI _abilityAI;
        public AbilityAI AbilityAI => _abilityAI;

        void IBaseInfoInitialize.Initialize()
        {
            ReplaceClasses();
        }

        //Any classes MUST BE replaced to a new ones!
        protected override void ReplaceClasses()
        {
            _charge = (Charge)_charge.Clone();
            _abilityAI = (AbilityAI)_abilityAI.Clone();
        }
    }
}
