
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

        
        private IEnumerator FadeTimer()
        {
            var num = feedbackDuration/0.01;
            var posLeft = scoreFeedbackTextLeft.transform.localPosition;
            var posRight = scoreFeedbackTextRight.transform.localPosition;
            for (var i = 0; i < num; i++)
            {
                scoreFeedbackTextLeft.transform.localPosition += (Vector3.up * 0.01f);
                scoreFeedbackTextRight.transform.localPosition += (Vector3.up * 0.01f);

                scoreFeedbackTextLeft.SetText("+1");
                scoreFeedbackTextRight.SetText("+1");
                
                scoreFeedbackTextLeft.alpha = 1.0f - i * 0.01f;
                scoreFeedbackTextRight.alpha = 1.0f - i * 0.01f;
                
                yield return new WaitForSeconds(0.01f);
            }
            scoreFeedbackTextLeft.transform.localPosition = posLeft;
            scoreFeedbackTextRight.transform.localPosition = posRight;
            
            scoreFeedbackTextLeft.alpha = 0.0f;
            scoreFeedbackTextRight.alpha = 0.0f;
        }
        private void OnGameOver()
        {
            _audioSource.PlayOneShot(gameOverSound);
        }

        private void OnIncreaseScore()
        {
            _soundPlayCounter++;
            StartCoroutine(FadeTimer());
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