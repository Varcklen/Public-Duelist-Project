using UnityEngine;
using System;
using Project.Utils.Events;
using Project.Utils.Events.Interfaces;

namespace Project.Stats
{
    [Serializable]
    public sealed class SpeedStat : Stat
    {
        public override StatType StatType => StatType.Speed;

        [SerializeField, Range(MIN_VALUE, MAX_VALUE)] private int _value;
        public override int MaxValue
        {
            get
            {
                var modifiedValue = Mathf.RoundToInt(ValueMultiplier * _value);
                return Mathf.Clamp(modifiedValue, MIN_VALUE, MAX_VALUE);
            }
            set
            {
                bool isChanged = _value != value;
                _value = value;
                if (!Application.isPlaying) return;

                if (isChanged == false) return;

                OnMaxValueChanged?.Invoke(MaxValue);
                ((IEventInvoke)Events.OnSpeedStatChanged).Invoke();
            }
        }

        public const int MIN_VALUE = -16;
        public const int MAX_VALUE = 16;
    }
}
