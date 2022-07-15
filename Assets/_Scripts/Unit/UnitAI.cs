using Project.Abilities;
using UnityEngine;
using System;
using Project.BattlefieldNS;

namespace Project.UnitNS
{
    /// <summary>
    /// The class is responsible for the behavior of the unit when controlling the AI.
    /// </summary>
    public class UnitAI : MonoBehaviour
    {
        private UnitAbilities _unitAbilities;
        private UnitTurn _unitSelection;
        private Unit _unit;

        private bool _isActive;
        /// <summary>
        /// Returns true if the unit is AI controlled.
        /// </summary>
        public bool IsActive => _isActive;

        private void Awake()
        {
            _unitAbilities = GetComponent<UnitAbilities>();
            _unitSelection = GetComponent<UnitTurn>();
            _unit = GetComponent<Unit>();

            _unitSelection.OnTurnStart += EnableAI;
            _unitSelection.OnTurnEnd += DisableAI;
            _unitSelection.OnUnitNextAction.AddListener(NextAction);
        }

        private void OnDestroy()
        {
            _unitSelection.OnTurnStart -= EnableAI;
            _unitSelection.OnTurnEnd -= DisableAI;
            _unitSelection.OnUnitNextAction.RemoveListener(NextAction);
        }

        private void EnableAI()
        {
            if (_unit.GetSideType() == SideType.Ally) return;
            _isActive = true;
            NextAction();
        }

        private void DisableAI()
        {
            _isActive = false;
        }

        private void NextAction()
        {
            if (_unit.GetSideType() == SideType.Ally || !_unitSelection.IsSelected)
            {
                DisableAI();
                return;
            }
            var bestAbility = GetBestAbility();
            if (bestAbility == null)
            {
                throw new ArgumentNullException("Ability to use for AI must excist!");
            }
            bestAbility.TryUseAbility(isUsedRandomly:true);
        }

        private ClickableAbility GetBestAbility()
        {
            float maxScore = 0f;
            int abilityIndex = -1;
            var list = _unitAbilities.ClickableAbilities;
            int listCount = list.Count;
            float currentScore;
            for (int i = 0; i < listCount; i++)
            {
                currentScore = list[i].ActionAI.GetScore();
                if (currentScore > maxScore)
                {
                    maxScore = currentScore;
                    abilityIndex = i;
                }
            }
            if (abilityIndex == -1)
            {
                throw new Exception($"Unit {_unit} don't have any usable clickable abilities.");
            }
            return list[abilityIndex];
        }
    }
}

