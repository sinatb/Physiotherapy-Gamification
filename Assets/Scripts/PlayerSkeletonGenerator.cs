using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkeletonGenerator : MonoBehaviour
{
    public GameObject joint;
    public GameObject initialPos;
    public GameObject lrPrefab;
    public ConnectionMatrix connectionMatrix;
    public float scale;
    public float visibilityThreshold = 0.2f;
    private List<GameObject> _joints;
    private List<GameObject> _lrList;
    
    private Tuple<int,int> GetSrcDstIndex(int i)
    {
        var srcIndex = connectionMatrix.matrix[i].src;
        var dstIndex = connectionMatrix.matrix[i].dst;
        return new Tuple<int, int>(srcIndex, dstIndex);
    }
    private Tuple<Vector3,Vector3> GetSrcDstPos(int i)
    {
        var indices = GetSrcDstIndex(i);
        var srcPos = _joints[indices.Item1].transform.position;
        var dstPos = _joints[indices.Item2].transform.position;
        return new Tuple<Vector3, Vector3> (srcPos, dstPos);
    }
    private void Start()
    {
        GameManager.UpdateDataEvent += update_data;
        _joints = new List<GameObject>();
        _lrList = new List<GameObject>();
        for (int i=0; i<33; i++)
        {
            var obj = Instantiate(joint, initialPos.transform.position,Quaternion.identity,initialPos.transform);
            _joints.Add(obj);
        }
        for (int i=0; i< connectionMatrix.matrix.Count; i++) 
        {
            var indices = GetSrcDstIndex(i);
            var pos = GetSrcDstPos(i);
            var lrObj = Instantiate(lrPrefab, pos.Item1, Quaternion.identity, _joints[indices.Item1].transform);
            _lrList.Add(lrObj);
        }
    }
    public void update_data(PointDataList pdl) 
    {
        for(int i=0; i<pdl.points.Count;i++)
        {

            if (pdl.points[i].visibility < visibilityThreshold)
            {
                _joints[i].SetActive(false);
                continue;
            }else if (pdl.points[i].visibility > visibilityThreshold && !_joints[i].activeSelf) 
            {
                _joints[i].SetActive(true);
            }
            var point = new Vector3(pdl.points[i].x * scale,
                                    -pdl.points[i].y * scale, 
                                    pdl.points[i].z * scale
                                    );
            _joints[i].transform.position = point;
        }
        for (int i = 0; i < connectionMatrix.matrix.Count; i++)
        {
            var lr = _lrList[i].GetComponent<LineRenderer>();
            var pos = GetSrcDstPos(i);
            lr.SetPosition(0, pos.Item1);
            lr.SetPosition(1, pos.Item2);
        }
    }
}
