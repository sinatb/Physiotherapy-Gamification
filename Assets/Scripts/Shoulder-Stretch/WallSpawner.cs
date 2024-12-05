using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject wallPrefab;
    public float wallOffset;
    public float spawnInterval;
    public int poolSize;
    private List<GameObject> _pool;
    
    private void Start()
    {
        _pool = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < poolSize; i++)
        {
            tmp = Instantiate(wallPrefab);
            tmp.SetActive(false);
            _pool.Add(tmp);
        }
        InvokeRepeating(nameof(SpawnWall),0.0f,spawnInterval);
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
        var spawnpos = new Vector3(transform.position.x - side *wallOffset,
                                        1.5f,
                                        transform.position.z);
        GameObject wall = GetPooledObject();
        wall.transform.position = spawnpos;
        wall.SetActive(true);
    }
    
}
