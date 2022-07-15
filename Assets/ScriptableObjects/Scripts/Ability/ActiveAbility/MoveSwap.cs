using UnityEngine;
using System;
using Project.Abilities.Interfaces;
using Project.UnitNS;
using System.Threading.Tasks;
using Project.BattlefieldNS;

namespace Project.Abilities.Data
{
	[CreateAssetMenu(fileName="New MoveSwap", menuName="Scriptable Objects/Ability/ActiveAbility/MoveSwap")]
	public class MoveSwap : ActiveAbilityData
	{
		[SerializeField] private MoveSwapInfo _MoveSwapInfo;
		public override PropertyInfo Property { get => _MoveSwapInfo; }
        public override Type PropertyType => typeof(MoveSwapComponent);
    }
}

namespace Project.Abilities
{
    [Serializable]
	public class MoveSwapInfo : PropertyInfo
	{
		
    }

	public class MoveSwapComponent : ActiveAbilityComponent<MoveSwapInfo>, IAbilityUseArea
	{
        public ParticleSystem GetParticle(Area targetResult)
        {
            return null;
        }

        public void Use(Area targetResult)
        {
            Caster.UnitMovement.MoveOrSwap(targetResult);
        }
    }
}