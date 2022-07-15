using Project.Utils.BaseInfoNS;
using Project.Stats;
using Project.Stats.Interfaces;
using Project.UnitNS.Interfaces;
using System;
using UnityEngine;
using Project.Utils.IconNS;
using System.Collections.Generic;
using System.Linq;

namespace Project.UnitNS
{
    
    [Serializable]
    public class UnitInfo : BaseInfo, IBaseInfoInitialize
    {
        public string Name;
        public Icon Icon;
        [SerializeField] private SpeedStat _speed;
        public SpeedStat Speed => _speed;
        [SerializeField] private AttackStat _attack;
        public AttackStat Attack => _attack;
        [SerializeField] private HealthStat _health;
        public HealthStat Health => _health;
        [SerializeField] private ManaStat _mana;
        public ManaStat Mana => _mana;

        private List<Stat> _stats;
        private List<ResourceStat> _resources;

        void IBaseInfoInitialize.Initialize()
        {
            ReplaceClasses();
            Health.SetFullResource();
            Mana.SetFullResource();
            SetBaseValues();

            _stats.Add(_speed);
            _stats.Add(_attack);
            _stats.Add(_health);
            _stats.Add(_mana);

            _resources.Add(_health);
            _resources.Add(_mana);
        }

        private void SetBaseValues()
        {
            ((ISetBaseValue)Speed).SetBaseValue(Speed.MaxValue);
            ((ISetBaseValue)Attack).SetBaseValue(Attack.MaxValue);
            ((ISetBaseValue)Health).SetBaseValue(Health.MaxValue);
            ((ISetBaseValue)Mana).SetBaseValue(Mana.MaxValue);
        }

        //Any classes MUST BE replaced to a new ones!
        protected override void ReplaceClasses()
        {
            _speed = (SpeedStat)_speed.Clone();
            _attack = (AttackStat)_attack.Clone();
            _health = (HealthStat)_health.Clone();
            _mana = (ManaStat)_mana.Clone();
            _stats = new List<Stat>();
            _resources = new List<ResourceStat>();
        }

        #region GetMethods
        public Stat GetStat(StatType statType)
        {
            return _stats.First(x => x.StatType == statType);
        }

        public ResourceStat GetResourceStat(ResourceType resourceType)
        {
            return _resources.First(x => x.ResourceType == resourceType);
        }
        #endregion
    }
}
