﻿using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] EntitySetup _entitySetup;
        [SerializeField] MeshRenderer _meshRenderer;
        [SerializeField] GameObject _colliderParent;

        EntitySettings _entitySettings;
        SIEnemyStatistics _enemyEntityStatistics;
        
//        [SerializeField] private SIVFXManager _destroyVFX;
//        [SerializeField] SIBonusSelectorSystem bonusSelectorSystem;
//        [SerializeField] SIWeaponEntity weaponEntity;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            _entitySettings = _entitySetup.entitySettings;
            _enemyEntityStatistics = new SIEnemyStatistics
            {
                isAlive = true, currentHealth = _entitySettings.initialStatistics.currentHealth, enemyLevel = 1
            };
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
            SIEventsHandler.OnWaveEnd += HandleOnWaveEnd;
            SIGameplayEvents.OnDamage += HandleOnDamage;
            //            SIEventsHandler.OnEnemyDeath += HandleOnEnemyDeath;

        }
        void RemoveEvents()
        {
            SIEventsHandler.OnWaveEnd -= HandleOnWaveEnd;
            SIGameplayEvents.OnDamage -= HandleOnDamage;
            //            SIEventsHandler.OnEnemyDeath -= HandleOnEnemyDeath;
        }

        void HandleOnWaveEnd()
        {
            SetEnemyAlive();
        }

        void HandleOnDamage(DamageInfo damageInfo)
        {
            if (this != damageInfo.ObjectToDamage)
                return;
            
            ApplyDamage(damageInfo.Damage);
            TryToBroadcastEnemyDeath();
        }
        void ApplyDamage(float damage)
        {
            _enemyEntityStatistics.currentHealth -= damage;
        }
        
        public bool IsEnemyAlive()
        {
            return _enemyEntityStatistics.isAlive;
        }

        void TryToBroadcastEnemyDeath()
        {
            if (_enemyEntityStatistics.currentHealth > 0)
                return;
            
            SIEventsHandler.BroadcastOnEnemyDeath(this);
            SetEnemyDead();
        }

        void SetEnemyDead()
        {
            SetEnemyVisibility(false);
            SetEnemyStatistics(false);
        }

        void SetEnemyAlive()
        {
            SetEnemyVisibility(true);
            SetEnemyStatistics(true);
        }
        
        void SetEnemyVisibility(bool isEnabled)
        {
            _colliderParent.SetActive(isEnabled);
            _meshRenderer.enabled = isEnabled;
        }

        void SetEnemyStatistics(bool isEnabled)
        {
            _enemyEntityStatistics.isAlive = isEnabled;
            // todo: edit when all data will be completed;
            _enemyEntityStatistics.enemyLevel = 1;
            _enemyEntityStatistics.currentHealth = 100;
        }
        
        //todo: DONT REMOVE
//        void HandleOnEnemyDeath(SIEnemyBehaviour enemyBehaviour)
//        {
//            // in case of testing make instant death option
//            if (this != enemyBehaviour)
//                return;
//            
//            EnableEnemyVisibility(false);
//            _enemyEntityStatistics.isAlive = false;
//        }
        
        //        public void Death()
//        {
//            if (_enemyStatistics.isAlive == false) return;
//
//            EnableEnemyVisibility(false);
//            playerBonusManager.DropBonus();
//            _enemyStatistics.isAlive = false;
//            weaponEntity.HandleWaitOnProjectileReset();
//
//            SIEventsHandler.BroadcastOnShootingEnemiesUpdate(enemyIndex);
//        }
    }
}