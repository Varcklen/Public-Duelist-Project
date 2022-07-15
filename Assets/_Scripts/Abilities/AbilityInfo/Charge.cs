using System;
using UnityEngine;

namespace Project.Abilities
{
    /// <summary>
    /// Stores information related to the ability's charge.
    /// </summary>
    [Serializable]
    public class Charge : ICloneable
    {
        [SerializeField] private bool _visibleInGame;
        /// <summary>
        /// If true, the ability will display a counter equal to CurrentCharge.
        /// </summary>
        public bool VisibleInGame => _visibleInGame;
        public int CurrentCharge;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
