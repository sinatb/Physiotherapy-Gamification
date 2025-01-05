
using System.Collections.Generic;
using Util;
using UnityEngine;

public class OHStretchController : MonoBehaviour
{
    [SerializeField] private float spanMultiplier;
    [SerializeField] private float heightMultiplier;
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
        
        rightHand.y = -rightHand.y;
        leftHand.y = -leftHand.y;

        rightHand.x *= spanMultiplier;
        leftHand.x *= spanMultiplier;
        
        rightHand.y *= heightMultiplier;
        leftHand.y *= heightMultiplier;
        
        rHand.transform.position = rightHand;
        lHand.transform.position = leftHand;
    }
}
