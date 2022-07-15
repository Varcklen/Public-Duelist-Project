using UnityEngine;
using System;
using Project.Abilities.Interfaces;
using Project.UnitNS;
using Project.BattlefieldNS;
using Project.Particles;

namespace Project.Abilities.Data
{
	[CreateAssetMenu(fileName="New TestProperty", menuName="Scriptable Objects/Ability/ActiveAbility/TestProperty")]
	public class TestProperty : ActiveAbilityData
	{
		[SerializeField] private TestPropertyInfo _testPropertyInfo;
		public override PropertyInfo Property { get => _testPropertyInfo; }
        public override Type PropertyType => typeof(TestPropertyComponent);
    }
}

namespace Project.Abilities
{
    [Serializable]
	public class TestPropertyInfo : PropertyInfo
	{
		public string SomeName;
		public int MinAttack;
    }

	public class TestPropertyComponent : ActiveAbilityComponent<TestPropertyInfo>, IAbilityUse
	{
        protected override Func<Area, bool> GetTargetCondition()
        {
			return x => x.PlacedUnit.UnitStats.UnitInfo.Attack.MaxValue >= Info.MinAttack;
        }

        public void Use(Unit targetResult)
        {
			Debug.Log($"Ability used: {AbilityInfo.Name}");
			targetResult.UnitResourceManagment.TakeDamage(Caster, 1);
			//Debug.Log($"Used! {GetType()}");
			//Debug.Log($"_unit: {Caster}, target: {targetResult}");
			//AbilityInfo.Charge.CurrentCharge++;

			/*var units = Battlefield.Instance.GetAllUnits();

			var rand = new System.Random();
            foreach (var unit in units)
            {
				unit.UnitStats.UnitInfo.Speed.MaxValue = rand.Next(-16, 16);
            }*/

			//var unit = Battlefield.Instance.GetSide(SideType.Enemy).GetRandomUnit();
			//unit.UnitStats.UnitInfo.Speed.MaxValue++;

		}

        public ParticleSystem GetParticle(Unit targetResult)
        {
			return Particle.Create(ParticleType.Explode, targetResult.Area.transform);
		}
	}

}