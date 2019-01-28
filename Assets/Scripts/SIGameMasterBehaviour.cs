﻿using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameMasterBehaviour : SIGenericSingleton<SIGameMasterBehaviour>
    {
        [SerializeField] private bool _isGameStarted;
        [SerializeField] private Camera _mainCamera;

        public Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    Debug.LogError("No camera assigned!");
                }
                return _mainCamera;
            }
        }

        private void OnEnable()
        {
            SIEventsHandler.OnGameStarted += StartGame;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnGameStarted -= StartGame;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            if (_isGameStarted == false)
            {
                return;
            }


            OnUpdateMovements();
        }

        private void OnUpdateMovements()
        {
            SIEventsHandler.OnObjectMovement?.Invoke();
        }

        public void OnGameStarted()
        {
            SIEventsHandler.OnGameStarted?.Invoke();
        }

        private void StartGame()
        {
            _isGameStarted = true;
        }
    }
}
