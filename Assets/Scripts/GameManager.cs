using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [FormerlySerializedAs("server_debug_data")] [SerializeField]
    public TextMeshProUGUI serverDebugData;
    
    public delegate void UpdateData(PointDataList data);
    public static UpdateData UpdateDataEvent;

    private void Awake()
    {
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
