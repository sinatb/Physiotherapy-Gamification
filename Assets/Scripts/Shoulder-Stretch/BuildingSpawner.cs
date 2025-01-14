using UnityEngine;
using Util;

namespace Shoulder_Stretch
{
    public class BuildingSpawner : MonoBehaviour
    {
        public ObjectPool  buildingPool;
        public GameObject  rSpawn;
        public GameObject  lSpawn;
        
        private void Start()
        {
            InvokeRepeating(nameof(SpawnBuilding), 0, 5.0f);
        }

        private void SpawnBuilding()
        {
            if (!WallSpawner.Instance.IsRunning || !GameManager.Instance.canSpawn)
            {
                return;
            }

            var rBuilding = buildingPool.GetPooledObject();
            rBuilding.transform.position = rSpawn.transform.position;
            rBuilding.transform.rotation = Quaternion.Euler(new Vector3(0.0f,-90.0f,0.0f));
            rBuilding.SetActive(true);

            var lBuilding = buildingPool.GetPooledObject();
            lBuilding.transform.position = lSpawn.transform.position;
            lBuilding.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
            lBuilding.SetActive(true);
        }
    }
}