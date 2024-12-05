using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_skeleton_generator : MonoBehaviour
{
    public GameObject joint;
    public GameObject initial_pos;
    public GameObject lr_prefab;
    public ConnectionMatrix connectionMatrix;
    public float scale;
    public float visibility_treshold = 0.2f;
    private List<GameObject> joints;
    private List<GameObject> lr_list;
    
    private Tuple<int,int> getSrcDstIndex(int i)
    {
        var src_index = connectionMatrix.matrix[i].src;
        var dst_index = connectionMatrix.matrix[i].dst;
        return new Tuple<int, int>(src_index, dst_index);
    }
    private Tuple<Vector3,Vector3> getSrcDstPos(int i)
    {
        var indeces = getSrcDstIndex(i);
        var src_pos = joints[indeces.Item1].transform.position;
        var dst_pos = joints[indeces.Item2].transform.position;
        return new Tuple<Vector3, Vector3> (src_pos, dst_pos);
    }
    private void Start()
    {
        GameManager.update_data_event += update_data;
        joints = new List<GameObject>();
        lr_list = new List<GameObject>();
        for (int i=0; i<33; i++)
        {
            var obj = Instantiate(joint, initial_pos.transform.position,Quaternion.identity,initial_pos.transform);
            joints.Add(obj);
        }
        for (int i=0; i< connectionMatrix.matrix.Count; i++) 
        {
            var indeces = getSrcDstIndex(i);
            var pos = getSrcDstPos(i);
            var lr_obj = Instantiate(lr_prefab, pos.Item1, Quaternion.identity, joints[indeces.Item1].transform);
            lr_list.Add(lr_obj);
        }
    }
    public void update_data(PointDataList pdl) 
    {
        for(int i=0; i<pdl.points.Count;i++)
        {

            if (pdl.points[i].visibility < visibility_treshold)
            {
                joints[i].SetActive(false);
                continue;
            }else if (pdl.points[i].visibility > visibility_treshold && !joints[i].activeSelf) 
            {
                joints[i].SetActive(true);
            }
            var point = new Vector3(pdl.points[i].x * scale,
                                    -pdl.points[i].y * scale, 
                                    pdl.points[i].z * scale
                                    );
            joints[i].transform.position = point;
        }
        for (int i = 0; i < connectionMatrix.matrix.Count; i++)
        {
            var lr = lr_list[i].GetComponent<LineRenderer>();
            var pos = getSrcDstPos(i);
            lr.SetPosition(0, pos.Item1);
            lr.SetPosition(1, pos.Item2);
        }
    }
}
