using UnityEngine;
using System;

namespace Project.Stats
{
    [Serializable]
    public sealed class AttackStat : Stat
    {
        public override StatType StatType => StatType.Attack;

        [SerializeField, Min(0)] private int _value;
        public override int MaxValue
        {
            get
            {
                return Mathf.RoundToInt(ValueMultiplier*_value);
            }
            set
            {
                _value = value;
                if (!Application.isPlaying) return;
                OnMaxValueChanged?.Invoke(MaxValue);
            }
        }

    }
}
