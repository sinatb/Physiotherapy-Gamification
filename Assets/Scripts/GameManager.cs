using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Util;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public PlayerData                        Player;
    public GameType                          gameType;
    public bool                              canSpawn;
    public int                               PlayerScore => _playerScoreValue;
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

    public delegate void GameStart();
    public static GameStart GameStartEvent;
    
    public delegate void DdlSet();
    public static DdlSet DdlSetEvent;
    #endregion
    public void OnRestart()
    {
        RestartEvent?.Invoke();
        _playerScoreValue = 0;
    }
    private void OnGameOver()
    {
        if (Player != null)
            PostHighScore();
    }
    private void OnIncreaseScore()
    {
        _playerScoreValue++;
    }
    private void Awake()
    {
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

    #region callback
    public void set_player_data(string data)
    {
        Player = JsonConvert.DeserializeObject<PlayerData>(data);
        DdlSetEvent.Invoke();
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
        UpdateDataEvent.Invoke(pdl);
    }
    #endregion
    
    #region backend
    [System.Serializable]
    private class HighScoreData
    {
        public int game_type;
        public int score;
    }
    private void PostHighScore()
    {
        StartCoroutine(PostHighScoreCoroutine());
    }

    private IEnumerator PostHighScoreCoroutine()
    {
        var gameType = 1;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Overhead_Stretch" : gameType = 2; break;
            case "Thumb_Exercise" : gameType = 3; break;
            default: gameType = 1; break;
        }
        
        var highScoreData = new HighScoreData()
        {
            game_type = gameType,
            score = _playerScoreValue
        };

        var jsonData = JsonConvert.SerializeObject(highScoreData);

        using (var request = new UnityWebRequest("http://127.0.0.1:8000/api/scores/submit", "POST"))
        {
            var jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {Player.token}");

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
