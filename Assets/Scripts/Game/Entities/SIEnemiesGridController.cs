﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [System.Serializable]
    public class SIShootBehaviourSetup
    {
        public int enemyIndex;
        public int enemyRow;
        public int enemyColumn;
        public SIEnemyBehaviour frontNeighbour;
        public SIEnemyBehaviour backNeighbour;
        public SIEnemyBehaviour leftNeighbour;
        public SIEnemyBehaviour rightNeighbour;
        public List<SIEnemyBehaviour> neighbours;
    }

    public class SIEnemiesGridController : MonoBehaviour
    {
        [SerializeField] GridControllerSetup _gridSetup;
        [SerializeField] SIEnemyBehaviour[] _availableEnemies;

        int _maxEnemies;
        int _maxInRow;
        int _maxInColumn;
        int _maxNeighboursOfEnemyCount;
        int _livingEnemies;
        int _gridSpeedTiers;
        int _minEnemiesToUpdateGridSpeed;
        GridControllerSettings _gridSettings;

        void Awake()
        {
            PreInitialise();
        }

        void PreInitialise()
        {
            LoadSetup();
            SetupEnemies();
        }

        void SetupEnemies()
        {
            SIEnemyBehaviour currentShootBehaviour;
            SIShootBehaviourSetup currentSetup;
            int currentRow, currentColumn;
            int leftNbIndex, rightNbIndex, frontNbIndex, backNbIndex; 
            for (int i = 0; i < _maxEnemies; i++)
            {
                currentRow = i / _gridSettings.maxEnemiesInGridRow;
                currentColumn = i / _gridSettings.maxEnemiesInGridColumn;
                currentShootBehaviour = _availableEnemies[i];
                backNbIndex = i - _maxInRow;
                frontNbIndex = i + _maxInRow;
                leftNbIndex = i - 1;
                rightNbIndex = i + 1;
                currentSetup = new SIShootBehaviourSetup
                {
                    enemyIndex = i,
                    enemyRow = currentRow,
                    enemyColumn = currentColumn,
                    backNeighbour = IsInMinMaxRange(backNbIndex) ? _availableEnemies[backNbIndex] : null,
                    frontNeighbour = IsInMinMaxRange(frontNbIndex) ? _availableEnemies[frontNbIndex] : null,
                    leftNeighbour = IsInRowHorizontalRange(leftNbIndex, currentRow) ? _availableEnemies[leftNbIndex] : null,
                    rightNeighbour = IsInRowHorizontalRange(rightNbIndex, currentRow) ? _availableEnemies[rightNbIndex] : null,
//                    neighbours = GetNeighbours(i, currentRow)
                };
                currentShootBehaviour.UpdateShootBehaviourSetup(currentSetup);
            }
        }
//
//        List<SIEnemyBehaviour> GetNeighbours(int enemyIndex, int enemyRow)
//        {
//            int currentNeighbourIndex;
//            int halfNeighbours = (int) (_maxNeighboursOfEnemyCount * 0.5f);
//            int[] neightboursIndexes = new int[_maxNeighboursOfEnemyCount];
//            List<SIEnemyBehaviour> neighbours = new List<SIEnemyBehaviour>();
//
//            neightboursIndexes[0] = enemyIndex - _maxInRow;
//            neightboursIndexes[1] = enemyIndex + _maxInRow;
//            neightboursIndexes[2] = enemyIndex - 1;
//            neightboursIndexes[3] = enemyIndex + 1;
//
//            
//            //NOTE: First half of neighbours are horizontal, the rest vertical.
//            for (int i = 0; i < _maxNeighboursOfEnemyCount; i++)
//            {
//                currentNeighbourIndex = neightboursIndexes[i];
//                if(IsInRowHorizontalRange(currentNeighbourIndex, enemyRow) && i >= halfNeighbours)
//                    neighbours.Add(_availableEnemies[currentNeighbourIndex]);
//                if(IsInMinMaxRange(currentNeighbourIndex) && i >= 0 && i < halfNeighbours)
//                    neighbours.Add(_availableEnemies[currentNeighbourIndex]);
//            }
//            
//            return neighbours;
//        }

        bool IsInRowHorizontalRange(int currentNeighbourIndex, int enemyRow)
        {
            int minHorizontal = enemyRow * _maxInRow;
            int maxHorizontal = minHorizontal + _maxInRow;
            return currentNeighbourIndex >= minHorizontal && currentNeighbourIndex < maxHorizontal;
        }

        bool IsInMinMaxRange(int currentNeighbourIndex)
        {
            return currentNeighbourIndex >= 0 && currentNeighbourIndex < _maxEnemies;
        }

        void LoadSetup()
        {
            _gridSettings = _gridSetup.gridControllerSettings;
            _maxEnemies = _gridSettings.maxEnemiesInGrid;
            _livingEnemies = _maxEnemies;
            _maxNeighboursOfEnemyCount = _gridSettings.maxNeighboursOfEnemyCount;
            _maxInColumn = _gridSettings.maxEnemiesInGridColumn;
            _maxInRow = _gridSettings.maxEnemiesInGridRow;
            _gridSpeedTiers = _gridSettings.enemiesLeftToUpdateGridMovementTier.Length;
            _minEnemiesToUpdateGridSpeed = _gridSettings.enemiesLeftToUpdateGridMovementTier[0];
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            SIEventsHandler.OnGameStarted += HandleOnGameStarted;
            SIEventsHandler.OnEnemyDeath += HandleOnEnemyDeath;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnGameStarted -= HandleOnGameStarted;
            SIEventsHandler.OnEnemyDeath -= HandleOnEnemyDeath;
        }

        void HandleOnEnemyDeath(MonoBehaviour deadEnemy)
        {
            --_livingEnemies;
            TryToBroadcastNewMovementSpeedTier();
            TryToFinalizeWave();
        }

        void TryToFinalizeWave()
        {
            if (_livingEnemies > 0)
                return;

            StartCoroutine(FinalizeAndRestartWaveRoutine());
        }

        void HandleOnGameStarted()
        {
            MoveEnemiesGrid();
        }

        void TryToBroadcastNewMovementSpeedTier()
        {
            if (_livingEnemies > _minEnemiesToUpdateGridSpeed)
                return;

            for (int i = 0; i < _gridSpeedTiers; i++)
            {
                if (_livingEnemies != _gridSettings.enemiesLeftToUpdateGridMovementTier[i])
                    continue;

                SIEnemyGridEvents.BroadcastOnUpdateGridMovementSpeedTier(i);
                return;
            }
        }

        IEnumerator FinalizeAndRestartWaveRoutine()
        {
            SIEnemyGridEvents.BroadcastOnGridReset();
            yield return StartCoroutine(SIWaitUtils.WaitAndInvoke(_gridSettings.endWaveCooldown,
                SIEventsHandler.BroadcastOnWaveEnd));
            SetLivingEnemiesCount();
            yield return StartCoroutine(SIWaitUtils.WaitAndInvoke(_gridSettings.newWaveCooldown, MoveEnemiesGrid));
        }

        void SetLivingEnemiesCount()
        {
            //todo: temporary
            _livingEnemies = _maxEnemies;
        }

        void MoveEnemiesGrid()
        {
            SIEnemyGridEvents.BroadcastOnGridStarted();
        }
    }
}