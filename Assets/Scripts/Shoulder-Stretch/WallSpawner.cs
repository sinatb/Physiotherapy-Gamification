using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WallSpawner : MonoBehaviour
{
    public GameObject wallPrefab;
    public float wallOffset;
    public float spawnInterval;
    public int poolSize;
    public float baseSpeed;
    public float increaseTime;
    public float increaseSpeed;
    public int maximumIncrease;
    private List<GameObject> _pool;
    private float _timer = 0.0f;
    private float _currentSpeed;
    private CircularList<int> _history;
    private void Start()
    {
        _currentSpeed = baseSpeed;
        _history = new CircularList<int>(3);
        _pool = new List<GameObject>();
        GameObject tmp;
        for (var i = 0; i < poolSize; i++)
        {
            tmp = Instantiate(wallPrefab);
            tmp.SetActive(false);
            _pool.Add(tmp);
        }
        InvokeRepeating(nameof(UpdateSpeed),0.0f,increaseTime);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            SpawnWall();
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
        GameObject wall = GetPooledObject();
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
        spawnInterval -= 0.1f;
        foreach (var g in _pool)
        {
            if (g.activeInHierarchy)
                g.GetComponent<WallBehaviour>().SetSpeed(_currentSpeed);
        }
    }
    
}
