using System;
using System.Collections;
using System.Collections.Generic;
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
    private Side _mSide = Side.Middle;
    private Rigidbody _mRigidBody;

    private Dictionary<Side, Vector3> _positions;
    private void Awake()
    {
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
        if (Input.GetKeyUp(KeyCode.A))
        {
            MoveLeft();
        }else if (Input.GetKeyUp(KeyCode.D))
        {
            MoveRight();
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
    
}
