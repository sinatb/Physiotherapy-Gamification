using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_skeleton_generator : MonoBehaviour
{
    public GameObject joint;
    public GameObject initial_pos;
    private List<GameObject> joints;

    private void Start()
    {
        GameManager.update_data_event += update_data;
        joints = new List<GameObject>();
        for (int i=0; i<32; i++)
        {
            var obj = Instantiate(joint, initial_pos.transform.position,Quaternion.identity,initial_pos.transform);
            joints.Add(obj);
        }
    }
    public void update_data(PointDataList pdl) 
    {
        for(int i=0; i<32;i++)
        {
            var point = new Vector3(pdl.points[i].x,
                                    pdl.points[i].y, 
                                    pdl.points[i].z
                                    );
            joints[i].transform.position = point;    
        }
    }
}
