using Project.Attributes;
using Project.Stats;
using System;
using UnityEngine;

namespace Project.Abilities
{
    /// <summary>
    /// Stores information related to the cost of this ability.
    /// </summary>
    [Serializable]
    public struct Cost
    {
        /// <summary>
        /// The required amount of resource to use this ability.
        /// </summary>
        [Min(0)]
        public int Amount;
        /// <summary>
        /// When consumed, uses the selected resource type.
        /// </summary>
        [IntHide("Amount", 0, hideInInspector: true)]
        public ResourceType CostType;
    }

}
