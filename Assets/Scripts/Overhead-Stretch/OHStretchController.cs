using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OHStretchController : MonoBehaviour
{
    [SerializeField] private GameObject rHand;
    [SerializeField] private GameObject lHand;


    private void Awake()
    {
        GameManager.UpdateDataEvent += UpdateData;
    }


    private void UpdateData(PointDataList pdl)
    {
        var rightHand = VectorUtil.MeanVector(new List<Vector3>()
        {
            pdl.points[15].Vect,
            pdl.points[17].Vect,
            pdl.points[19].Vect,
        });
        
        var leftHand = VectorUtil.MeanVector(new List<Vector3>()
        {
            pdl.points[16].Vect,
            pdl.points[18].Vect,
            pdl.points[20].Vect,
        });
        rHand.transform.position = rightHand;
        lHand.transform.position = leftHand;
    }
}
