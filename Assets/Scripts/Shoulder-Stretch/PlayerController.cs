using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum Side
    {
        Left,
        Middle,
        Right
    }

    [SerializeField] private float moveOffset;
    [SerializeField] private float speed;
    [SerializeField] private float changeTimer;
    private float _timer;
    private Side _mSide = Side.Middle;
    private Rigidbody _mRigidBody;
    private char _nextMove = 'm';
    private Dictionary<Side, Vector3> _positions;
    private void Awake()
    {
        GameManager.UpdateDataEvent += UpdateData;
        _mRigidBody = GetComponent<Rigidbody>();
        _positions = new Dictionary<Side, Vector3>()
        {
            { Side.Left , new Vector3(-moveOffset, 0.5f, 0)},
            { Side.Middle, new Vector3(0,0.5f,0)},
            { Side.Right, new Vector3(moveOffset, 0.5f, 0)}
        };
    }
    
    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _positions[_mSide]) == 0.0f)
            return;
        if (Vector3.Distance(transform.position, _positions[_mSide]) > 0.1f)
            _mRigidBody.MovePosition(transform.position -
                                     (transform.position - _positions[_mSide]).normalized *
                                     (speed * Time.fixedDeltaTime));
        else
            _mRigidBody.MovePosition(_positions[_mSide]);
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.A) || _nextMove == 'l')
        {
            MoveLeft();
            _nextMove = 'm';
        }else if (Input.GetKeyUp(KeyCode.D) || _nextMove == 'r')
        {
            MoveRight();
            _nextMove = 'm';
        }
    }

    private void MoveRight()
    {
        if (_mSide == Side.Right)
            return;
        _mSide ++;
    }

    private void MoveLeft()
    {
        if (_mSide == Side.Left)
            return;
        _mSide --;
    }

    private void UpdateData(PointDataList pdl)
    {
        if (_timer < changeTimer)
            return;
        _timer = 0.0f;
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
        
        if (leftHand.x > pdl.points[11].x)
            _nextMove = 'l';
        else if (rightHand.x < pdl.points[12].x)
            _nextMove = 'r';
    }
}
