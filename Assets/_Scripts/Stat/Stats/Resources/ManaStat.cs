using System;
using UnityEngine;

namespace Project.Stats
{
    [Serializable]
    public sealed class ManaStat : ResourceStat
    {
        public override StatType StatType => StatType.Mana;
        public override Color Color => Color.blue;
        public override ResourceType ResourceType => ResourceType.Mana;
    }
}