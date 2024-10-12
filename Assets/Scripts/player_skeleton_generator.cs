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
    private Tuple<Vector3,Vector3> getSrcDstPos(int i)
    {
        var src_index = connectionMatrix.matrix[i].src;
        var dst_index = connectionMatrix.matrix[i].dst;
        var src_pos = joints[src_index].transform.position;
        var dst_pos = joints[dst_index].transform.position;
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
            var src_index = connectionMatrix.matrix[i].src;
            var pos = getSrcDstPos(i);
            var lr_obj = Instantiate(lr_prefab, pos.Item1, Quaternion.identity, joints[src_index].transform);
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
