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
        
        private bool               _isRunning = false;
        private float              _timer;
        private bool               _isDdlLoaded = false;
  

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
                SetSpeed(g);
            }
            _isDdlLoaded = true;
        }
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= 5.0f && !_isDdlLoaded)
            {
                CurrentDdl = dynamicDifficultyData[0];
                SetupDdl(CurrentDdl);
                _isRunning = true;
            }
            if (_timer >= CurrentSpawnInterval && _isRunning)
            {
                Spawn();
                _timer = 0.0f;
            }
        }
        private void OnGameOver()
        {
            _isRunning = false;
            pool.DeactivateObjects();
        }
        private void OnRestart()
        {
            Setup();
            _isRunning = true;
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