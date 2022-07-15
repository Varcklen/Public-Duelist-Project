using Project.Utils.Extension.ObjectNS;
using Project.Singleton.MonoBehaviourSingleton;
using System.Collections.Generic;
using Project.UnitNS;

namespace Project.BattlefieldNS
{
    /// <summary>
    /// The component is responsible for the battlefield and the stages of the battle.
    /// </summary>
    public sealed class Battlefield : MonoBehaviourSingleton<Battlefield>
    {
        public Side AllySide { get; private set; }
        public Side EnemySide { get; private set; }
        public Graveyard Graveyard { get; private set; }
        public BattlefieldStages BattleStages { get; private set; }

        private new void Awake()
        {
            base.Awake();
            Graveyard = transform.GetComponentInChildren<Graveyard>().IsNullException();
            AllySide = transform.Find("Ally Side").GetComponent<Side>().IsNullException();
            EnemySide = transform.Find("Enemy Side").GetComponent<Side>().IsNullException();
            BattleStages = GetComponent<BattlefieldStages>().IsNullException();
        }

        #region GetMethods
        public Side GetOppositeSide(SideType sideType)
        {
            if (sideType == SideType.Ally)
            {
                return EnemySide;
            }
            return AllySide;
        }

        public Side GetSide(SideType sideType)
        {
            if (sideType == SideType.Ally)
            {
                return AllySide;
            }
            return EnemySide;
        }

        public List<Area> GetAllUnitAreas()
        {
            var list = new List<Area>(AllySide.UnitAreas);
            list.AddRange(EnemySide.UnitAreas);
            return list;
        }

        public List<Unit> GetAllUnits()
        {
            var list = new List<Unit>(AllySide.GetAllUnits());
            list.AddRange(EnemySide.GetAllUnits());
            return list;
        }
        #endregion
    }
}
