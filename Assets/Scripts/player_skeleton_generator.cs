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
    private List<GameObject> joints;
    private List<GameObject> lr_list;
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
            var dst_index = connectionMatrix.matrix[i].dst;
            var src_pos = joints[src_index].transform.position;
            var dst_pos = joints[dst_index].transform.position;

            var lr_obj = Instantiate(lr_prefab, src_pos, Quaternion.identity, joints[src_index].transform);
            lr_list.Add(lr_obj);
        }
    }
    public void update_data(PointDataList pdl) 
    {
        for(int i=0; i<pdl.points.Count;i++)
        {
            var point = new Vector3(pdl.points[i].x * scale,
                                    -pdl.points[i].y * scale, 
                                    pdl.points[i].z * scale
                                    );
            joints[i].transform.position = point;    
        }
        for (int i = 0; i < connectionMatrix.matrix.Count; i++)
        {
            var lr = lr_list[i].GetComponent<LineRenderer>();
            var src_index = connectionMatrix.matrix[i].src;
            var dst_index = connectionMatrix.matrix[i].dst;
            var src_pos = joints[src_index].transform.position;
            var dst_pos = joints[dst_index].transform.position;

            lr.SetPosition(0, src_pos);
            lr.SetPosition(1, dst_pos);
        }
    }
}
