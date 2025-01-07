using System.Collections;
using System.Collections.Generic;
using DDL;
using UnityEngine;
using Util;

namespace Thumb_Exercise
{
    public class TileSpawner : MonoBehaviour
    {
        public List<GameObject> spawnPosition;
        public ObjectPool pool;
        public List<DdlThumbData> dynamicDifficultyData;

        private float _timer;
        private bool _isRunning = true;
        private float _currentSpawnInterval;
        private float _baseSpeed;
        private float _currentSpeed;
        
        
        private const float SpawnInterval = 2.0f;

        private void Start()
        {
            
            // _currentSpeed = baseSpeed;
            _currentSpawnInterval = SpawnInterval;
            GameManager.GameOverEvent += OnGameOver;
            GameManager.RestartEvent += OnRestart;
        }
        private void OnRestart()
        {
            _isRunning = true;
            _currentSpawnInterval = SpawnInterval;
            // _currentSpeed = baseSpeed;
            _timer = 0.0f;
        }
        private void OnGameOver()
        {
            _isRunning = false;
            pool.DeactivateObjects();
        }
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _currentSpawnInterval && _isRunning)
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