using UnityEngine;
using System;

namespace Project.Abilities.Data
{
	[CreateAssetMenu(fileName = "New PassiveProperty", menuName = "Scriptable Objects/Ability/PassiveAbility/PassiveProperty")]
	public class PassiveProperty : PassiveAbilityData
	{
		[SerializeField] private PassivePropertyInfo _testPropertyInfo;
		public override PropertyInfo Property { get => _testPropertyInfo; }

		public override Type PropertyType => typeof(PassivePropertyComponent);
	}
}

namespace Project.Abilities
{
	[Serializable]
	public class PassivePropertyInfo : PropertyInfo
	{
		public float SomeFloat;
	}

	public class PassivePropertyComponent : PassiveAbilityComponent<PassivePropertyInfo>
	{

	}

}
