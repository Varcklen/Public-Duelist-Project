using System;
using UnityEditor;
using UnityEngine;
using Project.BattlefieldNS;
using Project.UnitNS;
using Project.Systems.DeathSystemNS;
using Project.Singleton.TestDataLoaderNS;
using Project.Abilities.Data;
using Project.Abilities.Interfaces;
using Project.Utils.Extension.ObjectNS;
using Project.Abilities;
using System.Collections.Generic;
using Project.Particles;
using System.Threading.Tasks;

namespace Project.Tests.MenuItemNS
{
    /// <summary>
    /// Contains tests for actions on the battlefield.
    /// </summary>
    internal sealed class TestBattlefield : TestMenuItem
    {
        private const string mainPath = "Test/Battlefied/";

        private static Battlefield _battlefield;

        #region Test Helpers
        private static void BattlefieldCheck()
        {
            PlayingCheck();
            _battlefield = Battlefield.Instance;
            if (_battlefield == null)
            {
                Debug.LogWarning("You must use it at Battle scene.");
                return;
            }
        }

        private static void SetFirstUnit(SideType sideType, ref Unit unit)
        {
            if (unit == null)
            {
                Side side = _battlefield.GetSide(sideType);
                if (side.IsEmpty())
                {
                    throw new ArgumentNullException($"{sideType} is empty.");
                }
                unit = side.GetFirstUnit();
            }
            if (unit.GetComponent<UnitDeath>().IsDead)
            {
                throw new Exception($"{unit.name} is dead.");
            }
        }

        private static void SetNumberedUnit(SideType sideType, ref Unit unit, int position)
        {
            if (unit == null)
            {
                Side side = _battlefield.GetSide(sideType);
                if (side.IsEmpty())
                {
                    throw new ArgumentNullException($"{sideType} is empty.");
                }
                unit = side.GetUnitArea(position).PlacedUnit;
                if (unit == null)
                {
                    throw new ArgumentNullException($"There is no unit in {sideType} position №{position}.");
                }
            }
            if (unit.GetComponent<UnitDeath>().IsDead)
            {
                throw new Exception($"{unit.name} is dead.");
            }
        }

        private static void SetNumberedArea(SideType sideType, ref Area unitArea, int position)
        {
            if (unitArea == null)
            {
                unitArea = _battlefield.GetSide(sideType).GetUnitArea(position);
            }
        }

        private static void SetUnitFromGraveyard(SideType sideType, ref Unit unit)
        {
            if (unit == null)
            {
                Graveyard graveyard = Graveyard.Instance;
                if (graveyard.IsEmpty(sideType))
                {
                    throw new ArgumentNullException($"{sideType} graveyard is empty.");
                }
                unit = graveyard.GetRandomUnit(sideType);
            }
        }
        #endregion

        //Info
        private const string infoPath = "Info/";
        #region Show Ally Side Unit Count
        [MenuItem(mainPath + infoPath + "/Show ally side unit count")]
        private static void ShowAllySideUnitCount()
        {
            BattlefieldCheck();
            Debug.Log($"Units: {_battlefield.AllySide.GetUnitCount()}");
        }
        #endregion

        //Battle
        private const string battlePath = "Battle/";
        #region Attack Enemy
        private static Unit unitTarget;
        private static Unit unitDealer;
        [MenuItem(mainPath + battlePath + "First ally attack first enemy")]
        private static void AttackEnemy()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Enemy, ref unitTarget);
            SetFirstUnit(SideType.Ally, ref unitDealer);
            unitTarget.GetComponent<UnitResourceManagment>().TakeDamage(unitDealer, 5);
        }
        #endregion

        #region Attack Himself
        private static Unit unitPainer;
        [MenuItem(mainPath + battlePath + "Deal 1 damage to himself (first ally)")]
        private static void DealHimself()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitPainer);
            unitPainer.GetComponent<UnitResourceManagment>().TakeDamage(unitPainer, 1);
        }
        #endregion

        #region Set Resources
        private static Unit unitToSet;
        [MenuItem(mainPath + battlePath + "First ally set resources to 50%.")]
        private static void SetResource()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitToSet);
            unitToSet.UnitStats.UnitInfo.Health.SetPercentResource(50);
            unitToSet.UnitStats.UnitInfo.Mana.SetPercentResource(50);
        }
        #endregion

        #region Set 0
        private static Unit unitToSetZero;
        [MenuItem(mainPath + battlePath + "First Enemy set health to 0%.")]
        private static void SetResourceZero()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Enemy, ref unitToSetZero);
            unitToSetZero.UnitStats.UnitInfo.Health.SetPercentResource(0);
        }
        #endregion

        #region Restore Resources
        private static Unit unitToRestore;
        [MenuItem(mainPath + battlePath + "First ally restore health and mana by 5.")]
        private static void RestoreResources()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitToRestore);
            unitToRestore.UnitResourceManagment.Restore(null, Stats.ResourceType.Health, 5);
            unitToRestore.UnitResourceManagment.Restore(null, Stats.ResourceType.Mana, 5);
        }
        #endregion

        //Movement
        //Turned off, because not working correct in this stage of development
        //private const string movementPath = "Movement/";
        #region Move Unit
        /*private static Unit unitToMove;
        private static UnitArea area;
        [MenuItem(mainPath + movementPath + "Move first ally to ally area №6")]
        private static void MoveUnit()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitToMove);
            SetNumberedArea(SideType.Ally, ref area, 6);
            unitToMove.GetComponent<UnitMovement>().MoveTo(area);
        }*/
        #endregion

        #region Swap Units
        /*private static Unit unitFirst;
        private static Unit unitSecond;
        [MenuItem(mainPath + movementPath + "Swap ally №1 with ally №2")]
        private static void SwapUnits()
        {
            BattlefieldCheck();
            SetNumberedUnit(SideType.Ally, ref unitFirst, 1);
            SetNumberedUnit(SideType.Ally, ref unitSecond, 2);
            unitFirst.GetComponent<UnitMovement>().SwapWith(unitSecond);
        }*/
        #endregion

        #region Become Enemy
        /*private static Unit unitToBecome;
        private static UnitArea areaToMove;
        [MenuItem(mainPath + movementPath + "Move first ally to enemy area №6")]
        private static void BecomeEnemy()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitToBecome);
            SetNumberedArea(SideType.Enemy, ref areaToMove, 6);
            unitToBecome.GetComponent<UnitMovement>().MoveTo(areaToMove);
        }*/
        #endregion

        //Graveyard
        private const string graveyardPath = "Graveyard/";
        #region Ressurect Ally
        private static Unit unitToRessurect;
        [MenuItem(mainPath + graveyardPath + "Ressurect random ally")]
        private static void Ressurect()
        {
            BattlefieldCheck();
            SetUnitFromGraveyard(SideType.Ally, ref unitToRessurect);
            unitToRessurect.GetComponent<UnitDeath>().Ressurect(SideType.Ally);
        }
        #endregion

        #region Kill Ally
        private static Unit unitToKill;
        [MenuItem(mainPath + graveyardPath + "Kill first ally")]
        private static void Kill()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitToKill);
            unitToKill.GetComponent<UnitDeath>().Kill();
        }
        #endregion

        #region Kill Allies
        [MenuItem(mainPath + graveyardPath + "Kill Allies")]
        private static void KillAllies()
        {
            BattlefieldCheck();
            var list = Battlefield.Instance.GetSide(SideType.Ally).GetAllUnits();
            foreach (var unit in list)
            {
                unit.GetComponent<UnitDeath>().Kill();
            }
        }
        #endregion

        #region Kill Enemies
        [MenuItem(mainPath + graveyardPath + "Kill Enemies")]
        private static void KillEnemies()
        {
            BattlefieldCheck();
            var list = Battlefield.Instance.GetSide(SideType.Enemy).GetAllUnits();
            foreach (var unit in list)
            {
                unit.GetComponent<UnitDeath>().Kill();
            }
        }
        #endregion

        //Turn
        private const string selectionPath = "Turn/";
        private static IBattleStagesSkipUnit SkipUnit = BattlefieldStages.Instance;
        #region Select Ally
        /*private static Unit unitToSelect;
        [MenuItem(mainPath + selectionPath + "Select first ally")]
        private static void Select()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitToSelect);
            unitToSelect.GetComponent<UnitSelection>().SelectUnit();
        }*/
        #endregion

        #region DeSelect Ally
        /*private static Unit unitToDeSelect;
        [MenuItem(mainPath + selectionPath + "Deselect first ally")]
        private static void DeSelect()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitToDeSelect);
            unitToDeSelect.GetComponent<UnitSelection>().DeselectUnit();
        }*/
        #endregion

        #region Next Turn
        [MenuItem(mainPath + selectionPath + "Next Turn")]
        private static void NextTurn()
        {
            BattlefieldCheck();
            SkipUnit.SkipUnit();
        }
        #endregion

        //Turned off
        #region Next Turn x3
        [MenuItem(mainPath + selectionPath + "Next Turn x3")]
        private static void NextTurn3()
        {
            BattlefieldCheck();
            SkipUnit.SkipUnit(3);
        }
        #endregion

        #region Refresh Round
        [MenuItem(mainPath + selectionPath + "Refresh Round")]
        private static void RefreshRound()
        {
            BattlefieldCheck();
            _ = Battlefield.Instance.BattleStages.RefreshRound();
        }
        #endregion

        //Create
        private const string createPath = "Create/";
        #region Create Ally
        [MenuItem(mainPath + createPath + "Create ally hero")]
        private static void Create()
        {
            BattlefieldCheck();
            Unit.Create<Hero>(TestDataLoader.Instance.HeroData, SideType.Ally);
        }
        #endregion

        #region Create Particle
        private static Unit unitToGetParticle;
        [MenuItem(mainPath + createPath + "Create particle on first ally")]
        private static void CreateParticle()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitToGetParticle);
            Particle.Create(ParticleType.Explode, unitToGetParticle.transform.position);
        }
        #endregion

        //Buff
        private const string buffPath = "Buff/";
        #region Increase Attack and Speed
        private static Unit unitToBuff;
        [MenuItem(mainPath + buffPath + "Increase attack, speed and health of first ally by 8.")]
        private static void AttackSpeedBuff()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitToBuff);
            unitToBuff.GetComponent<UnitStats>().UnitInfo.Attack.MaxValue += 8;
            unitToBuff.GetComponent<UnitStats>().UnitInfo.Speed.MaxValue += 8;
            unitToBuff.GetComponent<UnitStats>().UnitInfo.Health.MaxValue += 8;
        }
        #endregion

        #region Decrease Attack and Speed
        private static Unit unitToDebuff;
        [MenuItem(mainPath + buffPath + "Decrease attack, speed and health of first ally by 4.")]
        private static void AttackSpeedDeBuff()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitToDebuff);
            unitToDebuff.GetComponent<UnitStats>().UnitInfo.Attack.MaxValue -= 4;
            unitToDebuff.GetComponent<UnitStats>().UnitInfo.Speed.MaxValue -= 4;
            unitToDebuff.GetComponent<UnitStats>().UnitInfo.Health.MaxValue -= 4;
        }
        #endregion

        #region Increase Speed on random enemy
        [MenuItem(mainPath + buffPath + "Increase Speed on random enemy by 2.")]
        private static void RandomSpeedBuff()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitToBuff);
            var randomEnemy = Battlefield.Instance.GetSide(SideType.Enemy).GetRandomUnit();
            if (randomEnemy == null)
            {
                Debug.LogWarning("There are no alive enemies found.");
                return;
            }
            randomEnemy.GetComponent<UnitStats>().UnitInfo.Speed.MaxValue += 2;
        }
        #endregion

        //Ability
        private const string abilityPath = "Ability/";
        #region ReplaceAbility
        private static Unit unitToReplaceAbility;
        [MenuItem(mainPath + abilityPath + "Replace first ally first active ability")]
        private static void ReplaceAbility()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitToReplaceAbility);
            unitToReplaceAbility.GetComponent<UnitAbilities>().ReplaceActiveAbility(TestDataLoader.Instance.ActiveAbilityData, 0);
        }
        #endregion

        #region ClickAbility
        private static Unit unitToClickAbility;
        [MenuItem(mainPath + abilityPath + "Click first active ability on first ally")]
        private static void ClickAbility()
        {
            BattlefieldCheck();
            SetFirstUnit(SideType.Ally, ref unitToClickAbility);
            var ability = unitToClickAbility.GetComponent<UnitAbilities>().ActiveAbilities[0].IsNullException();
            ((IAbilityUse)ability).Use(unitToClickAbility);
        }
        #endregion
    }
}
