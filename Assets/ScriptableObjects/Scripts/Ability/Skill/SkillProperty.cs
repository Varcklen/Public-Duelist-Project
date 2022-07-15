using UnityEngine;
using System;
using Project.Abilities.Interfaces;
using Project.UnitNS;

namespace Project.Abilities.Data
{
	[CreateAssetMenu(fileName = "New SkillProperty", menuName = "Scriptable Objects/Ability/Skill/SkillProperty")]
	public class SkillProperty : SkillData
	{
		[SerializeField] private SkillPropertyInfo _testPropertyInfo;
		public override PropertyInfo Property { get => _testPropertyInfo; }

        public override Type PropertyType => typeof(SkillPropertyComponent);
    }
}

namespace Project.Abilities
{
	[Serializable]
	public class SkillPropertyInfo : PropertyInfo
	{
		public int SomeInt;
	}

    public class SkillPropertyComponent : SkillComponent<SkillPropertyInfo>, IAbilityUse
    {
        public ParticleSystem GetParticle(Unit targetResult)
        {
            return null;
        }

        public void Use(Unit targetResult)
        {
            Caster.UnitResourceManagment.Restore(Caster, Stats.ResourceType.Mana, 2);
        }
    }

}
