using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thumb_Exercise
{
    public class Tile : MonoBehaviour
    {
        private Rigidbody _mRigidbody;
        private float     _speed = 10.0f;

        private void Start()
        {
            _mRigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _mRigidbody.MovePosition(transform.position +
                                     Vector3.down * (_speed * Time.fixedDeltaTime));
        }
        
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
    }
}