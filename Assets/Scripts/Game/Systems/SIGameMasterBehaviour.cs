﻿using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameMasterBehaviour : SIGenericSingleton<SIGameMasterBehaviour>
    {
        [SerializeField] bool _isGameStarted;
        [SerializeField] Camera _mainCamera;
        [SerializeField] SIPlayerBehaviour _player;
        [SerializeField] SIScreenAreaCalculator _screeenAreaCalculator;

        public Camera MainCamera
        {
            get
            {
                if (_mainCamera != null) 
                    return _mainCamera;
                
                SIHelpers.SISimpleLogger(this, "No camera assigned!", SimpleLoggerTypes.Error);
                return null;

            }
        }

        public SIPlayerBehaviour Player
        {
            get
            {
                if (_player != null)
                    return _player;

                SIHelpers.SISimpleLogger(this, "No player assigned to SIGameMasterBehaviour", SimpleLoggerTypes.Error);
                return null;
            }
        }

        public SIScreenAreaCalculator ScreenAreaCalculator => _screeenAreaCalculator;
        

        // Initialization in Start because, Awake is taken by signleton and some scripts may depend from it.
        protected void Start()
        {
            InitializeSpawners();
        }

        void InitializeSpawners()
        {
            SIEventsHandler.BroadcastOnSpawnObject();
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void AssignEvents()
        {
            SIEventsHandler.OnGameStarted += StartGame;
            SIEventsHandler.OnGameQuit += QuitGame;
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnGameStarted -= StartGame;
            SIEventsHandler.OnGameQuit -= QuitGame;
        }

        void StartGame()
        {
            _isGameStarted = true;
        }

        void Update()
        {
            HandleOnUpdate();
        }

        void HandleOnUpdate()
        {
            OnUpdateNoPlayableObjects();
            
            if (_isGameStarted == false)
                return;
                
            OnUpdateMovements();
            OnShadersUpdateCallback();
            OnDebugInputHandling();
            OnGameQuitCallback();
        }

        void OnUpdateNoPlayableObjects()
        {
            SIEventsHandler.BroadcastOnNonPlayableUpdate();
        }

        void OnDebugInputHandling()
        {
            SIEventsHandler.BroadcastOnDebugInputHandling();
        }

        void OnGameQuitCallback()
        {
            SIEventsHandler.BroadcastOnGameQuit();
        }

        void OnShadersUpdateCallback()
        {
            SIEventsHandler.BroadcastOnShadersUpdate();
        }

        void QuitGame()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                    Application.Quit();
#elif UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }

        void OnUpdateMovements()
        {
            SIEventsHandler.BroadcastOnUpdate();
        }

        public void OnGameStarted()
        {
            SIEventsHandler.BroadcastOnGameStarted();
        }
    }
}