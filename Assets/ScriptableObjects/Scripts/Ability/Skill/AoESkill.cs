using UnityEngine;
using System;
using Project.Abilities.Interfaces;
using Project.UnitNS;
using System.Threading.Tasks;
using System.Collections.Generic;
using Project.Particles;

namespace Project.Abilities.Data
{
	[CreateAssetMenu(fileName="New AoESkill", menuName="Scriptable Objects/Ability/Skill/AoESkill")]
	public class AoESkill : SkillData
	{
		[SerializeField] private AoESkillInfo _AoESkillInfo;
		public override PropertyInfo Property { get => _AoESkillInfo; }
        public override Type PropertyType => typeof(AoESkillComponent);
    }
}

namespace Project.Abilities
{
    [Serializable]
	public class AoESkillInfo : PropertyInfo
	{
        public string Something = "Something";
    }

	public class AoESkillComponent : SkillComponent<AoESkillInfo>, IAbilityUseTargets
	{
        public List<ParticleSystem> GetParticles(TargetList targetList)
        {
            return null;
        }

        public void Use(TargetList targetResult)
        {
            //Debug.Log($"ability: {AbilityInfo.Name}");
            /*Debug.Log($"Used! {GetType()}");
            Debug.Log($"_unit: {Caster}, targets:");
            foreach (var target in targetResult)
            {
                Debug.Log(target);
            }*/
        }
    }
}