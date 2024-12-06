using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    private Rigidbody _mRigidbody;
    private float _speed;    
    private void Start()
    {
        _mRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _mRigidbody.MovePosition(transform.position +
                                 Vector3.back * (_speed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Despawner"))
        {
            gameObject.SetActive(false);
        }
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
