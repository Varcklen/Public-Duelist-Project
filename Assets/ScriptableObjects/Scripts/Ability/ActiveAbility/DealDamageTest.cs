using UnityEngine;
using System;
using Project.Abilities.Interfaces;
using Project.UnitNS;
using System.Threading.Tasks;

namespace Project.Abilities.Data
{
	[CreateAssetMenu(fileName="New DealDamageTest", menuName="Scriptable Objects/Ability/ActiveAbility/DealDamageTest")]
	public class DealDamageTest : ActiveAbilityData
	{
		[SerializeField] private DealDamageTestInfo _DealDamageTestInfo;
		public override PropertyInfo Property { get => _DealDamageTestInfo; }
        public override Type PropertyType => typeof(DealDamageTestComponent);
    }
}

namespace Project.Abilities
{
    [Serializable]
	public class DealDamageTestInfo : PropertyInfo
	{
		
    }

	public class DealDamageTestComponent : ActiveAbilityComponent<DealDamageTestInfo>, IAbilityUse
	{
        public ParticleSystem GetParticle(Unit targetResult)
        {
            return null;
        }

        public void Use(Unit targetResult)
        {
            targetResult.UnitResourceManagment.TakeDamage(Caster, Mathf.CeilToInt(targetResult.UnitStats.UnitInfo.Health.MaxValue * 0.6f));
        }
    }
}