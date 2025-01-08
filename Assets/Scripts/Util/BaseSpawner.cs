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
        
        private float              _timer;
        private bool               _isDdlLoaded = false;
        private bool               _isGameOver = false;
        private bool               _isRunning = false;


        protected abstract void Spawn();
        protected abstract void Setup();
        protected abstract void SetupDdl(DdlBase d);
        protected abstract void SetSpeed(GameObject g);
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