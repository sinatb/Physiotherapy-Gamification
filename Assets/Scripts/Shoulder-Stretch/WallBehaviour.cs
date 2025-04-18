using System;
using UnityEngine;
using Util;

namespace Shoulder_Stretch
{
    public class WallBehaviour : BaseObstacle
    {
        private void Awake()
        {
            Direction = Vector3.back;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Despawner"))
            {
                gameObject.SetActive(false);
                GameManager.IncreaseScoreEvent.Invoke();
            }
        }
    }
}