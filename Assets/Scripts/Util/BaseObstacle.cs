using UnityEngine;

namespace Util
{
    public abstract class BaseObstacle : MonoBehaviour
    {
        protected Vector3 Direction = Vector3.back;
        private Rigidbody _mRigidbody;
        private float     _speed;
        private void Start()
        {
            _mRigidbody = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            _mRigidbody.MovePosition(transform.position +
                                     Direction * (_speed * Time.fixedDeltaTime));
        }
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
    }
}