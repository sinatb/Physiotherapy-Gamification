using System.Collections.Generic;
using DDL;
using UnityEngine;
using Random = UnityEngine.Random;
using Util;

namespace Shoulder_Stretch
{
    public class WallSpawner : MonoBehaviour
    {
        public float         wallOffset;
        public float         increaseTime;
        public float         increaseSpeed;
        public int           maximumIncrease;
        public List<DdlData> dynamicDifficultyData;
        public ObjectPool    pool;
        
        private float             _timer;
        private DdlData           _currentDdl;
        private float             _currentSpeed;
        private float             _currentSpawnInterval;
        private CircularList<int> _history;
        private bool              _isRunning = false;
        private void Start()
        {
            if (_currentDdl != null)
            {
                _currentSpeed = _currentDdl.baseSpeed;
                _currentSpawnInterval = _currentDdl.baseSpawnInterval;
            }
            _history = new CircularList<int>(3);
            GameManager.GameOverEvent += OnGameOver;
            GameManager.RestartEvent += OnRestart;
            GameManager.DdlSetEvent += OnDdlSet;

            InvokeRepeating(nameof(UpdateSpeed),0.0f,increaseTime);
        }
        private void OnRestart()
        {
            _isRunning = true;
            _currentSpawnInterval = _currentDdl.baseSpawnInterval;
            _currentSpeed = _currentDdl.baseSpeed;
            _timer = 0.0f;
        }
        private void OnGameOver()
        {
            _isRunning = false;
            pool.DeactivateObjects();
        }
        private void OnDdlSet()
        {
            if (GameManager.Instance.Player != null)
            {
                foreach (var d in dynamicDifficultyData)
                {
                    if (d.InRange(GameManager.Instance.Player.high_score))
                    {
                        _currentSpeed = d.baseSpeed;
                        _currentSpawnInterval = d.baseSpawnInterval;
                    }
                }
            }
            var active = pool.GetActiveObjects();
            foreach (var g in active)
            {
                g.GetComponent<WallBehaviour>().SetSpeed(_currentSpeed);
            }
        }
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_currentDdl != null)
            {
                _isRunning = true;
            }else if (_timer >= 5.0f)
            {
                _currentDdl = dynamicDifficultyData[0];
                _currentSpeed = _currentDdl.baseSpeed;
                _currentSpawnInterval = _currentDdl.baseSpawnInterval;
                _isRunning = true;
            }
            if (_timer >= _currentSpawnInterval && _isRunning)
            {
                SpawnWall();
                _timer = 0.0f;
            }
        }
        private void SpawnWall()
        {
            var side = Random.Range(-1, 2);
            while (_history.CountEquals(side) > 1)
            {
                side = Random.Range(-1, 2);
            }
            _history.Add(side);
            var spawnpos = new Vector3(transform.position.x - side *wallOffset,
                                            1.5f,
                                            transform.position.z);
            GameObject wall = pool.GetPooledObject();
            wall.transform.position = spawnpos;
            wall.GetComponent<WallBehaviour>().SetSpeed(_currentSpeed);
            wall.SetActive(true);
        }
        private void UpdateSpeed()
        {
            if (_currentSpeed + increaseSpeed > maximumIncrease)
            {
                CancelInvoke(nameof(UpdateSpeed));
                return;
            }
            _currentSpeed += increaseSpeed;
            _currentSpawnInterval -= 0.1f;
            var active = pool.GetActiveObjects();
            foreach (var g in active)
            {
                g.GetComponent<WallBehaviour>().SetSpeed(_currentSpeed);
            }
        }
        
    }
}

