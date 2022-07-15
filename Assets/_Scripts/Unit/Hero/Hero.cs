using Project.UnitNS.DataNS;
using UnityEngine;

namespace Project.UnitNS
{
    public sealed class Hero : Unit
    {
        [SerializeField] private HeroData _data;
        protected override UnitData data { get { return _data; } set { _data = (HeroData)value; } }

        public override UnitType UnitType => UnitType.Hero;
    }
}