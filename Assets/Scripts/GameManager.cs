using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Util;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public PlayerData                        Player;
    public TextMeshProUGUI                   serverDebugData;
    public TextMeshProUGUI                   playerScore;
    
    [SerializeField] private GameObject      gameOverPanel;
    [SerializeField] private TextMeshProUGUI highScoreText;
    
    private int                              _playerScoreValue;

    #region events
    
    public delegate void UpdateData(PointDataList data);
    public static UpdateData UpdateDataEvent;

    public delegate void GameOver();
    public static GameOver GameOverEvent;

    public delegate void Restart();
    public static Restart RestartEvent;

    public delegate void IncreaseScore();
    public static IncreaseScore IncreaseScoreEvent;

    #endregion
    public void OnRestart()
    {
        RestartEvent?.Invoke();
        gameOverPanel.SetActive(false);
        _playerScoreValue = 0;
        playerScore.text = "Score : " + _playerScoreValue;
    }
    private void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        if (Player != null)
            PostHighScore();
        highScoreText.text = $"Score: {_playerScoreValue}";
    }
    private void OnIncreaseScore()
    {
        _playerScoreValue++;
        if (playerScore != null)
            playerScore.text = "Score : " + _playerScoreValue;
    }
    private void Awake()
    {
        if (playerScore != null)
            playerScore.text = "Score : 0";
        GameOverEvent += OnGameOver;
        IncreaseScoreEvent += OnIncreaseScore;
        if (Instance == null)
        {
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }
    public void set_player_data(string data)
    {
        Player = JsonConvert.DeserializeObject<PlayerData>(data);
    }
    public void update_client_data(string data)
    {
        var cleanData = data.Substring(1, data.Length - 2);
        var points = JsonUtil.DeserializeToList<PointData>(cleanData);
        var pdl = new PointDataList();
        pdl.points = points.ToList();
        UpdateDataEvent.Invoke(pdl);
    }
    public void update_server_debug_data(string data)
    {
        var cleanData = data.Substring(1, data.Length - 2);
        var points = JsonUtil.DeserializeToList<PointData>(cleanData);
        var pdl = new PointDataList();
        pdl.points = points.ToList();
        if (serverDebugData != null)
            serverDebugData.text = pdl.points[0].ToString();
        UpdateDataEvent.Invoke(pdl);
    }

    #region backend
    private class HighScoreData
    {
        public int HighScore;
    }
    public void PostHighScore()
    {
        StartCoroutine(PostHighScoreCoroutine());
    }
    //@TODO
    //change the apiUrl when amirreza provides it
    //
    private IEnumerator PostHighScoreCoroutine()
    {
        var highScoreData = new HighScoreData()
        {
            HighScore = _playerScoreValue
        };

        var jsonData = JsonConvert.SerializeObject(highScoreData);

        using (var request = new UnityWebRequest("apiUrl", "POST"))
        {
            var jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {Player.Token}");

            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("High score uploaded successfully!");
                Debug.Log("Server response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Failed to upload high score.");
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text);
            }
        }
    }
    #endregion
}
