using Project.Attributes;
using System;
using UnityEngine;

namespace Project.Abilities
{
    /// <summary>
    /// Stores information related to an ability's cooldown.
    /// </summary>
    [Serializable]
    public struct Cooldown
    {
        /// <summary>
        /// The number of turns required to wait before the ability becomes active again after using it.
        /// </summary>
        [Min(0)]
        public int Value;
        /// <summary>
        /// If true, ability will be on cooldown at the start of battle.
        /// </summary>
        [IntHide("Value", 0, hideInInspector: true), Tooltip("If true, ability will be on cooldown at the start of battle.")]
        public bool StartCooldown;
    }
}
