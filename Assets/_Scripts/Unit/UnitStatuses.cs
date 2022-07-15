using UnityEngine;

namespace Project.UnitNS
{
    /// <summary>
    /// Stores information about all possible standard unit statuses.
    /// </summary>
    public class UnitStatuses : MonoBehaviour
    {
        public readonly StatusEffect Stun = new StatusEffect();
        public readonly StatusEffect Silence = new StatusEffect();
    }
}
