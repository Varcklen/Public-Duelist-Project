using TMPro;
using Project.Utils.Extension.ObjectNS;
using Project.BattlefieldNS.Interfaces;
using Project.Stats;
using System;

namespace Project.BattlefieldNS
{
    public class StatDisplay : AreaUI, ISetResourceType
    {
        private Stat _stat;
        protected override Stat Stat { get => _stat; }

        private new void Awake()
        {
            base.Awake();
            Text = GetComponentInChildren<TextMeshPro>().IsNullException();
            _stat = UnitArea.PlacedUnit.UnitStats.UnitInfo.GetStat(StatType);
        }

        private new void OnEnable()
        {
            base.OnEnable();
            _stat.OnMaxValueChanged += SetValue;
        }

        private new void OnDisable()
        {
            base.OnDisable();
            _stat.OnMaxValueChanged -= SetValue;
        }

        public override void SetStatType(StatType statType)
        {
            if (statType != StatType.Attack && statType != StatType.Speed)
            {
                throw new Exception($"Wrong StatType. Must be {StatType.Attack} or {StatType.Speed}, but not {statType}.");
            }
            StatType = statType;
        }

        protected override void UpdateUI()
        {
            SetValue(_stat.MaxValue);
        }

        public void SetValue(int value)
        {
            Text.text = value.ToString();
            ChangeColor();
        }
    }
}
    
