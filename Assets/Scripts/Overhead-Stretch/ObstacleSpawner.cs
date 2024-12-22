using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public List<GameObject> spawnPosition;
    public int poolSize;
    public float increaseSpeed;
    public float maximumIncrease;
    public float increaseTime;
    public float spawnInterval;
    private List<GameObject> _pool;
    private float _timer;
    private bool _isRunning = true;
    private float _currentSpeed;
    private void Start()
    {
        _pool = new List<GameObject>();
        GameManager.GameOverEvent += GameOverFunction;
        GameManager.RestartEvent += RestartFunction;
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
        if (_timer >= spawnInterval && _isRunning)
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
        var side = Random.Range(0, 3);
        
        var w1 = GetPooledObject();
        var w2 = GetPooledObject();

        var spawnPosition1 = spawnPosition[side].transform.position;
        var spawnPosition2 = spawnPosition[side].transform.position;
        
        var horizontalCoef = 0.6f;
        if (side == 1)
            horizontalCoef = 1.0f;
        else if (side == 2)
            horizontalCoef = 2.0f;
        
        spawnPosition1.x += horizontalCoef;
        spawnPosition2.x -= horizontalCoef;
        
        w1.transform.position = spawnPosition1;
        w2.transform.position = spawnPosition2;
        
        w1.GetComponent<WallBehaviour>().SetSpeed(_currentSpeed);
        w2.GetComponent<WallBehaviour>().SetSpeed(_currentSpeed);
        
        w1.SetActive(true);
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
        spawnInterval -= 0.1f;
        foreach (var g in _pool)
        {
            if (g.activeInHierarchy)
                g.GetComponent<WallBehaviour>().SetSpeed(_currentSpeed);
        }
    }
    private void RestartFunction()
    {
        _isRunning = true;
        _timer = 0.0f;
    }
    private void GameOverFunction()
    {
        _isRunning = false;
        foreach (var go in _pool)
        {
            go.SetActive(false);
        }
    }
}
