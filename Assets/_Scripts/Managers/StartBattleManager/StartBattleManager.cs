using System.Collections.Generic;
using UnityEngine;
using Project.UnitNS;
using Project.UnitNS.Interfaces;
using Project.BattlefieldNS;
using System;
using Project.Singleton.PrefabLoaderNS;
using Project.UnitNS.DataNS;
using Project.UI.BattlefieldNS;

namespace Project.Managers.StartBattle
{
    /// <summary>
    /// The component launches a battle on the battlefield.
    /// </summary>
    public class StartBattleManager : MonoBehaviour
    {
        private List<Unit> _allyUnits;
        private List<UnitData> _enemyUnitsData;

        private void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            if (_allyUnits == null || _enemyUnitsData == null)
            {
                throw new ArgumentNullException($"_allyUnits ({_allyUnits == null}) and _enemyUnitsData ({_enemyUnitsData == null}) cannot be null.");
            }
            foreach (var unit in _allyUnits)
            {
                ((IOnUnitCreateForLimbo)unit).OnUnitCreateForLimbo();
            }
            foreach (var enemyData in _enemyUnitsData)
            {
                Unit.Create<Unit>(enemyData, SideType.Enemy);
            }
            await BattlefieldUI.Instance.ShowUIElementsAsync();
            Battlefield.Instance.BattleStages.StartBattle();
            Destroy(gameObject);
        }

        #region Create
        public static void Create(List<Unit> allyUnits, List<UnitData> enemyUnitsData)
        {
            if (Battlefield.Instance.BattleStages.BattleStage != BattleStage.Preparation)
            {
                Debug.LogWarning("You trying create StartManager, but preparation stage already ended.");
                return;
            }
            StartBattleManager startManager = Instantiate(PrefabLoader.Instance.StartManagerPrefab).GetComponent<StartBattleManager>();
            startManager._allyUnits = allyUnits;
            startManager._enemyUnitsData = enemyUnitsData;
        }

        public static void Create(List<Unit> allyUnits, EnemyBundle bundle)
        {
            Create(allyUnits, bundle.Bundle);
        }
        #endregion
    }
}

