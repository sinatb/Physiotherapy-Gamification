using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Util;

namespace Thumb_Exercise
{
    public class Tile : BaseObstacle
    {
        
        public bool                   isPlaying;
        public bool                   isInvalidated;
        public bool                   isLast;
        public float                  scoreIncreaseInterval;
        public AudioClip              Audio { get; set; }

        private AudioSource           _source;
        private MaterialPropertyBlock _propertyBlock;
        private MeshRenderer          _meshRenderer;
        private float                 _scoreIncreaseTimer;
        public IEnumerator ChangeColor(Color c, float duration)
        {
            var step = duration/100;
            var baseColor = Color.white;
            for (var i = 0; i < 100; i++)
            {
                _propertyBlock.SetColor("_BaseColor", Color.Lerp(baseColor, c,step*i/duration));
                _meshRenderer.SetPropertyBlock(_propertyBlock);
                yield return new WaitForSeconds(step);
            }
        }
        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            _meshRenderer = GetComponent<MeshRenderer>();
            _source = GetComponent<AudioSource>();
            Direction = Vector3.down;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") &&
                !isInvalidated && 
                _scoreIncreaseTimer >= scoreIncreaseInterval)
            {
                GameManager.IncreaseScoreEvent?.Invoke();
                _scoreIncreaseTimer = 0.0f;
            }

            _scoreIncreaseTimer += Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isInvalidated)
            {
                if (!isPlaying)
                {
                    StartCoroutine(ChangeColor(Color.green, 1.5f));
                    _source.PlayOneShot(Audio);
                }
                isPlaying = true;
                
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Despawner"))
            {
                if (isLast)
                    GameManager.GameOverEvent?.Invoke();
                
                gameObject.SetActive(false);
            }
        }
        public void SetYScale(float yScale)
        {
            transform.localScale = new Vector3(transform.localScale.x,
                yScale,
                transform.localScale.z);
        }

        private void OnDisable()
        {
            isPlaying = false;
            isInvalidated = false;
            isLast = false;
            StopAllCoroutines();
            _propertyBlock.SetColor("_BaseColor", Color.white);
            _meshRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}