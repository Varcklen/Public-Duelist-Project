using Project.Singleton.ScriptableObjects;
using UnityEngine;

namespace Project.Singleton.PrefabLoaderNS
{
    /// <summary>
    /// Stores information about prefabs for game objects.
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Singleton/PrefabLoader", fileName = "PrefabLoader")]
    public sealed class PrefabLoader : SingletonScriptableObject<PrefabLoader>
    {
        [SerializeField] private GameObject _heroPrefab;
        public GameObject HeroPrefab => _heroPrefab;

        [SerializeField] private GameObject _minionPrefab;
        public GameObject MinionPrefab => _minionPrefab;

        [SerializeField] private GameObject _startManagerPrefab;
        public GameObject StartManagerPrefab => _startManagerPrefab;
    }
}


