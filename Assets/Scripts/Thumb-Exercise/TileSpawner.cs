using System.Collections.Generic;
using DDL;
using UnityEngine;
using Util;

namespace Thumb_Exercise
{
    public class TileSpawner : MonoBehaviour
    {
        public List<GameObject>   spawnPosition;
        public ObjectPool         pool;
        public List<DdlThumbData> dynamicDifficultyData;
        public float              baseSpeed;
        
        private float        _timer;
        private DdlThumbData _currentDdl;
        private bool         _isRunning;
        private float        _baseSpeed;
        private float        _currentSpeed;
        
        private const float  SpawnInterval = 2.0f;

        private void Start()
        {
            if (_currentDdl != null)
            {
                _currentSpeed = baseSpeed;
            }
            GameManager.GameOverEvent += OnGameOver;
            GameManager.RestartEvent += OnRestart;
            GameManager.DdlSetEvent += OnDdlSet;
        }
        private void OnRestart()
        {
            _isRunning = true;
            _currentSpeed = _currentDdl.speed;
            _timer = 0.0f;
        }
        private void OnDdlSet()
        {
            if (GameManager.Instance.Player != null)
            {
                foreach (var d in dynamicDifficultyData)
                {
                    if (d.InRange(GameManager.Instance.Player.high_score))
                    {
                        _currentSpeed = d.speed;
                    }
                }
            }
            var active = pool.GetActiveObjects();
            foreach (var g in active)
            {
                g.GetComponent<WallBehaviour>().SetSpeed(_currentSpeed);
            }
        }
        private void OnGameOver()
        {
            _isRunning = false;
            pool.DeactivateObjects();
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
                _currentSpeed = _currentDdl.speed;
                _isRunning = true;
            }
            if (_timer >= SpawnInterval && _isRunning)
            {
                SpawnTile();
                _timer = 0.0f;
            }
        }
        private void SpawnTile()
        {
            var randomIndex = Random.Range(0, spawnPosition.Count);
            var obj = pool.GetPooledObject();
            obj.transform.position = spawnPosition[randomIndex].transform.position;
            obj.SetActive(true);
            obj.GetComponent<Tile>().SetSpeed(_currentSpeed);
        }
    }
}