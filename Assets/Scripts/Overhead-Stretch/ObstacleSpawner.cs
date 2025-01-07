using System.Collections.Generic;
using DDL;
using UnityEngine;

namespace Overhead_Stretch
{
    public class ObstacleSpawner : MonoBehaviour
    {
        public GameObject        obstaclePrefab;
        public List<GameObject>  spawnPosition;
        public int               poolSize;
        public float             increaseSpeed;
        public float             maximumIncrease;
        public float             increaseTime;
        public float             spawnInterval;
        public float             baseSpeed;
        public List<DdlData>     dynamicDifficultyData;
        
        private List<GameObject> _pool;
        private float            _timer;
        private bool             _isRunning = true;
        private float            _currentSpeed;
        private float            _currentSpawnInterval;
        
        private void Start()
        {
            _pool = new List<GameObject>();
            _currentSpeed = baseSpeed;
            _currentSpawnInterval = spawnInterval;
            
            GameManager.GameOverEvent += OnGameOver;
            GameManager.RestartEvent += OnRestart;
            GameManager.DdlSetEvent += OnDdlSet;
            
            GameObject tmp;
            for (var i = 0; i < poolSize; i++)
            {
                tmp = Instantiate(obstaclePrefab);
                tmp.SetActive(false);
                _pool.Add(tmp);
            }
            InvokeRepeating(nameof(UpdateSpeed),0.0f,increaseTime);
        }
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _currentSpawnInterval && _isRunning)
            {
                SpawnObstacle();    
                _timer = 0.0f;
            }
        }
        private GameObject GetPooledObject()
        {
            foreach (var g in _pool)
            {
                if (!g.activeInHierarchy)
                    return g;
            }
            return null;
        }
        private void SpawnObstacle()
        {
            var side = Random.Range(0, 2);
            
            var horizontalCoef = 0.0f;
            if (side == 0)
                horizontalCoef = 1.0f;
            else if (side == 1)
                horizontalCoef = 1.4f;
            
                    
            var w1 = GetPooledObject();
            var spawnPosition1 = spawnPosition[side].transform.position;
            spawnPosition1.x += horizontalCoef;
            w1.transform.position = spawnPosition1;
            w1.GetComponent<ObstacleBehaviour>().SetSpeed(_currentSpeed);

            w1.SetActive(true);
            
            var w2 = GetPooledObject();
            var spawnPosition2 = spawnPosition[side].transform.position;
            spawnPosition2.x -= horizontalCoef;
            w2.transform.position = spawnPosition2;
            w2.GetComponent<ObstacleBehaviour>().SetSpeed(_currentSpeed);
            
            w2.SetActive(true);
            
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
            foreach (var g in _pool)
            {
                if (g.activeInHierarchy)
                    g.GetComponent<ObstacleBehaviour>().SetSpeed(_currentSpeed);
            }
        }
        private void OnRestart()
        {
            _currentSpeed = baseSpeed;
            _currentSpawnInterval = spawnInterval;
            _isRunning = true;
            _timer = 0.0f;
        }
        private void OnGameOver()
        {
            _isRunning = false;
            foreach (var go in _pool)
            {
                go.SetActive(false);
            }
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
                        baseSpeed = d.baseSpeed;
                        spawnInterval = d.baseSpawnInterval;
                    }
                }
            }
            foreach (var g in _pool)
            {
                if (g.activeInHierarchy)
                    g.GetComponent<ObstacleBehaviour>().SetSpeed(_currentSpeed);
            }
        }
    }
}

