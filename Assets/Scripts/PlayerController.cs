using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 4f;
    
    private Rigidbody2D _playerRb;
    private Vector2 _inputDirection;
    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            _inputDirection = data.direction;
        }
        else
        {
            _inputDirection = Vector2.zero;
        }

        _playerRb.velocity = _inputDirection.normalized * speed;
    }
}
