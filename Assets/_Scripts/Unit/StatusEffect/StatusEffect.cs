using UnityEngine;

namespace Project.UnitNS
{
    public class StatusEffect
    {
        private int _statusPoints = 0;
        public bool Enabled => _statusPoints > 0;

        public void Add()
        {
            _statusPoints++;
        }

        public void Remove()
        {
            _statusPoints--;
            if (_statusPoints < 0)
            {
                _statusPoints = 0;
                Debug.LogWarning("You can't remove disabled status effect.");
            }
        }
    }
}
