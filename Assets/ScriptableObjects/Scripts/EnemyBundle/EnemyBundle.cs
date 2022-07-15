using Project.UnitNS.DataNS;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Bundle/EnemyBundle", fileName = "New Enemy Bundle")]
public sealed class EnemyBundle : ScriptableObject
{
    [SerializeField] private List<UnitData> _bundle;
    public List<UnitData> Bundle => _bundle;
}
