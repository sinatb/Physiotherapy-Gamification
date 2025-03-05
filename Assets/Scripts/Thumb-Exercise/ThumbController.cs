using System.Collections.Generic;
using UnityEngine;

namespace Thumb_Exercise
{
    enum Position
    {
        NoPos,
        Pos0,
        Pos1,
        Pos2,
        Pos3,
    }
    public class ThumbController : MonoBehaviour
    {
        public List<GameObject> inputPoints;
        private AudioSource     _audioSource;
        private Position        _currentPosition;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            GameManager.UpdateDataEvent += UpdateData;
        }

        private void OnTriggerEnter(Collider other)
        {
            var t = other.GetComponent<Tile>(); 
            if (t?.isPlaying == false)
            {
                if (!t.isInvalidated)
                {
                    _audioSource.PlayOneShot(_audioSource.clip);
                    StartCoroutine(t.ChangeColor(Color.red, 1.5f));
                }   
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

        private void UpdateData(PointDataList pdl)
        {
            if ((pdl.points[4].Vect - pdl.points[8].Vect).magnitude < 0.04f)
            {
                _currentPosition = Position.Pos0;
            }else if ((pdl.points[4].Vect - pdl.points[12].Vect).magnitude < 0.04f)
            {
                _currentPosition = Position.Pos1;
            }else if ((pdl.points[4].Vect - pdl.points[16].Vect).magnitude < 0.04f)
            {
                _currentPosition = Position.Pos2;
            }else if ((pdl.points[4].Vect - pdl.points[20].Vect).magnitude < 0.04f)
            {
                _currentPosition = Position.Pos3;
            }
            else
            {
                _currentPosition = Position.NoPos;
            }
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                _currentPosition = Position.Pos0;
            }else if (Input.GetKey(KeyCode.W))
            {
                _currentPosition = Position.Pos1;
            }else if (Input.GetKey(KeyCode.E))
            {
                _currentPosition = Position.Pos2;
            }else if (Input.GetKey(KeyCode.R))
            {
                _currentPosition = Position.Pos3;
            }
            ActivatePoint((int)_currentPosition-1);
        }
    }
}