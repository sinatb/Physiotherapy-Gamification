using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    [SerializeField] private float mSpeed;
    private Rigidbody _mRigidbody;
    
    private void Start()
    {
        _mRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _mRigidbody.MovePosition(transform.position +
                                 Vector3.back * (mSpeed * Time.fixedDeltaTime));
    }
}
