using UnityEngine;
using System;
using Project.Abilities.Interfaces;
using Project.UnitNS;
using System.Threading.Tasks;
using Project.BattlefieldNS;

namespace Project.Abilities.Data
{
	[CreateAssetMenu(fileName="New AreaTargetTest", menuName="Scriptable Objects/Ability/ActiveAbility/AreaTargetTest")]
	public class AreaTargetTest : ActiveAbilityData
	{
		[SerializeField] private AreaTargetTestInfo _AreaTargetTestInfo;
		public override PropertyInfo Property { get => _AreaTargetTestInfo; }
        public override Type PropertyType => typeof(AreaTargetTestComponent);
    }
}

namespace Project.Abilities
{
    [Serializable]
	public class AreaTargetTestInfo : PropertyInfo
	{
		
    }

	public class AreaTargetTestComponent : ActiveAbilityComponent<AreaTargetTestInfo>, IAbilityUseArea
	{
        protected override Func<Area, bool> GetTargetCondition()
        {
            return x => x.PlacedUnit == null;
        }

        public ParticleSystem GetParticle(Area targetResult)
        {
            return null;
        }

        public void Use(Area targetResult)
        {
            Debug.Log($"Choosed area: {targetResult}");
        }
    }
}