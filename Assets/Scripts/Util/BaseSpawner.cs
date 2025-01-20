using System.Collections.Generic;
using DDL;
using UnityEngine;

namespace Util
{
    public abstract class BaseSpawner : MonoBehaviour
    {
        public ObjectPool          pool;
        public List<DdlBase>       dynamicDifficultyData;
        public bool                IsRunning => _isRunning;
        public float               Speed => CurrentSpeed;

        protected float            CurrentSpeed;
        protected float            CurrentSpawnInterval;
        protected DdlBase          CurrentDdl;

        private bool               _isRunning = false;
        private float              _timer;
        private bool               _isDdlLoaded = false;
        private bool               _isGameOver = false;

        /// <summary>
        /// Spawning logic implementation. called at every spawnInterval
        /// </summary>
        protected abstract void Spawn();
        /// <summary>
        /// Common setup done on start and restart of each game
        /// </summary>
        protected abstract void Setup();
        /// <summary>
        /// Sets the class fields with respect to dynamic difficulty settings
        /// </summary>
        /// <param name="d">Dynamic difficulty Object</param>
        protected abstract void SetupDdl(DdlBase d);
        /// <summary>
        /// When DDL is set up from the server this event function updates obstacle speeds and also sets ddl data
        /// </summary>
        private void OnDdlSet()
        {
            if (GameManager.Instance.Player != null)
            {
                foreach (var d in dynamicDifficultyData)
                {
                    if (!d.InRange(GameManager.Instance.Player.high_score)) 
                        continue;
                    SetupDdl(d);
                    CurrentDdl = d;
                }
            }
            var active = pool.GetActiveObjects();
            foreach (var g in active)
            {
               g.GetComponent<BaseObstacle>().SetSpeed(CurrentSpeed);
            }
            _isDdlLoaded = true;
            if (!_isGameOver && !_isRunning)
                _isRunning = true;
        }
        private void Update()
        {
            if (!GameManager.Instance.canSpawn)
                return;
            _timer += Time.deltaTime;
            if (!_isGameOver && _timer >= 5.0f && !_isDdlLoaded)
            {
                CurrentDdl = dynamicDifficultyData[0];
                SetupDdl(CurrentDdl);
                _isRunning = true;
            }
            if (_isRunning && _timer >= CurrentSpawnInterval)
            {
                Spawn();
                _timer = 0.0f;
            }
        }
        private void OnGameOver()
        {
            _isRunning = false;
            _isGameOver = true;
            foreach (var d in dynamicDifficultyData)
            {
                if (!d.InRange(GameManager.Instance.PlayerScore)) 
                    continue;
                SetupDdl(d);
                CurrentDdl = d;
            }
            pool.DeactivateObjects();
        }
        private void OnRestart()
        {
            Setup();
            _isRunning = true;
            _isGameOver = false;
            _timer = 0.0f;
        }
        private void Awake()
        {
            if (CurrentDdl != null)
                Setup();
            GameManager.GameOverEvent += OnGameOver;
            GameManager.RestartEvent  += OnRestart;
            GameManager.DdlSetEvent   += OnDdlSet;
        }
    }
}