using Project.Stats.Interfaces;
using System;

namespace Project.Stats
{
    /// <summary>
    /// The class is the base for any stat.
    /// </summary>
    public abstract class Stat : ISetBaseValue, ICloneable
    {
        public abstract StatType StatType { get; }

        /// <summary>
        /// The current value of the resource.
        /// </summary>
        public abstract int MaxValue { get; set; }
        /// <summary>
        /// The data base value of the resource.
        /// </summary>
        public int BaseValue { get; private set; }
        /// <summary>
        /// Allows you to change MaxValue as a percentage. 0.2f of ValueMultiplier is 20%.
        /// </summary>
        private float _valueMultiplier = 1;
        public virtual float ValueMultiplier
        {
            get { return _valueMultiplier; }
            set
            {
                _valueMultiplier = value;
                OnMaxValueChanged?.Invoke(MaxValue);
            }
        }

        public Action<int> OnMaxValueChanged;

        void ISetBaseValue.SetBaseValue(int value)
        {
            BaseValue = value;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public enum StatType : byte
    {
        Health,
        Mana,
        Attack,
        Speed
    }
}
