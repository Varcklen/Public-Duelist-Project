using Project.BattlefieldNS;
using Project.UnitNS;
using Project.Utils.Events;
using Project.Utils.Extension.UnitAreaNS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Abilities
{
    public abstract class AreaSelect : TargetUnitCheckingSelect
    {
        protected List<Area> resultAreas = new List<Area>();

        protected override ITargetResult GetTargetResult()
        {
            if (resultAreas.Count == 0)
            {
                return null;
            }
            var resultTargets = resultAreas.GetPlacedUnits();
            resultAreas.Clear();
            TargetList targetList = new TargetList();
            foreach (var target in resultTargets)
            {
                targetList.Add(target);
            }
            return targetList;
        }

        /// <summary>
        /// Fires when the cursor enters a potential target.
        /// </summary>
        protected abstract void OnCursorEnable(Area unitArea);

        protected override void Cancel()
        {
            resultAreas.Clear();
            base.Cancel();
        }

        /// <summary>
        /// Fires when the cursor enters the Target Circle.
        /// </summary>
        private void CursorEnable()
        {
            Vector3 vector = _camera.ScreenToWorldPoint(_mousePosition.ReadValue<Vector2>());
            RaycastHit2D hit = Physics2D.Raycast(vector, Vector2.zero, _camera.farClipPlane, _targetCircleLayerMask);
            if (hit.collider != null)
            {
                var unitArea = hit.transform.GetComponentInParent<Area>();
                SetTargets(unitArea);
            }
        }

        protected override void SetTargets(Area unitArea)
        {
            resultAreas.Clear();
            resultAreas.Add(unitArea);
            OnCursorEnable(unitArea);
            foreach (var area in resultAreas)
            {
                area.TargetCircle.EnableFlashing();
            }
        }

        /// <summary>
        /// Fires when the cursor leaves a potential target.
        /// </summary>
        private void CursorDisable()
        {
            foreach (var area in resultAreas)
            {
                area.TargetCircle.DisableFlashing();
            }
        }

        /// <summary>
        /// It will be possible to add a zone to the list if the conditions are met.
        /// </summary>
        protected void TryAddContains(Area unitArea)
        {
            if (_potentialTargets.Contains(unitArea) && !resultAreas.Contains(unitArea))
            {
                resultAreas.Add(unitArea);
            }
        }
        /// <summary>
        /// It will be possible to add a zone to the list if the conditions are met.
        /// </summary>
        protected void TryAddContains(List<Area> unitAreas)
        {
            foreach (var unitArea in unitAreas)
            {
                if (_potentialTargets.Contains(unitArea) && !resultAreas.Contains(unitArea))
                {
                    resultAreas.Add(unitArea);
                }
            }
        }

        protected sealed override void EnablePotentialTargets(Func<Area, bool> condition)
        {
            base.EnablePotentialTargets(condition);
            Events.OnTargetCircleCursorEnter.AddListener(CursorEnable);
            Events.OnTargetCircleCursorExit.AddListener(CursorDisable);
        }

        protected sealed override void DisablePotentialTargets()
        {
            base.DisablePotentialTargets();
            Events.OnTargetCircleCursorEnter.RemoveListener(CursorEnable);
            Events.OnTargetCircleCursorExit.RemoveListener(CursorDisable);
        }
    }
}
