using System;
using System.Collections.Generic;
using UnityEngine;
using Project.Utils.Extension.ObjectNS;
using Project.Utils.Events;
using Project.Utils.Events.Interfaces;
using Project.BattlefieldNS.Interfaces;
using Project.BattlefieldNS;
using Project.UnitNS.Interfaces;
using Project.UnitNS.DataNS;
using Project.Abilities;
using Project.Systems.DeathSystemNS;

namespace Project.UnitNS
{
    /// <summary>
    /// The class is the base for any unit. This class binds together the entire logic of the unit's behavior.
    /// </summary>
    [RequireComponent(
        typeof(UnitTurn), 
        typeof(UnitStats),
        typeof(UnitMovement)
    )]
    public abstract class Unit : ActiveObject, IOnAreaUpdate, IOnUnitCreateForLimbo, ITargetResult, IUnitData
    {
        protected abstract UnitData data { get; set; }
        /// <summary>
        /// The field is a link to the basic information about the unit from the Scriptable Object. 
        /// The data in Data is not allowed to be changed, as this will lead to changes in the Scriptable Object.
        /// </summary>
        UnitData IUnitData.Data => data;

        public abstract UnitType UnitType { get; }

        /// <summary>
        /// Returns the area in which the given unit is located.
        /// </summary>
        public Area Area { get; private set; }

        /// <summary>
        /// Stores information about the stats of the current unit.
        /// </summary>
        public UnitStats UnitStats { get; private set; }
        /// <summary>
        /// Stores information about the abilities of the current unit.
        /// </summary>
        public UnitAbilities UnitAbilities { get; private set; }
        /// <summary>
        /// Stores information about the turn of the current unit.
        /// </summary>
        public UnitTurn UnitTurn { get; private set; }
        /// <summary>
        /// Performs interactions with the unit's resources.
        /// </summary>
        public UnitResourceManagment UnitResourceManagment { get; private set; }
        /// <summary>
        /// Stores information about all possible standard unit statuses.
        /// </summary>
        public UnitStatuses UnitStatuses { get; private set; }
        /// <summary>
        /// Makes the unit interact with the battlefield, allowing it to move.
        /// </summary>
        public UnitMovement UnitMovement { get; private set; }
        /// <summary>
        /// The class is responsible for the unit's interaction with the graveyard and death.
        /// </summary>
        public UnitDeath UnitDeath { get; private set; }

        /// <summary>
        /// Used by Limbo units to reuse the Awake method
        /// </summary>
        private bool _isFirstTime = true;

        protected void Awake()
        {
            UnitStats = GetComponent<UnitStats>().IsNullException();
            UnitAbilities = GetComponent<UnitAbilities>().IsNullException();
            UnitTurn = GetComponent<UnitTurn>().IsNullException();
            UnitResourceManagment = GetComponent<UnitResourceManagment>().IsNullException();
            UnitStatuses = GetComponent<UnitStatuses>().IsNullException();
            UnitMovement = GetComponent<UnitMovement>().IsNullException();
            UnitDeath = GetComponent<UnitDeath>().IsNullException();
        }

        private void Start()
        {
            //Not recommended to use Start as Limbo units will not reuse it like Awake method.
        }

        protected virtual void AddData()
        {
            data.IsNullException();
            ((IUnitStatsInit)UnitStats).Init(data);
        }

        #region Create
        private static Dictionary<Type, Type> typeDictionary = new()
        {
            [typeof(HeroData)] = typeof(Hero),
            [typeof(MinionData)] = typeof(Minion),
        };

        /// <exception cref="Exception"></exception>
        public static Unit Create<T>(UnitData data, Area unitArea) where T : Unit
        {
            if (unitArea.PlacedUnit != null)
            {
                Debug.LogError($"You cannot use \"{unitArea.name}\" as parent, since it contains a unit.");
                return null;
            }
            bool isSuccesfull = typeDictionary.TryGetValue(data.GetType(), out Type myType);
            if (isSuccesfull == false)
            {
                throw new Exception($"typeDictionary variable has no mapping [key, value]. Maybe you forgot to add a new map?");
            }

            T unit = (T)Instantiate(data.unitPrefab, unitArea.transform, false).GetComponent(myType);
            unit.data = data;
            unit.InitializeUnit(unitArea);
            return unit;
        }

        /// <summary>
        /// This is where all the information needed to initialize a unit is stored (regardless of whether it's taken from limbo or instantiated).
        /// </summary>
        /// <param name="unitArea">Area in which the unit will be located.</param>
        private void InitializeUnit(Area unitArea)
        {
            AddData();
            unitArea.SetPlacedUnit(this);
            ((IEventInvoke<Unit>)Events.OnUnitCreate)?.Invoke(this);
            var awakes = transform.GetComponents<IOnAwake>();
            foreach (var awake in awakes)
            {
                awake.OnAwake();
            }
        }

        public static Unit Create<T>(UnitData data, SideType sideType) where T : Unit
        {
            Side side = Battlefield.Instance.GetSide(sideType);
            if (side.IsFull())
            {
                Debug.LogWarning($"You cant create new unit. {sideType} side is full.");
                return null;
            }
            return Create<T>(data, side.GetFirstEmptyArea());
        }
        #endregion

        #region GetMethod
        /// <exception cref="Exception"></exception>
        public SideType GetSideType()
        {
            if (Area == null)
            {
                throw new Exception("You cannot get sideType of unit there is not in battlefield.");
            }
            return Area.Side.SideType;
        }
        #endregion

        /// <summary>
        /// Called on the UnitArea to set it to a field of the Unit class.
        /// </summary>
        void IOnAreaUpdate.OnAreaUpdate(Area unitArea)
        {
            Area = unitArea;
        }

        /// <summary>
        /// Called on StartBattle to initialize the created unit as if it were instantiated.
        /// </summary>
        void IOnUnitCreateForLimbo.OnUnitCreateForLimbo()
        {
            gameObject.SetActive(true);
            if (_isFirstTime)
            {
                _isFirstTime = false;
            }
            else
            {
                Awake();
            }
            InitializeUnit(Battlefield.Instance.AllySide.GetFirstEmptyArea());
        }
    }

    public enum UnitType
    {
        Minion,
        Hero
    }
}