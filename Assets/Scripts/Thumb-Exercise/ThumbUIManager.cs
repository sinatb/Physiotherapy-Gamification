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
        }
     
    }
}