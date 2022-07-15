using Project.BattlefieldNS;
using Project.UnitNS;
using System.Collections.Generic;
using UnityEngine;
using Project.Abilities.Interfaces.Parents;

namespace Project.Abilities.Interfaces
{
    namespace Parents
    {
        /// <summary>
        /// Base interface for ability usage interfaces.
        /// </summary>
        public interface IAbilityClickable{}

        public interface IAbilityCast<T> : IAbilityClickable where T : ITargetResult 
        {
            void Use(T targetResult);
        }

        /// <summary>
        /// A class for all abilities with single target selection.
        /// </summary>
        public interface IAbilitySingleCast<T> : IAbilityCast<T> where T : ITargetResult
        {
            public ParticleSystem GetParticle(T targetResult);
        }
        /// <summary>
        /// A class for all abilities with multiply target selection.
        /// </summary>
        public interface IAbilityMultiplyCast<T> : IAbilityCast<T> where T : ITargetResult
        {
            public List<ParticleSystem> GetParticles(T targetResult);
        }
    }
    /// <summary>
    /// Add to an ability to use abilities with a single unit target.
    /// </summary>
    public interface IAbilityUse : IAbilitySingleCast<Unit> { }

    /// <summary>
    /// Add to an ability to use abilities with single area target.
    /// </summary>
    public interface IAbilityUseArea : IAbilitySingleCast<Area> { }

    /// <summary>
    /// Add to an ability to use abilities with multiple unit targets.
    /// </summary>
    public interface IAbilityUseTargets : IAbilityMultiplyCast<TargetList> { }

    /// <summary>
    /// Add to ability to cast after ability is used.
    /// </summary>
    internal interface IAbilityAfterUse
    {
        void AfterUse();
    }

    /// <summary>
    /// Triggered each time the owner's turn begins.
    /// </summary>
    internal interface IAbilityTurnStart
    {
        void TurnStart();
    }

    /// <summary>
    /// Triggered each time the owner's turn ends.
    /// </summary>
    internal interface IAbilityTurnEnd
    {
        void TurnEnd();
    }

    /// <summary>
    /// Triggers at the start of combat.
    /// </summary>
    internal interface IAbilityBattleStart
    {
        void BattleStart();
    }

    /// <summary>
    /// Triggers at the end of combat.
    /// </summary>
    internal interface IAbilityBattleEnd
    {
        void BattleEnd(SideType winnerSide);
    }

    /// <summary>
    /// Triggers at the end of the round.
    /// </summary>
    internal interface IAbilityRoundEnd
    {
        void RoundEnd(int newRound);
    }

    /// <summary>
    /// Triggered when an ability becomes usable and unusable.
    /// </summary>
    internal interface IAbilityUsable
    {
        void OnUsable();
        void OnUnusable();
    }
}
