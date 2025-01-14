using System;
using UnityEngine;
namespace Shoulder_Stretch
{
    public class BuildingBehaviour : MonoBehaviour
    {
        public float        speed;

        private void Update()
        {
            if (WallSpawner.Instance.IsRunning)
            {
                transform.position += Vector3.back * ((speed + WallSpawner.Instance.Speed / 20) * Time.deltaTime);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("BuildingDespawner"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}