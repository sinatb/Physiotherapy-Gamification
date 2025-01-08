using System.Collections.Generic;
using DDL;
using UnityEngine;
using Util;

namespace Overhead_Stretch
{
    public class ObstacleSpawner : BaseSpawner
    {
        public List<GameObject>  spawnPosition;
        public float             increaseSpeed;
        public float             maximumIncrease;
        public float             increaseTime;
        
        private void Start()
        {
            InvokeRepeating(nameof(UpdateSpeed),0.0f,increaseTime);
        }
        protected override void Spawn()
        {
            var side = Random.Range(0, 2);
            
            var horizontalCoef = 0.0f;
            if (side == 0)
                horizontalCoef = 1.0f;
            else if (side == 1)
                horizontalCoef = 1.4f;
            
                    
            var w1 = pool.GetPooledObject();
            var spawnPosition1 = spawnPosition[side].transform.position;
            spawnPosition1.x += horizontalCoef;
            w1.transform.position = spawnPosition1;
            w1.GetComponent<ObstacleBehaviour>().SetSpeed(CurrentSpeed);

            w1.SetActive(true);
            
            var w2 = pool.GetPooledObject();
            var spawnPosition2 = spawnPosition[side].transform.position;
            spawnPosition2.x -= horizontalCoef;
            w2.transform.position = spawnPosition2;
            w2.GetComponent<ObstacleBehaviour>().SetSpeed(CurrentSpeed);
            
            w2.SetActive(true);
            
        }
        private void UpdateSpeed()
        {
            if (CurrentSpeed + increaseSpeed > maximumIncrease)
            {
                CancelInvoke(nameof(UpdateSpeed));
                return;
            }
            CurrentSpeed += increaseSpeed;
            CurrentSpawnInterval -= 0.2f;
            var active = pool.GetActiveObjects();
            foreach (var g in active)
            {
                g.GetComponent<ObstacleBehaviour>().SetSpeed(CurrentSpeed);
            }
        }
        protected override void Setup()
        {
            CurrentSpeed = CurrentDdl.baseSpeed;
            CurrentSpawnInterval = ((DdlData)CurrentDdl).baseSpawnInterval;
        }
        protected override void SetupDdl(DdlBase d)
        {
            CurrentSpeed = d.baseSpeed;
            CurrentSpawnInterval = ((DdlData)d).baseSpawnInterval;
        }
        protected override void SetSpeed(GameObject g)
        {
            g.GetComponent<ObstacleBehaviour>().SetSpeed(CurrentSpeed);
        }
    }
}

