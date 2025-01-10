using System;
using UnityEngine;
using Util;

namespace Overhead_Stretch
{
    public class ObstacleBehaviour : BaseObstacle
    {
        private void Awake()
        {
            Direction = Vector3.back;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                gameObject.SetActive(false);
                GameManager.IncreaseScoreEvent.Invoke();
            }
            else if (other.gameObject.CompareTag("Despawner"))
            {
                GameManager.GameOverEvent.Invoke();
            }
        }
    }
}