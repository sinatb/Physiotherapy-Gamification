using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Shoulder_Stretch
{
    
    public class PlayerController : MonoBehaviour
    {
        private enum Side
        {
            Left,
            Middle,
            Right
        }

        [SerializeField] private TextMeshProUGUI scoreFeedbackText;
        [SerializeField] private float           moveOffset;
        [SerializeField] private float           speed;
        [SerializeField] private float           changeTimer;
        [SerializeField] private float           feedbackDuration;
        [SerializeField] private AudioClip       swipeSound;
        [SerializeField] private AudioClip       pointSound;
        [SerializeField] private AudioClip       gameOverSound;
        private float                            _timer;
        private Side                             _mSide = Side.Middle;
        private Rigidbody                        _mRigidBody;
        private char                             _nextMove = 'm';
        private Dictionary<Side, Vector3>        _positions;
        private bool                             _isRunning = true;
        private AudioSource                      _audioSource;

        private IEnumerator FadeTimer()
        {
            var num = feedbackDuration/0.01;
            var pos = scoreFeedbackText.transform.localPosition;
            for (var i = 0; i < num; i++)
            {
                scoreFeedbackText.transform.localPosition += (Vector3.up * 0.01f);
                scoreFeedbackText.SetText("+1");
                scoreFeedbackText.alpha = 1.0f - i * 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            scoreFeedbackText.transform.localPosition = pos;
            scoreFeedbackText.alpha = 0.0f;
        }

        private void Awake()
        {
            GameManager.UpdateDataEvent += UpdateData;
            GameManager.GameOverEvent += GameOverFunction;
            GameManager.RestartEvent += RestartFunction;
            GameManager.IncreaseScoreEvent += IncreaseScoreFunction;
            _audioSource = GetComponent<AudioSource>();
            _mRigidBody = GetComponent<Rigidbody>();
            _positions = new Dictionary<Side, Vector3>()
            {
                { Side.Left, new Vector3(-moveOffset, 0.5f, 0) },
                { Side.Middle, new Vector3(0, 0.5f, 0) },
                { Side.Right, new Vector3(moveOffset, 0.5f, 0) }
            };
        }

        private void IncreaseScoreFunction()
        {
            StartCoroutine(FadeTimer());
            _audioSource.PlayOneShot(pointSound);
        }

        private void RestartFunction()
        {
            _isRunning = true;
        }

        private void GameOverFunction()
        {
            _isRunning = false;
        }

        private void FixedUpdate()
        {
            if (_isRunning)
            {
                if (Vector3.Distance(transform.position, _positions[_mSide]) == 0.0f)
                    return;
                if (Vector3.Distance(transform.position, _positions[_mSide]) > 0.1f)
                    _mRigidBody.MovePosition(transform.position -
                                             (transform.position - _positions[_mSide]).normalized *
                                             (speed * Time.fixedDeltaTime));
                else
                    _mRigidBody.MovePosition(_positions[_mSide]);
            }
        }

        private void Update()
        {
            if (_isRunning)
            {
                _timer += Time.deltaTime;
                if (Input.GetKeyUp(KeyCode.A) || _nextMove == 'l')
                {
                    _audioSource.PlayOneShot(swipeSound);
                    MoveLeft();
                    _nextMove = 'm';
                }
                else if (Input.GetKeyUp(KeyCode.D) || _nextMove == 'r')
                {
                    _audioSource.PlayOneShot(swipeSound);
                    MoveRight();
                    _nextMove = 'm';
                }
            }
        }

        private void MoveRight()
        {
            if (_mSide == Side.Right)
                return;
            _mSide++;
        }

        private void MoveLeft()
        {
            if (_mSide == Side.Left)
                return;
            _mSide--;
        }

        private void UpdateData(PointDataList pdl)
        {
            if (_timer < changeTimer)
                return;
            _timer = 0.0f;
            var rightHand = VectorUtil.MeanVector(new List<Vector3>()
            {
                pdl.points[15].Vect,
                pdl.points[17].Vect,
                pdl.points[19].Vect,
            });

            var leftHand = VectorUtil.MeanVector(new List<Vector3>()
            {
                pdl.points[16].Vect,
                pdl.points[18].Vect,
                pdl.points[20].Vect,
            });

            if (leftHand.x > pdl.points[11].x)
                _nextMove = 'l';
            else if (rightHand.x < pdl.points[12].x)
                _nextMove = 'r';
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                _audioSource.PlayOneShot(gameOverSound);   
                GameManager.GameOverEvent.Invoke();
            }
        }
    }
}