using DDL;
using Graphics;
using UnityEngine;
using Random = UnityEngine.Random;
using Util;

namespace Shoulder_Stretch
{
    public class WallSpawner : BaseSpawner
    {
        public float            wallOffset;
        public float            increaseTime;
        public float            increaseSpeed;
        public PlayerController player;
        public int              maximumIncrease;
        

        private CircularList<int> _history;

        private void OnGameOverSetup()
        {
            GraphicsManager.Instance.roadSpeed = 0;
            player.rotationSpeed = 0;
        }

        private void Start()
        {
            GameManager.GameOverEvent += OnGameOverSetup;
            _history = new CircularList<int>(3);
            InvokeRepeating(nameof(UpdateSpeed),0.0f,increaseTime);
        }
        protected override void Setup()
        {
            CurrentSpeed = CurrentDdl.baseSpeed;
            GraphicsManager.Instance.roadSpeed = CurrentSpeed/26;
            player.rotationSpeed = CurrentSpeed*3;
            CurrentSpawnInterval = ((DdlData)CurrentDdl).baseSpawnInterval;
        }

        protected override void SetupDdl(DdlBase d)
        {
            CurrentSpeed = d.baseSpeed;
            GraphicsManager.Instance.roadSpeed = CurrentSpeed/26;
            player.rotationSpeed = CurrentSpeed*3;
            CurrentSpawnInterval = ((DdlData)d).baseSpawnInterval;
        }
        
        protected override void Spawn()
        {
            var side = Random.Range(-1, 2);
            while (_history.CountEquals(side) > 1)
            {
                side = Random.Range(-1, 2);
            }
            _history.Add(side);
            var spawnpos = new Vector3(transform.position.x - side *wallOffset,
                                            0,
                                            transform.position.z);
            GameObject wall = pool.GetPooledObject();
            wall.transform.position = spawnpos;
            wall.GetComponent<BaseObstacle>().SetSpeed(CurrentSpeed);
            wall.SetActive(true);
        }
        private void UpdateSpeed()
        {
            if (CurrentSpeed + increaseSpeed > maximumIncrease || !IsRunning)
            {
                return;
            }
            CurrentSpeed += increaseSpeed;
            GraphicsManager.Instance.roadSpeed = CurrentSpeed/26;
            player.rotationSpeed = CurrentSpeed*3;
            CurrentSpawnInterval -= 0.2f;
            var active = pool.GetActiveObjects();
            foreach (var g in active)
            {
                g.GetComponent<BaseObstacle>().SetSpeed(CurrentSpeed);
            }
        }
        
    }
}

