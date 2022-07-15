using Project.Abilities.Interfaces;
using Project.Utils.BaseInfoNS;
using UnityEngine;

namespace Project.Abilities
{
	/// <summary>
	/// It is the base class for classes that store unique ability information.
	/// </summary>
	public abstract class PropertyInfo : BaseInfo { }

	//ActiveAbility
	public abstract class ActiveAbilityComponent<T> : ActiveAbility, IAbilityComponent where T : PropertyInfo
	{
		[SerializeField] private T _info;
        PropertyInfo IAbilityComponent._info { get => _info; set => _info = (T)value; }
		public T Info => _info;
	}

	//Passive Ability
	public abstract class PassiveAbilityComponent<T> : PassiveAbility, IAbilityComponent where T : PropertyInfo
	{
		[SerializeField] private T _info;
		PropertyInfo IAbilityComponent._info { get => _info; set => _info = (T)value; }
		public T Info => _info;
	}

	//Skill
	public abstract class SkillComponent<T> : Skill, IAbilityComponent where T : PropertyInfo
	{
		[SerializeField] private T _info;
		PropertyInfo IAbilityComponent._info { get => _info; set => _info = (T)value; }
		public T Info => _info;

	}
}
