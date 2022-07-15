using Project.Singleton.ConfigurationNS;
using UnityEngine;
using System;
using System.Collections.Generic;
using Project.UnitNS;
using Project.BattlefieldNS;

namespace Project.UI.BattlefieldNS
{
    public class QueueSeparator : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private const int _positionMin = 1;
        private const int _cellSpacing = 100;
        private int _positionMax;
        private float _minRectPosition;

        private BattlefieldStages _battlefieldStages;
    
        private void Awake()
        {
            _positionMax = Configuration.Instance.AllSlotsCount;
            _rectTransform = GetComponent<RectTransform>();
            _battlefieldStages = BattlefieldStages.Instance;
            _minRectPosition = _rectTransform.localPosition.x - _cellSpacing;
            _battlefieldStages.OnTurnChangedRoundUnits.AddListener(SetPosition);
        }

        //Возможно будет вызывать ошибки, так как не в OnDisable/OnEnable
        private void OnDestroy()
        {
            _battlefieldStages.OnTurnChangedRoundUnits.RemoveListener(SetPosition);
        }

        private void SetPosition(List<Unit> units)
        {
            SetPosition(units.Count);
        }

        public void SetPosition(int position)
        {
            if (position < _positionMin || position > _positionMax)
            {
                throw new ArgumentOutOfRangeException($"position must be on range [{_positionMin},{_positionMax}]. Current: {position}.");
            }
            _rectTransform.localPosition = new Vector2(_minRectPosition + (_cellSpacing * position), _rectTransform.localPosition.y);
        }
    }
}