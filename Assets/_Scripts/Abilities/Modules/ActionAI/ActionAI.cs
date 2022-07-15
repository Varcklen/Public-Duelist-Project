using Project.AI;
using Project.Singleton.ConfigurationAbilitiesNS;
using Project.UnitNS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Abilities
{
    /// <summary>
    /// The class is responsible for the interaction of the ability with the AI of the unit.
    /// </summary>
    public class ActionAI
    {
        private readonly List<Consideration> _considerations;
        private readonly List<Consideration> _baseConsiderations;
        private readonly Unit _unit;
        private readonly AbilityInfo _abilityInfo;
        private readonly ClickableAbility _ability;

        private float _scoreMultiplier = 1;
        public float ScoreMultiplier { get { return _scoreMultiplier; } private set { _scoreMultiplier = Mathf.Clamp01(value); } }

        private const float _scoreMultiplierConsumedPerUse = 0.5f;
        private float _scoreConsumed = 0;
        private bool isUsedThisTurn = false;

        public ActionAI(ClickableAbility ability, Unit unit)
        {
            _unit = unit;
            _ability = ability;
            _abilityInfo = ability.AbilityInfo;
            _considerations = _abilityInfo.AbilityAI.Considerations;
            _baseConsiderations = ConfigurationAbilities.Instance.BaseConsiderations;

            unit.UnitTurn.OnTurnStart += OnTurnStart;
            unit.UnitTurn.OnTurnEnd += OnTurnEnd;
            ability.OnAbilityBeforeCast.AddListener(AfterUse);
            _scoreMultiplier = (int)_abilityInfo.AbilityAI.UsePriority * 0.2f;
        }

        ~ActionAI()
        {
            _unit.UnitTurn.OnTurnStart -= OnTurnStart;
            _unit.UnitTurn.OnTurnEnd -= OnTurnEnd;
            _ability.OnAbilityBeforeCast.RemoveListener(AfterUse);
        }

        /*
         * When an AI unit uses an ability, the interest multiplier decreases, causing the AI to want to use the ability less frequently.
         * If the AI did not use the ability during the turn, then at the end of the turn the multiplier returns to its original value.
         */
        #region Multiplier Changes on Use
        private void AfterUse()
        {
            isUsedThisTurn = true;
            var currentConsume = ScoreMultiplier * _scoreMultiplierConsumedPerUse;
            _scoreConsumed += currentConsume;
            ScoreMultiplier -= currentConsume;
        }

        private void OnTurnStart()
        {
            isUsedThisTurn = false;
        }

        private void OnTurnEnd()
        {
            if (isUsedThisTurn == false && _scoreConsumed > 0)
            {
                ScoreMultiplier += _scoreConsumed;
                _scoreConsumed = 0;
            }
        }
        #endregion

        /// <exception cref="Exception"></exception>
        public float GetScore()
        {
            float score = 1f;
            CheckScore(_baseConsiderations, ref score);
            if (score == 0) return 0;
            CheckScore(_considerations, ref score);
            if (score == 0) return 0;

            int divide = _baseConsiderations.Count + _considerations.Count;
            if (divide == 0)
            {
                throw new Exception("Ability must have at least one consideration to work properly!");
            }
            float modFactor = 1 - (1 / divide);
            float makeupValue = (1 - score) * modFactor;
            float result = (score * makeupValue) + score;
            return result * ScoreMultiplier;


            void CheckScore(List<Consideration> considerations, ref float score)
            {
                foreach (var consideration in considerations)
                {
                    if (consideration == null) throw new ArgumentNullException(consideration.ToString());

                    score *= consideration.Score(_unit, _ability);

                    if (score <= 0.0001f)
                    {
                        score = 0;
                        break;
                    }
                }
            }
        }
    }

    public enum AIAbilityUsePriority : byte
    {
        Normal = 3,
        Lowest = 1,
        Low = 2,
        High = 4,
        Highest = 5
    }
}
