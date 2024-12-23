using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI highScoreText;
    public TextMeshProUGUI serverDebugData;
    public TextMeshProUGUI playerScore;
    private int _playerScoreValue;
    
    public delegate void UpdateData(PointDataList data);
    public static UpdateData UpdateDataEvent;

    public delegate void GameOver();
    public static GameOver GameOverEvent;

    public delegate void Restart();
    public static Restart RestartEvent;

    public void RestartFunction()
    {
        RestartEvent?.Invoke();
        gameOverPanel.SetActive(false);
        _playerScoreValue = 0;
        playerScore.text = "Score : " + _playerScoreValue;
    }
    private void GameOverFunction()
    {
        gameOverPanel.SetActive(true);
        highScoreText.text = $"Score: {_playerScoreValue}";
    }
    public void IncreaseScore()
    {
        _playerScoreValue++;
        if (playerScore != null)
            playerScore.text = "Score : " + _playerScoreValue;
    }
    private void Awake()
    {
        if (playerScore != null)
            playerScore.text = "Score : 0";
        GameOverEvent += GameOverFunction;
        if (Instance == null)
        {
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
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
}
