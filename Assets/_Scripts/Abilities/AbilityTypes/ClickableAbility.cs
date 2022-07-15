using Project.Abilities.Interfaces;
using Project.Abilities.Interfaces.Parents;
using Project.BattlefieldNS;
using Project.UnitNS;
using Project.Utils.Events;
using Project.Utils.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Abilities
{
    /// <summary>
    /// These abilities are any abilities that can be used.
    /// </summary>
    public abstract class ClickableAbility : Ability
    {
        public readonly ActionEvent OnAbilityBeforeCast = new ActionEvent();
        private IEventInvoke OnAbilityBeforeCastInterface => OnAbilityBeforeCast;
        public readonly ActionEvent OnAbilityAfterCast = new ActionEvent();
        private IEventInvoke OnAbilityAfterCastInterface => OnAbilityAfterCast;

        /// <summary>
        /// The class is responsible for the interaction of the ability with the AI of the unit.
        /// </summary>
        public ActionAI ActionAI { get; private set; }

        private void Start()
        {
            ActionAI = new ActionAI(this, Caster);
        }

        public void Click()
        {
            TryUseAbility(false);
        }

        /// <summary>
        /// Makes an attempt to use an ability by finding targets and casting the ability on them.
        /// </summary>
        /// <param name="isUsedRandomly">Instead of a normal target search, targets will be randomly selected.</param>
        public async void TryUseAbility(bool isUsedRandomly)
        {
            if ((this is IAbilityClickable) == false)
            {
                Debug.LogWarning($"You're trying to use ability that don't have {nameof(IAbilityClickable)} successor interface.");
                return;
            }
            // == Checking ==
            BanModule.UpdateAbilityBan(useWarnings: true);
            if (BanModule.AbilityBanStatus != AbilityBan.None) return;

            ((IEventInvoke<ClickableAbility>)Events.OnAbilitySearchTargetStart).Invoke(this);
            ITargetResult result = await TargetModule.GetTargetResultAsync(GetTargetCondition(), isUsedRandomly);
            if (result == null)
            {
                Debug.Log("Canceled!");
                ((IEventInvoke<ClickableAbility>)Events.OnAbilityCanceled).Invoke(this);
                return;
            }

            // == true => Consume ==
            Consume();
            // == Use ==
            UseAsync(result);

            void Consume()
            {
                ResourceModule.Spend();
                CooldownModule.SetCooldown();
            }
        }

        public override void Destroy()
        {
            //Возможно стоит вернуть? Нужны тесты
            //OnAbilityAfterCast.Clear();
            base.Destroy();
        }

        /// <summary>
        /// Depending on the found target, applies the ability.
        /// </summary>
        /// <param name="result">Found target.</param>
        private async void UseAsync(ITargetResult result)
        {
            OnAbilityBeforeCastInterface.Invoke();
            bool isFinded = await CheckSingleAsync<IAbilityUse, Unit>(result);
            if (isFinded == false) isFinded = await CheckSingleAsync<IAbilityUseArea, Area>(result);
            if (isFinded == false) isFinded = await CheckMultiplyAsync<IAbilityUseTargets, TargetList>(result);

            if (Caster.UnitTurn.HasActionPoints == false) return;

            if (isFinded == false)
            {
                throw new Exception($"You're trying to use incorrect target ability type and target type. Ability: {GetType()}, Interface: {result.GetType()}.");
            }
            if (this is IAbilityAfterUse thisInterface)
            {
                thisInterface.AfterUse();
            }
            OnAbilityAfterCastInterface.Invoke();
            ((IEventInvoke<ClickableAbility>)Events.OnAbilityCastEnd).Invoke(this);
        }

        //If the ability type and interface match, the ability is used.
        #region CheckAbilityTargetType
        private async Task<bool> CheckSingleAsync<T, K>(ITargetResult result) where T : IAbilitySingleCast<K> where K : ITargetResult
        {
            if (this is T ability && result is K target)
            {
                await PlaySingleParticleAsync(ability, target);
                ability.Use(target);
                return true;
            }
            return false;
        }

        private async Task PlaySingleParticleAsync<T, K>(T abilityUse, K targetResult) where T : IAbilitySingleCast<K> where K : ITargetResult
        {
            ParticleSystem ps = abilityUse.GetParticle(targetResult);
            if (ps == null)
            {
                await Task.Delay(250);
                return;
            }
            int miliseconds = (int)(ps.main.duration * 1000);
            await Task.Delay(miliseconds);
        }

        private async Task<bool> CheckMultiplyAsync<T, K>(ITargetResult result) where T : IAbilityMultiplyCast<K> where K : ITargetResult
        {
            if (this is T ability && result is K target)
            {
                await PlayMultiplyParticleAsync(ability, target);
                ability.Use(target);
                return true;
            }
            return false;
        }

        private async Task PlayMultiplyParticleAsync<T, K>(T abilityUse, K targetResult) where T : IAbilityMultiplyCast<K> where K : ITargetResult
        {
            List<ParticleSystem> ps = abilityUse.GetParticles(targetResult);
            if (ps == null)
            {
                await Task.Delay(250);
                return;
            }
            var taskList = new List<Task>();
            int miliseconds;
            foreach (var item in ps)
            {
                miliseconds = (int)(item.main.duration * 1000);
                taskList.Add(Task.Delay(miliseconds));
            }
            await Task.WhenAll(taskList);
        }
        #endregion

        /// <summary>
        /// Additional conditions for the search for a target are prescribed here.
        /// </summary>
        protected virtual Func<Area, bool> GetTargetCondition()
        {
            return x => true;
        }
    }
}
