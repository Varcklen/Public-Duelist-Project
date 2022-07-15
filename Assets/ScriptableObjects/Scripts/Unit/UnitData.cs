using UnityEngine;
using System.Collections.Generic;
using Project.Abilities.Data;

namespace Project.UnitNS.DataNS
{
    public abstract class UnitData : ScriptableObject
    {
        public abstract GameObject unitPrefab { get; }

        [SerializeField] private UnitInfo _unitInfo;
        public UnitInfo UnitInfo => _unitInfo;

        [SerializeField] private List<ActiveAbilityData> _activeAbilities;
        public List<ActiveAbilityData> ActiveAbilities => _activeAbilities;

        [SerializeField] private List<PassiveAbilityData> _passiveAbilities;
        public List<PassiveAbilityData> PassiveAbilities => _passiveAbilities;

        [ContextMenu("Set Random Data")]
        private void SetRandomData()
        {
            var random = new System.Random();
            _unitInfo.Attack.MaxValue = random.Next(5, 10);
            _unitInfo.Speed.MaxValue = random.Next(-10, 10);
            _unitInfo.Health.MaxValue = random.Next(8, 15);
            _unitInfo.Mana.MaxValue = random.Next(5, 15);
        }
    }
}

