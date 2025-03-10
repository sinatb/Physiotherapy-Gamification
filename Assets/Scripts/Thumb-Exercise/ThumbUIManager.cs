using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Util;

namespace Thumb_Exercise
{
    public class ThumbUIManager : MonoBehaviour
    {
        public TileSpawner                       spawner;
        public List<AudioClip>                   musics;
        [SerializeField] private GameObject      gameOverPanel;
        [SerializeField] private GameObject      gameStartPanel;
        [SerializeField] private TextMeshProUGUI playerScore;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TMP_Dropdown    musicSelect;
        [SerializeField] private TextMeshProUGUI feedbackText;

        private Coroutine _scoreIncreaseCoroutine;
        private int _accumulatedScore;
        // Start the game with the selected music
        public void StartGame()
        {
            gameStartPanel.SetActive(false);
            spawner.SetMusic(musics[musicSelect.value]);
            GameManager.Instance.canSpawn = true;
            playerScore.text = "Score : 0";
        }
        
        private void Start()
        {
            gameStartPanel.SetActive(true);
            GameManager.GameOverEvent += OnGameOver;
            GameManager.RestartEvent += OnRestart;
            GameManager.IncreaseScoreEvent += OnIncreaseScore;
        }

        private void OnGameOver()
        {
            highScoreText.text = $"Score: {GameManager.Instance.PlayerScore}";
            gameOverPanel.SetActive(true);
        }

        private void OnRestart()
        {
            gameOverPanel.SetActive(false);
            gameStartPanel.SetActive(true);
            GameManager.Instance.canSpawn = false;
        }

        private void OnIncreaseScore()
        {
            playerScore.text = "Score : " + GameManager.Instance.PlayerScore;
            _accumulatedScore++;
            if (_scoreIncreaseCoroutine == null)
             _scoreIncreaseCoroutine = StartCoroutine(PointFeedBack());
        }

        private IEnumerator PointFeedBack()
        {
            feedbackText.gameObject.SetActive(true);
            var score = 1;
            while (true)
            {
                feedbackText.text = "+" + score;
                yield return new WaitForSeconds(0.2f);
                if (score != _accumulatedScore)
                    break;
                score++;
            }
            _accumulatedScore = 0;
            feedbackText.gameObject.SetActive(false);
            _scoreIncreaseCoroutine = null;
        }
    }
}