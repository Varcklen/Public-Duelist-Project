using System;
using UnityEngine;

namespace Project.Stats
{
    [Serializable]
    public sealed class HealthStat : ResourceStat
    {
        public override StatType StatType => StatType.Health;
        public override Color Color => Color.red;
        public override ResourceType ResourceType => ResourceType.Health;
    }
}