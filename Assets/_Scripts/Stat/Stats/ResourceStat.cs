using UnityEngine;
using System;

namespace Project.Stats
{
    /// <summary>
    /// This class is responsible for any type of stat that has a current value and its maximum value.
    /// </summary>
    [Serializable]
    public abstract class ResourceStat : Stat
    {
        public abstract ResourceType ResourceType { get; }
        public abstract Color Color { get; }
        /// <summary>
        /// Current value of resource. Can be equal to "MaxValue" or lower.
        /// </summary>
        public int Value { get; private set; }

        //They both must be non-ActionEvent, else subscribe will be affected to anyone.
        public event Action OnResourceEmpty;
        public event Action<int, int> OnResourceChanged;

        [SerializeField, Min(0)] private int _maxValue;
        /// <summary>
        /// Max value of resource.
        /// </summary>
        public override int MaxValue
        {
            get
            {
                return Mathf.RoundToInt(ValueMultiplier * _maxValue);
            }
            set
            {
                _maxValue = value;
                if (!Application.isPlaying) return;
                ResourceUpdate();
                OnMaxValueChanged?.Invoke(MaxValue);
            }
        }

        private float _resourceMultiplier = 1;
        public override float ValueMultiplier
        {
            get { return _resourceMultiplier; }
            set
            {
                _resourceMultiplier = value;
                if (!Application.isPlaying) return;
                ResourceUpdate();
                OnMaxValueChanged?.Invoke(MaxValue);
            }
        }

        public void SetFullResource()
        {
            Value = MaxValue;
            ResourceUpdate();
        }

        public void AddResource(int resource)
        {
            if (resource < 0)
            {
                Debug.Log($"You are using a negative value in {nameof(AddResource)}. We recommend using {nameof(SpendResource)} instead.");
            }
            Value += resource;
            ResourceUpdate();
        }

        public void SpendResource(int resource)
        {
            if (resource < 0)
            {
                Debug.Log($"You are using a negative value in {nameof(SpendResource)}. We recommend using {nameof(AddResource)} instead.");
            }
            Value -= resource;
            ResourceUpdate();
        }

        public void AddPercentResource(int percent)
        {
            Value += percent * MaxValue / 100;
            ResourceUpdate();
        }

        public void SetResource(int resource)
        {
            Value = resource;
            ResourceUpdate();
        }

        public void SetPercentResource(int percent)
        {
            Value = percent * MaxValue / 100;
            ResourceUpdate();
        }

        private void ResourceUpdate()
        {
            Value = Mathf.Clamp(Value, 0, MaxValue);
            OnResourceChanged?.Invoke(Value, MaxValue);
            if (Value <= 0)
            {
                Value = 0;
                OnResourceEmpty?.Invoke();
            }
        }
    }

    public enum ResourceType
    {
        Mana = StatType.Mana,
        Health = StatType.Health
    }
}
