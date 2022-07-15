using UnityEngine;
using Project.Singleton.ScriptableObjects;
using System.Collections.Generic;
using Project.Abilities.Data;
using Project.AI;

namespace Project.Singleton.ConfigurationAbilitiesNS
{
	[CreateAssetMenu(fileName ="ConfigurationAbilities", menuName = "Scriptable Objects/Singleton/ConfigurationAbilities")]
	public class ConfigurationAbilities : SingletonScriptableObject<ConfigurationAbilities>
	{
		[SerializeField] private int _activeAbilitiesLimit;
		public int ActiveAbilitiesLimit => _activeAbilitiesLimit;
		[SerializeField] private int _passiveAbilitiesLimit;
		public int PassiveAbilitiesLimit => _activeAbilitiesLimit;

		[SerializeField] private List<SkillData> _baseSkills;
		public List<SkillData> BaseSkills => _baseSkills;

		[SerializeField] private List<Consideration> _baseConsiderations;
		public List<Consideration> BaseConsiderations => _baseConsiderations;
	}
}