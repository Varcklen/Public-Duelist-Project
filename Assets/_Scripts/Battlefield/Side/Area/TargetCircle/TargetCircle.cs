using UnityEngine;
using Project.UI.Cursors;
using UnityEngine.EventSystems;
using System;
using Project.Utils.Events;
using Project.Utils.Events.Interfaces;

namespace Project.BattlefieldNS
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TargetCircle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private IUnitSelectCircle _unitSelectCircle;
        private Animator _animator;
        private int _flashingLayerIndex;

        private IEventInvoke _onCursorEnter;
        private IEventInvoke _onCursorExit;

        private void Awake()
        {
            _unitSelectCircle = transform.parent.GetComponentInChildren<UnitSelectCircle>(includeInactive: true);
            _animator = GetComponent<Animator>();
            _flashingLayerIndex = _animator.GetLayerIndex("Flashing");

            _onCursorEnter = Events.OnTargetCircleCursorEnter;
            _onCursorExit = Events.OnTargetCircleCursorExit;
        }

        private void OnDisable()
        {
            PointerExit();
            _animator.Rebind();
        }

        public void TargetCircleEnable()
        {
            gameObject.SetActive(true);
            _unitSelectCircle.DisableSelectCircle();
        }

        public void TargetCircleDisable()
        {
            gameObject.SetActive(false);
            _unitSelectCircle.EnableSelectCircle();
        }

        #region Flashing
        public bool IsFlashing { get; private set; }
        public void EnableFlashing()
        {
            if (IsFlashing) return;
            IsFlashing = true;
            _animator.SetLayerWeight(_flashingLayerIndex, 1);
        }

        public void DisableFlashing()
        {
            if (!IsFlashing) return;
            IsFlashing = false;
            _animator.SetLayerWeight(_flashingLayerIndex, 0);
        }
        #endregion

        #region OnPointer
        public bool IsPointed { get; private set; }
        public void OnPointerEnter(PointerEventData eventData)
        {
            IsPointed = true;
            EnableFlashing();
            CursorController.SetCursor(CursorType.Lightning);
            _onCursorEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExit();
            _onCursorExit.Invoke();
        }

        private void PointerExit()
        {
            IsPointed = false;
            DisableFlashing();
            CursorController.SetCursor(CursorType.Default);
        }
        #endregion

    }
}
