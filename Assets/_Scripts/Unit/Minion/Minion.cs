using Project.UnitNS.DataNS;
using UnityEngine;

namespace Project.UnitNS
{
    public sealed class Minion : Unit
    {
        [SerializeField] private MinionData _data;
        protected override UnitData data { get { return _data; } set { _data = (MinionData)value; } }

        public override UnitType UnitType => UnitType.Minion;
    }
}
