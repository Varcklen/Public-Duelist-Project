using Project.BattlefieldNS;
using Project.Singleton.ConfigurationNS;
using Project.Utils.Extension.UnitAreaNS;
using Project.Utils.TaskEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Abilities
{
    /// <summary>
    /// Any abilities that choose a target are children of this class.
    /// </summary>
    public abstract class TargetCheckingSelect : TargetSelection
    {
        protected List<Area> _potentialTargets { get; private set; } = new List<Area>();

        protected Camera _camera;

        protected int _targetCircleLayerMask;

        //InputSystem
        protected BaseControl _baseControl;
        protected InputAction _mousePosition;

        /// <summary>
        /// The variable is responsible for whether the player clicked to select the target of the ability or to cancel it.
        /// </summary>
        protected bool _isClicked;

        /// <summary>
        /// Fires when the left mouse button is pressed.
        /// </summary>
        protected void FindTargets()
        {
            Vector3 vector = _camera.ScreenToWorldPoint(_mousePosition.ReadValue<Vector2>());
            RaycastHit2D hit = Physics2D.Raycast(vector, Vector2.zero, _camera.farClipPlane, _targetCircleLayerMask);
            if (hit.collider != null)
            {
                OnCorrectClick(hit.transform.GetComponentInParent<Area>());
                _isClicked = true;
            }
        }
        /// <summary>
        /// Fires when the button is correctly clicked and returns the click target.
        /// </summary>
        /// <param name="unitArea"></param>
        protected virtual void OnCorrectClick(Area unitArea) { }

        /// <summary>
        /// Fires when the cancel button is pressed. In the method, you need to describe the actions to cancel the selection of the target.
        /// </summary>
        protected virtual void Cancel()
        {
            _isClicked = true;
        }

        protected override void Init()
        {
            base.Init();
            _targetCircleLayerMask = LayerMask.GetMask(Configuration.Instance.TargetCircleTag);
            _camera = Camera.main;

            //InputSystem
            _baseControl = new BaseControl();
            _mousePosition = _baseControl.Battlefield.MousePosition;
        }

        public async override Task<ITargetResult> GetTargetResultAsync(Func<Area, bool> condition, bool choosedRandomly = false)
        {
            EnablePotentialTargets(condition);
            if (_potentialTargets.Count == 0)
            {
                Debug.LogWarning("No targets for ability!");
                DisablePotentialTargets();
                return null;
            }
            if (choosedRandomly)
            {
                SetRandomTarget();
                DisablePotentialTargets();
                return GetTargetResult();
            }
            _isClicked = false;
            await TaskEx.WaitUntil(() => _isClicked);
            DisablePotentialTargets();
            return GetTargetResult();

            void SetRandomTarget()
            {
                var unitArea = _potentialTargets.GetRandomUnitArea();
                SetTargets(unitArea);
            }
        }

        protected abstract void SetTargets(Area unitArea);

        /// <summary>
        /// Returns the target or targets of the ability.
        /// </summary>
        protected abstract ITargetResult GetTargetResult();

        /// <summary>
        /// Performs all basic actions when target selection is enabled.
        /// </summary>
        protected virtual void EnablePotentialTargets(Func<Area, bool> condition)
        {
            //InputSystem
            _baseControl.Enable();
            _baseControl.Battlefield.TargetChoosing.performed += _ => FindTargets();
            _baseControl.Battlefield.TargetCanceling.performed += _ => Cancel();
            //
            SetPotentialTargets(condition);
            foreach (Area unitArea in _potentialTargets)
            {
                unitArea.TargetArea();
            }
        }

        /// <summary>
        /// Performs all basic actions when target selection is turned off.
        /// </summary>
        protected virtual void DisablePotentialTargets()
        {
            //InputSystem
            _baseControl.Disable();
            _baseControl.Battlefield.TargetChoosing.performed -= _ => FindTargets();
            _baseControl.Battlefield.TargetCanceling.performed -= _ => Cancel();
            //
            foreach (Area unitArea in _potentialTargets)
            {
                unitArea.DetargetArea();
            }
            _potentialTargets.Clear();
        }

        private void SetPotentialTargets(Func<Area, bool> condition)
        {
            Battlefield battlefield = Battlefield.Instance;
            TargetSide targetSide = _targetType.TargetSide;
            List<Area> list;
            switch (targetSide)
            {
                case TargetSide.Ally:
                    list = battlefield.GetSide(Caster.GetSideType()).UnitAreas;
                    break;
                case TargetSide.Enemy:
                    list = battlefield.GetOppositeSide(Caster.GetSideType()).UnitAreas;
                    break;
                case TargetSide.All:
                    list = battlefield.GetAllUnitAreas();
                    break;
                default: throw new Exception($"There no current targetSide: {targetSide}.");
            }
            ExtraConditions(ref list);
            list = list.Where(x => condition(x)).ToList();
            _potentialTargets.AddRange(list);
        }

        protected virtual void ExtraConditions(ref List<Area> list) { }
    }
}
