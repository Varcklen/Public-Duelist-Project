using UnityEngine;
using Project.Singleton.ScriptableObjects;
using Project.UnitNS.DataNS;
using Project.Abilities.Data;

namespace Project.Singleton.TestDataLoaderNS
{
	[CreateAssetMenu(fileName ="TestDataLoader", menuName = "Scriptable Objects/Singleton/TestDataLoader")]
	public class TestDataLoader : SingletonScriptableObject<TestDataLoader>
	{
		[SerializeField] private HeroData _heroData;
		public HeroData HeroData => _heroData;

		[SerializeField] private ActiveAbilityData _activeAbilityData;
		public ActiveAbilityData ActiveAbilityData =>_activeAbilityData;
	}
}