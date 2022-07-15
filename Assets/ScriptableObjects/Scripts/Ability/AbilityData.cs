using System;
using UnityEngine;

namespace Project.Abilities.Data
{
    [Serializable]
    public abstract class AbilityData : ScriptableObject
    {
        [SerializeField] private AbilityInfo _abilityInfo;
        public AbilityInfo AbilityInfo => _abilityInfo;

        public abstract PropertyInfo Property { get; }

        public abstract Type PropertyType { get; }

    }
}
