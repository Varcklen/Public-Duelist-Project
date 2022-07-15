using UnityEngine;
using Project.Singleton.ScriptableObjects;

namespace Project.Singleton.ConfigurationNS
{
	[CreateAssetMenu(fileName ="Configuration", menuName = "Scriptable Objects/Singleton/Configuration/Configuration")]
	public class Configuration : SingletonScriptableObject<Configuration>
	{
        [Header("Input")]
		[SerializeField] private string _targetCircleTag;
		public string TargetCircleTag => _targetCircleTag;

        [Header("Battlefield")]
        [SerializeField] private int _allySlotsCount;
		public int AllySlotsCount => _allySlotsCount;
        [SerializeField] private int _enemySlotsCount;
		public int EnemySlotsCount => _enemySlotsCount;
		public int AllSlotsCount => _allySlotsCount + _enemySlotsCount;
	}
}