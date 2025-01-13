using System.Collections.Generic;
using DDL;
using UnityEngine;

namespace Util
{
    public abstract class BaseSpawner : MonoBehaviour
    {
        public ObjectPool          pool;
        public List<DdlBase>       dynamicDifficultyData;


        protected float            CurrentSpeed;
        protected float            CurrentSpawnInterval;
        protected DdlBase          CurrentDdl;
        protected bool             IsRunning = false;

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
            if (!_isGameOver && !IsRunning)
                IsRunning = true;
        }
        private void Update()
        {
            _timer += Time.deltaTime;
            if (!_isGameOver && _timer >= 5.0f && !_isDdlLoaded)
            {
                CurrentDdl = dynamicDifficultyData[0];
                SetupDdl(CurrentDdl);
                IsRunning = true;
            }
            if (IsRunning && _timer >= CurrentSpawnInterval)
            {
                Spawn();
                _timer = 0.0f;
            }
        }
        private void OnGameOver()
        {
            IsRunning = false;
            _isGameOver = true;
            pool.DeactivateObjects();
        }
        private void OnRestart()
        {
            Setup();
            IsRunning = true;
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