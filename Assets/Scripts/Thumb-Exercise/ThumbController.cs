using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thumb_Exercise
{
    public class ThumbController : MonoBehaviour
    {
        public List<GameObject> inputPoints;

        private void OnTriggerEnter(Collider other)
        {
            var t = other.GetComponent<Tile>(); 
            if (t?.isPlaying == false)
            {
                if (!t.isInvalidated)
                    StartCoroutine(t.ChangeColor(Color.red, 1.5f));
                t.isInvalidated = true;
            }
        }

        private void ActivatePoint(int index)
        {
            for (var i = 0; i < inputPoints.Count; i++)
            {
                 inputPoints[i].SetActive(i == index && index != -1);
            }
        }
        void Update()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                ActivatePoint(0);
            }else if (Input.GetKey(KeyCode.W))
            {
                ActivatePoint(1);
            }else if (Input.GetKey(KeyCode.E))
            {
                ActivatePoint(2);
            }else if (Input.GetKey(KeyCode.R))
            {
                ActivatePoint(3);
            }else
            {
                ActivatePoint(-1);
            }
        }
    }
}