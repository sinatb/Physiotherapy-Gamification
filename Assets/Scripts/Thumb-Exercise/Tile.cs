using System;
using UnityEngine;
using Util;

namespace Thumb_Exercise
{
    public class Tile : BaseObstacle
    {
        private void Awake()
        {
            Direction = Vector3.down;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Despawner"))
            {
                gameObject.SetActive(false);
            }
        }
        public void SetYScale(float yScale)
        {
            transform.localScale = new Vector3(transform.localScale.x,
                yScale,
                transform.localScale.z);
        }
    }
}