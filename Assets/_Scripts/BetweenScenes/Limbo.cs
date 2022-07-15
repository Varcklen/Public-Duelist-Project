using Project.UnitNS;
using System.Collections.Generic;
using UnityEngine;

namespace Project.BetweenScenes
{
    /// <summary>
    /// This is where all the travel information that needs to be transferred from one scene to another is stored.
    /// </summary>
    public sealed class Limbo : MonoBehaviour
    {
        [SerializeField] private List<Unit> _allyUnits;
        public List<Unit> AllyUnits => _allyUnits;

        public static Limbo Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

#if UNITY_EDITOR
            SetLimboUnits(_allyUnits);
#endif
        }

        /// <summary>
        /// Transfers the selected units to Limbo.
        /// </summary>
        public void SetLimboUnits(List<Unit> newUnits)
        {
            _allyUnits = newUnits;
            if (_allyUnits == null)
            {
                return;
            }
            foreach (var unit in _allyUnits)
            {
                unit.transform.SetParent(transform, false);
                unit.gameObject.SetActive(false);
            }
        }
    }
}
