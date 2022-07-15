using UnityEngine;
using System;
using Project.Abilities.Interfaces;
using Project.UnitNS;
using Project.BattlefieldNS;
using System.Threading.Tasks;
using System.Collections.Generic;
using Project.Particles;

namespace Project.Abilities.Data
{
    [CreateAssetMenu(fileName = "New AoEActiveTest", menuName = "Scriptable Objects/Ability/ActiveAbility/AoEActiveTest")]
    public class AoEActiveTest : ActiveAbilityData
    {
        [SerializeField] private AoEActiveTestInfo _testPropertyInfo;
        public override PropertyInfo Property { get => _testPropertyInfo; }
        public override Type PropertyType => typeof(AoEActiveTestComponent);
    }
}

namespace Project.Abilities
{
    [Serializable]
    public class AoEActiveTestInfo : PropertyInfo
    {
        public Unit SomeUnit;
    }

    public class AoEActiveTestComponent : ActiveAbilityComponent<AoEActiveTestInfo>, IAbilityUseTargets, IAbilityBattleStart, IAbilityTurnStart, IAbilityTurnEnd, IAbilityBattleEnd, IAbilityRoundEnd
    {
        protected override bool ExtraUsableConditions()
        {
            return Caster.UnitStats.UnitInfo.Mana.Value <= 2;
        }

        protected override Func<Area, bool> GetTargetCondition()
        {
            return x => x.PlacedUnit.UnitStats.UnitInfo.Attack.MaxValue < 10;
        }
        public void Use(TargetList targetResult)
        {
            Debug.Log($"Used! {GetType()}");
            Debug.Log($"Caster: {Caster}, targets:");
            foreach (var target in targetResult)
            {
                Debug.Log(target);
            }
            
            Debug.Log($"AoEActiveTestInfo.unit: {Info.SomeUnit}.");
            //Info.SomeName += "e";
            //AbilityInfo.Name += "e";
        }

        public void BattleStart()
        {
            //Debug.Log("Ability BattleStart");
        }

        public void TurnStart()
        {
            //Debug.Log("Ability TurnStart");
        }

        public void TurnEnd()
        {
            //Debug.Log("Ability TurnEnd");
        }

        public void BattleEnd(SideType sideType)
        {
            //Debug.Log("Battle End");
        }

        public void RoundEnd(int newRound)
        {
            //Debug.Log($"Round End: {newRound}.");
        }

        public List<ParticleSystem> GetParticles(TargetList targetList)
        {
            var list = new List<ParticleSystem>();
            ParticleSystem particle;
            foreach (var item in targetList)
            {
                particle = Particle.Create(ParticleType.Explode, item.Area.transform);
                list.Add(particle);
            }
            return list;
        }
    }

}