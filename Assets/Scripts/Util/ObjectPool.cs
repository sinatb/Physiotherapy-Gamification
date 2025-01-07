using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class ObjectPool:MonoBehaviour
    {
        public GameObject prefab;
        public int poolSize;
        private List<GameObject> _pool = new List<GameObject>();

        private void Awake()
        {
            GameObject tmp;
            for (var i = 0; i < poolSize; i++)
            {
                tmp = Instantiate(prefab);
                tmp.SetActive(false);
                _pool.Add(tmp);
            }
        }
        public GameObject GetPooledObject()
        {
            foreach (var g in _pool)
            {
                if (!g.activeInHierarchy)
                    return g;
            }
            return null;
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