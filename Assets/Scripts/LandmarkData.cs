using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointData
{
    public float x;
    public float y;
    public float z;
    public float visibility;

    public override string ToString()
    {
        return "{" + "x:" + x.ToString("0.00") + " ,"
                   + "y:" + y.ToString("0.00") + " ,"
                   + "z:" + z.ToString("0.00") + " ,"
                   + "v:" + visibility.ToString("0.00") + " " + "}";
    }
}

public class PointDataList
{
    public List<PointData> points;
}