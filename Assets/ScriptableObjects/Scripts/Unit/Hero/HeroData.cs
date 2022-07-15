using UnityEngine;
using Project.Singleton.PrefabLoaderNS;

namespace Project.UnitNS.DataNS
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Unit/Hero", fileName = "New Hero")]
    public sealed class HeroData : UnitData
    {
        public override GameObject unitPrefab => PrefabLoader.Instance.HeroPrefab;
    }
}