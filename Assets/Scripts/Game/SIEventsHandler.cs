﻿using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEventsHandler
    {
        public static event Action OnGameStarted = delegate { };
        public static event Action OnGameFinished = delegate { };

        public static event Action OnSpawnObject = delegate { };
        public static event Action OnGameQuit = delegate { };
        public static event Action OnEnemiesRespawn = delegate { };

        public static event Action OnNonPlayableUpdate = delegate {}; 
        public static event Action OnUpdate = delegate { };
        public static event Action OnShadersUpdate = delegate { };
        public static event Action OnEnemyDeath = delegate { };
        public static event Action OnDebugInputHandling = delegate { };
        public static event Action OnWaveEnd = delegate { };
        public static event Action<float> OnEnemySpeedMultiplierChanged = delegate { };

        public static event Action<Vector3> OnInputCollected = delegate { };

        public static event Action<int> OnShootingEnemiesUpdate = delegate { };
        public static event Action<SIBonusInfo> OnBonusCollision = delegate { };

        public static void BroadcastOnInputCollected(Vector3 inputVector)
        {
            OnInputCollected?.Invoke(inputVector);
        }

        public static void BroadcastOnShootingEnemiesUpdate(int index)
        {
            OnShootingEnemiesUpdate?.Invoke(index);
        }

        public static void BroadcastOnSpawnObject()
        {
            OnSpawnObject?.Invoke();
        }

        public static void BroadcastOnGameStarted()
        {
            OnGameStarted?.Invoke();
        }

        public static void BroadcastOnGameFinished()
        {
            OnGameFinished?.Invoke();
        }

        public static void BroadcastOnNonPlayableUpdate()
        {
            OnNonPlayableUpdate?.Invoke();
        }

        public static void BroadcastOnGameQuit()
        {
            OnGameQuit?.Invoke();
        }

        public static void BroadcastOnEnemiesRespawn()
        {
            OnEnemiesRespawn?.Invoke();
        }
        
        public static void BroadcastOnUpdate()
        {
            OnUpdate?.Invoke();
        }
        
        public static void BroadcastOnShadersUpdate()
        {
            OnShadersUpdate?.Invoke();
        }
        
        public static void BroadcastOnEnemyDeath()
        {
            OnEnemyDeath?.Invoke();
        }
        
        public static void BroadcastOnDebugInputHandling()
        {
            OnDebugInputHandling?.Invoke();
        }
        
        public static void BroadcastOnWaveEnd()
        {
            OnWaveEnd?.Invoke();
        }

        public static void BroadcastOnEnemySpeedMultiplierChanged(float multiplier)
        {
            OnEnemySpeedMultiplierChanged?.Invoke(multiplier);
        }
        
        public static void BroadcastOnBonusCollision(SIBonusInfo bonusInfo)
        {
            OnBonusCollision?.Invoke(bonusInfo);
        }
        
    }
}   