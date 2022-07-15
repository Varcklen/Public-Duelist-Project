using TMPro;
using UnityEngine;
using Project.Utils.Extension.ObjectNS;
using Project.Stats;
using System;

namespace Project.BattlefieldNS
{
    public class ResourceBar : AreaUI
    {
        private Transform _slider;

        private ResourceStat _resourceStat;
        protected override Stat Stat { get => _resourceStat; }

        private new void Awake()
        {
            base.Awake();
            _slider = transform.Find("Bar").Find("BarSlider").transform.IsNullException();
            Text = transform.GetComponentInChildren<TextMeshPro>().IsNullException();
            _resourceStat = UnitArea.PlacedUnit.UnitStats.UnitInfo.GetResourceStat((ResourceType)StatType);
        }

        private new void OnEnable()
        {
            base.OnEnable();
            _resourceStat.OnResourceChanged += SetSliderValue;
        }

        private new void OnDisable()
        {
            base.OnDisable();
            _resourceStat.OnResourceChanged -= SetSliderValue;
        }

        public override void SetStatType(StatType statType)
        {
            if (statType != StatType.Health && statType != StatType.Mana)
            {
                throw new Exception($"Wrong StatType. Must be {StatType.Health} or {StatType.Mana}, but not {statType}.");
            }
            StatType = statType;
        }

        protected override void UpdateUI()
        {
            SetSliderValue(_resourceStat.Value, _resourceStat.MaxValue);
        }

        private void SetSliderValue(int value, int maxValue)
        {
            if (maxValue == 0)
            {
                Debug.LogWarning("maxValue equal to 0.");
                Hide();
                return;
            }
            Text.text = $"{value}/{maxValue}";
            float clampValue = Mathf.Clamp01((float)value / maxValue);
            _slider.localScale = new Vector2(clampValue, _slider.localScale.y);
            ChangeColor();
        }
    }
}
