using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameStartPanel;
    [SerializeField] private GameObject timeSelect;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI playerScore;

    private void Awake()
    {
        playerScore.text = "Score : 0";
    }
    private void Start()
    {
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
        playerScore.text = "Score : 0";
    }

    private void OnIncreaseScore()
    {
        playerScore.text = "Score : " + GameManager.Instance.PlayerScore;
    }
}
