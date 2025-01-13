using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class ObjectPool:MonoBehaviour
    {
        public List<GameObject> prefabs;
        public int poolSize;
        private List<GameObject> _pool = new List<GameObject>();

        private void Awake()
        {
            GameObject tmp;
            for (var i = 0; i < poolSize; i++)
            {
                foreach (var p in prefabs)
                {
                    tmp = Instantiate(p);
                    tmp.SetActive(false);
                    _pool.Add(tmp);   
                }
            }
        }
        public GameObject GetPooledObject()
        {
            var g = _pool[Random.Range(0, _pool.Count)];
            while (g.activeInHierarchy)
            {
                g = _pool[Random.Range(0, _pool.Count)];
            }
            return g;
        }
        public List<GameObject> GetActiveObjects()
        {
            var result = new List<GameObject>();
            foreach (var g in _pool)
            {
                if (g.activeInHierarchy)
                    result.Add(g);
            }
            return result;
        }

        public void DeactivateObjects()
        {
            foreach (var g in _pool)
            {
                g.SetActive(false);
            }
        }
    }
}