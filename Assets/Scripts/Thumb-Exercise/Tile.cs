using System;
using System.Collections;
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
        private Coroutine             _colorChangeCoroutine;
        private bool                  _isTouched;
        private readonly int          _colorID = Shader.PropertyToID("_BaseColor");
        private GameObject            _player;
        
        private IEnumerator ChangeColor(Color c, float duration)
        {
            var totalTime = 0.0f;
            var baseColor = Color.white;
            while (totalTime < duration)
            {
                totalTime += Time.deltaTime;
                if (_isTouched)
                {
                    _propertyBlock.SetColor(_colorID, Color.Lerp(baseColor, c, totalTime / duration));
                    _meshRenderer.SetPropertyBlock(_propertyBlock);
                }
                yield return null;
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
            if (isInvalidated || !other.CompareTag("Player")) return;
            if (_scoreIncreaseTimer >= scoreIncreaseInterval)
            {
                GameManager.IncreaseScoreEvent?.Invoke();
                _scoreIncreaseTimer = 0.0f;
            }
            if (_colorChangeCoroutine == null)
                _colorChangeCoroutine = StartCoroutine(ChangeColor(Color.green, Audio.length));
            _isTouched = true;
            _scoreIncreaseTimer += Time.fixedDeltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || isInvalidated) return;
            if (!isPlaying)
            {
                _source.PlayOneShot(Audio);
            }
            _player = other.gameObject;
            isPlaying = true;
            _isTouched = true;
            StartCoroutine(MonitorPlayer());

        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Despawner")) return;
            if (isLast)
                GameManager.GameOverEvent?.Invoke();
            gameObject.SetActive(false);
        }
        public void SetYScale(float yScale)
        {
            transform.localScale = new Vector3(transform.localScale.x,
                yScale,
                transform.localScale.z);
        }
        private IEnumerator MonitorPlayer()
        {
            while (_isTouched)
            {
                if (_player == null || !_player.activeInHierarchy)
                {
                    HandlePlayerExit(); 
                    yield break;
                }
                yield return null;
            }
        }

        private void HandlePlayerExit()
        {
            _isTouched = false;
            _player = null;

            if (_colorChangeCoroutine == null) return;
            StopCoroutine(_colorChangeCoroutine);
            _colorChangeCoroutine = null;
        }

        private void OnDisable()
        {
            isPlaying = false;
            isInvalidated = false;
            isLast = false;
            StopAllCoroutines();
            _colorChangeCoroutine = null;
            _isTouched = false;
            _player = null;
            _propertyBlock.SetColor(_colorID, Color.white);
            _meshRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}