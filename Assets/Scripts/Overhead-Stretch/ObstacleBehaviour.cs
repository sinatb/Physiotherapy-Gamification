using UnityEngine;

namespace Overhead_Stretch
{
    public class ObstacleBehaviour : MonoBehaviour
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
                                     Vector3.back * (_speed * Time.fixedDeltaTime));
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

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
    }
}