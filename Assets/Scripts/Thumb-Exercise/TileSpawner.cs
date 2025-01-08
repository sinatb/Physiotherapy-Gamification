using System.Collections.Generic;
using DDL;
using UnityEngine;
using Util;

namespace Thumb_Exercise
{
    public class TileSpawner : BaseSpawner
    {
        public List<GameObject>   spawnPosition;
        private void Start()
        {
            CurrentSpawnInterval = 2.0f;
        }
        protected override void Setup()
        {
            CurrentSpeed = CurrentDdl.baseSpeed;
        }

        protected override void SetupDdl(DdlBase d)
        {
            CurrentSpeed = d.baseSpeed;
        }
        
        protected override void Spawn()
        {
            var randomIndex = Random.Range(0, spawnPosition.Count);
            var obj = pool.GetPooledObject();
            obj.transform.position = spawnPosition[randomIndex].transform.position;
            obj.SetActive(true);
            obj.GetComponent<BaseObstacle>().SetSpeed(CurrentSpeed);
            obj.GetComponent<Tile>().SetYScale(((DdlThumbData)CurrentDdl).size);
        }
    }
}