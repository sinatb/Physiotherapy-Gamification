using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    public TextMeshProUGUI server_debug_data;
    
    public delegate void update_data(PointDataList data);
    public static update_data update_data_event;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }
    public void update_client_data(string data)
    {
        var clean_data = data.Substring(1, data.Length - 2);
        var points = JsonUtil.DeserializeToList<PointData>(clean_data);
        var pdl = new PointDataList();
        pdl.points = points.ToList();
        update_data_event.Invoke(pdl);
    }

    public void update_server_debug_data(string data)
    {
        var clean_data = data.Substring(1, data.Length - 2);
        var points = JsonUtil.DeserializeToList<PointData>(clean_data);
        var pdl = new PointDataList();
        pdl.points = points.ToList();
        server_debug_data.text = pdl.points[0].ToString();
        update_data_event.Invoke(pdl);
    }
}
