using Project.AI;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Project.Abilities
{
    /// <summary>
    /// Stores unit AI related information related to this ability.
    /// </summary>
    [Serializable]
    public class AbilityAI : ICloneable
    {
        [SerializeField] private AIAbilityUsePriority _usePriority = AIAbilityUsePriority.Normal;
        /// <summary>
        /// Changes the priorities for the AI to use this ability.
        /// </summary>
        public AIAbilityUsePriority UsePriority => _usePriority;
        [SerializeField] private List<Consideration> _considerations;
        /// <summary>
        /// Is a list of considerations that, depending on the conditions, allow the AI to choose how much to prioritize using this ability at the moment.
        /// </summary>
        public List<Consideration> Considerations => _considerations;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
