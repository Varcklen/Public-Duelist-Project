using UnityEngine;
using Project.Managers.StartBattle;
using Project.BetweenScenes;

public class TEST_Start : MonoBehaviour
{
    [SerializeField] private EnemyBundle testEnemies;

    void Start()
    {
        StartBattleManager.Create(Limbo.Instance.AllyUnits, testEnemies);
    }
 
}
