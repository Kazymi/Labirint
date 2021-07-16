using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Joystick _playerJoystick;
    [SerializeField] private bool mobile;

    private void Awake()
    {
        ServiceLocator.Subscribe<InputHandler>(this);
    }

    public Vector2 MoveDirection()
    {
        if (mobile)
        {
            return -_playerJoystick.Direction;
        }
        else
        {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}
