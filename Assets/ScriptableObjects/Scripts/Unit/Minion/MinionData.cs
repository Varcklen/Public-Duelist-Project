using Project.Singleton.PrefabLoaderNS;
using UnityEngine;

namespace Project.UnitNS.DataNS
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Unit/Minion", fileName = "New Minion")]
    public sealed class MinionData : UnitData
    {
        public override GameObject unitPrefab => PrefabLoader.Instance.MinionPrefab;
    }
}