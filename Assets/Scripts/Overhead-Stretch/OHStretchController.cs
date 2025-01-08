
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Util;
using UnityEngine;

namespace Overhead_Stretch
{


    public class OhStretchController : MonoBehaviour
    {
        [SerializeField] private float           spanMultiplier;
        [SerializeField] private float           heightMultiplier;
        [SerializeField] private GameObject      rHand;
        [SerializeField] private GameObject      lHand;
        [SerializeField] private TextMeshProUGUI scoreFeedbackTextLeft;
        [SerializeField] private TextMeshProUGUI scoreFeedbackTextRight;
        [SerializeField] private float           feedbackDuration;
        [SerializeField] private AudioClip       pointSound;
        [SerializeField] private AudioClip       gameOverSound;
        private AudioSource                      _audioSource;
        private int                              _soundPlayCounter;
        
        private void OnGameOver()
        {
            _audioSource.PlayOneShot(gameOverSound);
        }

        private void OnIncreaseScore()
        {
            _soundPlayCounter++;
            
            StartCoroutine(TextFeedback.FadeTimer(feedbackDuration,scoreFeedbackTextLeft));
            StartCoroutine(TextFeedback.FadeTimer(feedbackDuration,scoreFeedbackTextRight));
            
            if (_soundPlayCounter == 2)
            {
                _audioSource.PlayOneShot(pointSound);
                _soundPlayCounter = 0;
            }
        }
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            GameManager.GameOverEvent += OnGameOver;
            GameManager.IncreaseScoreEvent += OnIncreaseScore;
            GameManager.UpdateDataEvent += UpdateData;
        }
        private void UpdateData(PointDataList pdl)
        {
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

            rightHand.y = -rightHand.y;
            leftHand.y = -leftHand.y;

            rightHand.x *= spanMultiplier;
            leftHand.x *= spanMultiplier;

            rightHand.y *= heightMultiplier;
            leftHand.y *= heightMultiplier;

            rHand.transform.position = rightHand;
            lHand.transform.position = leftHand;
        }
    }
}