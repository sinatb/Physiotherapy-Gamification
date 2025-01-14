using System;
using TMPro;
using UnityEngine;
using Util;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject      gameOverPanel;
    [SerializeField] private GameObject      gameStartPanel;
    [SerializeField] private GameObject      timeSelect;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private TMP_Dropdown    timeDropdown;
    

    // Set the game type to endless and start the game
    public void SetEndlessMode()
    {
        GameManager.Instance.gameType = GameType.Endless;
        GameManager.Instance.canSpawn = true;
        gameStartPanel.SetActive(false);
        playerScore.text = "Score : 0";

    }
    // Set the game type to timed and show the time selection panel
    public void SetTimedMode()
    {
        GameManager.Instance.gameType = GameType.Timed;
        timeSelect.SetActive(true);
        gameStartPanel.SetActive(false);
    }
    // Start the game with the selected time
    public void StartGame()
    {
        GameManager.Instance.time = Char.GetNumericValue(timeDropdown.options[timeDropdown.value].text[0]) * 60;
        GameManager.Instance.canSpawn = true;
        timeSelect.SetActive(false);
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
        gameOverText.text = "Game Over";
        if (GameManager.Instance.gameType == GameType.Timed && GameManager.Instance.time <= 0)
            gameOverText.text = "You Won";
        GameManager.Instance.gameType = GameType.NotSet;
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
