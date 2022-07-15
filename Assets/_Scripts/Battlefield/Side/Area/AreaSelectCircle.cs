using UnityEngine;
using Project.Utils.Extension.ObjectNS;
using Project.Interfaces;

namespace Project.BattlefieldNS
{
    /// <summary>
    /// The component is responsible for interacting with the selection circle.
    /// </summary>
    [RequireComponent(typeof(Area))]
    public class AreaSelectCircle : MonoBehaviour
    {
        private Area _unitArea;
        private IShowHide _unitSelectCircle;
        private TargetCircle _targetCircle;
        private BattlefieldStages _battlefieldStages;

        private void Awake()
        {
            _battlefieldStages = BattlefieldStages.Instance;
            _unitArea = GetComponent<Area>();
            _unitSelectCircle = GetComponentInChildren<UnitSelectCircle>(includeInactive: true).IsNullException("chooseOutline cannot be null.");
            _targetCircle = GetComponentInChildren<TargetCircle>(includeInactive: true).IsNullException("_targetCircle cannot be null.");
        }

        private void OnEnable()
        {
            _battlefieldStages.OnActionEnded.AddListener(Check);
            _unitArea.OnUnitTargeted.AddListener(_targetCircle.TargetCircleEnable);
            _unitArea.OnUnitDetargeted.AddListener(_targetCircle.TargetCircleDisable);
        }

        private void OnDisable()
        {
            _battlefieldStages.OnActionEnded.RemoveListener(Check);
            _unitArea.OnUnitTargeted.RemoveListener(_targetCircle.TargetCircleEnable);
            _unitArea.OnUnitDetargeted.RemoveListener(_targetCircle.TargetCircleDisable);
        }

        private void Check()
        {
            if (_unitArea.PlacedUnit?.UnitTurn.IsSelected ?? false)
            {
                _unitSelectCircle.Show();
            }
            else
            {
                _unitSelectCircle.Hide();
            }
        }
    }
}
