using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Project.Utils.Events;
using Project.Utils.Events.Interfaces;
using Project.Singleton.MonoBehaviourSingleton;
using Project.UnitNS;
using Project.Utils.Exceptions;

namespace Project.BattlefieldNS
{
    /// <summary>
    /// Here are all the units that died during the battle. The class allows you to interact with them.
    /// </summary>
    public class Graveyard : MonoBehaviourSingleton<Graveyard>
    {
        private readonly Dictionary<Unit, SideType> _units = new Dictionary<Unit, SideType>();

        public readonly ActionEvent<Unit> OnGraveyardAdded = new ActionEvent<Unit>();
        private IEventInvoke<Unit> OnGraveyardAddedInterface => OnGraveyardAdded;

        private void OnEnable()
        {
            Events.OnUnitKill.AddListener(Add);
            Events.OnUnitRessurect.AddListener(RemoveFromGraveyard);
        }

        private void OnDisable()
        {
            Events.OnUnitKill.RemoveListener(Add);
            Events.OnUnitRessurect.RemoveListener(RemoveFromGraveyard);
        }

        private void Add(Unit unit)
        {
            if (_units.ContainsKey(unit))
            {
                Debug.LogWarning($"{unit.name} already in graveyard.");
                return;
            }
            if (unit.transform.IsChildOf(transform) == false)
            {
                unit.transform.SetParent(transform, false);
            }
            var sideType = unit.Area.Side.SideType;
            _units.Add(unit, sideType);
            OnGraveyardAddedInterface.Invoke(unit);
        }

        private void RemoveFromGraveyard(Unit unit)
        {
            bool isRemoved = _units.Remove(unit);
            if (!isRemoved)
            {
                Debug.LogWarning($"{unit.name} not in graveyard.");
            }
        }

        #region GetMethods
        public bool IsEmpty()
        {
            return _units.Count == 0;
        }
        public bool IsEmpty(SideType sideType)
        {
            return _units.Where(x => x.Value == sideType).ToList().Count == 0;
        }

        public Unit GetRandomUnit()
        {
            if (IsEmpty())
            {
                Debug.LogWarning($"Graveyard is empty. You can't get random unit.");
                return null;
            }
            var random = new System.Random();
            return _units.Keys.ElementAt(random.Next(0, _units.Count()));
        }
        public Unit GetRandomUnit(SideType sideType)
        {
            if (IsEmpty(sideType))
            {
                Debug.LogWarning($"{sideType} graveyard is empty. You can't get random unit.");
                return null;
            }
            var random = new System.Random();
            var list = _units.Where(x => x.Value == sideType).ToList();
            return list.ElementAtOrDefault(random.Next(0, list.Count())).Key;
        }

        public bool Contains(Unit unit)
        {
            return _units.ContainsKey(unit);
        }

        /// <exception cref="ElementNotContainedException"></exception>
        public SideType GetSideType(Unit unit)
        {
            bool isContained = _units.TryGetValue(unit, out SideType sideType);
            if (isContained == false) throw new ElementNotContainedException($"The current unit {unit} is not in the dictionary.");
            return sideType;
        }
        #endregion
    }
}
