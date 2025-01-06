using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Thumb_Exercise
{
    public class TileSpawner : MonoBehaviour
    {
        public List<GameObject> spawnPosition;
        public float baseSpeed;
        public float spawnInterval;
        public float increaseTime;
        public float increaseSpeed;
        public float maximumIncrease;
        public ObjectPool pool;
        
        private float _timer;
        private bool _isRunning = true;
        private float _currentSpawnInterval;
        private float _currentSpeed;
        private void Start()
        {
            _currentSpeed = baseSpeed;
            _currentSpawnInterval = spawnInterval;
            GameManager.GameOverEvent += OnGameOver;
            GameManager.RestartEvent += OnRestart;
            InvokeRepeating(nameof(UpdateSpeed),0.0f,increaseTime);
        }
        private void OnRestart()
        {
            _isRunning = true;
            _currentSpawnInterval = spawnInterval;
            _currentSpeed = baseSpeed;
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
                // if (g.activeInHierarchy)
                //     g.GetComponent<Tile>().SetSpeed(_currentSpeed);
            }
        }


    }
}