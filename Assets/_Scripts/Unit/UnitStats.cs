using UnityEngine;
using Project.UnitNS.DataNS;
using Project.Utils.BaseInfoNS;
using Project.UnitNS.Interfaces;

namespace Project.UnitNS
{
    /// <summary>
    /// Stores information about the stats of the current unit.
    /// </summary>
    public sealed class UnitStats : MonoBehaviour, IUnitStatsInit
    {
        [SerializeField] private UnitInfo _unitInfo;
        public UnitInfo UnitInfo => _unitInfo;

        void IUnitStatsInit.Init(UnitData unitData)
        {
            _unitInfo = unitData.UnitInfo.GetClone<UnitInfo>();
            ((IBaseInfoInitialize)_unitInfo).Initialize();
        }
    }
}
